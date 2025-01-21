Public Class GeometryLineGroup
    Inherits GeometryGroup

    Public ReadOnly Property Lines As New List(Of GeometryLine)

    Public Sub UpdateBounds()
        Dim vMin As Vector3
        Dim vMax As Vector3
        Dim blnBoundsInit As Boolean

        For Each glLine As GeometryLine In Lines
            If Not blnBoundsInit Then
                vMin = glLine.Min
                vMax = glLine.Max
                blnBoundsInit = True
            Else
                vMin = Min(vMin, glLine.Min)
                vMax = Max(vMax, glLine.Max)
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

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
