Public Structure Quaternion
    Public X As Single
    Public Y As Single
    Public Z As Single
    Public W As Single

    Public Sub New(sngX As Single, sngY As Single, sngZ As Single, sngW As Single)
        X = sngX
        Y = sngY
        Z = sngZ
        W = sngW
    End Sub

    Public Sub New(vAxis As Vector3, sngAngle As Single)
        vAxis.Normalize()
        X = vAxis.X * Math.Sin(sngAngle / 2)
        Y = vAxis.Y * Math.Sin(sngAngle / 2)
        Z = vAxis.Z * Math.Sin(sngAngle / 2)
        W = Math.Cos(sngAngle / 2)
    End Sub

    Public Function Magnitude() As Single
        Return Math.Sqrt(X * X + Y * Y + Z * Z + W * W)
    End Function

    Public Sub Normalize()
        Dim sngMag As Single = Magnitude()

        X /= sngMag
        Y /= sngMag
        Z /= sngMag
        W /= sngMag
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

    Public Shared Function Normalize(qQuat As Quaternion) As Quaternion
        Dim sngMag As Single = qQuat.Magnitude()

        Return New Quaternion(qQuat.X / sngMag, qQuat.Y / sngMag, qQuat.Z / sngMag, qQuat.W / sngMag)
    End Function

    Public Shared Function FromVectors(vFrom As Vector3, vTo As Vector3) As Quaternion
        Dim vCross As Vector3 = Vector3.Cross(vFrom, vTo)
        Dim sngCosTheta As Single = Vector3.DotProduct(vFrom, vTo)
        Dim sngK As Single

        If sngCosTheta >= 1 Then
            Return Quaternion.Identity()
        End If

        sngK = Math.Sqrt(vFrom.LengthSquared() * vTo.LengthSquared())

        If sngCosTheta / sngK <= -1 Then
            If Vector3.DotProduct(vFrom, New Vector3(1, 0, 0)) < 1 Then
                vCross = Vector3.Cross(vFrom, New Vector3(1, 0, 0))
                sngK = 0
                sngCosTheta = 0
            Else
                vCross = Vector3.Cross(vFrom, New Vector3(0, 1, 0))
                sngK = 0
                sngCosTheta = 0
            End If
        End If

        Return New Quaternion(vCross.X, vCross.Y, vCross.Z, sngK + sngCosTheta)
    End Function

    Public Shared Operator *(qQuat As Quaternion, vVec As Vector3) As Vector3
        Dim qVec As New Quaternion(vVec.X, vVec.Y, vVec.Z, 0)
        Dim qRes As Quaternion = qQuat * qVec * qQuat.Conjugate()

        Return New Vector3(qRes.X, qRes.Y, qRes.Z)
    End Operator

    Public Shared Operator *(vVec As Vector3, qQuat As Quaternion) As Vector3
        Dim qVec As New Quaternion(vVec.X, vVec.Y, vVec.Z, 0)
        Dim qRes As Quaternion = qQuat * qVec * qQuat.Conjugate()

        Return New Vector3(qRes.X, qRes.Y, qRes.Z)
    End Operator

    Public Shared Operator *(qLeft As Quaternion, qRight As Quaternion) As Quaternion
        Return New Quaternion(
            qLeft.W * qRight.X + qLeft.X * qRight.W + qLeft.Y * qRight.Z - qLeft.Z * qRight.Y,
            qLeft.W * qRight.Y - qLeft.X * qRight.Z + qLeft.Y * qRight.W + qLeft.Z * qRight.X,
            qLeft.W * qRight.Z + qLeft.X * qRight.Y - qLeft.Y * qRight.X + qLeft.Z * qRight.W,
            qLeft.W * qRight.W - qLeft.X * qRight.X - qLeft.Y * qRight.Y - qLeft.Z * qRight.Z
        )
    End Operator
End Structure
