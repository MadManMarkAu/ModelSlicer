Public Class MesherXYZ
    Private _ree As SpaceTree3

    Public ReadOnly Points() As Vector3
    Public ReadOnly Triangles() As MeshTriangle
    Public ReadOnly Edges() As MeshEdge
    Public ReadOnly DisconnectedEdges() As MeshEdge

    Public Sub New(triGroup As GeometryTriangleGroup)
        Dim vectors As New List(Of Vector3)
        Dim meshTriangle As MeshTriangle
        Dim meshEdge As MeshEdge
        Dim tempEdges As New List(Of Tuple(Of Integer, Integer))
        Dim v1Index As Integer
        Dim v2Index As Integer
        Dim v3Index As Integer
        Dim v12EdgeIndex As Integer
        Dim v23EdgeIndex As Integer
        Dim v31EdgeIndex As Integer

        Dim triangles As New List(Of MeshTriangle)
        Dim points As New List(Of Vector3)
        Dim edges As New List(Of MeshEdge)
        Dim disconnectedEdges As New List(Of MeshEdge)

        For Each tri As GeometryTriangle In triGroup.Triangles
            vectors.Add(tri.V1)
            vectors.Add(tri.V2)
            vectors.Add(tri.V3)
        Next

        _ree = New SpaceTree3(vectors)

        For Each point As SpaceTree3.SpaceTreePoint In _ree.GetPoints()
            points.Add(point.ToVector3())
        Next

        For Each tri As GeometryTriangle In triGroup.Triangles
            v1Index = _ree.GetPoint(tri.V1).Index
            v2Index = _ree.GetPoint(tri.V2).Index
            v3Index = _ree.GetPoint(tri.V3).Index

            v12EdgeIndex = GetOrAddEdge(tempEdges, v1Index, v2Index)
            v23EdgeIndex = GetOrAddEdge(tempEdges, v2Index, v3Index)
            v31EdgeIndex = GetOrAddEdge(tempEdges, v3Index, v1Index)

            triangles.Add(New MeshTriangle(v1Index, v2Index, v3Index, v12EdgeIndex, v23EdgeIndex, v31EdgeIndex, tri.SurfaceNormal))
        Next

        Dim triangleList(tempEdges.Count - 1) As List(Of MeshTriangle)

        For Each meshTriangle In triangles
            If triangleList(meshTriangle.V12EdgeIndex) Is Nothing Then
                triangleList(meshTriangle.V12EdgeIndex) = New List(Of MeshTriangle) From {meshTriangle}
            Else
                triangleList(meshTriangle.V12EdgeIndex).Add(meshTriangle)
            End If

            If triangleList(meshTriangle.V23EdgeIndex) Is Nothing Then
                triangleList(meshTriangle.V23EdgeIndex) = New List(Of MeshTriangle) From {meshTriangle}
            Else
                triangleList(meshTriangle.V23EdgeIndex).Add(meshTriangle)
            End If

            If triangleList(meshTriangle.V31EdgeIndex) Is Nothing Then
                triangleList(meshTriangle.V31EdgeIndex) = New List(Of MeshTriangle) From {meshTriangle}
            Else
                triangleList(meshTriangle.V31EdgeIndex).Add(meshTriangle)
            End If
        Next

        For index As Integer = 0 To tempEdges.Count - 1
            edges.Add(New MeshEdge(tempEdges(index).Item1, tempEdges(index).Item2, triangleList(index).ToArray()))
        Next

        For Each meshEdge In edges
            If meshEdge.Triangles.Count <= 1 Then
                disconnectedEdges.Add(meshEdge)
            End If
        Next

        Me.Points = points.ToArray()
        Me.Triangles = triangles.ToArray()
        Me.Edges = edges.ToArray()
        Me.DisconnectedEdges = disconnectedEdges.ToArray()
    End Sub

    Public Function CreateDisconnectedEdgesLineGroup(color As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup

        output.EnableLighting = False

        For Each meEdge As MeshEdge In DisconnectedEdges
            output.Lines.Add(New GeometryLine(color, Points(meEdge.V1Index), Points(meEdge.V2Index)))
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Function CreateSilhouette(eyeDir As Vector3, color As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup
        Dim edges As New List(Of MeshEdge)
        Dim hasBack As Boolean
        Dim hasFront As Boolean

        For Each edge As MeshEdge In Me.Edges
            If edge.Triangles.Count <= 1 Then
                edges.Add(edge)
            Else
                hasBack = False
                hasFront = False
                For Each tri As MeshTriangle In edge.Triangles
                    If Vector3.DotProduct(tri.Normal, eyeDir) >= 0 Then
                        hasBack = True
                    Else
                        hasFront = True
                    End If

                    If hasBack AndAlso hasFront Then
                        Exit For
                    End If
                Next

                If hasBack AndAlso hasFront Then
                    edges.Add(edge)
                End If
            End If
        Next

        output.EnableLighting = False

        For Each edge As MeshEdge In edges
            output.Lines.Add(New GeometryLine(color, Points(edge.V1Index), Points(edge.V2Index)))
        Next

        output.UpdateBounds()

        Return output
    End Function

    Private Function GetOrAddEdge(edges As List(Of Tuple(Of Integer, Integer)), v1Index As Integer, v2Index As Integer) As Integer
        Dim index As Integer

        For Each edge As Tuple(Of Integer, Integer) In edges
            If (edge.Item1 = v1Index AndAlso edge.Item2 = v2Index) OrElse (edge.Item1 = v2Index AndAlso edge.Item2 = v1Index) Then
                Return index
                Exit For
            End If
            index += 1
        Next

        edges.Add(New Tuple(Of Integer, Integer)(v1Index, v2Index))

        Return index
    End Function

    Public Structure MeshTriangle
        Public ReadOnly V1Index As Integer
        Public ReadOnly V2Index As Integer
        Public ReadOnly V3Index As Integer

        Public ReadOnly V12EdgeIndex As Integer
        Public ReadOnly V23EdgeIndex As Integer
        Public ReadOnly V31EdgeIndex As Integer

        Public ReadOnly Normal As Vector3

        Public Sub New(v1IndexValue As Integer, v2IndexValue As Integer, v3IndexValue As Integer, v12EdgeIndexValue As Integer, v23EdgeIndexValue As Integer, v31EdgeIndexValue As Integer, normalValue As Vector3)
            V1Index = v1IndexValue
            V2Index = v2IndexValue
            V3Index = v3IndexValue

            V12EdgeIndex = v12EdgeIndexValue
            V23EdgeIndex = v23EdgeIndexValue
            V31EdgeIndex = v31EdgeIndexValue

            Normal = normalValue
        End Sub
    End Structure

    Public Structure MeshEdge
        Public ReadOnly V1Index As Integer
        Public ReadOnly V2Index As Integer

        Public ReadOnly Triangles() As MeshTriangle

        Public Sub New(v1IndexValue As Integer, v2IndexValue As Integer, trianglesValue() As MeshTriangle)
            V1Index = v1IndexValue
            V2Index = v2IndexValue

            Triangles = trianglesValue
        End Sub
    End Structure
End Class
