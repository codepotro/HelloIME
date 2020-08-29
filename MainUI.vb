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



Public Class MainUI
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HookKeyboard()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MessageBox.Show("HelloIME is a IME model for the Developers by Codepotro. Licensed under GPL V3",
    "About HelloIME", MessageBoxButtons.OK, MessageBoxIcon.Information, 0)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs)

    End Sub

    Private Sub MainUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
