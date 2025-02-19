Imports System.ComponentModel

<DefaultEvent("MouseDrag")>
Public Class ModelDisplay
    Inherits Control

    Public Event MouseDrag(sender As Object, e As MouseDragEventArgs)

    Private _geometryGroups() As GeometryGroup = New GeometryGroup() {}

    Private _modelMatrix As Matrix = Matrix.Identity()
    Private _viewQuat As Quaternion = Quaternion.Identity()

    Private _dragging As Boolean
    Private _lastMouseX As Integer
    Private _lastMouseY As Integer

    Private _renderer As Renderer

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ModelMatrix As Matrix
        Get
            Return _modelMatrix
        End Get
        Set(value As Matrix)
            _modelMatrix = value

            Invalidate()
        End Set
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ViewQuaternion As Quaternion
        Get
            Return _viewQuat
        End Get
        Set(value As Quaternion)
            _viewQuat = value

            Invalidate()
        End Set
    End Property

    <DefaultValue(False)>
    Public Property EnableRotation As Boolean = False

    <DefaultValue(1.0! / 128.0!)>
    Public Property RotationRate As Single = 1 / 128

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim params As CreateParams

            params = MyBase.CreateParams
            params.ExStyle = params.ExStyle Or &H10000I
            params.ExStyle = params.ExStyle And (Not &H200I)
            params.Style = params.Style Or &H800000I

            Return params
        End Get
    End Property

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, False)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.UserPaint, True)

        Call UpdateProjectionMatrix()
    End Sub

    Protected Overrides Sub DestroyHandle()
        If _renderer IsNot Nothing Then
            _renderer.Dispose()
            _renderer = Nothing
        End If

        MyBase.DestroyHandle()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        If ClientSize.Width > 0 AndAlso ClientSize.Height > 0 Then
            If _renderer Is Nothing Then
                If Not DesignMode Then
                    _renderer = New Renderer(Handle, ClientSize.Width, ClientSize.Height)
                End If
            Else
                _renderer.Resize(ClientSize.Width, ClientSize.Height)
            End If
        End If

        Call UpdateProjectionMatrix()
        Invalidate()

        MyBase.OnResize(e)
    End Sub

    Public Sub SetDrawData(ParamArray aggGeometryGroups() As GeometryGroup)
        _geometryGroups = aggGeometryGroups

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If e.Button And MouseButtons.Left Then
            _dragging = True
            _lastMouseX = e.X
            _lastMouseY = e.Y
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        If _dragging Then
            If EnableRotation Then
                Call ApplyDragRotation(e.X - _lastMouseX, e.Y - _lastMouseY)
            End If

            RaiseEvent MouseDrag(Me, New MouseDragEventArgs(e.X - _lastMouseX, e.Y - _lastMouseY))

            _lastMouseX = e.X
            _lastMouseY = e.Y
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If e.Button And MouseButtons.Left Then
            If EnableRotation Then
                Call ApplyDragRotation(e.X - _lastMouseX, e.Y - _lastMouseY)
            End If

            RaiseEvent MouseDrag(Me, New MouseDragEventArgs(e.X - _lastMouseX, e.Y - _lastMouseY))

            _lastMouseX = e.X
            _lastMouseY = e.Y
            _dragging = False
        End If
    End Sub

    Private Sub ApplyDragRotation(intDeltaX As Integer, intDeltaY As Integer)
        _viewQuat *= Quaternion.FromVectors(New Vector3(0, 0, -1), Vector3.Normalize(New Vector3(-intDeltaX * RotationRate, intDeltaY * RotationRate, -1)))
        _viewQuat.Normalize()

        Invalidate()
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If _renderer IsNot Nothing Then
            _renderer.ModelMatrix = _modelMatrix
            _renderer.ViewMatrix = _viewQuat.ToMatrix()
            _renderer.BackColor = Color3.FromColor(BackColor)
            _renderer.AmbientLightColor = New Color3(0.25, 0.25, 0.25)
            _renderer.DirectionalLightColor = New Color3(0.75, 0.75, 0.75)
            _renderer.DirectionalLightDir = New Vector3(-1, -1, 1)
            _renderer.BeginFrame()
            For Each ggGroup As GeometryGroup In _geometryGroups
                _renderer.ClearDepthBuffer()
                _renderer.EnableLighting = ggGroup.EnableLighting
                If TypeOf ggGroup Is GeometryTriangleGroup Then
                    _renderer.DrawTriangles(DirectCast(ggGroup, GeometryTriangleGroup).Triangles.ToArray())
                ElseIf TypeOf ggGroup Is GeometryLineGroup Then
                    _renderer.DrawLines(DirectCast(ggGroup, GeometryLineGroup).Lines.ToArray())
                End If
            Next
            _renderer.EndFrame()
        Else
            e.Graphics.Clear(BackColor)
        End If
    End Sub

    Private Sub UpdateProjectionMatrix()
        Dim sngSmallest As Single
        Dim sngLargest As Single

        sngSmallest = Math.Min(ClientSize.Width, ClientSize.Height)
        sngLargest = Math.Max(ClientSize.Width, ClientSize.Height)

        If _renderer IsNot Nothing Then
            _renderer.ProjMatrix = New Matrix(ClientSize.Height / sngLargest, 0, 0, 0, 0, ClientSize.Width / sngLargest, 0, 0, 0, 0, 0.0001, 0, 0, 0, 0.5, 1) ' Matrix.PerspectiveFovLH(Math.PI / 4, ClientSize.Width / CSng(ClientSize.Height), 0.1, 1000)
        End If
    End Sub
End Class
