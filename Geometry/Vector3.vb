Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Vector3
    Public X As Single
    Public Y As Single
    Public Z As Single

    Public Sub New(sngX As Single, sngY As Single, sngZ As Single)
        X = sngX
        Y = sngY
        Z = sngZ
    End Sub

    Public Function LengthSquared() As Single
        Return X * X + Y * Y + Z * Z
    End Function

    Public Function Length() As Single
        Return Math.Sqrt(LengthSquared())
    End Function

    Public Sub Normalize()
        Dim sngLength As Single = Length()

        X = X / sngLength
        Y = Y / sngLength
        Z = Z / sngLength
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
        Return "X=" & X & ", Y=" & Y & ", Z=" & Z
    End Function

    Public Function SwapUpAxis()
        Return New Vector3(X, -Z, -Y)
    End Function

    Public Shared Function Normalize(vVec As Vector3) As Vector3
        Dim sngLength As Single = vVec.Length()

        Return New Vector3(vVec.X / sngLength, vVec.Y / sngLength, vVec.Z / sngLength)
    End Function

    Public Shared Function Cross(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(
            vVec1.Y * vVec2.Z - vVec1.Z * vVec2.Y,
            vVec1.Z * vVec2.X - vVec1.X * vVec2.Z,
            vVec1.X * vVec2.Y - vVec1.Y * vVec2.X)
    End Function

    Public Shared Function DotProduct(vVec1 As Vector3, vVec2 As Vector3) As Single
        Return vVec1.X * vVec2.X + vVec1.Y * vVec2.Y + vVec1.Z * vVec2.Z
    End Function

    Public Shared Operator +(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(vVec1.X + vVec2.X, vVec1.Y + vVec2.Y, vVec1.Z + vVec2.Z)
    End Operator

    Public Shared Operator -(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(vVec1.X - vVec2.X, vVec1.Y - vVec2.Y, vVec1.Z - vVec2.Z)
    End Operator

    Public Shared Operator *(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(vVec1.X * vVec2.X, vVec1.Y * vVec2.Y, vVec1.Z * vVec2.Z)
    End Operator

    Public Shared Operator *(vVec1 As Vector3, sngK As Single) As Vector3
        Return New Vector3(vVec1.X * sngK, vVec1.Y * sngK, vVec1.Z * sngK)
    End Operator

    Public Shared Operator *(sngK As Single, vVec1 As Vector3) As Vector3
        Return New Vector3(vVec1.X * sngK, vVec1.Y * sngK, vVec1.Z * sngK)
    End Operator

    Public Shared Operator /(vVec1 As Vector3, sngK As Single) As Vector3
        Return New Vector3(vVec1.X / sngK, vVec1.Y / sngK, vVec1.Z / sngK)
    End Operator

    Public Shared Operator =(vVec1 As Vector3, vVec2 As Vector3) As Boolean
        Return vVec1.X = vVec2.X AndAlso vVec1.Y = vVec2.Y AndAlso vVec1.Z = vVec2.Z
    End Operator

    Public Shared Operator <>(vVec1 As Vector3, vVec2 As Vector3) As Boolean
        Return vVec1.X <> vVec2.X OrElse vVec1.Y <> vVec2.Y OrElse vVec1.Z <> vVec2.Z
    End Operator
End Structure
