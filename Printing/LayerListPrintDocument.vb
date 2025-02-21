Imports System.Drawing.Printing

' Print units are 1/100th of an inch. 

Public Class LayerListPrintDocument
    Inherits PrintDocument

    Private ReadOnly _printLayers As PrintLayer() ' Array of PrintLayer objects to print.

    Private _pageIndex As Integer ' The current page number being printed.
    Private _layerIndex As Integer ' The current layer step, from bottom to top, that is being printed.
    Private _viewOffsetX As Single ' The X offset, in paper units, where the current page's origin sits.
    Private _viewOffsetY As Single ' The Y offset, in paper units, where the current page's origin sits.
    Private _viewIndexX As Single ' The X index of the current page.
    Private _viewIndexY As Single ' The Y index of the current page.

    Private _headerFont As Font
    Private _headerBrush As Brush
    Private _headerStringFormat As StringFormat
    Private _borderPen As Pen
    Private _contentBrush As Brush
    Private _topPen As Pen
    Private _bottomPen As Pen

    Public Sub New(layers As IEnumerable(Of Layer), units As Unit)
        _printLayers = layers.Select(Function(x) New PrintLayer(x, units)).ToArray() ' Convert EVERYTHING to a PrintLayer/PrintTri/PrintLine in PAPER UNITS.
    End Sub

    Protected Overrides Sub OnBeginPrint(e As PrintEventArgs)
        ' Initialize variables for the print.
        _pageIndex = 0
        _layerIndex = 0
        _viewOffsetX = _printLayers(0).BoundsMinX
        _viewOffsetY = _printLayers(0).BoundsMinY
        _viewIndexX = 0
        _viewIndexY = 0

        ' Set up resources needed for printing.
        _headerFont = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
        _headerBrush = New SolidBrush(Color.Black)
        _headerStringFormat = New StringFormat(StringFormatFlags.NoWrap)
        _borderPen = New Pen(Color.Black)
        _contentBrush = New SolidBrush(Color.Gray)
        _topPen = New Pen(Color.Red)
        _bottomPen = New Pen(Color.Blue)

        MyBase.OnBeginPrint(e)
    End Sub

    Protected Overrides Sub OnPrintPage(e As PrintPageEventArgs)
        Dim currentLayer As PrintLayer
        Dim pageHeader As String ' The header text to display at the top of the page.
        Dim drawArea As RectangleF ' The area we can use for drawing the model slice.
        Dim layerPrinted As Boolean

        MyBase.OnPrintPage(e)

        ' Printable area is defined by e.MarginBounds

        currentLayer = _printLayers(_layerIndex)

        ' Keep on looping until something is actually put on paper (skip blank pages).
        Do
            ' Figure out the renderable area for the slice by calculating the header size.
            pageHeader = $"Page:{_pageIndex}, Layer:{_layerIndex}, X:{_viewIndexX}, Y:{_viewIndexY}"
            drawArea = CalculateDrawArea(e.Graphics, e.MarginBounds, pageHeader)

            ' Print the layer.
            layerPrinted = PrintLayerSection(e.Graphics, currentLayer, drawArea, New PointF(_viewOffsetX, _viewOffsetY))

            If layerPrinted Then
                ' Print the header and border.
                ' Do these last so they appear on top of the layer render.
                PrintHeader(e.Graphics, e.MarginBounds, pageHeader)
                PrintBorder(e.Graphics, drawArea)
            End If

            ' Move view X offset to slide the printing window horizontally over the layer.
            ' Then, reset the X offset and move the Y offset to slide the printing window vertically over the layer.
            ' Then, reset the X and Y offsets and move to the next layer.

            _pageIndex += 1

            _viewOffsetX += drawArea.Width
            _viewIndexX += 1
            If _viewOffsetX > currentLayer.BoundsMaxX Then
                _viewOffsetX = currentLayer.BoundsMinX
                _viewIndexX = 0

                _viewOffsetY += drawArea.Height
                _viewIndexY += 1

                If _viewOffsetY > currentLayer.BoundsMaxY Then
                    _viewOffsetY = currentLayer.BoundsMinY
                    _viewIndexY = 0

                    _layerIndex += 1

                    If _layerIndex >= _printLayers.Length Then
                        e.HasMorePages = False
                    Else
                        ' Reset offsets ready for the new layer.
                        _viewOffsetX = _printLayers(_layerIndex).BoundsMinX
                        _viewOffsetY = _printLayers(_layerIndex).BoundsMinY
                        e.HasMorePages = True
                    End If
                Else
                    e.HasMorePages = True
                End If
            Else
                e.HasMorePages = True
            End If
        Loop Until layerPrinted
    End Sub

    Private Function CalculateDrawArea(canvas As Graphics, printableArea As Rectangle, pageHeader As String) As RectangleF
        Dim headerSize As Single

        headerSize = canvas.MeasureString(pageHeader, _headerFont, New PointF(0, 0), _headerStringFormat).Height

        Return New RectangleF(printableArea.X, printableArea.Y + headerSize, printableArea.Width, printableArea.Height - headerSize)
    End Function

    ' layerOrigin is the X/Y coordinates on the current layer where the page origin will sit.
    Private Function PrintLayerSection(canvas As Graphics, layer As PrintLayer, printableArea As RectangleF, layerOrigin As PointF) As Boolean
        Dim printingArea As RectangleF
        Dim oldMatrix As Drawing2D.Matrix
        Dim hasPrintedAnything As Boolean

        ' Calculate the rectangle on the printed layer where the page will sit.
        printingArea = New RectangleF(layerOrigin.X, layerOrigin.Y, printableArea.Width, printableArea.Height)

        ' Apply a translation to the graphics to move the layer section around the page.
        oldMatrix = canvas.Transform
        canvas.Transform = New Drawing2D.Matrix(1, 0, 0, 1, -layerOrigin.X + printableArea.X, -layerOrigin.Y + printableArea.Y)

        ' Crate a clipping region so we don't draw outside of our layout rectangle.
        canvas.SetClip(New RectangleF(layerOrigin.X, layerOrigin.Y, printableArea.Width, printableArea.Height))

        ' Draw triangles.
        For Each tri As PrintTri In layer.Triangles
            If printingArea.IntersectsWith(tri.Bounds) Then
                canvas.FillPolygon(_contentBrush, New PointF() {tri.V1, tri.V2, tri.V3})
                hasPrintedAnything = True
            End If
        Next

        ' Draw top.
        For Each line As PrintLine In layer.TopLines
            If printingArea.IntersectsWith(line.Bounds) Then
                canvas.DrawLine(_topPen, line.V1, line.V2)
                hasPrintedAnything = True
            End If
        Next

        ' Draw bottom.
        For Each line As PrintLine In layer.BottomLines
            If printingArea.IntersectsWith(line.Bounds) Then
                canvas.DrawLine(_bottomPen, line.V1, line.V2)
                hasPrintedAnything = True
            End If
        Next

        ' Restore canvas state.
        canvas.Transform = oldMatrix
        canvas.ResetClip()

        Return hasPrintedAnything
    End Function

    Private Sub PrintHeader(canvas As Graphics, printableArea As Rectangle, pageHeader As String)
        canvas.DrawString(pageHeader, _headerFont, _headerBrush, New PointF(printableArea.X, printableArea.Y), _headerStringFormat)
    End Sub

    Private Sub PrintBorder(canvas As Graphics, borderArea As RectangleF)
        canvas.DrawRectangle(_borderPen, borderArea.X, borderArea.Y, borderArea.Width - 1, borderArea.Height - 1)
    End Sub

    ' Clean up any resources used to render the slice print.
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _bottomPen?.Dispose()
            _bottomPen = Nothing

            _topPen?.Dispose()
            _topPen = Nothing

            _contentBrush?.Dispose()
            _contentBrush = Nothing

            _borderPen?.Dispose()
            _borderPen = Nothing

            _headerStringFormat?.Dispose()
            _headerStringFormat = Nothing

            _headerBrush?.Dispose()
            _headerBrush = Nothing

            _headerFont?.Dispose()
            _headerFont = Nothing
        End If

        MyBase.Dispose(disposing)
    End Sub

    Private Shared Function InchToPaper(inch As Single) As Single
        Return inch * 100 ' Paper units are 1/100th of an inch, and this is fixed by Windows.
    End Function

    ' Structure for holding a flattened triangle, converted to paper units.
    Private Structure PrintTri
        Public ReadOnly V1 As PointF
        Public ReadOnly V2 As PointF
        Public ReadOnly V3 As PointF
        Public ReadOnly Bounds As RectangleF

        Public Sub New(tri As GeometryTriangle, units As Unit)
            Dim v1X As Single
            Dim v1Y As Single
            Dim v2X As Single
            Dim v2Y As Single
            Dim v3X As Single
            Dim v3Y As Single
            Dim minX As Single
            Dim minY As Single
            Dim maxX As Single
            Dim maxY As Single

            ' Notice how the output Y coodinage is the input Z coordinate.
            v1X = InchToPaper(ConvertUnit(tri.V1.X, units, Unit.Inch))
            v1Y = InchToPaper(ConvertUnit(tri.V1.Z, units, Unit.Inch))
            v2X = InchToPaper(ConvertUnit(tri.V2.X, units, Unit.Inch))
            v2Y = InchToPaper(ConvertUnit(tri.V2.Z, units, Unit.Inch))
            v3X = InchToPaper(ConvertUnit(tri.V3.X, units, Unit.Inch))
            v3Y = InchToPaper(ConvertUnit(tri.V3.Z, units, Unit.Inch))

            ' Really fast 3-way min/max functions.
            minX = If(v1X < v2X, v1X, v2X)
            minY = If(v1Y < v2Y, v1Y, v2Y)
            If v3X < minX Then minX = v3X
            If v3Y < minY Then minY = v3Y

            maxX = If(v1X > v2X, v1X, v2X)
            maxY = If(v1Y > v2Y, v1Y, v2Y)
            If v3X > maxX Then maxX = v3X
            If v3Y > maxY Then maxY = v3Y

            V1 = New PointF(v1X, v1Y)
            V2 = New PointF(v2X, v2Y)
            V3 = New PointF(v3X, v3Y)
            Bounds = New RectangleF(minX, minY, maxX - minX, maxY - minY)
        End Sub
    End Structure

    Private Structure PrintLine
        Public ReadOnly V1 As PointF
        Public ReadOnly V2 As PointF
        Public ReadOnly Bounds As RectangleF

        Public Sub New(line As GeometryLine, units As Unit)
            Dim v1X As Single
            Dim v1Y As Single
            Dim v2X As Single
            Dim v2Y As Single
            Dim minX As Single
            Dim minY As Single
            Dim maxX As Single
            Dim maxY As Single

            v1X = InchToPaper(ConvertUnit(line.V1.X, units, Unit.Inch))
            v1Y = InchToPaper(ConvertUnit(line.V1.Z, units, Unit.Inch))
            v2X = InchToPaper(ConvertUnit(line.V2.X, units, Unit.Inch))
            v2Y = InchToPaper(ConvertUnit(line.V2.Z, units, Unit.Inch))

            minX = If(v1X < v2X, v1X, v2X)
            minY = If(v1Y < v2Y, v1Y, v2Y)
            maxX = If(v1X > v2X, v1X, v2X)
            maxY = If(v1Y > v2Y, v1Y, v2Y)

            V1 = New PointF(v1X, v1Y)
            V2 = New PointF(v2X, v2Y)
            Bounds = New RectangleF(minX, minY, maxX - minX, maxY - minY)
        End Sub
    End Structure

    Private Structure PrintLayer
        ' Linked lists are faster to scan over than regular lists.
        Public ReadOnly Triangles As LinkedList(Of PrintTri)
        Public ReadOnly TopLines As LinkedList(Of PrintLine)
        Public ReadOnly BottomLines As LinkedList(Of PrintLine)
        Public ReadOnly BoundsMinX As Single
        Public ReadOnly BoundsMinY As Single
        Public ReadOnly BoundsMaxX As Single
        Public ReadOnly BoundsMaxY As Single

        Public Sub New(layer As Layer, units As Unit)
            Triangles = New LinkedList(Of PrintTri)(layer.Contents.Triangles.Select(Function(x) New PrintTri(x, units)))
            TopLines = New LinkedList(Of PrintLine)(layer.TopOutline.Lines.Select(Function(x) New PrintLine(x, units)))
            BottomLines = New LinkedList(Of PrintLine)(layer.BottomOutline.Lines.Select(Function(x) New PrintLine(x, units)))

            BoundsMinX = InchToPaper(ConvertUnit(layer.Bounds.Minimum.X, units, Unit.Inch))
            BoundsMinY = InchToPaper(ConvertUnit(layer.Bounds.Minimum.Z, units, Unit.Inch))
            BoundsMaxX = InchToPaper(ConvertUnit(layer.Bounds.Maximum.X, units, Unit.Inch))
            BoundsMaxY = InchToPaper(ConvertUnit(layer.Bounds.Maximum.Z, units, Unit.Inch))
        End Sub
    End Structure
End Class
