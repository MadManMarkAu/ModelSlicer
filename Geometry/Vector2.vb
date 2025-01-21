Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Vector2
    Public X As Single
    Public Y As Single

    Public Sub New(sngX As Single, sngY As Single)
        X = sngX
        Y = sngY
    End Sub

    Public Function Length() As Single
        Return Math.Sqrt(X * X + Y * Y)
    End Function

    Public Sub Normalize()
        Dim sngLength As Single = Length()

        X = X / sngLength
        Y = Y / sngLength
    End Sub

    Public Shared Function Normalize(vVec As Vector2) As Vector2
        Dim sngLength As Single = vVec.Length()

        Return New Vector2(vVec.X / sngLength, vVec.Y / sngLength)
    End Function

    Public Overrides Function ToString() As String
        Return "X=" & X & ", Y=" & Y
    End Function

    Public Shared Operator =(vVec1 As Vector2, vVec2 As Vector2) As Boolean
        Return vVec1.X = vVec2.X AndAlso vVec1.Y = vVec2.Y
    End Operator

    Public Shared Operator <>(vVec1 As Vector2, vVec2 As Vector2) As Boolean
        Return vVec1.X <> vVec2.X OrElse vVec1.Y <> vVec2.Y
    End Operator
End Structure
