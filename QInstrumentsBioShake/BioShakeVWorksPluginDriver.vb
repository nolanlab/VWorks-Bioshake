﻿Imports System.Windows.Forms
Imports System.IO
Imports System.IO.Ports
Imports System.Threading

Public Class BioShakeVWorksPluginDriver

    Inherits IWorksDriver.CControllerClientClass
    Implements IWorksDriver.IWorksDriver
    Implements IWorksDriver.IWorksDiags

    Private Structure shakeParams
        Public speed As String
        Public duration As String
        Public acceleration As String
    End Structure

    Private Enum ShakeState
        Active = 0
        Stopping = 1
        Breaking = 2
        Homed = 3
        Manual = 4
        Accelerating = 5
        Decelerating = 6
        DeceleratingWithStopping = 7
        EcoMode = 90
        Booting = 99
    End Enum

    Private controllerInstance As IWorksDriver.CWorksController

    Private errorString As String = ""

    Friend activeProfile As Profile
    Dim serialPort As SerialPort = New SerialPort()

    Private WithEvents frmDiags As Diags = New Diags(Me)

    Public Sub Abort(ByVal ErrorContext As String) Implements IWorksDriver.IWorksDriver.Abort
        If activeProfile IsNot Nothing AndAlso activeProfile.initialized Then
            stopShake()
            serialPort.Write("setElmUnlockPos" & vbCr)
        End If
    End Sub

    Public Sub Close() Implements IWorksDriver.IWorksDriver.Close
        ' TODO if initialized
        serialPort.Write("setEcoMode" & vbCr)
        serialPort.Close()
    End Sub

    Private Function commandToShakeParams(ByRef xcommand As XDocument) As shakeParams
        Dim result As New shakeParams

        For Each parameter As XElement In xcommand...<Parameter>
            If parameter.@Name = "Shake Rate" Then
                result.speed = parameter.@Value
            End If
            If parameter.@Name = "Shake Time" Then
                result.duration = parameter.@Value
            End If
            If parameter.@Name = "Shake Acceleration" Then
                result.acceleration = parameter.@Value
            End If
        Next

        Return result
    End Function

    Public Function Command(ByVal CommandXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Command
        If activeProfile Is Nothing Or activeProfile.initialized = False Then
            errorString = "Device is not initialized."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        Dim xcommand As XDocument = XDocument.Parse(CommandXML)
        Dim commandName = xcommand...<Command>.@Name

        Dim status As IWorksDriver.ReturnCode = IWorksDriver.ReturnCode.RETURN_SUCCESS

        Select Case commandName
            Case "Start Shaker"
                status = shake(xcommand)
                serialPort.Write("setElmUnlockPos" & vbCr)
            Case "Stop Shaker"
                stopShake()
                serialPort.Write("setElmUnlockPos" & vbCr)
            Case "Clamp Plate"
                serialPort.Write("setElmLockPos" & vbCr)
            Case "Declamp Plate"
                serialPort.Write("setElmUnlockPos" & vbCr)
        End Select

        Return status
    End Function

    Private Sub stopShake()
        ' TODO if initialized
        serialPort.Write("soff" & vbCr) ' FIXME the port is closed
        Thread.Sleep(3000)
        waitForHomed()
    End Sub

    Private Function shake(ByRef xcommand As XDocument) As IWorksDriver.ReturnCode
        Dim params As shakeParams = commandToShakeParams(xcommand)

        If params.speed = "" Then
            errorString = "Shake rate was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        If params.acceleration = "" Then
            errorString = "Shake acceleration was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        If params.duration = "" Then
            errorString = "Shake duration was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        serialPort.Write("setElmLockPos" & vbCr)
        serialPort.Write("setShakeTargetSpeed" & params.speed & vbCr)
        serialPort.Write("shakeOnWithRuntime" & params.duration & vbCr)

        ' Ughhhh
        Thread.Sleep(3000)

        waitForHomed()

        Return IWorksDriver.ReturnCode.RETURN_SUCCESS

    End Function

    ' Waits for the shaker to come to the home position.
    Private Sub waitForHomed()
        serialPort.DiscardInBuffer()

        While True
            serialPort.Write("gsst" & vbCr)
            Thread.Sleep(100)
            Dim statusCode As String = serialPort.ReadLine()
            Dim message As ShakeState = Integer.Parse(statusCode)
            If message = ShakeState.Homed Then
                Return
            Else
                Thread.Sleep(1000)
            End If
        End While
    End Sub

    Public Function Compile(ByVal iCompileType As IWorksDriver.CompileType, ByVal MetaDataXML As String) As String Implements IWorksDriver.IWorksDriver.Compile
        'No errors or warnings to report, basically ever.
        Return ""
    End Function

    Public Function ControllerQuery(ByVal Query As String) As String Implements IWorksDriver.IWorksDriver.ControllerQuery
        'Not implemented
        Return ""
    End Function

    Public Function Get32x32Bitmap(ByVal CommandName As String) As stdole.IPictureDisp Implements IWorksDriver.IWorksDriver.Get32x32Bitmap
        Dim img As System.Drawing.Bitmap = My.Resources.icon
        Return(Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPictureDisp(img))
    End Function

    Public Function GetDescription(ByVal CommandXML As String, ByVal Verbose As Boolean) As String Implements IWorksDriver.IWorksDriver.GetDescription
        'Verbose indicates if the description goes in the log or under a task icon.
        Dim xcommand As XDocument = XDocument.Parse(CommandXML)
        Dim params As shakeParams = commandToShakeParams(xcommand)
        If params.speed <> "" AndAlso params.duration <> "" Then
            Return "Shake at " & params.speed & " RPMs, " & params.duration & " s"
        Else
            Return "Shake"
        End If
    End Function

    Public Function GetErrorInfo() As String Implements IWorksDriver.IWorksDriver.GetErrorInfo
        Return errorString
    End Function

    Public Function GetLayoutBitmap(ByVal LayoutInfoXML As String) As stdole.IPictureDisp Implements IWorksDriver.IWorksDriver.GetLayoutBitmap
        'Not implemented or necessary
        Return Nothing
    End Function

    Public Function GetMetaData(ByVal iDataType As IWorksDriver.MetaDataType, ByVal current_metadata As String) As String Implements IWorksDriver.IWorksDriver.GetMetaData
        Dim profilesNames As String() = Profile.GetProfiles()

        Dim metadata As XDocument =
            <?xml version='1.0' encoding='ASCII'?>
            <Velocity11 file='MetaData' version='1.0'>
                <MetaData>
                    <Device Description='Q.Instruments BioShake' DynamicLocations='0' MiscAttributes='0' HasBarcodeReader='0' HardwareManufacturer='QUATIFOIL Instruments' Name='Q.Instruments BioShake' PreferredTab='Plate Handling' RegistryName='Q Instruments BioShake\Profiles'>
                        <Parameters>
                            <Parameter Name='Profile' Style='0' Type='2'>
                                <Ranges>
                                    <%= From pn In profilesNames
                                        Select <Range Value=<%= pn %>/>
                                    %>
                                </Ranges>
                            </Parameter>
                        </Parameters>
                        <Locations>
                            <Location Name='Stage' Type='1'/>
                        </Locations>
                    </Device>
                    <Versions>
                        <Version Author='zbjornson' Date='January 20, 2015' Name='Q.Instruments BioShake' Version='1.0.0'/>
                    </Versions>
                    <Commands>
                        <Command Compiler='0' Description='Start Shaking Plate' Editor='10' Name='Start Shaker' NextTaskToExecute='1' RequiresRefresh='0' TaskRequiresLocation='1' VisibleAvailability='1'>
                            <Parameters>
                                <Parameter Description='Set the target mixing speed. For 3000 series allowable range is 0 - 3,000 rpm and for 5000 series allowable range is 0 - 5000 rpm' Name='Shake Rate' Scriptable='1' Style='0' Type='8' Units='rpm' Value='250'>
                                    <Ranges>
                                        <Range Value='0'/>
                                        <Range Value='5000'/>
                                    </Ranges>
                                </Parameter>
                                <Parameter Description='Set the acceleration/deceleration value in seconds. Allowable range is 0 - 10 seconds' Name='Shake Acceleration' Scriptable='1' Style='0' Type='8' Units='sec' Value='5'>
                                    <Ranges>
                                        <Range Value='0'/>
                                        <Range Value='10'/>
                                    </Ranges>
                                </Parameter>
                                <Parameter Description='Set the shake time with the current mixing speed and acceleration in seconds. Allowable range: 0 - 99,999 seconds. If this parameter value is 0, then the shaker will shake indefinitely until a stop command is issued.' Name='Shake Time' Scriptable='1' Style='0' Type='8' Units='sec' Value='120'>
                                    <Ranges>
                                        <Range Value='0'/>
                                        <Range Value='99999'/>
                                    </Ranges>
                                </Parameter>
                                <Parameter Description='Set the shake temperature. Allowable range: 0 - 99.0 deg C. Please note that this parameter is only valid for models with temperature control enabled, otherwise this parameter will be ignored.' Name='Shake Temperature' Scriptable='1' Style='0' Type='12' Units='deg C' Value='24.5'>
                                    <Ranges>
                                        <Range Value='0'/>
                                        <Range Value='99.0'/>
                                    </Ranges>
                                </Parameter>
                            </Parameters>
                        </Command>
                        <Command Compiler='0' Description='Stop Shaking Plate' Editor='10' Name='Stop Shaker' NextTaskToExecute='1' RequiresRefresh='0' TaskRequiresLocation='1' VisibleAvailability='1'/>
                        <Command Compiler='0' Description='Clamps the plate on the shaker' Editor='10' Name='Clamp Plate' NextTaskToExecute='1' RequiresRefresh='0' TaskRequiresLocation='1' VisibleAvailability='1'/>
                        <Command Compiler='0' Description='Declamps the plate on the shaker' Editor='10' Name='Declamp Plate' NextTaskToExecute='1' RequiresRefresh='0' TaskRequiresLocation='1' VisibleAvailability='1'/>
                    </Commands>
                </MetaData>
            </Velocity11>

        ' TODO: I'd like to remove the Clamp Plate, Declamp Plate and Stop Shaker commands. Why would you use them?

        Return metadata.Declaration.ToString() + metadata.ToString()
    End Function

    Public Function Ignore(ByVal ErrorContext As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Ignore
        'Any way to ignore? Don't think so.
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function Initialize(ByVal CommandXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Initialize
        If activeProfile IsNot Nothing AndAlso activeProfile.initialized = True Then
            Return IWorksDriver.ReturnCode.RETURN_SUCCESS
        End If

        Dim xcommand As XDocument = XDocument.Parse(CommandXML)
        For Each parameter As XElement In xcommand...<Parameter>
            If parameter.@Name = "Profile" Then
                Dim profileName = parameter.@Value
                activeProfile = Profile.FromRegistry(profileName)
            End If
        Next

        If activeProfile Is Nothing Then
            errorString = "Error loading profile."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        serialPort.PortName = activeProfile.commPort
        serialPort.BaudRate = 9600
        serialPort.Parity = Parity.None
        serialPort.DataBits = 8
        serialPort.StopBits = StopBits.One
        serialPort.Handshake = Handshake.None

        serialPort.ReadTimeout = 10000
        serialPort.WriteTimeout = 10000

        serialPort.Open()

        serialPort.NewLine = vbCrLf

        serialPort.Write("shakeGoHome" & vbCr)

        ' Ughhhh
        Thread.Sleep(3000)

        waitForHomed()

        activeProfile.initialized = True

        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function IsLocationAvailable(ByVal LocationAvailableXML As String) As Boolean Implements IWorksDriver.IWorksDriver.IsLocationAvailable
        ' See API docs for why this is basically always true.
        Return True
    End Function

    Public Function MakeLocationAvailable(ByVal LocationAvailableXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.MakeLocationAvailable
        If activeProfile IsNot Nothing And activeProfile.initialized Then
            serialPort.Write("setElmUnlockPos" & vbCr)
            Thread.Sleep(3000)
            Dim returnValue = serialPort.ReadLine()
            If returnValue = "ok" Then
                Return IWorksDriver.ReturnCode.RETURN_SUCCESS
            Else
                Return IWorksDriver.ReturnCode.RETURN_FAIL
            End If
        Else
            errorString = "Shaker not initialized."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If
    End Function

    Public Function PlateDroppedOff(ByVal PlateInfoXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.PlateDroppedOff
        ' API docs, wat?
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function PlatePickedUp(ByVal PlateInfoXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.PlatePickedUp
        serialPort.Write("setElmLockPos" & vbCr)
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Sub PlateTransferAborted(ByVal PlateInfoXML As String) Implements IWorksDriver.IWorksDriver.PlateTransferAborted

    End Sub

    Public Function PrepareForRun(ByVal LocationInfoXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.PrepareForRun
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function Retry(ByVal ErrorContext As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Retry
        'TODO
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    'Formsy stuff. Would be nicer in Diags.vb but not sure how to make VWorks find the pointer to the class implementing IWorksDriver.IWorksDiags.
    Private Sub DiagsForm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles frmDiags.FormClosed
        controllerInstance.OnCloseDiagsDialog(Me)
    End Sub

    Private Sub Diags_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles frmDiags.FormClosing
        ' Do not dispose of the diagnostics form if closed by the (x) button.
        MsgBox("closing")
        If e.CloseReason = Windows.Forms.CloseReason.UserClosing Then
            e.Cancel = True
            controllerInstance.OnCloseDiagsDialog(Me)
            frmDiags.Hide()
        Else
            MsgBox("Not true")
        End If
    End Sub

    Public Function CloseDiagsDialog() As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDiags.CloseDiagsDialog
        frmDiags.Hide()
        controllerInstance.OnCloseDiagsDialog(Me)
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function IsDiagsDialogOpen() As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDiags.IsDiagsDialogOpen
        Return frmDiags.Visible
    End Function

    Public Sub ShowDiagsDialog(ByVal iSecurity As IWorksDriver.SecurityLevel) Implements IWorksDriver.IWorksDriver.ShowDiagsDialog
        ' This is obsolete, says the docs, except it's still used.
        frmDiags.Show()
    End Sub

    Public Sub ShowDiagsDialog(ByVal iSecurity As IWorksDriver.SecurityLevel, ByVal bModal As Boolean) Implements IWorksDriver.IWorksDiags.ShowDiagsDialog
        frmDiags.Show()
    End Sub

    Public Overrides Sub SetController(ByVal Controller As IWorksDriver.CWorksController)
        controllerInstance = Controller
    End Sub
End Class
