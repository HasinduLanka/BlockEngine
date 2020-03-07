Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Board
    Public Shared SB As SpriteBatch
    Public Shared IsDebugInfoVisible As Boolean = True

    Public Shared Font1 As SpriteFont

    Public Shared DebugText As String
    Public Shared DebugTextCords As New Vector2


    Public Shared SelectedToolAndBlock As String = ""


    Public Shared Sampler As SamplerState

    Public Shared StillImageSampler As SamplerState
    Public Shared MovingImageSampler As SamplerState
    Public Shared DepthBuff As DepthStencilState
    Public Shared Rasterizer As RasterizerState



    Public Shared Sub Load(FFont1 As SpriteFont)
        Font1 = FFont1




#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        StillImageSampler = New SamplerState With {
            .Filter = TextureFilter.Anisotropic,
            .AddressU = TextureAddressMode.Wrap,
            .AddressV = TextureAddressMode.Wrap,
            .AddressW = TextureAddressMode.Wrap,
            .MaxMipLevel = 64,
            .MipMapLevelOfDetailBias = MapVariablePipeline.LODBias,
            .ComparisonFunction = CompareFunction.Less
            }
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.


#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        MovingImageSampler = New SamplerState With {
            .Filter = TextureFilter.Anisotropic,
            .AddressU = TextureAddressMode.Wrap,
            .AddressV = TextureAddressMode.Wrap,
            .AddressW = TextureAddressMode.Wrap,
            .MaxMipLevel = 64,
            .MipMapLevelOfDetailBias = Math.Max(MapVariablePipeline.LODBias, 0),
            .ComparisonFunction = CompareFunction.Less
            }
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.


        DepthBuff = New DepthStencilState With {
            .DepthBufferEnable = True,
            .DepthBufferFunction = CompareFunction.Less,
            .CounterClockwiseStencilDepthBufferFail = StencilOperation.Keep,
            .CounterClockwiseStencilFail = StencilOperation.Keep,
            .CounterClockwiseStencilFunction = CompareFunction.Less,
            .CounterClockwiseStencilPass = StencilOperation.Keep,
            .StencilEnable = True,
            .StencilDepthBufferFail = StencilOperation.Keep
        }


        Rasterizer = New RasterizerState With {
            .MultiSampleAntiAlias = True,
            .CullMode = CullMode.CullCounterClockwiseFace,
            .FillMode = FillMode.Solid
            }


        Sampler = StillImageSampler


    End Sub



    Public Shared Sub Draw()


        DebugText = ""

		DebugText += "Chunk = " + Player1.CurrentChunk.Index.ToString +
		"   Position = " + (Player1.Position).ToString + vbNewLine +
	   "Time = " + Math.Round(TimeOfTheDay / 60, 2).ToString +
		"    FPS = " + FPS.ToString + " @ " + RenderDistance.ToString + vbNewLine +
		"Health " + Player1.Health.ToString & "   Money " & Player1.Money.ToString + vbNewLine +
		"Sun : " & SunlightDirection.ToString

		DebugText += vbNewLine + SelectedToolAndBlock





        SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Sampler, DepthBuff, Rasterizer)


#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        If Board.IsDebugInfoVisible Then SB.DrawString(Font1, DebugText, DebugTextCords, Color.Red, 0, Vector2.Zero, MapVariablePipeline.GraphicQuality, SpriteEffects.None, 0)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.



        SB.DrawString(Font1, "x", New Vector2(Game1.Graphics.GraphicsDevice.Viewport.Width \ 2, Game1.Graphics.GraphicsDevice.Viewport.Height \ 2), Color.Red)


        SB.End()
    End Sub

End Class

