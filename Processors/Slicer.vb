Public Class Slicer
    Private Sub New()
    End Sub

    Public Shared Function Slice(group As GeometryTriangleGroup, thickness As Single, topColor As Color, bottomColor As Color, contentColor As Color) As List(Of Layer)
        Dim output As New List(Of Layer)
        Dim yStart As Single
        Dim startPlanePoint As Vector3
        Dim endPlanePoint As Vector3
        Dim planeNormal As Vector3
        Dim topOutline As GeometryLineGroup
        Dim bottomOutline As GeometryLineGroup
        Dim contents As GeometryTriangleGroup

        yStart = group.Bounds.Minimum.Y
        planeNormal = New Vector3(0, -1, 0)

        While yStart < group.Bounds.Maximum.Y
            startPlanePoint = New Vector3(0, yStart + thickness, 0)
            endPlanePoint = New Vector3(0, yStart, 0)

            topOutline = OutlineModelPlane(group, endPlanePoint, planeNormal, topColor)
            bottomOutline = OutlineModelPlane(group, startPlanePoint, planeNormal, bottomColor)
            contents = ExtractBetweenPlanes(group, startPlanePoint, endPlanePoint, planeNormal).Between.CloneWithColor(contentColor)

            output.Add(New Layer(topOutline, bottomOutline, contents))

            yStart += thickness
        End While

        Return output
    End Function

    Public Shared Function Slice(group As GeometryTriangleGroup, startPlanePoint As Vector3, endPlanePoint As Vector3, planeNormal As Vector3, topColor As Color, bottomColor As Color, contentColor As Color) As Layer
        Dim topOutline As GeometryLineGroup
        Dim bottomOutline As GeometryLineGroup
        Dim contents As GeometryTriangleGroup

        topOutline = OutlineModelPlane(group, endPlanePoint, planeNormal, topColor)
        bottomOutline = OutlineModelPlane(group, startPlanePoint, planeNormal, bottomColor)
        contents = ExtractBetweenPlanes(group, startPlanePoint, endPlanePoint, planeNormal).Between.CloneWithColor(contentColor)

        Return New Layer(topOutline, bottomOutline, contents)
    End Function


    Public Shared Function OutlineModelPlane(geometry As Geometry, planePoint As Vector3, planeNormal As Vector3, color As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup

        For Each segment As GeometryTriangleGroup In geometry.Groups
            output.Lines.AddRange(OutlineModelPlane(segment, planePoint, planeNormal, color).Lines)
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Shared Function ExtractBetweenPlanes(geometry As Geometry, startPlanePoint As Vector3, endPlanePoint As Vector3, planeNormal As Vector3) As Geometry
        Return Bifurcate(Bifurcate(geometry, startPlanePoint, planeNormal).Above, endPlanePoint, planeNormal).Below
    End Function

    Public Shared Function Bifurcate(geometry As Geometry, planePoint As Vector3, planeNormal As Vector3) As (Above As Geometry, Below As Geometry)
        Dim output As (Above As Geometry, Below As Geometry)
        Dim bifurcateResult As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)

        output = (New Geometry, New Geometry)

        For Each group As GeometryTriangleGroup In geometry.Groups
            bifurcateResult = Bifurcate(group, planePoint, planeNormal)

            If bifurcateResult.Above.Triangles.Count > 0 Then
                output.Above.Groups.Add(bifurcateResult.Above)
            End If

            If bifurcateResult.Below.Triangles.Count > 0 Then
                output.Below.Groups.Add(bifurcateResult.Below)
            End If
        Next

        output.Above.UpdateBounds()
        output.Below.UpdateBounds()

        Return output
    End Function

    Public Shared Function OutlineModelPlane(group As GeometryTriangleGroup, planePoint As Vector3, planeNormal As Vector3, color As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup
        Dim result1 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim result2 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim result3 As (Valid As Boolean, Point As Vector3, Normal As Vector3)

        output.Name = group.Name

        For Each tri As GeometryTriangle In group.Triangles
            result1 = IntersectLinePlane(planePoint, planeNormal, tri.V1, tri.V2, tri.V1Normal, tri.V2Normal)
            result2 = IntersectLinePlane(planePoint, planeNormal, tri.V2, tri.V3, tri.V2Normal, tri.V3Normal)
            result3 = IntersectLinePlane(planePoint, planeNormal, tri.V3, tri.V1, tri.V3Normal, tri.V1Normal)

            If result1.Valid AndAlso result2.Valid Then
                output.Lines.Add(New GeometryLine(color, result1.Point, result2.Point, result1.Normal, result2.Normal, tri.SurfaceNormal))
            End If

            If result2.Valid AndAlso result3.Valid Then
                output.Lines.Add(New GeometryLine(color, result2.Point, result3.Point, result2.Normal, result3.Normal, tri.SurfaceNormal))
            End If

            If result3.Valid AndAlso result1.Valid Then
                output.Lines.Add(New GeometryLine(color, result3.Point, result1.Point, result3.Normal, result1.Normal, tri.SurfaceNormal))
            End If
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Shared Function ExtractBetweenPlanes(group As GeometryTriangleGroup, startPlanePoint As Vector3, endPlanePoint As Vector3, planeNormal As Vector3) As (Above As GeometryTriangleGroup, Between As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim tSlice1 As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim tSlice2 As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)

        ' For result of Bifurcate, .Below == above the plane, because plane normal points down (-y)
        tSlice1 = Bifurcate(group, startPlanePoint, planeNormal)
        tSlice2 = Bifurcate(tSlice1.Above, endPlanePoint, planeNormal)

        Return (tSlice1.Below, tSlice2.Below, tSlice2.Above)
    End Function

    Public Shared Function Bifurcate(group As GeometryTriangleGroup, planePoint As Vector3, planeNormal As Vector3) As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim output As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup) = (New GeometryTriangleGroup, New GeometryTriangleGroup)
        Dim result As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle))

        output.Above.Name = group.Name
        output.Below.Name = group.Name

        For Each tri As GeometryTriangle In group.Triangles
            result = Bifurcate(tri, planePoint, planeNormal)

            output.Above.Triangles.AddRange(result.Above)
            output.Below.Triangles.AddRange(result.Below)
        Next

        output.Above.UpdateBounds()
        output.Below.UpdateBounds()

        Return output
    End Function

    ' This function should preserve winding direction when creating new triangles (untested)
    Public Shared Function Bifurcate(tri As GeometryTriangle, planePoint As Vector3, planeNormal As Vector3) As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle))
        Dim output As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle)) = (New List(Of GeometryTriangle), New List(Of GeometryTriangle))
        Dim point1 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim point2 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim point3 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim pointTemp As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim aboveCount As Integer
        Dim result1 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim result2 As (Valid As Boolean, Point As Vector3, Normal As Vector3)

        point1 = (tri.V1, IsPointAbovePlane(planePoint, planeNormal, tri.V1), tri.V1Normal)
        point2 = (tri.V2, IsPointAbovePlane(planePoint, planeNormal, tri.V2), tri.V2Normal)
        point3 = (tri.V3, IsPointAbovePlane(planePoint, planeNormal, tri.V3), tri.V3Normal)

        ' Count how many points are above the plane
        If point1.IsAbove Then
            aboveCount += 1
        End If
        If point2.IsAbove Then
            aboveCount += 1
        End If
        If point3.IsAbove Then
            aboveCount += 1
        End If

        Select Case aboveCount
            Case 0
                output.Below.Add(tri)

            Case 1
                ' Rotate the triangle until the first point is above
                While point1.IsAbove = False
                    pointTemp = point1
                    point1 = point3
                    point3 = point2
                    point2 = pointTemp
                End While

                result1 = IntersectLinePlane(planePoint, planeNormal, point1.Point, point2.Point, point1.Normal, point2.Normal)
                result2 = IntersectLinePlane(planePoint, planeNormal, point1.Point, point3.Point, point1.Normal, point3.Normal)

                output.Above.Add(New GeometryTriangle(tri.Color, point1.Point, result1.Point, result2.Point, point1.Normal, result1.Normal, result2.Normal, tri.SurfaceNormal))
                output.Below.Add(New GeometryTriangle(tri.Color, result2.Point, result1.Point, point2.Point, result2.Normal, result1.Normal, point2.Normal, tri.SurfaceNormal))
                output.Below.Add(New GeometryTriangle(tri.Color, result2.Point, point2.Point, point3.Point, result2.Normal, point2.Normal, point3.Normal, tri.SurfaceNormal))

            Case 2
                ' Rotate the triangle until the first two points are above
                While point1.IsAbove = False OrElse point2.IsAbove = False
                    pointTemp = point1
                    point1 = point3
                    point3 = point2
                    point2 = pointTemp
                End While

                result1 = IntersectLinePlane(planePoint, planeNormal, point1.Point, point3.Point, point1.Normal, point3.Normal)
                result2 = IntersectLinePlane(planePoint, planeNormal, point2.Point, point3.Point, point2.Normal, point3.Normal)

                output.Below.Add(New GeometryTriangle(tri.Color, result1.Point, result2.Point, point3.Point, result1.Normal, result2.Normal, point3.Normal, tri.SurfaceNormal))
                output.Above.Add(New GeometryTriangle(tri.Color, point1.Point, point2.Point, result2.Point, point1.Normal, point2.Normal, result2.Normal, tri.SurfaceNormal))
                output.Above.Add(New GeometryTriangle(tri.Color, point1.Point, result2.Point, result1.Point, point1.Normal, result2.Normal, result1.Normal, tri.SurfaceNormal))

            Case 3
                output.Above.Add(tri)

        End Select

        Return output
    End Function

    Private Shared Function IntersectLinePlane(planePoint As Vector3, planeNormal As Vector3, linePoint1 As Vector3, linePoint2 As Vector3, lineNormal1 As Vector3, lineNormal2 As Vector3) As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim lineDir As Vector3
        Dim lineDirNormalized As Vector3
        Dim distanceFromLineStart As Single
        Dim ratio As Single

        lineDir = linePoint2 - linePoint1

        If lineDir.LengthSquared() < 0.000001 Then
            Return (False, Nothing, Nothing) ' Zero length is invalid
        End If

        lineDirNormalized = Vector3.Normalize(lineDir)

        If Vector3.DotProduct(planeNormal, lineDirNormalized) = 0 Then
            Return (False, Nothing, Nothing) ' Line is parallel to plane
        End If

        distanceFromLineStart = (Vector3.DotProduct(planeNormal, planePoint) - Vector3.DotProduct(planeNormal, linePoint1)) / Vector3.DotProduct(planeNormal, lineDirNormalized)

        If distanceFromLineStart < -0.000001 OrElse distanceFromLineStart > lineDir.Length() + 0.000001 Then
            Return (False, Nothing, Nothing) ' Line intersects plane before the start, or after the end
        End If

        ratio = distanceFromLineStart / lineDir.Length()

        Return (True, linePoint1 + lineDirNormalized * distanceFromLineStart, Vector3.Normalize(lineNormal1 * (1 - ratio) + lineNormal2 * ratio))
    End Function

    Private Shared Function IsPointAbovePlane(planePoint As Vector3, planeNormal As Vector3, point As Vector3) As Boolean
        Return Vector3.DotProduct(planeNormal, point - planePoint) >= 0
    End Function
End Class
