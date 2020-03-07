<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLog
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
        Me.components = New System.ComponentModel.Container()
        Me.TxtLog = New System.Windows.Forms.TextBox()
        Me.TmrLog = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'TxtLog
        '
        Me.TxtLog.BackColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.TxtLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TxtLog.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.TxtLog.Location = New System.Drawing.Point(0, 0)
        Me.TxtLog.Multiline = True
        Me.TxtLog.Name = "TxtLog"
        Me.TxtLog.ReadOnly = True
        Me.TxtLog.Size = New System.Drawing.Size(543, 384)
        Me.TxtLog.TabIndex = 7
        '
        'TmrLog
        '
        Me.TmrLog.Interval = 500
        '
        'FrmLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 384)
        Me.Controls.Add(Me.TxtLog)
        Me.Name = "FrmLog"
        Me.Text = "Log"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtLog As Windows.Forms.TextBox
    Friend WithEvents TmrLog As Windows.Forms.Timer
End Class
