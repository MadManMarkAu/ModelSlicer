Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Color3
    Public R As Single
    Public G As Single
    Public B As Single

    Public Sub New(red As Single, green As Single, blue As Single)
        R = red
        G = green
        B = blue
    End Sub

    Public Shared Function FromColor(color As Color) As Color3
        Return New Color3(CSng(color.R) / 255, CSng(color.G) / 255, CSng(color.B) / 255)
    End Function
End Structure
