''' <summary>
''' Stores an axis-aligned bounding box
''' </summary>
Public Class BoundingBox
    ''' <summary>
    ''' Contains the minimum (X,Y,Z) coordinates describing this bounding box
    ''' </summary>
    Public Minimum As Vector3

    ''' <summary>
    ''' Contains the maximum (X,Y,Z) coordinates describing this bounding box
    ''' </summary>
    Public Maximum As Vector3

    ''' <summary>
    ''' Gets the width of this bounding box (size of X dimension)
    ''' </summary>
    Public ReadOnly Property Width As Single
        Get
            Return Math.Abs(Maximum.X - Minimum.X)
        End Get
    End Property

    ''' <summary>
    ''' Gets the height of this bounding box (size of Y dimension)
    ''' </summary>
    Public ReadOnly Property Height As Single
        Get
            Return Math.Abs(Maximum.Y - Minimum.Y)
        End Get
    End Property

    ''' <summary>
    ''' Gets the depth of this bounding box (size of Z dimension)
    ''' </summary>
    Public ReadOnly Property Depth As Single
        Get
            Return Math.Abs(Maximum.Z - Minimum.Z)
        End Get
    End Property

    Public Sub New(vMinimum As Vector3, vMaximum As Vector3)
        Minimum = vMinimum
        Maximum = vMaximum
    End Sub
End Class
