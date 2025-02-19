Public Class SpaceTree3
    Private Const MAX_POINT_COUNT As Integer = 8

    Private _root As Entry

    Public Sub New(vectors As List(Of Vector3))
        If vectors.Count <= MAX_POINT_COUNT Then
            _root = New PointList(vectors, 0)
        Else
            _root = New BinarySpace(vectors, 0, 0)
        End If
    End Sub

    Public Function GetPoint(vector As Vector3) As SpaceTreePoint
        Return _root.GetPoint(vector)
    End Function

    Public Function GetPoints() As SpaceTreePoint()
        Return _root.GetPoints()
    End Function

    Private Shared Function CompareFloat(value1 As Single, value2 As Single) As Integer
        Return value1.CompareTo(value2)

        Dim tau As Single = Math.Max(Math.Abs(value1), Math.Abs(value2)) * 0.00000001

        If value1 - value2 > tau Then
            Return -1
        ElseIf value1 - value2 < -tau Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private MustInherit Class Entry
        Public MustOverride Function GetPoint(vector As Vector3) As SpaceTreePoint
        Public MustOverride Function GetPoints() As SpaceTreePoint()
    End Class

    Private Class BinarySpace
        Inherits Entry

        Private _direction As Integer
        Private _split As Single
        Private _lower As Entry
        Private _equal As Entry
        Private _higher As Entry

        Public Sub New(vectors As List(Of Vector3), direction As Integer, ByRef index As Integer)
            Dim lower As New List(Of Vector3)
            Dim equal As New List(Of Vector3)
            Dim higher As New List(Of Vector3)
            Dim value As Single

            _direction = direction

            ' Find the center
            For Each point As Vector3 In vectors
                _split += GetValue(point)
            Next
            _split /= vectors.Count

            ' Divvy up the points
            For Each vector As Vector3 In vectors
                value = GetValue(vector)

                Select Case CompareFloat(value, _split)
                    Case -1
                        lower.Add(vector)

                    Case 0
                        equal.Add(vector)

                    Case 1
                        higher.Add(vector)

                End Select
            Next

            If lower.Count = vectors.Count OrElse higher.Count = vectors.Count Then
                equal.AddRange(lower)
                equal.AddRange(higher)
                lower.Clear()
                higher.Clear()
            End If

            ' Create the objects
            If lower.Count <= MAX_POINT_COUNT Then
                _lower = New PointList(lower, index)
            Else
                _lower = New BinarySpace(lower, (direction + 1) Mod 3, index)
            End If

            _equal = New PointList(equal, index)

            If higher.Count <= MAX_POINT_COUNT Then
                _higher = New PointList(higher, index)
            Else
                _higher = New BinarySpace(higher, (direction + 1) Mod 3, index)
            End If
        End Sub

        Public Overrides Function GetPoint(vector As Vector3) As SpaceTreePoint
            Dim value As Single = GetValue(vector)
            Dim output As SpaceTreePoint = Nothing

            Select Case CompareFloat(value, _split)
                Case -1
                    output = _lower.GetPoint(vector)

                    If output Is Nothing Then
                        output = _equal.GetPoint(vector)
                    End If

                Case 0
                    output = _equal.GetPoint(vector)

                Case 1
                    output = _higher.GetPoint(vector)

                    If output Is Nothing Then
                        output = _equal.GetPoint(vector)
                    End If

            End Select

            Return output
        End Function

        Public Overrides Function GetPoints() As SpaceTreePoint()
            Dim output As New List(Of SpaceTreePoint)

            output.AddRange(_lower.GetPoints())
            output.AddRange(_equal.GetPoints())
            output.AddRange(_higher.GetPoints())

            Return output.ToArray()
        End Function

        Private Function GetValue(vector As Vector3) As Single
            Select Case _direction
                Case 0
                    Return vector.X

                Case 1
                    Return vector.Y

                Case 2
                    Return vector.Z

                Case Else
                    Return 0

            End Select
        End Function

    End Class

    Private Class PointList
        Inherits Entry

        Public ReadOnly Points As New List(Of SpaceTreePoint)

        Public Sub New(vectors As List(Of Vector3), ByRef index As Integer)
            Dim found As Boolean

            For Each vector As Vector3 In vectors
                found = False
                For Each point As SpaceTreePoint In Points
                    If CompareFloat(point.X, vector.X) = 0 AndAlso CompareFloat(point.Y, vector.Y) = 0 AndAlso CompareFloat(point.Z, vector.Z) = 0 Then
                        found = True
                        Exit For
                    End If
                Next

                If Not found Then
                    Points.Add(New SpaceTreePoint(vector, index))
                    index += 1
                End If
            Next
        End Sub

        Public Overrides Function GetPoint(vector As Vector3) As SpaceTreePoint
            For Each point As SpaceTreePoint In Points
                If CompareFloat(point.X, vector.X) = 0 AndAlso CompareFloat(point.Y, vector.Y) = 0 AndAlso CompareFloat(point.Z, vector.Z) = 0 Then
                    Return point
                End If
            Next

            Return Nothing
        End Function

        Public Overrides Function GetPoints() As SpaceTreePoint()
            Return Points.ToArray()
        End Function
    End Class

    Public Class SpaceTreePoint
        Public ReadOnly X As Single
        Public ReadOnly Y As Single
        Public ReadOnly Z As Single
        Public ReadOnly Index As Integer

        Public Sub New(vector As Vector3, index As Integer)
            X = vector.X
            Y = vector.Y
            Z = vector.Z
            index = index
        End Sub

        Public Function ToVector3() As Vector3
            Return New Vector3(X, Y, Z)
        End Function
    End Class
End Class
