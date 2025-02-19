Public Structure Layer
    Private _bounds As BoundingBox

    Public ReadOnly Property TopOutline As GeometryLineGroup
    Public ReadOnly Property BottomOutline As GeometryLineGroup
    Public ReadOnly Property Contents As GeometryTriangleGroup

    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return _bounds
        End Get
    End Property

    Public Sub New(topOutlineValue As GeometryLineGroup, bottomOutlineValue As GeometryLineGroup, contentsValue As GeometryTriangleGroup)
        topOutlineValue = topOutlineValue
        BottomOutline = bottomOutlineValue
        Contents = contentsValue

        _bounds = contentsValue.Bounds
    End Sub
End Structure
