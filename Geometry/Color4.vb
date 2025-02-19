Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Color4
    Public R As Single
    Public G As Single
    Public B As Single
    Public A As Single

    Public Sub New(red As Single, green As Single, blue As Single, alpha As Single)
        R = red
        G = green
        B = blue
        A = alpha
    End Sub

    Public Shared Function FromColor(color As Color) As Color4
        Return New Color4(CSng(color.R) / 255, CSng(color.G) / 255, CSng(color.B) / 255, CSng(color.A) / 255)
    End Function
End Structure
