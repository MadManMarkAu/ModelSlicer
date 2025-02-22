Imports System.IO

Module modSvg
    Private Const SVG_DPI As Single = 96

    Public Sub ExportSlicesToSvg(layers As List(Of Layer), savePath As String, fileNameBase As String, units As Unit,
                                    exportTop As Boolean, exportFill As Boolean, exportBottom As Boolean,
                                    colorTop As Color, colorFill As Color, colorBottom As Color)
        Dim svgNs As XNamespace
        Dim xmlDoc As XDocument
        Dim xmlSvg As XElement
        Dim xMin As Single
        Dim zMin As Single
        Dim xMax As Single
        Dim zMax As Single
        Dim layer As Layer

        For layerIndex As Integer = 0 To layers.Count - 1
            layer = layers(layerIndex)

            xMin = ConvertUnit(layer.Bounds.Minimum.X, units, Unit.Inch) * SVG_DPI
            zMin = ConvertUnit(layer.Bounds.Minimum.Z, units, Unit.Inch) * SVG_DPI
            xMax = ConvertUnit(layer.Bounds.Maximum.X, units, Unit.Inch) * SVG_DPI
            zMax = ConvertUnit(layer.Bounds.Maximum.Z, units, Unit.Inch) * SVG_DPI

            svgNs = "http://www.w3.org/2000/svg"

            xmlSvg = New XElement(svgNs + "svg",
                New XAttribute("width", xMax - xMin),
                New XAttribute("height", zMax - zMin)
            )

            If exportFill Then
                xmlSvg.Add(ConvertFillLayertoSvg(layer.Contents, units, xMin, zMin, "fill", colorFill))
            End If

            If exportTop Then
                xmlSvg.Add(ConvertOutlineLayerToSvg(layer.TopOutline, units, xMin, zMin, "top", colorTop))
            End If

            If exportBottom Then
                xmlSvg.Add(ConvertOutlineLayerToSvg(layer.BottomOutline, units, xMin, zMin, "bottom", colorBottom))
            End If

            xmlDoc = New XDocument(xmlSvg)
            xmlDoc.Save(Path.Combine(savePath, $"{fileNameBase} (Layer {layerIndex}).svg"))
        Next
    End Sub

    Private Function ConvertFillLayertoSvg(tris As GeometryTriangleGroup, units As Unit, xMin As Single, zMin As Single, layerName As String, color As Color) As XElement
        Dim ns As XNamespace
        Dim layer As XElement
        Dim svgTri As XElement
        Dim points As String

        ns = "http://www.w3.org/2000/svg"
        layer = New XElement(ns + "g", New XAttribute("id", layerName))
        For Each tri As GeometryTriangle In tris.Triangles
            points =
                $"{ConvertUnit(tri.V1.X, units, Unit.Inch) * SVG_DPI - xMin},{ConvertUnit(tri.V1.Z, units, Unit.Inch) * SVG_DPI - zMin}" +
                " " +
                $"{ConvertUnit(tri.V2.X, units, Unit.Inch) * SVG_DPI - xMin},{ConvertUnit(tri.V2.Z, units, Unit.Inch) * SVG_DPI - zMin}" +
                " " +
                $"{ConvertUnit(tri.V3.X, units, Unit.Inch) * SVG_DPI - xMin},{ConvertUnit(tri.V3.Z, units, Unit.Inch) * SVG_DPI - zMin}"

            svgTri = New XElement(ns + "polygon",
                New XAttribute("points", points),
                New XAttribute("style", $"fill:{ColorToSvg(color)};stroke:{ColorToSvg(color)}")
            )

            layer.Add(svgTri)
        Next

        Return layer
    End Function

    Private Function ConvertOutlineLayerToSvg(lines As GeometryLineGroup, units As Unit, xMin As Single, zMin As Single, layerName As String, color As Color) As XElement
        Dim ns As XNamespace
        Dim layer As XElement
        Dim svgLine As XElement

        ns = "http://www.w3.org/2000/svg"
        layer = New XElement(ns + "g", New XAttribute("id", layerName))
        For Each line As GeometryLine In lines.Lines
            svgLine = New XElement(ns + "line")
            svgLine.Add(New XAttribute("x1", ConvertUnit(line.V1.X, units, Unit.Inch) * SVG_DPI - xMin))
            svgLine.Add(New XAttribute("y1", ConvertUnit(line.V1.Z, units, Unit.Inch) * SVG_DPI - zMin))
            svgLine.Add(New XAttribute("x2", ConvertUnit(line.V2.X, units, Unit.Inch) * SVG_DPI - xMin))
            svgLine.Add(New XAttribute("y2", ConvertUnit(line.V2.Z, units, Unit.Inch) * SVG_DPI - zMin))
            svgLine.Add(New XAttribute("stroke", ColorToSvg(color)))
            layer.Add(svgLine)
        Next

        Return layer
    End Function

    Private Function ColorToSvg(color As Color) As String
        Return $"#{color.R:X2}{color.G:X2}{color.B:X2}"
    End Function
End Module
