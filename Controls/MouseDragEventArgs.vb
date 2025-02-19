Public Class MouseDragEventArgs
    Inherits EventArgs

    Public ReadOnly Property DeltaX As Integer
    Public ReadOnly Property DeltaY As Integer

    Public Sub New(deltaXValue As Integer, deltaYValue As Integer)
        DeltaX = deltaXValue
        DeltaY = deltaYValue
    End Sub
End Class
