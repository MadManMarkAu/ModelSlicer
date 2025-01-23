Imports System.ComponentModel
Imports System.IO
Imports System.Xml

Public Class frmMain
    Private m_gGeometry As Geometry
    Private m_gtgSelectedObject As GeometryTriangleGroup
    Private WithEvents m_bwSlicer As New BackgroundWorker With {.WorkerReportsProgress = True}
    Private m_lstLayers As List(Of Layer)
    Private WithEvents m_importForm As Form

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'mdFront.ViewMatrix = Matrix.Identity() ' .RotationY(Math.PI)
        'mdRight.ViewMatrix = Matrix.RotationY(-Math.PI / 2)
        'mdBottom.ViewMatrix = Matrix.RotationX(Math.PI / 2)
        'mdIso.ViewMatrix = Matrix.RotationY(Math.PI)

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
                Call showImportForm(ofdOpen.FileName)
            End If
        End Using
    End Sub

    Private Sub mnuFileExport_Click(sender As Object, e As EventArgs) Handles mnuFileExport.Click
        m_bwSlicer.RunWorkerAsync(New SliceArgs(m_gtgSelectedObject, CSng(nudThickness.Value) / 1000, Color.Red, Color.Blue, Color.LightGray, False))
    End Sub

    Private Sub mnuFilePrint_Click(sender As Object, e As EventArgs) Handles mnuFilePrint.Click

    End Sub

    Private Sub mnuFilePrintPreview_Click(sender As Object, e As EventArgs) Handles mnuFilePrintPreview.Click
        tsslStatus.Text = "Slicing..."
        mnuFilePrint.Enabled = False
        mnuFilePrintPreview.Enabled = False
        tspbProgress.Visible = True

        m_bwSlicer.RunWorkerAsync(New SliceArgs(m_gtgSelectedObject, CSng(nudThickness.Value), Color.Red, Color.Blue, Color.LightGray, True))
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

    Private Sub nudHeight_ValueChanged(sender As Object, e As EventArgs)
        Call UpdateTrackBar()
        Call UpdateDisplay()
    End Sub

    Private Sub tbSlice_Scroll(sender As Object, e As EventArgs) Handles tbSlice.Scroll
        Call UpdateDisplay()
    End Sub

    Private Sub m_bwSlicer_DoWork(sender As Object, e As DoWorkEventArgs) Handles m_bwSlicer.DoWork
        Dim saArgs As SliceArgs = DirectCast(e.Argument, SliceArgs)
        Dim lstOutput As New List(Of Layer)
        Dim sngYStart As Single
        Dim vStartPlanePoint As Vector3
        Dim vEndPlanePoint As Vector3
        Dim vPlaneNormal As Vector3
        Dim intSliceIndex As Integer
        Dim intNumSlices As Integer

        sngYStart = saArgs.Section.Bounds.Minimum.Y
        vPlaneNormal = New Vector3(0, 1, 0)

        intSliceIndex = 0
        intNumSlices = Math.Ceiling(saArgs.Section.Bounds.Height / saArgs.SliceSize)

        While sngYStart < saArgs.Section.Bounds.Maximum.Y
            m_bwSlicer.ReportProgress(0, New SliceProgress(intSliceIndex, intNumSlices))

            vStartPlanePoint = New Vector3(0, sngYStart, 0)
            vEndPlanePoint = New Vector3(0, sngYStart + saArgs.SliceSize, 0)

            lstOutput.Add(Slicer.Slice(m_gtgSelectedObject, vStartPlanePoint, vEndPlanePoint, vPlaneNormal, saArgs.TopColor, saArgs.BottomColor, saArgs.ContentColor))

            sngYStart += saArgs.SliceSize
            intSliceIndex += 1
        End While

        m_lstLayers = lstOutput
        e.Result = saArgs.Print
    End Sub

    Private Sub m_bwSlicer_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles m_bwSlicer.ProgressChanged
        Dim spProgress As SliceProgress = DirectCast(e.UserState, SliceProgress)

        tspbProgress.Maximum = spProgress.NumSlices
        tspbProgress.Value = spProgress.SliceIndex
    End Sub

    Private Function ConvertLayerToSvg(xmlDoc As XmlDocument, lines As GeometryLineGroup, layerName As String) As XmlElement
        Dim svgLayer As XmlElement = xmlDoc.CreateElement("g", "http://www.w3.org/2000/svg")
        svgLayer.RemoveAllAttributes()
        svgLayer.SetAttribute("id", layerName)
        For Each line As GeometryLine In lines.Lines
            Dim svgLine As XmlElement = xmlDoc.CreateElement("line", "http://www.w3.org/2000/svg")
            svgLine.SetAttribute("x1", (line.V1.X*1000).ToString())
            svgLine.SetAttribute("y1", (line.V1.Z*1000).ToString())
            svgLine.SetAttribute("x2", (line.V2.X*1000).ToString())
            svgLine.SetAttribute("y2", (line.V2.Z*1000).ToString())
            svgLine.SetAttribute("stroke", line.Color.Name)
            svgLayer.AppendChild(svgLine)
        Next
        Return svgLayer
    End Function

    Private Sub m_bwSlicer_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles m_bwSlicer.RunWorkerCompleted
        Dim printBool As Boolean = DirectCast(e.Result, Boolean)
        tsslStatus.Text = String.Empty
        tspbProgress.Value = 0
        tspbProgress.Visible = False
        mnuFilePrint.Enabled = True
        mnuFilePrintPreview.Enabled = True

        If e.Error Is Nothing Then
            If printBool Then
                Using ppdPreview As New PrintPreviewDialog
                    Using llpdPrint As New LayerListPrintDocument(m_lstLayers)
                        ppdPreview.Document = llpdPrint

                        ppdPreview.ShowDialog(Me)
                    End Using
                End Using
            Else
                Using sf As New SaveFileDialog
                    sf.FileName = "Select the folder to save SVG files"
                    If sf.ShowDialog() = DialogResult.OK Then
                        Dim selectedPath As String = Path.GetDirectoryName(sf.FileName)
                        For Each layer As Layer In m_lstLayers
                            Dim xmlDoc As New XmlDocument()
                            Dim svgRoot As XmlElement = xmlDoc.CreateElement("svg", "http://www.w3.org/2000/svg")
                            xmlDoc.AppendChild(svgRoot)

                            Dim svgLayerTop As XmlElement = ConvertLayerToSvg(xmlDoc, layer.TopOutline, "top")
                            svgRoot.AppendChild(svgLayerTop)

                            Dim svgLayerBottom As XmlElement = ConvertLayerToSvg(xmlDoc, layer.BottomOutline, "bottom")
                            svgRoot.AppendChild(svgLayerBottom)

                            xmlDoc.Save(Path.Combine(selectedPath, "layer" & m_lstLayers.IndexOf(layer) & ".svg"))
                        Next
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub m_importForm_FormClosed(sender as Object, e as FormClosedEventArgs) Handles m_importForm.FormClosed
        Dim importForm As Form = DirectCast(sender, Form)

        If importForm.DialogResult = DialogResult.OK Then
            Dim zUpRadioButton As RadioButton = DirectCast(importForm.Controls(2), RadioButton)
            Dim zUp As Boolean = zUpRadioButton.Checked

            Dim unitsComboBox As ComboBox = DirectCast(importForm.Controls(4), ComboBox)
            Dim units As String = unitsComboBox.SelectedItem.ToString()
            Dim scale As Decimal
            Select Case units
                Case "mm"
                    scale = 1
                Case "cm"
                    scale = 10
                Case "m"
                    scale = 1000
                Case "in"
                    scale = 25.4
                Case "ft"
                    scale = 304.8
            End Select

            Dim filePath As Label = DirectCast(importForm.Controls(7), Label)

            Call OpenModelFile(filePath.Text, scale, zUp)
        End If
    End Sub

    Private Sub OpenModelFile(strFile As String, scale As Decimal, zUp As Boolean)
        Dim decTotalHeight As Decimal
        Dim decTotalWidth As Decimal
        Dim decTotalDepth As Decimal
        Dim decTotalArea As Decimal

        m_gGeometry = Geometry.LoadWavefrontObj(strFile, scale, zUp)

        decTotalHeight = (From gtgPart As GeometryTriangleGroup In m_gGeometry.Groups Select gtgPart.Bounds.Height).Sum()
        decTotalWidth = (From gtgPart As GeometryTriangleGroup In m_gGeometry.Groups Select gtgPart.Bounds.Width).Sum()
        decTotalDepth = (From gtgPart As GeometryTriangleGroup In m_gGeometry.Groups Select gtgPart.Bounds.Depth).Sum()
        decTotalArea = (From gtgPart As GeometryTriangleGroup In m_gGeometry.Groups Select gtgPart.Bounds.Height * gtgPart.Bounds.Width * gtgPart.Bounds.Depth).Sum() / 1000000000

        lblTotalHeight.Text = decTotalHeight.ToString("#,##0") & " mm"
        lblTotalWidth.Text = decTotalWidth.ToString("#,##0") & " mm"
        lblTotalDepth.Text = decTotalDepth.ToString("#,##0") & " mm"
        lblTotalVolume.Text = decTotalArea.ToString("#,##0.000") & " M³"

        mnuFileExport.Enabled = True
        mnuFilePrint.Enabled = True
        mnuFilePrintPreview.Enabled = True

        lbObjects.DataSource = m_gGeometry.Groups
    End Sub

    Private Sub SetSelectedObject(gtgObject As GeometryTriangleGroup)
        Dim mModelMatrix As Matrix

        m_gtgSelectedObject = gtgObject

        If m_gtgSelectedObject IsNot Nothing Then
            mModelMatrix = Matrix.FromBoundingBox(m_gtgSelectedObject.Bounds)

            mdFront.ModelMatrix = mModelMatrix
            mdRight.ModelMatrix = mModelMatrix
            mdBottom.ModelMatrix = mModelMatrix
            mdIso.ModelMatrix = mModelMatrix * Matrix.Scale(1 / 1.212)

            lblHeight.Text = (m_gtgSelectedObject.Bounds.Height).ToString("#,##0") & " mm"
            lblWidth.Text = (m_gtgSelectedObject.Bounds.Width).ToString("#,##0") & " mm"
            lblDepth.Text = (m_gtgSelectedObject.Bounds.Depth).ToString("#,##0") & " mm"
            lblVolume.Text = (m_gtgSelectedObject.Bounds.Height * m_gtgSelectedObject.Bounds.Width * m_gtgSelectedObject.Bounds.Depth / 1000000000).ToString("#,##0.000") & " M³"
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
        If m_gtgSelectedObject IsNot Nothing Then
            Dim intNumSlices As Integer = Math.Ceiling(m_gtgSelectedObject.Bounds.Height / nudThickness.Value)
            tbSlice.Maximum = intNumSlices
            lblSlices.Text = intNumSlices
        Else
            tbSlice.Maximum = 0
            lblSlices.Text = String.Empty
        End If
    End Sub

    Private Sub UpdateDisplay()
        Dim vSliceStart As Vector3
        Dim vSliceEnd As Vector3
        Dim vSliceDir As Vector3
        Dim gtgSlice As GeometryTriangleGroup
        Dim mMesh As MesherXYZ
        Dim gtgFullGeom As GeometryTriangleGroup
        Dim gtgSliceGeom As GeometryTriangleGroup
        Dim glgSliceLines As GeometryLineGroup
        Dim glgEndLines As GeometryLineGroup
        Dim glgStartLines As GeometryLineGroup
        Dim glgOpeningLines As GeometryLineGroup
        Dim glgSilhouette As GeometryLineGroup

        If m_gtgSelectedObject IsNot Nothing Then
            vSliceStart = New Vector3(0, m_gtgSelectedObject.Bounds.Minimum.Y + CSng(nudThickness.Value) * (tbSlice.Value + 1), 0)
            vSliceEnd = New Vector3(0, m_gtgSelectedObject.Bounds.Minimum.Y + CSng(nudThickness.Value) * tbSlice.Value, 0)
            vSliceDir = New Vector3(0, -1, 0)

            gtgSlice = Slicer.ExtractBetweenPlanes(m_gtgSelectedObject, vSliceStart, vSliceEnd, vSliceDir)
            mMesh = New MesherXYZ(m_gtgSelectedObject)

            gtgFullGeom = m_gtgSelectedObject.CloneWithColor(Color.LightGray)
            gtgSliceGeom = gtgSlice.CloneWithColor(Color.DarkGray)
            glgSliceLines = gtgSlice.ToLineGroup(Color.Black)
            glgEndLines = Slicer.OutlineModelPlane(gtgSlice, vSliceEnd, vSliceDir, Color.Blue)
            glgStartLines = Slicer.OutlineModelPlane(gtgSlice, vSliceStart, vSliceDir, Color.Red)
            glgOpeningLines = mMesh.CreateDisconnectedEdgesLineGroup(Color.Orange)
            glgSilhouette = mMesh.CreateSilhouette(vSliceDir, Color.Green)

            mdFront.SetDrawData(gtgFullGeom, gtgSliceGeom, glgEndLines, glgStartLines, glgOpeningLines)
            mdRight.SetDrawData(gtgFullGeom, gtgSliceGeom, glgEndLines, glgStartLines, glgOpeningLines)
            mdBottom.SetDrawData(gtgFullGeom, gtgSliceGeom, glgEndLines, glgStartLines, glgOpeningLines, glgSilhouette)
            'mdIso.SetDrawData(gtgFullGeom, gtgSliceGeom, glgSliceLines, glgEndLines, glgStartLines, glgOpeningLines)
            mdIso.SetDrawData(gtgFullGeom, gtgSliceGeom, glgEndLines, glgStartLines, glgOpeningLines, glgSilhouette)
        Else
            mdFront.SetDrawData()
            mdRight.SetDrawData()
            mdBottom.SetDrawData()
            mdIso.SetDrawData()
        End If
    End Sub

    Private Sub showImportForm(strFile As String)
        m_importForm = New Form()
        Dim pathAry() As String = strFile.Split("\")
        m_importForm.Text = "Importing " & pathAry(pathAry.Length-1)
        m_importForm.autoSize = True
        m_importForm.AutoSizeMode = AutoSizeMode.GrowAndShrink
        m_importForm.FormBorderStyle = FormBorderStyle.FixedDialog
        m_importForm.MaximizeBox = False
        m_importForm.MinimizeBox = False
        m_importForm.StartPosition = FormStartPosition.CenterParent
        m_importForm.Padding = New Padding(10)

        Dim lblUpAxis As New Label()
        lblUpAxis.Text = "Up axis:"
        lblUpAxis.Location = New Point(10, 12)
        lblUpAxis.AutoSize = True
        m_importForm.Controls.Add(lblUpAxis)

        Dim yUpRadioButton As New RadioButton()
        yUpRadioButton.Text = "Y-Up"
        yUpRadioButton.Location = New Point(60, 10)
        yUpRadioButton.Checked = True
        yUpRadioButton.AutoSize = True
        m_importForm.Controls.Add(yUpRadioButton)

        Dim zUpRadioButton As New RadioButton()
        zUpRadioButton.Text = "Z-Up"
        zUpRadioButton.Location = New Point(yUpRadioButton.Left + yUpRadioButton.Width + 10, yUpRadioButton.Top)
        zUpRadioButton.AutoSize = True
        m_importForm.Controls.Add(zUpRadioButton)

        Dim lblUnits As New Label()
        lblUnits.Text = "Units:"
        lblUnits.Location = New Point(10, 37)
        lblUnits.AutoSize = True
        m_importForm.Controls.Add(lblUnits)

        Dim unitsComboBox As New ComboBox()
        unitsComboBox.Items.Add("mm")
        unitsComboBox.Items.Add("cm")
        unitsComboBox.Items.Add("m")
        unitsComboBox.Items.Add("in")
        unitsComboBox.Items.Add("ft")
        unitsComboBox.SelectedIndex = 0
        unitsComboBox.Location = New Point(60, 35)
        m_importForm.Controls.Add(unitsComboBox)

        Dim cancelButton As New Button()
        cancelButton.Text = "Cancel"
        cancelButton.DialogResult = DialogResult.Cancel
        cancelButton.Location = New Point(10, 65)
        m_importForm.CancelButton = cancelButton
        m_importForm.Controls.Add(cancelButton)

        Dim okButton As New Button()
        okButton.Text = "OK"
        okButton.DialogResult = DialogResult.OK
        okButton.Location = New Point(100, 65)
        m_importForm.AcceptButton = okButton
        m_importForm.Controls.Add(okButton)

        Dim filePath As New Label()
        filePath.Text = strFile
        filePath.Visible = False
        m_importForm.Controls.Add(filePath)

        m_importForm.ShowDialog()
    End Sub

    Private Class SliceArgs
        Public Section As GeometryTriangleGroup
        Public SliceSize As Single
        Public TopColor As Color
        Public BottomColor As Color
        Public ContentColor As Color
        Public Print As Boolean

        Public Sub New(gtgSection As GeometryTriangleGroup, sngSliceSize As Single, cTopColor As Color, cBottomColor As Color, cContentColor As Color, cPrint As Boolean)
            Section = gtgSection
            SliceSize = sngSliceSize
            TopColor = cTopColor
            BottomColor = cBottomColor
            ContentColor = cContentColor
            Print = cPrint
        End Sub
    End Class

    Private Class SliceProgress
        Public SliceIndex As Integer
        Public NumSlices As Integer

        Public Sub New(intSliceIndex As Integer, intNumSlices As Integer)
            SliceIndex = intSliceIndex
            NumSlices = intNumSlices
        End Sub
    End Class
End Class
