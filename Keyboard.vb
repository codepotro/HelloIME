''   Copyright© Codepotro.

''   This file Is part Of HelloIME.

''   HelloIME Is free software: you can redistribute it And/Or modify
''   it under the terms Of the GNU General Public License As published by
''   the Free Software Foundation, either version 3 Of the License, Or
''   (at your option) any later version.

''   HelloIME Is distributed In the hope that it will be useful,
''   but WITHOUT ANY WARRANTY; without even the implied warranty of
''   MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE. See the
''   GNU General Public License For more details.

''   You should have received a copy Of the GNU General Public License
''   along with HelloIME.  If Not, see < https: //www.gnu.org/licenses/>.


Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading

Module Keyboard
    Public Declare Function UnhookWindowsHookEx Lib "user32" _
      (ByVal hHook As Integer) As Integer

    Public Declare Function SetWindowsHookEx Lib "user32" _
      Alias "SetWindowsHookExA" (ByVal idHook As Integer,
      ByVal lpfn As KeyboardHookDelegate, ByVal hmod As Integer,
      ByVal dwThreadId As Integer) As Integer

    Private Declare Function GetAsyncKeyState Lib "user32" _
      (ByVal vKey As Integer) As Integer

    Private Declare Function CallNextHookEx Lib "user32" _
      (ByVal hHook As Integer,
      ByVal nCode As Integer,
      ByVal wParam As Integer,
      ByVal lParam As KBDLLHOOKSTRUCT) As Integer

    Public Structure KBDLLHOOKSTRUCT
        Public vkCode As Integer
        Public scanCode As Integer
        Public flags As Integer
        Public time As Integer
        Public dwExtraInfo As Integer
    End Structure

    ' Low-Level Keyboard Constants
    Private Const HC_ACTION As Integer = 0
    Private Const LLKHF_EXTENDED As Integer = &H1
    Private Const LLKHF_INJECTED As Integer = &H10
    Private Const LLKHF_ALTDOWN As Integer = &H20
    Private Const LLKHF_UP As Integer = &H80

    Private Const WM_KEYDOWN As Integer = 256

    ' Virtual Keys
    Public Const VK_TAB = &H9
    Public Const VK_CONTROL = &H11
    Public Const VK_ESCAPE = &H1B
    Public Const VK_DELETE = &H2E

    Private Const WH_KEYBOARD_LL As Integer = 13&
    Public KeyboardHandle As Integer






    Public Function IsHooked(
      ByRef Hookstruct As KBDLLHOOKSTRUCT) As Boolean

        Debug.WriteLine("Hookstruct.vkCode: " & Hookstruct.vkCode)




        If (Hookstruct.vkCode = 65) Then
            SendKey("আ")
            Return True
        End If

        ''Here 65 is the code for Character "A"
        ''SendKey("") sends the string if the key("A") is pressed
        ''"Return True" blocks input "A"



        Return False
    End Function

    Private Sub HookedState(ByVal Text As String)
        Debug.WriteLine(Text)
    End Sub

    Public Function KeyboardCallback(ByVal Code As Integer,
      ByVal wParam As Integer,
      ByRef lParam As KBDLLHOOKSTRUCT) As Integer

        If Code = HC_ACTION And wParam = WM_KEYDOWN Then
            Debug.WriteLine("Calling IsHooked")

            If (IsHooked(lParam)) Then
                Return 1
            End If

        End If

        Return CallNextHookEx(KeyboardHandle,
          Code, wParam, lParam)

    End Function


    Public Delegate Function KeyboardHookDelegate(
      ByVal Code As Integer,
      ByVal wParam As Integer, ByRef lParam As KBDLLHOOKSTRUCT) _
                   As Integer

    <MarshalAs(UnmanagedType.FunctionPtr)>
    Private callback As KeyboardHookDelegate

    Public Sub HookKeyboard()
        callback = New KeyboardHookDelegate(AddressOf KeyboardCallback)

        KeyboardHandle = SetWindowsHookEx(
          WH_KEYBOARD_LL, callback,
          Marshal.GetHINSTANCE(
          [Assembly].GetExecutingAssembly.GetModules()(0)).ToInt32, 0)

        Call CheckHooked()
    End Sub

    Public Sub CheckHooked()
        If (Hooked()) Then
            Debug.WriteLine("Keyboard successfully hooked")
        Else
            Debug.WriteLine("Keyboard hooking failed: " & Err.LastDllError)
        End If
    End Sub

    Private Function Hooked()
        Hooked = KeyboardHandle <> 0
    End Function

    Public Sub UnhookKeyboard()
        If (Hooked()) Then
            Call UnhookWindowsHookEx(KeyboardHandle)
        End If
    End Sub



    Private Sub DoKeyBoard(ByVal flags As NativeMethods.KEYEVENTF, ByVal key As Keys)
        Dim input As New NativeMethods.INPUT
        Dim ki As New NativeMethods.KEYBDINPUT
        input.dwType = NativeMethods.InputType.Keyboard
        input.ki = ki
        input.ki.wVk = 0
        input.ki.wScan = Convert.ToInt16(key)
        input.ki.time = 0
        input.ki.dwFlags = flags
        input.ki.dwExtraInfo = IntPtr.Zero
        Dim cbSize As Integer = Marshal.SizeOf(GetType(NativeMethods.INPUT))
        Dim result As Integer = NativeMethods.SendInput(1, input, cbSize)
        If result = 0 Then Debug.WriteLine(Marshal.GetLastWin32Error)
    End Sub

    Private Sub SendKey(ByVal Text As String)
        Dim length As Integer = Text.Length
        For i As Integer = 1 To length Step 1
            SendKeyByte(CShort(Strings.AscW(Strings.Mid(Text, i, 1))))
        Next

    End Sub

    Private Sub SendKeyByte(ByVal KeyCode As Short)

        DoKeyBoard(NativeMethods.KEYEVENTF.UNICODE, KeyCode)
        DoKeyBoard(NativeMethods.KEYEVENTF.KEYUP, KeyCode)

    End Sub


End Module