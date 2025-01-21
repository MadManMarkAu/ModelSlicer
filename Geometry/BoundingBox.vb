Public Structure BoundingBox
    Public Minimum As Vector3
    Public Maximum As Vector3

    Public ReadOnly Property Width As Single
        Get
            Return Math.Abs(Maximum.X - Minimum.X)
        End Get
    End Property

    Public ReadOnly Property Height As Single
        Get
            Return Math.Abs(Maximum.Y - Minimum.Y)
        End Get
    End Property

    Public ReadOnly Property Depth As Single
        Get
            Return Math.Abs(Maximum.Z - Minimum.Z)
        End Get
    End Property

    Public Sub New(vMinimum As Vector3, vMaximum As Vector3)
        Minimum = vMinimum
        Maximum = vMaximum
    End Sub

    Public Shared Function CombineBounds(ParamArray abbBounds() As BoundingBox) As BoundingBox
        Dim vMin As Vector3
        Dim vMax As Vector3
        Dim blnBoundsSet As Boolean

        For Each bbBounds As BoundingBox In abbBounds
            If Not blnBoundsSet Then
                blnBoundsSet = True
                vMin = bbBounds.Minimum
                vMax = bbBounds.Maximum
            Else
                If vMin.X > bbBounds.Minimum.X Then
                    vMin.X = bbBounds.Minimum.X
                End If
                If vMin.Y > bbBounds.Minimum.Y Then
                    vMin.Y = bbBounds.Minimum.Y
                End If
                If vMin.Z > bbBounds.Minimum.Z Then
                    vMin.Z = bbBounds.Minimum.Z
                End If
                If vMax.X < bbBounds.Maximum.X Then
                    vMax.X = bbBounds.Maximum.X
                End If
                If vMax.Y < bbBounds.Maximum.Y Then
                    vMax.Y = bbBounds.Maximum.Y
                End If
                If vMax.Z < bbBounds.Maximum.Z Then
                    vMax.Z = bbBounds.Maximum.Z
                End If
            End If
        Next
    End Function
End Structure
