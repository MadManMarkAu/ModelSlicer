Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Vector2
    Public X As Single
    Public Y As Single

    Public Sub New(xValue As Single, yValue As Single)
        X = xValue
        Y = yValue
    End Sub

    Public Function Length() As Single
        Return Math.Sqrt(X * X + Y * Y)
    End Function

    Public Sub Normalize()
        Dim len As Single = Length()

        X = X / Length()
        Y = Y / Length()
    End Sub

    Public Shared Function Normalize(vec As Vector2) As Vector2
        Dim len As Single = vec.Length()

        Return New Vector2(vec.X / len, vec.Y / len)
    End Function

    Public Overrides Function ToString() As String
        Return $"X={X}, Y={Y}"
    End Function

    Public Shared Operator =(vec1 As Vector2, vec2 As Vector2) As Boolean
        Return vec1.X = vec2.X AndAlso vec1.Y = vec2.Y
    End Operator

    Public Shared Operator <>(vec1 As Vector2, vec2 As Vector2) As Boolean
        Return vec1.X <> vec2.X OrElse vec1.Y <> vec2.Y
    End Operator
End Structure
