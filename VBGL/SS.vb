

Public Class GameType
    Public Property IsFree As Boolean

    Public Property Name As String

    Public Property WinMode As String
    Public Property LossMode As String

    Public Sub New(GT As GameType)
        Name = GT.Name

        LossMode = GT.LossMode
        WinMode = GT.WinMode
    End Sub

    Public Sub New()

    End Sub
End Class

