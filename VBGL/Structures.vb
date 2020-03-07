Imports Microsoft.Xna.Framework


Public Class Struct

    Public ID As Byte
    Public Size3D As Vector3

    ''' <summary>
    ''' Size3D - 1
    ''' </summary>
    Public Length3D As Vector3


    Public IsStructArray()() As Boolean

    ''' <summary>
    ''' X Z Y
    ''' </summary>
    Public Content()()() As BlockType

    Public HasEList As Boolean = False
    Public eList As New List(Of XEntity)

    Public Sub New()

    End Sub

    Public Enum EnumStructs As Byte

        None = 0
        PlaceHolder = 1

        Tree1 = 10
        TreeJak = 11

        HouseSmall1 = 30
        HouseSmall2 = 31

        WatchTower1 = 40
        Hut1 = 41

    End Enum


    Public Shared Lst() As Struct
    Public Shared Choices() As Struct

    Public Shared Sub Initialize()
        Lst = New Struct(100) {}

        Lst(EnumStructs.Tree1) = Tree1
        Lst(EnumStructs.TreeJak) = TreeJak
        Lst(EnumStructs.HouseSmall1) = HouseSmall1
        Lst(EnumStructs.HouseSmall2) = HouseSmall2
        Lst(EnumStructs.WatchTower1) = WatchTower1
        Lst(EnumStructs.Hut1) = Hut1


        Choices = New Struct() {Tree1, TreeJak, HouseSmall1, HouseSmall2, WatchTower1, Hut1}


    End Sub


    Private Shared ReadOnly Property Tree1 As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 10
                .Size3D = New Vector3(6, 11, 6)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)

            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next



#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next




            For X = 2 To 4
                For Z = 2 To 4
                    For Y = 1 To 10
                        O.Content(X)(Z)(Y) = BlockType.PlaceHolder
                    Next
                    O.IsStructArray(X)(Z) = True
                Next
            Next

            O.Content(3)(3)(0) = BlockType.Tree1
            O.IsStructArray(3)(3) = True

            Return O

        End Get
    End Property

    Private Shared ReadOnly Property TreeJak As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 11
                .Size3D = New Vector3(7, 15, 7)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)
            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next




            For X = 3 To 5
                For Z = 3 To 5
                    For Y = 1 To 14
                        O.Content(X)(Z)(Y) = BlockType.PlaceHolder
                    Next
                    O.IsStructArray(X)(Z) = True
                Next
            Next


            O.Content(4)(4)(0) = BlockType.Jak
            O.IsStructArray(4)(4) = True


            Return O

        End Get
    End Property




    Private Shared ReadOnly Property HouseSmall1 As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 30
                .Size3D = New Vector3(11, 12, 11)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)
            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next


            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(0) = BlockType.Stone
                    O.IsStructArray(X)(Z) = True
                Next
            Next

            For X = 1 To 9
                For Y = 0 To 5
                    O.Content(X)(1)(Y) = BlockType.Brick
                Next
                O.IsStructArray(X)(1) = True
            Next

            For Y = 1 To 3
                O.Content(5)(1)(Y) = BlockType.Air
            Next


            For X = 1 To 9
                For Y = 0 To 5
                    O.Content(X)(9)(Y) = BlockType.Brick
                Next
                O.IsStructArray(X)(9) = True
            Next

            For Z = 1 To 9
                For Y = 0 To 5
                    O.Content(1)(Z)(Y) = BlockType.Brick
                Next
                O.IsStructArray(1)(Z) = True
            Next

            For Z = 1 To 9
                For Y = 0 To 5
                    O.Content(9)(Z)(Y) = BlockType.Brick
                Next
                O.IsStructArray(9)(Z) = True
            Next



            For X = 0 To 10
                For Z = 0 To 10
                    O.Content(X)(Z)(6) = BlockType.WoodPlank
                    O.IsStructArray(X)(Z) = True
                Next
            Next

            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(6) = BlockType.Air
                    'O.IsStructArray(X)(Z) = True
                Next
            Next

            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(7) = BlockType.WoodPlank
                Next
            Next

            For X = 2 To 8
                For Z = 2 To 8
                    O.Content(X)(Z)(7) = BlockType.Air
                Next
            Next

            For X = 2 To 8
                For Z = 2 To 8
                    O.Content(X)(Z)(8) = BlockType.WoodPlank
                Next
            Next

            For X = 3 To 7
                For Z = 3 To 7
                    O.Content(X)(Z)(8) = BlockType.Air
                Next
            Next


            For X = 3 To 7
                For Z = 3 To 7
                    O.Content(X)(Z)(9) = BlockType.WoodPlank
                Next
            Next

            For X = 4 To 6
                For Z = 4 To 6
                    O.Content(X)(Z)(9) = BlockType.Air
                Next
            Next


            For X = 4 To 6
                For Z = 4 To 6
                    O.Content(X)(Z)(10) = BlockType.WoodPlank
                Next
            Next









            O.HasEList = True

            Dim H = Human.AddNewHuman(EntityTypes.Civilian, "CivilianGenHouseSmall1", New Vector3(200, 150, 200), Nothing, 1)
            O.eList.Add(New XEntity(H))


            Return O

        End Get
    End Property




    Private Shared ReadOnly Property HouseSmall2 As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 31
                .Size3D = New Vector3(22, 12, 22)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)
            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next



            For X = 1 To 20
                For Z = 1 To 20
                    O.Content(X)(Z)(0) = BlockType.Stone
                    O.IsStructArray(X)(Z) = True
                Next
            Next



            For X = 1 To 20
                For Y = 0 To 5
                    O.Content(X)(1)(Y) = BlockType.Brick
                Next
            Next

            For Y = 1 To 3
                O.Content(5)(1)(Y) = BlockType.Air
            Next


            For X = 1 To 20
                For Y = 0 To 5
                    O.Content(X)(20)(Y) = BlockType.Brick
                Next
            Next




            For Z = 1 To 20
                For Y = 0 To 5
                    O.Content(1)(Z)(Y) = BlockType.Brick
                Next
            Next

            For Z = 1 To 20
                For Y = 0 To 5
                    O.Content(20)(Z)(Y) = BlockType.Brick
                Next
            Next



            'Room Walls
            For X = 1 To 20
                For Y = 0 To 5
                    O.Content(X)(10)(Y) = BlockType.Brick
                Next
            Next

            For Z = 10 To 20
                For Y = 0 To 5
                    O.Content(10)(Z)(Y) = BlockType.Brick
                Next
            Next


            For Y = 1 To 3
                O.Content(17)(10)(Y) = BlockType.Air
            Next

            For Y = 1 To 3
                O.Content(4)(10)(Y) = BlockType.Air
            Next

            'Roof

            For X = 0 To 21
                For Z = 0 To 21
                    O.Content(X)(Z)(6) = BlockType.WoodPlank
                    O.IsStructArray(X)(Z) = True
                Next
            Next

            For X = 1 To 20
                For Z = 1 To 20
                    O.Content(X)(Z)(6) = BlockType.Air
                Next
            Next

            For X = 1 To 20
                For Z = 1 To 20
                    O.Content(X)(Z)(7) = BlockType.WoodPlank
                Next
            Next

            For X = 2 To 19
                For Z = 2 To 19
                    O.Content(X)(Z)(7) = BlockType.Air
                Next
            Next

            For X = 2 To 19
                For Z = 2 To 19
                    O.Content(X)(Z)(8) = BlockType.WoodPlank
                Next
            Next

            For X = 3 To 18
                For Z = 3 To 18
                    O.Content(X)(Z)(8) = BlockType.Air
                Next
            Next


            For X = 3 To 18
                For Z = 3 To 18
                    O.Content(X)(Z)(9) = BlockType.WoodPlank
                Next
            Next

            For X = 4 To 17
                For Z = 4 To 17
                    O.Content(X)(Z)(9) = BlockType.Air
                Next
            Next


            For X = 4 To 17
                For Z = 4 To 17
                    O.Content(X)(Z)(10) = BlockType.WoodPlank
                Next
            Next









            O.HasEList = True

            Dim H = Human.AddNewHuman(EntityTypes.Civilian, "CivilianGenHouseSmall2", New Vector3(200, 150, 200), Nothing, 1)
            O.eList.Add(New XEntity(H))


            Return O

        End Get
    End Property









    Private Shared ReadOnly Property WatchTower1 As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 40
                .Size3D = New Vector3(11, 20, 11)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)
            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next


            'Legs
            For Y = 0 To 16
                O.Content(0)(0)(Y) = BlockType.Wood
            Next
            O.IsStructArray(0)(0) = True
            O.Content(0)(0)(1) = BlockType.Brick


            For Y = 0 To 16
                O.Content(10)(0)(Y) = BlockType.Wood
            Next
            O.IsStructArray(10)(0) = True
            O.Content(10)(0)(1) = BlockType.Brick


            For Y = 0 To 16
                O.Content(10)(10)(Y) = BlockType.Wood
            Next
            O.IsStructArray(10)(10) = True
            O.Content(10)(10)(1) = BlockType.Brick


            For Y = 0 To 16
                O.Content(0)(10)(Y) = BlockType.Wood
            Next
            O.IsStructArray(0)(10) = True
            O.Content(0)(10)(1) = BlockType.Brick



            'Hut

            'Floor
            For X = 0 To 10
                For Z = 0 To 10
                    O.Content(X)(Z)(11) = BlockType.Wood
                    O.IsStructArray(X)(Z) = True
                Next
            Next

            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(11) = BlockType.WoodPlank
                Next
            Next



            'Roof
            For X = 0 To 10
                For Z = 0 To 10
                    O.Content(X)(Z)(16) = BlockType.Wood
                Next
            Next
            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(16) = BlockType.WoodPlank
                Next
            Next



            'Side Walls
            For X = 0 To 10
                For Z = 0 To 10
                    O.Content(X)(Z)(12) = BlockType.WoodPlank
                Next
            Next

            For X = 1 To 9
                For Z = 1 To 9
                    O.Content(X)(Z)(12) = BlockType.Air
                Next
            Next









            O.HasEList = True

            Dim H = Human.AddNewHuman(EntityTypes.Guard, "WatchTowerGuard", New Vector3(200, 50 * 14, 200),
                                      Tool.Buy(RandomOf({Tool.iSword1, Tool.iRSword})), 1)
            H.InRandomMovement = True
            H.InRandomMovementTime = 100000
            H.CRandomMovement = Actions.Null

            O.eList.Add(XEntity.NewXE(H, New Vector3(200, 50 * 14, 200)))
            O.eList.Add(XEntity.NewXE(H, New Vector3(300, 50 * 14, 300)))

            Return O

        End Get
    End Property



    Private Shared ReadOnly Property Hut1 As Struct
        Get
            Dim O As New Struct
            With O

                .ID = 41
                .Size3D = New Vector3(6, 8, 6)
                .Length3D = New Vector3(.Size3D.X - 1, .Size3D.Y - 1, .Size3D.Z - 1)
            End With


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.Content = New BlockType(O.Length3D.X)()() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.Content(X) = New BlockType(O.Length3D.Z)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                For Z = 0 To O.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    O.Content(X)(Z) = New BlockType(O.Length3D.Y) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    For Y = 0 To O.Length3D.Y
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                        O.Content(X)(Z)(Y) = BlockType.Air
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                    Next

                Next
            Next


#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            O.IsStructArray = New Boolean(O.Length3D.X)() {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            For X = 0 To O.Length3D.X
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                O.IsStructArray(X) = New Boolean(O.Length3D.Z) {}
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
            Next



            'Hut





            'Walls
            For X = 0 To 5
                For Y = 0 To 7
                    O.Content(X)(0)(Y) = BlockType.WoodPlank
                Next
            Next

            For X = 0 To 5
                For Y = 0 To 7
                    O.Content(X)(5)(Y) = BlockType.WoodPlank
                Next
            Next


            For Z = 0 To 5
                For Y = 0 To 7
                    O.Content(0)(Z)(Y) = BlockType.WoodPlank
                Next
            Next

            For Z = 0 To 5
                For Y = 0 To 7
                    O.Content(5)(Z)(Y) = BlockType.WoodPlank
                Next
            Next


            For Y = 1 To 3
                O.Content(0)(2)(Y) = BlockType.Air
            Next


            'Floor
            For X = 0 To 5
                For Z = 0 To 5
                    O.Content(X)(Z)(0) = BlockType.Dirt
                    O.IsStructArray(X)(Z) = True
                Next
            Next


            'Roof
            For X = 0 To 5
                For Z = 0 To 5
                    O.Content(X)(Z)(6) = BlockType.Wood
                Next
            Next




            O.HasEList = True

            Dim H = Human.AddNewHuman(RandomOf({EntityTypes.Human1, EntityTypes.Civilian, EntityTypes.Human1, EntityTypes.Murderer}), "HutGen", New Vector3(100, 50, 100), Tool.Buy(RandomOf({Tool.iSword1, Tool.iRSword})), 1)

            H.InRandomMovement = True
            H.InRandomMovementTime = 100000
            H.CRandomMovement = Actions.Null

            O.eList.Add(XEntity.NewXE(H, New Vector3(100, 100, 100)))

            Return O

        End Get
    End Property


    Public Shared Function GenerateStructMap(ByRef HM As HeightMap, ByRef IsStruct()() As Boolean, ByRef Heights()() As Byte, ByRef eList As List(Of XEntity)) As BlockType()()()

        Dim Bars(HeightMap.Size - 1)()() As BlockType
        IsStruct = New Boolean(HeightMap.Size - 1)() {}
        Heights = New Byte(HeightMap.Size - 1)() {}
        For X = 0 To HeightMap.Size - 1
            IsStruct(X) = New Boolean(HeightMap.Size - 1) {}
            Bars(X) = New BlockType(HeightMap.Size - 1)() {}
            Heights(X) = New Byte(HeightMap.Size - 1) {}
        Next



        Dim BiomeStructCount = HM.Bm.Structs.Length

        For X = 10 To HeightMap.Size - 10

            For Z = 10 To HeightMap.Size - 10

                If Not IsStruct(X)(Z) Then
ReselectStruct:
                    Dim R = RND.Next(BiomeStructCount)
                    If RND.NextDouble < HM.Bm.StructsRarity(R) Then

                        'Apply
                        Dim S = Lst(HM.Bm.Structs(R))

                        If HeightMap.Size <= X + S.Length3D.X OrElse HeightMap.Size <= Z + S.Length3D.Z Then
                            GoTo ReselectStruct
                        End If

                        Dim IsIntersects = False
                        Dim BaseHeightTotal = 0I
                        For SX = 0 To S.Length3D.X
                            For SZ = 0 To S.Length3D.Z
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                BaseHeightTotal += HM.B(X + SX)(Z + SZ)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                If IsStruct(X + SX)(Z + SZ) Then
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                    IsIntersects = True
                                    Exit For
                                End If
                            Next
                            If IsIntersects Then Exit For
                        Next


                        If Not IsIntersects Then
                            'Apply
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
                            Dim StructureBaseHeight As Integer = Math.Truncate(BaseHeightTotal / (S.Size3D.X * S.Size3D.Z))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

                            CloneXELise(S.eList, eList, New Vector3(X * Ground.BlockSize, StructureBaseHeight * Ground.BlockSize, Z * Ground.BlockSize))

                            For SX = 0 To S.Length3D.X
                                For SZ = 0 To S.Length3D.Z

#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                    IsStruct(X + SX)(Z + SZ) = True
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.

#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                    If S.IsStructArray(SX)(SZ) Then
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                        HM.B(X + SX)(Z + SZ) = StructureBaseHeight
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                        Bars(X + SX)(Z + SZ) = S.Content(SX)(SZ)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                        Heights(X + SX)(Z + SZ) = S.Length3D.Y
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
                                    End If

                                Next
                            Next

                        End If


                    End If

                End If

            Next

        Next


        Return Bars
    End Function



    Private Shared Sub CloneXELise(XELst As List(Of XEntity), Dest As List(Of XEntity), Pos As Vector3)

        For Each XE In XELst
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
            Entity.LICode += 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
            Dest.Add(New XEntity() With {.Accelaration = XE.Accelaration,
                     .CRandomMovement = XE.CRandomMovement,
                     .CRandomMovementTime = XE.CRandomMovementTime,
                     .CTool = XE.CTool,
                     .Enemies = XE.Enemies,
                     .FacingDirection = XE.FacingDirection,
                     .FallingSpeed = XE.FallingSpeed,
                     .Friendlies = XE.Friendlies,
                     .Health = XE.Health,
                     .MaxHealth = XE.MaxHealth,
                     .HighestTargetMode = XE.HighestTargetMode,
                     .HighestTatget = XE.HighestTatget,
                     .HighestTatgetE = XE.HighestTatgetE,
                     .ICode = Entity.LICode,
                     .InRandomMovement = XE.InRandomMovement,
                     .InRandomMovementTime = XE.InRandomMovementTime,
                     .IsDead = XE.IsDead,
                     .Jumping = XE.Jumping,
                     .MaxVelocity = XE.MaxVelocity,
                     .ModelVelocity = XE.ModelVelocity,
                     .MovedFB = XE.MovedFB,
                     .Name = XE.Name,
                     .NoTarget = XE.NoTarget,
                     .OnGround = XE.OnGround,
                     .Position = XE.Position + Pos,
                     .RotationAccelaration = XE.RotationAccelaration,
                     .RotationVelocity = XE.RotationVelocity,
                     .Strength = XE.Strength,
                     .Target = XE.Target,
                     .TargetLockedAIs = XE.TargetLockedAIs,
                     .TargetMode = XE.TargetMode,
                     .Tools = XE.Tools,
                     .TypeID = XE.TypeID,
                     .WalkingFw = XE.WalkingFw,
                     .Weight = XE.Weight,
                     .XP = XE.XP})
        Next

    End Sub

End Class

