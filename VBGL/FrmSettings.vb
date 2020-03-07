Imports System.ComponentModel

Public Class FrmSettings
    Public BarValue As Integer = 100

    Private Sub BunifuSlider1_ValueChanged(sender As Object, e As EventArgs) Handles SliderGQ.ValueChanged
        CalGraphicQuality(SliderGQ.Value)
    End Sub



	Public Sub CalGraphicQuality(Value As Integer)

		MapVariablePipeline.GraphicQuality = (Value) / 100

			Dim LODBias As Double
			If MapVariablePipeline.GraphicQuality > 1.5R Then
				LODBias = (MapVariablePipeline.GraphicQuality - 1) * 0.5 - 0.8

			Else
				LODBias = -(1.5 - MapVariablePipeline.GraphicQuality) * 0.6 - 0.8
			End If
			LblGraphicQuality.Text = Value & "%"
			LblLODAndBuff.Text = $"LOD Bias {LODBias}  Buffer Ratio {MapVariablePipeline.GraphicQuality}"


			MapVariablePipeline.LODBias = LODBias

			BarValue = Value


	End Sub

	Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SliderGQ.Value = BarValue
    End Sub

    Private Sub FrmSettings_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        CalGraphicQuality(SliderGQ.Value)
        My.Settings.GraphicQuality = BarValue
    End Sub

    Private Sub FrmSettings_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SliderGQ.Value = BarValue
    End Sub
End Class