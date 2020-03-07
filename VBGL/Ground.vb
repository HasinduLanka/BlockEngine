Imports System.ComponentModel
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics


#Region "Blocks"

Public Class Block

    Public BID As Byte
    Public Varient As Byte

    Public IsAir As Boolean = True
    Public SurfaceRelation As Boolean = False

    Public CPosition As BVector3



    Public Shared Widening Operator CType(ByVal initialData As DBlock) As Block
        Return initialData.RealBlock
    End Operator



End Class



Public Class DBlock


    Public RealBlock As Block

    Public Chunk As Chunk

    Public ReadOnly Property Index As IntVector3
        Get
            Return RealBlock.CPosition + (Chunk.Index * 8)
        End Get
    End Property



    Public Shared Function FromBlock(initialData As Block) As DBlock
        Return New DBlock(initialData)
    End Function

    Public Shared Function FromBlock(initialData As Block, Ch As Chunk) As DBlock
        Return New DBlock(initialData) With {.Chunk = Ch}
    End Function



    Public Sub New(B As Block)
        RealBlock = B
    End Sub


End Class








Public Class BlockType
    'Public Model As Model
    Public Mesh As ModelMesh
    Public Transform As Matrix
    Public Name As String

    Public ID As Byte
    Public Varient As Byte

    Public nEffects As Boolean = False
    Public IsAir As Boolean = False

    Public Transparent As Boolean = False

    Public Shared Air As BlockType
    Public Shared PlaceHolder As BlockType


    Public Shared Sand As BlockType

    Public Shared Dirt As BlockType
    Public Shared GrassBlock As BlockType

    Public Shared WoodPlank As BlockType
    Public Shared Brick As BlockType
    Public Shared Stone As BlockType
    Public Shared StoneWall As BlockType
    Public Shared Tree1 As BlockType
    Public Shared Jak As BlockType
    Public Shared Wood As BlockType

    ''' <summary>
    ''' Actual block type count = BTCount + 1
    ''' </summary>
    Public Const BTCount As Integer = 10

    Public Shared BTList(BTCount) As BlockType


    Public Function NewBlock() As Block
        Dim B As New Block With {
            .BID = ID
        }

        Return B
    End Function

    Public Shared Function GetByBID(BID As Short) As BlockType
        Return BTList(BID)
    End Function


End Class


#End Region



Public Class Ground

    Public Shared GroundLvl As Single = 0
    Public Shared Genarated As Boolean = False
    Public Shared LastChunkIndex As Integer = 0

    'Public Shared WorldSize As IntVector3


    Public Shared CStack As Stack


    Public Const BlockSize As Integer = 50
    Public Shared BlockSizeV3 As New Vector3(Ground.BlockSize)
    Public Shared BlockSizeYonlyV3 As New Vector3(0, Ground.BlockSize, 0)
    Public Shared BlockSizeHalfYonlyV3 As New Vector3(0, Ground.BlockSize / 2, 0)

    Public Shared LastBlockIndex As Integer = 0

    Public Shared MaxHeight As Integer
    Public Shared ChunkBarHeight As Integer
    Public Shared BaseHeight As Integer = 10


    Public Shared FilledSFC As Integer = 0
    Public Shared SurfaceChunks(5000) As Chunk

    Public Shared TmpFilledSFC As Integer = 0
    Public Shared TmpSurfaceChunks(60) As Chunk

    Public Shared SpeedSaving As Boolean = True



    Public Shared Sub Generate(MapName As String, MapSize As Integer, IBM As Integer, SpeedSave As Boolean)



        Log("Generating a new world..")

        HeightMap.Size = MapSize * 8
        HeightMap.ByteCodeLength = (HeightMap.Size * HeightMap.Size) + 4

        Dim BM = BiomeList.Lst(IBM)

        Log("Generating Biome " + BM.Name)


        MaxHeight = (Stack.Size * Chunk.IntSize).Y
        ChunkBarHeight = Stack.Size.Y
        BM.MaxHeight = MaxHeight - BaseHeight - 10


        Loader.CreateMap(MapName)
        Dim XeList As New List(Of XEntity)

        '''''''''''''''''''''''''''''''''''''
        Dim HMG = Loader.GenareteHMap(BM, IntVector3.Zero)
        Loader.SaveHeightMap(HMG)
        HMG = Nothing

        Dim HM = Loader.LoadHeightMap(IntVector3.Zero)

        'Loader.SaveChunks(GenChunksFromHeightMap(HM, HeightMap.Size / 8), True)
        SpeedSaving = SpeedSave
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        GenAndSaveChunksFromHeightMap(HM, HeightMap.Size / 8, XeList)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

        HM = Nothing
        ''''''''''''''''''''''''''''''''''''''

        CStack = New Stack
        CStack.NewChunkList()


        CStack.LoadChunks(New IntVector3(Stack.Size.X, 0, Stack.Size.Z))



        GC.Collect()
        Genarated = True

        Log("Your new world is ready!")

    End Sub


    Public Shared Sub CheckAndGenerateOrLoad(MapName As String)
        If Loader.CheckIfMapExits(MapName) Then
            Loader.LoadWorld(MapName)
        Else
            Generate(MapName, MapVariablePipeline.NewMapSize, MapVariablePipeline.NewMapBiome, MapVariablePipeline.NewMapSpeedSave)
        End If
        Log("Started.")
    End Sub





    Public Shared StpWatch As New Stopwatch
    Public Shared Sub GenAndSaveChunksFromHeightMap(HM As HeightMap, nChunks As Integer, eList As List(Of XEntity))
        Log("Creating and saving chunks from height map. Length : " + nChunks.ToString)


        'Normal Saving methord - ~1572.44ms per chunk bar
        'Cached Saving methord -  ~911.17ms per chunk bar
        'Cached saving methord is 170% faster than normal saving. It needs ~400MB RAM for 100x100 world


        Log(If(SpeedSaving, "Speed Saving.", "Progressive Saving."))

        StpWatch.Start()

        Log("Starting...")


        Dim yChunks As Integer = ChunkBarHeight


        Dim BA = HM.B

        'Structure Byte Array
        Dim IsStructuredBiome = HM.Bm.Structs IsNot Nothing

        Dim SBars()()() As BlockType = Nothing
        Dim IsStructArray()() As Boolean = Nothing
        Dim SHeights()() As Byte = Nothing
        If IsStructuredBiome Then SBars = Struct.GenerateStructMap(HM, IsStructArray, SHeights, eList)

        Dim XChunkBars(nChunks - 1)() As Chunk

        For xCh = 0 To nChunks - 1
            Dim nXChunkBars = 0

            For zCh = 0 To nChunks - 1


                Dim ChunkBar(yChunks - 1) As Chunk


                For yCh = 0 To yChunks - 1

                    Dim CH = New Chunk With {.Index = New IntVector3(xCh, yCh, zCh) + (HM.Index * nChunks),
                        .Position = .Index * Chunk.Volume}

                    'Creating Block list from Air blocks
                    CH.BlockList = New Block(7)()() {}
                    For BX = 0 To 7
                        CH.BlockList(BX) = New Block(7)() {}
                        For BY = 0 To 7
                            CH.BlockList(BX)(BY) = New Block(7) {}
                            For BZ = 0 To 7

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
                                CH.BlockList(BX)(BY)(BZ) = New Block() With {.BID = 0, .IsAir = True,
                               .CPosition = New BVector3(BX, BY, BZ), .SurfaceRelation = False}
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.


                            Next
                        Next
                    Next


                    ChunkBar(yCh) = CH
                Next

                Dim ChIsAir = False
                Dim B As Block


                'CPoses
                For Bx = 0 To 7
                    For Bz = 0 To 7

                        'Block array X and Z
                        Dim BAX = Bx + xCh * 8
                        Dim BAZ = Bz + zCh * 8

                        'Height
                        Dim h = BA(BAX)(BAZ)



                        Dim yCh = 0
                        Dim Ch = ChunkBar(yCh)
                        For y = 0 To h - 1
                            If yCh <> Math.Truncate(y / 8) Then
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                                yCh = Math.Truncate(y / 8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                                Ch = ChunkBar(yCh)
                            End If

                            Dim BY = y - (yCh * 8)

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
                            B = New Block() With {.BID = HM.Bm.InnerBlock.ID, .Varient = HM.Bm.InnerBlock.Varient,
                                .CPosition = New BVector3(Bx, BY, Bz), .IsAir = False}
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.

                            Ch.BlockList(Bx)(BY)(Bz) = B

                            Ch.IsAir = False

                            If BAX > 0 Then

                                'Right
                                If BA(BAX - 1)(BAZ) < y Then
                                    SetBlockVisibleForGenarating(B, Ch)
                                Else
                                    If BAX < HeightMap.Size - 1 Then
                                        'Left
                                        If BA(BAX + 1)(BAZ) < y Then
                                            SetBlockVisibleForGenarating(B, Ch)
                                        Else
                                            If BAZ > 0 Then

                                                'forward
                                                If BA(BAX)(BAZ - 1) < y Then
                                                    SetBlockVisibleForGenarating(B, Ch)
                                                Else
                                                    If BAZ < HeightMap.Size - 1 Then

                                                        'Back
                                                        If BA(BAX)(BAZ + 1) < y Then
                                                            SetBlockVisibleForGenarating(B, Ch)
                                                        End If
                                                    End If
                                                End If
                                            Else
                                                SetBlockVisibleForGenarating(B, Ch)
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                SetBlockVisibleForGenarating(B, Ch)
                            End If






                        Next

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                        yCh = Math.Truncate(h / 8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

                        Ch = ChunkBar(yCh)

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
                        B = New Block() With {.BID = HM.Bm.SurfaceBlock.ID, .Varient = HM.Bm.SurfaceBlock.Varient,
                                .CPosition = New BVector3(Bx, h - (yCh * 8), Bz), .IsAir = False, .SurfaceRelation = False}
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.

                        SetBlockVisibleForGenarating(B, Ch)

                        Ch.BlockList(Bx)(B.CPosition.Y)(Bz) = B
                        Ch.IsAir = False



                        'Struct 
                        If IsStructuredBiome Then


                            If IsStructArray(BAX)(BAZ) Then

                                Dim SBar = SBars(BAX)(BAZ)
                                Dim CurSHeight = SHeights(BAX)(BAZ)
                                Dim StrctHeight = Math.Min(h + CurSHeight, MaxHeight)
                                Dim hStructMin = StrctHeight - CurSHeight

                                Dim Sn = 0
                                For SY = hStructMin To StrctHeight - 1

                                    Dim StrctBarBlock = SBar(Sn)
                                    If Not StrctBarBlock.IsAir Then
                                        Dim SY8 = Math.Truncate(SY / 8)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                                        Dim SCh = ChunkBar(SY8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
                                        Dim SB = New Block() With {.BID = StrctBarBlock.ID, .Varient = StrctBarBlock.Varient,
                                    .CPosition = New BVector3(Bx, SY - (SCh.Index.Y * 8), Bz), .IsAir = StrctBarBlock.IsAir,
                                    .SurfaceRelation = (StrctBarBlock.ID > 0)}
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.


                                        If SB.SurfaceRelation Then SetBlockVisibleForGenarating(SB, SCh)


                                        SCh.BlockList(Bx)(SB.CPosition.Y)(Bz) = SB
                                        SCh.IsAir = False
                                        'ChunkBar(SY8) = SCh


                                    End If
                                    Sn += 1
                                Next


                            End If

                        End If




                    Next
                Next


                XChunkBars(nXChunkBars) = ChunkBar
                nXChunkBars += 1




            Next

            If SpeedSaving Then
                For Each ChunkBar In XChunkBars
                    Loader.SaveChunkBarCachedAdd(ChunkBar, True)
                Next
                Loader.CheckAndSaveCachedChunkBars()

            Else
                For Each ChunkBar In XChunkBars
                    Loader.SaveChunkBar(ChunkBar, True)
                Next

            End If



            Log(xCh.ToString + " | ", False)

        Next

        XEntity.Save(Loader.FileEntity, eList)

        If SpeedSaving Then
            Log("Chunks genarated.")

            Loader.SaveCashedChunkBars()

            Log("Chunks Saved.")


        Else
            Log("Chunks genarated and saved.")

        End If


        Log(StpWatch.ElapsedMilliseconds.ToString & " ms Elapsed")
        StpWatch.Stop()
        'Return O




Err:



    End Sub







#Region "Functions"



    Public Shared Function RelCPos(CPos As BVector3, Rel As IntVector3, ByRef ChunkIndex As IntVector3) As BVector3
        Dim Out As IntVector3 = CPos + Rel
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        ChunkIndex += New IntVector3(Math.Truncate(Out.X / 8.0F), Math.Truncate(Out.Y / 8.0F), Math.Truncate(Out.Z / 8.0F))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Return Out Mod 8
    End Function


    Public Shared Function CPosOfIndex(Index As IntVector3, ByRef ChunkIndex As IntVector3) As IntVector3
        ChunkIndex = IntVector3.FromV3Truncated(Index / 8)
        Return Index Mod 8
    End Function

    Public Shared Function BlockIndexOfPosition(Position As Vector3) As IntVector3
        Return IntVector3.FromV3Rounded(Position / BlockSize)
    End Function

    Public Shared Function BlockIndexOfPosition(Position As Vector3, ByRef ChunkIndex As IntVector3) As IntVector3
        Dim Index = IntVector3.FromV3Rounded(Position / BlockSize)
        ChunkIndex = IntVector3.FromV3Truncated(Index / 8)
        Return Index
    End Function

    Public Shared Function CPosOfPosition(Position As Vector3, ByRef ChunkIndex As IntVector3) As IntVector3
        Dim Index = IntVector3.FromV3Rounded(Position / 50)
        ChunkIndex = IntVector3.FromV3Truncated(Index / 8)
        Return New IntVector3(Index.X Mod 8, Index.Y Mod 8, Index.Z Mod 8)
    End Function



    Public Shared Function ChunkIndexOfBlockIndex(Index As IntVector3) As IntVector3
        Return IntVector3.FromV3Truncated(Index / 8)
    End Function

    Public Shared Function ChunkIndexOfPosition(Pos As Vector3) As IntVector3
        Return IntVector3.FromV3Truncated(IntVector3.RoundV3(Pos / 50) / 8)
    End Function

    Public Shared Function ChunkIndexOfPositionDownABlock(Pos As Vector3) As IntVector3
        Dim O = IntVector3.FromV3Rounded(Pos / 50)
        O.Y -= 1
        Return IntVector3.FromV3Truncated(O / 8)
    End Function


    Public Class BlockEnvironment

        Public CurrentBlock As DBlock
        Public LegsBlock As Block
        Public BodyBlock As Block
        Public HeadBlock As Block

    End Class




    Public Shared Function GetBlockEnvironment(Position As Vector3,
                                               Optional SupressErrors As Boolean = False) As BlockEnvironment
        If SupressErrors Then
            'on error GoTo Err
        End If


        Dim ChI As New IntVector3
        Dim CPos = CPosOfPosition(Position, ChI)

        Dim O As New BlockEnvironment

        Dim Ch As Chunk = WaitAndGetChunk(ChI)
        If Ch Is Nothing Then
            Return Nothing
        End If
        O.CurrentBlock = DBlock.FromBlock(Ch.BlockList(CPos.X)(CPos.Y)(CPos.Z), Ch)



        Dim LegsChI = ChI
        Dim LegsCPos = RelCPos(CPos, IntVector3.Up, LegsChI)

        If LegsChI = ChI Then
            O.LegsBlock = Ch.BlockList(LegsCPos.X)(LegsCPos.Y)(LegsCPos.Z)
        Else
            Dim LegsCh = WaitAndGetChunk(LegsChI)
            If LegsCh Is Nothing Then
                Return Nothing
            End If
            O.LegsBlock = LegsCh.BlockList(LegsCPos.X)(LegsCPos.Y)(LegsCPos.Z)
        End If





        Dim BodyChI = LegsChI
        Dim BodyCPos = RelCPos(LegsCPos, IntVector3.Up, BodyChI)

        If BodyChI = ChI Then
            O.BodyBlock = Ch.BlockList(BodyCPos.X)(BodyCPos.Y)(BodyCPos.Z)
        Else
            Dim BodyCh = WaitAndGetChunk(BodyChI)
            If BodyCh Is Nothing Then
                Return Nothing
            End If
            O.BodyBlock = BodyCh.BlockList(BodyCPos.X)(BodyCPos.Y)(BodyCPos.Z)
        End If



        Dim HeadChI = BodyChI
        Dim HeadCPos = RelCPos(BodyCPos, IntVector3.Up, BodyChI)

        If HeadChI = ChI Then
            O.HeadBlock = Ch.BlockList(HeadCPos.X)(HeadCPos.Y)(HeadCPos.Z)
        Else
            Dim HeadCh = WaitAndGetChunk(HeadChI)
            If HeadCh Is Nothing Then
                Return Nothing
            End If
            O.HeadBlock = HeadCh.BlockList(HeadCPos.X)(HeadCPos.Y)(HeadCPos.Z)
        End If



        Return O

        Exit Function

Err:
        Return Nothing

    End Function

    Public Shared Function WaitAndGetChunk(ChI As IntVector3) As Chunk
        Dim Ch = Ground.CStack.GetChunk(ChI)
        If Ch Is Nothing Then
            Return Nothing
        End If
        If Ch.BlockList Is Nothing Then
            'Chunk still loading
            While Ch.BlockList Is Nothing
                Threading.Thread.Sleep(50)
                Ch = Ground.CStack.GetChunk(ChI)
            End While
        End If

        Return Ch
    End Function








    ''' <summary>
    ''' 0 = XNextIsAir , 1 = ZNextIsAir , 2 = XUpNextIsAir , 3 = ZUpNextIsAir
    ''' </summary>
    ''' <param name="Position"></param>
    ''' <param name="FacingDirections"></param>
    ''' <returns></returns>
    Public Shared Function GetFacingBlocks(Position As Vector3,
                                           FacingDirections() As Direction) As DBlock()

        ''on error GoTo ErrHandle


        Dim CP = (Position / Chunk.Volume)
        Dim CPX = CInt(Math.Truncate(CP.X))
        Dim CPY = CInt(Math.Truncate(CP.Y))
        Dim CPZ = CInt(Math.Truncate(CP.Z))




        Dim BP = ((Position - New Vector3(CPX * Chunk.VolumeI, CPY * Chunk.VolumeI, CPZ * Chunk.VolumeI)) / BlockSize)


        Dim BPX = CInt(BP.X)
        While BPX < 0
            CPX -= 1
            BPX += 8
        End While
        While BPX > 7
            CPX += 1
            BPX -= 8
        End While


        Dim BPY = CInt(BP.Y)
        While BPY < 0
            CPY -= 1
            BPY += 8
        End While
        While BPY > 7
            CPY += 1
            BPY -= 8
        End While


        Dim BPZ = CInt(BP.Z)
        While BPZ < 0
            CPZ -= 1
            BPZ += 8
        End While
        While BPZ > 7
            CPZ += 1
            BPZ -= 8
        End While




        'If BPY = 7 Then
        '    BPY = 0
        '    CPY += 1
        'Else
        '    BPY += 1
        'End If




        Dim BPY2 As Integer = 0
        Dim CPY2 As Integer = 0

        Dim CH As Chunk
        Dim CH2 As Chunk

        Dim O(3) As DBlock

        If BPY = 7 Then
            BPY2 = 0
            CPY2 = CPY + 1

        Else

            BPY2 = BPY + 1
            CPY2 = CPY
        End If


        'Facing directional Block X

        Dim BPXO = BPX
        Dim CPXO = CPX
        If FacingDirections(1) = Direction.Left Then
            If BPX = 0 Then
                CPX -= 1
                BPX = 7
            Else
                BPX -= 1

            End If

            CH = Ground.CStack.GetChunk(CPX, CPY, CPZ)
            If CH Is Nothing Then Return Nothing
            O(0) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
            O(0).Chunk = CH
            O(0).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)

            CH2 = Ground.CStack.GetChunk(CPX, CPY2, CPZ)
            If CH2 Is Nothing Then Return Nothing
            O(2) = DBlock.FromBlock(CH2.BlockList(BPX)(BPY2)(BPZ))
            O(2).Chunk = CH
            O(2).RealBlock.CPosition = New Vector3(BPX, BPY2, BPZ)


        ElseIf FacingDirections(1) = Direction.Right Then
            If BPX = 7 Then
                CPX += 1
                BPX = 0
            Else
                BPX += 1

            End If

            CH = Ground.CStack.GetChunk(CPX, CPY, CPZ)
            If CH Is Nothing Then Return Nothing
            O(0) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
            O(0).Chunk = CH
            O(0).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)


            CH2 = Ground.CStack.GetChunk(CPX, CPY2, CPZ)
            If CH2 Is Nothing Then Return Nothing
            O(2) = DBlock.FromBlock(CH2.BlockList(BPX)(BPY2)(BPZ))
            O(2).Chunk = CH
            O(2).RealBlock.CPosition = New Vector3(BPX, BPY2, BPZ)




        End If






        'Facing directional Block Z
        If FacingDirections(0) = Direction.Forward Then
            If BPZ = 0 Then
                CPZ -= 1
                BPZ = 7
            Else
                BPZ -= 1

            End If

            CH = Ground.CStack.GetChunk(CPXO, CPY, CPZ)
            If CH Is Nothing Then Return Nothing
            O(1) = DBlock.FromBlock(CH.BlockList(BPXO)(BPY)(BPZ))
            O(1).Chunk = CH
            O(1).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)


            CH2 = Ground.CStack.GetChunk(CPXO, CPY2, CPZ)
            If CH2 Is Nothing Then Return Nothing
            O(3) = DBlock.FromBlock(CH2.BlockList(BPXO)(BPY2)(BPZ))
            O(3).Chunk = CH
            O(3).RealBlock.CPosition = New Vector3(BPX, BPY2, BPZ)

        ElseIf FacingDirections(0) = Direction.Backward Then
            If BPZ = 7 Then
                CPZ += 1
                BPZ = 0
            Else
                BPZ += 1
            End If

            CH = Ground.CStack.GetChunk(CPXO, CPY, CPZ)
            If CH Is Nothing Then Return Nothing
            O(1) = DBlock.FromBlock(CH.BlockList(BPXO)(BPY)(BPZ))
            O(1).Chunk = CH
            O(1).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)

            CH2 = Ground.CStack.GetChunk(CPXO, CPY2, CPZ)
            If CH2 Is Nothing Then Return Nothing
            O(3) = DBlock.FromBlock(CH2.BlockList(BPXO)(BPY2)(BPZ))
            O(3).Chunk = CH
            O(3).RealBlock.CPosition = New Vector3(BPX, BPY2, BPZ)
        End If






        Return O

        Exit Function

ErrHandle: Return Nothing




    End Function


    ''' <summary>
    ''' 0= X, 1= Y, 2= UX, 3= UY
    ''' </summary>
    Public Shared Function GetFacingIsAir(Position As Vector3,
                                          FacingDirections() As Direction, D As Vector3) As Boolean()

        'on error GoTo ErrHandle



        Dim CP = (Position / Chunk.Volume)
        Dim CPX = CInt(Math.Truncate(CP.X))
        Dim CPY = CInt(Math.Truncate(CP.Y))
        Dim CPZ = CInt(Math.Truncate(CP.Z))



        Dim BP As Vector3 = (Position - New Vector3(CPX * Chunk.VolumeI,
                                                    CPY * Chunk.VolumeI,
                                                    CPZ * Chunk.VolumeI)) / BlockSize


        Dim BPX = CInt(BP.X)
        While BPX < 0
            CPX -= 1
            BPX += 8
        End While
        While BPX > 7
            CPX += 1
            BPX -= 8
        End While


        Dim BPY = CInt(BP.Y)
        While BPY < 0
            CPY -= 1
            BPY += 8
        End While
        While BPY > 7
            CPY += 1
            BPY -= 8
        End While


        Dim BPZ = CInt(BP.Z)
        While BPZ < 0
            CPZ -= 1
            BPZ += 8
        End While
        While BPZ > 7
            CPZ += 1
            BPZ -= 8
        End While







        Dim BPY2 As Integer = 0
        Dim CPY2 As Integer = 0

        Dim CH As Chunk
        Dim CH2 As Chunk

        Dim O(4) As Boolean

        If BPY = 7 Then
            BPY2 = 0
            CPY2 = CPY + 1

        Else

            BPY2 = BPY + 1
            CPY2 = CPY
        End If


        'Facing directional Block X

        Dim BPXO = BPX
        Dim CPXO = CPX
        If FacingDirections(1) = Direction.Left Then
            If BPX = 0 Then
                CPX -= 1
                BPX = 7
            Else
                BPX -= 1
            End If

        ElseIf FacingDirections(1) = Direction.Right Then
            If BPX = 7 Then
                CPX += 1
                BPX = 0
            Else
                BPX += 1
            End If


        End If


        CH = Ground.CStack.GetChunk(CPX, CPY, CPZ)
        If CH Is Nothing Then Return Nothing
        O(0) = CH.AirGrid(BPX)(BPY)(BPZ)

        CH2 = Ground.CStack.GetChunk(CPX, CPY2, CPZ)
        If CH2 Is Nothing Then Return Nothing
        O(2) = CH2.AirGrid(BPX)(BPY2)(BPZ)





        'Facing directional Block Z
        If FacingDirections(0) = Direction.Forward Then
            If BPZ = 0 Then
                CPZ -= 1
                BPZ = 7
            Else
                BPZ -= 1
            End If

        ElseIf FacingDirections(0) = Direction.Backward Then
            If BPZ = 7 Then
                CPZ += 1
                BPZ = 0
            Else
                BPZ += 1
            End If

        End If

        CH = Ground.CStack.GetChunk(CPXO, CPY, CPZ)
        If CH Is Nothing Then Return Nothing
        O(1) = CH.AirGrid(BPXO)(BPY)(BPZ)


        CH2 = Ground.CStack.GetChunk(CPXO, CPY2, CPZ)
        If CH2 Is Nothing Then Return Nothing
        O(3) = CH2.AirGrid(BPXO)(BPY2)(BPZ)


        'Upper XZ block
        CH2 = Ground.CStack.GetChunk(CPX, CPY2, CPZ)
        If CH2 Is Nothing Then Return Nothing
        O(4) = CH2.AirGrid(BPX)(BPY2)(BPZ)

        '0=x, 1=z, 2=Ux, 3=Uz, 4=Uxz




        Return O

        Exit Function

ErrHandle: Return Nothing




    End Function










    Public Shared Function GetBlockInTheDirection(Pos As Vector3, Dir As Vector3,
                                                  MaxDistance As Integer, IsAir As Boolean) As DBlock

        ''on error GoTo Err

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim RootI = New IntVector3(Math.Round(Pos.X / BlockSize), Math.Round(Pos.Y / BlockSize),
                                   Math.Round(Pos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

        Dim RootChI = RootI / Chunk.Size

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.X = Math.Truncate(RootChI.X)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Y = Math.Truncate(RootChI.Y)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Z = Math.Truncate(RootChI.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.




        Dir.Normalize()


        Dim O As DBlock = Nothing


        Dim CH As Chunk


        Dim NIndex As New Vector3
        Dim NChIndex As New IntVector3
        Dim CPos As New IntVector3
        Dim NPos = Pos


        ''Befores

        For n = 1 To MaxDistance

            NPos = Pos + (Dir * n)

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            NIndex = New IntVector3(Math.Round(NPos.X / BlockSize),
                                    Math.Round(NPos.Y / BlockSize), Math.Round(NPos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


            NChIndex = IntVector3.FromV3Truncated(NIndex / Chunk.Size)


            CPos = IntVector3.FromV3Truncated(NIndex - (NChIndex * Chunk.Size))


            CH = CStack.GetChunk(NChIndex.X, NChIndex.Y, NChIndex.Z)
            If CH Is Nothing Then Return Nothing


            If CH.AirGrid(CPos.X)(CPos.Y)(CPos.Z) = IsAir Then


                O = DBlock.FromBlock(CH.BlockList(CPos.X)(CPos.Y)(CPos.Z))
                O.RealBlock.CPosition = CPos
                O.Chunk = CH

                Return O
                Exit Function

            End If


        Next


        Return O

        Exit Function


Err:
        Return Nothing


    End Function





    Public Shared Function GetBeforeBlockInTheDirection(Pos As Vector3, Dir As Vector3,
                                                        MaxDistance As Integer, IsAir As Boolean) As DBlock
        'on error GoTo Err


#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim RootI = New IntVector3(Math.Round(Pos.X / BlockSize), Math.Round(Pos.Y / BlockSize),
                                   Math.Round(Pos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

        Dim RootChI = RootI / Chunk.Size

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.X = Math.Truncate(RootChI.X)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Y = Math.Truncate(RootChI.Y)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Z = Math.Truncate(RootChI.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.




        Dir.Normalize()


        Dim O As DBlock = Nothing


        Dim CH As Chunk


        Dim NIndex As New Vector3
        Dim NChIndex As New IntVector3
        Dim CPos As New IntVector3
        Dim NPos = Pos


        'Befores
        Dim BChunk As Chunk = Nothing
        Dim BCPos As IntVector3 = Nothing

        For n = 1 To MaxDistance

            NPos = Pos + (Dir * n)

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            NIndex = New IntVector3(Math.Round(NPos.X / BlockSize), Math.Round(NPos.Y / BlockSize),
                                    Math.Round(NPos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            'NIndex = RootI + (Dir * n)

            NChIndex = IntVector3.FromV3Truncated(NIndex / Chunk.Size)

#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.X = Math.Truncate(NChIndex.X)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.Y = Math.Truncate(NChIndex.Y)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.Z = Math.Truncate(NChIndex.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.




            CPos = IntVector3.FromV3Rounded(NIndex - (NChIndex * Chunk.Size))

            If CPos.X >= 8 Then
                CPos.X -= 8
                NChIndex.X += 1
            End If
            If CPos.Y >= 8 Then
                CPos.Y -= 8
                NChIndex.Y += 1
            End If
            If CPos.Z >= 8 Then
                CPos.Z -= 8
                NChIndex.Z += 1
            End If



            CH = CStack.GetChunk(NChIndex)
            If CH Is Nothing Then Return Nothing


            If CH.AirGrid(CPos.X)(CPos.Y)(CPos.Z) = IsAir Then
                'O = DBlock.FromBlock(CH.BlockList(CPos.X)(CPos.Y)(CPos.Z))
                'O.RealBlock.CPosition = CPos
                'O.Chunk = CH


                If IsNothing(BCPos) Then
                    Return Nothing
                    Exit Function

                Else
                    O = DBlock.FromBlock(BChunk.BlockList(BCPos.X)(BCPos.Y)(BCPos.Z))
                    O.RealBlock.CPosition = BCPos
                    O.Chunk = BChunk

                    Return O
                    Exit Function

                End If




            Else

                BCPos = CPos
                BChunk = CH

            End If






        Next





        Return O


        Exit Function

Err:
        Return Nothing

    End Function



    Public Shared Function GetIsAirDistanceInTheDirection(Pos As Vector3, Dir As Vector3,
                                                          MaxDistance As Integer, IsAir As Boolean) As Integer

        ''on error GoTo Err

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        Dim RootI = IntVector3.FromV3Rounded(Math.Round(Pos.X / BlockSize), Math.Round(Pos.Y / BlockSize),
                                   Math.Round(Pos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.

        Dim RootChI = RootI / Chunk.Size

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.X = Math.Truncate(RootChI.X)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Y = Math.Truncate(RootChI.Y)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        RootChI.Z = Math.Truncate(RootChI.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.




        Dir.Normalize()


        Dim O As DBlock = Nothing


        Dim CH As Chunk





        Dim NIndex As New Vector3
        Dim NChIndex As New IntVector3
        Dim CPos As New IntVector3
        Dim NPos = Pos





        For n = 1 To MaxDistance Step Math.Sign(MaxDistance)

            NPos = Pos + (Dir * n)

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            NIndex = New IntVector3(Math.Round(NPos.X / BlockSize),
                                    Math.Round(NPos.Y / BlockSize), Math.Round(NPos.Z / BlockSize))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


            NChIndex = IntVector3.FromV3Truncated(NIndex / Chunk.Size)

#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.X = Math.Truncate(NChIndex.X)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.Y = Math.Truncate(NChIndex.Y)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
            NChIndex.Z = Math.Truncate(NChIndex.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.




            CPos = IntVector3.FromV3Rounded(NIndex - (NChIndex * Chunk.Size))

            If CPos.X >= 8 Then
                CPos.X -= 8
                NChIndex.X += 1
            End If
            If CPos.Y >= 8 Then
                CPos.Y -= 8
                NChIndex.Y += 1
            End If
            If CPos.Z >= 8 Then
                CPos.Z -= 8
                NChIndex.Z += 1
            End If



            CH = CStack.GetChunk(NChIndex)
            If CH Is Nothing Then Return -1


            If CH.AirGrid(CPos.X)(CPos.Y)(CPos.Z) = IsAir Then


                Return n
                Exit Function

            End If


        Next


        Return -1

        Exit Function


Err:
        Return -2


    End Function











    Public Shared Sub SetBlock(ByRef B As DBlock, BT As BlockType)



        If B.RealBlock.SurfaceRelation Then


            Dim SBLst = B.Chunk.SurfaceBlocks.ToList
            SBLst.Remove(B.RealBlock)
            B.Chunk.SurfaceBlocks = SBLst.ToArray

            B.Chunk.FilledSB -= 1


            For n = 0 To B.Chunk.BIDFilledI - 1

                Dim BArr = B.Chunk.BIDList(n)

                If BArr(0) = B.RealBlock.CPosition.X AndAlso BArr(1) = B.RealBlock.CPosition.Y AndAlso
                    BArr(2) = B.RealBlock.CPosition.Z Then

                    Dim BArrLst = B.Chunk.BIDList.ToList
                    BArrLst.RemoveAt(n)
                    BArrLst.Add(Nothing)
                    B.Chunk.BIDList = BArrLst.ToArray

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
                    B.Chunk.BIDFilledI -= 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.

                    Dim BlockTranslationsLst = B.Chunk.BlockTranslations.ToList
                    BlockTranslationsLst.RemoveAt(n)
                    B.Chunk.BlockTranslations = BlockTranslationsLst.ToArray


                    B.Chunk.CountForBlockTypes(B.RealBlock.BID) -= 1

                    Exit For

                End If



            Next



        End If






        If BT.ID <> 0 Then
            B.RealBlock.SurfaceRelation = True
        Else
            B.RealBlock.SurfaceRelation = False
        End If

        B.RealBlock.IsAir = BT.IsAir
        B.RealBlock.BID = BT.ID



        B.Chunk.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) = BT.IsAir
        B.Chunk.BlockList(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) = B.RealBlock



        If Not B.Chunk.IsInTheSurface Then
            B.Chunk.IsInTheSurface = True
            B.Chunk.SurfaceChunkIndex = FilledSFC
            SurfaceChunks(FilledSFC) = B.Chunk
            FilledSFC += 1
        End If


        If B.RealBlock.SurfaceRelation Then
            B.Chunk.SurfaceBlocks(B.Chunk.FilledSB) = B.RealBlock
            B.Chunk.FilledSB += 1
        End If


        B.Chunk.Changed = True


        CStack.SavingChunks(CStack.nSavingChunks) = B.Chunk
        CStack.nSavingChunks += 1
        B.Chunk.Removed = False



        If B.RealBlock.SurfaceRelation Then


            Dim BBlock(4) As Byte

            BBlock(0) = B.RealBlock.CPosition.X
            BBlock(1) = B.RealBlock.CPosition.Y
            BBlock(2) = B.RealBlock.CPosition.Z
            BBlock(3) = BT.ID
            BBlock(4) = BT.Varient

            B.Chunk.BIDList(B.Chunk.BIDFilledI) = BBlock

            If B.Chunk.BIDList.Length < B.Chunk.BIDFilledI + 10 Then
                Array.Resize(B.Chunk.BIDList, B.Chunk.BIDList.Length + 20)
            End If


            Dim BWorld = New Vector4(BBlock(0) * 50 + B.Chunk.Position.X, BBlock(1) * 50 + B.Chunk.Position.Y,
                                                              BBlock(2) * 50 + B.Chunk.Position.Z, 1)
            Array.Resize(B.Chunk.BlockTranslations, B.Chunk.BIDFilledI + 1)
            B.Chunk.BlockTranslations(B.Chunk.BIDFilledI) = BWorld
            B.Chunk.CountForBlockTypes(BT.ID) += 1

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
            B.Chunk.BIDFilledI += 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
            B.Chunk.GenerateBIDForBTranslations()
        End If

        IsGroundChanged = True
    End Sub




    Public Shared Sub BreakBlock(ByRef B As DBlock)



        If B.RealBlock.SurfaceRelation Then


            Dim SBLst = B.Chunk.SurfaceBlocks.ToList
            SBLst.Remove(B.RealBlock)
            B.Chunk.SurfaceBlocks = SBLst.ToArray

            B.Chunk.FilledSB -= 1


            For n = 0 To B.Chunk.BIDFilledI - 1

                Dim BArr = B.Chunk.BIDList(n)

                If BArr(0) = B.RealBlock.CPosition.X AndAlso BArr(1) _
                    = B.RealBlock.CPosition.Y AndAlso BArr(2) = B.RealBlock.CPosition.Z Then

                    Dim BArrLst = B.Chunk.BIDList.ToList
                    BArrLst.RemoveAt(n)
                    BArrLst.Add(Nothing)
                    B.Chunk.BIDList = BArrLst.ToArray


#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
                    B.Chunk.BIDFilledI -= 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.

                    Dim BlockTranslationsLst = B.Chunk.BlockTranslations.ToList
                    BlockTranslationsLst.RemoveAt(n)
                    B.Chunk.BlockTranslations = BlockTranslationsLst.ToArray


                    B.Chunk.CountForBlockTypes(B.RealBlock.BID) -= 1

                    Exit For

                End If



            Next


            B.Chunk.GenerateBIDForBTranslations()
        End If



        B.RealBlock.BID = 0
        B.RealBlock.IsAir = True
        B.RealBlock.SurfaceRelation = False

        B.Chunk.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) = True
        B.Chunk.BlockList(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) = B.RealBlock



        B.Chunk.Changed = True
        'If Not B.Chunk.Changed Then

        CStack.SavingChunks(CStack.nSavingChunks) = B.Chunk
        CStack.nSavingChunks += 1
        B.Chunk.Removed = False

        'End If


        ScanBlockVisibilityChanges(B)



        IsGroundChanged = True


    End Sub



    Public Shared Sub SetBlockVisible(B As DBlock)


        If B.RealBlock.SurfaceRelation = False Then



            If Not B.Chunk.IsInTheSurface Then
                B.Chunk.IsInTheSurface = True
                B.Chunk.SurfaceChunkIndex = FilledSFC
                SurfaceChunks(FilledSFC) = B.Chunk
                FilledSFC += 1
            End If


            B.RealBlock.SurfaceRelation = B.RealBlock.BID <> 0


            B.Chunk.Changed = True

            If B.RealBlock.SurfaceRelation Then

                B.Chunk.SurfaceBlocks(B.Chunk.FilledSB) = B.RealBlock
                B.Chunk.FilledSB += 1



                Dim BBlock(4) As Byte

                BBlock(0) = B.RealBlock.CPosition.X
                BBlock(1) = B.RealBlock.CPosition.Y
                BBlock(2) = B.RealBlock.CPosition.Z
                BBlock(3) = B.RealBlock.BID
                BBlock(4) = B.RealBlock.Varient

                B.Chunk.BIDList(B.Chunk.BIDFilledI) = BBlock


                If B.Chunk.BIDList.Length < B.Chunk.BIDFilledI + 10 Then
                    Array.Resize(B.Chunk.BIDList, B.Chunk.BIDList.Length + 40)
                End If


                Dim BWorld = New Vector4(BBlock(0) * 50 + B.Chunk.Position.X, BBlock(1) * 50 + B.Chunk.Position.Y,
                                                          BBlock(2) * 50 + B.Chunk.Position.Z, 1)
                Array.Resize(B.Chunk.BlockTranslations, B.Chunk.BIDFilledI + 1)
                B.Chunk.BlockTranslations(B.Chunk.BIDFilledI) = BWorld
                B.Chunk.CountForBlockTypes(B.RealBlock.BID) += 1

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
                B.Chunk.BIDFilledI += 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.

                B.Chunk.GenerateBIDForBTranslations()
            End If

        End If

        IsGroundChanged = True
    End Sub


    Public Shared Sub SetBlockVisibleForGenarating(ByRef B As Block, ByRef Ch As Chunk)


        'If B.SurfaceRelation = False Then



        Ch.IsInTheSurface = True


        B.SurfaceRelation = True

        Ch.SurfaceBlocks(Ch.FilledSB) = B
        Ch.FilledSB += 1



        Dim BBlock(4) As Byte

        BBlock(0) = B.CPosition.X
        BBlock(1) = B.CPosition.Y
        BBlock(2) = B.CPosition.Z
        BBlock(3) = B.BID
        BBlock(4) = B.Varient

        Ch.BIDList(Ch.BIDFilledI) = BBlock

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
        Ch.BIDFilledI += 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
        If Ch.BIDList.Length < Ch.BIDFilledI + 10 Then
            Array.Resize(Ch.BIDList, Ch.BIDList.Length + 40)
        End If



        'End If

    End Sub



    Public Shared Sub PutChunkVisible(Ch As Chunk)
        If Not Ch.IsInTheSurface Then
            Ch.IsInTheSurface = True
            Ch.SurfaceChunkIndex = FilledSFC
            SurfaceChunks(FilledSFC) = Ch
            FilledSFC += 1
            Ch.Changed = True
        End If
        IsGroundChanged = True
    End Sub


    Public Shared V3_7X As New Vector3(7, 0, 0)
    Public Shared V3_7Y As New Vector3(0, 7, 0)
    Public Shared V3_7Z As New Vector3(0, 0, 7)


    Public Shared Sub ScanBlockVisibilityChanges(B As DBlock)
        'on error Resume Next
        Dim Ch = B.Chunk

        Dim CH2 As Chunk

Right:

        If B.RealBlock.CPosition.X = 7 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X + 1, B.Chunk.Index.Y, B.Chunk.Index.Z)
            If CH2 Is Nothing Then GoTo Left
            If Not CH2.AirGrid(0)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(0)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition - V3_7X
                SetBlockVisible(DB)
            End If

        Else

            If Not Ch.AirGrid(B.RealBlock.CPosition.X + 1)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X + 1) _
                    (B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Right
                SetBlockVisible(DB)
            End If

        End If




Left:
        If B.RealBlock.CPosition.X = 0 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X - 1, B.Chunk.Index.Y, B.Chunk.Index.Z)
            If CH2 Is Nothing Then GoTo Up
            If Not CH2.AirGrid(7)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(7)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + V3_7X
                SetBlockVisible(DB)
            End If

        Else
            If Not Ch.AirGrid(B.RealBlock.CPosition.X - 1)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X - 1) _
                                          (B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Left
                SetBlockVisible(DB)
            End If
        End If



Up:

        If B.RealBlock.CPosition.Y = 7 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X, B.Chunk.Index.Y + 1, B.Chunk.Index.Z)
            If CH2 Is Nothing Then GoTo Down
            If Not CH2.AirGrid(B.RealBlock.CPosition.X)(0)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(B.RealBlock.CPosition.X)(0)(B.RealBlock.CPosition.Z), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition - V3_7Y
                SetBlockVisible(DB)
            End If

        Else

            If Not Ch.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y + 1)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X) _
                    (B.RealBlock.CPosition.Y + 1)(B.RealBlock.CPosition.Z), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Up
                SetBlockVisible(DB)
            End If
        End If



Down:
        If B.RealBlock.CPosition.Y = 0 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X, B.Chunk.Index.Y - 1, B.Chunk.Index.Z)
            If CH2 Is Nothing Then GoTo Back
            If Not CH2.AirGrid(B.RealBlock.CPosition.X)(7)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(B.RealBlock.CPosition.X)(7)(B.RealBlock.CPosition.Z), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + V3_7Y
                SetBlockVisible(DB)
            End If

        Else
            If Not Ch.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y - 1)(B.RealBlock.CPosition.Z) Then

                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X) _
                    (B.RealBlock.CPosition.Y - 1)(B.RealBlock.CPosition.Z), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Down
                SetBlockVisible(DB)
            End If
        End If





Back:
        If B.RealBlock.CPosition.Z = 7 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X, B.Chunk.Index.Y, B.Chunk.Index.Z + 1)
            If CH2 Is Nothing Then GoTo Fow
            If Not CH2.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(0) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(0), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition - V3_7Z
                SetBlockVisible(DB)
            End If

        Else

            If Not Ch.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z + 1) Then
                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X) _
                    (B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z + 1), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Backward
                SetBlockVisible(DB)
            End If
        End If


Fow:
        If B.RealBlock.CPosition.Z = 0 Then

            CH2 = CStack.GetChunk(B.Chunk.Index.X, B.Chunk.Index.Y, B.Chunk.Index.Z - 1)
            If CH2 Is Nothing Then Return
            If Not CH2.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(7) Then

                Dim DB = DBlock.FromBlock(CH2.BlockList(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(7), CH2)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + V3_7Z
                SetBlockVisible(DB)
            End If

        Else
            'Fow
            If Not Ch.AirGrid(B.RealBlock.CPosition.X)(B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z - 1) Then

                Dim DB = DBlock.FromBlock(Ch.BlockList(B.RealBlock.CPosition.X) _
                    (B.RealBlock.CPosition.Y)(B.RealBlock.CPosition.Z - 1), Ch)
                DB.RealBlock.CPosition = B.RealBlock.CPosition + Vector3.Forward
                SetBlockVisible(DB)
            End If
        End If
        IsGroundChanged = True
    End Sub

#End Region


End Class




