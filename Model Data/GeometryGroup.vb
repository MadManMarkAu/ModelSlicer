Public MustInherit Class GeometryGroup
    Protected m_bbBounds As BoundingBox

    Public Property Name As String = String.Empty
    Public Property EnableLighting As Boolean = False

    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return m_bbBounds
        End Get
    End Property
End Class
