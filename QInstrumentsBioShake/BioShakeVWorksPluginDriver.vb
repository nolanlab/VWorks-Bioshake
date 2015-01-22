Imports System.Windows.Forms
Imports System.IO
Imports System.IO.Ports
Imports System.Threading

Public Class BioShakeVWorksPluginDriver

    Inherits IWorksDriver.CControllerClientClass
    Implements IWorksDriver.IWorksDriver
    Implements IWorksDriver.IWorksDiags

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
        If activeProfile IsNot Nothing And activeProfile.initialized Then ' FIXME
            ' TODO
        End If
    End Sub

    Public Sub Close() Implements IWorksDriver.IWorksDriver.Close
        serialPort.Write("setEcoMode" & vbCr)
        serialPort.Close()
    End Sub

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
                serialPort.Write("soff" & vbCr)
                Thread.Sleep(3000)
                waitForHomed()
            Case "Clamp Plate"
                serialPort.Write("setElmLockPos" & vbCr)
            Case "Declamp Plate"
                serialPort.Write("setElmUnlockPos" & vbCr)
        End Select

        Return status
    End Function

    Private Function shake(ByRef xcommand As XDocument) As IWorksDriver.ReturnCode
        Dim shakeRate As String = ""
        Dim shakeAcceleration As String = ""
        Dim shakeDuration As String = ""

        For Each parameter As XElement In xcommand...<Parameter>
            If parameter.@Name = "Shake Rate" Then
                shakeRate = parameter.@Value
            End If
            If parameter.@Name = "Shake Time" Then
                shakeDuration = parameter.@Value
            End If
            If parameter.@Name = "Shake Acceleration" Then
                shakeAcceleration = parameter.@Value
            End If
        Next

        If shakeRate = "" Then
            errorString = "Shake rate was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        If shakeAcceleration = "" Then
            errorString = "Shake acceleration was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        If shakeDuration = "" Then
            errorString = "Shake duration was not specified."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        serialPort.Write("setElmLockPos" & vbCr)
        serialPort.Write("setShakeTargetSpeed" & shakeRate & vbCr)
        serialPort.Write("shakeOnWithRuntime" & shakeDuration & vbCr)

        ' Ughhhh
        Thread.Sleep(3000)

        waitForHomed()

        Return IWorksDriver.ReturnCode.RETURN_SUCCESS

    End Function

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
        'Verbose indicates if the description goes in the log or under a task icon. Not used here.
        Dim programName As String = ""

        Dim xcommand As XDocument = XDocument.Parse(CommandXML)
        For Each parameter As XElement In xcommand...<Parameter>
            If parameter.@Name = "Program name" Then
                Dim programPath As String = parameter.@Value
                programName = Path.GetFileName(programPath)
            End If
        Next

        If programName = "" Then
            programName = "shake"
        End If

        Return "Run " & programName
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

        Return metadata.Declaration.ToString() + metadata.ToString()
    End Function

    Public Function Ignore(ByVal ErrorContext As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Ignore
        'Any way to ignore? Don't think so.
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function Initialize(ByVal CommandXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.Initialize
        'Dim xcommand As XDocument = XDocument.Parse(CommandXML)
        'For Each parameter As XElement In xcommand...<Parameter>
        '    If parameter.@Name = "Profile" Then
        '        Dim profileName = parameter.@Value
        '        activeProfile = Profile.FromRegistry(profileName)
        '    End If
        'Next

        If activeProfile Is Nothing Then
            errorString = "Error loading profile."
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If

        'Dim returnValue As Runner_ReturnCode

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

        activeProfile.initialized = True

        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function IsLocationAvailable(ByVal LocationAvailableXML As String) As Boolean Implements IWorksDriver.IWorksDriver.IsLocationAvailable
        Return True
    End Function

    Public Function MakeLocationAvailable(ByVal LocationAvailableXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.MakeLocationAvailable
        ' TODO check profile and port
        serialPort.Write("setElmUnlockPos" & vbCr)
        Thread.Sleep(3000)
        Dim returnValue = serialPort.ReadLine()
        If returnValue = "ok" Then
            Return IWorksDriver.ReturnCode.RETURN_SUCCESS
        Else
            Return IWorksDriver.ReturnCode.RETURN_FAIL
        End If
    End Function

    Public Function PlateDroppedOff(ByVal PlateInfoXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.PlateDroppedOff
        Return IWorksDriver.ReturnCode.RETURN_SUCCESS
    End Function

    Public Function PlatePickedUp(ByVal PlateInfoXML As String) As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDriver.PlatePickedUp
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

    Public Function CloseDiagsDialog() As IWorksDriver.ReturnCode Implements IWorksDriver.IWorksDiags.CloseDiagsDialog
        frmDiags.Hide()
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
