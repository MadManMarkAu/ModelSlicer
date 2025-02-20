Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

Public Class SettingsContainer
    Private Const SETTINGS_PATH As String = "MadManMarkAu\ModelSlicer"

    Private Shared _instance As SettingsContainer

    Public Shared ReadOnly Property Instance As SettingsContainer
        Get
            If _instance Is Nothing Then
                _instance = Load()
            End If

            Return _instance
        End Get
    End Property

#Region " Settings properties go in this block. "

    Public Property DisplayUnits As DisplayUnit = DisplayUnit.Millimeter

#End Region

    Public Sub Save()
        Dim tempFile As String
        Dim sb As StringBuilder
        Dim serializer As XmlSerializer
        Dim namespaces As XmlSerializerNamespaces

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

        File.WriteAllText(tempFile, sb.ToString())
        File.Copy(tempFile, GetSettingsFile(), True)
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
                    serializer = New XmlSerializer(GetType(SettingsContainer))
                    output = serializer.Deserialize(stream)
                End Using
            Catch
                ' Settings failed to load. Corrupt settings file? Restore defaults.
                output = New SettingsContainer()
            End Try
        Else
            output = New SettingsContainer() ' Load defaults.
        End If

        Return output
    End Function

    Private Shared Function GetSettingsFile() As String
        Return Path.Combine(Application.UserAppDataPath, SETTINGS_PATH, "settings.xml")
    End Function
End Class
