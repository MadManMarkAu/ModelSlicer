Imports System.ComponentModel
Imports System.IO
Imports System.Xml

Public Class frmMain
    Private _geometry As Geometry
    Private _selectedObject As GeometryTriangleGroup
    Private WithEvents _slicer As New BackgroundWorker With {.WorkerReportsProgress = True}
    Private _layers As List(Of Layer)
    Private _fileName As String

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mdFront.ViewQuaternion = Quaternion.Identity()
        mdRight.ViewQuaternion = New Quaternion(New Vector3(0, 1, 0), -Math.PI / 2)
        mdBottom.ViewQuaternion = New Quaternion(New Vector3(1, 0, 0), Math.PI / 2)
        mdIso.ViewQuaternion = Quaternion.Identity() ' New Quaternion(New Vector3(0, 1, 0), Math.PI)
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

    Private Sub mnuFileReload_Click(sender As Object, e As EventArgs) Handles mnuFileReload.Click
        ReloadModelFile()
    End Sub

    Private Sub mnuFileExport_Click(sender As Object, e As EventArgs) Handles mnuFileExport.Click
        tsslStatus.Text = "Slicing..."
        mnuFilePrint.Enabled = False
        mnuFilePrintPreview.Enabled = False
        tspbProgress.Visible = True

        _slicer.RunWorkerAsync(New SliceArgs(_selectedObject, CSng(nudThickness.Value), Color.Red, Color.Blue, Color.LightGray, False))
    End Sub

    Private Sub mnuFilePrint_Click(sender As Object, e As EventArgs) Handles mnuFilePrint.Click

    End Sub

    Private Sub mnuFilePrintPreview_Click(sender As Object, e As EventArgs) Handles mnuFilePrintPreview.Click
        tsslStatus.Text = "Slicing..."
        mnuFilePrint.Enabled = False
        mnuFilePrintPreview.Enabled = False
        tspbProgress.Visible = True

        _slicer.RunWorkerAsync(New SliceArgs(_selectedObject, CSng(nudThickness.Value), Color.Red, Color.Blue, Color.LightGray, True))
    End Sub

    Private Sub mnuFileExit_Click(sender As Object, e As EventArgs) Handles mnuFileExit.Click
        Close()
    End Sub

    Private Sub lbObjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbObjects.SelectedIndexChanged
        Call SetSelectedObject(lbObjects.SelectedItem)
    End Sub

    Private Sub nudThickness_ValueChanged(sender As Object, e As EventArgs) Handles nudThickness.ValueChanged
        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub zUpRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles zUpRadioButton.CheckedChanged
        Dim upAxis As Axis = If(zUpRadioButton.Checked, Axis.Z, Axis.Y)

        If _geometry IsNot Nothing Then
            _geometry.ChangeUpAxis(upAxis)
            LoadModelStats()
            SetSelectedObject(lbObjects.SelectedItem)
        End If
    End Sub

    Private Sub unitsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles unitsComboBox.SelectedIndexChanged
        Dim units As String = unitsComboBox.SelectedItem.ToString()
        Dim newUnit As Unit = Geometry.StringToUnit(units)

        If _geometry IsNot Nothing Then
            _geometry.ChangeScale(newUnit)
            LoadModelStats()
            SetSelectedObject(lbObjects.SelectedItem)
        End If
    End Sub

    Private Sub nudHeight_ValueChanged(sender As Object, e As EventArgs)
        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub tbSlice_Scroll(sender As Object, e As EventArgs) Handles tbSlice.Scroll
        Call UpdateDisplay()
    End Sub

    Private Sub m_bwSlicer_DoWork(sender As Object, e As DoWorkEventArgs) Handles _slicer.DoWork
        Dim args As SliceArgs = DirectCast(e.Argument, SliceArgs)
        Dim output As New List(Of Layer)
        Dim yStart As Single
        Dim startPlanePoint As Vector3
        Dim endPlanePoint As Vector3
        Dim planeNormal As Vector3
        Dim sliceIndex As Integer
        Dim numSlices As Integer

        yStart = args.Section.Bounds.Minimum.Y
        planeNormal = New Vector3(0, 1, 0)

        sliceIndex = 0
        numSlices = Math.Ceiling(args.Section.Bounds.Height / args.SliceSize)

        While yStart < args.Section.Bounds.Maximum.Y
            _slicer.ReportProgress(0, New SliceProgress(sliceIndex, numSlices))

            startPlanePoint = New Vector3(0, yStart, 0)
            endPlanePoint = New Vector3(0, yStart + args.SliceSize, 0)

            output.Add(Slicer.Slice(_selectedObject, startPlanePoint, endPlanePoint, planeNormal, args.TopColor, args.BottomColor, args.ContentColor))

            yStart += args.SliceSize
            sliceIndex += 1
        End While

        _layers = output
        e.Result = args.Print
    End Sub

    Private Sub m_bwSlicer_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles _slicer.ProgressChanged
        Dim progress As SliceProgress = DirectCast(e.UserState, SliceProgress)

        tspbProgress.Maximum = progress.NumSlices
        tspbProgress.Value = progress.SliceIndex
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

    Private Sub m_bwSlicer_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _slicer.RunWorkerCompleted
        Dim printBool As Boolean = DirectCast(e.Result, Boolean)

        tsslStatus.Text = String.Empty
        tspbProgress.Value = 0
        tspbProgress.Visible = False
        mnuFilePrint.Enabled = True
        mnuFilePrintPreview.Enabled = True

        If e.Error Is Nothing Then
            If printBool Then
                Using preview As New PrintPreviewDialog
                    Using llpdPrint As New LayerListPrintDocument(_layers)
                        preview.Document = llpdPrint

                        preview.ShowDialog(Me)
                    End Using
                End Using
            Else
                Using saveDialog As New SaveFileDialog
                    saveDialog.FileName = "Select the folder to save SVG files"
                    If saveDialog.ShowDialog() = DialogResult.OK Then
                        Dim selectedPath As String = Path.GetDirectoryName(saveDialog.FileName)

                        For Each layer As Layer In _layers
                            Dim xmlDoc As New XmlDocument()
                            Dim svgRoot As XmlElement = xmlDoc.CreateElement("svg", "http://www.w3.org/2000/svg")
                            xmlDoc.AppendChild(svgRoot)

                            Dim xMin As Single = layer.Bounds.Minimum.X
                            Dim zMin As Single = layer.Bounds.Minimum.Z

                            Dim svgLayerTop As XmlElement = ConvertLayerToSvg(xmlDoc, layer.TopOutline, xMin, zMin, "top")
                            svgRoot.AppendChild(svgLayerTop)

                            Dim svgLayerBottom As XmlElement = ConvertLayerToSvg(xmlDoc, layer.BottomOutline, xMin, zMin, "bottom")
                            svgRoot.AppendChild(svgLayerBottom)

                            xmlDoc.Save(Path.Combine(selectedPath, "layer" & _layers.IndexOf(layer) & ".svg"))
                        Next
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub LoadModelStats()
        Dim totalHeight As Decimal
        Dim totalWidth As Decimal
        Dim totalDepth As Decimal
        Dim totalArea As Decimal

        totalHeight = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Height).Sum()
        totalWidth = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Width).Sum()
        totalDepth = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Depth).Sum()
        totalArea = (From gtgPart As GeometryTriangleGroup In _geometry.Groups Select gtgPart.Bounds.Height * gtgPart.Bounds.Width * gtgPart.Bounds.Depth).Sum() / 1000000000

        lblTotalHeight.Text = totalHeight.ToString("#,##0") & " mm"
        lblTotalWidth.Text = totalWidth.ToString("#,##0") & " mm"
        lblTotalDepth.Text = totalDepth.ToString("#,##0") & " mm"
        lblTotalVolume.Text = totalArea.ToString("#,##0.000") & " M³"
    End Sub

    Private Sub ReloadModelFile()
        If _fileName IsNot Nothing Then
            OpenModelFile(_fileName)
        End If
    End Sub

    Private Sub OpenModelFile(strFile As String)
        Dim units As String = unitsComboBox.SelectedItem.ToString()
        Dim loadUnit As Unit = Geometry.StringToUnit(units)
        Dim upAxis As Axis = If(zUpRadioButton.Checked, Axis.Z, Axis.Y)

        _geometry = Geometry.LoadWavefrontObj(strFile, loadUnit, upAxis)

        LoadModelStats()

        tsslFile.Text = strFile

        tsslFile.Text = strFile
        _fileName = strFile

        mnuFileReload.Enabled = True
        mnuFileExport.Enabled = True
        mnuFilePrint.Enabled = True
        mnuFilePrintPreview.Enabled = True

        lbObjects.DataSource = _geometry.Groups
    End Sub

    Private Sub SetSelectedObject(gtgObject As GeometryTriangleGroup)
        Dim modelMatrix As Matrix

        _selectedObject = gtgObject

        If _selectedObject IsNot Nothing Then
            modelMatrix = Matrix.FromBoundingBox(_selectedObject.Bounds)

            mdFront.ModelMatrix = modelMatrix
            mdRight.ModelMatrix = modelMatrix
            mdBottom.ModelMatrix = modelMatrix
            mdIso.ModelMatrix = modelMatrix * Matrix.Scale(1 / 1.212)

            lblHeight.Text = (_selectedObject.Bounds.Height).ToString("#,##0") & " mm"
            lblWidth.Text = (_selectedObject.Bounds.Width).ToString("#,##0") & " mm"
            lblDepth.Text = (_selectedObject.Bounds.Depth).ToString("#,##0") & " mm"
            lblVolume.Text = (_selectedObject.Bounds.Height * _selectedObject.Bounds.Width * _selectedObject.Bounds.Depth / 1000000000).ToString("#,##0.000") & " M³"
        Else
            lblHeight.Text = String.Empty
            lblWidth.Text = String.Empty
            lblDepth.Text = String.Empty
            lblVolume.Text = String.Empty
        End If

        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub UpdateTrackBar()
        If _selectedObject IsNot Nothing Then
            Dim intNumSlices As Integer = Math.Ceiling(_selectedObject.Bounds.Height / nudThickness.Value)
            tbSlice.Maximum = intNumSlices
            lblSlices.Text = intNumSlices
        Else
            tbSlice.Maximum = 0
            lblSlices.Text = String.Empty
        End If
    End Sub

    Private Sub UpdateDisplay()
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
            sliceStart = New Vector3(0, _selectedObject.Bounds.Minimum.Y + CSng(nudThickness.Value) * (tbSlice.Value + 1), 0)
            sliceEnd = New Vector3(0, _selectedObject.Bounds.Minimum.Y + CSng(nudThickness.Value) * tbSlice.Value, 0)
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
