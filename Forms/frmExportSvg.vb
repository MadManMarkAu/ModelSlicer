Imports System.IO

Public Class frmExportSvg
    Public Property Layers As List(Of Layer)
    Public Property Units As Unit

    Private Sub frmExportSvg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Private Sub butExport_Click(sender As Object, e As EventArgs) Handles butExport.Click
        Dim savePath As String
        Dim fileNameBase As String

        Using saveDialog As New SaveFileDialog
            saveDialog.Title = "Select the folder to save SVG files"
            saveDialog.Filter = "SVG files|*.svg|All Files (*.*)|*.*"
            saveDialog.DefaultExt = ".svg"
            saveDialog.InitialDirectory = SettingsContainer.Instance.ExportSvgLastExportDir
            If saveDialog.ShowDialog() = DialogResult.OK Then
                savePath = Path.GetDirectoryName(saveDialog.FileName)
                fileNameBase = Path.GetFileNameWithoutExtension(saveDialog.FileName)

                ExportSlicesToSvg(Layers, savePath, fileNameBase, Units, chkExportTop.Checked, chkExportFill.Checked, chkExportBottom.Checked, cpColorTop.Value, cpColorFill.Value, cpColorBottom.Value)

                SaveSettings(savePath)

                If MessageBox.Show(Me, "Export complete. Open output folder?", "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                    Process.Start(New ProcessStartInfo() With {.FileName = savePath, .UseShellExecute = True, .Verb = "open"})
                End If

                DialogResult = DialogResult.OK
                Close()
            End If
        End Using
    End Sub

    Private Sub LoadSettings()
        chkExportTop.Checked = SettingsContainer.Instance.ExportSvgIncludeTop
        chkExportFill.Checked = SettingsContainer.Instance.ExportSvgIncludeFill
        chkExportBottom.Checked = SettingsContainer.Instance.ExportSvgIncludeBottom
        cpColorTop.Value = SettingsContainer.Instance.ExportSvgColorTop
        cpColorFill.Value = SettingsContainer.Instance.ExportSvgColorFill
        cpColorBottom.Value = SettingsContainer.Instance.ExportSvgColorBottom
    End Sub

    Private Sub SaveSettings(savePath As String)
        SettingsContainer.Instance.ExportSvgIncludeTop = chkExportTop.Checked
        SettingsContainer.Instance.ExportSvgIncludeFill = chkExportFill.Checked
        SettingsContainer.Instance.ExportSvgIncludeBottom = chkExportBottom.Checked
        SettingsContainer.Instance.ExportSvgColorTop = cpColorTop.Value
        SettingsContainer.Instance.ExportSvgColorFill = cpColorFill.Value
        SettingsContainer.Instance.ExportSvgColorBottom = cpColorBottom.Value
        SettingsContainer.Instance.ExportSvgUseDefaults = chkDoNotAsk.Checked
        SettingsContainer.Instance.ExportSvgLastExportDir = savePath
        SettingsContainer.Instance.Save()
    End Sub
End Class