Imports System.IO

Public Class Geometry
    Public Shared Function StringToUnit(name As String) As Unit
        Select Case name.ToLower()
            Case "millimeters"
                Return Unit.Millimeter
            Case "centimeters"
                Return Unit.Centimeter
            Case "meters"
                Return Unit.Meter
            Case "inches"
                Return Unit.Inch
            Case "feet"
                Return Unit.Foot
            Case Else
                Return Unit.Millimeter
        End Select
    End Function

    Private _bounds As BoundingBox
    Private _upAxis As Axis = Axis.Y

    ''' <summary>
    ''' Describes the objects contained in the geometry data
    ''' </summary>
    Public ReadOnly Property Groups As New List(Of GeometryTriangleGroup)

    ''' <summary>
    ''' Returns the axis-aligned bounding box (AABB) of this geometry data
    ''' </summary>
    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return _bounds
        End Get
    End Property

    ''' <summary>
    ''' Defines what units the vertex values are expressed in. Affects sizing when outputting.
    ''' </summary>
    Public Property Units As Unit

    Public ReadOnly Property UpAxis As Axis
        Get
            Return _upAxis
        End Get
    End Property

    'Public Sub ChangeScale(newUnit As Unit)
    '    Dim scale As Single = GetUnitConversionRatio(_units, newUnit)

    '    For i As Integer = 0 To Groups.Count - 1
    '        Groups(i).Scale(scale)
    '    Next i

    '    _units = newUnit
    'End Sub

    Public Sub ChangeUpAxis(newUpAxis As Axis)
        Dim oldUp As Vector3
        Dim newUp As Vector3
        Dim quat As Quaternion

        If _upAxis = newUpAxis Then
            Return
        End If

        Select Case _upAxis
            Case Axis.X
                oldUp = New Vector3(1, 0, 0)

            Case Axis.Y
                oldUp = New Vector3(0, 1, 0)

            Case Axis.Z
                oldUp = New Vector3(0, 0, 1)

        End Select

        Select Case newUpAxis
            Case Axis.X
                newUp = New Vector3(1, 0, 0)

            Case Axis.Y
                newUp = New Vector3(0, 1, 0)

            Case Axis.Z
                newUp = New Vector3(0, 0, 1)

        End Select

        quat = Quaternion.FromVectors(oldUp, newUp)

        For i As Integer = 0 To (Groups.Count - 1)
            Groups(i).ApplyQuaternion(quat)
        Next

        _upAxis = newUpAxis
    End Sub

    ''' <summary>
    ''' Recalculates the axis-aligned bounding box of this geometry data
    ''' </summary>
    Public Sub UpdateBounds()
        Dim isBoundsInit As Boolean

        For Each triGroup As GeometryTriangleGroup In Groups
            triGroup.UpdateBounds()

            If Not isBoundsInit Then
                _bounds = triGroup.Bounds
                isBoundsInit = True
            Else
                _bounds = UpdateBounds(_bounds, triGroup.Bounds)
            End If

            For Each gtTriangle As GeometryTriangle In triGroup.Triangles
            Next
        Next
    End Sub

    Private Function UpdateBounds(bounds1 As BoundingBox, bounds2 As BoundingBox) As BoundingBox
        Return New BoundingBox(
            New Vector3(
                Math.Min(bounds1.Minimum.X, bounds2.Minimum.X),
                Math.Min(bounds1.Minimum.Y, bounds2.Minimum.Y),
                Math.Min(bounds1.Minimum.Z, bounds2.Minimum.Z)
            ),
            New Vector3(
                Math.Max(bounds1.Maximum.X, bounds2.Maximum.X),
                Math.Max(bounds1.Maximum.Y, bounds2.Maximum.Y),
                Math.Max(bounds1.Maximum.Z, bounds2.Maximum.Z)
            )
        )
    End Function

    Public Function ToTriangleGroup() As GeometryTriangleGroup
        Dim output As New GeometryTriangleGroup
        Dim triGroup As GeometryTriangleGroup
        Dim tri As GeometryTriangle

        For Each triGroup In Groups
            For Each tri In triGroup.Triangles
                output.Triangles.Add(tri)
            Next
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Function ToTriangleGroup(cColor As Color) As GeometryTriangleGroup
        Dim output As New GeometryTriangleGroup
        Dim triGroup As GeometryTriangleGroup
        Dim tri As GeometryTriangle

        For Each triGroup In Groups
            For Each tri In triGroup.Triangles
                output.Triangles.Add(New GeometryTriangle(cColor, tri.V1, tri.V2, tri.V3, tri.V1Normal, tri.V2Normal, tri.V3Normal, tri.SurfaceNormal))
            Next
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Function ToLineGroup(cColor As Color) As GeometryLineGroup
        Dim output As New GeometryLineGroup
        Dim triGroup As GeometryTriangleGroup
        Dim tri As GeometryTriangle

        For Each triGroup In Groups
            For Each tri In triGroup.Triangles
                output.Lines.Add(New GeometryLine(cColor, tri.V1, tri.V2, tri.V1Normal, tri.V2Normal, tri.SurfaceNormal))
                output.Lines.Add(New GeometryLine(cColor, tri.V2, tri.V3, tri.V2Normal, tri.V3Normal, tri.SurfaceNormal))
                output.Lines.Add(New GeometryLine(cColor, tri.V3, tri.V1, tri.V3Normal, tri.V1Normal, tri.SurfaceNormal))
            Next
        Next

        output.UpdateBounds()

        Return output
    End Function

    Public Function ToNormalLineGroup(cColor As Color, sngLength As Single) As GeometryLineGroup
        Dim output As New GeometryLineGroup
        Dim triGroup As GeometryTriangleGroup
        Dim tri As GeometryTriangle
        Dim center As Vector3

        For Each triGroup In Groups
            For Each tri In triGroup.Triangles
                center = (tri.V1 + tri.V2 + tri.V3) / 3
                output.Lines.Add(New GeometryLine(cColor, center, center + tri.SurfaceNormal * sngLength))
            Next
        Next

        Return output
    End Function

    ''' <summary>
    ''' Loads a Wavefront OBJ file (*.obj) from a file.
    ''' </summary>
    ''' <param name="fileName">The obj file to load.</param>
    ''' <param name="units">The units to convert the model file to. The assumption is that model units are expressed as millimeters.</param>
    ''' <param name="upAxis">If the model is z up or y up.</param>
    ''' <returns>A <see cref="Geometry"/> object describing the model.</returns>
    Public Shared Function LoadWavefrontObj(fileName As String, units As Unit, upAxis As Axis) As Geometry
        Dim output As New Geometry

        Dim line As String
        Dim parts() As String
        Dim part1 As Single
        Dim part2 As Single
        Dim part3 As Single

        Dim verts As New List(Of Vector3)
        Dim norms As New List(Of Vector3)
        Dim currTriGroup As New GeometryTriangleGroup

        Dim v1 As (Vertex As Vector3, Normal As Vector3)
        Dim v2 As (Vertex As Vector3, Normal As Vector3)
        Dim v3 As (Vertex As Vector3, Normal As Vector3)

        output.Groups.Add(currTriGroup)

        Using stream As New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Using reader As New StreamReader(stream)
                While Not reader.EndOfStream
                    line = reader.ReadLine()
                    If line.StartsWith("#") Then
                        line = String.Empty
                    End If
                    parts = line.Split(" "c)

                    Select Case parts(0)
                        Case "o", "g"
                            If currTriGroup.Triangles.Count > 0 Then
                                currTriGroup = New GeometryTriangleGroup
                                output.Groups.Add(currTriGroup)
                            End If
                            If parts.Length >= 2 Then
                                currTriGroup.Name = line.Split(New Char() {" "c}, 2)(1)
                            End If

                        Case "v"
                            If parts.Length >= 4 AndAlso Single.TryParse(parts(1), part1) AndAlso Single.TryParse(parts(2), part2) AndAlso Single.TryParse(parts(3), part3) Then
                                verts.Add(New Vector3(part1, part2, -part3))
                            Else
                                Throw New ApplicationException("Vertex data had less than 3 elements, or one of the elements was non-numeric")
                            End If

                        Case "vn"
                            If parts.Length >= 4 AndAlso Single.TryParse(parts(1), part1) AndAlso Single.TryParse(parts(2), part2) AndAlso Single.TryParse(parts(3), part3) Then
                                norms.Add(New Vector3(part1, part2, -part3))
                            Else
                                Throw New Exception()
                            End If

                        Case "f"
                            v1 = ParseFacepoint(parts(1), verts, norms)
                            v3 = ParseFacepoint(parts(2), verts, norms)

                            For intIndex As Integer = 3 To parts.Length - 1
                                v2 = v3
                                v3 = ParseFacepoint(parts(intIndex), verts, norms)

                                currTriGroup.Triangles.Add(New GeometryTriangle(v3.Vertex, v2.Vertex, v1.Vertex, v3.Normal, v2.Normal, v1.Normal))
                            Next

                    End Select
                End While
            End Using
        End Using

        output.Units = units
        output.ChangeUpAxis(upAxis)
        output.UpdateBounds()

        Return output
    End Function

    Private Shared Function ParseFacepoint(data As String, verts As List(Of Vector3), norms As List(Of Vector3)) As (Point As Vector3, Normal As Vector3)
        Dim output As New Vector3
        Dim parts() As String = data.Split("/"c)
        Dim index As Integer
        Dim vertex As Vector3
        Dim normal As Vector3

        If parts.Length < 1 OrElse parts(0).Length = 0 Then
            Throw New ApplicationException("Face data element did not contain a vertex index")
        End If

        If Integer.TryParse(parts(0), index) Then
            vertex = verts(index - 1)
        Else
            Throw New ApplicationException("Face data element has a non-numeric vertex index")
        End If

        If parts.Length >= 2 Then
            If Integer.TryParse(parts(2), index) Then
                normal = norms(index - 1)
            Else
                Throw New ApplicationException("Face data element has a non-numeric normal index")
            End If
        End If

        Return (vertex, normal)
    End Function
End Class
