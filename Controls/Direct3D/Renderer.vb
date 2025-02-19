Imports SharpDX
Imports SharpDX.DXGI
Imports SharpDX.Direct3D11
Imports Device = SharpDX.Direct3D11.Device
Imports SharpDX.Mathematics.Interop
Imports SharpDX.D3DCompiler
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO

Public Class Renderer
    Implements IDisposable

    Private Const MULTISAMPLE_COUNT As Integer = 8
    Private Const VERTEX_BUFFER_SIZE As Integer = 4096

    Private _device As Device
    Private _swapChainDescription As SwapChainDescription
    Private _swapChain As SwapChain
    Private _backBuffer As Texture2D
    Private _depthBufferDesc As Texture2DDescription
    Private _depthBuffer As Texture2D
    Private _renderTargetView As RenderTargetView
    Private _depthStencilView As DepthStencilView
    Private _context As DeviceContext
    Private _disposed As Boolean

    Private _vertexShaderBytecode As CompilationResult
    Private _vertexShader As VertexShader
    Private _pixelShaderBytecode As CompilationResult
    Private _pixelShader As PixelShader
    Private _shaderSignature As ShaderSignature
    Private _shaderInputLayout As InputLayout

    Private _vertexBuffer As Buffer

    Private _matrices As New Matrices(Matrix.Identity, Matrix.Identity, Matrix.Identity)
    Private _matricesBuffer As Buffer
    Private _modelMatrix As Matrix
    Private _viewMatrix As Matrix
    Private _projMatrix As Matrix

    Private _lightParams As New LightParams(New Color3(1, 1, 1), New Color3(0.1, 0.1, 0.1), New Vector3(1, 1, -1), False)
    Private _lightParamsBuffer As Buffer

    Public Property BackColor As Color3 = New Color3(0.25, 0.25, 0.75)

    Public Property AmbientLightColor As Color3
        Get
            Return _lightParams.AmbientLightColor
        End Get
        Set(value As Color3)
            _lightParams.AmbientLightColor = value

            _context.UpdateSubresource(_lightParams, _lightParamsBuffer)
        End Set
    End Property

    Public Property DirectionalLightColor As Color3
        Get
            Return _lightParams.DirectionalLightColor
        End Get
        Set(value As Color3)
            _lightParams.DirectionalLightColor = value

            _context.UpdateSubresource(_lightParams, _lightParamsBuffer)
        End Set
    End Property

    Public Property DirectionalLightDir As Vector3
        Get
            Return _lightParams.DirectionalLightDir
        End Get
        Set(value As Vector3)
            _lightParams.DirectionalLightDir = value

            _context.UpdateSubresource(_lightParams, _lightParamsBuffer)
        End Set
    End Property

    Public Property EnableLighting As Boolean
        Get
            Return _lightParams.EnableLighting <> 0
        End Get
        Set(value As Boolean)
            _lightParams.EnableLighting = If(value, -1, 0)

            _context.UpdateSubresource(_lightParams, _lightParamsBuffer)
        End Set
    End Property

    Public Property ModelMatrix As Matrix
        Get
            Return _modelMatrix
        End Get
        Set(value As Matrix)
            _modelMatrix = value
            _matrices.Model = value

            _matrices.Model.Transpose()

            _context.UpdateSubresource(_matrices, _matricesBuffer)
        End Set
    End Property

    Public Property ViewMatrix As Matrix
        Get
            Return _viewMatrix
        End Get
        Set(value As Matrix)
            _viewMatrix = value
            _matrices.View = value

            _matrices.View.Transpose()

            _context.UpdateSubresource(_matrices, _matricesBuffer)
        End Set
    End Property

    Public Property ProjMatrix As Matrix
        Get
            Return _projMatrix
        End Get
        Set(value As Matrix)
            _projMatrix = value
            _matrices.Proj = value

            _matrices.Proj.Transpose()

            _context.UpdateSubresource(_matrices, _matricesBuffer)
        End Set
    End Property

    Public Sub New(hWnd As IntPtr, intWidth As Integer, intHeight As Integer)
        _swapChainDescription = New SwapChainDescription() With {
            .ModeDescription = New ModeDescription(intWidth, intHeight, New Rational(60, 1), Format.R8G8B8A8_UNorm),
            .SampleDescription = New SampleDescription(MULTISAMPLE_COUNT, 0),
            .Usage = Usage.BackBuffer Or Usage.RenderTargetOutput,
            .BufferCount = 1,
            .Flags = SwapChainFlags.None,
            .IsWindowed = True,
            .OutputHandle = hWnd,
            .SwapEffect = SwapEffect.Discard
        }

        Device.CreateWithSwapChain(Direct3D.DriverType.Hardware,
            DeviceCreationFlags.None,
            New Direct3D.FeatureLevel() {Direct3D.FeatureLevel.Level_11_1, Direct3D.FeatureLevel.Level_11_0, Direct3D.FeatureLevel.Level_10_1, Direct3D.FeatureLevel.Level_10_0},
            _swapChainDescription, _device, _swapChain)

        _context = _device.ImmediateContext

        _backBuffer = Direct3D11.Resource.FromSwapChain(Of Texture2D)(_swapChain, 0)
        _renderTargetView = New RenderTargetView(_device, _backBuffer)

        _depthBufferDesc = New Texture2DDescription() With {
            .Format = Format.D32_Float,
            .ArraySize = 1,
            .MipLevels = 1,
            .Width = intWidth,
            .Height = intHeight,
            .SampleDescription = New SampleDescription(MULTISAMPLE_COUNT, 0),
            .Usage = ResourceUsage.Default,
            .BindFlags = BindFlags.DepthStencil,
            .CpuAccessFlags = CpuAccessFlags.None,
            .OptionFlags = ResourceOptionFlags.None
        }

        Dim desc As New DepthStencilStateDescription() With {
            .IsDepthEnabled = True,
            .DepthWriteMask = DepthWriteMask.All,
            .DepthComparison = Comparison.Less,
            .StencilReadMask = 255,
            .StencilWriteMask = 255,
            .FrontFace = New DepthStencilOperationDescription() With {
                .FailOperation = StencilOperation.Keep,
                .DepthFailOperation = StencilOperation.Increment,
                .PassOperation = StencilOperation.Keep,
                .Comparison = Comparison.Always
            },
            .BackFace = New DepthStencilOperationDescription() With {
                .FailOperation = StencilOperation.Keep,
                .DepthFailOperation = StencilOperation.Decrement,
                .PassOperation = StencilOperation.Keep,
                .Comparison = Comparison.Always
            }
        }

        Dim state As New DepthStencilState(_device, desc)

        _depthBuffer = New Texture2D(_device, _depthBufferDesc)
        _depthStencilView = New DepthStencilView(_device, _depthBuffer)

        _context.Rasterizer.SetViewport(New RawViewportF() With {.Width = intWidth, .Height = intHeight, .MinDepth = 0F, .MaxDepth = 1.0F})
        _context.OutputMerger.SetTargets(_depthStencilView, _renderTargetView)
        _context.OutputMerger.SetDepthStencilState(state)

        'm_crVertexShaderBytecode = ShaderBytecode.CompileFromFile("shader.fx", "VS", "vs_4_0")
        _vertexShaderBytecode = ShaderBytecode.Compile(GetResource("shader.fx"), "VS", "vs_4_0")
        _vertexShader = New VertexShader(_device, _vertexShaderBytecode)

        'm_crPixelShaderBytecode = ShaderBytecode.CompileFromFile("shader.fx", "PS", "ps_4_0")
        _pixelShaderBytecode = ShaderBytecode.Compile(GetResource("shader.fx"), "PS", "ps_4_0")
        _pixelShader = New PixelShader(_device, _pixelShaderBytecode)

        _shaderSignature = ShaderSignature.GetInputSignature(_vertexShaderBytecode)
        _shaderInputLayout = New InputLayout(
            _device,
            _shaderSignature,
            New InputElement() {
                New InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                New InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                New InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
            })

        _context.InputAssembler.InputLayout = _shaderInputLayout

        ' Create vertex buffer
        _vertexBuffer = New Buffer(_device, Utilities.SizeOf(Of Vertex)() * VERTEX_BUFFER_SIZE, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, Utilities.SizeOf(Of Vertex)())

        ' Create constant buffers
        _matricesBuffer = New Buffer(_device, Utilities.SizeOf(Of Matrices)(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0)
        _lightParamsBuffer = New Buffer(_device, Utilities.SizeOf(Of LightParams)(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0)

        ' Assign the matrices buffer
        _context.UpdateSubresource(_matrices, _matricesBuffer)
        _context.VertexShader.SetConstantBuffer(0, _matricesBuffer)
        _context.PixelShader.SetConstantBuffer(0, _matricesBuffer)

        ' Assign the lighting params buffer
        _context.UpdateSubresource(_lightParams, _lightParamsBuffer)
        _context.VertexShader.SetConstantBuffer(1, _lightParamsBuffer)
        _context.PixelShader.SetConstantBuffer(1, _lightParamsBuffer)

        _context.VertexShader.Set(_vertexShader)
        _context.PixelShader.Set(_pixelShader)
    End Sub

    Public Sub Resize(intWidth As Integer, intHeight As Integer)
        _depthStencilView.Dispose()
        _depthBuffer.Dispose()
        _renderTargetView.Dispose()
        _backBuffer.Dispose()

        _swapChain.ResizeBuffers(_swapChainDescription.BufferCount, intWidth, intHeight, Format.Unknown, SwapChainFlags.None)

        _backBuffer = Direct3D11.Resource.FromSwapChain(Of Texture2D)(_swapChain, 0)
        _renderTargetView = New RenderTargetView(_device, _backBuffer)

        _depthBufferDesc.Width = intWidth
        _depthBufferDesc.Height = intHeight

        _depthBuffer = New Texture2D(_device, _depthBufferDesc)
        _depthStencilView = New DepthStencilView(_device, _depthBuffer)

        _context.Rasterizer.SetViewport(0, 0, intWidth, intHeight, 0F, 1.0F)
        _context.OutputMerger.SetTargets(_depthStencilView, _renderTargetView)
    End Sub

    Public Sub BeginFrame()
        _context.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0F, 0)
        _context.ClearRenderTargetView(_renderTargetView, New RawColor4(BackColor.R, BackColor.G, BackColor.B, 1.0))
    End Sub

    Public Sub ClearDepthBuffer()
        _context.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0F, 0)
    End Sub

    Public Sub DrawTriangles(triangles() As GeometryTriangle)
        If triangles.Length > 0 Then
            Dim vertexes(triangles.Length * 3 - 1) As Vertex
            Dim offset As Integer

            For Each gtTriangle As GeometryTriangle In triangles
                vertexes(offset) = New Vertex(gtTriangle.V1, gtTriangle.V1Normal, gtTriangle.Color)
                offset += 1
                vertexes(offset) = New Vertex(gtTriangle.V2, gtTriangle.V2Normal, gtTriangle.Color)
                offset += 1
                vertexes(offset) = New Vertex(gtTriangle.V3, gtTriangle.V3Normal, gtTriangle.Color)
                offset += 1
            Next

            _context.InputAssembler.PrimitiveTopology = Direct3D.PrimitiveTopology.TriangleList

            Call SendVertexArray(vertexes, 3)
        End If
    End Sub

    Public Sub DrawLines(lines() As GeometryLine)
        If lines.Length > 0 Then
            Dim vertexes(lines.Length * 2 - 1) As Vertex
            Dim offset As Integer

            For Each glLine As GeometryLine In lines
                vertexes(offset) = New Vertex(glLine.V1, glLine.V1Normal, glLine.Color)
                offset += 1
                vertexes(offset) = New Vertex(glLine.V2, glLine.V2Normal, glLine.Color)
                offset += 1
            Next

            _context.InputAssembler.PrimitiveTopology = Direct3D.PrimitiveTopology.LineList

            Call SendVertexArray(vertexes, 2)
        End If
    End Sub

    Private Sub SendVertexArray(vertexes() As Vertex, grouping As Integer)
        Dim offset As Integer
        Dim length As Integer
        Dim box As DataBox

        Do
            length = Math.Min(vertexes.Length - offset, VERTEX_BUFFER_SIZE)
            length -= length Mod grouping

            box = _context.MapSubresource(_vertexBuffer, 0, MapMode.WriteDiscard, Direct3D11.MapFlags.None)

            WriteStructArrayToIntPtr(vertexes, offset, length, box.DataPointer)
            offset += length

            _context.UnmapSubresource(_vertexBuffer, 0)


            _context.InputAssembler.SetVertexBuffers(0, New VertexBufferBinding(_vertexBuffer, Utilities.SizeOf(Of Vertex)(), 0))
            _context.Draw(length, 0)

        Loop Until offset >= vertexes.Length
    End Sub

    Private Sub WriteStructArrayToIntPtr(Of T As Structure)(array() As T, offset As Integer, length As Integer, dest As IntPtr)
        Dim structSize As Integer = Marshal.SizeOf(Of T)()
        Dim sourceHandle As GCHandle
        Dim sourcePtr As IntPtr

        Try
            sourceHandle = GCHandle.Alloc(array, GCHandleType.Pinned)
            sourcePtr = sourceHandle.AddrOfPinnedObject()

            CopyMemory(dest, sourcePtr + offset * structSize, length * structSize)
        Finally
            sourceHandle.Free()
        End Try
    End Sub

    Public Sub EndFrame()
        _swapChain.Present(0, PresentFlags.None)
    End Sub

    Private Function GetResource(name As String) As Byte()
        Dim assem As Assembly

        assem = Assembly.GetExecutingAssembly()

        Using stream As Stream = assem.GetManifestResourceStream(assem.GetName().Name & "." & name)
            Using msStream As New MemoryStream
                stream.CopyTo(msStream)

                Return msStream.ToArray()
            End Using
        End Using
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Matrices
        Public Model As Matrix
        Public View As Matrix
        Public Proj As Matrix

        Public Sub New(modelValue As Matrix, viewValue As Matrix, projValue As Matrix)
            Model = modelValue
            View = viewValue
            Proj = projValue
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Private Structure LightParams
        Public AmbientLightColor As Color3
        Public Padding1 As Single
        Public DirectionalLightColor As Color3
        Public Padding2 As Single
        Public DirectionalLightDir As Vector3
        Public Padding3 As Single
        Public EnableLighting As Integer
        Public Padding4 As Single
        Public Padding5 As Single
        Public Padding6 As Single

        Public Sub New(ambientLightColorValue As Color3, directionalLightColorValue As Color3, directionalLightDirValue As Vector3, enableLightingValue As Boolean)
            AmbientLightColor = ambientLightColorValue
            DirectionalLightColor = directionalLightColorValue
            DirectionalLightDir = directionalLightDirValue
            EnableLighting = If(enableLightingValue, -1, 0)
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure Vertex
        Public Pos As Vector3
        Public Normal As Vector3
        Public Color As Color4

        Public Sub New(posValue As Vector3, normalValue As Vector3, colorValue As Color4)
            Pos = posValue
            Normal = normalValue
            Color = colorValue
        End Sub

        Public Sub New(posValue As Vector3, normalValue As Vector3, colorValue As Color)
            Pos = posValue
            Normal = normalValue
            Color = New Color4(CSng(colorValue.R) / 255, CSng(colorValue.G) / 255, CSng(colorValue.B) / 255, CSng(colorValue.A) / 255)
        End Sub
    End Structure

#Region "IDisposable Support"
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                _lightParamsBuffer.Dispose()
                _matricesBuffer.Dispose()

                _vertexBuffer.Dispose()
                _shaderInputLayout.Dispose()
                _shaderSignature.Dispose()
                _vertexShaderBytecode.Dispose()
                _vertexShader.Dispose()
                _pixelShaderBytecode.Dispose()
                _pixelShader.Dispose()

                _renderTargetView.Dispose()
                _depthStencilView.Dispose()
                _backBuffer.Dispose()
                _depthBuffer.Dispose()
                _context.ClearState()
                _context.Dispose()
                _device.Dispose()
                _swapChain.Dispose()
            End If
        End If
        _disposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region

    <DllImport("msvcrt.dll", EntryPoint:="memcpy", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub CopyMemory(ByVal dest As IntPtr, ByVal src As IntPtr, ByVal count As Integer)
    End Sub
End Class
