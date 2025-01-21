Public Class SpaceTree3
    Private Const MAX_POINT_COUNT As Integer = 8

    Private m_eRoot As Entry

    Public Sub New(lstVectors As List(Of Vector3))
        If lstVectors.Count <= MAX_POINT_COUNT Then
            m_eRoot = New PointList(lstVectors, 0)
        Else
            m_eRoot = New BinarySpace(lstVectors, 0, 0)
        End If
    End Sub

    Public Function GetPoint(vVector As Vector3) As SpaceTreePoint
        Return m_eRoot.GetPoint(vVector)
    End Function

    Public Function GetPoints() As SpaceTreePoint()
        Return m_eRoot.GetPoints()
    End Function

    Private Shared Function CompareFloat(sngValue1 As Single, sngValue2 As Single) As Integer
        Return sngValue1.CompareTo(sngValue2)

        Dim sngTau As Single = Math.Max(Math.Abs(sngValue1), Math.Abs(sngValue2)) * 0.00000001

        If sngValue1 - sngValue2 > sngTau Then
            Return -1
        ElseIf sngValue1 - sngValue2 < -sngTau Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private MustInherit Class Entry
        Public MustOverride Function GetPoint(vVector As Vector3) As SpaceTreePoint
        Public MustOverride Function GetPoints() As SpaceTreePoint()
    End Class

    Private Class BinarySpace
        Inherits Entry

        Private m_intDirection As Integer
        Private m_sngSplit As Single
        Private m_eLower As Entry
        Private m_eEqual As Entry
        Private m_eHigher As Entry

        Public Sub New(lstVectors As List(Of Vector3), intDirection As Integer, ByRef intIndex As Integer)
            Dim lstLower As New List(Of Vector3)
            Dim lstEqual As New List(Of Vector3)
            Dim lstHigher As New List(Of Vector3)
            Dim sngValue As Single

            m_intDirection = intDirection

            ' Find the center
            For Each vPoint As Vector3 In lstVectors
                m_sngSplit += GetValue(vPoint)
            Next
            m_sngSplit /= lstVectors.Count

            ' Divvy up the points
            For Each vVector As Vector3 In lstVectors
                sngValue = GetValue(vVector)

                Select Case CompareFloat(sngValue, m_sngSplit)
                    Case -1
                        lstLower.Add(vVector)

                    Case 0
                        lstEqual.Add(vVector)

                    Case 1
                        lstHigher.Add(vVector)

                End Select
            Next

            If lstLower.Count = lstVectors.Count OrElse lstHigher.Count = lstVectors.Count Then
                lstEqual.AddRange(lstLower)
                lstEqual.AddRange(lstHigher)
                lstLower.Clear()
                lstHigher.Clear()
            End If

            ' Create the objects
            If lstLower.Count <= MAX_POINT_COUNT Then
                m_eLower = New PointList(lstLower, intIndex)
            Else
                m_eLower = New BinarySpace(lstLower, (intDirection + 1) Mod 3, intIndex)
            End If

            m_eEqual = New PointList(lstEqual, intIndex)

            If lstHigher.Count <= MAX_POINT_COUNT Then
                m_eHigher = New PointList(lstHigher, intIndex)
            Else
                m_eHigher = New BinarySpace(lstHigher, (intDirection + 1) Mod 3, intIndex)
            End If
        End Sub

        Public Overrides Function GetPoint(vVector As Vector3) As SpaceTreePoint
            Dim sngValue As Single = GetValue(vVector)
            Dim stpOutput As SpaceTreePoint = Nothing

            Select Case CompareFloat(sngValue, m_sngSplit)
                Case -1
                    stpOutput = m_eLower.GetPoint(vVector)

                    If stpOutput Is Nothing Then
                        stpOutput = m_eEqual.GetPoint(vVector)
                    End If

                Case 0
                    stpOutput = m_eEqual.GetPoint(vVector)

                Case 1
                    stpOutput = m_eHigher.GetPoint(vVector)

                    If stpOutput Is Nothing Then
                        stpOutput = m_eEqual.GetPoint(vVector)
                    End If

            End Select

            Return stpOutput
        End Function

        Public Overrides Function GetPoints() As SpaceTreePoint()
            Dim lstOutput As New List(Of SpaceTreePoint)

            lstOutput.AddRange(m_eLower.GetPoints())
            lstOutput.AddRange(m_eEqual.GetPoints())
            lstOutput.AddRange(m_eHigher.GetPoints())

            Return lstOutput.ToArray()
        End Function

        Private Function GetValue(vVector As Vector3) As Single
            Select Case m_intDirection
                Case 0
                    Return vVector.X

                Case 1
                    Return vVector.Y

                Case 2
                    Return vVector.Z

                Case Else
                    Return 0

            End Select
        End Function

    End Class

    Private Class PointList
        Inherits Entry

        Public ReadOnly Points As New List(Of SpaceTreePoint)

        Public Sub New(lstVectors As List(Of Vector3), ByRef intIndex As Integer)
            Dim blnFound As Boolean

            For Each vVector As Vector3 In lstVectors
                blnFound = False
                For Each stpPoint As SpaceTreePoint In Points
                    If CompareFloat(stpPoint.X, vVector.X) = 0 AndAlso CompareFloat(stpPoint.Y, vVector.Y) = 0 AndAlso CompareFloat(stpPoint.Z, vVector.Z) = 0 Then
                        blnFound = True
                        Exit For
                    End If
                Next

                If Not blnFound Then
                    Points.Add(New SpaceTreePoint(vVector, intIndex))
                    intIndex += 1
                End If
            Next
        End Sub

        Public Overrides Function GetPoint(vVector As Vector3) As SpaceTreePoint
            For Each stpPoint As SpaceTreePoint In Points
                If CompareFloat(stpPoint.X, vVector.X) = 0 AndAlso CompareFloat(stpPoint.Y, vVector.Y) = 0 AndAlso CompareFloat(stpPoint.Z, vVector.Z) = 0 Then
                    Return stpPoint
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

        Public Sub New(vVector As Vector3, intIndex As Integer)
            X = vVector.X
            Y = vVector.Y
            Z = vVector.Z
            Index = intIndex
        End Sub

        Public Function ToVector3() As Vector3
            Return New Vector3(X, Y, Z)
        End Function
    End Class
End Class
