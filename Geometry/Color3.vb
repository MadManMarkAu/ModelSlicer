Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Color3
    Public R As Single
    Public G As Single
    Public B As Single

    Public Sub New(sngR As Single, sngG As Single, sngB As Single)
        R = sngR
        G = sngG
        B = sngB
    End Sub

    Public Shared Function FromColor(cColor As Color) As Color3
        Return New Color3(CSng(cColor.R) / 255, CSng(cColor.G) / 255, CSng(cColor.B) / 255)
    End Function
End Structure
