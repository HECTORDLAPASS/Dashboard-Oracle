Public Class Form3
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Form1.Show()
        Form2.Close()
    End Sub

    Private Sub Dasboarh_Click(sender As Object, e As EventArgs) Handles Dasboarh.Click
        Form2.Show()
        Form1.Close()
    End Sub
End Class