Imports System.ComponentModel

<DefaultEvent("MouseDrag")>
Public Class ModelDisplay
    Inherits Control

    Public Event MouseDrag(sender As Object, e As MouseDragEventArgs)

    Private m_aggGeometryGroups() As GeometryGroup = New GeometryGroup() {}

    Private m_mModelMatrix As Matrix = Matrix.Identity()
    Private m_qViewQuat As Quaternion = Quaternion.Identity()

    Private m_blnDragging As Boolean
    Private m_intLastMouseX As Integer
    Private m_intLastMouseY As Integer

    Private m_rRenderer As Renderer

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ModelMatrix As Matrix
        Get
            Return m_mModelMatrix
        End Get
        Set(value As Matrix)
            m_mModelMatrix = value

            Invalidate()
        End Set
    End Property

    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ViewQuaternion As Quaternion
        Get
            Return m_qViewQuat
        End Get
        Set(value As Quaternion)
            m_qViewQuat = value

            Invalidate()
        End Set
    End Property

    <DefaultValue(False)>
    Public Property EnableRotation As Boolean = False

    <DefaultValue(1.0! / 128.0!)>
    Public Property RotationRate As Single = 1 / 128

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cpParams As CreateParams

            cpParams = MyBase.CreateParams
            cpParams.ExStyle = cpParams.ExStyle Or &H10000I
            cpParams.ExStyle = cpParams.ExStyle And (Not &H200I)
            cpParams.Style = cpParams.Style Or &H800000I

            Return cpParams
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
        If m_rRenderer IsNot Nothing Then
            m_rRenderer.Dispose()
            m_rRenderer = Nothing
        End If

        MyBase.DestroyHandle()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        If ClientSize.Width > 0 AndAlso ClientSize.Height > 0 Then
            If m_rRenderer Is Nothing Then
                If Not DesignMode Then
                    m_rRenderer = New Renderer(Handle, ClientSize.Width, ClientSize.Height)
                End If
            Else
                m_rRenderer.Resize(ClientSize.Width, ClientSize.Height)
            End If
        End If

        Call UpdateProjectionMatrix()
        Invalidate()

        MyBase.OnResize(e)
    End Sub

    Public Sub SetDrawData(ParamArray aggGeometryGroups() As GeometryGroup)
        m_aggGeometryGroups = aggGeometryGroups

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If e.Button And MouseButtons.Left Then
            m_blnDragging = True
            m_intLastMouseX = e.X
            m_intLastMouseY = e.Y
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        If m_blnDragging Then
            If EnableRotation Then
                Call ApplyDragRotation(e.X - m_intLastMouseX, e.Y - m_intLastMouseY)
            End If

            RaiseEvent MouseDrag(Me, New MouseDragEventArgs(e.X - m_intLastMouseX, e.Y - m_intLastMouseY))

            m_intLastMouseX = e.X
            m_intLastMouseY = e.Y
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If e.Button And MouseButtons.Left Then
            If EnableRotation Then
                Call ApplyDragRotation(e.X - m_intLastMouseX, e.Y - m_intLastMouseY)
            End If

            RaiseEvent MouseDrag(Me, New MouseDragEventArgs(e.X - m_intLastMouseX, e.Y - m_intLastMouseY))

            m_intLastMouseX = e.X
            m_intLastMouseY = e.Y
            m_blnDragging = False
        End If
    End Sub

    Private Sub ApplyDragRotation(intDeltaX As Integer, intDeltaY As Integer)
        m_qViewQuat *= Quaternion.FromVectors(New Vector3(0, 0, -1), Vector3.Normalize(New Vector3(-intDeltaX * RotationRate, intDeltaY * RotationRate, -1)))
        m_qViewQuat.Normalize()

        Invalidate()
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If m_rRenderer IsNot Nothing Then
            m_rRenderer.ModelMatrix = m_mModelMatrix
            m_rRenderer.ViewMatrix = m_qViewQuat.ToMatrix()
            m_rRenderer.BackColor = Color3.FromColor(BackColor)
            m_rRenderer.AmbientLightColor = New Color3(0.25, 0.25, 0.25)
            m_rRenderer.DirectionalLightColor = New Color3(0.75, 0.75, 0.75)
            m_rRenderer.DirectionalLightDir = New Vector3(-1, -1, 1)
            m_rRenderer.BeginFrame()
            For Each ggGroup As GeometryGroup In m_aggGeometryGroups
                m_rRenderer.ClearDepthBuffer()
                m_rRenderer.EnableLighting = ggGroup.EnableLighting
                If TypeOf ggGroup Is GeometryTriangleGroup Then
                    m_rRenderer.DrawTriangles(DirectCast(ggGroup, GeometryTriangleGroup).Triangles.ToArray())
                ElseIf TypeOf ggGroup Is GeometryLineGroup Then
                    m_rRenderer.DrawLines(DirectCast(ggGroup, GeometryLineGroup).Lines.ToArray())
                End If
            Next
            m_rRenderer.EndFrame()
        Else
            e.Graphics.Clear(BackColor)
        End If
    End Sub

    Private Sub UpdateProjectionMatrix()
        Dim sngSmallest As Single
        Dim sngLargest As Single

        sngSmallest = Math.Min(ClientSize.Width, ClientSize.Height)
        sngLargest = Math.Max(ClientSize.Width, ClientSize.Height)

        If m_rRenderer IsNot Nothing Then
            m_rRenderer.ProjMatrix = New Matrix(ClientSize.Height / sngLargest, 0, 0, 0, 0, ClientSize.Width / sngLargest, 0, 0, 0, 0, 0.0001, 0, 0, 0, 0.5, 1) ' Matrix.PerspectiveFovLH(Math.PI / 4, ClientSize.Width / CSng(ClientSize.Height), 0.1, 1000)
        End If
    End Sub
End Class
