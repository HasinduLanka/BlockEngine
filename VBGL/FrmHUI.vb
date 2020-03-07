Imports System.Windows.Forms

Public Class FrmHUI

    Dim Frm As Form
    Dim CTRLs As New List(Of Control)

    Public Sub Apply(Form As Form)
        CheckForIllegalCrossThreadCalls = False

        Frm = Form


        CTRLs.Clear()

        Me.Size = Frm.Size
        Refresh()

        With Frm
            ' .TransparencyKey = TransparencyKey
        End With


        BarHealth.Width = PnlDown.Width - LblHealth.Width
        LblHealth.Left = BarHealth.Width

        For Each Pnl As Control In Controls

            If TypeOf (Pnl) Is Panel Then
                For Each C As Control In Pnl.Controls

                    C.Top += Pnl.Top
                    C.Left += Pnl.Left
                    CTRLs.Add(C)
                Next
            End If

        Next

        'CTRLs.Add(BarHealth)
        Frm.Controls.Clear()
        Frm.Controls.AddRange(CTRLs.ToArray)


    End Sub



    Public Sub UpdateUI()
        'Values
        BarHealth.Value = CInt((Player1.Health / Player1.MaxHealth) * 100)
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'String'.
        LblHealth.Text = Player1.Health
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'String'.


        'Layout
        BarHealth.Width = PnlDown.Width - LblHealth.Width
        LblHealth.Left = BarHealth.Width
        LblMoney.Text = "◉ " & Player1.Money.ToString

        Refresh()
        '  Frm.Refresh()
    End Sub




End Class
