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

    Public Sub New(vV1 As Vector3, vV2 As Vector3, vV3 As Vector3, vV1Normal As Vector3, vV2Normal As Vector3, vV3Normal As Vector3)
        V1 = vV1
        V2 = vV2
        V3 = vV3

        V1Normal = vV1Normal
        V2Normal = vV2Normal
        V3Normal = vV3Normal
        SurfaceNormal = Vector3.Normalize((vV1Normal + vV2Normal + vV3Normal) / 3)

        Min = New Vector3(Math.Min(V1.X, Math.Min(V2.X, V3.X)), Math.Min(V1.Y, Math.Min(V2.Y, V3.Y)), Math.Min(V1.Z, Math.Min(V2.Z, V3.Z)))
        Max = New Vector3(Math.Max(V1.X, Math.Max(V2.X, V3.X)), Math.Max(V1.Y, Math.Max(V2.Y, V3.Y)), Math.Max(V1.Z, Math.Max(V2.Z, V3.Z)))
    End Sub

    Public Sub New(cColor As Color, vV1 As Vector3, vV2 As Vector3, vV3 As Vector3, vV1Normal As Vector3, vV2Normal As Vector3, vV3Normal As Vector3, vSurfaceNormal As Vector3)
        Color = cColor
        V1 = vV1
        V2 = vV2
        V3 = vV3

        V1Normal = vV1Normal
        V2Normal = vV2Normal
        V3Normal = vV3Normal
        SurfaceNormal = vSurfaceNormal

        Min = New Vector3(Math.Min(V1.X, Math.Min(V2.X, V3.X)), Math.Min(V1.Y, Math.Min(V2.Y, V3.Y)), Math.Min(V1.Z, Math.Min(V2.Z, V3.Z)))
        Max = New Vector3(Math.Max(V1.X, Math.Max(V2.X, V3.X)), Math.Max(V1.Y, Math.Max(V2.Y, V3.Y)), Math.Max(V1.Z, Math.Max(V2.Z, V3.Z)))
    End Sub
End Structure
