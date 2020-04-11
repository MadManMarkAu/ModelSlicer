''' <summary>
''' Module containing the functionality for slicing geometry
''' </summary>
Public Class Slicer
    ' Private New() procedure to prevent instantiating this class (static class)
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Slices the passed gemoetry along the XZ axis, and a given height.
    ''' </summary>
    ''' <param name="gGeometry">The <see cref="Geometry"/> object containing the geometry to slice.</param>
    ''' <param name="sngYSlice">The Y position of the XZ plane where slicing will occur.</param>
    ''' <returns>A <see cref="List(Of Linesegment)"/> containing the lines where each piece of geometry intersects the slice plane.</returns>
    Public Shared Function SliceModel(gGeometry As Geometry, sngYSlice As Single) As List(Of LineSegment)
        Dim lstLineSegments As New List(Of LineSegment) ' Output variable
        Dim tTriangle As GeometryTriangle ' Iteration variable
        Dim vV1 As Vector3 ' Storage for points 1, 2, and 3 that make up a triangle
        Dim vV2 As Vector3 ' We use separate point structures to allow us to sort the points by height
        Dim vV3 As Vector3 ' "
        Dim vTemp As Vector3 ' Temporary point for doing in-place swap sort
        Dim sngHeight As Single ' Stores the height of the current triangle - used for interpolation
        Dim vOut1 As Vector2 ' Beginning point of line
        Dim vOut2 As Vector2 ' Ending point of line

        ' Iterate through each triangle in geometry
        For Each tTriangle In gGeometry.Triangles
            ' Grab a copy of each vertex in triangle
            vV1 = tTriangle.V1
            vV2 = tTriangle.V2
            vV3 = tTriangle.V3

            ' If the triangle doesn't intersect plane (triangle is entirely above plane), then ignore this triangle
            If vV1.Y > sngYSlice AndAlso vV2.Y > sngYSlice AndAlso vV3.Y > sngYSlice Then
                Continue For
            End If

            ' If the triangle doesn't intersect plane (triangle is entirely below plane), then ignore this triangle
            If vV1.Y < sngYSlice AndAlso vV2.Y < sngYSlice AndAlso vV3.Y < sngYSlice Then
                Continue For
            End If

            ' Sort the triangle vertices by Y position, ascending

            If vV1.Y > vV2.Y Then
                vTemp = vV1
                vV1 = vV2
                vV2 = vTemp
            End If

            If vV1.Y > vV3.Y Then
                vTemp = vV1
                vV1 = vV3
                vV3 = vTemp
            End If

            If vV2.Y > vV3.Y Then
                vTemp = vV2
                vV2 = vV3
                vV3 = vTemp
            End If

            ' Get the total height of this triangle, in the Y direction
            sngHeight = vV3.Y - vV1.Y

            If sngHeight < 0.0000001 Then
                ' This triangle pretty much sits directly on the plane, in a coplanar fashion
                ' Output all three edges of triangle
                lstLineSegments.Add(New LineSegment(New Vector2(vV1.X, vV1.Z), New Vector2(vV2.X, vV2.Z)))
                lstLineSegments.Add(New LineSegment(New Vector2(vV2.X, vV2.Z), New Vector2(vV3.X, vV3.Z)))
                lstLineSegments.Add(New LineSegment(New Vector2(vV3.X, vV3.Z), New Vector2(vV1.X, vV1.Z)))
                Continue For
            End If

            ' There are two different ways the triangle can intersect the plane
            ' 1) Two points are below the plane, and one point is above
            ' 2) One point is below the plane, and two points are above
            ' Because we sorted the triangle vertices by Y position, we can look at the middle vertex (V2) to determine which type of intersection this is

            If vV2.Y < sngYSlice Then
                ' V2 is below the slice plane, so this is the "two points below, one point above" style of intersection
                ' We know V1 and V2 are both below the plane, and V2 is above (or on) the plane
                ' To find the line that describes the intersections on the plane, we need to figure out
                ' 1) What point along (V1 - V3) intersects the plane
                ' 2) What point along (V2 - V3) intersects the plane
                ' Finding these two points will tell us the intersecting lines start and end points

                ' (V1 - V3) intersection
                vOut1 = Interpolate(vV1, vV3, sngYSlice)

                ' (V2 - V3) intersection
                vOut2 = Interpolate(vV2, vV3, sngYSlice)

                ' Add the resulting line to the output
                lstLineSegments.Add(New LineSegment(vOut1, vOut2))
            Else
                ' V2 is above (or on) the slice plane, so this is the "one point below, two points above" style of intersection
                ' We know V1  is below the plane, and V2 and V3 are above (or on) the plane
                ' To find the line that describes the intersections on the plane, we need to figure out
                ' 1) What point along (V1 - V3) intersects the plane
                ' 2) What point along (V1 - V2) intersects the plane
                ' Finding these two points will tell us the intersecting lines start and end points

                ' (V1 - V3) intersection
                vOut1 = Interpolate(vV1, vV3, sngYSlice)

                ' (V1 - V2) intersection
                vOut2 = Interpolate(vV1, vV2, sngYSlice)

                ' Add the resulting line to the output
                lstLineSegments.Add(New LineSegment(vOut1, vOut2))
            End If
        Next

        ' Return the list of generated line segments
        Return lstLineSegments
    End Function

    ' This procedure finds the intersecting point between a vertex below the plane, and a vertex above the plane
    ' It does this by using the Y height of both vertexes, and the Y height of the plane, to create a ratio between (V1.Y - PlaneY) and (V2.Y - PlaneY)
    ' This ratio is then used to linearly interpolate the X and Z coordinates from the two points
    ' The result is, a point is returned that sits exactly on the plane, where a line between V1 and V2 would intersect
    ' Note that to make things easier when calculating the ratio, the Y coordinates of both the points and the plane are brought down so the lowest point sits at 0
    '   All Y coordinates are brought down the same amount. This is fine, as the Y component will not be part of the output.
    Private Shared Function Interpolate(vV1 As Vector3, vV2 As Vector3, sngYPos As Single) As Vector2
        Dim sngX1 As Single = vV1.X
        Dim sngY1 As Single = vV1.Y - vV1.Y
        Dim sngZ1 As Single = vV1.Z
        Dim sngX2 As Single = vV2.X
        Dim sngY2 As Single = vV2.Y - vV1.Y
        Dim sngZ2 As Single = vV2.Z
        Dim sngRatio As Single = (sngYPos - vV1.Y) / sngY2

        ' Standard linear interpotation function
        Return New Vector2(sngX2 * sngRatio + sngX1 * (1 - sngRatio), sngZ2 * sngRatio + sngZ1 * (1 - sngRatio))
    End Function
End Class
