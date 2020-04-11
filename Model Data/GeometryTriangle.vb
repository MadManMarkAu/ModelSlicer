''' <summary>
''' A structure to hold a single 3D triangle, devined by three vertices
''' </summary>
Public Structure GeometryTriangle
    ''' <summary>
    ''' The first vertex of this triangle
    ''' </summary>
    Public V1 As Vector3

    ''' <summary>
    ''' The second vertex of this triangle
    ''' </summary>
    Public V2 As Vector3

    ''' <summary>
    ''' The third vertex of this triangle
    ''' </summary>
    Public V3 As Vector3

    Public Sub New(vV1 As Vector3, vV2 As Vector3, vV3 As Vector3)
        V1 = vV1
        V2 = vV2
        V3 = vV3
    End Sub

    ''' <summary>
    ''' Gets the minimum (X,Y,Z) coordinates of the bounding box of this triangle
    ''' </summary>
    Public Function GetMin() As Vector3
        Return New Vector3(
                Math.Min(V1.X, Math.Min(V2.X, V3.X)),
                Math.Min(V1.Y, Math.Min(V2.Y, V3.Y)),
                Math.Min(V1.Z, Math.Min(V2.Z, V3.Z))
            )
    End Function

    ''' <summary>
    ''' Gets the maximum (X,Y,Z) coordinates of the bounding box of this triangle
    ''' </summary>
    Public Function GetMax() As Vector3
        Return New Vector3(
                Math.Max(V1.X, Math.Max(V2.X, V3.X)),
                Math.Max(V1.Y, Math.Max(V2.Y, V3.Y)),
                Math.Max(V1.Z, Math.Max(V2.Z, V3.Z))
            )
    End Function
End Structure
