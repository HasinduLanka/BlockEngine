#If WINDOWS Or XBOX Then

Module Program
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Sub Main(ByVal args As String()) '

        ' MMenu.ShowDialog()
        'Game1.GameType1 = GameTypes.SinglePlayerFreeMode

        Using game As New Game1()
            game.Run()
        End Using
    End Sub
End Module

#End If
