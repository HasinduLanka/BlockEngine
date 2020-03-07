<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmHUI
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
		Me.PnlUp = New System.Windows.Forms.Panel()
		Me.LblMoney = New System.Windows.Forms.Label()
		Me.PnlDown = New System.Windows.Forms.Panel()
		Me.LblHealth = New Bunifu.Framework.UI.BunifuCustomLabel()
		Me.BarHealth = New Bunifu.Framework.UI.BunifuProgressBar()
		Me.PnlLeft = New System.Windows.Forms.Panel()
		Me.PnlRight = New System.Windows.Forms.Panel()
		Me.PnlUp.SuspendLayout()
		Me.PnlDown.SuspendLayout()
		Me.SuspendLayout()
		'
		'PnlUp
		'
		Me.PnlUp.Controls.Add(Me.LblMoney)
		Me.PnlUp.Dock = System.Windows.Forms.DockStyle.Top
		Me.PnlUp.Location = New System.Drawing.Point(0, 0)
		Me.PnlUp.Name = "PnlUp"
		Me.PnlUp.Size = New System.Drawing.Size(984, 49)
		Me.PnlUp.TabIndex = 0
		'
		'LblMoney
		'
		Me.LblMoney.AutoSize = True
		Me.LblMoney.BackColor = System.Drawing.Color.White
		Me.LblMoney.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblMoney.Location = New System.Drawing.Point(9, 9)
		Me.LblMoney.Margin = New System.Windows.Forms.Padding(0)
		Me.LblMoney.Name = "LblMoney"
		Me.LblMoney.Size = New System.Drawing.Size(74, 25)
		Me.LblMoney.TabIndex = 0
		Me.LblMoney.Text = "◉ 1000"
		Me.LblMoney.Visible = False
		'
		'PnlDown
		'
		Me.PnlDown.Controls.Add(Me.LblHealth)
		Me.PnlDown.Controls.Add(Me.BarHealth)
		Me.PnlDown.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.PnlDown.Location = New System.Drawing.Point(0, 453)
		Me.PnlDown.Name = "PnlDown"
		Me.PnlDown.Size = New System.Drawing.Size(984, 42)
		Me.PnlDown.TabIndex = 0
		'
		'LblHealth
		'
		Me.LblHealth.AutoSize = True
		Me.LblHealth.BackColor = System.Drawing.Color.Red
		Me.LblHealth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.LblHealth.Location = New System.Drawing.Point(937, 21)
		Me.LblHealth.Name = "LblHealth"
		Me.LblHealth.Size = New System.Drawing.Size(45, 20)
		Me.LblHealth.TabIndex = 1
		Me.LblHealth.Text = "1000"
		'
		'BarHealth
		'
		Me.BarHealth.BackColor = System.Drawing.Color.White
		Me.BarHealth.BorderRadius = 0
		Me.BarHealth.Location = New System.Drawing.Point(1, 22)
		Me.BarHealth.Margin = New System.Windows.Forms.Padding(0)
		Me.BarHealth.MaximumValue = 100
		Me.BarHealth.Name = "BarHealth"
		Me.BarHealth.ProgressColor = System.Drawing.Color.Red
		Me.BarHealth.Size = New System.Drawing.Size(933, 20)
		Me.BarHealth.TabIndex = 6
		Me.BarHealth.Value = 40
		'
		'PnlLeft
		'
		Me.PnlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.PnlLeft.Location = New System.Drawing.Point(0, 49)
		Me.PnlLeft.Name = "PnlLeft"
		Me.PnlLeft.Size = New System.Drawing.Size(200, 404)
		Me.PnlLeft.TabIndex = 0
		'
		'PnlRight
		'
		Me.PnlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.PnlRight.Location = New System.Drawing.Point(784, 49)
		Me.PnlRight.Name = "PnlRight"
		Me.PnlRight.Size = New System.Drawing.Size(200, 404)
		Me.PnlRight.TabIndex = 0
		'
		'FrmHUI
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.Color.CornflowerBlue
		Me.ClientSize = New System.Drawing.Size(984, 495)
		Me.Controls.Add(Me.PnlLeft)
		Me.Controls.Add(Me.PnlRight)
		Me.Controls.Add(Me.PnlDown)
		Me.Controls.Add(Me.PnlUp)
		Me.Name = "FrmHUI"
		Me.Text = "FrmHUI"
		Me.TransparencyKey = System.Drawing.Color.RosyBrown
		Me.PnlUp.ResumeLayout(False)
		Me.PnlUp.PerformLayout()
		Me.PnlDown.ResumeLayout(False)
		Me.PnlDown.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents PnlUp As Windows.Forms.Panel
    Friend WithEvents PnlDown As Windows.Forms.Panel
    Friend WithEvents PnlLeft As Windows.Forms.Panel
    Friend WithEvents PnlRight As Windows.Forms.Panel
    Friend WithEvents BarHealth As Bunifu.Framework.UI.BunifuProgressBar
    Friend WithEvents LblHealth As Bunifu.Framework.UI.BunifuCustomLabel
    Friend WithEvents LblMoney As Windows.Forms.Label
End Class
