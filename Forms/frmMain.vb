Imports System.IO

Public Class frmMain
    Private _geometry As Geometry
    Private _selectedObject As GeometryTriangleGroup
    Private _fileName As String

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mdFront.ViewQuaternion = Quaternion.Identity()
        mdRight.ViewQuaternion = New Quaternion(New Vector3(0, 1, 0), -Math.PI / 2)
        mdBottom.ViewQuaternion = New Quaternion(New Vector3(1, 0, 0), Math.PI / 2)
        mdIso.ViewQuaternion = Quaternion.Identity()
    End Sub

    Private Sub mnuFileOpen_Click(sender As Object, e As EventArgs) Handles mnuFileOpen.Click
        Using ofdOpen As New OpenFileDialog
            ofdOpen.Title = "Open Model File"
            ofdOpen.Filter = "Wavefront OBJ Files|*.obj|All Files (*.*)|*.*"
            ofdOpen.InitialDirectory = SettingsContainer.Instance.ImportLastOpenDir
            If ofdOpen.ShowDialog(Me) = DialogResult.OK Then
                OpenModelFile(ofdOpen.FileName)
                SettingsContainer.Instance.ImportLastOpenDir = Path.GetDirectoryName(ofdOpen.FileName)
                SettingsContainer.Instance.Save()
            End If
        End Using
    End Sub


    Private Sub mnuFileRefreshModel_Click(sender As Object, e As EventArgs) Handles mnuFileRefreshModel.Click
        If _geometry IsNot Nothing AndAlso File.Exists(_fileName) Then
            RefreshModelFile()
        End If
    End Sub


    Private Sub mnuFileExportToSvg_Click(sender As Object, e As EventArgs) Handles mnuFileExportToSvg.Click
        Dim savePath As String
        Dim fileNameBase As String

        If _selectedObject IsNot Nothing Then
            tsslStatus.Text = "Slicing..."

            Using slicer As New frmSlicer
                slicer.TriangleGroup = _selectedObject
                slicer.Thickness = ConvertUnit(nudThickness.Value, Unit.Millimeter, _geometry.Units)

                If slicer.ShowDialog(Me) = DialogResult.OK Then
                    ' Slicing complete.

                    If SettingsContainer.Instance.ExportSvgUseDefaults Then
                        ' No export dialog.

                        Using saveDialog As New SaveFileDialog
                            saveDialog.Title = "Select the folder to save SVG files"
                            saveDialog.Filter = "SVG files|*.svg|All Files (*.*)|*.*"
                            saveDialog.DefaultExt = ".svg"
                            saveDialog.InitialDirectory = SettingsContainer.Instance.ExportSvgLastExportDir
                            If saveDialog.ShowDialog() = DialogResult.OK Then
                                savePath = Path.GetDirectoryName(saveDialog.FileName)
                                fileNameBase = Path.GetFileNameWithoutExtension(saveDialog.FileName)

                                ExportSlicesToSvg(slicer.Result, savePath, fileNameBase, _geometry.Units,
                                                  SettingsContainer.Instance.ExportSvgIncludeTop,
                                                  SettingsContainer.Instance.ExportSvgIncludeFill,
                                                  SettingsContainer.Instance.ExportSvgIncludeBottom,
                                                  SettingsContainer.Instance.ExportSvgColorTop,
                                                  SettingsContainer.Instance.ExportSvgColorFill,
                                                  SettingsContainer.Instance.ExportSvgColorBottom)

                                SettingsContainer.Instance.ExportSvgLastExportDir = savePath
                                SettingsContainer.Instance.Save()

                                If MessageBox.Show(Me, "Export complete. Open output folder?", "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                                    Process.Start(New ProcessStartInfo() With {.FileName = savePath, .UseShellExecute = True, .Verb = "open"})
                                End If
                            End If
                        End Using
                    Else
                        ' Use export dialog.
                        Using export As New frmExportSvg
                            export.Layers = slicer.Result
                            export.Units = _geometry.Units
                            export.ShowDialog(Me)
                        End Using
                    End If
                End If
            End Using
        End If
    End Sub

    Private Sub mnuFilePrintPreview_Click(sender As Object, e As EventArgs) Handles mnuFilePrintPreview.Click
        If _selectedObject IsNot Nothing Then
            tsslStatus.Text = "Slicing..."

            Using slicer As New frmSlicer
                slicer.TriangleGroup = _selectedObject
                slicer.Thickness = ConvertUnit(nudThickness.Value, Unit.Millimeter, _geometry.Units)

                If slicer.ShowDialog(Me) = DialogResult.OK Then
                    PrintSlices(slicer.Result)
                End If
            End Using
        End If
    End Sub

    Private Sub mnuFileExit_Click(sender As Object, e As EventArgs) Handles mnuFileExit.Click
        Close()
    End Sub

    Private Sub mnuToolsPreferences_Click(sender As Object, e As EventArgs) Handles mnuToolsPreferences.Click
        Using prefs As New frmPreferences
            If prefs.ShowDialog(Me) = DialogResult.OK Then
                If _geometry IsNot Nothing Then
                    LoadModelStats()
                    SetSelectedObject(lbObjects.SelectedItem)
                End If
            End If
        End Using
    End Sub

    Private Sub mnuToolsChangeUnits_Click(sender As Object, e As EventArgs) Handles mnuToolsChangeUnits.Click
        If _geometry IsNot Nothing Then
            Using changeUnits = New frmChangeUnits
                changeUnits.Geometry = _geometry
                If changeUnits.ShowDialog(Me) = DialogResult.OK Then
                    LoadModelStats()
                    SetSelectedObject(lbObjects.SelectedItem)
                    UpdateTrackBar()
                End If
            End Using
        End If
    End Sub

    Private Sub mnuToolsChangeUpAxis_Click(sender As Object, e As EventArgs) Handles mnuToolsChangeUpAxis.Click
        If _geometry IsNot Nothing Then
            Using changeUpAxis = New frmChangeUpAxis
                changeUpAxis.Geometry = _geometry
                If changeUpAxis.ShowDialog(Me) = DialogResult.OK Then
                    LoadModelStats()
                    SetSelectedObject(lbObjects.SelectedItem)
                    UpdateTrackBar()
                End If
            End Using
        End If
    End Sub

    Private Sub lbObjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbObjects.SelectedIndexChanged
        Call SetSelectedObject(lbObjects.SelectedItem)
    End Sub

    Private Sub nudThickness_ValueChanged(sender As Object, e As EventArgs) Handles nudThickness.ValueChanged
        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub tbSlice_Scroll(sender As Object, e As EventArgs) Handles tbSlice.Scroll
        Call UpdateDisplay()
    End Sub

    Private Sub PrintSlices(layers As List(Of Layer))
        Using preview As New PrintPreviewDialog
            Using llpdPrint As New LayerListPrintDocument(layers, _geometry.Units)
                preview.Document = llpdPrint

                preview.ShowDialog(Me)
            End Using
        End Using
    End Sub

    Private Sub LoadModelStats()
        Dim units As Unit
        Dim totalHeight As Single
        Dim totalWidth As Single
        Dim totalDepth As Single
        Dim totalArea As Single

        units = GetUnitFromDisplayUnit(SettingsContainer.Instance.DisplayUnits)

        totalHeight = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Height).Sum()
        totalWidth = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Width).Sum()
        totalDepth = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Depth).Sum()
        totalArea = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select
                                                                                   ConvertUnit(gtgPart.Bounds.Height, _geometry.Units, units) *
                                                                                   ConvertUnit(gtgPart.Bounds.Width, _geometry.Units, units) *
                                                                                   ConvertUnit(gtgPart.Bounds.Depth, _geometry.Units, units)).Sum()


        lblTotalHeight.Text = FormatUnit(totalHeight, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
        lblTotalWidth.Text = FormatUnit(totalWidth, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
        lblTotalDepth.Text = FormatUnit(totalDepth, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
        lblTotalVolume.Text = FormatUnit(totalArea, SettingsContainer.Instance.DisplayUnits) & "³"
    End Sub

    Private Sub OpenModelFile(fileName As String)
        If SettingsContainer.Instance.ImportUseDefaults Then
            _geometry = Geometry.LoadWavefrontObj(fileName, SettingsContainer.Instance.ImportDefaultUnits, SettingsContainer.Instance.ImportDefaultUpAxis)
        Else
            Using importDialog As New frmImport
                importDialog.FileName = fileName
                If importDialog.ShowDialog(Me) <> DialogResult.OK Then
                    Return
                End If

                _geometry = importDialog.Result
            End Using
        End If

        LoadModelStats()

        _fileName = fileName

        mnuFileRefreshModel.Enabled = True
        mnuFileExport.Enabled = True
        mnuFilePrintPreview.Enabled = True
        mnuToolsChangeUnits.Enabled = True
        mnuToolsChangeUpAxis.Enabled = True

        Text = $"Slicer - {Path.GetFileName(_fileName)}"

        lbObjects.DataSource = _geometry.Groups
    End Sub

    Private Sub RefreshModelFile()
        _geometry = Geometry.LoadWavefrontObj(_fileName, _geometry.Units, _geometry.UpAxis)

        LoadModelStats()

        lbObjects.DataSource = _geometry.Groups
    End Sub

    Private Sub SetSelectedObject(gtgObject As GeometryTriangleGroup)
        Dim units As Unit
        Dim modelMatrix As Matrix

        _selectedObject = gtgObject

        If _selectedObject IsNot Nothing Then
            modelMatrix = Matrix.FromBoundingBox(_selectedObject.Bounds)

            mdFront.ModelMatrix = modelMatrix
            mdRight.ModelMatrix = modelMatrix
            mdBottom.ModelMatrix = modelMatrix
            mdIso.ModelMatrix = modelMatrix * Matrix.Scale(1 / 1.212)

            units = GetUnitFromDisplayUnit(SettingsContainer.Instance.DisplayUnits)

            lblHeight.Text = FormatUnit(_selectedObject.Bounds.Height, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
            lblWidth.Text = FormatUnit(_selectedObject.Bounds.Width, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
            lblDepth.Text = FormatUnit(_selectedObject.Bounds.Depth, _geometry.Units, SettingsContainer.Instance.DisplayUnits)
            lblVolume.Text = FormatUnit(ConvertUnit(_selectedObject.Bounds.Height, _geometry.Units, units) *
                ConvertUnit(_selectedObject.Bounds.Width, _geometry.Units, units) *
                ConvertUnit(_selectedObject.Bounds.Depth, _geometry.Units, units), SettingsContainer.Instance.DisplayUnits) & "³"
        Else
            lblHeight.Text = "-"
            lblWidth.Text = "-"
            lblDepth.Text = "-"
            lblVolume.Text = "-"
        End If

        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub UpdateTrackBar()
        If _selectedObject IsNot Nothing Then
            Dim modelHeight As Single = ConvertUnit(_selectedObject.Bounds.Height, _geometry.Units, Unit.Millimeter)
            Dim numSlices As Integer = Math.Ceiling(modelHeight / nudThickness.Value)
            tbSlice.Maximum = numSlices
            lblSlices.Text = numSlices
        Else
            tbSlice.Maximum = 0
            lblSlices.Text = String.Empty
        End If
    End Sub

    Private Sub UpdateDisplay()
        Dim thickness As Single
        Dim sliceStart As Vector3
        Dim sliceEnd As Vector3
        Dim sliceDir As Vector3
        Dim slice As (Above As GeometryTriangleGroup, Between As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim mesher As MesherXYZ
        Dim geomFull As GeometryTriangleGroup
        Dim geomAbove As GeometryTriangleGroup
        Dim geomBetween As GeometryTriangleGroup
        Dim geomBelow As GeometryTriangleGroup
        Dim sliceLines As GeometryLineGroup
        Dim endLines As GeometryLineGroup
        Dim startLines As GeometryLineGroup
        Dim openingLines As GeometryLineGroup
        Dim silhouette As GeometryLineGroup

        If _selectedObject IsNot Nothing Then
            thickness = ConvertUnit(nudThickness.Value, Unit.Millimeter, _geometry.Units)

            sliceStart = New Vector3(0, _selectedObject.Bounds.Minimum.Y + thickness * (tbSlice.Value + 1), 0)
            sliceEnd = New Vector3(0, _selectedObject.Bounds.Minimum.Y + thickness * tbSlice.Value, 0)
            sliceDir = New Vector3(0, -1, 0)

            slice = Slicer.ExtractBetweenPlanes(_selectedObject, sliceStart, sliceEnd, sliceDir)
            mesher = New MesherXYZ(_selectedObject)

            geomFull = _selectedObject.CloneWithColor(Color.LightGray)
            geomAbove = slice.Above.CloneWithColor(Color.LightGray)
            geomBetween = slice.Between.CloneWithColor(Color.DarkGray)
            geomBelow = slice.Below.CloneWithColor(Color.LightGray)
            sliceLines = slice.Between.ToLineGroup(Color.Black)
            endLines = Slicer.OutlineModelPlane(slice.Between, sliceEnd, sliceDir, Color.Blue)
            startLines = Slicer.OutlineModelPlane(slice.Between, sliceStart, sliceDir, Color.Red)
            openingLines = mesher.CreateDisconnectedEdgesLineGroup(Color.Orange)
            silhouette = mesher.CreateSilhouette(sliceDir, Color.Green)

            mdFront.SetDrawData(geomAbove, geomBetween, geomBelow, endLines, startLines, openingLines)
            mdRight.SetDrawData(geomAbove, geomBetween, geomBelow, endLines, startLines, openingLines)
            mdBottom.SetDrawData(geomAbove, geomBetween, geomBelow, endLines, startLines, openingLines, silhouette)
            mdIso.SetDrawData(geomAbove, geomBetween, geomBelow, endLines, startLines, openingLines, silhouette)
        Else
            mdFront.SetDrawData()
            mdRight.SetDrawData()
            mdBottom.SetDrawData()
            mdIso.SetDrawData()
        End If
    End Sub

    Private Class SliceArgs
        Public Section As GeometryTriangleGroup
        Public SliceSize As Single
        Public TopColor As Color
        Public BottomColor As Color
        Public ContentColor As Color
        Public Print As Boolean

        Public Sub New(sectionValue As GeometryTriangleGroup, sliceSizeValue As Single, topColorValue As Color, bottomColorValue As Color, contentColorValue As Color, printValue As Boolean)
            Section = sectionValue
            SliceSize = sliceSizeValue
            TopColor = topColorValue
            BottomColor = bottomColorValue
            ContentColor = contentColorValue
            Print = printValue
        End Sub
    End Class

    Private Class SliceProgress
        Public SliceIndex As Integer
        Public NumSlices As Integer

        Public Sub New(sliceIndexValue As Integer, numSlicesValue As Integer)
            SliceIndex = sliceIndexValue
            NumSlices = numSlicesValue
        End Sub
    End Class
End Class
