Imports System.Windows.Forms

Public Class FrmLog
    Public nLog As Long = 0
    Public Now_nLog As Long = 0
    Private Sub FrmLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TmrLog.Start()
    End Sub

    Private Sub TmrLog_Tick(sender As Object, e As EventArgs) Handles TmrLog.Tick

        If nLog <> Now_nLog Then
            TxtLog.Text = LogContent
            Now_nLog = nLog
        End If

    End Sub

    Public Sub LogUpdate(n As Long)
        While nLog <> n
            nLog = n
            Threading.Thread.Sleep(10)
        End While

    End Sub

    Private Sub TxtLog_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtLog.KeyDown
        If e.KeyCode = Keys.Escape Then
            ExitApp()
        End If
    End Sub
End Class