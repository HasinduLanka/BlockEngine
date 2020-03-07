Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Tool : Inherits ePart
    Public Shared Tools As New List(Of Tool)(4)
    Public Shared iSword1 As Integer
    Public Shared iBow1 As Integer
    Public Shared iRSword As Integer
    Public Shared iShovel As Integer


    Public Model As Model
    Public Transforms As Matrix()


    Public Velocity As Vector3 = Vector3.Zero
    Public Owner As ePart
    Public Side As Integer
    Public Attacking As Boolean = False
    Public Type1 As Types

    Public RewardsToVictim As RewardSet

    Public R As Ray


    Public MaxAttackingDistance As Single = 90
    Public MinAttackingDistance As Single = 70


    Public Shared Function Buy(ToolI As Integer) As Tool
        'O = Tools(ToolI)
        Dim O As New Tool With {
            .Model = Tools(ToolI).Model,
            .Transforms = Tools(ToolI).Transforms,
            .Name = Tools(ToolI).Name,
            .Rotation = Tools(ToolI).Rotation,
            .RelativePosition = Tools(ToolI).RelativePosition,
            .OriginalRotation = Tools(ToolI).OriginalRotation,
            .DefualtRotation = Tools(ToolI).DefualtRotation,
            .Type1 = Tools(ToolI).Type1,
            .Length = Tools(ToolI).Length,
            .RewardsToVictim = Tools(ToolI).RewardsToVictim,
            .MaxAttackingDistance = Tools(ToolI).MaxAttackingDistance,
            .MinAttackingDistance = Tools(ToolI).MinAttackingDistance
        }
        Return O
    End Function

#Disable Warning BC42016 ' Implicit conversion
    Public CAnimation As ePAnimationChain
    Public Sub Use(A As Action)
        If Not Attacking Then



            If Owner.LockedeA = False Then
                Owner.LockedeA = True
                Attacking = True


                CAnimation = AnimationLst.Item(A).Clone(Owner)

                CAnimation.Name = "PA1"
                CAnimation.Reset_LockedeA_After_eA.Add(Owner)
                CAnimation.Revert_After_eA.AddRange({Owner, Me})
                AddHandler CAnimation.Completed, AddressOf AttackCompleted


                For Each eA In CAnimation.AnimationList

                    eA.ePAnimationChain1 = CAnimation

                    For Each PeA In eA.Parellel_eAs
                        PeA.ePAnimationChain1 = CAnimation
                    Next

                Next



                CAnimation.Start()

            End If






        End If
    End Sub

#Enable Warning BC42016 ' Implicit conversion



    Public Sub Update()
        R = New Ray(RelativePosition + Parent.Position, Rotation.Up)

    End Sub









    Public Sub AttackCompleted(Sender As ePAnimationChain, e As EventArgs)

        eAChildRotationGradient = 0

        Owner.ChildRotationGradient = 0

        Attacking = False
        Charter.StandStraitInstantly()
        'Charter.ResumeWalking()

        Charter.PuaseWalking()
        Charter.ResumeWalking()

        RemoveHandler CAnimation.Completed, AddressOf AttackCompleted

    End Sub



    Public Enum Action
        PrimaryAttack1
        PrimaryAttack2
        PrimaryAttack3
        ShortAttack
        SpecialAttak1
        SpecialAttak2
    End Enum

    Public Enum Types
        Sword
        Bow

        Shovel
    End Enum



    Public Shared AnimationLst As New Dictionary(Of Action, ePAnimationChain)


    Public Shared Sub InitializeAnimations()


        If True Then

            'Action.PrimaryAttack1
            Dim FeA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-2, 0}, False, {15, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), 1.05) With {
                .IsFakeTimer = True
            }

            Dim eA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-1.5, -0.5}, False, {15, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), 1.05)

            Dim eA2 = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-0.2, 0.5}, False, {15, 10}, New Axis("U".ToCharArray.First), 1)

            Dim eAS = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {0.1, 1}, False, {15, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), 2)

            FeA.Parellel_eAs.AddRange({eA, eA2, eAS})

            Dim CAnimation1 As New ePAnimationChain(New List(Of ePAnimation) From {FeA}, False)
            AnimationLst.Add(Action.PrimaryAttack1, CAnimation1)

        End If



        If True Then


            'Action.PrimaryAttack2
            Dim FeA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-2, 0}, False, {20, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), 0.5) With {
                .IsFakeTimer = True
            }

            Dim eA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-2, 1}, False, {20, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), 1)
            Dim eA2 = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {-0.4, 0.5}, False, {5, 25}, New Axis("U".ToCharArray.First), 1)


            FeA.Parellel_eAs.AddRange({eA, eA2})

            Dim CAnimation1 As New ePAnimationChain(New List(Of ePAnimation) From {FeA}, False)
            AnimationLst.Add(Action.PrimaryAttack2, CAnimation1)

        End If


        If True Then
            'Action.ShortAttack

            Dim FeA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {0.2, -0.8, 0}, False, {5, 10, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), -1.8) With {
                .IsFakeTimer = True
            }

            Dim eA = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {0.2, -0.5, 0}, False, {5, 10, 10}, New Axis("L".ToCharArray.First, False, New ePart(New Entity(EntityTypes.Human1))), -1.5)
            Dim eA2 = New ePAnimation(New ePart(New Entity(EntityTypes.Human1)), New Vector3, {0.2, 0.4, 0}, False, {5, 10, 10}, New Axis("U".ToCharArray.First), 1)


            FeA.Parellel_eAs.AddRange({eA, eA2})


            Dim CAnimation1 As New ePAnimationChain(New List(Of ePAnimation) From {FeA}, False)
            AnimationLst.Add(Action.ShortAttack, CAnimation1)

        End If



    End Sub




    Public Sub New(PParent As Entity)
        MyBase.New(PParent)
    End Sub

    Public Sub New()
        MyBase.New(New Entity(EntityTypes.WayPoint))
    End Sub
End Class




'Public Class AnimationList



'    'Public Shared Function GetAnimation(Tool.Action) As ePAnimationChain

'    'End Function


'End Class




