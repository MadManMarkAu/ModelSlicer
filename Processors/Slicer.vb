Public Class Slicer
    Private Sub New()
    End Sub

    Public Shared Function Slice(gtgGroup As GeometryTriangleGroup, sngThickness As Single, cTopColor As Color, cBottomColor As Color, cContentColor As Color) As List(Of Layer)
        Dim lstOutput As New List(Of Layer)
        Dim sngYStart As Single
        Dim vStartPlanePoint As Vector3
        Dim vEndPlanePoint As Vector3
        Dim vPlaneNormal As Vector3
        Dim glgTopOutline As GeometryLineGroup
        Dim glgBottomOutline As GeometryLineGroup
        Dim gtgContents As GeometryTriangleGroup

        sngYStart = gtgGroup.Bounds.Minimum.Y
        vPlaneNormal = New Vector3(0, -1, 0)

        While sngYStart < gtgGroup.Bounds.Maximum.Y
            vStartPlanePoint = New Vector3(0, sngYStart + sngThickness, 0)
            vEndPlanePoint = New Vector3(0, sngYStart, 0)

            glgTopOutline = OutlineModelPlane(gtgGroup, vEndPlanePoint, vPlaneNormal, cTopColor)
            glgBottomOutline = OutlineModelPlane(gtgGroup, vStartPlanePoint, vPlaneNormal, cBottomColor)
            gtgContents = ExtractBetweenPlanes(gtgGroup, vStartPlanePoint, vEndPlanePoint, vPlaneNormal).CloneWithColor(cContentColor)

            lstOutput.Add(New Layer(glgTopOutline, glgBottomOutline, gtgContents))

            sngYStart += sngThickness
        End While

        Return lstOutput
    End Function

    Public Shared Function Slice(gtgGroup As GeometryTriangleGroup, vStartPlanePoint As Vector3, vEndPlanePoint As Vector3, vPlaneNormal As Vector3, cTopColor As Color, cBottomColor As Color, cContentColor As Color) As Layer
        Dim glgTopOutline As GeometryLineGroup
        Dim glgBottomOutline As GeometryLineGroup
        Dim gtgContents As GeometryTriangleGroup

        glgTopOutline = OutlineModelPlane(gtgGroup, vEndPlanePoint, vPlaneNormal, cTopColor)
        glgBottomOutline = OutlineModelPlane(gtgGroup, vStartPlanePoint, vPlaneNormal, cBottomColor)
        gtgContents = ExtractBetweenPlanes(gtgGroup, vStartPlanePoint, vEndPlanePoint, vPlaneNormal).CloneWithColor(cContentColor)

        Return New Layer(glgTopOutline, glgBottomOutline, gtgContents)
    End Function


    Public Shared Function OutlineModelPlane(gGeometry As Geometry, vPlanePoint As Vector3, vPlaneNormal As Vector3, cColor As Color) As GeometryLineGroup
        Dim glgOutput As New GeometryLineGroup
        Dim gsSegment As GeometryTriangleGroup

        For Each gsSegment In gGeometry.Groups
            glgOutput.Lines.AddRange(OutlineModelPlane(gsSegment, vPlanePoint, vPlaneNormal, cColor).Lines)
        Next

        glgOutput.UpdateBounds()

        Return glgOutput
    End Function

    Public Shared Function ExtractBetweenPlanes(gGeometry As Geometry, vStartPlanePoint As Vector3, vEndPlanePoint As Vector3, vPlaneNormal As Vector3) As Geometry
        Return Bifurcate(Bifurcate(gGeometry, vStartPlanePoint, vPlaneNormal).Above, vEndPlanePoint, vPlaneNormal).Below
    End Function

    Public Shared Function Bifurcate(gGeometry As Geometry, vPlanePoint As Vector3, vPlaneNormal As Vector3) As (Above As Geometry, Below As Geometry)
        Dim tOutput As (Above As Geometry, Below As Geometry)
        Dim tBifurcateResult As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim gtgGroup As GeometryTriangleGroup

        tOutput = (New Geometry, New Geometry)

        For Each gtgGroup In gGeometry.Groups
            tBifurcateResult = Bifurcate(gtgGroup, vPlanePoint, vPlaneNormal)

            If tBifurcateResult.Above.Triangles.Count > 0 Then
                tOutput.Above.Groups.Add(tBifurcateResult.Above)
            End If

            If tBifurcateResult.Below.Triangles.Count > 0 Then
                tOutput.Below.Groups.Add(tBifurcateResult.Below)
            End If
        Next

        tOutput.Above.UpdateBounds()
        tOutput.Below.UpdateBounds()

        Return tOutput
    End Function

    Public Shared Function OutlineModelPlane(gtgGroup As GeometryTriangleGroup, vPlanePoint As Vector3, vPlaneNormal As Vector3, cColor As Color) As GeometryLineGroup
        Dim glgOutput As New GeometryLineGroup
        Dim gtTriangle As GeometryTriangle
        Dim tResult1 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim tResult2 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim tResult3 As (Valid As Boolean, Point As Vector3, Normal As Vector3)

        glgOutput.Name = gtgGroup.Name

        For Each gtTriangle In gtgGroup.Triangles
            tResult1 = IntersectLinePlane(vPlanePoint, vPlaneNormal, gtTriangle.V1, gtTriangle.V2, gtTriangle.V1Normal, gtTriangle.V2Normal)
            tResult2 = IntersectLinePlane(vPlanePoint, vPlaneNormal, gtTriangle.V2, gtTriangle.V3, gtTriangle.V2Normal, gtTriangle.V3Normal)
            tResult3 = IntersectLinePlane(vPlanePoint, vPlaneNormal, gtTriangle.V3, gtTriangle.V1, gtTriangle.V3Normal, gtTriangle.V1Normal)

            If tResult1.Valid AndAlso tResult2.Valid Then
                glgOutput.Lines.Add(New GeometryLine(cColor, tResult1.Point, tResult2.Point, tResult1.Normal, tResult2.Normal, gtTriangle.SurfaceNormal))
            End If

            If tResult2.Valid AndAlso tResult3.Valid Then
                glgOutput.Lines.Add(New GeometryLine(cColor, tResult2.Point, tResult3.Point, tResult2.Normal, tResult3.Normal, gtTriangle.SurfaceNormal))
            End If

            If tResult3.Valid AndAlso tResult1.Valid Then
                glgOutput.Lines.Add(New GeometryLine(cColor, tResult3.Point, tResult1.Point, tResult3.Normal, tResult1.Normal, gtTriangle.SurfaceNormal))
            End If
        Next

        glgOutput.UpdateBounds()

        Return glgOutput
    End Function

    Public Shared Function ExtractBetweenPlanes(gtgGroup As GeometryTriangleGroup, vStartPlanePoint As Vector3, vEndPlanePoint As Vector3, vPlaneNormal As Vector3) As GeometryTriangleGroup
        Return Bifurcate(Bifurcate(gtgGroup, vStartPlanePoint, vPlaneNormal).Above, vEndPlanePoint, vPlaneNormal).Below
    End Function

    Public Shared Function Bifurcate(gtgGroup As GeometryTriangleGroup, vPlanePoint As Vector3, vPlaneNormal As Vector3) As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup)
        Dim tOutput As (Above As GeometryTriangleGroup, Below As GeometryTriangleGroup) = (New GeometryTriangleGroup, New GeometryTriangleGroup)
        Dim gtTriangle As GeometryTriangle
        Dim tResult As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle))

        tOutput.Above.Name = gtgGroup.Name
        tOutput.Below.Name = gtgGroup.Name

        For Each gtTriangle In gtgGroup.Triangles
            tResult = Bifurcate(gtTriangle, vPlanePoint, vPlaneNormal)

            tOutput.Above.Triangles.AddRange(tResult.Above)
            tOutput.Below.Triangles.AddRange(tResult.Below)
        Next

        tOutput.Above.UpdateBounds()
        tOutput.Below.UpdateBounds()

        Return tOutput
    End Function

    ' This function should preserve winding direction when creating new triangles (untested)
    Public Shared Function Bifurcate(gtTriangle As GeometryTriangle, vPlanePoint As Vector3, vPlaneNormal As Vector3) As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle))
        Dim tOutput As (Above As List(Of GeometryTriangle), Below As List(Of GeometryTriangle)) = (New List(Of GeometryTriangle), New List(Of GeometryTriangle))
        Dim tPoint1 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim tPoint2 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim tPoint3 As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim tPointTemp As (Point As Vector3, IsAbove As Boolean, Normal As Vector3)
        Dim intAboveCount As Integer
        Dim tResult1 As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim tResult2 As (Valid As Boolean, Point As Vector3, Normal As Vector3)

        tPoint1 = (gtTriangle.V1, IsPointAbovePlane(vPlanePoint, vPlaneNormal, gtTriangle.V1), gtTriangle.V1Normal)
        tPoint2 = (gtTriangle.V2, IsPointAbovePlane(vPlanePoint, vPlaneNormal, gtTriangle.V2), gtTriangle.V2Normal)
        tPoint3 = (gtTriangle.V3, IsPointAbovePlane(vPlanePoint, vPlaneNormal, gtTriangle.V3), gtTriangle.V3Normal)

        ' Count how many points are above the plane
        If tPoint1.IsAbove Then
            intAboveCount += 1
        End If
        If tPoint2.IsAbove Then
            intAboveCount += 1
        End If
        If tPoint3.IsAbove Then
            intAboveCount += 1
        End If

        Select Case intAboveCount
            Case 0
                tOutput.Below.Add(gtTriangle)

            Case 1
                ' Rotate the triangle until the first point is above
                While tPoint1.IsAbove = False
                    tPointTemp = tPoint1
                    tPoint1 = tPoint3
                    tPoint3 = tPoint2
                    tPoint2 = tPointTemp
                End While

                tResult1 = IntersectLinePlane(vPlanePoint, vPlaneNormal, tPoint1.Point, tPoint2.Point, tPoint1.Normal, tPoint2.Normal)
                tResult2 = IntersectLinePlane(vPlanePoint, vPlaneNormal, tPoint1.Point, tPoint3.Point, tPoint1.Normal, tPoint3.Normal)

                tOutput.Above.Add(New GeometryTriangle(gtTriangle.Color, tPoint1.Point, tResult1.Point, tResult2.Point, tPoint1.Normal, tResult1.Normal, tResult2.Normal, gtTriangle.SurfaceNormal))
                tOutput.Below.Add(New GeometryTriangle(gtTriangle.Color, tResult2.Point, tResult1.Point, tPoint2.Point, tResult2.Normal, tResult1.Normal, tPoint2.Normal, gtTriangle.SurfaceNormal))
                tOutput.Below.Add(New GeometryTriangle(gtTriangle.Color, tResult2.Point, tPoint2.Point, tPoint3.Point, tResult2.Normal, tPoint2.Normal, tPoint3.Normal, gtTriangle.SurfaceNormal))

            Case 2
                ' Rotate the triangle until the first two points are above
                While tPoint1.IsAbove = False OrElse tPoint2.IsAbove = False
                    tPointTemp = tPoint1
                    tPoint1 = tPoint3
                    tPoint3 = tPoint2
                    tPoint2 = tPointTemp
                End While

                tResult1 = IntersectLinePlane(vPlanePoint, vPlaneNormal, tPoint1.Point, tPoint3.Point, tPoint1.Normal, tPoint3.Normal)
                tResult2 = IntersectLinePlane(vPlanePoint, vPlaneNormal, tPoint2.Point, tPoint3.Point, tPoint2.Normal, tPoint3.Normal)

                tOutput.Below.Add(New GeometryTriangle(gtTriangle.Color, tResult1.Point, tResult2.Point, tPoint3.Point, tResult1.Normal, tResult2.Normal, tPoint3.Normal, gtTriangle.SurfaceNormal))
                tOutput.Above.Add(New GeometryTriangle(gtTriangle.Color, tPoint1.Point, tPoint2.Point, tResult2.Point, tPoint1.Normal, tPoint2.Normal, tResult2.Normal, gtTriangle.SurfaceNormal))
                tOutput.Above.Add(New GeometryTriangle(gtTriangle.Color, tPoint1.Point, tResult2.Point, tResult1.Point, tPoint1.Normal, tResult2.Normal, tResult1.Normal, gtTriangle.SurfaceNormal))

            Case 3
                tOutput.Above.Add(gtTriangle)

        End Select

        Return tOutput
    End Function

    Private Shared Function IntersectLinePlane(vPlanePoint As Vector3, vPlaneNormal As Vector3, vLinePoint1 As Vector3, vLinePoint2 As Vector3, vLineNormal1 As Vector3, vLineNormal2 As Vector3) As (Valid As Boolean, Point As Vector3, Normal As Vector3)
        Dim vLineDir As Vector3
        Dim vLineDirNormalized As Vector3
        Dim sngDistanceFromLineStart As Single
        Dim sngRatio As Single

        vLineDir = vLinePoint2 - vLinePoint1
        vLineDirNormalized = Vector3.Normalize(vLineDir)

        If Vector3.DotProduct(vPlaneNormal, vLineDirNormalized) = 0 Then
            Return (False, Nothing, Nothing) ' Line is parallel to plane
        End If

        sngDistanceFromLineStart = (Vector3.DotProduct(vPlaneNormal, vPlanePoint) - Vector3.DotProduct(vPlaneNormal, vLinePoint1)) / Vector3.DotProduct(vPlaneNormal, vLineDirNormalized)

        If sngDistanceFromLineStart < -0.0000001 OrElse sngDistanceFromLineStart > vLineDir.Length() + 0.0000001 Then
            Return (False, Nothing, Nothing) ' Line intersects plane before the start, or after the end
        End If

        sngRatio = sngDistanceFromLineStart / vLineDir.Length()

        Return (True, vLinePoint1 + vLineDirNormalized * sngDistanceFromLineStart, Vector3.Normalize(vLineNormal1 * (1 - sngRatio) + vLineNormal2 * sngRatio))
    End Function

    Private Shared Function IsPointAbovePlane(vPlanePoint As Vector3, vPlaneNormal As Vector3, vPoint As Vector3) As Boolean
        Return Vector3.DotProduct(vPlaneNormal, vPoint - vPlanePoint) >= 0
    End Function
End Class
