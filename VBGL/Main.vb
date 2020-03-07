Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Module Main

	Public Game As Game1
	Public GameThrd As Threading.Thread


	Public Viewport As Viewport


    Public NowGameTime As Long = 0

    Public MinFPS As Integer = 50
    Public MaxFPS As Integer = 58

    Public MaxRenderDistance As Single = 10000
    Public RenderDistance As Single = 6000
    Public MinRenderDistance As Single = 5000

    Public FOV As Single



    Public Player1 As Player





    Public DeadEntities As New List(Of Entity)


    Public MouseDefPosition As New Point(300, 300)
    Public MouseSensivity As Single = 0.4

    Public MouseWheelValue As Integer = 0


	Public cameraPosition As New Vector3(0F, 0F, -2000.0F)
	Public SunlightDirection As Vector3 = Vector3.Normalize(New Vector3(0F, 1.0F, -1.0F))
	Public CameraRelativeYPos As New Vector3(0, 146.5, 0)
    Public projectionMatrix As Matrix
    Public viewMatrix As Matrix


    Public FPS As Integer
    Public MouseLookEnadbled As Boolean = True


    ''' <summary>
    ''' 1 = 1 min
    ''' </summary>
    Public TimeOfTheDay As Double




    Public BackColor As Color = Color.LightBlue

	Public SunLightIntencity As Single = 1


	Public STW As New Stopwatch
    Public STWLast As Long


    Public FHUI As FrmHUI
    Public FGame As Windows.Forms.Form

	Public RunningSlow As Boolean = False
	Public IsGroundChanged As Boolean = False

    Public CurrUpdateCount As Long = 0

    Public TstSoundEff As Audio.SoundEffect

    Public CurrentMapName As String

    Public Class MapVariablePipeline
        Public Shared NewMap As Boolean = False
        Public Shared NewMapSize As Integer
        Public Shared NewMapBiome As Integer
        Public Shared NewMapSpeedSave As Boolean

        Public Shared GraphicQuality As Double = 1
        Public Shared LODBias As Double = -0.8

    End Class







    Public Started As Boolean = False
	Public Sub StartGame()


		GameThrd = New Threading.Thread(AddressOf StartGamenewThrd)
		GameThrd.Start()


	End Sub

	Private Sub StartGamenewThrd()
		Game = New Game1
		Started = True
		Game.Run()
	End Sub


    Private ReadOnly FileLog As String = "Log.txt"
    Public LogContent As String = ""
    Public nLog As Long = 0
    Public LastnLog As Long = 0
    Public Sub Log(S As String, Optional NewLine As Boolean = True)

        If NewLine Then
            LogContent = nLog & " -  " & S & vbNewLine & LogContent
            nLog += 5
        Else
            LogContent = S & LogContent
            nLog += 1
        End If

        If IsLogerEnabled AndAlso LastnLog < nLog - 5 Then
            Loger1.LogUpdate(nLog)
            LastnLog = nLog
        End If


    End Sub



    Public Sub Delay(Millies As Integer)
        Threading.Thread.Sleep(Millies)
    End Sub

    Public Sub RunInNewThread(Meth As [Delegate], Paras() As Object, Optional Delay As Integer = 0)
#Disable Warning BC42016 ' Implicit conversion from 'Object' to 'Tuple(Of [Delegate], Object())'.
		Dim Thr As New Threading.Thread(AddressOf ThrRunInNewThread)
#Enable Warning BC42016 ' Implicit conversion from 'Object' to 'Tuple(Of [Delegate], Object())'.
		Thr.Start(New Tuple(Of [Delegate], Object())(Meth, Paras))
	End Sub


	Private Sub ThrRunInNewThread(Input As Tuple(Of [Delegate], Object()))
		Input.Item1.DynamicInvoke(Input.Item2)
	End Sub


	Public Sub ExitApp()
		If Started Then
			MouseLookEnadbled = False


			If Ground.CStack.nSavingChunks > 0 Then

				Loader.SaveChunks(Ground.CStack.SavingChunks, Ground.CStack.nSavingChunks, False)

				Ground.CStack.nSavingChunks = 0

			End If


			Dim XeLst As New List(Of XEntity)
			For Each e In Ground.CStack.eList
				If Not e.IsDead OrElse e.eType.IsPlayer Then
					XeLst.Add(New XEntity(e))
				End If

			Next
			XEntity.Save(Loader.FileEntity, XeLst)
			Loader.MInfo.PlayerPosition = Player1.Position
			Loader.MInfo.Save(Loader.MapInfoFile)

			Ground.CStack.ChunkList = Nothing
			Ground.CStack.eList = Nothing



			Game.ExitGame()

			GC.Collect()
		End If


		ExitProgram()

	End Sub

	Public Sub ExitProgram()
		LogContent = "Log - " & Now.ToShortDateString & " " & Now.ToLongTimeString & vbNewLine & "_______________________________" & vbNewLine & LogContent
		My.Computer.FileSystem.WriteAllText(FileLog, LogContent, True)

		Process.GetCurrentProcess.Close()
		Process.GetCurrentProcess.Kill()
	End Sub


	Public IsLogerEnabled As Boolean = False
	Public Loger1 As FrmLog
	Public Sub ShowLoger()
		If Not IsLogerEnabled Then
			Loger1 = New FrmLog
			Dim LogThread As New Threading.Thread(AddressOf Loger1.ShowDialog)
			LogThread.Start()
			IsLogerEnabled = True

		Else
			Loger1.BringToFront()
		End If

	End Sub





	Public Function GetSunlightIntencity(Time As Double) As Single
		Dim h = Time / 60

		Dim u, l, a, b As Double


		If h < 5 Then
			Return 0.2

		ElseIf h < 6 Then
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return ((h - 5) * 0.4) + 0.2
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		ElseIf h < 8 Then

			l = 6
			u = 8
			a = 1.3
			b = 0.6
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((h - l) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		ElseIf h < 10 Then

			l = 8
			u = 10
			a = 1.4
			b = 1.3
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((h - l) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.


		ElseIf h < 13 Then

			l = 10
			u = 13
			a = 1.55
			b = 1.4
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((h - l) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		ElseIf h < 15 Then

			l = 13
			u = 15
			a = 1.55
			b = 1.35
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((u - h) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		ElseIf h < 17 Then

			l = 15
			u = 17
			a = 1.3
			b = 1.2
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((u - h) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		ElseIf h < 19 Then

			l = 17
			u = 19
			a = 1.2
			b = 0.5
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((u - h) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.


		ElseIf h < 22 Then

			l = 19
			u = 22
			a = 0.5
			b = 0.3
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
			Return (((u - h) / (u - l)) * (a - b)) + b
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

		Else

            Return 0.2

        End If

        'h<12
        ' (((h - l) / (u - l)) * (a - b)) + b

        'h>12
        '(((u - h) / (u - l)) * (a - b)) + b

    End Function



    Public Sub CloneObject(Of T)(originalobject As T, ByRef clone As T)

        'copy one object to another via reflection properties
        For Each p As System.Reflection.PropertyInfo In originalobject.GetType().GetProperties()
            If p.CanRead Then
                clone.GetType().GetProperty(p.Name).SetValue(clone, p.GetValue(originalobject, Nothing))
            End If
        Next
    End Sub



End Module
