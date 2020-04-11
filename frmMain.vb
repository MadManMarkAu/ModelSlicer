' The main display form of the app
Public Class frmMain
    Private m_gGeometry As Geometry ' Class variable to store model geometry

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load the Utah Teapot geometry
        m_gGeometry = Geometry.LoadWavefrontObj("teapot.obj")

        ' Inform the line drawing control about the area we expect line data to be contain within
        ldDrawer.ViewBounds = New RectangleF(m_gGeometry.Bounds.Minimum.X, m_gGeometry.Bounds.Minimum.Z, m_gGeometry.Bounds.Width, m_gGeometry.Bounds.Depth)

        ' Set the min, max and current value of the slider control
        ' Note we multiply by 100, as the slider control only takes integers
        tbSlice.Minimum = m_gGeometry.Bounds.Minimum.Y * 100
        tbSlice.Maximum = m_gGeometry.Bounds.Maximum.Y * 100
        tbSlice.Value = m_gGeometry.Bounds.Minimum.Y * 100

        ' App just loaded, we need to update the display
        Call UpdateDisplay()
    End Sub

    Private Sub tbSlice_Scroll(sender As Object, e As EventArgs) Handles tbSlice.Scroll
        ' Slice slider was scrolled, we need to update the display
        Call UpdateDisplay()
    End Sub

    ' Performs a slice on the loaded geometry, and sends the lines to the line drawing control
    Private Sub UpdateDisplay()
        Dim sngYSlice As Single
        Dim lstSliceLines As List(Of LineSegment)

        ' Get the Y position of the cutting plane
        sngYSlice = CSng(tbSlice.Value) / 100

        ' Cut the model
        lstSliceLines = Slicer.SliceModel(m_gGeometry, sngYSlice)

        ' Send the lines to the line drawing control
        ldDrawer.SetDrawData(lstSliceLines.ToArray())
    End Sub
End Class
