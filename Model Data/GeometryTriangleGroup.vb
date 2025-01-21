Public Class GeometryTriangleGroup
    Inherits GeometryGroup

    Public ReadOnly Property Triangles As New List(Of GeometryTriangle)

    Public Sub New()
        EnableLighting = True
    End Sub

    Public Sub UpdateBounds()
        Dim vMin As Vector3
        Dim vMax As Vector3
        Dim blnBoundsInit As Boolean

        For Each gtTriangle As GeometryTriangle In Triangles
            If Not blnBoundsInit Then
                vMin = gtTriangle.Min
                vMax = gtTriangle.Max
                blnBoundsInit = True
            Else
                vMin = Min(vMin, gtTriangle.Min)
                vMax = Max(vMax, gtTriangle.Max)
            End If
        Next

        m_bbBounds = New BoundingBox(vMin, vMax)
    End Sub

    Private Function Min(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Min(vVec1.X, vVec2.X),
            Math.Min(vVec1.Y, vVec2.Y),
            Math.Min(vVec1.Z, vVec2.Z)
        )
    End Function

    Private Function Max(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Max(vVec1.X, vVec2.X),
            Math.Max(vVec1.Y, vVec2.Y),
            Math.Max(vVec1.Z, vVec2.Z)
        )
    End Function

    Public Function CloneWithColor(cColor As Color) As GeometryTriangleGroup
        Dim gtgOutput As New GeometryTriangleGroup

        gtgOutput.Name = Name

        For Each gtTriangle As GeometryTriangle In Triangles
            gtgOutput.Triangles.Add(New GeometryTriangle(cColor, gtTriangle.V1, gtTriangle.V2, gtTriangle.V3, gtTriangle.V1Normal, gtTriangle.V2Normal, gtTriangle.V3Normal, gtTriangle.SurfaceNormal))
        Next

        gtgOutput.UpdateBounds()

        Return gtgOutput
    End Function

    Public Function ToLineGroup(cColor As Color) As GeometryLineGroup
        Dim glgOutput As New GeometryLineGroup
        Dim gtTriangle As GeometryTriangle

        For Each gtTriangle In Triangles
            glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V1, gtTriangle.V2, gtTriangle.V1Normal, gtTriangle.V2Normal, gtTriangle.SurfaceNormal))
            glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V2, gtTriangle.V3, gtTriangle.V2Normal, gtTriangle.V3Normal, gtTriangle.SurfaceNormal))
            glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V3, gtTriangle.V1, gtTriangle.V3Normal, gtTriangle.V1Normal, gtTriangle.SurfaceNormal))
        Next

        glgOutput.UpdateBounds()

        Return glgOutput
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
