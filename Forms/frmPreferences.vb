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
        chkImportUseDefaults.Checked = SettingsContainer.Instance.ImportUseDefaults
        cmbImportDefaultUnits.SelectedIndex = SettingsContainer.Instance.ImportDefaultUnits
        cmbImportDefaultUpAxis.SelectedIndex = SettingsContainer.Instance.ImportDefaultUpAxis
    End Sub

    Private Sub SavePreferences()
        SettingsContainer.Instance.DisplayUnits = cmbDisplayUnits.SelectedIndex
        SettingsContainer.Instance.ImportUseDefaults = chkImportUseDefaults.Checked
        SettingsContainer.Instance.ImportDefaultUnits = cmbImportDefaultUnits.SelectedIndex
        SettingsContainer.Instance.ImportDefaultUpAxis = cmbImportDefaultUpAxis.SelectedIndex
        SettingsContainer.Instance.Save()
    End Sub
End Class