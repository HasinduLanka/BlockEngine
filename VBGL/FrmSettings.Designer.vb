<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSettings
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
        Me.SliderGQ = New Bunifu.Framework.UI.BunifuSlider()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblGraphicQuality = New System.Windows.Forms.Label()
        Me.LblLODAndBuff = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'SliderGQ
        '
        Me.SliderGQ.BackColor = System.Drawing.Color.Transparent
        Me.SliderGQ.BackgroudColor = System.Drawing.Color.DarkGray
        Me.SliderGQ.BorderRadius = 0
        Me.SliderGQ.IndicatorColor = System.Drawing.Color.SeaGreen
        Me.SliderGQ.Location = New System.Drawing.Point(170, 18)
        Me.SliderGQ.MaximumValue = 400
        Me.SliderGQ.Name = "SliderGQ"
        Me.SliderGQ.Size = New System.Drawing.Size(380, 30)
        Me.SliderGQ.TabIndex = 0
        Me.SliderGQ.Value = 100
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Graphic Quality"
        '
        'LblGraphicQuality
        '
        Me.LblGraphicQuality.AutoSize = True
        Me.LblGraphicQuality.Location = New System.Drawing.Point(131, 25)
        Me.LblGraphicQuality.Name = "LblGraphicQuality"
        Me.LblGraphicQuality.Size = New System.Drawing.Size(33, 13)
        Me.LblGraphicQuality.TabIndex = 2
        Me.LblGraphicQuality.Text = "100%"
        '
        'LblLODAndBuff
        '
        Me.LblLODAndBuff.AutoSize = True
        Me.LblLODAndBuff.Location = New System.Drawing.Point(12, 40)
        Me.LblLODAndBuff.Name = "LblLODAndBuff"
        Me.LblLODAndBuff.Size = New System.Drawing.Size(144, 13)
        Me.LblLODAndBuff.TabIndex = 2
        Me.LblLODAndBuff.Text = "LOD Bias -0.8  Buffer Ratio 1"
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(555, 523)
        Me.Controls.Add(Me.LblLODAndBuff)
        Me.Controls.Add(Me.LblGraphicQuality)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SliderGQ)
        Me.Name = "FrmSettings"
        Me.Text = "Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SliderGQ As Bunifu.Framework.UI.BunifuSlider
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents LblGraphicQuality As Windows.Forms.Label
    Friend WithEvents LblLODAndBuff As Windows.Forms.Label
End Class
