Imports System.IO
Imports System.Xml

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
            If ofdOpen.ShowDialog(Me) = DialogResult.OK Then
                OpenModelFile(ofdOpen.FileName)
            End If
        End Using
    End Sub

    Private Sub mnuFileExport_Click(sender As Object, e As EventArgs) Handles mnuFileExport.Click
        If _selectedObject IsNot Nothing Then
            tsslStatus.Text = "Slicing..."

            Using slicer As New frmSlicer
                slicer.TriangleGroup = _selectedObject
                slicer.Thickness = ConvertUnit(nudThickness.Value, Unit.Millimeter, _geometry.Units)

                If slicer.ShowDialog(Me) = DialogResult.OK Then
                    ExportSlices(slicer.Result)
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

    Private Sub ExportSlices(layers As List(Of Layer))
        Using saveDialog As New SaveFileDialog
            saveDialog.FileName = "Select the folder to save SVG files"
            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim selectedPath As String = Path.GetDirectoryName(saveDialog.FileName)

                For Each layer As Layer In layers
                    Dim xmlDoc As New XmlDocument()
                    Dim svgRoot As XmlElement = xmlDoc.CreateElement("svg", "http://www.w3.org/2000/svg")
                    xmlDoc.AppendChild(svgRoot)

                    Dim xMin As Single = layer.Bounds.Minimum.X
                    Dim zMin As Single = layer.Bounds.Minimum.Z

                    Dim svgLayerTop As XmlElement = ConvertLayerToSvg(xmlDoc, layer.TopOutline, xMin, zMin, "top")
                    svgRoot.AppendChild(svgLayerTop)

                    Dim svgLayerBottom As XmlElement = ConvertLayerToSvg(xmlDoc, layer.BottomOutline, xMin, zMin, "bottom")
                    svgRoot.AppendChild(svgLayerBottom)

                    xmlDoc.Save(Path.Combine(selectedPath, "layer" & layers.IndexOf(layer) & ".svg"))
                Next
            End If
        End Using
    End Sub

    Private Function ConvertLayerToSvg(xmlDoc As XmlDocument, lines As GeometryLineGroup, xMin As Single, zMin As Single, layerName As String) As XmlElement
        Dim layer As XmlElement = xmlDoc.CreateElement("g", "http://www.w3.org/2000/svg")

        layer.RemoveAllAttributes()
        layer.SetAttribute("id", layerName)
        For Each line As GeometryLine In lines.Lines
            Dim svgLine As XmlElement = xmlDoc.CreateElement("line", "http://www.w3.org/2000/svg")
            svgLine.SetAttribute("x1", (line.V1.X - xMin).ToString())
            svgLine.SetAttribute("y1", (line.V1.Z - zMin).ToString())
            svgLine.SetAttribute("x2", (line.V2.X - xMin).ToString())
            svgLine.SetAttribute("y2", (line.V2.Z - zMin).ToString())
            svgLine.SetAttribute("stroke", line.Color.Name)
            layer.AppendChild(svgLine)
        Next
        Return layer
    End Function

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
        Using importDialog As New frmImport
            importDialog.FileName = fileName
            If importDialog.ShowDialog(Me) = DialogResult.OK Then
                _geometry = importDialog.Result

                LoadModelStats()

                _fileName = fileName

                mnuFileExport.Enabled = True
                mnuFilePrintPreview.Enabled = True
                mnuToolsChangeUnits.Enabled = True
                mnuToolsChangeUpAxis.Enabled = True

                Text = $"Slicer - {Path.GetFileName(_fileName)}"

                lbObjects.DataSource = _geometry.Groups
            End If
        End Using
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
        Dim slice As GeometryTriangleGroup
        Dim mesher As MesherXYZ
        Dim fullGeom As GeometryTriangleGroup
        Dim sliceGeom As GeometryTriangleGroup
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

            fullGeom = _selectedObject.CloneWithColor(Color.LightGray)
            sliceGeom = slice.CloneWithColor(Color.DarkGray)
            sliceLines = slice.ToLineGroup(Color.Black)
            endLines = Slicer.OutlineModelPlane(slice, sliceEnd, sliceDir, Color.Blue)
            startLines = Slicer.OutlineModelPlane(slice, sliceStart, sliceDir, Color.Red)
            openingLines = mesher.CreateDisconnectedEdgesLineGroup(Color.Orange)
            silhouette = mesher.CreateSilhouette(sliceDir, Color.Green)

            mdFront.SetDrawData(fullGeom, sliceGeom, endLines, startLines, openingLines)
            mdRight.SetDrawData(fullGeom, sliceGeom, endLines, startLines, openingLines)
            mdBottom.SetDrawData(fullGeom, sliceGeom, endLines, startLines, openingLines, silhouette)
            'mdIso.SetDrawData(gtgFullGeom, gtgSliceGeom, glgSliceLines, glgEndLines, glgStartLines, glgOpeningLines)
            mdIso.SetDrawData(fullGeom, sliceGeom, endLines, startLines, openingLines, silhouette)
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
