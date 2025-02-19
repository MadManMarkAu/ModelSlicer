Public Structure GeometryTriangle
    Public ReadOnly Color As Color

    Public ReadOnly V1 As Vector3
    Public ReadOnly V2 As Vector3
    Public ReadOnly V3 As Vector3

    Public ReadOnly V1Normal As Vector3
    Public ReadOnly V2Normal As Vector3
    Public ReadOnly V3Normal As Vector3
    Public ReadOnly SurfaceNormal As Vector3

    Public ReadOnly Min As Vector3
    Public ReadOnly Max As Vector3

    Public Sub New(v1Value As Vector3, v2Value As Vector3, v3Value As Vector3, v1NormalValue As Vector3, v2NormalValue As Vector3, v3NormalValue As Vector3)
        V1 = v1Value
        V2 = v2Value
        V3 = v3Value

        V1Normal = v1NormalValue
        V2Normal = v2NormalValue
        V3Normal = v3NormalValue
        SurfaceNormal = Vector3.Normalize((v1NormalValue + v2NormalValue + v3NormalValue) / 3)

        Min = New Vector3(Math.Min(V1.X, Math.Min(V2.X, V3.X)), Math.Min(V1.Y, Math.Min(V2.Y, V3.Y)), Math.Min(V1.Z, Math.Min(V2.Z, V3.Z)))
        Max = New Vector3(Math.Max(V1.X, Math.Max(V2.X, V3.X)), Math.Max(V1.Y, Math.Max(V2.Y, V3.Y)), Math.Max(V1.Z, Math.Max(V2.Z, V3.Z)))
    End Sub

    Public Sub New(colorValue As Color, v1Value As Vector3, v2Value As Vector3, v3Value As Vector3, v1NormalValue As Vector3, v2NormalValue As Vector3, v3NormalValue As Vector3, surfaceNormalValue As Vector3)
        Color = colorValue
        V1 = v1Value
        V2 = v2Value
        V3 = v3Value

        V1Normal = v1NormalValue
        V2Normal = v2NormalValue
        V3Normal = v3NormalValue
        SurfaceNormal = surfaceNormalValue

        Min = New Vector3(Math.Min(V1.X, Math.Min(V2.X, V3.X)), Math.Min(V1.Y, Math.Min(V2.Y, V3.Y)), Math.Min(V1.Z, Math.Min(V2.Z, V3.Z)))
        Max = New Vector3(Math.Max(V1.X, Math.Max(V2.X, V3.X)), Math.Max(V1.Y, Math.Max(V2.Y, V3.Y)), Math.Max(V1.Z, Math.Max(V2.Z, V3.Z)))
    End Sub
End Structure
