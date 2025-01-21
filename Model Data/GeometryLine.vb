Public Structure GeometryLine
    Public ReadOnly Color As Color

    Public ReadOnly V1 As Vector3
    Public ReadOnly V2 As Vector3

    Public ReadOnly V1Normal As Vector3
    Public ReadOnly V2Normal As Vector3
    Public ReadOnly LineNormal As Vector3

    Public ReadOnly Min As Vector3
    Public ReadOnly Max As Vector3

    Public Sub New(cColor As Color, vV1 As Vector3, vV2 As Vector3)
        Color = cColor
        V1 = vV1
        V2 = vV2

        Min = New Vector3(Math.Min(V1.X, V2.X), Math.Min(V1.Y, V2.Y), Math.Min(V1.Z, V2.Z))
        Max = New Vector3(Math.Max(V1.X, V2.X), Math.Max(V1.Y, V2.Y), Math.Max(V1.Z, V2.Z))
    End Sub

    Public Sub New(cColor As Color, vV1 As Vector3, vV2 As Vector3, vV1Normal As Vector3, vV2Normal As Vector3, vLineNormal As Vector3)
        Color = cColor

        V1 = vV1
        V2 = vV2

        V1Normal = vV1Normal
        V2Normal = vV2Normal
        LineNormal = vLineNormal

        Min = New Vector3(Math.Min(V1.X, V2.X), Math.Min(V1.Y, V2.Y), Math.Min(V1.Z, V2.Z))
        Max = New Vector3(Math.Max(V1.X, V2.X), Math.Max(V1.Y, V2.Y), Math.Max(V1.Z, V2.Z))
    End Sub

    Public Shared Operator =(glLeft As GeometryLine, glRight As GeometryLine) As Boolean
        Return glLeft.V1 = glRight.V1 AndAlso glLeft.V2 = glRight.V2 AndAlso glLeft.Color = glRight.Color
    End Operator

    Public Shared Operator <>(glLeft As GeometryLine, glRight As GeometryLine) As Boolean
        Return glLeft.V1 <> glRight.V1 OrElse glLeft.V2 <> glRight.V2 OrElse glLeft.Color <> glRight.Color
    End Operator
End Structure
