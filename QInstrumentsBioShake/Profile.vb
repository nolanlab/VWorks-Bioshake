Public Class Profile
    Public Property name As String = Nothing
    Public Property commPort As String = ""
    Public Property initialized As Boolean = False

    Private Const REG_PATH As String = "SOFTWARE\Velocity11\Q Instruments BioShake\Profiles\"

    Public Function ToRegistry() As Integer
        Try
            Dim hive As String = REG_PATH & name
            My.Computer.Registry.LocalMachine.CreateSubKey(hive)
            My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\" & hive, "Serial port", commPort)
            Return 0
        Catch ex As Exception
            Debug.Print(ex.ToString())
            Return 1
        End Try
    End Function

    Public Shared Function FromRegistry(ByVal name As String) As Profile
        If My.Computer.Registry.LocalMachine.GetValue(REG_PATH, name, Nothing) Is Nothing Then
            Return Nothing
        End If

        Dim newProfile As New Profile
        newProfile.name = name

        If My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\" & REG_PATH & name, "Serial port", Nothing) IsNot Nothing Then
            newProfile.commPort = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\" & REG_PATH & name, "Serial port", Nothing)
        End If

        Return newProfile
    End Function

    Public Shared Function DeleteProfile(ByVal name As String) As Integer
        Try
            My.Computer.Registry.LocalMachine.DeleteSubKey(REG_PATH & name)
            Return 0

        Catch ex As Exception
            Return 1

        End Try
    End Function

    Public Shared Function GetProfiles() As String()
        Dim sProfiles As String()
        My.Computer.Registry.LocalMachine.CreateSubKey(REG_PATH)
        sProfiles = My.Computer.Registry.LocalMachine.OpenSubKey(REG_PATH, False).GetSubKeyNames()

        Return sProfiles
    End Function

End Class
