Public Class frmChangeUnits
    Public Property Geometry As Geometry

    Private Sub frmChangeUnits_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtCurrentUnits.Text = Geometry.Units.ToString()
        cmbNewUnits.SelectedIndex = Geometry.Units
    End Sub

    Private Sub butOk_Click(sender As Object, e As EventArgs) Handles butOk.Click
        Geometry.Units = cmbNewUnits.SelectedIndex
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class