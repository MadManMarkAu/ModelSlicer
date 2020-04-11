''' <summary>
''' Structure to hold a single line segment, from P1 to P2
''' </summary>
Public Structure LineSegment
    ''' <summary>
    ''' The beginning point of this line segment
    ''' </summary>
    Public P1 As Vector2

    ''' <summary>
    ''' The ending point of this line segment
    ''' </summary>
    Public P2 As Vector2

    Public Sub New(vP1 As Vector2, vP2 As Vector2)
        P1 = vP1
        P2 = vP2
    End Sub
End Structure
