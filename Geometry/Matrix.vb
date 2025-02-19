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

    Public Sub New(matrix00 As Single, matrix01 As Single, matrix02 As Single, matrix03 As Single,
                   matrix10 As Single, matrix11 As Single, matrix12 As Single, matrix13 As Single,
                   matrix20 As Single, matrix21 As Single, matrix22 As Single, matrix23 As Single,
                   matrix30 As Single, matrix31 As Single, matrix32 As Single, matrix33 As Single)
        M00 = matrix00
        M01 = matrix01
        M02 = matrix02
        M03 = matrix03
        M10 = matrix10
        M11 = matrix11
        M12 = matrix12
        M13 = matrix13
        M20 = matrix20
        M21 = matrix21
        M22 = matrix22
        M23 = matrix23
        M30 = matrix30
        M31 = matrix31
        M32 = matrix32
        M33 = matrix33
    End Sub

    Public Sub Normalize()
        Dim length As Single

        length = Math.Sqrt(M00 * M00 + M01 * M01 + M02 * M02)
        M00 /= length
        M01 /= length
        M02 /= length

        length = Math.Sqrt(M10 * M10 + M11 * M11 + M12 * M12)
        M10 /= length
        M11 /= length
        M12 /= length

        length = Math.Sqrt(M20 * M20 + M21 * M21 + M22 * M22)
        M20 /= length
        M21 /= length
        M22 /= length
    End Sub

    Public Sub Transpose()
        Dim temp As Matrix

        temp = Me

        M00 = temp.M00
        M01 = temp.M10
        M02 = temp.M20
        M03 = temp.M30
        M10 = temp.M01
        M11 = temp.M11
        M12 = temp.M21
        M13 = temp.M31
        M20 = temp.M02
        M21 = temp.M12
        M22 = temp.M22
        M23 = temp.M32
        M30 = temp.M03
        M31 = temp.M13
        M32 = temp.M23
        M33 = temp.M33
    End Sub

    Public Shared Function PerspectiveFovLH(fov As Single, aspect As Single, zNear As Single, zFar As Single) As Matrix
        Dim output As New Matrix
        Dim yScale As Single
        Dim q As Single

        yScale = 1.0! / Math.Tan(fov * 0.5)
        q = zFar / (zFar - zNear)

        output.M00 = yScale / aspect
        output.M11 = yScale
        output.M22 = q
        output.M23 = 1
        output.M32 = -q * zNear

        Return output
    End Function

    Public Shared Function LookAtLH(eye As Vector3, target As Vector3, up As Vector3) As Matrix
        Dim output As Matrix
        Dim xAxis As Vector3
        Dim yAxis As Vector3
        Dim zAxis As Vector3

        zAxis = target - eye
        zAxis.Normalize()

        xAxis = Vector3.Cross(up, zAxis)
        xAxis.Normalize()

        yAxis = Vector3.Cross(zAxis, xAxis)

        output.M00 = xAxis.X
        output.M10 = xAxis.Y
        output.M20 = xAxis.Z
        output.M01 = yAxis.X
        output.M11 = yAxis.Y
        output.M21 = yAxis.Z
        output.M02 = zAxis.X
        output.M12 = zAxis.Y
        output.M22 = zAxis.Z
        output.M30 = -Vector3.DotProduct(xAxis, eye)
        output.M31 = -Vector3.DotProduct(yAxis, eye)
        output.M32 = -Vector3.DotProduct(zAxis, eye)
        output.M33 = 1

        Return output
    End Function

    Public Shared Function Normalize(matrix As Matrix) As Matrix
        Dim output As Matrix
        Dim length As Single

        length = Math.Sqrt(matrix.M00 * matrix.M00 + matrix.M01 * matrix.M01 + matrix.M02 * matrix.M02)
        output.M00 = matrix.M00 / length
        output.M01 = matrix.M01 / length
        output.M02 = matrix.M02 / length
        output.M03 = matrix.M03

        length = Math.Sqrt(matrix.M10 * matrix.M10 + matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12)
        output.M10 = matrix.M10 / length
        output.M11 = matrix.M11 / length
        output.M12 = matrix.M12 / length
        output.M13 = matrix.M13

        length = Math.Sqrt(matrix.M20 * matrix.M20 + matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22)
        output.M20 = matrix.M20 / length
        output.M21 = matrix.M21 / length
        output.M22 = matrix.M22 / length
        output.M23 = matrix.M23

        output.M30 = matrix.M30
        output.M31 = matrix.M31
        output.M32 = matrix.M32
        output.M33 = matrix.M33

        Return output
    End Function

    Public Shared Function Identity() As Matrix
        Return New Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)
    End Function

    ' Given a bounding box, calculates a matrix to scale+translate all coordinates to (-1 <= xyz <= 1)
    Public Shared Function FromBoundingBox(bounds As BoundingBox) As Matrix
        Dim output As New Matrix
        Dim smallestScale As Single

        smallestScale = Math.Min(Math.Min(2 / bounds.Width, 2 / bounds.Height), 2 / bounds.Depth)

        output.M00 = smallestScale
        output.M11 = smallestScale
        output.M22 = smallestScale

        output.M30 = -(bounds.Minimum.X + bounds.Width / 2) * output.M00
        output.M31 = -(bounds.Minimum.Y + bounds.Height / 2) * output.M11
        output.M32 = -(bounds.Minimum.Z + bounds.Depth / 2) * output.M22

        output.M33 = 1

        Return output
    End Function

    Public Shared Function RotationX(angleRad As Single) As Matrix
        Dim out As Matrix

        out.M00 = 1
        out.M11 = Math.Cos(angleRad)
        out.M12 = Math.Sin(angleRad)
        out.M21 = -Math.Sin(angleRad)
        out.M22 = Math.Cos(angleRad)
        out.M33 = 1

        Return out
    End Function

    Public Shared Function RotationY(angleRad As Single) As Matrix
        Dim out As Matrix

        out.M00 = Math.Cos(angleRad)
        out.M02 = Math.Sin(angleRad)
        out.M11 = 1
        out.M20 = -Math.Sin(angleRad)
        out.M22 = Math.Cos(angleRad)
        out.M33 = 1

        Return out
    End Function

    Public Shared Function RotationZ(angleRad As Single) As Matrix
        Dim out As Matrix

        out.M00 = Math.Cos(angleRad)
        out.M01 = Math.Sin(angleRad)
        out.M10 = -Math.Sin(angleRad)
        out.M11 = Math.Cos(angleRad)
        out.M22 = 1
        out.M33 = 1

        Return out
    End Function

    Public Shared Function Translation(x As Single, y As Single, z As Single) As Matrix
        Dim out As Matrix

        out.M00 = 1
        out.M11 = 1
        out.M22 = 1
        out.M30 = x
        out.M31 = y
        out.M32 = z
        out.M33 = 1

        Return out
    End Function

    Public Shared Function Translation(v As Vector3) As Matrix
        Dim out As Matrix

        out.M00 = 1
        out.M11 = 1
        out.M22 = 1
        out.M30 = v.X
        out.M31 = v.Y
        out.M32 = v.Z
        out.M33 = 1

        Return out
    End Function

    Public Shared Function Scale(value As Single) As Matrix
        Dim out As Matrix

        out.M00 = value
        out.M11 = value
        out.M22 = value
        out.M33 = 1

        Return out
    End Function

    Public Shared Function Scale(x As Single, y As Single, z As Single) As Matrix
        Dim out As Matrix

        out.M00 = x
        out.M11 = y
        out.M22 = z
        out.M33 = 1

        Return out
    End Function

    Public Shared Function Scale(v As Vector3) As Matrix
        Dim out As Matrix

        out.M00 = v.X
        out.M11 = v.Y
        out.M22 = v.Z
        out.M33 = 1

        Return out
    End Function

    Public Shared Operator *(matrix As Matrix, vector As Vector3) As Vector3
        Return New Vector3(
            vector.X * matrix.M00 + vector.Y * matrix.M10 + vector.Z * matrix.M20 + 1 * matrix.M30,
            vector.X * matrix.M01 + vector.Y * matrix.M11 + vector.Z * matrix.M21 + 1 * matrix.M31,
            vector.X * matrix.M02 + vector.Y * matrix.M12 + vector.Z * matrix.M22 + 1 * matrix.M32
        )
    End Operator

    Public Shared Operator *(vector As Vector3, matrix As Matrix) As Vector3
        Return New Vector3(
            vector.X * matrix.M00 + vector.Y * matrix.M10 + vector.Z * matrix.M20 + 1 * matrix.M30,
            vector.X * matrix.M01 + vector.Y * matrix.M11 + vector.Z * matrix.M21 + 1 * matrix.M31,
            vector.X * matrix.M02 + vector.Y * matrix.M12 + vector.Z * matrix.M22 + 1 * matrix.M32
        )
    End Operator

    Public Shared Operator *(mat1 As Matrix, mat2 As Matrix) As Matrix
        Dim out As Matrix

        out.M00 = mat1.M00 * mat2.M00 + mat1.M01 * mat2.M10 + mat1.M02 * mat2.M20 + mat1.M03 * mat2.M30
        out.M10 = mat1.M10 * mat2.M00 + mat1.M11 * mat2.M10 + mat1.M12 * mat2.M20 + mat1.M13 * mat2.M30
        out.M20 = mat1.M20 * mat2.M00 + mat1.M21 * mat2.M10 + mat1.M22 * mat2.M20 + mat1.M23 * mat2.M30
        out.M30 = mat1.M30 * mat2.M00 + mat1.M31 * mat2.M10 + mat1.M32 * mat2.M20 + mat1.M33 * mat2.M30
        out.M01 = mat1.M00 * mat2.M01 + mat1.M01 * mat2.M11 + mat1.M02 * mat2.M21 + mat1.M03 * mat2.M31
        out.M11 = mat1.M10 * mat2.M01 + mat1.M11 * mat2.M11 + mat1.M12 * mat2.M21 + mat1.M13 * mat2.M31
        out.M21 = mat1.M20 * mat2.M01 + mat1.M21 * mat2.M11 + mat1.M22 * mat2.M21 + mat1.M23 * mat2.M31
        out.M31 = mat1.M30 * mat2.M01 + mat1.M31 * mat2.M11 + mat1.M32 * mat2.M21 + mat1.M33 * mat2.M31
        out.M02 = mat1.M00 * mat2.M02 + mat1.M01 * mat2.M12 + mat1.M02 * mat2.M22 + mat1.M03 * mat2.M32
        out.M12 = mat1.M10 * mat2.M02 + mat1.M11 * mat2.M12 + mat1.M12 * mat2.M22 + mat1.M13 * mat2.M32
        out.M22 = mat1.M20 * mat2.M02 + mat1.M21 * mat2.M12 + mat1.M22 * mat2.M22 + mat1.M23 * mat2.M32
        out.M32 = mat1.M30 * mat2.M02 + mat1.M31 * mat2.M12 + mat1.M32 * mat2.M22 + mat1.M33 * mat2.M32
        out.M03 = mat1.M00 * mat2.M03 + mat1.M01 * mat2.M13 + mat1.M02 * mat2.M23 + mat1.M03 * mat2.M33
        out.M13 = mat1.M10 * mat2.M03 + mat1.M11 * mat2.M13 + mat1.M12 * mat2.M23 + mat1.M13 * mat2.M33
        out.M23 = mat1.M20 * mat2.M03 + mat1.M21 * mat2.M13 + mat1.M22 * mat2.M23 + mat1.M23 * mat2.M33
        out.M33 = mat1.M30 * mat2.M03 + mat1.M31 * mat2.M13 + mat1.M32 * mat2.M23 + mat1.M33 * mat2.M33

        Return out
    End Operator
End Structure
