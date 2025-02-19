Public Structure Quaternion
    Public X As Single
    Public Y As Single
    Public Z As Single
    Public W As Single

    Public Sub New(xValue As Single, yValue As Single, zValue As Single, wValue As Single)
        X = xValue
        Y = yValue
        Z = zValue
        W = wValue
    End Sub

    Public Sub New(axis As Vector3, angle As Single)
        axis.Normalize()
        X = axis.X * Math.Sin(angle / 2)
        Y = axis.Y * Math.Sin(angle / 2)
        Z = axis.Z * Math.Sin(angle / 2)
        W = Math.Cos(angle / 2)
    End Sub

    Public Function Magnitude() As Single
        Return Math.Sqrt(X * X + Y * Y + Z * Z + W * W)
    End Function

    Public Sub Normalize()
        Dim mag As Single = Magnitude()

        X /= mag
        Y /= mag
        Z /= mag
        W /= mag
    End Sub

    Public Function Conjugate() As Quaternion
        Return New Quaternion(-X, -Y, -Z, W)
    End Function

    Public Function Forward() As Vector3
        Return New Vector3(
            2 * (X * Z + W * Y),
            2 * (Y * Z - W * X),
            1 - 2 * (X * X + Y * Y)
        )
    End Function

    Public Function Up() As Vector3
        Return New Vector3(
            2 * (X * Y - W * Z),
            1 - 2 * (X * X + Z * Z),
            2 * (Y * Z + W * Z)
        )
    End Function

    Public Function Left() As Vector3
        Return New Vector3(
            1 - 2 * (Y * Y + Z * Z),
            2 * (X * Y + W * Z),
            2 * (X * Z - W * Y)
        )
    End Function

    Public Function ToMatrix() As Matrix
        Return New Matrix(
            1 - 2 * Y * Y - 2 * Z * Z, 2 * X * Y - 2 * Z * W, 2 * X * Z + 2 * Y * W, 0,
            2 * X * Y + 2 * Z * W, 1 - 2 * X * X - 2 * Z * Z, 2 * Y * Z - 2 * X * W, 0,
            2 * X * Z - 2 * Y * W, 2 * Y * Z + 2 * X * W, 1 - 2 * X * X - 2 * Y * Y, 0,
            0, 0, 0, 1
        )
    End Function

    Public Shared Function Identity() As Quaternion
        Return New Quaternion(0, 0, 0, 1)
    End Function

    Public Shared Function Normalize(quat As Quaternion) As Quaternion
        Dim mag As Single = quat.Magnitude()

        Return New Quaternion(quat.X / mag, quat.Y / mag, quat.Z / mag, quat.W / mag)
    End Function

    Public Shared Function FromVectors(fromVector As Vector3, toVector As Vector3) As Quaternion
        Dim cross As Vector3 = Vector3.Cross(fromVector, toVector)
        Dim cosTheta As Single = Vector3.DotProduct(fromVector, toVector)
        Dim k As Single

        If cosTheta >= 1 Then
            Return Identity()
        End If

        k = Math.Sqrt(fromVector.LengthSquared() * toVector.LengthSquared())

        If cosTheta / k <= -1 Then
            If Vector3.DotProduct(fromVector, New Vector3(1, 0, 0)) < 1 Then
                cross = Vector3.Cross(fromVector, New Vector3(1, 0, 0))
                k = 0
                cosTheta = 0
            Else
                cross = Vector3.Cross(fromVector, New Vector3(0, 1, 0))
                k = 0
                cosTheta = 0
            End If
        End If

        Return New Quaternion(cross.X, cross.Y, cross.Z, k + cosTheta)
    End Function

    Public Shared Operator *(quat As Quaternion, vec As Vector3) As Vector3
        Dim quatVec As New Quaternion(vec.X, vec.Y, vec.Z, 0)
        Dim result As Quaternion = quat * quatVec * quat.Conjugate()

        Return New Vector3(result.X, result.Y, result.Z)
    End Operator

    Public Shared Operator *(vec As Vector3, quat As Quaternion) As Vector3
        Dim quatVec As New Quaternion(vec.X, vec.Y, vec.Z, 0)
        Dim result As Quaternion = quat * quatVec * quat.Conjugate()

        Return New Vector3(result.X, result.Y, result.Z)
    End Operator

    Public Shared Operator *(left As Quaternion, right As Quaternion) As Quaternion
        Return New Quaternion(
            left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y,
            left.W * right.Y - left.X * right.Z + left.Y * right.W + left.Z * right.X,
            left.W * right.Z + left.X * right.Y - left.Y * right.X + left.Z * right.W,
            left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z
        )
    End Operator
End Structure
