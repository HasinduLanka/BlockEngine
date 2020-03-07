Public Class Loader


    Public Shared MapName As String
    Public Shared MapDir As String
    Public Shared MapRoot As String
    Public Shared FileEntity As String

    Public Shared MapInfoFile As String
    Public Shared MInfo As MapInfo



    Public Shared LoadedChunks() As IntVector3
    Public Shared nLoadedChunks As Integer = 0

    Public Shared SavedHMapFile As String
    Public Shared SavedHMaps As New List(Of IntVector3)
    Public Shared LoadedHMaps As New List(Of HeightMap)

    Public Shared MaxWorldBorders As IntVector3


    Public Shared ExtChunk As String = ".C"
    Public Shared ExtHeightMap As String = ".HM"


    Public Shared SW As IO.StreamWriter
    Public Shared SR As IO.StreamReader

    Public Shared XS As Xml.Serialization.XmlSerializer

    Public Shared ChunkByteCodeLength As Integer = (512 * 3) + 2

#Region "Map Funcs"

    Public Shared Function CheckIfMapExits(MMapName As String) As Boolean


        If My.Computer.FileSystem.DirectoryExists("Maps/" + MMapName) Then
            If My.Computer.FileSystem.FileExists("Maps/" + MMapName + "/I.Map") Then
                Return True
            End If
        End If

        Return False
    End Function


    Public Shared Sub CreateMap(MMapName As String)

        MapName = MMapName
        MInfo = New MapInfo With {
            .Name = MapName,
            .HMapSize = HeightMap.Size,
            .PlayerPosition = New Microsoft.Xna.Framework.Vector3(50 * 8 * Stack.Size.X / 2, 1000, 50 * 8 * Stack.Size.Z / 2)
        }
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.



        MapDir = "Maps/" + MapName
        MapRoot = MapDir + "/"
        MapInfoFile = MapRoot + "I.Map"
        'SavedChunksFile = MapRoot + "SC"
        SavedHMapFile = MapRoot + "SHM"
        FileEntity = MapRoot & "E.xml"

        My.Computer.FileSystem.CreateDirectory(MapDir)

        MInfo.Save(MapInfoFile)

        XEntity.Save(FileEntity, New List(Of XEntity))


        If My.Computer.FileSystem.FileExists(SavedHMapFile) Then
            Dim Lst = My.Computer.FileSystem.ReadAllText(SavedHMapFile).Split(";"c).ToList
            Lst.RemoveAt(Lst.Count - 1)
            SavedHMaps = SavedNamesToIndices(Lst.ToArray).ToList

            Dim MaxHMID As IntVector3 = IntVector3.Zero
            For Each HM In SavedHMaps
                If MaxHMID.X < HM.X + 1 AndAlso MaxHMID.Z < HM.Z + 1 Then
                    MaxHMID = New IntVector3(HM.X + 1, 0, HM.Z + 1)
                End If
            Next

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            MaxWorldBorders = MaxHMID * (HeightMap.Size / 8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

        Else
            SavedHMaps = New List(Of IntVector3)
            My.Computer.FileSystem.WriteAllText(SavedHMapFile, "", False)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
            MaxWorldBorders = New IntVector3(HeightMap.Size / 8, Ground.MaxHeight, HeightMap.Size / 8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        End If




    End Sub




    Public Shared Sub LoadWorld(MMapName As String)

        'on error GoTo Err

        Log("Loading your world")

        MapName = MMapName


        MapDir = "Maps/" + MapName
        MapRoot = MapDir + "/"
        MapInfoFile = MapRoot + "I.Map"
        'SavedChunksFile = MapRoot + "SC"
        SavedHMapFile = MapRoot + "SHM"
        FileEntity = MapRoot & "E.xml"

        MInfo = MapInfo.Load(MapInfoFile)

        If MInfo Is Nothing Then
            Log("Error occured!. World Information file is corrupted. Creating a new Map info file.")

            MInfo = New MapInfo With {
                .Name = MapName,
                .HMapSize = HeightMap.Size,
                .PlayerPosition = New Microsoft.Xna.Framework.Vector3(50 * 8 * Stack.Size.X / 2, 1000, 50 * 8 * Stack.Size.Z / 2)
            }
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
            MInfo.Save(MapInfoFile)

            MInfo = MapInfo.Load(MapInfoFile)

        End If

        HeightMap.Size = MInfo.HMapSize

        If Stack.Size.X >= (HeightMap.Size \ 8) Then
            Dim NewSS = (HeightMap.Size \ 8) - 1
            Stack.Size.X = NewSS
            Stack.Size.Z = NewSS
            Stack.Volume = Stack.Size * Chunk.Size * Ground.BlockSize
        End If

        Ground.MaxHeight = (Stack.Size * Chunk.IntSize).Y
        Ground.ChunkBarHeight = Stack.Size.Y



        Dim LstHM = My.Computer.FileSystem.ReadAllText(SavedHMapFile).Split(";"c).ToList
        LstHM.RemoveAt(LstHM.Count - 1)
        SavedHMaps = SavedNamesToIndices(LstHM.ToArray).ToList

        Dim MaxHMID As IntVector3 = IntVector3.Zero
        For Each HM In SavedHMaps
            If MaxHMID.X < HM.X + 1 AndAlso MaxHMID.Z < HM.Z + 1 Then
                MaxHMID = New IntVector3(HM.X + 1, 0, HM.Z + 1)
            End If
        Next

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        MaxWorldBorders = MaxHMID * (HeightMap.Size / 8)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


        Ground.CStack = New Stack
        'Ground.CStack.eList.AddRange(eLst)


        Ground.CStack.NewChunkList()




        Ground.FilledSFC = 0
        Ground.SurfaceChunks = New Chunk(9000) {}

        Ground.CStack.LoadChunks(Ground.ChunkIndexOfPosition(MInfo.PlayerPosition))

        Ground.Genarated = True

        Log("World loaded!")

        Exit Sub
Err:
        'Log("Map files are damaged! Plz delete them on  " + My.Computer.FileSystem.CurrentDirectory)
        'Game1.Game.ExitApp()

        Log("Error occured!. Delete the world or overwrite it")


    End Sub


#End Region


#Region "Save"


    Public Shared Sub SaveChunkBar(Ch() As Chunk, Optional Dispose As Boolean = False)

        Dim ChunkBarByteCode((ChunkByteCodeLength * Ch.Length) - 1) As Byte

        For n = 0 To Ch.Length - 1
            Array.Copy(GenarateChunkByteCode(Ch(n)), 0, ChunkBarByteCode, ChunkByteCodeLength * n, ChunkByteCodeLength)

            If Dispose Or Ch(n).Removed Then
                Ch(n).Dispose()
            End If

        Next

        My.Computer.FileSystem.WriteAllBytes(ChunkBarPathOf(Ch(0)), ChunkBarByteCode, False)

    End Sub

    Public Shared Function GenarateChunkBarByteCode(Ch() As Chunk) As Byte()

        Dim ChunkBarByteCode((ChunkByteCodeLength * Ch.Length) - 1) As Byte

        For n = 0 To Ch.Length - 1
            Array.Copy(GenarateChunkByteCode(Ch(n)), 0, ChunkBarByteCode, ChunkByteCodeLength * n, ChunkByteCodeLength)
        Next

        Return ChunkBarByteCode

    End Function

    Public Shared Sub SaveChunks(Chunks() As Chunk, Count As Integer, DisposeChunks As Boolean)
        Dim BarCount = 0I
        Dim Bars = CreateChunkBarsFromChunks(Chunks, Count, BarCount)

        For n = 0 To BarCount - 1

            SaveChunkBar(Bars(n), DisposeChunks)

        Next


    End Sub


    Private Shared Function CreateChunkBarsFromChunks(Chunks() As Chunk, Count As Integer, ByRef BarCount As Integer) As Chunk()()

        Dim nBars = 0I
        Dim Bars(Count - 1)() As Chunk
        Dim BarIndices(Count - 1) As IntVector3


        For n = 0 To Count - 1

            Dim Ch = Chunks(n)
            Dim ChBarIndex = New IntVector3(Ch.Index.X, 0, Ch.Index.Z)

            If BarIndices.Contains(ChBarIndex) Then

                For m = 0 To nBars - 1

                    If BarIndices(m) = ChBarIndex Then
                        Bars(m)(Ch.Index.Y) = Ch

                    End If

                Next


            Else

                'Get Chunk Bar from Stack
                Dim ChBar = Ground.CStack.GetChunkBar(Ch.Index)

                If ChBar Is Nothing Then
                    'Ch Bar is not in the stack. Load it
                    ChBar = CompileChunkBarByteCode(LoadFile(ChunkBarPathOf(ChBarIndex)), ChBarIndex)
                End If

                Bars(nBars) = ChBar
                Bars(nBars)(Ch.Index.Y) = Ch
                BarIndices(nBars) = ChBarIndex
                nBars += 1

            End If


            'If DisposeChunks Or Ch.Removed Then
            '    Ch.Dispose()
            'End If

        Next

        BarCount = nBars
        Return Bars


    End Function


    Private Shared nCachSaveChunks As Integer = 0
    Private Shared CachSaveChunksContents(500)() As Byte
    Private Shared CachSaveChunksPaths(500) As String
    Public Shared MaxnCachSaveChunks As Integer = 3000


    Public Shared Sub SaveChunkBarCachedAdd(Chunks() As Chunk, DisposeChunks As Boolean)


        CachSaveChunksPaths(nCachSaveChunks) = ChunkBarPathOf(Chunks(0))
        CachSaveChunksContents(nCachSaveChunks) = GenarateChunkBarByteCode(Chunks)
        nCachSaveChunks += 1

        If CachSaveChunksPaths.Length < nCachSaveChunks + 5 Then
            Array.Resize(CachSaveChunksContents, CachSaveChunksPaths.Length + 100)
            Array.Resize(CachSaveChunksPaths, CachSaveChunksPaths.Length + 100)
        End If




        If DisposeChunks Then
            For x = 0 To Ground.ChunkBarHeight - 1
                Chunks(x).Dispose()
            Next

        Else

            For x = 0 To Ground.ChunkBarHeight - 1

                If Chunks(x).Removed Then
                    Chunks(x).Dispose()
                End If

            Next
        End If



    End Sub

    Public Shared Sub CheckAndSaveCachedChunkBars()
        If nCachSaveChunks >= MaxnCachSaveChunks Then
            SaveCashedChunkBars()
        End If
    End Sub



    Private Shared ReadOnly FilesPerThreadSession As Integer = 30
    Private Shared CurrThrCachedChunk As Integer = 0
    Private Shared ReadOnly nThrCachedChunkSavers As Integer = 16
    Private Shared ReadOnly ThrCachedChunkSavers(nThrCachedChunkSavers) As Threading.Thread
    Private Shared ReadOnly ThrCachedChunkSaversIsCompleted(nThrCachedChunkSavers) As Boolean
    Private Shared SaveCashedChunksCompleted As Boolean = True

    Public Shared Sub SaveCashedChunkBars()

        SaveCashedChunksCompleted = False
        CurrThrCachedChunk = 0

        Dim TmrRep As New Timers.Timer(1000)
        AddHandler TmrRep.Elapsed, AddressOf SaveCashedChunksReport
        TmrRep.Start()


        For nThr = 0 To nThrCachedChunkSavers
            ThrCachedChunkSaversIsCompleted(nThr) = True
            'ThrCachedChunkSavers(nThr) = New Threading.Thread(AddressOf ThrCachedChunkSave)
        Next


        Dim LoopMax = nCachSaveChunks - (2 * FilesPerThreadSession) - 1

        Do Until CurrThrCachedChunk >= LoopMax

            For nThr = 0 To nThrCachedChunkSavers

                If ThrCachedChunkSaversIsCompleted(nThr) Then

#Disable Warning BC42016 ' Implicit conversion from 'Object' to 'Integer()'.
                    ThrCachedChunkSavers(nThr) = New Threading.Thread(AddressOf ThrCachedChunkSave)
#Enable Warning BC42016 ' Implicit conversion from 'Object' to 'Integer()'.
                    ThrCachedChunkSavers(nThr).Start({CurrThrCachedChunk, nThr})

                    CurrThrCachedChunk += FilesPerThreadSession

                    Exit For
                End If

            Next


        Loop


        For n = CurrThrCachedChunk To nCachSaveChunks - 1
            My.Computer.FileSystem.WriteAllBytes(CachSaveChunksPaths(n), CachSaveChunksContents(n), False)
        Next

        For nThr = 0 To nThrCachedChunkSavers
            If Not ThrCachedChunkSaversIsCompleted(nThr) Then

                Do Until ThrCachedChunkSaversIsCompleted(nThr)
                    Log(". ", False)
                    Threading.Thread.Sleep(1000)
                Loop

            End If
        Next



        TmrRep.Stop()
        TmrRep.Dispose()



        '    CachSaveChunksPaths = Nothing
        '  CachSaveChunksContents = Nothing
        nCachSaveChunks = 0

        'GC.Collect()

        SaveCashedChunksCompleted = True
    End Sub


    Private Shared Sub ThrCachedChunkSave(n() As Integer)
        ThrCachedChunkSaversIsCompleted(n(1)) = False

        For m = n(0) To n(0) + FilesPerThreadSession - 1
            My.Computer.FileSystem.WriteAllBytes(CachSaveChunksPaths(m), CachSaveChunksContents(m), False)
        Next


        ThrCachedChunkSaversIsCompleted(n(1)) = True
    End Sub


    Public Shared Sub SaveCashedChunksReport()
        Log(CurrThrCachedChunk.ToString & " of " & nCachSaveChunks.ToString)
    End Sub







    Private Shared BytesLoadFile() As Byte
    Public Shared Function LoadFile(Path As String) As Byte()
        BytesLoadFile = My.Computer.FileSystem.ReadAllBytes(Path)
        Return BytesLoadFile
    End Function

#End Region



#Region "Genarate ByteCodes"
    Public Shared AirChunkByteCode As Byte()

    Public Shared Function GenarateChunkByteCode(Ch As Chunk) As Byte()

        If Ch.IsAir Then
            Return AirChunkByteCode
            Exit Function
        End If

        Dim O(ChunkByteCodeLength) As Byte


        Dim n = 0
        For X = 0 To 7
            For Y = 0 To 7
                For Z = 0 To 7


                    O(n) = (Ch.BlockList(X)(Y)(Z).Varient)
                    O(n + 1) = (Ch.BlockList(X)(Y)(Z).BID)

                    If Ch.BlockList(X)(Y)(Z).IsAir Then
                        If Ch.BlockList(X)(Y)(Z).SurfaceRelation Then
                            O(n + 2) = 3

                        Else
                            O(n + 2) = 2

                        End If

                    Else
                        If Ch.BlockList(X)(Y)(Z).SurfaceRelation Then
                            O(n + 2) = 1

                        Else
                            O(n + 2) = 0

                        End If

                    End If



                    n += 3
                Next
            Next
        Next


        O(ChunkByteCodeLength - 1) = CByte(Ch.IsInTheSurface)


        Return O
    End Function


    Private Shared Function CompileChunkByteCode(ByRef O As Byte(), ChunkIndex As IntVector3) As Chunk
        Dim Ch As New Chunk

        'O((512 * 7) + 1)

        Dim X, Y, Z As Byte
        Dim B As Block


        Ch.BlockList = New Block(7)()() {}
        For X = 0 To 7
            Ch.BlockList(X) = New Block(7)() {}
            For Y = 0 To 7
                Ch.BlockList(X)(Y) = New Block(7) {}
            Next
        Next



        Dim IsNotAir As Boolean = True

        Dim n = 0
        For X = 0 To 7
            For Y = 0 To 7
                For Z = 0 To 7

                    B = New Block() With {.BID = O(n + 1), .Varient = O(n),
                    .CPosition = New IntVector3(X, Y, Z),
                    .IsAir = CBool(O(n + 2) \ 2),
                    .SurfaceRelation = CBool(O(n + 2) Mod 2)}


                    n += 3

                    IsNotAir = IsNotAir OrElse Not B.IsAir


                    Ch.BlockList(X)(Y)(Z) = B

                    If B.SurfaceRelation Then
                        Dim BBlock(4) As Byte

                        BBlock(0) = X
                        BBlock(1) = Y
                        BBlock(2) = Z

                        'BBlock(3) = CByte(B.BID)

                        BBlock(3) = B.BID
                        BBlock(4) = B.Varient

                        Ch.BIDList(Ch.BIDFilledI) = BBlock
                        Ch.BIDFilledI += 1S

                        If Ch.BIDList.Length < Ch.BIDFilledI + 5 Then
                            Array.Resize(Ch.BIDList, Ch.BIDList.Length + 40)
                        End If


                        Ch.SurfaceBlocks(Ch.FilledSB) = B
                        Ch.FilledSB += 1

                    End If





                Next
            Next
        Next




        Ch.IsInTheSurface = CBool(O(ChunkByteCodeLength - 1))

        'If Ch.IsInTheSurface Then
        '    'Ground.SurfaceChunks(Ground.FilledSFC) = Ch
        '    'Ground.FilledSFC += 1
        '    Dim xdd = 0
        'End If


        Ch.Index = ChunkIndex
        Ch.Position = ChunkIndex * Chunk.Volume
        Ch.IsAir = Not IsNotAir

        Ch.GenarateAirGrid()
        Ch.GenarateBlockTranslations()
        Ch.GenerateBIDForBTranslations()

        Return Ch
    End Function



    Public Shared Function CompileChunkBarByteCode(ByRef O As Byte(), ChunkBarIndex As IntVector3) As Chunk()

        Dim Chs(Ground.ChunkBarHeight - 1) As Chunk
        Dim ChBC(ChunkByteCodeLength) As Byte

        Dim ChunkIndex = ChunkBarIndex

        For y = 0 To Ground.ChunkBarHeight - 1

            Array.Copy(O, y * ChunkByteCodeLength, ChBC, 0, ChunkByteCodeLength)

            ChunkIndex.Y = y
            Chs(y) = CompileChunkByteCode(ChBC, ChunkIndex)

        Next

        Return Chs
    End Function

#End Region


#Region "Load"

    Public Shared Sub LoadAndReplaceChunksRegion(Min As IntVector3, Max As IntVector3)

        Array.Clear(ChunkIndices, 0, ChunkIndices.Length)

        Dim nChunkIndices = (Max.X - Min.X) * (Max.Z - Min.Z)

        If nChunkIndices > ChunkIndices.Length - 10 Then
            ReDim ChunkIndices(nChunkIndices + 200)
        End If

        Dim n = 0
        For X = Min.X To Max.X - 1
            For Z = Min.Z To Max.Z - 1

                ChunkIndices(n) = New IntVector3(X, 0, Z)
                n += 1

            Next
        Next


        Dim S As IntVector3

        For m = 0 To n - 1
            S = ChunkIndices(m)

            Dim Chs = CompileChunkBarByteCode(LoadFile(SavedPathOf(S, ExtChunk)), S)

            Dim Y = 0
            Dim SIndex = S
            For Each Ch In Chs

                SIndex.Y = Y
                Ground.CStack.SetChunk(SIndex, Ch)

                If Ch.IsInTheSurface Then
                    Ch.SurfaceChunkIndex = Ground.FilledSFC
                    Ground.SurfaceChunks(Ground.FilledSFC) = Ch
                    Ground.FilledSFC += 1
                End If
                Y += 1
            Next





        Next


    End Sub



    Private Shared ChunkIndices(3000) As IntVector3
    Public Shared Sub LoadAndReplaceChunksRegions(Min1 As IntVector3, Max1 As IntVector3, Min2 As IntVector3, Max2 As IntVector3)


        Array.Clear(ChunkIndices, 0, ChunkIndices.Length)


        Dim nChunkIndices = (((Max1.X - Min1.X) * (Max1.Z - Min1.Z)) + ((Max2.X - Min2.X) * (Max2.Z - Min2.Z)))

        If nChunkIndices > ChunkIndices.Length - 10 Then
            ReDim ChunkIndices(nChunkIndices + 200)
        End If

        Dim n = 0
        For X = Min1.X To Max1.X - 1
            For Z = Min1.Z To Max1.Z - 1


                ChunkIndices(n) = New IntVector3(X, 0, Z)

                n += 1


            Next
        Next

        For X = Min2.X To Max2.X - 1
            For Z = Min2.Z To Max2.Z - 1


                ChunkIndices(n) = New IntVector3(X, 0, Z)

                n += 1


            Next
        Next




        Dim S As IntVector3


        For m = 0 To n - 1
            S = ChunkIndices(m)


            Dim Chs = CompileChunkBarByteCode(LoadFile(SavedPathOf(S, ExtChunk)), S)

            Dim Y = 0
            Dim SIndex = S
            For Each Ch In Chs

                SIndex.Y = Y
                Ground.CStack.SetChunk(SIndex, Ch)

                If Ch.IsInTheSurface Then
                    Ch.SurfaceChunkIndex = Ground.FilledSFC
                    Ground.SurfaceChunks(Ground.FilledSFC) = Ch
                    Ground.FilledSFC += 1
                End If

                Y += 1
            Next



        Next





        'Log("Chunk region loaded")

    End Sub



    Public Shared Sub LoadAndReplaceChunk(Index As IntVector3)

        Dim Chs = CompileChunkBarByteCode(LoadFile(SavedPathOf(Index, ExtChunk)), Index)

        Dim Y = 0
        Dim SIndex = Index
        For Each Ch In Chs

            SIndex.Y = Y
            Ground.CStack.SetChunk(SIndex, Ch)

            If Ch.IsInTheSurface Then
                Ch.SurfaceChunkIndex = Ground.FilledSFC
                Ground.SurfaceChunks(Ground.FilledSFC) = Ch
                Ground.FilledSFC += 1
            End If

            Y += 1
        Next



        'Log("Chunk loaded :: " + Ch.Index.ToString)


    End Sub

    Public Shared Sub LoadAndReplaceChunkTempory(Index As IntVector3)

        Dim Chs = CompileChunkBarByteCode(LoadFile(SavedPathOf(Index, ExtChunk)), Index)


        Dim Y = 0
        Dim SIndex = Index
        For Each Ch In Chs

            SIndex.Y = Y
            Ground.CStack.SetChunk(SIndex, Ch)

            If Ch.IsInTheSurface AndAlso Ground.TmpFilledSFC < Ground.TmpSurfaceChunks.Length Then
                If Not Contains(Ground.TmpSurfaceChunks, Ground.TmpFilledSFC, Ch) Then
                    Ch.SurfaceChunkIndex = Ground.TmpFilledSFC
                    Ground.TmpSurfaceChunks(Ground.TmpFilledSFC) = Ch
                    Ground.TmpFilledSFC += 1
                End If
            End If

            Y += 1

        Next


    End Sub



    Public Shared Function SavedNameToIndex(S As String) As IntVector3
        Dim A = S.Split(","c)
        Return New IntVector3(CInt(A(0)), 0, CInt(A(1)))
    End Function


    ''' <summary>
    ''' Can be used to any saved file type
    ''' </summary>
    Public Shared Function SavedNamesToIndices(Names As String()) As IntVector3()
        Dim O(Names.Length - 1) As IntVector3
        Dim n = 0
        For Each S In Names
            Dim A = S.Split(","c)
            O(n) = New IntVector3(CInt(A(0)), 0, CInt(A(1)))
            n += 1
        Next
        Return O
    End Function





    Public Shared Function ChunkBarPathOf(Ch As Chunk) As String
        Return MapRoot & Ch.Index.X.ToString & "," & Ch.Index.Z.ToString & ".C"
    End Function

    Public Shared Function ChunkBarPathOf(Index As IntVector3) As String
        Return MapRoot & Index.X.ToString & "," & Index.Z.ToString & ".C"
    End Function

    Public Shared Function ChunkBarNameOf(Ch As Chunk) As String
        Return Ch.Index.X.ToString & "," & Ch.Index.Z.ToString
    End Function

    Public Shared Function ChunkBarNameOf(Index As IntVector3) As String
        Return Index.X.ToString + "," + Index.Z.ToString
    End Function

    ''' <summary>
    ''' Can be used to any saved file type
    ''' </summary>
    Public Shared Function SavedPathOf(Pos As IntVector3, Ext As String) As String
        Return MapRoot + Pos.X.ToString + "," + Pos.Z.ToString + Ext
    End Function


    ''' <summary>
    ''' Can be used to any saved file type
    ''' </summary>
    Public Shared Function SavedNameOf(Index As IntVector3) As String
        Return Index.X.ToString + "," + Index.Z.ToString
    End Function

    <Serializable>
    Public Class MapInfo
        Public Name As String

        Public HMapSize As Integer

        Public WorldHeight As Integer

        Public PlayerPosition As Microsoft.Xna.Framework.Vector3

        Public EntityLICode As Integer

        Public Sub Save(Path As String)

            EntityLICode = Entity.LICode

            XS = New Xml.Serialization.XmlSerializer(GetType(MapInfo))
            SW = New IO.StreamWriter(Path)

            XS.Serialize(SW, Me)

            SW.Close()

            My.Computer.FileSystem.CopyFile(Path, Path & "Backup", True)

        End Sub


        Public Shared Function Load(Path As String) As MapInfo


            XS = New Xml.Serialization.XmlSerializer(GetType(MapInfo))
            SR = New IO.StreamReader(Path)


            Dim Out As MapInfo
            Try
                Out = CType(XS.Deserialize(SR), MapInfo)
            Catch ex As Exception
                Out = Nothing
            End Try

            SR.Close()


            Try
                If Out Is Nothing Then

                    'Restore backup
                    If My.Computer.FileSystem.FileExists(Path & "Backup") Then
                        My.Computer.FileSystem.CopyFile(Path & "Backup", Path, True)
                        Out = Load(Path)

                    End If

                End If
            Catch ex As Exception
                Out = Nothing
            End Try



#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
            Entity.LICode = Out.EntityLICode
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.

            Return Out



        End Function

    End Class


#End Region


#Region "Height Maps"
    Public Shared Function GenareteHMap(Bm As Biome, Index As IntVector3) As HeightMap

        Dim HM As New HeightMap

        RND = New Random(100)


        Dim XLength = HeightMap.Size
        Dim YLength = HeightMap.Size
        Dim FlagDistance = Bm.FlagDistance
        Dim RowPattern = Bm.RowPattern
        Dim FlagLerpAmont = Bm.FlagLerpAmont
        Dim FlagPowerMin = Bm.FlagPowerMin
        Dim FlagPowerMax = Bm.FlagPowerMax
        Dim MaxHeight = Bm.MaxHeight


        Dim A()() As Byte
        A = New Byte(XLength - 1)() {}
        For x = 0 To YLength - 1
            A(x) = New Byte(YLength - 1) {}
        Next




#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim nFlagsX As Integer = Math.Truncate(XLength / FlagDistance)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim nFlagsY As Integer = Math.Truncate(YLength / FlagDistance)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


        Dim Flags(nFlagsX)() As Byte
        For x = 0 To nFlagsX
            Flags(x) = New Byte(nFlagsY) {}
        Next


        'Creating Flags
        Log("Creating flags")
        For X = 0 To nFlagsX
            For Y = 0 To nFlagsY

#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Byte'.
                Flags(X)(Y) = Math.Round(Math.Min(Math.Max(RowPattern(RND.Next(0, RowPattern.Length)), 0), MaxHeight))
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Byte'.

            Next

        Next


        Dim nFLAF = (FlagLerpAmont * 2) * (FlagLerpAmont * 2)


        'Lerping Flags
        Log("Lerping flags")
        If FlagLerpAmont > 0 Then

            For X = FlagLerpAmont To nFlagsX - 1 - FlagLerpAmont
                For Y = FlagLerpAmont To nFlagsY - 1 - FlagLerpAmont

                    Dim Total = 0

                    For FLAX = -FlagLerpAmont To FlagLerpAmont
                        For FLAY = -FlagLerpAmont To FlagLerpAmont
                            Total += Flags(X + FLAX)(Y + FLAY)
                        Next

                    Next

                    Dim Average = Total / nFLAF

                    Dim F = Flags(X)(Y)

                    Dim RndFlagPower = RND.Next(FlagPowerMin, FlagPowerMax + 1)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.

                Next
            Next



            Dim BnFLAF = FlagLerpAmont * FlagLerpAmont



            'Border Y L
            For X = 0 To FlagLerpAmont
                For Y = 0 To nFlagsY - 1 - FlagLerpAmont

                    Dim Total = 0

                    For FLAX = 0 To FlagLerpAmont
                        For FLAY = 0 To FlagLerpAmont
                            Total += Flags(X + FLAX)(Y + FLAY)
                        Next

                    Next

                    Dim Average = Total / BnFLAF

                    Dim F = Flags(X)(Y)

                    Dim RndFlagPower = RND.Next(FlagPowerMin, FlagPowerMax + 1)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.

                Next
            Next



            'Border Y R
            For X = nFlagsX - 1 - FlagLerpAmont To nFlagsX - 1
                For Y = 0 To nFlagsY - 1 - FlagLerpAmont

                    Dim Total = 0

                    For FLAX = FlagLerpAmont To 0 Step -1
                        For FLAY = FlagLerpAmont To 0 Step -1

                            Total += Flags(Math.Min(X + FLAX, nFlagsX - 1))(Math.Min(Y + FLAY, nFlagsY - 1))
                        Next

                    Next

                    Dim Average = Total / BnFLAF

                    Dim F = Flags(X)(Y)

                    Dim RndFlagPower = RND.Next(FlagPowerMin, FlagPowerMax + 1)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.

                Next
            Next




            'Border X U
            For X = 0 To nFlagsX - 1 - FlagLerpAmont
                For Y = 0 To FlagLerpAmont

                    Dim Total = 0

                    For FLAX = 0 To FlagLerpAmont
                        For FLAY = 0 To FlagLerpAmont
                            Total += Flags(X + FLAX)(Y + FLAY)
                        Next

                    Next

                    Dim Average = Total / BnFLAF

                    Dim F = Flags(X)(Y)

                    Dim RndFlagPower = RND.Next(FlagPowerMin, FlagPowerMax + 1)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.

                Next
            Next



            'Border X D
            For X = 0 To nFlagsX - 1 - FlagLerpAmont
                For Y = nFlagsY - 1 - FlagLerpAmont To nFlagsY - 1

                    Dim Total = 0

                    For FLAX = FlagLerpAmont To 0 Step -1
                        For FLAY = FlagLerpAmont To 0 Step -1

                            Total += Flags(Math.Min(X + FLAX, nFlagsX - 1))(Math.Min(Y + FLAY, nFlagsY - 1))
                        Next

                    Next

                    Dim Average = Total / BnFLAF

                    Dim F = Flags(X)(Y)

                    Dim RndFlagPower = RND.Next(FlagPowerMin, FlagPowerMax + 1)
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.

                Next
            Next




        End If






        'Creating Blocks by diffing flags
        Log("Creating Block heights")

        For X = 1 To nFlagsX
            For Y = 1 To nFlagsX

                Dim ThisFlag = Flags(X)(Y)

                Dim PXFlag = Flags(X - 1)(Y)
                Dim PYFlag = Flags(X)(Y - 1)

                Dim PXPYFlag = Flags(X - 1)(Y - 1)





                Dim DifPX = ListDif(PXFlag, ThisFlag, FlagDistance) '-------
                Dim DifPY = ListDif(PYFlag, ThisFlag, FlagDistance) '||||||


                Dim DifParrelelPX = ListDif(PXPYFlag, PYFlag, FlagDistance) '------
                Dim DifParrelelPY = ListDif(PXPYFlag, PXFlag, FlagDistance) '||||||





                Dim PosX = X * FlagDistance
                Dim PosY = Y * FlagDistance

                Dim PosPX = (X - 1) * FlagDistance
                Dim PosPY = (Y - 1) * FlagDistance


                Dim DDifX(FlagDistance - 1)() As Integer
                For m = 0 To FlagDistance - 1
                    DDifX(m) = ListDif(DifParrelelPY(m), DifPY(m), FlagDistance)
                Next



                Dim n = 0
                For BX = PosPX To PosX - 1
                    Dim m = 0

                    Dim DDifY = ListDif(DifParrelelPX(n), DifPX(n), FlagDistance)

                    For BY = PosPY To PosY - 1

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                        A(BX)(BY) = (DDifY(m) + DDifX(m)(n)) / 2
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Byte'.
                        'A(BX)(BY) = DDifY(m)

                        m += 1
                    Next
                    n += 1
                Next



            Next
        Next



        Log("Generating Height maps succeed")



        HM.B = A
        HM.Bm = Bm
        HM.Index = Index



        Return HM
    End Function






    Public Shared Sub SaveHeightMap(HM As HeightMap)

        Dim Exits = False

        For Each I In SavedHMaps
            If I = HM.Index Then
                Exits = True
            End If
        Next

        If Not Exits Then
            SavedHMaps.Add(HM.Index)
            My.Computer.FileSystem.WriteAllText(SavedHMapFile, SavedNameOf(HM.Index) + ";"c, True, Text.Encoding.ASCII)
        End If



        My.Computer.FileSystem.WriteAllBytes(SavedPathOf(HM.Index, ExtHeightMap), GenarateHMByteCode(HM), False)

        Log("Height map saved")


    End Sub



    Public Shared Function GenarateHMByteCode(HM As HeightMap) As Byte()

#Disable Warning BC42016 ' Implicit conversion from 'Long' to 'Integer'.
        Dim B(HeightMap.ByteCodeLength) As Byte
#Enable Warning BC42016 ' Implicit conversion from 'Long' to 'Integer'.


        For X = 0 To HeightMap.Size - 1
            For Y = 0 To HeightMap.Size - 1
                B((X * HeightMap.Size) + Y) = HM.B(X)(Y)
            Next
        Next

        Dim SizePow = HeightMap.Size * HeightMap.Size

        B(SizePow + 1) = CByte(HM.Index.X)
        B(SizePow + 2) = CByte(HM.Index.Y)
        B(SizePow + 3) = CByte(HM.Index.Z)
        B(SizePow + 4) = CByte(HM.Bm.Index)



        Return B
    End Function


    Public Shared Function LoadHeightMap(Index As IntVector3) As HeightMap

        Dim Exits = False

        For Each I In SavedHMaps
            If I = Index Then
                Exits = True
            End If
        Next

        If Exits Then
            Dim HM = CompileHMByteCode(LoadFile(SavedPathOf(Index, ExtHeightMap)))
            LoadedHMaps.Add(HM)
            Log("Height map loaded")
            Return HM
        Else
            Return Nothing
        End If

    End Function


    Public Shared Function CompileHMByteCode(B As Byte()) As HeightMap
        Dim HM As New HeightMap With {
            .B = New Byte(HeightMap.Size - 1)() {}
        }
        For X = 0 To HeightMap.Size - 1
            HM.B(X) = New Byte(HeightMap.Size - 1) {}
            For Y = 0 To HeightMap.Size - 1
                HM.B(X)(Y) = B((X * HeightMap.Size) + Y)
            Next
        Next

        Dim SizePow = HeightMap.Size * HeightMap.Size

        HM.Index = New IntVector3 With {
            .X = B(SizePow + 1),
            .Y = B(SizePow + 2),
            .Z = B(SizePow + 3)
        }

        HM.Bm = BiomeList.Lst(B(SizePow + 4))


        LoadedHMaps.Add(HM)

        Return HM
    End Function

#End Region
End Class



