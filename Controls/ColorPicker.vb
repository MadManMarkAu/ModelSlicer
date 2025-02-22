Public Class ColorPicker
    Private Const FIXED_HEIGHT As Integer = 23

    Private _color As Color

    Public Property Value As Color
        Get
            Return _color
        End Get
        Set(value As Color)
            _color = value
            pnlColorDisplay.BackColor = _color
        End Set
    End Property

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(MyBase.DefaultSize.Width, FIXED_HEIGHT)
        End Get
    End Property

    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        Return New Size(MyBase.GetPreferredSize(proposedSize).Width, FIXED_HEIGHT)
    End Function

    Protected Overrides Sub SetClientSizeCore(x As Integer, y As Integer)
        MyBase.SetClientSizeCore(x, FIXED_HEIGHT)
    End Sub

    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        MyBase.SetBoundsCore(x, y, width, FIXED_HEIGHT, specified)
    End Sub

    Private Sub butPickColor_Click(sender As Object, e As EventArgs) Handles butPickColor.Click
        Dim a As Control

        Using pick As New ColorDialog
            pick.AllowFullOpen = True
            pick.Color = _color
            If pick.ShowDialog(Me) = DialogResult.OK Then
                Value = Color.FromArgb(255, pick.Color) ' Remove any potential alpha value.
            End If
        End Using
    End Sub
End Class
