<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Diags
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Diags))
        Me.cmboProfile = New System.Windows.Forms.ComboBox()
        Me.cmboPort = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnUpdateProfile = New System.Windows.Forms.Button()
        Me.btnDeleteProfile = New System.Windows.Forms.Button()
        Me.btnNewProfile = New System.Windows.Forms.Button()
        Me.btnInitializeProfile = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnCloseELM = New System.Windows.Forms.Button()
        Me.btnOpenELM = New System.Windows.Forms.Button()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.nudDuration = New System.Windows.Forms.NumericUpDown()
        Me.nudAcceleration = New System.Windows.Forms.NumericUpDown()
        Me.nudSpeed = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.nudDuration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAcceleration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmboProfile
        '
        Me.cmboProfile.FormattingEnabled = True
        Me.cmboProfile.Location = New System.Drawing.Point(47, 19)
        Me.cmboProfile.Name = "cmboProfile"
        Me.cmboProfile.Size = New System.Drawing.Size(177, 21)
        Me.cmboProfile.TabIndex = 0
        '
        'cmboPort
        '
        Me.cmboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmboPort.FormattingEnabled = True
        Me.cmboPort.Location = New System.Drawing.Point(99, 19)
        Me.cmboPort.Name = "cmboPort"
        Me.cmboPort.Size = New System.Drawing.Size(121, 21)
        Me.cmboPort.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(67, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Port"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Name"
        '
        'btnUpdateProfile
        '
        Me.btnUpdateProfile.Location = New System.Drawing.Point(34, 73)
        Me.btnUpdateProfile.Name = "btnUpdateProfile"
        Me.btnUpdateProfile.Size = New System.Drawing.Size(92, 23)
        Me.btnUpdateProfile.TabIndex = 6
        Me.btnUpdateProfile.Text = "Update Profile"
        Me.btnUpdateProfile.UseVisualStyleBackColor = True
        '
        'btnDeleteProfile
        '
        Me.btnDeleteProfile.Location = New System.Drawing.Point(132, 44)
        Me.btnDeleteProfile.Name = "btnDeleteProfile"
        Me.btnDeleteProfile.Size = New System.Drawing.Size(92, 23)
        Me.btnDeleteProfile.TabIndex = 7
        Me.btnDeleteProfile.Text = "Delete Profile"
        Me.btnDeleteProfile.UseVisualStyleBackColor = True
        '
        'btnNewProfile
        '
        Me.btnNewProfile.Location = New System.Drawing.Point(34, 44)
        Me.btnNewProfile.Name = "btnNewProfile"
        Me.btnNewProfile.Size = New System.Drawing.Size(92, 23)
        Me.btnNewProfile.TabIndex = 8
        Me.btnNewProfile.Text = "New Profile"
        Me.btnNewProfile.UseVisualStyleBackColor = True
        '
        'btnInitializeProfile
        '
        Me.btnInitializeProfile.Location = New System.Drawing.Point(132, 73)
        Me.btnInitializeProfile.Name = "btnInitializeProfile"
        Me.btnInitializeProfile.Size = New System.Drawing.Size(92, 23)
        Me.btnInitializeProfile.TabIndex = 9
        Me.btnInitializeProfile.Text = "Initialize Profile"
        Me.btnInitializeProfile.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmboPort)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(242, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(226, 50)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Profile Settings"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(485, 145)
        Me.TabControl1.TabIndex = 11
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(477, 119)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Profiles"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnInitializeProfile)
        Me.GroupBox2.Controls.Add(Me.cmboProfile)
        Me.GroupBox2.Controls.Add(Me.btnUpdateProfile)
        Me.GroupBox2.Controls.Add(Me.btnDeleteProfile)
        Me.GroupBox2.Controls.Add(Me.btnNewProfile)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(230, 107)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Profile Management"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(477, 119)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Controls"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnCloseELM)
        Me.GroupBox4.Controls.Add(Me.btnOpenELM)
        Me.GroupBox4.Controls.Add(Me.btnStop)
        Me.GroupBox4.Controls.Add(Me.btnStart)
        Me.GroupBox4.Location = New System.Drawing.Point(209, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox4.TabIndex = 12
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Controls"
        '
        'btnCloseELM
        '
        Me.btnCloseELM.Location = New System.Drawing.Point(103, 55)
        Me.btnCloseELM.Name = "btnCloseELM"
        Me.btnCloseELM.Size = New System.Drawing.Size(75, 23)
        Me.btnCloseELM.TabIndex = 3
        Me.btnCloseELM.Text = "Close ELM"
        Me.btnCloseELM.UseVisualStyleBackColor = True
        '
        'btnOpenELM
        '
        Me.btnOpenELM.Location = New System.Drawing.Point(22, 55)
        Me.btnOpenELM.Name = "btnOpenELM"
        Me.btnOpenELM.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenELM.TabIndex = 2
        Me.btnOpenELM.Text = "Open ELM"
        Me.btnOpenELM.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(103, 26)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(75, 23)
        Me.btnStop.TabIndex = 1
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(22, 26)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.nudDuration)
        Me.GroupBox3.Controls.Add(Me.nudAcceleration)
        Me.GroupBox3.Controls.Add(Me.nudSpeed)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(197, 100)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Parameters"
        '
        'nudDuration
        '
        Me.nudDuration.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudDuration.Location = New System.Drawing.Point(127, 66)
        Me.nudDuration.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.nudDuration.Name = "nudDuration"
        Me.nudDuration.Size = New System.Drawing.Size(56, 20)
        Me.nudDuration.TabIndex = 15
        Me.nudDuration.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'nudAcceleration
        '
        Me.nudAcceleration.Location = New System.Drawing.Point(127, 40)
        Me.nudAcceleration.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudAcceleration.Name = "nudAcceleration"
        Me.nudAcceleration.Size = New System.Drawing.Size(56, 20)
        Me.nudAcceleration.TabIndex = 14
        Me.nudAcceleration.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'nudSpeed
        '
        Me.nudSpeed.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudSpeed.Location = New System.Drawing.Point(127, 14)
        Me.nudSpeed.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.nudSpeed.Name = "nudSpeed"
        Me.nudSpeed.Size = New System.Drawing.Size(56, 20)
        Me.nudSpeed.TabIndex = 13
        Me.nudSpeed.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 68)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Duration (seconds)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Acceleration (seconds)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Speed (RPM)"
        '
        'Diags
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(508, 166)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Diags"
        Me.Text = "Diagnostics - Q.Instruments BioShake v1.0.0"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.nudDuration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAcceleration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmboProfile As System.Windows.Forms.ComboBox
    Friend WithEvents cmboPort As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnUpdateProfile As System.Windows.Forms.Button
    Friend WithEvents btnDeleteProfile As System.Windows.Forms.Button
    Friend WithEvents btnNewProfile As System.Windows.Forms.Button
    Friend WithEvents btnInitializeProfile As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCloseELM As System.Windows.Forms.Button
    Friend WithEvents btnOpenELM As System.Windows.Forms.Button
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents nudDuration As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudAcceleration As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSpeed As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
