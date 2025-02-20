Public Class frmPreferences
    Private Sub frmPreferences_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPreferences()
    End Sub

    Private Sub butOk_Click(sender As Object, e As EventArgs) Handles butOk.Click
        SavePreferences()
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub LoadPreferences()
        cmbDisplayUnits.SelectedIndex = SettingsContainer.Instance.DisplayUnits
    End Sub

    Private Sub SavePreferences()
        SettingsContainer.Instance.DisplayUnits = cmbDisplayUnits.SelectedIndex
        SettingsContainer.Instance.Save()
    End Sub
End Class