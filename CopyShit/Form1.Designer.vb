<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.KbpsBox = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BlockNumBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BlockCountBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SinceLastBlockBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.TimeLeftBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OverCheck = New System.Windows.Forms.CheckBox()
        Me.ExistCheck = New System.Windows.Forms.CheckBox()
        Me.ValidCheck = New System.Windows.Forms.CheckBox()
        Me.RedCancelButton = New System.Windows.Forms.Button()
        Me.ProgBarBgPanel = New System.Windows.Forms.Panel()
        Me.ProgBarFgPanel = New System.Windows.Forms.Panel()
        Me.ElapsedBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PercentBox = New System.Windows.Forms.TextBox()
        Me.ProgBarBgPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.RestoreDirectory = True
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        Me.OpenFileDialog1.Title = "FILE TO READ"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(86, 12)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(472, 23)
        Me.TextBox1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(67, 25)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "READ"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 43)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(67, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "WRITE"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(87, 44)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(471, 23)
        Me.TextBox2.TabIndex = 3
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.LawnGreen
        Me.Button3.Font = New System.Drawing.Font("Segoe UI", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.Button3.Location = New System.Drawing.Point(87, 73)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(201, 60)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "COPY"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(10, 271)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(545, 70)
        Me.TextBox3.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 15)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Avg Kbps"
        '
        'KbpsBox
        '
        Me.KbpsBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.KbpsBox.Location = New System.Drawing.Point(66, 194)
        Me.KbpsBox.Name = "KbpsBox"
        Me.KbpsBox.ReadOnly = True
        Me.KbpsBox.Size = New System.Drawing.Size(110, 22)
        Me.KbpsBox.TabIndex = 10
        Me.KbpsBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'BlockNumBox
        '
        Me.BlockNumBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BlockNumBox.Location = New System.Drawing.Point(306, 143)
        Me.BlockNumBox.Name = "BlockNumBox"
        Me.BlockNumBox.ReadOnly = True
        Me.BlockNumBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BlockNumBox.Size = New System.Drawing.Size(85, 22)
        Me.BlockNumBox.TabIndex = 11
        Me.BlockNumBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(216, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 15)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Copying Block"
        '
        'BlockCountBox
        '
        Me.BlockCountBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.BlockCountBox.Location = New System.Drawing.Point(306, 171)
        Me.BlockCountBox.Name = "BlockCountBox"
        Me.BlockCountBox.ReadOnly = True
        Me.BlockCountBox.Size = New System.Drawing.Size(85, 22)
        Me.BlockCountBox.TabIndex = 13
        Me.BlockCountBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(257, 174)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 15)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Out Of"
        '
        'SinceLastBlockBox
        '
        Me.SinceLastBlockBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.SinceLastBlockBox.Location = New System.Drawing.Point(306, 199)
        Me.SinceLastBlockBox.Name = "SinceLastBlockBox"
        Me.SinceLastBlockBox.ReadOnly = True
        Me.SinceLastBlockBox.Size = New System.Drawing.Size(85, 22)
        Me.SinceLastBlockBox.TabIndex = 15
        Me.SinceLastBlockBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(222, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 15)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "ms Since Last"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.RestoreDirectory = True
        Me.SaveFileDialog1.SupportMultiDottedExtensions = True
        '
        'TimeLeftBox
        '
        Me.TimeLeftBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.TimeLeftBox.Location = New System.Drawing.Point(65, 167)
        Me.TimeLeftBox.Name = "TimeLeftBox"
        Me.TimeLeftBox.ReadOnly = True
        Me.TimeLeftBox.Size = New System.Drawing.Size(111, 22)
        Me.TimeLeftBox.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 167)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 15)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Time Left"
        '
        'OverCheck
        '
        Me.OverCheck.AutoSize = True
        Me.OverCheck.Location = New System.Drawing.Point(12, 93)
        Me.OverCheck.Name = "OverCheck"
        Me.OverCheck.Size = New System.Drawing.Size(77, 19)
        Me.OverCheck.TabIndex = 19
        Me.OverCheck.Text = "Overwrite"
        Me.OverCheck.UseVisualStyleBackColor = True
        '
        'ExistCheck
        '
        Me.ExistCheck.AutoSize = True
        Me.ExistCheck.Enabled = False
        Me.ExistCheck.Location = New System.Drawing.Point(12, 72)
        Me.ExistCheck.Name = "ExistCheck"
        Me.ExistCheck.Size = New System.Drawing.Size(55, 19)
        Me.ExistCheck.TabIndex = 20
        Me.ExistCheck.Text = "Exists"
        Me.ExistCheck.UseVisualStyleBackColor = True
        '
        'ValidCheck
        '
        Me.ValidCheck.AutoSize = True
        Me.ValidCheck.Location = New System.Drawing.Point(12, 114)
        Me.ValidCheck.Name = "ValidCheck"
        Me.ValidCheck.Size = New System.Drawing.Size(67, 19)
        Me.ValidCheck.TabIndex = 21
        Me.ValidCheck.Text = "Validate"
        Me.ValidCheck.UseVisualStyleBackColor = True
        '
        'RedCancelButton
        '
        Me.RedCancelButton.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.RedCancelButton.Enabled = False
        Me.RedCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RedCancelButton.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.RedCancelButton.ForeColor = System.Drawing.Color.LightYellow
        Me.RedCancelButton.Location = New System.Drawing.Point(397, 143)
        Me.RedCancelButton.Name = "RedCancelButton"
        Me.RedCancelButton.Size = New System.Drawing.Size(83, 29)
        Me.RedCancelButton.TabIndex = 22
        Me.RedCancelButton.Text = "CANCEL"
        Me.RedCancelButton.UseVisualStyleBackColor = False
        '
        'ProgBarBgPanel
        '
        Me.ProgBarBgPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ProgBarBgPanel.Controls.Add(Me.ProgBarFgPanel)
        Me.ProgBarBgPanel.Location = New System.Drawing.Point(10, 227)
        Me.ProgBarBgPanel.Name = "ProgBarBgPanel"
        Me.ProgBarBgPanel.Size = New System.Drawing.Size(545, 39)
        Me.ProgBarBgPanel.TabIndex = 23
        '
        'ProgBarFgPanel
        '
        Me.ProgBarFgPanel.BackColor = System.Drawing.Color.Gray
        Me.ProgBarFgPanel.Location = New System.Drawing.Point(0, 0)
        Me.ProgBarFgPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.ProgBarFgPanel.Name = "ProgBarFgPanel"
        Me.ProgBarFgPanel.Size = New System.Drawing.Size(99, 39)
        Me.ProgBarFgPanel.TabIndex = 0
        Me.ProgBarFgPanel.Visible = False
        '
        'ElapsedBox
        '
        Me.ElapsedBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.ElapsedBox.Location = New System.Drawing.Point(65, 139)
        Me.ElapsedBox.Name = "ElapsedBox"
        Me.ElapsedBox.ReadOnly = True
        Me.ElapsedBox.Size = New System.Drawing.Size(111, 22)
        Me.ElapsedBox.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 141)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 15)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Elapsed"
        '
        'PercentBox
        '
        Me.PercentBox.BackColor = System.Drawing.SystemColors.Menu
        Me.PercentBox.Font = New System.Drawing.Font("Lucida Sans Typewriter", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.PercentBox.ForeColor = System.Drawing.Color.Black
        Me.PercentBox.Location = New System.Drawing.Point(315, 70)
        Me.PercentBox.Name = "PercentBox"
        Me.PercentBox.ReadOnly = True
        Me.PercentBox.Size = New System.Drawing.Size(217, 64)
        Me.PercentBox.TabIndex = 26
        Me.PercentBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 350)
        Me.Controls.Add(Me.PercentBox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ElapsedBox)
        Me.Controls.Add(Me.ProgBarBgPanel)
        Me.Controls.Add(Me.RedCancelButton)
        Me.Controls.Add(Me.ValidCheck)
        Me.Controls.Add(Me.ExistCheck)
        Me.Controls.Add(Me.OverCheck)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TimeLeftBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.SinceLastBlockBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BlockCountBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BlockNumBox)
        Me.Controls.Add(Me.KbpsBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "CopyShit.EXE"
        Me.ProgBarBgPanel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents LastblocktimeBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents KbpsBox As TextBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents BlockNumBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents BlockCountBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ProgressBar2 As ProgressBar
    Friend WithEvents SinceLastBlockBox As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents TimeLeftBox As TextBox
    Friend WithEvents OverCheck As CheckBox
    Friend WithEvents ExistCheck As CheckBox
    Friend WithEvents ValidCheck As CheckBox
    Friend WithEvents RedCancelButton As Button
    Friend WithEvents ProgBarBgPanel As Panel
    Friend WithEvents ProgBarFgPanel As Panel
    Friend WithEvents ElapsedBox As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents PercentBox As TextBox
End Class
