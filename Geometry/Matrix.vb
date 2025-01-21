Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Matrix
    Public M00 As Single
    Public M01 As Single
    Public M02 As Single
    Public M03 As Single
    Public M10 As Single
    Public M11 As Single
    Public M12 As Single
    Public M13 As Single
    Public M20 As Single
    Public M21 As Single
    Public M22 As Single
    Public M23 As Single
    Public M30 As Single
    Public M31 As Single
    Public M32 As Single
    Public M33 As Single

    Public Sub New(sngM00 As Single, sngM01 As Single, sngM02 As Single, sngM03 As Single,
                   sngM10 As Single, sngM11 As Single, sngM12 As Single, sngM13 As Single,
                   sngM20 As Single, sngM21 As Single, sngM22 As Single, sngM23 As Single,
                   sngM30 As Single, sngM31 As Single, sngM32 As Single, sngM33 As Single)
        M00 = sngM00
        M01 = sngM01
        M02 = sngM02
        M03 = sngM03
        M10 = sngM10
        M11 = sngM11
        M12 = sngM12
        M13 = sngM13
        M20 = sngM20
        M21 = sngM21
        M22 = sngM22
        M23 = sngM23
        M30 = sngM30
        M31 = sngM31
        M32 = sngM32
        M33 = sngM33
    End Sub

    Public Sub Normalize()
        Dim sngLength As Single

        sngLength = Math.Sqrt(M00 * M00 + M01 * M01 + M02 * M02)
        M00 /= sngLength
        M01 /= sngLength
        M02 /= sngLength

        sngLength = Math.Sqrt(M10 * M10 + M11 * M11 + M12 * M12)
        M10 /= sngLength
        M11 /= sngLength
        M12 /= sngLength

        sngLength = Math.Sqrt(M20 * M20 + M21 * M21 + M22 * M22)
        M20 /= sngLength
        M21 /= sngLength
        M22 /= sngLength
    End Sub

    Public Sub Transpose()
        Dim mMatTemp As Matrix

        mMatTemp = Me

        M00 = mMatTemp.M00
        M01 = mMatTemp.M10
        M02 = mMatTemp.M20
        M03 = mMatTemp.M30
        M10 = mMatTemp.M01
        M11 = mMatTemp.M11
        M12 = mMatTemp.M21
        M13 = mMatTemp.M31
        M20 = mMatTemp.M02
        M21 = mMatTemp.M12
        M22 = mMatTemp.M22
        M23 = mMatTemp.M32
        M30 = mMatTemp.M03
        M31 = mMatTemp.M13
        M32 = mMatTemp.M23
        M33 = mMatTemp.M33
    End Sub

    Public Shared Function PerspectiveFovLH(sngFov As Single, sngAspect As Single, sngZNear As Single, sngZFar As Single) As Matrix
        Dim mOutput As New Matrix
        Dim sngYScale As Single
        Dim sngQ As Single

        sngYScale = 1.0! / Math.Tan(sngFov * 0.5)
        sngQ = sngZFar / (sngZFar - sngZNear)

        mOutput.M00 = sngYScale / sngAspect
        mOutput.M11 = sngYScale
        mOutput.M22 = sngQ
        mOutput.M23 = 1
        mOutput.M32 = -sngQ * sngZNear

        Return mOutput
    End Function

    Public Shared Function LookAtLH(vEye As Vector3, vTarget As Vector3, vUp As Vector3) As Matrix
        Dim mOutput As Matrix
        Dim vXAxis As Vector3
        Dim vYAxis As Vector3
        Dim vZAxis As Vector3

        vZAxis = vTarget - vEye
        vZAxis.Normalize()

        vXAxis = Vector3.Cross(vUp, vZAxis)
        vXAxis.Normalize()

        vYAxis = Vector3.Cross(vZAxis, vXAxis)

        mOutput.M00 = vXAxis.X
        mOutput.M10 = vXAxis.Y
        mOutput.M20 = vXAxis.Z
        mOutput.M01 = vYAxis.X
        mOutput.M11 = vYAxis.Y
        mOutput.M21 = vYAxis.Z
        mOutput.M02 = vZAxis.X
        mOutput.M12 = vZAxis.Y
        mOutput.M22 = vZAxis.Z
        mOutput.M30 = -Vector3.DotProduct(vXAxis, vEye)
        mOutput.M31 = -Vector3.DotProduct(vYAxis, vEye)
        mOutput.M32 = -Vector3.DotProduct(vZAxis, vEye)
        mOutput.M33 = 1

        Return mOutput
    End Function

    Public Shared Function Normalize(mMatrix As Matrix) As Matrix
        Dim mOutput As Matrix
        Dim sngLength As Single

        sngLength = Math.Sqrt(mMatrix.M00 * mMatrix.M00 + mMatrix.M01 * mMatrix.M01 + mMatrix.M02 * mMatrix.M02)
        mOutput.M00 = mMatrix.M00 / sngLength
        mOutput.M01 = mMatrix.M01 / sngLength
        mOutput.M02 = mMatrix.M02 / sngLength
        mOutput.M03 = mMatrix.M03

        sngLength = Math.Sqrt(mMatrix.M10 * mMatrix.M10 + mMatrix.M11 * mMatrix.M11 + mMatrix.M12 * mMatrix.M12)
        mOutput.M10 = mMatrix.M10 / sngLength
        mOutput.M11 = mMatrix.M11 / sngLength
        mOutput.M12 = mMatrix.M12 / sngLength
        mOutput.M13 = mMatrix.M13

        sngLength = Math.Sqrt(mMatrix.M20 * mMatrix.M20 + mMatrix.M21 * mMatrix.M21 + mMatrix.M22 * mMatrix.M22)
        mOutput.M20 = mMatrix.M20 / sngLength
        mOutput.M21 = mMatrix.M21 / sngLength
        mOutput.M22 = mMatrix.M22 / sngLength
        mOutput.M23 = mMatrix.M23

        mOutput.M30 = mMatrix.M30
        mOutput.M31 = mMatrix.M31
        mOutput.M32 = mMatrix.M32
        mOutput.M33 = mMatrix.M33

        Return mOutput
    End Function

    Public Shared Function Identity() As Matrix
        Return New Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)
    End Function

    ' Given a bounding box, calculates a matrix to scale+translate all coordinates to (-1 <= xyz <= 1)
    Public Shared Function FromBoundingBox(bbBounds As BoundingBox) As Matrix
        Dim mOutput As New Matrix
        Dim sngSmallestScale As Single

        sngSmallestScale = Math.Min(Math.Min(2 / bbBounds.Width, 2 / bbBounds.Height), 2 / bbBounds.Depth)

        mOutput.M00 = sngSmallestScale
        mOutput.M11 = sngSmallestScale
        mOutput.M22 = sngSmallestScale

        mOutput.M30 = -(bbBounds.Minimum.X + bbBounds.Width / 2) * mOutput.M00
        mOutput.M31 = -(bbBounds.Minimum.Y + bbBounds.Height / 2) * mOutput.M11
        mOutput.M32 = -(bbBounds.Minimum.Z + bbBounds.Depth / 2) * mOutput.M22

        mOutput.M33 = 1

        Return mOutput
    End Function

    Public Shared Function RotationX(sngAngleRad As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = 1
        mOut.M11 = Math.Cos(sngAngleRad)
        mOut.M12 = Math.Sin(sngAngleRad)
        mOut.M21 = -Math.Sin(sngAngleRad)
        mOut.M22 = Math.Cos(sngAngleRad)
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function RotationY(sngAngleRad As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = Math.Cos(sngAngleRad)
        mOut.M02 = Math.Sin(sngAngleRad)
        mOut.M11 = 1
        mOut.M20 = -Math.Sin(sngAngleRad)
        mOut.M22 = Math.Cos(sngAngleRad)
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function RotationZ(sngAngleRad As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = Math.Cos(sngAngleRad)
        mOut.M01 = Math.Sin(sngAngleRad)
        mOut.M10 = -Math.Sin(sngAngleRad)
        mOut.M11 = Math.Cos(sngAngleRad)
        mOut.M22 = 1
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function Translation(sngX As Single, sngY As Single, sngZ As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = 1
        mOut.M11 = 1
        mOut.M22 = 1
        mOut.M30 = sngX
        mOut.M31 = sngY
        mOut.M32 = sngZ
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function Translation(vVec As Vector3) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = 1
        mOut.M11 = 1
        mOut.M22 = 1
        mOut.M30 = vVec.X
        mOut.M31 = vVec.Y
        mOut.M32 = vVec.Z
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function Scale(sngValue As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = sngValue
        mOut.M11 = sngValue
        mOut.M22 = sngValue
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function Scale(sngX As Single, sngY As Single, sngZ As Single) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = sngX
        mOut.M11 = sngY
        mOut.M22 = sngZ
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Function Scale(vVec As Vector3) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = vVec.X
        mOut.M11 = vVec.Y
        mOut.M22 = vVec.Z
        mOut.M33 = 1

        Return mOut
    End Function

    Public Shared Operator *(mMatrix As Matrix, vVector As Vector3) As Vector3
        Return New Vector3(
            vVector.X * mMatrix.M00 + vVector.Y * mMatrix.M10 + vVector.Z * mMatrix.M20 + 1 * mMatrix.M30,
            vVector.X * mMatrix.M01 + vVector.Y * mMatrix.M11 + vVector.Z * mMatrix.M21 + 1 * mMatrix.M31,
            vVector.X * mMatrix.M02 + vVector.Y * mMatrix.M12 + vVector.Z * mMatrix.M22 + 1 * mMatrix.M32
        )
    End Operator

    Public Shared Operator *(vVector As Vector3, mMatrix As Matrix) As Vector3
        Return New Vector3(
            vVector.X * mMatrix.M00 + vVector.Y * mMatrix.M10 + vVector.Z * mMatrix.M20 + 1 * mMatrix.M30,
            vVector.X * mMatrix.M01 + vVector.Y * mMatrix.M11 + vVector.Z * mMatrix.M21 + 1 * mMatrix.M31,
            vVector.X * mMatrix.M02 + vVector.Y * mMatrix.M12 + vVector.Z * mMatrix.M22 + 1 * mMatrix.M32
        )
    End Operator

    Public Shared Operator *(mMat1 As Matrix, mMat2 As Matrix) As Matrix
        Dim mOut As New Matrix

        mOut.M00 = mMat1.M00 * mMat2.M00 + mMat1.M01 * mMat2.M10 + mMat1.M02 * mMat2.M20 + mMat1.M03 * mMat2.M30
        mOut.M10 = mMat1.M10 * mMat2.M00 + mMat1.M11 * mMat2.M10 + mMat1.M12 * mMat2.M20 + mMat1.M13 * mMat2.M30
        mOut.M20 = mMat1.M20 * mMat2.M00 + mMat1.M21 * mMat2.M10 + mMat1.M22 * mMat2.M20 + mMat1.M23 * mMat2.M30
        mOut.M30 = mMat1.M30 * mMat2.M00 + mMat1.M31 * mMat2.M10 + mMat1.M32 * mMat2.M20 + mMat1.M33 * mMat2.M30
        mOut.M01 = mMat1.M00 * mMat2.M01 + mMat1.M01 * mMat2.M11 + mMat1.M02 * mMat2.M21 + mMat1.M03 * mMat2.M31
        mOut.M11 = mMat1.M10 * mMat2.M01 + mMat1.M11 * mMat2.M11 + mMat1.M12 * mMat2.M21 + mMat1.M13 * mMat2.M31
        mOut.M21 = mMat1.M20 * mMat2.M01 + mMat1.M21 * mMat2.M11 + mMat1.M22 * mMat2.M21 + mMat1.M23 * mMat2.M31
        mOut.M31 = mMat1.M30 * mMat2.M01 + mMat1.M31 * mMat2.M11 + mMat1.M32 * mMat2.M21 + mMat1.M33 * mMat2.M31
        mOut.M02 = mMat1.M00 * mMat2.M02 + mMat1.M01 * mMat2.M12 + mMat1.M02 * mMat2.M22 + mMat1.M03 * mMat2.M32
        mOut.M12 = mMat1.M10 * mMat2.M02 + mMat1.M11 * mMat2.M12 + mMat1.M12 * mMat2.M22 + mMat1.M13 * mMat2.M32
        mOut.M22 = mMat1.M20 * mMat2.M02 + mMat1.M21 * mMat2.M12 + mMat1.M22 * mMat2.M22 + mMat1.M23 * mMat2.M32
        mOut.M32 = mMat1.M30 * mMat2.M02 + mMat1.M31 * mMat2.M12 + mMat1.M32 * mMat2.M22 + mMat1.M33 * mMat2.M32
        mOut.M03 = mMat1.M00 * mMat2.M03 + mMat1.M01 * mMat2.M13 + mMat1.M02 * mMat2.M23 + mMat1.M03 * mMat2.M33
        mOut.M13 = mMat1.M10 * mMat2.M03 + mMat1.M11 * mMat2.M13 + mMat1.M12 * mMat2.M23 + mMat1.M13 * mMat2.M33
        mOut.M23 = mMat1.M20 * mMat2.M03 + mMat1.M21 * mMat2.M13 + mMat1.M22 * mMat2.M23 + mMat1.M23 * mMat2.M33
        mOut.M33 = mMat1.M30 * mMat2.M03 + mMat1.M31 * mMat2.M13 + mMat1.M32 * mMat2.M23 + mMat1.M33 * mMat2.M33

        Return mOut
    End Operator
End Structure
