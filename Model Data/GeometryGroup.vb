Public MustInherit Class GeometryGroup
    Protected _bounds As BoundingBox

    Public Property Name As String = String.Empty
    Public Property EnableLighting As Boolean = False

    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return _bounds
        End Get
    End Property
End Class
