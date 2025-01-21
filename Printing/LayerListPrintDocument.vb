Imports System.Drawing.Printing

Public Class LayerListPrintDocument
    Inherits PrintDocument

    Private m_lstLayers As New List(Of Layer)
    Private m_intPageIndex As Integer
    Private m_intLayerIndex As Integer
    Private m_sngViewOffsetX As Single
    Private m_sngViewOffsetY As Single
    Private m_intViewIndexX As Single
    Private m_intViewIndexY As Single

    Public Sub New(eLayers As IEnumerable(Of Layer))
        m_lstLayers.AddRange(eLayers)
    End Sub

    Protected Overrides Sub OnBeginPrint(e As PrintEventArgs)
        Dim lLayer As Layer = m_lstLayers(0)

        m_intPageIndex = 0
        m_intLayerIndex = 0
        m_sngViewOffsetX = 0
        m_sngViewOffsetX = 0
        m_intViewIndexX = 0
        m_intViewIndexY = 0

        MyBase.OnBeginPrint(e)
    End Sub

    Protected Overrides Sub OnPrintPage(e As PrintPageEventArgs)
        MyBase.OnPrintPage(e)

        Dim sViewSize As New SizeF(TranslateUnitBack(e.MarginBounds.Width), TranslateUnitBack(e.MarginBounds.Height))
        Dim rPrintArea As Rectangle = e.MarginBounds
        Dim lLayer As Layer = m_lstLayers(m_intLayerIndex)

        sViewSize.Height -= PrintSection(e.Graphics, lLayer, e.MarginBounds, New PointF(m_sngViewOffsetX, m_sngViewOffsetY))

        e.HasMorePages = True

        m_sngViewOffsetX += sViewSize.Width
        m_intViewIndexX += 1
        If m_sngViewOffsetX > lLayer.Bounds.Width Then
            m_sngViewOffsetX = 0
            m_intViewIndexX = 0
            m_sngViewOffsetY += sViewSize.Height
            m_intViewIndexY += 1

            If m_sngViewOffsetY > lLayer.Bounds.Depth Then
                m_sngViewOffsetY = 0
                m_intViewIndexY = 0
                m_intLayerIndex += 1

                If m_intLayerIndex >= m_lstLayers.Count Then
                    e.HasMorePages = False
                End If
            End If
        End If

        m_intPageIndex += 1
    End Sub

    Private Function PrintSection(gCanvas As Graphics, lLayer As Layer, rPrintingBounds As RectangleF, pTopLeft As PointF) As Single
        Dim strHeader As String
        Dim sngYOffset As Single
        Dim sngOriginOffsetX As Single = lLayer.Bounds.Minimum.X + pTopLeft.X - TranslateUnitBack(rPrintingBounds.X)
        Dim sngOriginOffsetY As Single = lLayer.Bounds.Minimum.Z + pTopLeft.Y - TranslateUnitBack(rPrintingBounds.Y)

        strHeader = "Page:" & m_intPageIndex & ", Layer:" & m_intLayerIndex & ", X:" & m_intViewIndexX & ", Y:" & m_intViewIndexY

        ' Draw header text
        Using fFont As New Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            Using bBrush As New SolidBrush(Color.Black)
                Using sfFormat As New StringFormat(StringFormatFlags.NoWrap)
                    sngYOffset = gCanvas.MeasureString(strHeader, fFont, New PointF(0, 0), sfFormat).Height
                    gCanvas.DrawString(strHeader, fFont, bBrush, New PointF(rPrintingBounds.X, rPrintingBounds.Y), sfFormat)
                End Using
            End Using
        End Using

        ' Draw content
        For Each gtTriangle As GeometryTriangle In lLayer.Contents.Triangles
            Using bBrush As New SolidBrush(gtTriangle.Color)
                gCanvas.FillPolygon(bBrush, New PointF() {
                    New PointF(TranslateUnit(gtTriangle.V1.X - sngOriginOffsetX), TranslateUnit(gtTriangle.V1.Z - sngOriginOffsetY) + sngYOffset),
                    New PointF(TranslateUnit(gtTriangle.V2.X - sngOriginOffsetX), TranslateUnit(gtTriangle.V2.Z - sngOriginOffsetY) + sngYOffset),
                    New PointF(TranslateUnit(gtTriangle.V3.X - sngOriginOffsetX), TranslateUnit(gtTriangle.V3.Z - sngOriginOffsetY) + sngYOffset)
                })
            End Using
        Next

        ' Draw top
        For Each glLine As GeometryLine In lLayer.TopOutline.Lines
            Using pPen As New Pen(glLine.Color)
                gCanvas.DrawLine(pPen,
                                 TranslateUnit(glLine.V1.X - sngOriginOffsetX), TranslateUnit(glLine.V1.Z - sngOriginOffsetY) + sngYOffset,
                                 TranslateUnit(glLine.V2.X - sngOriginOffsetX), TranslateUnit(glLine.V2.Z - sngOriginOffsetY) + sngYOffset)
            End Using
        Next

        ' Draw bottom
        For Each glLine As GeometryLine In lLayer.BottomOutline.Lines
            Using pPen As New Pen(glLine.Color)
                gCanvas.DrawLine(pPen,
                                 TranslateUnit(glLine.V1.X - sngOriginOffsetX), TranslateUnit(glLine.V1.Z - sngOriginOffsetY) + sngYOffset,
                                 TranslateUnit(glLine.V2.X - sngOriginOffsetX), TranslateUnit(glLine.V2.Z - sngOriginOffsetY) + sngYOffset)
            End Using
        Next

        ' Draw page bounds
        Using pPen As New Pen(Color.Black)
            gCanvas.DrawRectangle(pPen, rPrintingBounds.X, rPrintingBounds.Y + sngYOffset, rPrintingBounds.Width - 1, rPrintingBounds.Height - 1 - sngYOffset)
        End Using

        Return TranslateUnitBack(sngYOffset)
    End Function

    Private Function TranslateUnit(sngInput As Single) As Single
        Return sngInput * 3937.01
    End Function

    Private Function TranslateUnitBack(sngInput As Single) As Single
        Return sngInput / 3937.01
    End Function
End Class
