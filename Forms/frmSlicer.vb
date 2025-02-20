Imports System.ComponentModel

Public Class frmSlicer
    Private _numSlices As Integer
    Private WithEvents _slicer As New BackgroundWorker With {.WorkerReportsProgress = True, .WorkerSupportsCancellation = True}
    Private _result As List(Of Layer)

    Public Property TriangleGroup As GeometryTriangleGroup
    Public Property Thickness As Single

    Public ReadOnly Property Result As List(Of Layer)
        Get
            Return _result
        End Get
    End Property

    Private Sub frmSlicer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _numSlices = Math.Ceiling(TriangleGroup.Bounds.Height / Thickness)

        _slicer.RunWorkerAsync()
    End Sub

    Private Sub butCancel_Click(sender As Object, e As EventArgs) Handles butCancel.Click
        _slicer.CancelAsync()
    End Sub

    Private Sub _slicer_DoWork(sender As Object, e As DoWorkEventArgs) Handles _slicer.DoWork
        Dim output As New List(Of Layer)
        Dim numSlices As Integer
        Dim sliceSize As Single
        Dim yStart As Single
        Dim yEnd As Single
        Dim planeNormal As Vector3
        Dim startPlanePoint As Vector3
        Dim endPlanePoint As Vector3
        Dim sliceIndex As Integer

        numSlices = _numSlices
        sliceSize = Thickness

        yStart = TriangleGroup.Bounds.Minimum.Y
        yEnd = TriangleGroup.Bounds.Maximum.Y
        planeNormal = New Vector3(0, 1, 0)

        sliceIndex = 0

        While yStart < yEnd
            If _slicer.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            _slicer.ReportProgress(0, New SliceProgress(sliceIndex, numSlices))

            startPlanePoint = New Vector3(0, yStart, 0)
            endPlanePoint = New Vector3(0, yStart + sliceSize, 0)

            output.Add(Slicer.Slice(TriangleGroup, startPlanePoint, endPlanePoint, planeNormal, Color.Red, Color.Blue, Color.LightGray))

            yStart += sliceSize
            sliceIndex += 1
        End While

        e.Result = output
    End Sub

    Private Sub _slicer_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles _slicer.ProgressChanged
        Dim progress As SliceProgress = DirectCast(e.UserState, SliceProgress)

        lblLayer.Text = $"Layer {progress.SliceIndex + 1} of {progress.NumSlices}"

        pbProgress.Maximum = progress.NumSlices
        pbProgress.Value = progress.SliceIndex
    End Sub

    Private Sub _slicer_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles _slicer.RunWorkerCompleted
        If e.Cancelled Then
            DialogResult = DialogResult.Cancel
        ElseIf e.Error IsNot Nothing Then
            ' Handle error.
            MessageBox.Show(Me, $"The slicer threw an error.{vbCrLf}{vbCrLf}{e.Error.GetType().Name}: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        Else
            _result = e.Result
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Class SliceProgress
        Public SliceIndex As Integer
        Public NumSlices As Integer

        Public Sub New(sliceIndexValue As Integer, numSlicesValue As Integer)
            SliceIndex = sliceIndexValue
            NumSlices = numSlicesValue
        End Sub
    End Class
End Class