Imports System.Windows.Forms

Public Class FrmMenu

    Public Maps As New List(Of String)
    Public BiomeLst As New List(Of String)

    Dim FrmSettings1 As New FrmSettings

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        ExitProgram()
    End Sub


    Private Sub Label4_MouseEnter(sender As Object, e As EventArgs)
        DirectCast(sender, System.Windows.Forms.Label).ForeColor = System.Drawing.Color.DodgerBlue
    End Sub

    Private Sub Label4_MouseLeave(sender As Object, e As EventArgs)
        DirectCast(sender, System.Windows.Forms.Label).ForeColor = Drawing.Color.White
    End Sub

    Private Sub FrmMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		If Not My.Computer.FileSystem.DirectoryExists("Maps") Then
			My.Computer.FileSystem.CreateDirectory("Maps")
		End If

		LstMaps.Items.Clear()
        For Each S In My.Computer.FileSystem.GetDirectories("Maps")
            Dim MapName = S.Substring(S.LastIndexOf("\"c) + 1)
            If Loader.CheckIfMapExits(MapName) Then
                Maps.Add(MapName)
                LstMaps.Items.Add(MapName)
                LstMaps.SelectedIndex = 0
            End If
        Next

        BiomeLst = [Enum].GetNames(GetType(BiomeList.Biomes)).ToList

        CmbBiomes.Items.AddRange(BiomeLst.ToArray)


        CmbBiomes.SelectedIndex = 0
		'FrmSettings1.Show()
		FrmSettings1.CalGraphicQuality(My.Settings.GraphicQuality)

	End Sub


    Private Sub LstMaps_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LstMaps.MouseDoubleClick
        If LstMaps.SelectedIndex > -1 Then
            BtnStart_Click(Nothing, Nothing)

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If LstMaps.SelectedIndex > -1 Then
            Try
                My.Computer.FileSystem.DeleteDirectory("Maps/" & LstMaps.SelectedItem.ToString, FileIO.DeleteDirectoryOption.DeleteAllContents)


            Catch ex As Exception
                Log(ex.Message)
            End Try

        End If


        LstMaps.Items.Clear()
        For Each S In My.Computer.FileSystem.GetDirectories("Maps")
            Dim MapName = S.Substring(S.LastIndexOf("\"c) + 1)
            If Loader.CheckIfMapExits(MapName) Then
                Maps.Add(MapName)
                LstMaps.Items.Add(MapName)
                LstMaps.SelectedIndex = 0
            End If
        Next

    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        If LstMaps.SelectedIndex > -1 Then
            If Loader.CheckIfMapExits(LstMaps.SelectedItem.ToString) Then
                Main.CurrentMapName = LstMaps.SelectedItem.ToString
                ShowLoger()
                Hide()
                StartGame()
            Else
                MsgBox("This Map does not exist. Please Create a new map")
            End If
        Else
            MsgBox("Please Create a new map or select one from the list")
        End If
    End Sub

    Private Sub BtnCreateNewMap_Click(sender As Object, e As EventArgs) Handles BtnCreateNewMap.Click

        If CmbBiomes.SelectedIndex < 0 Or TxtMapName.Text = "" Or TxtWorldSize.Text = "" Or (Not IsNumeric(TxtWorldSize.Text)) Then
            MsgBox("Please recorrect the details.")
            Exit Sub
        ElseIf Val(TxtWorldSize.Text) <= 20 Then
            MsgBox("World size must be geater than 20")
            Exit Sub
        End If

        MapVariablePipeline.NewMap = True
        MapVariablePipeline.NewMapBiome = CmbBiomes.SelectedIndex
        MapVariablePipeline.NewMapSpeedSave = ChkSS.Checked
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        MapVariablePipeline.NewMapSize = Val(TxtWorldSize.Text)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


        Main.CurrentMapName = TxtMapName.Text
        ShowLoger()
        Hide()
        StartGame()
    End Sub

    Private Sub BtnGraphicSetts_Click(sender As Object, e As EventArgs) Handles BtnGraphicSetts.Click
        FrmSettings1.ShowDialog()
        My.Settings.Save()
    End Sub
End Class