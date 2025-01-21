Public Structure Layer
    Private m_bbBounds As BoundingBox

    Public ReadOnly Property TopOutline As GeometryLineGroup
    Public ReadOnly Property BottomOutline As GeometryLineGroup
    Public ReadOnly Property Contents As GeometryTriangleGroup

    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return m_bbBounds
        End Get
    End Property

    Public Sub New(glgTopOutline As GeometryLineGroup, glgBottomOutline As GeometryLineGroup, gtgContents As GeometryTriangleGroup)
        TopOutline = glgTopOutline
        BottomOutline = glgBottomOutline
        Contents = gtgContents

        m_bbBounds = gtgContents.Bounds
    End Sub
End Structure
