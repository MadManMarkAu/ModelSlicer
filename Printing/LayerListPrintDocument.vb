Imports System.Drawing.Printing

Public Class LayerListPrintDocument
    Inherits PrintDocument

    Private _layers As New List(Of Layer)
    Private _pageIndex As Integer
    Private _layerIndex As Integer
    Private _viewOffsetX As Single
    Private _viewOffsetY As Single
    Private _viewIndexX As Single
    Private _viewIndexY As Single

    Public Sub New(eLayers As IEnumerable(Of Layer))
        _layers.AddRange(eLayers)
    End Sub

    Protected Overrides Sub OnBeginPrint(e As PrintEventArgs)
        _pageIndex = 0
        _layerIndex = 0
        _viewOffsetX = 0
        _viewOffsetX = 0
        _viewIndexX = 0
        _viewIndexY = 0

        MyBase.OnBeginPrint(e)
    End Sub

    Protected Overrides Sub OnPrintPage(e As PrintPageEventArgs)
        MyBase.OnPrintPage(e)

        Dim viewSize As New SizeF(TranslateUnitBack(e.MarginBounds.Width), TranslateUnitBack(e.MarginBounds.Height))
        Dim printArea As Rectangle = e.MarginBounds
        Dim layer As Layer = _layers(_layerIndex)

        viewSize.Height -= PrintSection(e.Graphics, layer, e.MarginBounds, New PointF(_viewOffsetX, _viewOffsetY))

        e.HasMorePages = True

        _viewOffsetX += viewSize.Width
        _viewIndexX += 1
        If _viewOffsetX > layer.Bounds.Width Then
            _viewOffsetX = 0
            _viewIndexX = 0
            _viewOffsetY += viewSize.Height
            _viewIndexY += 1

            If _viewOffsetY > layer.Bounds.Depth Then
                _viewOffsetY = 0
                _viewIndexY = 0
                _layerIndex += 1

                If _layerIndex >= _layers.Count Then
                    e.HasMorePages = False
                End If
            End If
        End If

        _pageIndex += 1
    End Sub

    Private Function PrintSection(canvas As Graphics, layer As Layer, printingBounds As RectangleF, topLeft As PointF) As Single
        Dim header As String
        Dim yOffset As Single
        Dim originOffsetX As Single = layer.Bounds.Minimum.X + topLeft.X - TranslateUnitBack(printingBounds.X)
        Dim originOffsetY As Single = layer.Bounds.Minimum.Z + topLeft.Y - TranslateUnitBack(printingBounds.Y)

        header = "Page:" & _pageIndex & ", Layer:" & _layerIndex & ", X:" & _viewIndexX & ", Y:" & _viewIndexY

        ' Draw header text
        Using font As New Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            Using bBrush As New SolidBrush(Color.Black)
                Using sfFormat As New StringFormat(StringFormatFlags.NoWrap)
                    yOffset = canvas.MeasureString(header, font, New PointF(0, 0), sfFormat).Height
                    canvas.DrawString(header, font, bBrush, New PointF(printingBounds.X, printingBounds.Y), sfFormat)
                End Using
            End Using
        End Using

        ' Draw content
        For Each tri As GeometryTriangle In layer.Contents.Triangles
            Using brush As New SolidBrush(tri.Color)
                canvas.FillPolygon(brush, New PointF() {
                    New PointF(TranslateUnit(tri.V1.X - originOffsetX), TranslateUnit(tri.V1.Z - originOffsetY) + yOffset),
                    New PointF(TranslateUnit(tri.V2.X - originOffsetX), TranslateUnit(tri.V2.Z - originOffsetY) + yOffset),
                    New PointF(TranslateUnit(tri.V3.X - originOffsetX), TranslateUnit(tri.V3.Z - originOffsetY) + yOffset)
                })
            End Using
        Next

        ' Draw top
        For Each line As GeometryLine In layer.TopOutline.Lines
            Using pen As New Pen(line.Color)
                canvas.DrawLine(pen,
                                 TranslateUnit(line.V1.X - originOffsetX), TranslateUnit(line.V1.Z - originOffsetY) + yOffset,
                                 TranslateUnit(line.V2.X - originOffsetX), TranslateUnit(line.V2.Z - originOffsetY) + yOffset)
            End Using
        Next

        ' Draw bottom
        For Each line As GeometryLine In layer.BottomOutline.Lines
            Using pen As New Pen(line.Color)
                canvas.DrawLine(pen,
                                 TranslateUnit(line.V1.X - originOffsetX), TranslateUnit(line.V1.Z - originOffsetY) + yOffset,
                                 TranslateUnit(line.V2.X - originOffsetX), TranslateUnit(line.V2.Z - originOffsetY) + yOffset)
            End Using
        Next

        ' Draw page bounds
        Using pen As New Pen(Color.Black)
            canvas.DrawRectangle(pen, printingBounds.X, printingBounds.Y + yOffset, printingBounds.Width - 1, printingBounds.Height - 1 - yOffset)
        End Using

        Return TranslateUnitBack(yOffset)
    End Function

    Private Function TranslateUnit(input As Single) As Single
        Return input * 3937.01
    End Function

    Private Function TranslateUnitBack(input As Single) As Single
        Return input / 3937.01
    End Function
End Class
