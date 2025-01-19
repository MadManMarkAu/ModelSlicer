Imports System.IO

Public Class Geometry
    Private m_bbBounds As BoundingBox
    Private m_scale As Decimal
    Private m_zUp As Boolean

    public sub change_scale(new_scale As Decimal)
        m_scale = new_scale
    End sub

    ''' <summary>
    ''' Creates a new instance of the <see cref="Geometry"/> class.
    ''' </

    ''' <summary>
    ''' Describes the objects contained in the geometry data
    ''' </summary>
    Public ReadOnly Property Groups As New List(Of GeometryTriangleGroup)

    ''' <summary>
    ''' Returns the axis-aligned bounding box of this geometry data
    ''' </summary>
    Public ReadOnly Property Bounds As BoundingBox
        Get
            Return m_bbBounds
        End Get
    End Property

    ''' <summary>
    ''' Recalculates the axis-aligned bounding box of this geometry data
    ''' </summary>
    Public Sub UpdateBounds()
        Dim blnBoundsInit As Boolean

        For Each gtgGroup As GeometryTriangleGroup In Groups
            gtgGroup.UpdateBounds()

            If Not blnBoundsInit Then
                m_bbBounds = gtgGroup.Bounds
                blnBoundsInit = True
            Else
                m_bbBounds = UpdateBounds(m_bbBounds, gtgGroup.Bounds)
            End If

            For Each gtTriangle As GeometryTriangle In gtgGroup.Triangles
            Next
        Next
    End Sub

    Private Function UpdateBounds(bbBounds1 As BoundingBox, bbBounds2 As BoundingBox) As BoundingBox
        Return New BoundingBox(
            New Vector3(
                Math.Min(bbBounds1.Minimum.X, bbBounds2.Minimum.X),
                Math.Min(bbBounds1.Minimum.Y, bbBounds2.Minimum.Y),
                Math.Min(bbBounds1.Minimum.Z, bbBounds2.Minimum.Z)
            ),
            New Vector3(
                Math.Max(bbBounds1.Maximum.X, bbBounds2.Maximum.X),
                Math.Max(bbBounds1.Maximum.Y, bbBounds2.Maximum.Y),
                Math.Max(bbBounds1.Maximum.Z, bbBounds2.Maximum.Z)
            )
        )
    End Function

    Public Function ToTriangleGroup() As GeometryTriangleGroup
        Dim gtgOutput As New GeometryTriangleGroup
        Dim gtgGroup As GeometryTriangleGroup
        Dim gtTriangle As GeometryTriangle

        For Each gtgGroup In Groups
            For Each gtTriangle In gtgGroup.Triangles
                gtgOutput.Triangles.Add(gtTriangle)
            Next
        Next

        gtgOutput.UpdateBounds()

        Return gtgOutput
    End Function

    Public Function ToTriangleGroup(cColor As Color) As GeometryTriangleGroup
        Dim gtgOutput As New GeometryTriangleGroup
        Dim gtgGroup As GeometryTriangleGroup
        Dim gtTriangle As GeometryTriangle

        For Each gtgGroup In Groups
            For Each gtTriangle In gtgGroup.Triangles
                gtgOutput.Triangles.Add(New GeometryTriangle(cColor, gtTriangle.V1, gtTriangle.V2, gtTriangle.V3, gtTriangle.V1Normal, gtTriangle.V2Normal, gtTriangle.V3Normal, gtTriangle.SurfaceNormal))
            Next
        Next

        gtgOutput.UpdateBounds()

        Return gtgOutput
    End Function

    Public Function ToLineGroup(cColor As Color) As GeometryLineGroup
        Dim glgOutput As New GeometryLineGroup
        Dim gtgGroup As GeometryTriangleGroup
        Dim gtTriangle As GeometryTriangle
        Dim lstRemove As New List(Of GeometryLine)

        For Each gtgGroup In Groups
            For Each gtTriangle In gtgGroup.Triangles
                glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V1, gtTriangle.V2, gtTriangle.V1Normal, gtTriangle.V2Normal, gtTriangle.SurfaceNormal))
                glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V2, gtTriangle.V3, gtTriangle.V2Normal, gtTriangle.V3Normal, gtTriangle.SurfaceNormal))
                glgOutput.Lines.Add(New GeometryLine(cColor, gtTriangle.V3, gtTriangle.V1, gtTriangle.V3Normal, gtTriangle.V1Normal, gtTriangle.SurfaceNormal))
            Next
        Next

        glgOutput.UpdateBounds()

        Return glgOutput
    End Function

    Public Function ToNormalLineGroup(cColor As Color, sngLength As Single) As GeometryLineGroup
        Dim glgOutput As New GeometryLineGroup
        Dim gtgGroup As GeometryTriangleGroup
        Dim gtTriangle As GeometryTriangle
        Dim vCenter As Vector3

        For Each gtgGroup In Groups
            For Each gtTriangle In gtgGroup.Triangles
                vCenter = (gtTriangle.V1 + gtTriangle.V2 + gtTriangle.V3) / 3
                glgOutput.Lines.Add(New GeometryLine(cColor, vCenter, vCenter + gtTriangle.SurfaceNormal * sngLength))
            Next
        Next

        Return glgOutput
    End Function

    ''' <summary>
    ''' Loads a Wavefront OBJ file (*.obj) from a file.
    ''' </summary>
    ''' <param name="strFileName">The obj file to load.</param>
    ''' <param name="scale">The scale factor to use.</param>
    ''' <param name="zUp">If the model is z up or y up.</param>
    ''' <returns>A <see cref="Geometry"/> object describing the model.</returns>
    Public Shared Function LoadWavefrontObj(strFileName As String, scale As Decimal, zUp As Boolean) As Geometry
        Dim gOutput As New Geometry

        gOutput.m_scale = scale

        Dim strLine As String
        Dim astrParts() As String
        Dim sngPart1 As Single
        Dim sngPart2 As Single
        Dim sngPart3 As Single

        Dim lstVerts As New List(Of Vector3)
        Dim lstNorms As New List(Of Vector3)
        Dim gtgCurrTriGroup As New GeometryTriangleGroup

        Dim vV1 As (Vertex As Vector3, Normal As Vector3)
        Dim vV2 As (Vertex As Vector3, Normal As Vector3)
        Dim vV3 As (Vertex As Vector3, Normal As Vector3)

        gOutput.Groups.Add(gtgCurrTriGroup)

        Using fsStream As New FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Using srReader As New StreamReader(fsStream)
                While Not srReader.EndOfStream
                    strLine = srReader.ReadLine()
                    If strLine.StartsWith("#") Then
                        strLine = String.Empty
                    End If
                    astrParts = strLine.Split(" "c)

                    Select Case astrParts(0)
                        Case "o", "g"
                            If gtgCurrTriGroup.Triangles.Count > 0 Then
                                gtgCurrTriGroup = New GeometryTriangleGroup
                                gOutput.Groups.Add(gtgCurrTriGroup)
                            End If
                            If astrParts.Length >= 2 Then
                                gtgCurrTriGroup.Name = strLine.Split(New Char() {" "c}, 2)(1)
                            End If

                        Case "v"
                            If astrParts.Length >= 4 AndAlso Single.TryParse(astrParts(1), sngPart1) AndAlso Single.TryParse(astrParts(2), sngPart2) AndAlso Single.TryParse(astrParts(3), sngPart3) Then
                                If zUp Then
                                    lstVerts.Add(New Vector3(sngPart1*scale, sngPart3*scale, -sngPart2*scale))
                                Else
                                    lstVerts.Add(New Vector3(sngPart1*scale, sngPart2*scale, -sngPart3*scale))
                                End If
                            Else
                                Throw New ApplicationException("Vertex data had less than 3 elements, or one of the elements was non-numeric")
                            End If

                        Case "vn"
                            If astrParts.Length >= 4 AndAlso Single.TryParse(astrParts(1), sngPart1) AndAlso Single.TryParse(astrParts(2), sngPart2) AndAlso Single.TryParse(astrParts(3), sngPart3) Then
                                If zUp Then
                                    lstNorms.Add(New Vector3(sngPart1*scale, sngPart3*scale, -sngPart2*scale))
                                Else
                                    lstNorms.Add(New Vector3(sngPart1*scale, sngPart2*scale, -sngPart3*scale))
                                End If
                            Else
                                Throw New Exception()
                            End If

                        Case "f"
                            vV1 = ParseFacepoint(astrParts(1), lstVerts, lstNorms)
                            vV3 = ParseFacepoint(astrParts(2), lstVerts, lstNorms)

                            For intIndex As Integer = 3 To astrParts.Length - 1
                                vV2 = vV3
                                vV3 = ParseFacepoint(astrParts(intIndex), lstVerts, lstNorms)

                                gtgCurrTriGroup.Triangles.Add(New GeometryTriangle(vV3.Vertex, vV2.Vertex, vV1.Vertex, vV3.Normal, vV2.Normal, vV1.Normal))
                            Next

                    End Select
                End While
            End Using
        End Using

        gOutput.UpdateBounds()

        Return gOutput
    End Function

    Private Shared Function ParseFacepoint(strData As String, lstVerts As List(Of Vector3), lstNorms As List(Of Vector3)) As (Point As Vector3, Normal As Vector3)
        Dim vOutput As New Vector3
        Dim astrParts() As String = strData.Split("/"c)
        Dim intIndex As Integer
        Dim vVertex As Vector3
        Dim vNormal As Vector3

        If astrParts.Length < 1 OrElse astrParts(0).Length = 0 Then
            Throw New ApplicationException("Face data element did not contain a vertex index")
        End If

        If Integer.TryParse(astrParts(0), intIndex) Then
            vVertex = lstVerts(intIndex - 1)
        Else
            Throw New ApplicationException("Face data element has a non-numeric vertex index")
        End If

        If astrParts.Length >= 2 Then
            If Integer.TryParse(astrParts(2), intIndex) Then
                vNormal = lstNorms(intIndex - 1)
            Else
                Throw New ApplicationException("Face data element has a non-numeric normal index")
            End If
        End If

        Return (vVertex, vNormal)
    End Function
End Class
