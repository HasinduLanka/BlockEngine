<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMenu))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LstMaps = New System.Windows.Forms.ListBox()
        Me.TxtMapName = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.BtnCreateNewMap = New System.Windows.Forms.Button()
        Me.ChkSS = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtWorldSize = New System.Windows.Forms.TextBox()
        Me.CmbBiomes = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnGraphicSetts = New Bunifu.Framework.UI.BunifuThinButton2()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Gray
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DarkRed
        Me.Label1.Location = New System.Drawing.Point(599, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 31)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "X"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Label2.Font = New System.Drawing.Font("Elephant", 45.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(35, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(553, 77)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Block Engine 3D"
        '
        'LstMaps
        '
        Me.LstMaps.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LstMaps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LstMaps.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstMaps.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LstMaps.FormattingEnabled = True
        Me.LstMaps.ItemHeight = 31
        Me.LstMaps.Location = New System.Drawing.Point(362, 107)
        Me.LstMaps.Name = "LstMaps"
        Me.LstMaps.Size = New System.Drawing.Size(259, 343)
        Me.LstMaps.TabIndex = 3
        '
        'TxtMapName
        '
        Me.TxtMapName.BackColor = System.Drawing.Color.Silver
        Me.TxtMapName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtMapName.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMapName.Location = New System.Drawing.Point(5, 78)
        Me.TxtMapName.Name = "TxtMapName"
        Me.TxtMapName.Size = New System.Drawing.Size(100, 33)
        Me.TxtMapName.TabIndex = 4
        Me.TxtMapName.Text = "Map1"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.BtnCreateNewMap)
        Me.Panel1.Controls.Add(Me.ChkSS)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.TxtWorldSize)
        Me.Panel1.Controls.Add(Me.CmbBiomes)
        Me.Panel1.Controls.Add(Me.TxtMapName)
        Me.Panel1.Location = New System.Drawing.Point(7, 227)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(338, 223)
        Me.Panel1.TabIndex = 5
        '
        'BtnCreateNewMap
        '
        Me.BtnCreateNewMap.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BtnCreateNewMap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.BtnCreateNewMap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.BtnCreateNewMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCreateNewMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCreateNewMap.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.BtnCreateNewMap.Location = New System.Drawing.Point(4, 13)
        Me.BtnCreateNewMap.Name = "BtnCreateNewMap"
        Me.BtnCreateNewMap.Size = New System.Drawing.Size(329, 47)
        Me.BtnCreateNewMap.TabIndex = 7
        Me.BtnCreateNewMap.Text = "■ Create new map"
        Me.BtnCreateNewMap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnCreateNewMap.UseVisualStyleBackColor = False
        '
        'ChkSS
        '
        Me.ChkSS.AutoSize = True
        Me.ChkSS.Checked = True
        Me.ChkSS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkSS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.ChkSS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ChkSS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkSS.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSS.Location = New System.Drawing.Point(85, 148)
        Me.ChkSS.Name = "ChkSS"
        Me.ChkSS.Size = New System.Drawing.Size(152, 29)
        Me.ChkSS.TabIndex = 8
        Me.ChkSS.Text = "Speed Saving"
        Me.ChkSS.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Label5.Location = New System.Drawing.Point(13, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "World Size"
        '
        'TxtWorldSize
        '
        Me.TxtWorldSize.BackColor = System.Drawing.Color.Silver
        Me.TxtWorldSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtWorldSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 17.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtWorldSize.Location = New System.Drawing.Point(14, 151)
        Me.TxtWorldSize.Name = "TxtWorldSize"
        Me.TxtWorldSize.Size = New System.Drawing.Size(57, 33)
        Me.TxtWorldSize.TabIndex = 6
        Me.TxtWorldSize.Text = "50"
        '
        'CmbBiomes
        '
        Me.CmbBiomes.BackColor = System.Drawing.Color.Silver
        Me.CmbBiomes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CmbBiomes.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbBiomes.FormattingEnabled = True
        Me.CmbBiomes.Location = New System.Drawing.Point(111, 78)
        Me.CmbBiomes.Name = "CmbBiomes"
        Me.CmbBiomes.Size = New System.Drawing.Size(211, 33)
        Me.CmbBiomes.TabIndex = 5
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(13, 170)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(332, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Delete"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'BtnStart
        '
        Me.BtnStart.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BtnStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
        Me.BtnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray
        Me.BtnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.BtnStart.Location = New System.Drawing.Point(13, 117)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(332, 47)
        Me.BtnStart.TabIndex = 7
        Me.BtnStart.Text = "■  Start"
        Me.BtnStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnStart.UseVisualStyleBackColor = False
        '
        'BtnGraphicSetts
        '
        Me.BtnGraphicSetts.ActiveBorderThickness = 1
        Me.BtnGraphicSetts.ActiveCornerRadius = 20
        Me.BtnGraphicSetts.ActiveFillColor = System.Drawing.Color.Cyan
        Me.BtnGraphicSetts.ActiveForecolor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BtnGraphicSetts.ActiveLineColor = System.Drawing.Color.Yellow
        Me.BtnGraphicSetts.BackColor = System.Drawing.SystemColors.GrayText
        Me.BtnGraphicSetts.BackgroundImage = CType(resources.GetObject("BtnGraphicSetts.BackgroundImage"), System.Drawing.Image)
        Me.BtnGraphicSetts.ButtonText = "Settings"
        Me.BtnGraphicSetts.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnGraphicSetts.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGraphicSetts.ForeColor = System.Drawing.Color.White
        Me.BtnGraphicSetts.IdleBorderThickness = 1
        Me.BtnGraphicSetts.IdleCornerRadius = 20
        Me.BtnGraphicSetts.IdleFillColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.BtnGraphicSetts.IdleForecolor = System.Drawing.Color.White
        Me.BtnGraphicSetts.IdleLineColor = System.Drawing.Color.Cyan
        Me.BtnGraphicSetts.Location = New System.Drawing.Point(7, 458)
        Me.BtnGraphicSetts.Margin = New System.Windows.Forms.Padding(5)
        Me.BtnGraphicSetts.Name = "BtnGraphicSetts"
        Me.BtnGraphicSetts.Size = New System.Drawing.Size(142, 50)
        Me.BtnGraphicSetts.TabIndex = 8
        Me.BtnGraphicSetts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FrmMenu
        '
        Me.AcceptButton = Me.BtnStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GrayText
        Me.ClientSize = New System.Drawing.Size(633, 518)
        Me.Controls.Add(Me.BtnGraphicSetts)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LstMaps)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmMenu"
        Me.Text = "FrmMenu"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents LstMaps As Windows.Forms.ListBox
    Friend WithEvents TxtMapName As Windows.Forms.TextBox
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents CmbBiomes As Windows.Forms.ComboBox
    Friend WithEvents ChkSS As Windows.Forms.CheckBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents TxtWorldSize As Windows.Forms.TextBox
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents BtnStart As Windows.Forms.Button
    Friend WithEvents BtnCreateNewMap As Windows.Forms.Button
    Friend WithEvents BtnGraphicSetts As Bunifu.Framework.UI.BunifuThinButton2
End Class
