Public Class Diags

    Private plugin As BioShakeVWorksPluginDriver

    Private Sub Diags_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load options into the comm port combo.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            cmboPort.Items.Add(sp)
        Next

        'Load profiles into the profiles combo
        Dim profiles As String() = Profile.GetProfiles()
        For Each pr As String In profiles
            cmboProfile.Items.Add(pr)
        Next

        'Select a particular profile
        If cmboProfile.Items.Count > 0 Then
            cmboProfile.SelectedIndex = 0
            'Firing of cmboProfile.selectedIndexChange should take over from here
        End If

    End Sub

    Private Sub cmboPort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboPort.SelectedIndexChanged
        If plugin.activeProfile IsNot Nothing Then
            plugin.activeProfile.commPort = cmboPort.SelectedItem
            btnUpdateProfile.Enabled = True
        End If
    End Sub

    Private Sub btnNewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewProfile.Click
        Dim newProfileName = InputBox("Please enter name for new profile", "New Profile")
        If newProfileName <> "" Then
            plugin.activeProfile = New Profile
            plugin.activeProfile.name = newProfileName
            plugin.activeProfile.ToRegistry()
            cmboProfile.Items.Add(plugin.activeProfile.name)
            cmboProfile.SelectedIndex = cmboProfile.Items.IndexOf(newProfileName)
        End If
    End Sub

    Private Sub btnDeleteProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteProfile.Click
        If MsgBox("Are you sure?", vbYesNo) = vbYes Then
            Dim profileName = plugin.activeProfile.name
            Profile.DeleteProfile(profileName)
            cmboProfile.Items.Remove(profileName)
            If cmboProfile.Items.Count > 0 Then
                cmboProfile.SelectedIndex = 0
                'Firing of cmboProfile.selectedIndexChange should take over from here
            End If
        End If
    End Sub

    Private Sub btnUpdateProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateProfile.Click
        plugin.activeProfile.ToRegistry()
        btnUpdateProfile.Enabled = False
    End Sub

    Private Sub btnInitializeProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInitializeProfile.Click
        If plugin.activeProfile.initialized Then
            plugin.Close()
        End If

        Dim status = plugin.Initialize("") 'TODO
        If status = IWorksDriver.ReturnCode.RETURN_SUCCESS Then
            btnInitializeProfile.Text = "Reinitialize Profile"
        End If
    End Sub

    Private Sub cmboProfile_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboProfile.SelectedIndexChanged
        plugin.activeProfile = Profile.FromRegistry(cmboProfile.SelectedItem)
        If plugin.activeProfile IsNot Nothing Then
            cmboPort.SelectedIndex = cmboPort.Items.IndexOf(plugin.activeProfile.commPort)
            cmboPort.Enabled = True
            btnUpdateProfile.Enabled = True
            btnDeleteProfile.Enabled = True
        Else
            cmboPort.Enabled = False
            btnUpdateProfile.Enabled = False
            btnDeleteProfile.Enabled = False
        End If
    End Sub

    Public Sub New(ByVal plugin As BioShakeVWorksPluginDriver)
        ' This call is required by the designer.
        InitializeComponent()

        Me.plugin = plugin
    End Sub

    Private Sub btnStart_Click(sender As System.Object, e As System.EventArgs) Handles btnStart.Click
        Dim shakeSpeed = nudSpeed.Value.ToString
        Dim shakeDuration = nudDuration.Value.ToString
        Dim shakeAcceleration = nudAcceleration.Value.ToString

        Dim command As XDocument =
            <?xml version='1.0' encoding='ASCII'?>
            <Velocity11 file='MetaData' version='1.0'>
                <Command Name="Start Shaker">
                    <Parameters>
                        <Parameter Name="Shake Rate" Value=<%= shakeSpeed %>></Parameter>
                        <Parameter Name="Shake Acceleration" Value=<%= shakeAcceleration %>></Parameter>
                        <Parameter Name="Shake Time" Value=<%= shakeDuration %>></Parameter>
                    </Parameters>
                </Command>
            </Velocity11>

        plugin.Command(command.Declaration.ToString() + command.ToString())
    End Sub

    Private Sub btnStop_Click(sender As System.Object, e As System.EventArgs) Handles btnStop.Click
        Dim command As XDocument =
            <?xml version='1.0' encoding='ASCII'?>
            <Velocity11 file='MetaData' version='1.0'>
                <Command Name="Stop Shaker">
                </Command>
            </Velocity11>

        plugin.Command(command.Declaration.ToString() + command.ToString())
    End Sub

    Private Sub btnOpenELM_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenELM.Click
        Dim command As XDocument =
            <?xml version='1.0' encoding='ASCII'?>
            <Velocity11 file='MetaData' version='1.0'>
                <Command Name="Declamp Plate">
                </Command>
            </Velocity11>

        plugin.Command(command.Declaration.ToString() + command.ToString())
    End Sub

    Private Sub btnCloseELM_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseELM.Click
        Dim command As XDocument =
            <?xml version='1.0' encoding='ASCII'?>
            <Velocity11 file='MetaData' version='1.0'>
                <Command Name="Clamp Plate">
                </Command>
            </Velocity11>

        plugin.Command(command.Declaration.ToString() + command.ToString())
    End Sub
End Class