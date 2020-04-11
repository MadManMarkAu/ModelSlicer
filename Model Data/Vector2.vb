''' <summary>
''' Stores a single (X,Y) point
''' </summary>
Public Structure Vector2
    ''' <summary>
    ''' The X component of this point
    ''' </summary>
    Public X As Single

    ''' <summary>
    ''' The Y component of this point
    ''' </summary>
    Public Y As Single

    Public Sub New(sngX As Single, sngY As Single)
        X = sngX
        Y = sngY
    End Sub
End Structure
