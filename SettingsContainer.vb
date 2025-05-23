﻿Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

<Serializable>
<XmlRoot("Settings")>
Public Class SettingsContainer
    Private Const SETTINGS_PATH As String = "MadManMarkAu\ModelSlicer"

    Private Shared __instance As SettingsContainer
    Private Shared __encoding As New UTF8Encoding(False)

    Public Shared ReadOnly Property Instance As SettingsContainer
        Get
            If __instance Is Nothing Then
                __instance = Load()
            End If

            Return __instance
        End Get
    End Property

#Region " Settings properties go in this block. "

    ' Display settings.

    Public Property DisplayUnits As DisplayUnit = DisplayUnit.Millimeter

    ' Import settings.

    Public Property ImportLastOpenDir As String = String.Empty
    Public Property ImportUseDefaults As Boolean = False
    Public Property ImportDefaultUnits As Unit = Unit.Millimeter
    Public Property ImportDefaultUpAxis As Axis = Axis.Y

    ' SVG export settings.

    Public Property ExportSvgLastExportDir As String = String.Empty

    Public Property ExportSvgUseDefaults As Boolean = False

    Public Property ExportSvgIncludeTop As Boolean = True

    Public Property ExportSvgIncludeFill As Boolean = True

    Public Property ExportSvgIncludeBottom As Boolean = True

    <XmlIgnore> ' Do not store this property directly.
    Public Property ExportSvgColorTop As Color = Color.Red

    <XmlIgnore> ' Do not store this property directly.
    Public Property ExportSvgColorFill As Color = Color.Gray

    <XmlIgnore> ' Do not store this property directly.
    Public Property ExportSvgColorBottom As Color = Color.Blue

#End Region

#Region " Serialization proxies "
    ' Proxies will allow serialization of types that do not directly support serialization, by replacing the actual property that gets serialized.

    <XmlElement("ExportSvgColorTop")>
    Public Property Proxy_ExportSvgColorTop As String
        Get
            Return $"{ExportSvgColorTop.R:X2}{ExportSvgColorTop.G:X2}{ExportSvgColorTop.B:X2}"
        End Get
        Set(value As String)
            Dim colorInt As Integer

            If Integer.TryParse(value, NumberStyles.HexNumber, Nothing, colorInt) Then
                ExportSvgColorTop = Color.FromArgb(colorInt Or &HFF000000) ' Force full opacity
            End If
        End Set
    End Property

    <XmlElement("ExportSvgColorFill")>
    Public Property Proxy_ExportSvgColorFill As String
        Get
            Return $"{ExportSvgColorFill.R:X2}{ExportSvgColorFill.G:X2}{ExportSvgColorFill.B:X2}"
        End Get
        Set(value As String)
            Dim colorInt As Integer

            If Integer.TryParse(value, NumberStyles.HexNumber, Nothing, colorInt) Then
                ExportSvgColorFill = Color.FromArgb(colorInt Or &HFF000000) ' Force full opacity
            End If
        End Set
    End Property

    <XmlElement("ExportSvgColorBottom")>
    Public Property Proxy_ExportSvgColorBottom As String
        Get
            Return $"{ExportSvgColorBottom.R:X2}{ExportSvgColorBottom.G:X2}{ExportSvgColorBottom.B:X2}"
        End Get
        Set(value As String)
            Dim colorInt As Integer

            If Integer.TryParse(value, NumberStyles.HexNumber, Nothing, colorInt) Then
                ExportSvgColorBottom = Color.FromArgb(colorInt Or &HFF000000) ' Force full opacity
            End If
        End Set
    End Property
#End Region

    Public Sub Save()
        Dim tempFile As String
        Dim sb As StringBuilder
        Dim serializer As XmlSerializer
        Dim namespaces As XmlSerializerNamespaces
        Dim settingsFile As String

        ' We write to a temp file first, because in very rare instances,
        ' writing directly to the settings file can corrupt the file,
        ' whereas a File.Copy() does not seem to corrupt the file.
        ' Weird, I know...

        tempFile = Path.GetTempFileName()

        sb = New StringBuilder()
        Using writer As New StringWriter(sb)
            ' Hack to remove extra namespaces from generated XML document.
            namespaces = New XmlSerializerNamespaces()
            namespaces.Add(String.Empty, String.Empty)

            serializer = New XmlSerializer(GetType(SettingsContainer))
            serializer.Serialize(writer, Me, namespaces)
        End Using

        settingsFile = GetSettingsFile()

        If Not Directory.Exists(Path.GetDirectoryName(settingsFile)) Then
            Directory.CreateDirectory(Path.GetDirectoryName(settingsFile))
        End If

        File.WriteAllText(tempFile, sb.ToString(), __encoding)
        File.Copy(tempFile, settingsFile, True)
        File.Delete(tempFile)
    End Sub

    Private Shared Function Load() As SettingsContainer
        Dim output As SettingsContainer
        Dim fileName As String
        Dim serializer As XmlSerializer

        fileName = GetSettingsFile()

        If File.Exists(fileName) Then
            ' Load settings XML file.
            Try
                Using stream As New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
                    Using reader As New StreamReader(stream, __encoding)
                        serializer = New XmlSerializer(GetType(SettingsContainer))
                        output = serializer.Deserialize(reader)
                    End Using
                End Using
            Catch ex As Exception
                ' Settings failed to load. Corrupt settings file? Restore defaults.
                output = New SettingsContainer()
            End Try
        Else
            output = New SettingsContainer() ' Load defaults.
        End If

        Return output
    End Function

    Private Shared Function GetSettingsFile() As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SETTINGS_PATH, "settings.xml")
    End Function
End Class
