Public Class MouseDragEventArgs
    Inherits EventArgs

    Public ReadOnly Property DeltaX As Integer
    Public ReadOnly Property DeltaY As Integer

    Public Sub New(intDeltaX As Integer, intDeltaY As Integer)
        DeltaX = intDeltaX
        DeltaY = intDeltaY
    End Sub
End Class
