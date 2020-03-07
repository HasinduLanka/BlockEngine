



Imports Microsoft.Xna.Framework

Public Class Biome

    Public Name As String

    Public Index As Integer

    Public SurfaceBlock As BlockType
    Public InnerBlock As BlockType


    Public RowPattern() As Integer

    Public FlagDistance As Integer = 10
    Public FlagLerpAmont As Integer = 0


    Public MaxHeight As Integer = 80


    Public FlagPowerMin As Integer = 1
    Public FlagPowerMax As Integer = 3

    Public Structs() As Struct.EnumStructs
    Public StructsRarity() As Double


    Public Function MakeRows(XLength As Integer, YLength As Integer) As Integer()()
        Dim A()() As Integer
        A = New Integer(XLength - 1)() {}
        For x = 0 To YLength - 1
            A(x) = New Integer(YLength - 1) {}
        Next


#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim nFlagsX As Integer = Math.Truncate(XLength / FlagDistance)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
        Dim nFlagsY As Integer = Math.Truncate(YLength / FlagDistance)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.


        Dim Flags(nFlagsX)() As Integer
        For x = 0 To nFlagsX
            Flags(x) = New Integer(nFlagsY) {}
        Next


        'Creating Flags
        For X = 0 To nFlagsX
            For Y = 0 To nFlagsY

#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
                Flags(X)(Y) = Math.Round(Math.Min(Math.Max(RowPattern(RND.Next(0, RowPattern.Length)), 0), MaxHeight))
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.

            Next

        Next


        Dim nFLAF = (FlagLerpAmont * 2) * (FlagLerpAmont * 2)


        'Lerping Flags
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
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

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
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

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
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

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
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

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
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                    Flags(X)(Y) = Math.Min(Math.Max(Math.Round(((F * RndFlagPower) + Average) / (RndFlagPower + 1)), 0), MaxHeight)
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

                Next
            Next




        End If





        'Creating Blocks by diffing flags


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

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                        A(BX)(BY) = (DDifY(m) + DDifX(m)(n)) / 2
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                        'A(BX)(BY) = DDifY(m)

                        m += 1
                    Next
                    n += 1
                Next



            Next
        Next




        Return A
    End Function










End Class









Public Class BiomeList


    Public Shared Lst As New List(Of Biome)

    Public Shared Sub Initialize()
        Lst.Clear()
        Lst.AddRange({Dessert, DessertHills, Flat, Plain, DraftHills, Hills, SuddenHills, DirtLands})



        'Air Chunk Byte Code
        Dim O(Loader.ChunkByteCodeLength) As Byte


        Dim n = 0
        For X = 0 To 7
            For Y = 0 To 7
                For Z = 0 To 7


                    O(n) = CByte(0)
                    O(n + 1) = CByte(0)

                    O(n + 2) = CByte(2)

                    n += 3
                Next
            Next
        Next


        O(Loader.ChunkByteCodeLength) = CByte(False)

        Loader.AirChunkByteCode = O

    End Sub



    Public Enum Biomes
        Dessert
        DessertHills

        Flat
        Plain

        DraftHills

        Hills
        SuddenHills

        DirtLands
    End Enum






    Public Shared ReadOnly Property Dessert As Biome
        Get
            Dim O As New Biome With {
                .Name = "Dessert",
                .Index = 0,
                .RowPattern = {0, 0, 10, 10, 0, 20, 30, 20, 0, 50},
                .InnerBlock = BlockType.Sand,
                .SurfaceBlock = BlockType.Sand,
                .FlagDistance = 20,
                .FlagLerpAmont = 4,
                .FlagPowerMax = 1,
                .FlagPowerMin = 0,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.HouseSmall1, Struct.EnumStructs.HouseSmall2, Struct.EnumStructs.WatchTower1},
                .StructsRarity = New Double() {0.0003, 0.000003, 0.000005}
            }

            Return O
        End Get
    End Property


    Public Shared ReadOnly Property DessertHills As Biome
        Get
            Dim O As New Biome With {
                .Name = "DessertHills",
                .Index = 1,
                .RowPattern = {0, 0, 20, 40, 0, 10},
                .InnerBlock = BlockType.Sand,
                .SurfaceBlock = BlockType.Sand,
                .FlagDistance = 40,
                .FlagLerpAmont = 2,
                .FlagPowerMax = 2,
                .FlagPowerMin = 0,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.HouseSmall1, Struct.EnumStructs.HouseSmall2, Struct.EnumStructs.WatchTower1},
                .StructsRarity = New Double() {0.0006, 0.0003, 0.00007}
            }

            Return O
        End Get
    End Property


    Public Shared ReadOnly Property Flat As Biome
        Get
            Dim O As New Biome With {
                .Name = "Flat",
                .Index = 2,
                .RowPattern = {0},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.GrassBlock,
                .FlagDistance = 20
            }
            Return O
        End Get
    End Property

    Public Shared ReadOnly Property Plain As Biome
        Get
            Dim O As New Biome With {
                .Name = "Plain",
                .Index = 3,
                .RowPattern = {8, 8, 8, 8, 8, 8, 10, 10, 12, 14, 15, 15, 15, 15, 16, 16},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.GrassBlock,
                .FlagDistance = 20,
                .FlagLerpAmont = 4,
                .FlagPowerMax = 1,
                .FlagPowerMin = 0,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.Tree1, Struct.EnumStructs.TreeJak, Struct.EnumStructs.HouseSmall1, Struct.EnumStructs.HouseSmall2, Struct.EnumStructs.WatchTower1, Struct.EnumStructs.Hut1},
                .StructsRarity = New Double() {0.008, 0.004, 0.0006, 0.0003, 0.00007, 0.0002}
            }

            Return O
        End Get
    End Property


    Public Shared ReadOnly Property DraftHills As Biome
        Get
            Dim O As New Biome With {
                .Name = "DraftHills",
                .Index = 4,
                .RowPattern = {0, 0, 0, 10, 10, 20, 30, 10, 10, 20},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.GrassBlock,
                .FlagLerpAmont = 2,
                .FlagDistance = 10,
                .FlagPowerMax = 2,
                .FlagPowerMin = 0,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.Tree1, Struct.EnumStructs.TreeJak, Struct.EnumStructs.HouseSmall1, Struct.EnumStructs.HouseSmall2, Struct.EnumStructs.WatchTower1, Struct.EnumStructs.Hut1},
                .StructsRarity = New Double() {0.01, 0.008, 0.0001, 0.0001, 0.0009, 0.0001}
            }

            Return O
        End Get
    End Property


    Public Shared ReadOnly Property Hills As Biome
        Get
            Dim O As New Biome With {
                .Name = "Hills",
                .Index = 5,
                .RowPattern = {0, 200, 100, 20, 50, 10, 50, 200, 0, 0},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.GrassBlock,
                .FlagDistance = 40,
                .FlagLerpAmont = 2,
                .FlagPowerMax = 2,
                .FlagPowerMin = 1,
                .Structs = New Struct.EnumStructs() {
                Struct.EnumStructs.Tree1,
                Struct.EnumStructs.TreeJak,
                Struct.EnumStructs.HouseSmall1,
                Struct.EnumStructs.HouseSmall2,
                Struct.EnumStructs.WatchTower1,
                Struct.EnumStructs.Hut1},
                .StructsRarity = New Double() {0.005, 0.004, 0.0001, 0.0001, 0.00007, 0.0003}
            }
            Return O
        End Get
    End Property




    Public Shared ReadOnly Property SuddenHills As Biome
        Get
            Dim O As New Biome With {
                .Name = "SuddenHills",
                .Index = 6,
                .RowPattern = {100, 100, 40, 40, 20, 10, 0, 0, 0, 100},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.GrassBlock,
                .FlagDistance = 40,
                .FlagLerpAmont = 2,
                .FlagPowerMax = 2,
                .FlagPowerMin = 1,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.Tree1, Struct.EnumStructs.TreeJak, Struct.EnumStructs.WatchTower1, Struct.EnumStructs.Hut1},
                .StructsRarity = New Double() {0.003, 0.002, 0.0003, 0.0003}
            }

            Return O
        End Get
    End Property






    Public Shared ReadOnly Property DirtLands As Biome
        Get
            Dim O As New Biome With {
                .Name = "DirtLands",
                .Index = 7,
                .RowPattern = {10, 20, 20, 0, 0},
                .InnerBlock = BlockType.Dirt,
                .SurfaceBlock = BlockType.Dirt,
                .FlagDistance = 10,
                .FlagLerpAmont = 2,
                .FlagPowerMax = 1,
                .FlagPowerMin = 0,
                .Structs = New Struct.EnumStructs() {Struct.EnumStructs.Tree1, Struct.EnumStructs.TreeJak, Struct.EnumStructs.HouseSmall1, Struct.EnumStructs.HouseSmall2, Struct.EnumStructs.WatchTower1, Struct.EnumStructs.Hut1},
                .StructsRarity = New Double() {0.008, 0.003, 0.009, 0.008, 0.00007, 0.001}
            }


            Return O
        End Get
    End Property



End Class






Public Class HeightMap

    Public Index As IntVector3
    Public Shared Size As Integer = 22 * 8


    Public B()() As Byte

    Public Bm As Biome


    Public Shared ByteCodeLength As Long = (Size * Size) + 4


End Class


