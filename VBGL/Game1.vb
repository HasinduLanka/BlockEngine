Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input


''' <summary>
''' This is the main type for your game
''' </summary> 
''' 
Public Class Game1
    Inherits Microsoft.Xna.Framework.Game

#Region "Initializing"

#Region "Paramenters"
    Public Shared WithEvents Graphics As GraphicsDeviceManager

    Public InitialHumanCount As Integer = 0

    Public LastCamSwapedTime As Long



    Public Sub New()


        Graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"

    End Sub


    Protected Overrides Sub Initialize()

        TimeOfTheDay = 9 * 60
        BackColor = Color.LightBlue
        IsMouseVisible = True

        MyBase.Initialize()
    End Sub


    Public Function SetupEffectDefaults(myModel As Model, projectionMatrix As Matrix, viewMatrix As Matrix) As Matrix()
        Dim absoluteTransforms As Matrix() = New Matrix(myModel.Bones.Count - 1) {}
        myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms)

        For Each mesh As ModelMesh In myModel.Meshes
            For Each effect As BasicEffect In mesh.Effects
                effect.EnableDefaultLighting()
                effect.Projection = projectionMatrix
                effect.View = viewMatrix
                'effect.PreferPerPixelLighting = True
                effect.FogEnabled = True
                effect.FogColor = New Vector3(0.1)
                effect.FogStart = RenderDistance - 300
                effect.FogEnd = RenderDistance + 300
                'effect.PreferPerPixelLighting = True
            Next
        Next
        Return absoluteTransforms
    End Function


#End Region

    Protected Overrides Sub LoadContent()


#Region "Initializing Graphics"

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * MapVariablePipeline.GraphicQuality
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * MapVariablePipeline.GraphicQuality
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

        Graphics.GraphicsDevice.BlendState = BlendState.Opaque
        Graphics.GraphicsProfile = GraphicsProfile.HiDef


        Graphics.ApplyChanges()





        FOV = MathHelper.ToRadians(80)

        Viewport = New Viewport(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 0, 10000)

        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(FOV, Viewport.AspectRatio, 1.0F, RenderDistance)

        viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up)




        For n = 0 To nArrays

            Dim count = (n + 1) * SizeOfRoom

            IVBs(n) = New DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration, count, BufferUsage.WriteOnly)
            MAs(n) = New Vector4(count - 1) {}


        Next


#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * MapVariablePipeline.GraphicQuality
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * MapVariablePipeline.GraphicQuality
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


        Graphics.ApplyChanges()


#End Region


#Region "Loading Content"

        Board.SB = New SpriteBatch(Graphics.GraphicsDevice)
        Board.Load(Content.Load(Of SpriteFont)("SF1"))

        GraphicsDevice.DepthStencilState = Board.DepthBuff
        GraphicsDevice.RasterizerState = Board.Rasterizer
        GraphicsDevice.SamplerStates(0) = Board.StillImageSampler

        EntityTypes.WayPoint = New EntityType
        EntityTypes.WayPoint.Load(Nothing, 0)
        EntityTypes.Lst.Add(EntityTypes.WayPoint)


        EntityTypes.Player1 = New EntityType
        EntityTypes.Player1.Load(Content.Load(Of Model)("Models\Player1"), 1)
        EntityTypes.Player1.IsHuman = True
        EntityTypes.Player1.SetMethods(GetType(Player))
        EntityTypes.Player1.IsPlayer = True
        EntityTypes.Player1.Transforms = SetupEffectDefaults(EntityTypes.Player1.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Player1.Width = 30
        EntityTypes.Lst.Add(EntityTypes.Player1)


        EntityTypes.Human1 = New EntityType
        EntityTypes.Human1.Load(Content.Load(Of Model)("Models\Man1"), 2)
        EntityTypes.Human1.IsHuman = True
        EntityTypes.Human1.SetMethods(GetType(Human))
        EntityTypes.Human1.CreateBasicTargetingRules()
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Human1.UpdateAI = AIs.YoungMan
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        'EntityTypes.Human1.EtFriendlies.Add(EntityTypes.Human1)
        EntityTypes.Human1.Transforms = SetupEffectDefaults(EntityTypes.Human1.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Human1.Width = 30
        EntityTypes.Lst.Add(EntityTypes.Human1)

        EntityTypes.Arrow1 = New EntityType
        EntityTypes.Arrow1.Load(Content.Load(Of Model)("Models\Shovel"), 3)
        EntityTypes.Arrow1.SetMethods(GetType(Arrow))
        EntityTypes.Arrow1.IsHuman = False
        EntityTypes.Arrow1.Transforms = SetupEffectDefaults(EntityTypes.Arrow1.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Lst.Add(EntityTypes.Arrow1)


        EntityTypes.Civilian = New EntityType
        EntityTypes.Civilian.Load(Content.Load(Of Model)("Models\Civilian"), 4)
        CType(EntityTypes.Civilian.Model.Meshes(Human.iRLeg).Effects(0), BasicEffect).DiffuseColor = New Vector3(0, 1, 1)
        CType(EntityTypes.Civilian.Model.Meshes(Human.iLLeg).Effects(0), BasicEffect).DiffuseColor = New Vector3(0, 1, 1)
        CType(EntityTypes.Civilian.Model.Meshes(Human.iRKnee).Effects(3), BasicEffect).DiffuseColor = New Vector3(0, 1, 1)
        CType(EntityTypes.Civilian.Model.Meshes(Human.iLKnee).Effects(1), BasicEffect).DiffuseColor = New Vector3(0, 1, 1)
        CType(EntityTypes.Civilian.Model.Meshes(Human.iBody).Effects(1), BasicEffect).DiffuseColor = New Vector3(0, 1, 1)
        EntityTypes.Civilian.IsHuman = True
        EntityTypes.Civilian.SetMethods(GetType(Human))
        EntityTypes.Civilian.CreateBasicTargetingRules()
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Civilian.UpdateAI = AIs.Civilian
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Civilian.Transforms = SetupEffectDefaults(EntityTypes.Civilian.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Civilian.Width = 30
        EntityTypes.Lst.Add(EntityTypes.Civilian)



        EntityTypes.Guard = New EntityType
        EntityTypes.Guard.Load(Content.Load(Of Model)("Models\Guard"), 5)
        EntityTypes.Guard.IsHuman = True
        EntityTypes.Guard.SetMethods(GetType(Human))
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Guard.UpdateAI = AIs.Guard
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Guard.CreateBasicTargetingRules()
        EntityTypes.Guard.TargetSelectingRulesForDetection(EntityRelationMode.Unknowen) = AITargetMode.FollowAndKill
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Guard.DelTargetScan = AIs.AttackWhenCloseBy
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Guard.EtFriendlies.Add(EntityTypes.Guard)
        EntityTypes.Guard.Transforms = SetupEffectDefaults(EntityTypes.Guard.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Guard.Width = 30
        EntityTypes.Lst.Add(EntityTypes.Guard)


        EntityTypes.Murderer = New EntityType
        EntityTypes.Murderer.Load(Content.Load(Of Model)("Models\Murderer"), 6)
        CType(EntityTypes.Murderer.Model.Meshes(Human.iRLeg).Effects(0), BasicEffect).DiffuseColor = New Vector3(1, 0, 0)
        CType(EntityTypes.Murderer.Model.Meshes(Human.iLLeg).Effects(0), BasicEffect).DiffuseColor = New Vector3(1, 0, 0)
        CType(EntityTypes.Murderer.Model.Meshes(Human.iRKnee).Effects(3), BasicEffect).DiffuseColor = New Vector3(1, 0, 0)
        CType(EntityTypes.Murderer.Model.Meshes(Human.iLKnee).Effects(1), BasicEffect).DiffuseColor = New Vector3(1, 0, 0)
        CType(EntityTypes.Murderer.Model.Meshes(Human.iBody).Effects(1), BasicEffect).DiffuseColor = New Vector3(1, 0, 0)
        EntityTypes.Murderer.IsHuman = True
        EntityTypes.Murderer.SetMethods(GetType(Human))
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Murderer.DelTargetScan = AIs.AttackOnSight
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Murderer.UpdateAI = AIs.Murdurer
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        EntityTypes.Murderer.CreateBasicTargetingRules()
        EntityTypes.Murderer.TargetSelectingRulesForDetection(EntityRelationMode.Unknowen) = AITargetMode.FollowAndKill
        EntityTypes.Murderer.EtEnemies.AddRange({EntityTypes.Guard, EntityTypes.Player1})
        EntityTypes.Murderer.EtFriendlies.Add(EntityTypes.Murderer)
        EntityTypes.Murderer.Transforms = SetupEffectDefaults(EntityTypes.Murderer.Model, projectionMatrix, viewMatrix).Last
        EntityTypes.Murderer.Width = 30
        EntityTypes.Lst.Add(EntityTypes.Murderer)




        Dim Sword1 As New Tool With {
            .Model = Content.Load(Of Model)("Models\RSword"),
            .Transforms = SetupEffectDefaults(.Model, projectionMatrix, viewMatrix)
        }



        With Sword1
            .Name = "Sword1"
            .Rotation = Matrix.Identity
            .RelativePosition = New Vector3(100, 0, 0)
            .OriginalRelativePosition = New Vector3(100, 0, 0)
            .OriginalRotation = Matrix.CreateFromAxisAngle(.Rotation.Left, 0.7)
            .DefualtRotation = Matrix.CreateFromAxisAngle(.Rotation.Left, 0.7)
            .Type1 = Tool.Types.Sword
            .Length = 85
            .MaxAttackingDistance = 85
            .MinAttackingDistance = 10
            .RewardsToVictim = New RewardSet(-5)
            'Change it in Tool.Buy too
        End With

        Tool.Tools.Add(Sword1)
        Tool.iSword1 = 0





        'Bow
        Dim Bow1 As New Tool With {
            .Model = Content.Load(Of Model)("Models\BowD")
        }
        Bow1.Transforms = SetupEffectDefaults(Bow1.Model, projectionMatrix, viewMatrix)


        With Bow1
            .Name = "Bow1"
            .Rotation = Matrix.Identity
            .RelativePosition = Vector3.Zero
            .OriginalRotation = Matrix.CreateFromAxisAngle(.Rotation.Left, 0.7)
            .DefualtRotation = Matrix.CreateFromAxisAngle(.Rotation.Left, 0.7)
            .Type1 = Tool.Types.Bow
            .Length = 10
            .RewardsToVictim = New RewardSet(0)
            .MaxAttackingDistance = 90
            .MinAttackingDistance = 70
            'Change it in Tool.Buy too
        End With

        Tool.Tools.Add(Bow1)
        Tool.iBow1 = 1



        Dim RSword As New Tool With {
            .Model = Content.Load(Of Model)("Models\RSword")
        }
        RSword.Transforms = SetupEffectDefaults(RSword.Model, projectionMatrix, viewMatrix)


        With RSword
            .Name = "RSword"
            .Rotation = Matrix.Identity
            .RelativePosition = Vector3.Zero
            .OriginalRotation = Matrix.CreateFromAxisAngle(.Rotation.Left, 0.7)
            .DefualtRotation = Matrix.CreateFromAxisAngle(Vector3.Left, 0.7)
            .Type1 = Tool.Types.Sword
            .Length = 120
            .RewardsToVictim = New RewardSet(-10)
            .MaxAttackingDistance = 100
            .MinAttackingDistance = 80
            '______Change it in Tool.Buy too_______
        End With

        Tool.Tools.Add(RSword)
        Tool.iRSword = 2






        Dim Shovel As New Tool With {
            .Model = Content.Load(Of Model)("Models\Shovel")
        }
        Shovel.Transforms = SetupEffectDefaults(Shovel.Model, projectionMatrix, viewMatrix)


        With Shovel
            .Name = "Shovel"
            .Rotation = Matrix.Identity
            .RelativePosition = Vector3.Zero
            .OriginalRotation = Matrix.Identity
            .DefualtRotation = Matrix.Identity
            .Type1 = Tool.Types.Shovel
            .Length = 120
            .RewardsToVictim = New RewardSet(-5)
            .MaxAttackingDistance = 100
            .MinAttackingDistance = 80
            '______Change it in Tool.Buy too_______
        End With

        Tool.Tools.Add(Shovel)
        Tool.iShovel = 3









        Tool.InitializeAnimations()


#End Region


#Region "Sounds"

        'Dim NoiseLength = 999999
        'Dim Noise(NoiseLength) As Byte

        ''Dim a = 0R
        ''For n = 0 To NoiseLength
        ''    a = Math.Max(Math.Min(a + RND.Next(-1, 1) + RND.NextDouble, 255), 0)
        ''    Noise(n) = Int(a)
        ''Next

        'Dim X = 0
        'Dim Y = 255
        'Dim a = 0
        'Dim Frame = 0R
        'Dim Index = 0I
        'Dim OffSetBool = False
        'Dim Offset = 5I
        'For n = 0 To ((NoiseLength + 1) \ 10000) - 1

        '    For i = 0 To 9999
        '        Frame = i / 10000
        '        a = (X * (1 - Frame)) + (Y * Frame)

        '        If OffSetBool Then
        '            a = Math.Max(Math.Min(a + Offset, 255), 0)
        '        Else
        '            a = Math.Max(Math.Min(a - Offset, 255), 0)
        '        End If
        '        OffSetBool = Not OffSetBool


        '        Noise(Index) = CByte(a)
        '        Index += 1
        '    Next

        '    Y = X
        '    X = RND.Next(0, 256)
        'Next


        'TstSoundEff = New Audio.SoundEffect(Noise, 44100, Audio.AudioChannels.Mono)

        'TstSoundEff.CreateInstance.Play()


#End Region



#Region "Block Types"


        BlockType.Air = New BlockType With {
            .ID = 0,
            .IsAir = True,
            .Name = "Air"
        }
        BlockType.BTList(0) = BlockType.Air

        BlockType.PlaceHolder = New BlockType With {
            .ID = 0,
            .Varient = 1,
            .IsAir = False,
            .Name = "PlaceHolder"
        }
        BlockType.BTList(0) = BlockType.PlaceHolder


        Dim M() As Matrix


        BlockType.Dirt = New BlockType
        Dim DirtModel = Content.Load(Of Model)("Models\Dirt")
        BlockType.Dirt.Mesh = DirtModel.Meshes(0)
        M = New Matrix(DirtModel.Bones.Count - 1) {}
        DirtModel.CopyBoneTransformsTo(M)
        BlockType.Dirt.Transform = M.Last
        BlockType.Dirt.ID = 1
        BlockType.BTList(1) = BlockType.Dirt
        BlockType.Dirt.Name = "Dirt"





        BlockType.GrassBlock = New BlockType
        Dim GrassM = Content.Load(Of Model)("Models\GrassBlock")
        M = New Matrix(GrassM.Bones.Count - 1) {}
        BlockType.GrassBlock.Name = "GrassBlock"
        BlockType.GrassBlock.Mesh = GrassM.Meshes(0)
        DirtModel.CopyAbsoluteBoneTransformsTo(M)
        BlockType.GrassBlock.Transform = M.Last
        BlockType.GrassBlock.ID = 2
        BlockType.GrassBlock.nEffects = True
        BlockType.BTList(2) = BlockType.GrassBlock
        'BlockType.GrassBlock.IsAir = False


        BlockType.Sand = New BlockType
        Dim SandM = Content.Load(Of Model)("Models\Sand")
        M = New Matrix(SandM.Bones.Count - 1) {}
        BlockType.Sand.Name = "Sand"
        BlockType.Sand.Mesh = SandM.Meshes(0)
        SandM.CopyAbsoluteBoneTransformsTo(M)
        BlockType.Sand.Transform = M.Last
        BlockType.Sand.ID = 3
        BlockType.BTList(3) = BlockType.Sand
        'BlockType.Sand.IsAir = False



        BlockType.WoodPlank = New BlockType
        Dim WoodPlankModel = Content.Load(Of Model)("Models\WoodPlank")
        BlockType.WoodPlank.Mesh = WoodPlankModel.Meshes(0)
        M = New Matrix(WoodPlankModel.Bones.Count - 1) {}
        WoodPlankModel.CopyBoneTransformsTo(M)
        BlockType.WoodPlank.Transform = M.Last
        BlockType.WoodPlank.ID = 4
        BlockType.BTList(4) = BlockType.WoodPlank
        BlockType.WoodPlank.Name = "WoodPlank"


        BlockType.Brick = New BlockType
        Dim BrickModel = Content.Load(Of Model)("Models\Brick")
        BlockType.Brick.Mesh = BrickModel.Meshes(0)
        M = New Matrix(BrickModel.Bones.Count - 1) {}
        BrickModel.CopyBoneTransformsTo(M)
        BlockType.Brick.Transform = M.Last
        BlockType.Brick.ID = 5
        BlockType.BTList(5) = BlockType.Brick
        BlockType.Brick.Name = "Brick"



        BlockType.Stone = New BlockType
        Dim StoneModel = Content.Load(Of Model)("Models\Stone")
        BlockType.Stone.Mesh = StoneModel.Meshes(0)
        M = New Matrix(StoneModel.Bones.Count - 1) {}
        StoneModel.CopyBoneTransformsTo(M)
        BlockType.Stone.Transform = M.Last
        BlockType.Stone.ID = 6
        BlockType.BTList(6) = BlockType.Stone
        BlockType.Stone.Name = "Stone"




        BlockType.StoneWall = New BlockType
        Dim StoneWallModel = Content.Load(Of Model)("Models\StoneWall")
        BlockType.StoneWall.Mesh = StoneWallModel.Meshes(0)
        M = New Matrix(StoneWallModel.Bones.Count - 1) {}
        StoneWallModel.CopyBoneTransformsTo(M)
        BlockType.StoneWall.Transform = M.Last
        BlockType.StoneWall.ID = 7
        BlockType.BTList(7) = BlockType.StoneWall
        BlockType.StoneWall.Name = "StoneWall"


        BlockType.Tree1 = New BlockType
        Dim Tree1Model = Content.Load(Of Model)("Models\Tree1")
        BlockType.Tree1.Mesh = Tree1Model.Meshes(0)
        M = New Matrix(Tree1Model.Bones.Count - 1) {}
        Tree1Model.CopyBoneTransformsTo(M)
        BlockType.Tree1.Transform = M.Last
        BlockType.Tree1.ID = 8
        BlockType.BTList(8) = BlockType.Tree1
        BlockType.Tree1.Name = "Tree1"


        BlockType.Jak = New BlockType
        Dim JakModel = Content.Load(Of Model)("Models\Jak")
        BlockType.Jak.Mesh = JakModel.Meshes(0)
        M = New Matrix(JakModel.Bones.Count - 1) {}
        JakModel.CopyBoneTransformsTo(M)
        BlockType.Jak.Transform = M.Last
        BlockType.Jak.ID = 9
        BlockType.BTList(9) = BlockType.Jak
        BlockType.Jak.Name = "Jak"



        BlockType.Wood = New BlockType
        Dim WoodBlockModel = Content.Load(Of Model)("Models\Wood")
        BlockType.Wood.Mesh = WoodBlockModel.Meshes(0)
        M = New Matrix(WoodBlockModel.Bones.Count - 1) {}
        WoodBlockModel.CopyBoneTransformsTo(M)
        BlockType.Wood.Transform = M.Last
        BlockType.Wood.ID = 10
        BlockType.BTList(10) = BlockType.Wood
        BlockType.Wood.Name = "Wood"


#End Region



#Region "Setting up player"

        Player1 = New Player(EntityTypes.Player1) With {
            .Name = "Player1",
            .eType = EntityTypes.Player1}

        With Player1
            .ControlsList = Controls.NewControlList(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.Z)
            .ControlsList.Add(New Controls.Control(Controls.Control.MouseKeys.LeftClick, Actions.Attack))
            .ControlsList.Add(New Controls.Control(Controls.Control.MouseKeys.RightClick, Actions.PlaceBlock))
            .ControlsList.Add(New Controls.Control(Keys.E, Actions.BreakBlock))
            .ControlsList.Add(New Controls.Control(Controls.Control.MouseKeys.WheelUp, Actions.ChangeBlock))
            .ControlsList.Add(New Controls.Control(Keys.F, Actions.Interact))
            .ControlsList.Add(New Controls.Control(Keys.Q, Actions.C1))
            .ControlsList.Add(New Controls.Control(Keys.R, Actions.C2))
            .ControlsList.Add(New Controls.Control(Keys.T, Actions.C3))
            .Health = 1000
            .MaxHealth = 1000
            .NeededBodyRotationGainingSpeed = 0.1
            .CamPosType = RacingCameraAngle.Inside
            .Load(EntityTypes.Player1)
            .GiveTool(Tool.Buy(Tool.iRSword), 1)
            .SelectedBlockType = BlockType.Dirt
            Board.SelectedToolAndBlock = "Tool:" & Player1.CTool.Name & "; Block:" & Player1.SelectedBlockType.Name
            .CollitionHierarchy = eCollitionHierarchy.CreateNewHumanHierarchy(Player1)
            .Accelaration = New Vector3(3.0F, 3.0F, 3.5F)
            .Name = "Crazy Cat"
        End With




#End Region



#Region "World Loading/Genaration"


        'Try
        '    If My.Computer.FileSystem.GetDriveInfo("R").IsReady Then

        '        If Not My.Computer.FileSystem.DirectoryExists("R:\BEMaps") Then
        '            My.Computer.FileSystem.CreateDirectory("R:\BEMaps")
        '        End If

        '        My.Computer.FileSystem.CurrentDirectory = "R:\BEMaps"

        '        log("Using RAM Disk as the memory.")

        '    End If
        'Catch ex As Exception

        'End Try




        BiomeList.Initialize()
        Struct.Initialize()

        If MapVariablePipeline.NewMap Then
            Ground.Generate(CurrentMapName, MapVariablePipeline.NewMapSize, MapVariablePipeline.NewMapBiome, MapVariablePipeline.NewMapSpeedSave)

        Else
            Loader.LoadWorld(CurrentMapName)

        End If


#End Region



#Region "Summoning Entities"

        Dim IsPlayerAlreadySaved As Boolean = False

        Dim eLst As New List(Of Entity)
        Dim XeLst = XEntity.Load(Loader.FileEntity)

        For Each Xe In XeLst
            If Xe.TypeID <> EntityTypes.Player1.ID Then
                eLst.Add(Xe.GetEntity)

            Else

#Disable Warning BC41999 ' Implicit conversion from 'Entity' to 'Player' in copying the value of 'ByRef' parameter 'e' back to the matching argument.
                Xe.SetEntitySettings(Player1)
#Enable Warning BC41999 ' Implicit conversion from 'Entity' to 'Player' in copying the value of 'ByRef' parameter 'e' back to the matching argument.

                If Player1.IsDead Then
                    Player1.IsDead = False
                    Player1.Health = 1000
                    Player1.Money = 500
                End If

                eLst.Add(Player1)

                IsPlayerAlreadySaved = True
            End If
        Next

        If Not IsPlayerAlreadySaved Then
            eLst.Add(Player1)
        End If
        Player1.Position = Loader.MInfo.PlayerPosition


        If XeLst.Count > 0 Then
            For i = 0 To XeLst.Count - 1
                XeLst(i).GenerateRelationships(eLst(i), eLst)
            Next
        End If



        Ground.CStack.eList.AddRange(eLst)





        For n = 0 To InitialHumanCount - 1

            Dim Pos = RNDVec3(300, 300)
            Pos.Y = 500

            Dim H = Human.AddNewHuman(EntityTypes.Murderer, "Human" + n.ToString, Pos,
                                      Tool.Buy(RandomOf({Tool.iSword1, Tool.iRSword})), 1)

            H.AddToTheCurrentStack()
        Next

#End Region



#Region "Finalizing"

        Stack.Volume = Stack.Size * Chunk.Size * Ground.BlockSize
        MaxRenderDistance = (Stack.Volume / 2).Length


        Window.AllowAltF4 = False
#Disable Warning BC42016 ' Implicit conversion from 'Control' to 'Form'.
        FGame = Windows.Forms.Control.FromHandle(Window.Handle)
#Enable Warning BC42016 ' Implicit conversion from 'Control' to 'Form'.

        'HUI
        FHUI = New FrmHUI()
        FHUI.Apply(FGame)


        MouseDefPosition = New Point(Mouse.GetState.X, Mouse.GetState.Y)


        Dim Tmr10s As New Timers.Timer(10000)
        AddHandler Tmr10s.Elapsed, AddressOf Tmr10sTick
        Tmr10s.Start()

        Dim Tmr1s As New Timers.Timer(1000)
        AddHandler Tmr1s.Elapsed, AddressOf Tmr1sTick
        Tmr1s.Start()

        Dim Tmr200ms As New Timers.Timer(200)
        AddHandler Tmr200ms.Elapsed, AddressOf Tmr200msTick
        Tmr200ms.Start()


        STW.Start()


#End Region

    End Sub

    Protected Overrides Sub UnloadContent()

    End Sub


#End Region



#Region "Update"

    Public HRAngle As Single = 0
    Private MouseSwitchLast As Double = 0
    Private DevUtilsTime As Long = 0L


    Protected Overrides Sub Update(gameTime As GameTime)


#Region "Dev Utilities"

        NowGameTime = CLng(gameTime.TotalGameTime.TotalMilliseconds)

        Dim KBState = Keyboard.GetState


        If KBState.IsKeyDown(Keys.Escape) Then
            ExitApp()
            Exit Sub
        End If




        Dim M = Player1
        If KBState.IsKeyDown(Keys.P) Then
            If gameTime.TotalGameTime.TotalSeconds > MouseSwitchLast + 2 Then

                If MouseLookEnadbled Then
                    MouseLookEnadbled = False
                Else
                    MouseLookEnadbled = True
                End If

                MouseSwitchLast = gameTime.TotalGameTime.TotalSeconds

            End If
        End If


        If KBState.IsKeyDown(Keys.N) Then
            TimeOfTheDay += 4

        ElseIf KBState.IsKeyDown(Keys.M) Then
            TimeOfTheDay -= 4





        ElseIf KBState.IsKeyDown(Keys.H) Then
            If DevUtilsTime + 100 < NowGameTime Then
                Dim RPos = RNDVec3(100, -100)
                RPos.Y = 0
                Dim H = Human.AddNewHuman(EntityTypes.Human1, "Human" + Ground.CStack.eList.Count.ToString,
                                      Player1.Position + RPos,
                                      Tool.Buy(RandomOf({Tool.iSword1, Tool.iRSword})), 1)

                H.AddToTheCurrentStack()
                DevUtilsTime = NowGameTime
            End If



        ElseIf KBState.IsKeyDown(Keys.J) Then
            If DevUtilsTime + 100 < NowGameTime Then
                Dim RPos = RNDVec3(100, -100)
                RPos.Y = 0
                Dim H = Human.AddNewHuman(EntityTypes.Guard, "Guard" + Ground.CStack.eList.Count.ToString,
                                      Player1.Position + RPos,
                                      Tool.Buy(RandomOf({Tool.iSword1, Tool.iRSword})), 1)

                H.AddToTheCurrentStack()
                DevUtilsTime = NowGameTime
            End If



        ElseIf KBState.IsKeyDown(Keys.C) Then
            Dim FE = Player1.FacingEntity(200)
            If FE IsNot Nothing Then
                FE.DefendHere()
            End If


        ElseIf KBState.IsKeyDown(Keys.V) Then



        ElseIf KBState.IsKeyDown(Keys.B) Then
            For Each e In Player1.FacingEntities(50 * 50, 70)
                If e.Target <> Player1 Then
                    e.BeEnemies(Player1)
                    e.LockTarget(Player1, AITargetMode.FollowAndAttackOnce)
                End If
            Next

        ElseIf KBState.IsKeyDown(Keys.Y) Then
            Player1.Health = 1000

        ElseIf KBState.IsKeyDown(Keys.Z) Then
            FCP = cameraPosition




        ElseIf KBState.IsKeyDown(Keys.I) Then
            If DevUtilsTime + 100 < NowGameTime Then
                Board.IsDebugInfoVisible = Not Board.IsDebugInfoVisible
                DevUtilsTime = NowGameTime
            End If


        ElseIf KBState.IsKeyDown(Keys.F10) Then

            Loader.SaveChunks({Player1.CurrentChunk}, 1, False)

        ElseIf KBState.IsKeyDown(Keys.F9) Then

            If gameTime.TotalGameTime.TotalSeconds > MouseSwitchLast Then
                Dim SFn = 0
                If Player1.CurrentChunk.IsInTheSurface Then
                    Dim SFCLst = Ground.SurfaceChunks.ToList
                    SFCLst.Remove(Player1.CurrentChunk)
                    Ground.SurfaceChunks = SFCLst.ToArray

                    Ground.FilledSFC -= 1

                End If

                Loader.LoadAndReplaceChunk(Player1.CurrentChunk.Index)


                MouseSwitchLast = gameTime.TotalGameTime.TotalSeconds
            End If
        End If


#End Region



#Region "Mouse Controls"
        If MouseLookEnadbled Then
            Dim MouseGetStateX = Mouse.GetState.X

            Dim RotChange = Matrix.CreateRotationY(MathHelper.ToRadians((MouseDefPosition.X - MouseGetStateX) * MouseSensivity))

            If MouseGetStateX > MouseDefPosition.X Then

                M.HeadRotation *= RotChange
                M.NeededBodyRotation *= RotChange
                M.NeededBodyRotationChanged = True


            ElseIf Mouse.GetState.X < MouseDefPosition.X Then

                M.HeadRotation *= RotChange
                M.NeededBodyRotation *= RotChange
                M.NeededBodyRotationChanged = True

            End If


            Dim MouseGetStateY = Mouse.GetState.Y
            Dim Amount = MathHelper.ToRadians((MouseGetStateY - MouseDefPosition.Y) * MouseSensivity)

            If MouseGetStateY > MouseDefPosition.Y Then


                If HRAngle + Amount < 1.6 Then
                    'Looks Down


                    M.HeadRotation *= Matrix.CreateFromAxisAngle(M.HeadRotation.Left, Amount)
                    HRAngle += Amount
                    M.InsiderHandXCurrRotation = Amount
                    M.InsiderHandXRotation += Amount
                End If

            ElseIf MouseGetStateY < MouseDefPosition.Y Then


                If HRAngle + Amount > -1.4 Then
                    '  Looks Up
                    '-Amount
                    M.HeadRotation *= Matrix.CreateFromAxisAngle(M.HeadRotation.Left, Amount)
                    HRAngle += Amount
                    M.InsiderHandXCurrRotation = Amount
                    M.InsiderHandXRotation += Amount
                End If


            End If

            Mouse.SetPosition(100, 100)
            MouseDefPosition.X = 100
            MouseDefPosition.Y = 100

        End If


#End Region


#Region "Updates"

        Player1.UpdateMan(KBState, Mouse.GetState)


        For Each e In Ground.CStack.eList
            e.Update()
        Next



        Dim n = 0
        While n < ePAnimation.LstAnimation.Count
            ePAnimation.LstAnimation(n).Animate()
            n += 1
        End While

        For Each e In DeadEntities
            Ground.CStack.eList.Remove(e)
        Next








#End Region


#Region "Camera"
        If KBState.IsKeyDown(Keys.L) Then
            If LastCamSwapedTime + 300 < NowGameTime Then
                If Player1.CamPosType = RacingCameraAngle.Back Then
                    Player1.CamPosType = RacingCameraAngle.Inside
                    Player1.BodyParts(Human.iRHand).Rotation *= Matrix.CreateFromAxisAngle(Player1.ModelRotation.Left, -MathHelper.PiOver4)
                ElseIf Player1.CamPosType = RacingCameraAngle.Inside Then
                    Player1.CamPosType = RacingCameraAngle.Back
                    Player1.BodyParts(Human.iRHand).Rotation *= Matrix.CreateFromAxisAngle(Player1.ModelRotation.Left, MathHelper.PiOver4)
                End If
                LastCamSwapedTime = NowGameTime
            End If
        End If




        If Not Player1.IsDead Then


            If Player1.CamPosType = RacingCameraAngle.Back Then
                Dim P1CamPos = Player1.Position + (Player1.FacingDirection * -400) + (Vector3.Up * 450)


                cameraPosition = P1CamPos
                viewMatrix = Matrix.CreateLookAt(cameraPosition, (Player1.Position + (Player1.FacingDirection * 300)), Vector3.Up)

            ElseIf Player1.CamPosType = RacingCameraAngle.Inside Then
                cameraPosition = Player1.Position + CameraRelativeYPos  '+ (Player1.FacingDirection * -20)
                viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + Player1.FacingDirection, Player1.HeadRotation.Up)
            End If

        End If

#End Region

        MyBase.Update(gameTime)

        CurrUpdateCount += 1
    End Sub

#End Region




#Region "Draw"




    Public BF As New BoundingFrustum(viewMatrix * projectionMatrix)
    Public BS As New BoundingSphere(Vector3.Zero, 800)

    Public LastviewMatrix As Matrix = Matrix.Identity


    Protected Overrides Sub Draw(gameTime As GameTime)

#Region "Setting up"
        FPS = CInt(1000 / Math.Max((STW.ElapsedMilliseconds - STWLast), 1))
        STWLast = STW.ElapsedMilliseconds


        GraphicsDevice.Clear(BackColor)



        'GraphicsDevice.BlendState = BlendState.AlphaBlend
        'GraphicsDevice.DepthStencilState  = DepthStencilState.Default
        'GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise

        Dim IsViewChanged = (LastviewMatrix <> viewMatrix) Or IsGroundChanged
        IsGroundChanged = False

        LastviewMatrix = viewMatrix



#End Region


#Region "Draw Entities"

        BF = New BoundingFrustum(viewMatrix * projectionMatrix)
        'Dim RenderDistanceSq = RenderDistance * RenderDistance

        Player1.DrawMan()
        '(Vector3.DistanceSquared(e.Position, cameraPosition) < 1000000) OrElse

        For Each e In Ground.CStack.eList
            If BF.Intersects(New BoundingSphere(e.Position, 150.0F)) Then
                e.Draw()
            End If
        Next

#End Region



#Region "Draw Blocks"

        If IsViewChanged Then

            Board.Sampler = Board.MovingImageSampler



            'Reset arrays
            For i = 1 To BlockType.BTCount

                CountForBT(i) = 0
                ArraysForBT(i) = Nothing
            Next

            For i = 0 To nArrays
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Boolean'.
                ArrayUsage(i) = 0
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Boolean'.
            Next




            ScanChunks()

            AssignArraysForBlockTypes()

            CopyTo_ArraysForBt()

        Else

            Board.Sampler = Board.StillImageSampler

        End If



        DrawBlocksFromArrays()

#End Region

        Board.Draw()




        MyBase.Draw(gameTime)



    End Sub


    Private Sub ScanChunks()
        'Search and store visible chunks. Calculate the number of blocks in each chunk

        'on error GoTo ErrChunkArraySize

        Dim CountTransforms = 0
        nChunkArray = 0

        Dim RenderDistanceSquared = RenderDistance * RenderDistance

        Dim ch As Chunk
        For nCH = 0 To Ground.FilledSFC - 1
            ch = Ground.SurfaceChunks(nCH)

            If (ch IsNot Nothing) Then
                SyncLock ch
                    If (Not ch.disposedValue) Then

                        If Vector3.DistanceSquared(ch.Position, cameraPosition) < RenderDistanceSquared Then
                            BS.Center = ch.Position
                            If BF.Intersects(BS) Then

                                For i = 1 To BlockType.BTCount
                                    CountForBT(i) += ch.CountForBlockTypes(i)
                                Next

                                ChunkArray(nChunkArray) = ch
                                nChunkArray += 1

                            End If

                        End If
                    End If
                End SyncLock
            End If

        Next


        If Ground.TmpFilledSFC > 0 Then

            For nCH = 0 To Ground.TmpFilledSFC - 1
                ch = Ground.TmpSurfaceChunks(nCH)

                'If Not IsNothing(ch) Then
                If Vector3.DistanceSquared(ch.Position, cameraPosition) < RenderDistance * RenderDistance Then
                    BS.Center = ch.Position
                    If BF.Intersects(BS) Then

                        For i = 1 To BlockType.BTCount
                            CountForBT(i) += ch.CountForBlockTypes(i)
                        Next

                        ChunkArray(nChunkArray) = ch
                        nChunkArray += 1

                    End If

                End If
                'End If

            Next

        End If

        Exit Sub

ErrChunkArraySize:
        If nChunkArray > ChunkArray.Length - 5 Then
            ChunkArray = New Chunk(nChunkArray + 100) {}
            nChunkArray = 0
            ScanChunks()
        End If
        Exit Sub
    End Sub

    Private Sub AssignArraysForBlockTypes()
        'Assign matrix arrays for their block count 



        For i = 1 To BlockType.BTCount

            Dim Count = CountForBT(i)



            If Count = 0 Then
                ArraysForBT(i) = Nothing
                IVBsForBT(i) = Nothing
            Else

                Dim n = Count \ SizeOfRoom
                Dim m = n

                Do While ArrayUsage(m)
                    m += 1
                Loop

                ArraysForBT(i) = MAs(m)
                ArrayUsage(m) = True


                IVBsForBT(i) = IVBs(n)


            End If


            nForBT(i) = 0
        Next
    End Sub


    Private Sub CopyTo_ArraysForBt()
        'on error Resume Next
        Dim Ch As Chunk
        'Copy matrixes to ArraysForBT according to the block type

        Dim BTID As Byte = 0
        For i = 0 To nChunkArray - 1
            Ch = ChunkArray(i)
            'If Not ch.disposedValue Then

            For CI = 0 To Ch.BIDFilledI - 1

                BTID = Ch.BIDForBTranslations(CI)
                ArraysForBT(BTID)(nForBT(BTID)) = Ch.BlockTranslations(CI)
                nForBT(BTID) += 1

            Next

            'End If
        Next

    End Sub
    Public FCP As New Vector3

    Private Sub DrawBlocksFromArrays()
        'Draw blocks 
        ''on error GoTo ErrDrawingBlocks
        Dim InstancesCount = 0
        Dim BlockInstances() As Vector4
        Dim instanceVertexBuffer As DynamicVertexBuffer
        For n = 1 To BlockType.BTCount
            Dim BT1 = BlockType.BTList(n)
            InstancesCount = CountForBT(n)



            If InstancesCount <> 0 Then


                BlockInstances = ArraysForBT(n)
                instanceVertexBuffer = IVBsForBT(n)

                'For i = InstancesCount To BlockInstances.Length - 1
                '    BlockInstances(i) = Nothing
                'Next


                ' Transfer the latest instance transform matrices into the instanceVertexBuffer.
                instanceVertexBuffer.SetData(BlockInstances, 0, CountForBT(n), SetDataOptions.Discard)

                Dim MeshVertexBufferBinding(1) As VertexBufferBinding
                MeshVertexBufferBinding(1) = New VertexBufferBinding(instanceVertexBuffer, 0, 1)

                Dim effect As Effect
                For Each meshPart As ModelMeshPart In BT1.Mesh.MeshParts
                    MeshVertexBufferBinding(0) = New VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0)



                    ' Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
                    GraphicsDevice.SetVertexBuffers(MeshVertexBufferBinding)


                    GraphicsDevice.Indices = meshPart.IndexBuffer

                    ' Set up the instance rendering effect.
                    effect = meshPart.Effect

                    'effect.CurrentTechnique = effect.Techniques("HardwareInstancing")

                    'effect.Parameters("World").SetValue(BT1.Transform)
                    effect.Parameters(0).SetValue(viewMatrix)
                    effect.Parameters(1).SetValue(projectionMatrix)


                    'effect.Parameters("LightDirection1").SetValue(LD1)
                    'effect.Parameters("LightDirection2").SetValue(LD2)


                    effect.Parameters(2).SetValue(SunLightIntencity)

                    effect.Parameters(3).SetValue(cameraPosition)
                    effect.Parameters(4).SetValue(SunlightDirection)
                    'effect.Parameters("AmbientLight").SetValue(AmbientLightColor)


                    'Draw all the instance copies in a single call.
                    For Each pass As EffectPass In effect.CurrentTechnique.Passes
                        pass.Apply()
#Disable Warning BC40000 ' Type or member is obsolete
                        GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
                                                               meshPart.StartIndex, meshPart.PrimitiveCount,
                               CountForBT(n))
#Enable Warning BC40000 ' Type or member is obsolete
                    Next
                    'effect.CurrentTechnique.Passes.Item(0).Apply()

                    'GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
                    ' meshPart.StartIndex, meshPart.PrimitiveCount, BlockInstances.Length)

                Next



                InstancesCount = 0

            End If
        Next
    End Sub










#Region "Block Drawing storage"
    Const nArrays As Integer = 256
    Const SizeOfRoom As Integer = 256

    Public IVBs(nArrays) As DynamicVertexBuffer
    Public MAs(nArrays)() As Vector4



    Public ArrayUsage(nArrays) As Boolean


    Public ChunkArray(1500) As Chunk
    Public nChunkArray As Integer = 0

    Public CountForBT(BlockType.BTCount) As Integer

    Public nForBT(BlockType.BTCount) As Integer

    Public ArraysForBT(BlockType.BTCount)() As Vector4
    Public IVBsForBT(BlockType.BTCount) As DynamicVertexBuffer

    Shared ReadOnly instanceVertexDeclaration As New VertexDeclaration(
        New VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0))

#End Region



    Public Shared Sub DrawModel(model As Model, modelTransform As Matrix, absoluteBoneTransforms1 As Matrix())


        'Draw the model, a model can have multiple meshes, so loop
        For Each mesh As ModelMesh In model.Meshes
            'This is where the mesh orientation is set
            For Each effect As BasicEffect In mesh.Effects
                effect.Projection = projectionMatrix
                effect.View = viewMatrix
                effect.World = absoluteBoneTransforms1(mesh.ParentBone.Index) * modelTransform

            Next
            'Draw the mesh, will use the effects set above.
            mesh.Draw()
        Next


    End Sub


#End Region





#Region "Multi-Threading.Threads"

    Public Sub Tmr10sTick()

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

    End Sub



    Public Sub Tmr1sTick()


        For Each E In Ground.CStack.eList

            E.ExpensiveUpdate()

            If RunningSlow AndAlso E.IsDead AndAlso Not E.eType.IsPlayer AndAlso Not DeadEntities.Contains(E) Then
                DeadEntities.Add(E)
            End If


        Next

    End Sub



    Public Sub Tmr200msTick()


        If FPS < MinFPS Then
            If RenderDistance > MinRenderDistance Then RenderDistance -= 70

            RunningSlow = True

        ElseIf FPS > MaxFPS Then
            If RenderDistance < MaxRenderDistance Then RenderDistance += 50

            RunningSlow = False
        End If

        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(FOV, Viewport.AspectRatio, 1.0F, RenderDistance)


        TimeOfTheDay += 1
        SunLightIntencity = GetSunlightIntencity(TimeOfTheDay)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        BackColor = Color.FromNonPremultiplied(Color.SkyBlue.ToVector4 * Math.Min(1.2, SunLightIntencity))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

        If TimeOfTheDay > 1440 Then '24h
            TimeOfTheDay = 0
        End If

        Dim SunOrMoonAngle = MathHelper.WrapAngle((TimeOfTheDay - 720) / 1440 * MathHelper.TwoPi)

        SunlightDirection = Matrix.CreateFromAxisAngle(Vector3.Right, SunOrMoonAngle).Forward

        Dim RChIndex = Ground.ChunkIndexOfPosition(Player1.Position)

        If Not Ground.CStack.Scrolling Then
            Ground.CStack.Scroll(New IntVector3(RChIndex.X, 0, RChIndex.Z))
        End If


        FHUI.UpdateUI()


    End Sub


    Public Sub ExitGame()
        UnloadContent()
    End Sub


#End Region



End Class
