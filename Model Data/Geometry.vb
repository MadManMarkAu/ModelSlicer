Imports System.IO

Public Class Geometry
    ' Backing variable for Bounds property
    Private m_bbBounds As BoundingBox

    ''' <summary>
    ''' Describes the triangles used to construct this geometry data
    ''' </summary>
    Public ReadOnly Property Triangles As New List(Of GeometryTriangle)

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
        Dim vMin As Vector3
        Dim vMax As Vector3
        Dim blnBoundsInit As Boolean

        ' Iterate through each triangle in this geometry object
        For Each tTriangle As GeometryTriangle In Triangles
            If Not blnBoundsInit Then
                ' This is the first time through the loop, so we take the bounds of the first triangle as-is
                vMin = tTriangle.GetMin()
                vMax = tTriangle.GetMax()
                blnBoundsInit = True
            Else
                ' Subsequent iterations through the loop update the min/max values, according to the bounds of the current triangle
                vMin = Min(vMin, tTriangle.GetMin())
                vMax = Max(vMax, tTriangle.GetMax())
            End If
        Next

        ' Construct the BoundingBox structure
        m_bbBounds = New BoundingBox(vMin, vMax)
    End Sub

    ' Returns a Vector3 describing the minimum (X,Y,Z) components of the two passed Vector3 structures
    Private Function Min(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Min(vVec1.X, vVec2.X),
            Math.Min(vVec1.Y, vVec2.Y),
            Math.Min(vVec1.Z, vVec2.Z)
        )
    End Function

    ' Returns a Vector3 describing the maximum (X,Y,Z) components of the two passed Vector3 structures
    Private Function Max(vVec1 As Vector3, vVec2 As Vector3) As Vector3
        Return New Vector3(
            Math.Max(vVec1.X, vVec2.X),
            Math.Max(vVec1.Y, vVec2.Y),
            Math.Max(vVec1.Z, vVec2.Z)
        )
    End Function

    ''' <summary>
    ''' Loads a Wavefront OBJ file (*.obj) from a file.
    ''' </summary>
    ''' <param name="strFileName">The obj file to load.</param>
    ''' <returns>A <see cref="Geometry"/> object describing the model.</returns>
    Public Shared Function LoadWavefrontObj(strFileName As String) As Geometry
        Dim gOutput As New Geometry

        ' Variables to store the text line as it is processed
        Dim strLine As String
        Dim astrParts() As String
        Dim sngPart1 As Single
        Dim sngPart2 As Single
        Dim sngPart3 As Single

        ' List of indexes vertices in the model file
        Dim lstVerts As New List(Of Vector3)

        ' Temporary storage used when reading face data from the file
        Dim vV1 As Vector3
        Dim vV2 As Vector3
        Dim vV3 As Vector3

        ' Open the file for reading
        Using fsStream As New FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            Using srReader As New StreamReader(fsStream)
                ' Continue until no more data
                While Not srReader.EndOfStream
                    ' Read the next line, and split it out into fields
                    strLine = srReader.ReadLine()
                    ' Ignore comment lines
                    If strLine.StartsWith("#") Then
                        strLine = String.Empty
                    End If
                    ' Split out the line into elements
                    astrParts = strLine.Split(" "c)

                    Select Case astrParts(0)
                        Case "v"
                            ' This line contains vertex data

                            ' Sanity check, and parse text into floating point values
                            If astrParts.Length >= 4 AndAlso Single.TryParse(astrParts(1), sngPart1) AndAlso Single.TryParse(astrParts(2), sngPart2) AndAlso Single.TryParse(astrParts(3), sngPart3) Then
                                lstVerts.Add(New Vector3(sngPart1, sngPart2, -sngPart3))
                            Else
                                Throw New ApplicationException("Vertex data had less than 3 elements, or one of the elements was non-numeric")
                            End If

                        Case "f"
                            ' This line contains face data

                            ' Parse the first two face data elements
                            ' Note we store the second face data in vV3 because we will transfer it to vV2 when we read the next face data element
                            vV1 = ParseFacepoint(astrParts(1), lstVerts)
                            vV3 = ParseFacepoint(astrParts(2), lstVerts)

                            ' Process all remaining face data elements
                            ' We do it this way so the loader can handle triangles, or quads
                            ' Quads will be split into two triangles
                            For intIndex As Integer = 3 To astrParts.Length - 1
                                vV2 = vV3
                                vV3 = ParseFacepoint(astrParts(intIndex), lstVerts)

                                gOutput.Triangles.Add(New GeometryTriangle(vV3, vV2, vV1))
                            Next

                    End Select
                End While
            End Using
        End Using

        ' Update the geometry bounds
        gOutput.UpdateBounds()

        Return gOutput
    End Function

    ' Given a face element, parses the data, discarding everything except the vertex index, and returns the referenced vertex
    Private Shared Function ParseFacepoint(strData As String, lstVerts As List(Of Vector3)) As Vector3
        Dim vOutput As New Vector3
        Dim astrParts() As String = strData.Split("/"c) ' Split out the face element into constituent parts
        Dim intIndex As Integer

        ' Sanity check
        If astrParts.Length < 1 OrElse astrParts(0).Length = 0 Then
            Throw New ApplicationException("Face data element did not contain a vertex index")
        End If

        If Integer.TryParse(astrParts(0), intIndex) Then
            Return lstVerts(intIndex - 1)
        Else
            Throw New ApplicationException("Face data element has a non-numeric vertex index")
        End If
    End Function
End Class
