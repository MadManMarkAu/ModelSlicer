Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Vector3
    Public X As Single
    Public Y As Single
    Public Z As Single

    Public Sub New(xValue As Single, yValue As Single, zValue As Single)
        X = xValue
        Y = yValue
        Z = zValue
    End Sub

    Public Function LengthSquared() As Single
        Return X * X + Y * Y + Z * Z
    End Function

    Public Function Length() As Single
        Return Math.Sqrt(LengthSquared())
    End Function

    Public Sub Normalize()
        Dim len As Single = Length()

        X = X / len
        Y = Y / len
        Z = Z / len
    End Sub

    Public Function Vector2XY() As Vector2
        Return New Vector2(X, Y)
    End Function

    Public Function Vector2XZ() As Vector2
        Return New Vector2(X, Z)
    End Function

    Public Function Vector2YX() As Vector2
        Return New Vector2(Y, X)
    End Function

    Public Function Vector2YZ() As Vector2
        Return New Vector2(Y, Z)
    End Function

    Public Function Vector2ZX() As Vector2
        Return New Vector2(Z, X)
    End Function

    Public Function Vector2ZY() As Vector2
        Return New Vector2(Z, Y)
    End Function

    Public Overrides Function ToString() As String
        Return $"X={X:0.00000}, Y={Y:0.00000}, Z={Z:0.00000}"
    End Function

    Public Shared Function Normalize(vec As Vector3) As Vector3
        Dim len As Single = vec.Length()

        Return New Vector3(vec.X / len, vec.Y / len, vec.Z / len)
    End Function

    Public Shared Function Cross(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(
            vec1.Y * vec2.Z - vec1.Z * vec2.Y,
            vec1.Z * vec2.X - vec1.X * vec2.Z,
            vec1.X * vec2.Y - vec1.Y * vec2.X)
    End Function

    Public Shared Function DotProduct(vec1 As Vector3, vec2 As Vector3) As Single
        Return vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z
    End Function

    Public Shared Operator +(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z)
    End Operator

    Public Shared Operator -(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z)
    End Operator

    Public Shared Operator *(vec1 As Vector3, vec2 As Vector3) As Vector3
        Return New Vector3(vec1.X * vec2.X, vec1.Y * vec2.Y, vec1.Z * vec2.Z)
    End Operator

    Public Shared Operator *(vec1 As Vector3, k As Single) As Vector3
        Return New Vector3(vec1.X * k, vec1.Y * k, vec1.Z * k)
    End Operator

    Public Shared Operator *(k As Single, vec1 As Vector3) As Vector3
        Return New Vector3(vec1.X * k, vec1.Y * k, vec1.Z * k)
    End Operator

    Public Shared Operator /(vec1 As Vector3, k As Single) As Vector3
        Return New Vector3(vec1.X / k, vec1.Y / k, vec1.Z / k)
    End Operator

    Public Shared Operator =(vec1 As Vector3, vec2 As Vector3) As Boolean
        Return vec1.X = vec2.X AndAlso vec1.Y = vec2.Y AndAlso vec1.Z = vec2.Z
    End Operator

    Public Shared Operator <>(vec1 As Vector3, vec2 As Vector3) As Boolean
        Return vec1.X <> vec2.X OrElse vec1.Y <> vec2.Y OrElse vec1.Z <> vec2.Z
    End Operator
End Structure
