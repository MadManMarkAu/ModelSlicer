Public Class frmChangeUpAxis
    Public Property Geometry As Geometry

    Private Sub frmChangeUpAxis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtCurrentUpAxis.Text = Geometry.UpAxis.ToString()
        cmbNewUpAxis.SelectedIndex = Geometry.UpAxis
    End Sub

    Private Sub butOk_Click(sender As Object, e As EventArgs) Handles butOk.Click
        Geometry.ChangeUpAxis(cmbNewUpAxis.SelectedIndex)
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class