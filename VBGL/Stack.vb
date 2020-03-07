Imports Microsoft.Xna.Framework

Public Class Chunk
    Implements IDisposable

    Public Const IntSize As Integer = 8
    Public Shared Size As Vector3 = New Vector3(8, 8, 8)
    Public Shared SizeInt As IntVector3 = New IntVector3(8, 8, 8)
    Public Shared Volume As Vector3 = Size * Ground.BlockSize
    Public Const VolumeI As Integer = 400


    ''' <summary>
    ''' X - Y - Z
    ''' </summary>
    Public BlockList()()() As Block

    ''' <summary>
    ''' (#) = Index, (#)(0)= CPosX ,  (#)(1)= CPosY , (#)(2)= CPosZ , (#)(3)= BID
    ''' </summary>
    Public BIDList()() As Byte
    Public BIDFilledI As Short = 0

    Public BIDForBTranslations() As Byte
    Public BlockTranslations() As Vector4

    'Public BlockList As New List(Of List(Of List(Of Block)))

    Public IsAir As Boolean = True

    Public Position As Vector3
    Public Index As IntVector3 = New IntVector3(-1, -1, -1)

    Public IsInTheSurface As Boolean = False
    Public SurfaceChunkIndex As Integer = -1

    Public SurfaceBlocks() As Block
    Public FilledSB As Integer = 0


    Public Changed As Boolean = False
    Public Removed As Boolean = False

    ''' <summary>
    ''' # = BT ID , (#) = count
    ''' </summary>
    Public CountForBlockTypes(BlockType.BTCount) As Integer

    ''' <summary>
    ''' X - Y - Z
    ''' </summary>
    Public AirGrid()()() As Boolean


    'Public FBlocks()()() As FBlock = New FBlock(7)()() {}


    Public Sub GenarateAirGrid()

        Dim XSet()()() As Boolean = New Boolean(7)()() {}
        For nX = 0 To 7
            Dim YSet()() As Boolean = New Boolean(7)() {}
            For nY = 0 To 7
                Dim ZSet() As Boolean = New Boolean(7) {}
                For nZ = 0 To 7
                    ZSet(nZ) = BlockList(nX)(nY)(nZ).IsAir
                Next
                YSet(nY) = ZSet
            Next
            XSet(nX) = YSet
        Next

        AirGrid = XSet


    End Sub



    Public Sub GenarateBlockTranslations()

        BlockTranslations = New Vector4(BIDFilledI - 1) {}

        For CI = 0 To BIDFilledI - 1

            Dim B = BIDList(CI)

            BlockTranslations(CI) = New Vector4(B(0) * 50 + Position.X,
                                                B(1) * 50 + Position.Y,
                                                B(2) * 50 + Position.Z, 1)

        Next


        For n = 0 To FilledSB - 1
            CountForBlockTypes(SurfaceBlocks(n).BID) += 1
        Next



    End Sub



    Public Sub GenerateBIDForBTranslations()

        If BIDList.Length <> BIDForBTranslations.Length Then
            ReDim BIDForBTranslations(BIDList.Length)
        End If

        For n = 0 To BIDFilledI - 1
            BIDForBTranslations(n) = BIDList(n)(3)
        Next


    End Sub




    Public Sub New()
        SurfaceBlocks = New Block(512) {}


        Dim XSet()()() As Boolean = New Boolean(7)()() {}
        For nX = 0 To 7
            Dim YSet()() As Boolean = New Boolean(7)() {}
            For nY = 0 To 7
                Dim ZSet() As Boolean = New Boolean(7) {}
                For nZ = 0 To 7
                    ZSet(nZ) = True
                Next
                YSet(nY) = ZSet
            Next
            XSet(nX) = YSet
        Next


        AirGrid = XSet

        BIDList = New Byte(256)() {}
        BIDForBTranslations = New Byte(256) {}

    End Sub







    Public Shared Operator =(Left As Chunk, Right As Chunk) As Boolean
        If Left.Index = Right.Index Then
            Return True
        Else
            Return False
        End If
    End Operator

    Public Shared Operator <>(Left As Chunk, Right As Chunk) As Boolean
        If Left.Index <> Right.Index Then
            Return True
        Else
            Return False
        End If
    End Operator


    Public Overrides Function Equals(obj As Object) As Boolean
        Dim R = DirectCast(obj, Chunk)
        If Index.X = R.Index.X AndAlso Index.Y = R.Index.Y AndAlso Index.Z = R.Index.Z Then
            Return True
        Else
            Return False
        End If
    End Function


#Region "IDisposable Support"
    Public disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        SyncLock Me

            If Not Me.disposedValue Then
                'If disposing Then
                '    ' TODO: dispose managed state (managed objects).

                'End If

                Me.disposedValue = True
                BIDFilledI = 0

                'If IsInTheSurface AndAlso SurfaceChunkIndex > -1 Then

                '    Ground.SurfaceChunks(SurfaceChunkIndex) = Ground.SurfaceChunks(Ground.FilledSFC - 1)
                '    Ground.SurfaceChunks(SurfaceChunkIndex).SurfaceChunkIndex = SurfaceChunkIndex
                '    Ground.SurfaceChunks(Ground.FilledSFC - 1) = Nothing
                '    Ground.FilledSFC -= 1
                '    SurfaceChunkIndex = -1
                'End If

                With Me
                    .SurfaceBlocks = Nothing
                    .BlockList = Nothing
                    .BIDList = Nothing
                    .BIDForBTranslations = Nothing
                    .AirGrid = Nothing
                    .BlockTranslations = Nothing
                    .CountForBlockTypes = Nothing
                    .Position = Nothing
                    .BIDFilledI = Nothing
                    .IsAir = Nothing
                    .Position = Nothing
                    .Index = Nothing
                    .IsInTheSurface = Nothing
                    .SurfaceChunkIndex = Nothing
                    .FilledSB = Nothing
                    .Changed = Nothing
                    .Removed = Nothing

                End With




            End If

        End SyncLock
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Do not call unless Ground.SurfaceChunks doesn't contain this Chunk. Remove it first
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region







End Class



Public Class Stack

    ''' <summary> 
    ''' <Must>X = Z</Must>
    ''' </summary>
    Public Shared Size As IntVector3 = New IntVector3(30, 15, 30)
    Public Shared Volume As Vector3 = Size * Chunk.Size * Ground.BlockSize


    ''' <summary>
    ''' X - Y - Z
    ''' </summary>
    Public ChunkList(19)()() As Chunk

    'Where the array begins
    Public RootX As Integer = 0
    Public RootZ As Integer = 0

    Public MinX As Integer = 0
    Public MinZ As Integer = 0

    Public ChunkRangeMin As IntVector3
    Public ChunkRangeMax As IntVector3
    Public ChunkRangeMiddle As IntVector3
    Public ChunkRangeAddingVal As IntVector3


    Public Position As Vector3
    Public Index As Integer

    Public SavingChunks(100) As Chunk
    Public nSavingChunks As Integer = 0


    'Public eHList As New List(Of Human)

    Public eList As New List(Of Entity)



    Public Function ChunkIndexToArrayIndex(Index As IntVector3) As IntVector3
        Dim O = Index - ChunkRangeMin

        O.X += RootX
        O.Z += RootZ

        If O.X >= Size.X Then
            O.X -= Size.X
        End If

        If O.Z >= Size.Z Then
            O.Z -= Size.Z
        End If

        Return O
    End Function


    Public Function ChunkIndexToArrayIndex(X As Integer, Y As Integer, Z As Integer) As Integer()
        'Dim O = Index - ChunkRangeMin

        Dim OX = X - MinX
        Dim OZ = Z - MinZ


        OX += RootX
        OZ += RootZ

        If OX >= Size.X Then
            OX -= Size.X
        End If

        If OZ >= Size.Z Then
            OZ -= Size.Z
        End If

        Return {OX, Y, OZ}
    End Function



    Public Function GetChunk(ByVal Index As IntVector3) As Chunk
        ''on error GoTo Err

        Index = ChunkIndexToArrayIndex(Index)

        Dim ch = ChunkList(Index.X)(Index.Y)(Index.Z)

        If ch.disposedValue Then
            Return Nothing
        End If
        While ch.BlockList Is Nothing
            Threading.Thread.Sleep(50)
        End While


        Return ch

        Exit Function

Err:
        'If Index.Y >= Size.Y Then
        '    Return ChunkList(Index.X)(Size.Y - 1)(Index.Z)
        'ElseIf Index.Y < 0 Then
        '    Return ChunkList(Index.X)(0)(Index.Z)
        'Else
        Return Nothing
        'End If

    End Function


    Public Function GetChunk(ByVal X As Integer, ByVal Y As Integer, ByVal Z As Integer) As Chunk
        'on error GoTo Err

        Dim I = ChunkIndexToArrayIndex(X, Y, Z)
        Dim ch = ChunkList(I(0))(I(1))(I(2))

        If ch.disposedValue Then
            Return Nothing
        End If
        While ch.BlockList Is Nothing
            Threading.Thread.Sleep(50)
        End While


        Return ch

        Exit Function
Err:    Return Nothing
    End Function

    Public Function GetChunkBar(ByVal Index As IntVector3) As Chunk()
        'on error GoTo Err

        Index = ChunkIndexToArrayIndex(Index)

        Dim ChunkBar(Ground.ChunkBarHeight - 1) As Chunk

        Dim ch As Chunk
        For y = 0 To Ground.ChunkBarHeight - 1
            ch = ChunkList(Index.X)(y)(Index.Z)

            If ch.disposedValue Then
                Return Nothing
            End If
            While ch.BlockList Is Nothing
                Threading.Thread.Sleep(50)
            End While
            ChunkBar(y) = ch
        Next

        Return ChunkBar


        Exit Function

Err:
        'If Index.Y >= Size.Y Then
        '    Return ChunkList(Index.X)(Size.Y - 1)(Index.Z)
        'ElseIf Index.Y < 0 Then
        '    Return ChunkList(Index.X)(0)(Index.Z)
        'Else
        Return Nothing
        'End If

    End Function





    Public Sub SetChunk(ByVal Index As IntVector3, ByVal Ch As Chunk)
        Index = ChunkIndexToArrayIndex(Index)

        ChunkList(Index.X)(Index.Y)(Index.Z) = Ch

    End Sub




    Public Sub SetChunk(ByVal X As Integer, ByVal Y As Integer, ByVal Z As Integer, ByVal Ch As Chunk)
        Dim I = ChunkIndexToArrayIndex(X, Y, Z)

        'Dim OldCh = ChunkList(I(0))(I(1))(I(2))

        'If Not IsNothing(OldCh) Then
        '    If OldCh.Changed Then
        '        SavingChunks(nSavingChunks) = OldCh
        '        nSavingChunks += 1
        '    Else
        '        OldCh.Dispose()
        '    End If
        'End If




        ChunkList(I(0))(I(1))(I(2)) = Ch
    End Sub



    'Public Sub SetOrReplaceChunk(ByVal Index As IntVector3, Ch As Chunk)
    '    Index = ChunkIndexToArrayIndex(Index)

    '    Dim OldCh = ChunkList(Index.X)(Index.Y)(Index.Z)

    '    If Not IsNothing(OldCh) Then
    '        If OldCh.Changed Then
    '            SavingChunks(nSavingChunks) = OldCh
    '            OldCh.Changed = False
    '            nSavingChunks += 1
    '        Else
    '            OldCh.Dispose()
    '        End If
    '    End If



    '    ChunkList(Index.X)(Index.Y)(Index.Z) = Ch

    'End Sub


    Public Sub CreateChunkRangeBounds(Middle As IntVector3)

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        ChunkRangeAddingVal = IntVector3.FromV3Truncated(New Vector3(Size.X / 2, 0, Size.Z / 2))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
        ChunkRangeMiddle = Middle



        ChunkRangeMax = Middle + ChunkRangeAddingVal + New IntVector3(0, Size.Y, 0)
        ChunkRangeMin = Middle - ChunkRangeAddingVal

        ChunkRangeMin.Y = 0

        MinX = ChunkRangeMin.X
        MinZ = ChunkRangeMin.Z

    End Sub


    Public Sub NewChunkList()
        ChunkList = New Chunk(Size.X - 1)()() {}

        For X = 0 To Size.X - 1
            ChunkList(X) = New Chunk(Size.Y - 1)() {}
            For Y = 0 To Size.Y - 1
                ChunkList(X)(Y) = New Chunk(Size.Z - 1) {}

            Next
        Next


    End Sub






    Public Sub LoadChunks(MiddleIndex As IntVector3)

        Log("Loading Stack...")

        For n = 0 To Ground.FilledSFC - 1
            Ground.SurfaceChunks(n).SurfaceChunkIndex = -1
            Ground.SurfaceChunks(n) = Nothing
        Next
        Ground.FilledSFC = 0

        CreateChunkRangeBounds(MiddleIndex)

        Dim OutOfRange = False
        Dim NewRangeMiddle = ChunkRangeMiddle
        If ChunkRangeMin.X < 0 Then
            OutOfRange = True
            NewRangeMiddle.X = ChunkRangeAddingVal.X


        ElseIf ChunkRangeMax.X >= Loader.MaxWorldBorders.X Then
            OutOfRange = True
            NewRangeMiddle.X = Loader.MaxWorldBorders.X - ChunkRangeAddingVal.X - 1

        End If

        If ChunkRangeMin.Z < 0 Then
            OutOfRange = True
            NewRangeMiddle.Z = ChunkRangeAddingVal.Z

        ElseIf ChunkRangeMax.Z >= Loader.MaxWorldBorders.Z Then
            OutOfRange = True
            NewRangeMiddle.Z = Loader.MaxWorldBorders.Z - ChunkRangeAddingVal.Z - 1

        End If

        If OutOfRange Then
            CreateChunkRangeBounds(NewRangeMiddle)
        End If


        Loader.LoadAndReplaceChunksRegion(ChunkRangeMin, ChunkRangeMax)

        Log("Stack loaded succeed")

    End Sub



    Public Scrolling As Boolean = False
    Public Sub Scroll(MiddleIndex As IntVector3)



        If Scrolling OrElse MiddleIndex = ChunkRangeMiddle Then
            Exit Sub
        End If


        Scrolling = True



        Dim OldChunkRangeMin = ChunkRangeMin
        Dim OldChunkRangeMax = ChunkRangeMax

        CreateChunkRangeBounds(MiddleIndex)

        Dim OutOfRange = False
        Dim NewRangeMiddle = ChunkRangeMiddle

        If ChunkRangeMin.X < 0 Then
            OutOfRange = True
            NewRangeMiddle.X = ChunkRangeAddingVal.X


        ElseIf ChunkRangeMax.X >= Loader.MaxWorldBorders.X Then
            OutOfRange = True
            NewRangeMiddle.X = Loader.MaxWorldBorders.X - ChunkRangeAddingVal.X - 1

        End If

        If ChunkRangeMin.Z < 0 Then
            OutOfRange = True
            NewRangeMiddle.Z = ChunkRangeAddingVal.Z

        ElseIf ChunkRangeMax.Z >= Loader.MaxWorldBorders.Z Then
            OutOfRange = True
            NewRangeMiddle.Z = Loader.MaxWorldBorders.Z - ChunkRangeAddingVal.Z - 1

        End If

        If OutOfRange Then
            CreateChunkRangeBounds(NewRangeMiddle)
        End If

        If ChunkRangeMax.X = OldChunkRangeMax.X AndAlso ChunkRangeMax.Z = OldChunkRangeMax.Z Then

            Scrolling = False
            Exit Sub
        End If



        Dim Ch As Chunk
        Dim TmpFilledSFC = Ground.FilledSFC
        Ground.FilledSFC = 0
        Dim nFSC = 0
        For n = 0 To TmpFilledSFC - 1

            Ch = Ground.SurfaceChunks(n)
            Ground.SurfaceChunks(n) = Nothing

            If Ch.Index.X >= ChunkRangeMin.X AndAlso
                Ch.Index.Z >= ChunkRangeMin.Z AndAlso
                Ch.Index.X <= ChunkRangeMax.X AndAlso
                Ch.Index.Z <= ChunkRangeMax.Z Then

                Ch.SurfaceChunkIndex = nFSC
                Ground.SurfaceChunks(nFSC) = Ch
                nFSC += 1

            Else

                Ch.SurfaceChunkIndex = -1

                If Ch.Changed Then
                    SavingChunks(nSavingChunks) = Ch
                    Ch.Changed = False
                    nSavingChunks += 1
                    Ch.Removed = True
                Else
                    Ch.Dispose()
                End If

            End If

        Next
        Ground.FilledSFC = nFSC

        'Array.Clear(Ground.SurfaceChunks, 0, Ground.FilledSFC)

        'Array.Copy(TmpSurfaceChunks, Ground.SurfaceChunks, TmpFilledSFC)

        'Array.Clear(TmpSurfaceChunks, 0, TmpFilledSFC)





        If ChunkRangeMax.X > OldChunkRangeMax.X Then





            If ChunkRangeMax.Z > OldChunkRangeMax.Z Then
                'X -> ++ and Z -> ++

                'X -> ++ 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX >= Size.X Then
                    RootX -= Size.X
                End If
                'Z -> ++
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ >= Size.Z Then
                    RootZ -= Size.Z
                End If



                Dim MinPX = New IntVector3(OldChunkRangeMax.X, 0, ChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(ChunkRangeMax.X, 0, ChunkRangeMax.Z)


                Dim MinPZ = New IntVector3(ChunkRangeMin.X, 0, OldChunkRangeMax.Z)
                Dim MaxPZ = New IntVector3(OldChunkRangeMax.X, 0, ChunkRangeMax.Z)

                Loader.LoadAndReplaceChunksRegions(MinPX, MaxPX, MinPZ, MaxPZ)


            ElseIf ChunkRangeMax.Z = OldChunkRangeMax.Z Then

                'X -> ++ 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX >= Size.X Then
                    RootX -= Size.X
                End If


                Dim MinPX = New IntVector3(OldChunkRangeMax.X, 0, OldChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(ChunkRangeMax.X, 0, OldChunkRangeMax.Z)

                Loader.LoadAndReplaceChunksRegion(MinPX, MaxPX)


            ElseIf ChunkRangeMax.Z < OldChunkRangeMax.Z Then

                'X -> ++ 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX >= Size.X Then
                    RootX -= Size.X
                End If

                'Z -> --
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ < 0 Then
                    RootZ += Size.Z
                End If




                Dim MinPX = New IntVector3(OldChunkRangeMax.X, 0, ChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(ChunkRangeMax.X, 0, ChunkRangeMax.Z)



                Dim MinPZ = New IntVector3(ChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPZ = New IntVector3(OldChunkRangeMax.X, 0, OldChunkRangeMin.Z)

                Loader.LoadAndReplaceChunksRegions(MinPX, MaxPX, MinPZ, MaxPZ)




            End If


        ElseIf ChunkRangeMax.X < OldChunkRangeMax.X Then


            If ChunkRangeMax.Z > OldChunkRangeMax.Z Then
                'X -> -- and Z -> ++

                'X -> -- 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX < 0 Then
                    RootX += Size.X
                End If
                'Z -> ++
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ >= Size.Z Then
                    RootZ -= Size.Z
                End If



                Dim MinPX = New IntVector3(ChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(OldChunkRangeMin.X, 0, ChunkRangeMax.Z)



                Dim MinPZ = New IntVector3(OldChunkRangeMin.X, 0, OldChunkRangeMax.Z)
                Dim MaxPZ = New IntVector3(ChunkRangeMax.X, 0, ChunkRangeMax.Z)

                Loader.LoadAndReplaceChunksRegions(MinPX, MaxPX, MinPZ, MaxPZ)


            ElseIf ChunkRangeMax.Z = OldChunkRangeMax.Z Then

                'X -> -- 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX < 0 Then
                    RootX += Size.X
                End If


                Dim MinPX = New IntVector3(ChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(OldChunkRangeMin.X, 0, ChunkRangeMax.Z)

                Loader.LoadAndReplaceChunksRegion(MinPX, MaxPX)


            ElseIf ChunkRangeMax.Z < OldChunkRangeMax.Z Then

                'X -> -- 
                RootX += ChunkRangeMax.X - OldChunkRangeMax.X
                If RootX < 0 Then
                    RootX += Size.X
                End If

                'Z -> --
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ < 0 Then
                    RootZ += Size.Z
                End If


                Dim MinPX = New IntVector3(ChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPX = New IntVector3(OldChunkRangeMin.X, 0, ChunkRangeMax.Z)


                Dim MinPZ = New IntVector3(OldChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPZ = New IntVector3(ChunkRangeMax.X, 0, OldChunkRangeMin.Z)

                Loader.LoadAndReplaceChunksRegions(MinPX, MaxPX, MinPZ, MaxPZ)



            End If





        ElseIf ChunkRangeMax.X = OldChunkRangeMax.X Then

            If ChunkRangeMax.Z > OldChunkRangeMax.Z Then
                'Z -> ++
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ >= Size.Z Then
                    RootZ -= Size.Z
                End If

                Dim MinPZ = New IntVector3(OldChunkRangeMin.X, 0, OldChunkRangeMax.Z)
                Dim MaxPZ = New IntVector3(OldChunkRangeMax.X, 0, ChunkRangeMax.Z)

                Loader.LoadAndReplaceChunksRegion(MinPZ, MaxPZ)


            ElseIf ChunkRangeMax.Z < OldChunkRangeMax.Z Then
                'Z -> --
                RootZ += ChunkRangeMax.Z - OldChunkRangeMax.Z
                If RootZ < 0 Then
                    RootZ += Size.Z
                End If

                Dim MinPZ = New IntVector3(ChunkRangeMin.X, 0, ChunkRangeMin.Z)
                Dim MaxPZ = New IntVector3(OldChunkRangeMax.X, 0, OldChunkRangeMin.Z)

                Loader.LoadAndReplaceChunksRegion(MinPZ, MaxPZ)


            End If


        End If






        Array.Clear(Ground.TmpSurfaceChunks, 0, Ground.TmpFilledSFC + 1)
        Ground.TmpFilledSFC = 0




        Scrolling = False



    End Sub

End Class