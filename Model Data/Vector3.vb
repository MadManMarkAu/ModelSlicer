''' <summary>
''' Stores a single (X,Y,Z) point
''' </summary>
Public Structure Vector3
    ''' <summary>
    ''' The X component of this point
    ''' </summary>
    Public X As Single

    ''' <summary>
    ''' The Y component of this point
    ''' </summary>
    Public Y As Single

    ''' <summary>
    ''' The Z component of this point
    ''' </summary>
    Public Z As Single

    Public Sub New(sngX As Single, sngY As Single, sngZ As Single)
        X = sngX
        Y = sngY
        Z = sngZ
    End Sub
End Structure
