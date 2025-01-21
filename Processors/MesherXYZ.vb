Public Class MesherXYZ
    Private m_stTree As SpaceTree3

    Public ReadOnly Points() As Vector3
    Public ReadOnly Triangles() As MeshTriangle
    Public ReadOnly Edges() As MeshEdge
    Public ReadOnly DisconnectedEdges() As MeshEdge

    Public Sub New(gtgTriangles As GeometryTriangleGroup)
        Dim lstVectors As New List(Of Vector3)
        Dim mtTriangle As MeshTriangle
        Dim meEdge As MeshEdge
        Dim lstTempEdges As New List(Of Tuple(Of Integer, Integer))
        Dim intV1Index As Integer
        Dim intV2Index As Integer
        Dim intV3Index As Integer
        Dim intV12EdgeIndex As Integer
        Dim intV23EdgeIndex As Integer
        Dim intV31EdgeIndex As Integer

        Dim lstTriangles As New List(Of MeshTriangle)
        Dim lstPoints As New List(Of Vector3)
        Dim lstEdges As New List(Of MeshEdge)
        Dim lstDisconnectedEdges As New List(Of MeshEdge)

        For Each gtTriangle As GeometryTriangle In gtgTriangles.Triangles
            lstVectors.Add(gtTriangle.V1)
            lstVectors.Add(gtTriangle.V2)
            lstVectors.Add(gtTriangle.V3)
        Next

        m_stTree = New SpaceTree3(lstVectors)

        For Each stpPoint As SpaceTree3.SpaceTreePoint In m_stTree.GetPoints()
            lstPoints.Add(stpPoint.ToVector3())
        Next

        For Each gtTriangle As GeometryTriangle In gtgTriangles.Triangles
            intV1Index = m_stTree.GetPoint(gtTriangle.V1).Index
            intV2Index = m_stTree.GetPoint(gtTriangle.V2).Index
            intV3Index = m_stTree.GetPoint(gtTriangle.V3).Index

            intV12EdgeIndex = GetOrAddEdge(lstTempEdges, intV1Index, intV2Index)
            intV23EdgeIndex = GetOrAddEdge(lstTempEdges, intV2Index, intV3Index)
            intV31EdgeIndex = GetOrAddEdge(lstTempEdges, intV3Index, intV1Index)

            lstTriangles.Add(New MeshTriangle(intV1Index, intV2Index, intV3Index, intV12EdgeIndex, intV23EdgeIndex, intV31EdgeIndex, gtTriangle.SurfaceNormal))
        Next

        Dim alstTriangles(lstTempEdges.Count - 1) As List(Of MeshTriangle)

        For Each mtTriangle In lstTriangles
            If alstTriangles(mtTriangle.V12EdgeIndex) Is Nothing Then
                alstTriangles(mtTriangle.V12EdgeIndex) = New List(Of MeshTriangle) From {mtTriangle}
            Else
                alstTriangles(mtTriangle.V12EdgeIndex).Add(mtTriangle)
            End If

            If alstTriangles(mtTriangle.V23EdgeIndex) Is Nothing Then
                alstTriangles(mtTriangle.V23EdgeIndex) = New List(Of MeshTriangle) From {mtTriangle}
            Else
                alstTriangles(mtTriangle.V23EdgeIndex).Add(mtTriangle)
            End If

            If alstTriangles(mtTriangle.V31EdgeIndex) Is Nothing Then
                alstTriangles(mtTriangle.V31EdgeIndex) = New List(Of MeshTriangle) From {mtTriangle}
            Else
                alstTriangles(mtTriangle.V31EdgeIndex).Add(mtTriangle)
            End If
        Next

        For intIndex As Integer = 0 To lstTempEdges.Count - 1
            lstEdges.Add(New MeshEdge(lstTempEdges(intIndex).Item1, lstTempEdges(intIndex).Item2, alstTriangles(intIndex).ToArray()))
        Next

        For Each meEdge In lstEdges
            If meEdge.Triangles.Count <= 1 Then
                lstDisconnectedEdges.Add(meEdge)
            End If
        Next

        Points = lstPoints.ToArray()
        Triangles = lstTriangles.ToArray()
        Edges = lstEdges.ToArray()
        DisconnectedEdges = lstDisconnectedEdges.ToArray()
    End Sub

    Public Function CreateDisconnectedEdgesLineGroup(cColor As Color) As GeometryLineGroup
        Dim glgGroup As New GeometryLineGroup

        glgGroup.EnableLighting = False

        For Each meEdge As MeshEdge In DisconnectedEdges
            glgGroup.Lines.Add(New GeometryLine(cColor, Points(meEdge.V1Index), Points(meEdge.V2Index)))
        Next

        glgGroup.UpdateBounds()

        Return glgGroup
    End Function

    Public Function CreateSilhouette(vEyeDir As Vector3, cColor As Color) As GeometryLineGroup
        Dim glgGroup As New GeometryLineGroup
        Dim lstEdges As New List(Of MeshEdge)
        Dim blnHasBack As Boolean
        Dim blnHasFront As Boolean

        For Each meEdge As MeshEdge In Edges
            If meEdge.Triangles.Count <= 1 Then
                lstEdges.Add(meEdge)
            Else
                blnHasBack = False
                blnHasFront = False
                For Each mtTriangle As MeshTriangle In meEdge.Triangles
                    If Vector3.DotProduct(mtTriangle.Normal, vEyeDir) >= 0 Then
                        blnHasBack = True
                    Else
                        blnHasFront = True
                    End If

                    If blnHasBack AndAlso blnHasFront Then
                        Exit For
                    End If
                Next

                If blnHasBack AndAlso blnHasFront Then
                    lstEdges.Add(meEdge)
                End If
            End If
        Next

        glgGroup.EnableLighting = False

        For Each meEdge As MeshEdge In lstEdges
            glgGroup.Lines.Add(New GeometryLine(cColor, Points(meEdge.V1Index), Points(meEdge.V2Index)))
        Next

        glgGroup.UpdateBounds()

        Return glgGroup
    End Function

    Private Function GetOrAddEdge(lstEdges As List(Of Tuple(Of Integer, Integer)), intV1Index As Integer, intV2Index As Integer) As Integer
        Dim intIndex As Integer

        For Each meEdge As Tuple(Of Integer, Integer) In lstEdges
            If (meEdge.Item1 = intV1Index AndAlso meEdge.Item2 = intV2Index) OrElse (meEdge.Item1 = intV2Index AndAlso meEdge.Item2 = intV1Index) Then
                Return intIndex
                Exit For
            End If
            intIndex += 1
        Next

        lstEdges.Add(New Tuple(Of Integer, Integer)(intV1Index, intV2Index))

        Return intIndex
    End Function

    Public Structure MeshTriangle
        Public ReadOnly V1Index As Integer
        Public ReadOnly V2Index As Integer
        Public ReadOnly V3Index As Integer

        Public ReadOnly V12EdgeIndex As Integer
        Public ReadOnly V23EdgeIndex As Integer
        Public ReadOnly V31EdgeIndex As Integer

        Public ReadOnly Normal As Vector3

        Public Sub New(intV1Index As Integer, intV2Index As Integer, intV3Index As Integer, intV12EdgeIndex As Integer, intV23EdgeIndex As Integer, intV31EdgeIndex As Integer, vNormal As Vector3)
            V1Index = intV1Index
            V2Index = intV2Index
            V3Index = intV3Index

            V12EdgeIndex = intV12EdgeIndex
            V23EdgeIndex = intV23EdgeIndex
            V31EdgeIndex = intV31EdgeIndex

            Normal = vNormal
        End Sub
    End Structure

    Public Structure MeshEdge
        Public ReadOnly V1Index As Integer
        Public ReadOnly V2Index As Integer

        Public ReadOnly Triangles() As MeshTriangle

        Public Sub New(intV1Index As Integer, intV2Index As Integer, atTriangles() As MeshTriangle)
            V1Index = intV1Index
            V2Index = intV2Index

            Triangles = atTriangles
        End Sub
    End Structure
End Class
