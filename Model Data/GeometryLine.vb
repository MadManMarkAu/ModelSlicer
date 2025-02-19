Public Structure GeometryLine
    Public ReadOnly Color As Color

    Public ReadOnly V1 As Vector3
    Public ReadOnly V2 As Vector3

    Public ReadOnly V1Normal As Vector3
    Public ReadOnly V2Normal As Vector3
    Public ReadOnly LineNormal As Vector3

    Public ReadOnly Min As Vector3
    Public ReadOnly Max As Vector3

    Public Sub New(colorValue As Color, v1Value As Vector3, v2Value As Vector3)
        Color = colorValue
        V1 = v1Value
        V2 = v2Value

        Min = New Vector3(Math.Min(V1.X, V2.X), Math.Min(V1.Y, V2.Y), Math.Min(V1.Z, V2.Z))
        Max = New Vector3(Math.Max(V1.X, V2.X), Math.Max(V1.Y, V2.Y), Math.Max(V1.Z, V2.Z))
    End Sub

    Public Sub New(colorValue As Color, v1Value As Vector3, v2Value As Vector3, v1NormalValue As Vector3, v2NormalValue As Vector3, lineNormalValue As Vector3)
        Color = colorValue

        V1 = v1Value
        V2 = v2Value

        V1Normal = v1NormalValue
        V2Normal = v2NormalValue
        LineNormal = lineNormalValue

        Min = New Vector3(Math.Min(V1.X, V2.X), Math.Min(V1.Y, V2.Y), Math.Min(V1.Z, V2.Z))
        Max = New Vector3(Math.Max(V1.X, V2.X), Math.Max(V1.Y, V2.Y), Math.Max(V1.Z, V2.Z))
    End Sub

    Public Shared Operator =(left As GeometryLine, right As GeometryLine) As Boolean
        Return left.V1 = right.V1 AndAlso left.V2 = right.V2 AndAlso left.Color = right.Color
    End Operator

    Public Shared Operator <>(left As GeometryLine, right As GeometryLine) As Boolean
        Return left.V1 <> right.V1 OrElse left.V2 <> right.V2 OrElse left.Color <> right.Color
    End Operator
End Structure
