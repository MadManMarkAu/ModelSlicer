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
        chkExportSvgUseDefaults.Checked = SettingsContainer.Instance.ExportSvgUseDefaults
        chkExportSvgIncludeTop.Checked = SettingsContainer.Instance.ExportSvgIncludeTop
        chkExportSvgIncludeFill.Checked = SettingsContainer.Instance.ExportSvgIncludeFill
        chkExportSvgIncludeBottom.Checked = SettingsContainer.Instance.ExportSvgIncludeBottom
        cpExportSvgColorTop.Value = SettingsContainer.Instance.ExportSvgColorTop
        cpExportSvgColorFill.Value = SettingsContainer.Instance.ExportSvgColorFill
        cpExportSvgColorBottom.Value = SettingsContainer.Instance.ExportSvgColorBottom
    End Sub

    Private Sub SavePreferences()
        SettingsContainer.Instance.DisplayUnits = cmbDisplayUnits.SelectedIndex
        SettingsContainer.Instance.ImportUseDefaults = chkImportUseDefaults.Checked
        SettingsContainer.Instance.ImportDefaultUnits = cmbImportDefaultUnits.SelectedIndex
        SettingsContainer.Instance.ImportDefaultUpAxis = cmbImportDefaultUpAxis.SelectedIndex
        SettingsContainer.Instance.ExportSvgUseDefaults = chkExportSvgUseDefaults.Checked
        SettingsContainer.Instance.ExportSvgIncludeTop = chkExportSvgIncludeTop.Checked
        SettingsContainer.Instance.ExportSvgIncludeFill = chkExportSvgIncludeFill.Checked
        SettingsContainer.Instance.ExportSvgIncludeBottom = chkExportSvgIncludeBottom.Checked
        SettingsContainer.Instance.ExportSvgColorTop = cpExportSvgColorTop.Value
        SettingsContainer.Instance.ExportSvgColorFill = cpExportSvgColorFill.Value
        SettingsContainer.Instance.ExportSvgColorBottom = cpExportSvgColorBottom.Value
        SettingsContainer.Instance.Save()
    End Sub
End Class