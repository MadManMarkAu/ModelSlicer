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

    Public Sub New(min As Vector3, max As Vector3)
        Minimum = min
        Maximum = max
    End Sub

    Public Shared Function CombineBounds(ParamArray bounds() As BoundingBox) As BoundingBox
        Dim min As Vector3
        Dim max As Vector3
        Dim boundsSet As Boolean

        For Each box As BoundingBox In bounds
            If Not boundsSet Then
                boundsSet = True
                min = box.Minimum
                max = box.Maximum
            Else
                If min.X > box.Minimum.X Then
                    min.X = box.Minimum.X
                End If
                If min.Y > box.Minimum.Y Then
                    min.Y = box.Minimum.Y
                End If
                If min.Z > box.Minimum.Z Then
                    min.Z = box.Minimum.Z
                End If
                If max.X < box.Maximum.X Then
                    max.X = box.Maximum.X
                End If
                If max.Y < box.Maximum.Y Then
                    max.Y = box.Maximum.Y
                End If
                If max.Z < box.Maximum.Z Then
                    max.Z = box.Maximum.Z
                End If
            End If
        Next
    End Function
End Structure
