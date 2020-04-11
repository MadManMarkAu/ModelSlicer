Public Class LineDrawer
    Inherits Control

    Private m_alsLineSegments() As LineSegment
    Private m_rViewBounds As RectangleF
    Private m_tmMatrix As New TransformMatrix

    ''' <summary>
    ''' Gets or sets the rectangular area to display.
    ''' </summary>
    Public Property ViewBounds As RectangleF
        Get
            Return m_rViewBounds
        End Get
        Set(value As RectangleF)
            m_tmMatrix.SetBounds(value)

            m_rViewBounds = value

            Invalidate()
        End Set
    End Property

    ' Override to give this control a border
    ' This is Windows API magic, don't mess with it
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cpParams As CreateParams

            cpParams = MyBase.CreateParams
            cpParams.ExStyle = cpParams.ExStyle Or &H10000I
            cpParams.ExStyle = cpParams.ExStyle And (Not &H200I)
            cpParams.Style = cpParams.Style Or &H800000I

            Return cpParams
        End Get
    End Property

    Public Sub New()
        ' Set up some control styles for this control
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    ' Called when the control has been resized
    Protected Overrides Sub OnResize(e As EventArgs)
        m_tmMatrix.SetViewport(ClientSize)

        MyBase.OnResize(e)
    End Sub

    ''' <summary>
    ''' Uploads an array of line segments to draw on the control.
    ''' </summary>
    ''' <param name="alsLineSegments">An array of <see cref="LineSegment"/> structures containing the lines to draw.</param>
    Public Sub SetDrawData(alsLineSegments() As LineSegment)
        m_alsLineSegments = alsLineSegments

        Invalidate()
    End Sub

    ' Empty override, to disable any background drawing
    ' All drawing will be done in OnPaint()
    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
    End Sub

    ' Called when the control needs to redraw
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        ' Clear the screen
        e.Graphics.Clear(BackColor)

        If m_alsLineSegments IsNot Nothing Then
            ' Create a Pen object to draw with
            Using pPen As New Pen(ForeColor)
                ' Transform each line point into display coordinates, and draw each line
                For Each lsSegment As LineSegment In m_alsLineSegments
                    e.Graphics.DrawLine(pPen, m_tmMatrix.Transform(lsSegment.P1), m_tmMatrix.Transform(lsSegment.P2))
                Next
            End Using
        End If
    End Sub

    ' This is a utility class for transforming line points into screen drawing coordinates
    ' Translates and scales the points, so the lines visible are those contained withing the rectanglular area that was set
    ' This is basically a matrix transform, but as we're only doing translation and scaling, and not rotation, we can omit some of the matrix terms
    Private Class TransformMatrix
        Private m_sngViewScaleX As Single     ' The translation+scale matrix, to convert line coordinates into view coordinates (-1 <= x <= 1)
        Private m_sngViewScaleY As Single     ' "
        Private m_sngViewTranslateX As Single ' "
        Private m_sngViewTranslateY As Single ' "

        Private m_sngScreenScaleX As Single     ' The translation+scale matrix, to convert view coordinates into screen coordinates
        Private m_sngScreenScaleY As Single     ' "
        Private m_sngScreenTranslateX As Single ' "
        Private m_sngScreenTranslateY As Single ' "

        ' Adjusts the area that the user will be able to see, in line coordinates, while preserving aspect ratio
        Public Sub SetBounds(rBounds As RectangleF)
            Dim sngLargest As Single

            ' Get the largest dimension of the view area, so we can scale down by that amount
            sngLargest = Math.Max(rBounds.Width, rBounds.Height)

            ' Set the X/Y scale factor
            ' This scales any points within the viewable area to be in the range -1 to 1 (view coordinates)
            m_sngViewScaleX = 2 / sngLargest
            m_sngViewScaleY = 2 / sngLargest

            ' Set the X/Y translation amount
            ' This moves any points within the viewable to be centered around (0, 0)
            m_sngViewTranslateX = -(rBounds.X + rBounds.Width / 2)
            m_sngViewTranslateY = -(rBounds.Y + rBounds.Height / 2)
        End Sub

        ' Adjusts the matrix so the output coordinates will be in the correct screen coordinates, while preserving aspect ratio
        Public Sub SetViewport(sSize As Size)
            Dim sngSmallest As Single

            ' Get the smallest dimension of the screen area, so we can scale up to that amount
            ' This makes it so the largest view dimension (X or Y) will still be visible
            sngSmallest = Math.Min(sSize.Width, sSize.Height)

            m_sngScreenScaleX = sngSmallest / 2
            m_sngScreenScaleY = sngSmallest / 2

            ' Set the X/Y translation amount
            ' This moves any points in view space to be centered around the center of the display area
            m_sngScreenTranslateX = CSng(sSize.Width) / 2
            m_sngScreenTranslateY = CSng(sSize.Height) / 2
        End Sub

        ' Transforms a given line point into screen coordinates
        Public Function Transform(vPoint As Vector2) As PointF
            Dim sngPosX As Single = vPoint.X
            Dim sngPosY As Single = vPoint.Y

            ' First, transform the point from line space to view space (translate, then scale - a pack operation)
            sngPosX = (sngPosX + m_sngViewTranslateX) * m_sngViewScaleX
            sngPosY = (sngPosY + m_sngViewTranslateY) * m_sngViewScaleY

            ' Next, transform the new point from view space to screen space (scale, then translate - an unpack operation)
            sngPosX = m_sngScreenTranslateX + sngPosX * m_sngScreenScaleX
            sngPosY = m_sngScreenTranslateY + sngPosY * m_sngScreenScaleY

            ' Output the transformed point
            Return New PointF(sngPosX, sngPosY)
        End Function
    End Class
End Class
