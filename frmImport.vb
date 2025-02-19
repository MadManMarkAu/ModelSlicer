Imports System.Runtime.CompilerServices

Public Class frmImport
    Private _fileName As String
    Private _result As Geometry

    Public Property FileName As String
        Get
            Return _fileName
        End Get
        Set(value As String)
            If Not Equals(_fileName, value) Then
                _fileName = value
                txtFileName.Text = _fileName
            End If
        End Set
    End Property

    Public ReadOnly Property Result As Geometry
        Get
            Return _result
        End Get
    End Property

    Private Sub butImport_Click(sender As Object, e As EventArgs) Handles butImport.Click
        Dim loadUnit As Unit
        Dim upAxis As Axis

        loadUnit = cmbUnits.SelectedIndex

        If rbUpX.Checked Then
            upAxis = Axis.X
        ElseIf rbUpY.Checked Then
            upAxis = Axis.Y
        ElseIf rbUpZ.Checked Then
            upAxis = Axis.Z
        End If

        _result = Geometry.LoadWavefrontObj(_fileName, loadUnit, upAxis)

        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class