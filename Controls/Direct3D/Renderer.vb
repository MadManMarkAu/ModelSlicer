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

    Private m_dDevice As Device
    Private m_scdSwapChainDescription As SwapChainDescription
    Private m_scSwapChain As SwapChain
    Private m_tBackBuffer As Texture2D
    Private m_tdDepthBufferDesc As Texture2DDescription
    Private m_tDepthBuffer As Texture2D
    Private m_rtvRenderTargetView As RenderTargetView
    Private m_dsvDepthStencilView As DepthStencilView
    Private m_dcContext As DeviceContext
    Private m_blnDisposed As Boolean

    Private m_crVertexShaderBytecode As CompilationResult
    Private m_vsVertexShader As VertexShader
    Private m_crPixelShaderBytecode As CompilationResult
    Private m_psPixelShader As PixelShader
    Private m_ssShaderSignature As ShaderSignature
    Private m_ilShaderInputLayout As InputLayout

    Private m_bVertexBuffer As Buffer

    Private m_mMatricies As New Matrices(Matrix.Identity, Matrix.Identity, Matrix.Identity)
    Private m_bMatricies As Buffer
    Private m_mModelMatrix As Matrix
    Private m_mViewMatrix As Matrix
    Private m_mProjMatrix As Matrix

    Private m_lpLightParams As New LightParams(New Color3(1, 1, 1), New Color3(0.1, 0.1, 0.1), New Vector3(1, 1, -1), False)
    Private m_bLightParams As Buffer

    Public Property BackColor As Color3 = New Color3(0.25, 0.25, 0.75)

    Public Property AmbientLightColor As Color3
        Get
            Return m_lpLightParams.AmbientLightColor
        End Get
        Set(value As Color3)
            m_lpLightParams.AmbientLightColor = value

            m_dcContext.UpdateSubresource(m_lpLightParams, m_bLightParams)
        End Set
    End Property

    Public Property DirectionalLightColor As Color3
        Get
            Return m_lpLightParams.DirectionalLightColor
        End Get
        Set(value As Color3)
            m_lpLightParams.DirectionalLightColor = value

            m_dcContext.UpdateSubresource(m_lpLightParams, m_bLightParams)
        End Set
    End Property

    Public Property DirectionalLightDir As Vector3
        Get
            Return m_lpLightParams.DirectionalLightDir
        End Get
        Set(value As Vector3)
            m_lpLightParams.DirectionalLightDir = value

            m_dcContext.UpdateSubresource(m_lpLightParams, m_bLightParams)
        End Set
    End Property

    Public Property EnableLighting As Boolean
        Get
            Return m_lpLightParams.EnableLighting <> 0
        End Get
        Set(value As Boolean)
            m_lpLightParams.EnableLighting = If(value, -1, 0)

            m_dcContext.UpdateSubresource(m_lpLightParams, m_bLightParams)
        End Set
    End Property

    Public Property ModelMatrix As Matrix
        Get
            Return m_mModelMatrix
        End Get
        Set(value As Matrix)
            m_mModelMatrix = value
            m_mMatricies.Model = value

            m_mMatricies.Model.Transpose()

            m_dcContext.UpdateSubresource(m_mMatricies, m_bMatricies)
        End Set
    End Property

    Public Property ViewMatrix As Matrix
        Get
            Return m_mViewMatrix
        End Get
        Set(value As Matrix)
            m_mViewMatrix = value
            m_mMatricies.View = value

            m_mMatricies.View.Transpose()

            m_dcContext.UpdateSubresource(m_mMatricies, m_bMatricies)
        End Set
    End Property

    Public Property ProjMatrix As Matrix
        Get
            Return m_mProjMatrix
        End Get
        Set(value As Matrix)
            m_mProjMatrix = value
            m_mMatricies.Proj = value

            m_mMatricies.Proj.Transpose()

            m_dcContext.UpdateSubresource(m_mMatricies, m_bMatricies)
        End Set
    End Property

    Public Sub New(hWnd As IntPtr, intWidth As Integer, intHeight As Integer)
        m_scdSwapChainDescription = New SwapChainDescription() With {
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
            DeviceCreationFlags.Debug,
            New Direct3D.FeatureLevel() {Direct3D.FeatureLevel.Level_11_1, Direct3D.FeatureLevel.Level_11_0, Direct3D.FeatureLevel.Level_10_1, Direct3D.FeatureLevel.Level_10_0},
            m_scdSwapChainDescription, m_dDevice, m_scSwapChain)

        m_dcContext = m_dDevice.ImmediateContext

        m_tBackBuffer = Direct3D11.Resource.FromSwapChain(Of Texture2D)(m_scSwapChain, 0)
        m_rtvRenderTargetView = New RenderTargetView(m_dDevice, m_tBackBuffer)

        m_tdDepthBufferDesc = New Texture2DDescription() With {
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

        Dim dssdDesc As New DepthStencilStateDescription() With {
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

        Dim dssState As New DepthStencilState(m_dDevice, dssdDesc)

        m_tDepthBuffer = New Texture2D(m_dDevice, m_tdDepthBufferDesc)
        m_dsvDepthStencilView = New DepthStencilView(m_dDevice, m_tDepthBuffer)

        m_dcContext.Rasterizer.SetViewport(New RawViewportF() With {.Width = intWidth, .Height = intHeight, .MinDepth = 0F, .MaxDepth = 1.0F})
        m_dcContext.OutputMerger.SetTargets(m_dsvDepthStencilView, m_rtvRenderTargetView)
        m_dcContext.OutputMerger.SetDepthStencilState(dssState)

        'm_crVertexShaderBytecode = ShaderBytecode.CompileFromFile("shader.fx", "VS", "vs_4_0")
        m_crVertexShaderBytecode = ShaderBytecode.Compile(GetResource("shader.fx"), "VS", "vs_4_0")
        m_vsVertexShader = New VertexShader(m_dDevice, m_crVertexShaderBytecode)

        'm_crPixelShaderBytecode = ShaderBytecode.CompileFromFile("shader.fx", "PS", "ps_4_0")
        m_crPixelShaderBytecode = ShaderBytecode.Compile(GetResource("shader.fx"), "PS", "ps_4_0")
        m_psPixelShader = New PixelShader(m_dDevice, m_crPixelShaderBytecode)

        m_ssShaderSignature = ShaderSignature.GetInputSignature(m_crVertexShaderBytecode)
        m_ilShaderInputLayout = New InputLayout(
            m_dDevice,
            m_ssShaderSignature,
            New InputElement() {
                New InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                New InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                New InputElement("COLOR", 0, Format.R32G32B32A32_Float, 24, 0)
            })

        m_dcContext.InputAssembler.InputLayout = m_ilShaderInputLayout

        ' Create vertex buffer
        m_bVertexBuffer = New Buffer(m_dDevice, Utilities.SizeOf(Of Vertex)() * VERTEX_BUFFER_SIZE, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, Utilities.SizeOf(Of Vertex)())

        ' Create constant buffers
        m_bMatricies = New Buffer(m_dDevice, Utilities.SizeOf(Of Matrices)(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0)
        m_bLightParams = New Buffer(m_dDevice, Utilities.SizeOf(Of LightParams)(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0)

        ' Assign the matrices buffer
        m_dcContext.UpdateSubresource(m_mMatricies, m_bMatricies)
        m_dcContext.VertexShader.SetConstantBuffer(0, m_bMatricies)
        m_dcContext.PixelShader.SetConstantBuffer(0, m_bMatricies)

        ' Assign the lighting params buffer
        m_dcContext.UpdateSubresource(m_lpLightParams, m_bLightParams)
        m_dcContext.VertexShader.SetConstantBuffer(1, m_bLightParams)
        m_dcContext.PixelShader.SetConstantBuffer(1, m_bLightParams)

        m_dcContext.VertexShader.Set(m_vsVertexShader)
        m_dcContext.PixelShader.Set(m_psPixelShader)
    End Sub

    Public Sub Resize(intWidth As Integer, intHeight As Integer)
        m_dsvDepthStencilView.Dispose()
        m_tDepthBuffer.Dispose()
        m_rtvRenderTargetView.Dispose()
        m_tBackBuffer.Dispose()

        m_scSwapChain.ResizeBuffers(m_scdSwapChainDescription.BufferCount, intWidth, intHeight, Format.Unknown, SwapChainFlags.None)

        m_tBackBuffer = Direct3D11.Resource.FromSwapChain(Of Texture2D)(m_scSwapChain, 0)
        m_rtvRenderTargetView = New RenderTargetView(m_dDevice, m_tBackBuffer)

        m_tdDepthBufferDesc.Width = intWidth
        m_tdDepthBufferDesc.Height = intHeight

        m_tDepthBuffer = New Texture2D(m_dDevice, m_tdDepthBufferDesc)
        m_dsvDepthStencilView = New DepthStencilView(m_dDevice, m_tDepthBuffer)

        m_dcContext.Rasterizer.SetViewport(0, 0, intWidth, intHeight, 0F, 1.0F)
        m_dcContext.OutputMerger.SetTargets(m_dsvDepthStencilView, m_rtvRenderTargetView)
    End Sub

    Public Sub BeginFrame()
        m_dcContext.ClearDepthStencilView(m_dsvDepthStencilView, DepthStencilClearFlags.Depth, 1.0F, 0)
        m_dcContext.ClearRenderTargetView(m_rtvRenderTargetView, New RawColor4(BackColor.R, BackColor.G, BackColor.B, 1.0))
    End Sub

    Public Sub ClearDepthBuffer()
        m_dcContext.ClearDepthStencilView(m_dsvDepthStencilView, DepthStencilClearFlags.Depth, 1.0F, 0)
    End Sub

    Public Sub DrawTriangles(agtTriangles() As GeometryTriangle)
        If agtTriangles.Length > 0 Then
            Dim avVertexes(agtTriangles.Length * 3 - 1) As Vertex
            Dim intOffset As Integer

            For Each gtTriangle As GeometryTriangle In agtTriangles
                avVertexes(intOffset) = New Vertex(gtTriangle.V1, gtTriangle.V1Normal, gtTriangle.Color)
                intOffset += 1
                avVertexes(intOffset) = New Vertex(gtTriangle.V2, gtTriangle.V2Normal, gtTriangle.Color)
                intOffset += 1
                avVertexes(intOffset) = New Vertex(gtTriangle.V3, gtTriangle.V3Normal, gtTriangle.Color)
                intOffset += 1
            Next

            m_dcContext.InputAssembler.PrimitiveTopology = Direct3D.PrimitiveTopology.TriangleList

            Call SendVertexArray(avVertexes, 3)
        End If
    End Sub

    Public Sub DrawLines(aglLines() As GeometryLine)
        If aglLines.Length > 0 Then
            Dim avVertexes(aglLines.Length * 2 - 1) As Vertex
            Dim intOffset As Integer

            For Each glLine As GeometryLine In aglLines
                avVertexes(intOffset) = New Vertex(glLine.V1, glLine.V1Normal, glLine.Color)
                intOffset += 1
                avVertexes(intOffset) = New Vertex(glLine.V2, glLine.V2Normal, glLine.Color)
                intOffset += 1
            Next

            m_dcContext.InputAssembler.PrimitiveTopology = Direct3D.PrimitiveTopology.LineList

            Call SendVertexArray(avVertexes, 2)
        End If
    End Sub

    Private Sub SendVertexArray(avVertexes() As Vertex, intGrouping As Integer)
        Dim intOffset As Integer
        Dim intLength As Integer
        Dim dbBox As DataBox

        Do
            intLength = Math.Min(avVertexes.Length - intOffset, VERTEX_BUFFER_SIZE)
            intLength -= intLength Mod intGrouping

            dbBox = m_dcContext.MapSubresource(m_bVertexBuffer, 0, MapMode.WriteDiscard, Direct3D11.MapFlags.None)

            WriteStructArrayToIntPtr(avVertexes, intOffset, intLength, dbBox.DataPointer)
            intOffset += intLength

            m_dcContext.UnmapSubresource(m_bVertexBuffer, 0)


            m_dcContext.InputAssembler.SetVertexBuffers(0, New VertexBufferBinding(m_bVertexBuffer, Utilities.SizeOf(Of Vertex)(), 0))
            m_dcContext.Draw(intLength, 0)

        Loop Until intOffset >= avVertexes.Length
    End Sub

    Private Sub WriteStructArrayToIntPtr(Of T As Structure)(atArray() As T, intOffset As Integer, intLength As Integer, ipDest As IntPtr)
        Dim intStructSize As Integer = Marshal.SizeOf(Of T)()
        Dim gchSource As GCHandle
        Dim ipSource As IntPtr

        Try
            gchSource = GCHandle.Alloc(atArray, GCHandleType.Pinned)
            ipSource = gchSource.AddrOfPinnedObject()

            CopyMemory(ipDest, ipSource + intOffset * intStructSize, intLength * intStructSize)
        Finally
            gchSource.Free()
        End Try
    End Sub

    Public Sub EndFrame()
        m_scSwapChain.Present(0, PresentFlags.None)
    End Sub

    Private Function GetResource(strName As String) As Byte()
        Dim aAssem As Assembly

        aAssem = Assembly.GetExecutingAssembly()

        Using sStream As Stream = aAssem.GetManifestResourceStream(aAssem.GetName().Name & "." & strName)
            Using msStream As New MemoryStream
                sStream.CopyTo(msStream)

                Return msStream.ToArray()
            End Using
        End Using
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Matrices
        Public Model As Matrix
        Public View As Matrix
        Public Proj As Matrix

        Public Sub New(mModel As Matrix, mView As Matrix, mProj As Matrix)
            Model = mModel
            View = mView
            Proj = mProj
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

        Public Sub New(cAmbientLightColor As Color3, cDirectionalLightColor As Color3, vDirectionalLightDir As Vector3, blnEnableLighting As Boolean)
            AmbientLightColor = cAmbientLightColor
            DirectionalLightColor = cDirectionalLightColor
            DirectionalLightDir = vDirectionalLightDir
            EnableLighting = If(blnEnableLighting, -1, 0)
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure Vertex
        Public Pos As Vector3
        Public Normal As Vector3
        Public Color As Color4

        Public Sub New(vPos As Vector3, vNormal As Vector3, cColor As Color4)
            Pos = vPos
            Normal = vNormal
            Color = cColor
        End Sub

        Public Sub New(vPos As Vector3, vNormal As Vector3, cColor As Color)
            Pos = vPos
            Normal = vNormal
            Color = New Color4(CSng(cColor.R) / 255, CSng(cColor.G) / 255, CSng(cColor.B) / 255, CSng(cColor.A) / 255)
        End Sub
    End Structure

#Region "IDisposable Support"
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not m_blnDisposed Then
            If disposing Then
                m_bLightParams.Dispose()
                m_bMatricies.Dispose()

                m_bVertexBuffer.Dispose()
                m_ilShaderInputLayout.Dispose()
                m_ssShaderSignature.Dispose()
                m_crVertexShaderBytecode.Dispose()
                m_vsVertexShader.Dispose()
                m_crPixelShaderBytecode.Dispose()
                m_psPixelShader.Dispose()

                m_rtvRenderTargetView.Dispose()
                m_dsvDepthStencilView.Dispose()
                m_tBackBuffer.Dispose()
                m_tDepthBuffer.Dispose()
                m_dcContext.ClearState()
                m_dcContext.Dispose()
                m_dDevice.Dispose()
                m_scSwapChain.Dispose()
            End If
        End If
        m_blnDisposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region

    <DllImport("msvcrt.dll", EntryPoint:="memcpy", CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Sub CopyMemory(ByVal dest As IntPtr, ByVal src As IntPtr, ByVal count As Integer)
    End Sub
End Class
