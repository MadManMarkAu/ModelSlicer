Public Class GeometryLineGroup
    Inherits GeometryGroup

    Public ReadOnly Property Lines As New List(Of GeometryLine)

    Public Sub UpdateBounds()
        Dim min As Vector3
        Dim max As Vector3
        Dim isBoundsInit As Boolean

        For Each line As GeometryLine In Lines
            If Not isBoundsInit Then
                min = line.Min
                max = line.Max
                isBoundsInit = True
            Else
                min = Me.Min(min, line.Min)
                max = Me.Max(max, line.Max)
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

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
