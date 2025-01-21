Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, Pack:=4)>
Public Structure Color4
    Public R As Single
    Public G As Single
    Public B As Single
    Public A As Single

    Public Sub New(sngR As Single, sngG As Single, sngB As Single, sngA As Single)
        R = sngR
        G = sngG
        B = sngB
        A = sngA
    End Sub

    Public Shared Function FromColor(cColor As Color) As Color4
        Return New Color4(CSng(cColor.R) / 255, CSng(cColor.G) / 255, CSng(cColor.B) / 255, CSng(cColor.A) / 255)
    End Function
End Structure
