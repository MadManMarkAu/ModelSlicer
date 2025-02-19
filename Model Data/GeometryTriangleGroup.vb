Public Class GeometryTriangleGroup
    Inherits GeometryGroup

    Public ReadOnly Property Triangles As New List(Of GeometryTriangle)

    Public Sub New()
        EnableLighting = True
    End Sub

    Public Sub UpdateBounds()
        Dim min As Vector3
        Dim max As Vector3
        Dim isBoundsInit As Boolean

        For Each tri As GeometryTriangle In Triangles
            If Not isBoundsInit Then
                min = tri.Min
                max = tri.Max
                isBoundsInit = True
            Else
                min = Me.Min(min, tri.Min)
                max = Me.Max(max, tri.Max)
            End If
        Next

        _bounds = New BoundingBox(min, max)
    End Sub

    Private Function Min(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Min(vec1.X, vec2.X),
            Math.Min(vec1.Y, vec2.Y),
            Math.Min(vec1.Z, vec2.Z)
        )
    End Function

    Private Function Max(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Max(vec1.X, vec2.X),
            Math.Max(vec1.Y, vec2.Y),
            Math.Max(vec1.Z, vec2.Z)
        )
    End Function

    Public Function CloneWithColor(color As Color) As GeometryTriangleGroup
        Dim output As New GeometryTriangleGroup

        output.Name = Name

        For Each tri As GeometryTriangle In Triangles
            output.Triangles.Add(New GeometryTriangle(color, tri.V1, tri.V2, tri.V3, tri.V1Normal, tri.V2Normal, tri.V3Normal, tri.SurfaceNormal))
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Sub Scale(scale As Single)
        Dim tri As GeometryTriangle

        For i As Integer = 0 To (Triangles.Count - 1)
            tri = Triangles(i)
            Triangles(i) = New GeometryTriangle(tri.Color, tri.V1 * scale, tri.V2 * scale, tri.V3 * scale, tri.V1Normal, tri.V2Normal, tri.V3Normal, tri.SurfaceNormal)
        Next i
        UpdateBounds()
    End Sub

    Public Sub SwapUpAxis()
        Dim tri As GeometryTriangle

        For i As Integer = 0 To (Triangles.Count - 1)
            tri = Triangles(i)
            Triangles(i) = New GeometryTriangle(tri.Color,
                                                 tri.V1.SwapUpAxis(),
                                                 tri.V2.SwapUpAxis(),
                                                 tri.V3.SwapUpAxis(),
                                                 tri.V1Normal.SwapUpAxis(),
                                                 tri.V2Normal.SwapUpAxis(),
                                                 tri.V3Normal.SwapUpAxis(),
                                                 tri.SurfaceNormal.SwapUpAxis())
        Next i
        UpdateBounds()
    End Sub

    Public Function ToLineGroup(color As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup

        For Each tri As GeometryTriangle In Triangles
            output.Lines.Add(New GeometryLine(color, tri.V1, tri.V2, tri.V1Normal, tri.V2Normal, tri.SurfaceNormal))
            output.Lines.Add(New GeometryLine(color, tri.V2, tri.V3, tri.V2Normal, tri.V3Normal, tri.SurfaceNormal))
            output.Lines.Add(New GeometryLine(color, tri.V3, tri.V1, tri.V3Normal, tri.V1Normal, tri.SurfaceNormal))
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
