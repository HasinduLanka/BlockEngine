Imports Microsoft.Xna.Framework

Public Class AIs

#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared YoungMan As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubYoungManAI)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared Civilian As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubCivilianAI)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared Guard As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubGuardAI)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared Worker As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubWorkersAI)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared Murdurer As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubMurdurerAI)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.

#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared AttackOnSight As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubAttackOnSight)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
#Disable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.
	Public Shared AttackWhenCloseBy As Action(Of Human) = New Action(Of Entity)(AddressOf AIs.SubAttackWhenCloseBy)
#Enable Warning BC42016 ' Implicit conversion from 'Entity' to 'Human'.

	Private Shared Sub SubYoungManAI(E As Human)
        With E



            .BodyParts(Human.iLHand).OriginalRotation = .ModelRotationY
            .BodyParts(Human.iRHand).OriginalRotation = .ModelRotationY

            If Not .NoTarget Then
                If .Target IsNot Nothing AndAlso Not .Target.IsDead Then
                    DoCurrentAITartgetMode(E)
                Else
                    .ReleaseTartget()
                End If

            Else


                'Random movements
                If Not .InRandomMovement Then

                    .PickNewRandomMovement()

                Else

                    .CurrActionLst.Add(.CRandomMovement)
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .InRandomMovement = False
                        .PickNewRandomMovement()
                    End If

                End If

            End If






            .DoCurrentActions()

        End With

    End Sub





    Private Shared Sub SubCivilianAI(E As Human)
        With E





            .BodyParts(Human.iLHand).OriginalRotation = .ModelRotationY
            .BodyParts(Human.iRHand).OriginalRotation = .ModelRotationY

            If Not .NoTarget Then
                If .Target IsNot Nothing AndAlso Not .Target.IsDead Then


                    DoCurrentAITartgetMode(E)

                Else
                    .ReleaseTartget()
                End If

            Else


                'No Target

                'Random movements
                If Not .InRandomMovement Then

                    .PickNewRandomMovement()

                Else

                    .CurrActionLst.Add(.CRandomMovement)
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .InRandomMovement = False
                        .PickNewRandomMovement()
                    End If

                End If

            End If




            .DoCurrentActions()



        End With

    End Sub






    Private Shared Sub SubMurdurerAI(E As Human)
        With E






            .BodyParts(Human.iLHand).OriginalRotation = .ModelRotationY
            .BodyParts(Human.iRHand).OriginalRotation = .ModelRotationY

            If Not .NoTarget Then
                If .Target IsNot Nothing AndAlso Not .Target.IsDead Then

                    DoCurrentAITartgetMode(E)

                Else
                    .ReleaseTartget()
                End If

            Else




                'No Target

                'Random movements
                If Not .InRandomMovement Then

                    .PickNewRandomMovement()

                Else

                    .CurrActionLst.Add(.CRandomMovement)
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .InRandomMovement = False
                        .PickNewRandomMovement()
                    End If

                End If


            End If




            .DoCurrentActions()


        End With

    End Sub

    '  Public Shared ViewerProjection As Matrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70.0F), 1, 1.0F, 1000)






    Private Shared Sub SubGuardAI(E As Human)
        With E




            .BodyParts(Human.iLHand).OriginalRotation = .ModelRotationY
            .BodyParts(Human.iRHand).OriginalRotation = .ModelRotationY

            If Not .NoTarget Then
                If .Target IsNot Nothing AndAlso Not .Target.IsDead Then

                    DoCurrentAITartgetMode(E)
                Else
                    .ReleaseTartget()
                End If

            Else


                'No Target

                'Random movements
                If Not .InRandomMovement Then

                    .PickNewRandomMovement()

                Else

                    .CurrActionLst.Add(.CRandomMovement)
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .InRandomMovement = False
                        .PickNewRandomMovement()
                    End If

                End If

            End If







            .DoCurrentActions()


        End With

    End Sub







    Private Shared Sub SubWorkersAI(E As Human)
        With E



            .BodyParts(Human.iLHand).OriginalRotation = .ModelRotationY
            .BodyParts(Human.iRHand).OriginalRotation = .ModelRotationY

            If Not .NoTarget Then
                If .Target IsNot Nothing AndAlso Not .Target.IsDead Then

                    DoCurrentAITartgetMode_DoNotPickNewRndMotion(E)

                Else
                    .ReleaseTartget()
                End If

            End If




            .DoCurrentActions()

        End With

    End Sub






    Private Shared Sub SubAttackOnSight(E As Human)


        With E
            If .NoTarget Then
                If Not .CheckOutOfStackRange Then


                    Dim ViewFrustum = New BoundingFrustum(Matrix.CreateLookAt(.Position, .Position + .HeadRotation.Forward, .HeadRotation.Up) * Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70.0F), 1, 1.0F, .CloseByRange))

                    Dim TargetDetected = False
                    For Each Ent In Ground.CStack.eList
                        If Ent.ICode <> .ICode Then
                            If ViewFrustum.Intersects(New BoundingSphere(Ent.Position, 100)) Then

                                If .CheckAndLockTargetForSee(Ent) <> AITargetMode.None Then
                                    TargetDetected = True
                                    Exit For
                                End If

                            End If
                        End If
                    Next


                End If

            End If
        End With

    End Sub





    Private Shared Sub SubAttackWhenCloseBy(E As Human)
        With E
            If .NoTarget Then
                If Not .CheckOutOfStackRange Then

                    Dim TargetDetected = False
                    Dim BS As New BoundingSphere(.Position, .CloseByRange)
                    For Each Ent In Ground.CStack.eList
                        If BS.Intersects(New BoundingSphere(Ent.Position, 100)) Then

                            If .CheckAndLockTargetForSee(Ent) <> AITargetMode.None Then
                                TargetDetected = True
                                Exit For
                            End If

                        End If
                    Next

                End If
            End If
        End With

    End Sub







    Private Shared Sub DoCurrentAITartgetMode(E As Human)
        With E


            Select Case .TargetMode
                Case AITargetMode.FollowAndKill

                    LookAtEntity(E, .Target)



                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)


                    If .CTool IsNot Nothing Then
                        If TargetDistace > .CTool.MaxAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Forward)
                        ElseIf TargetDistace < .CTool.MinAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Backward)
                        Else
                            .CurrActionLst.Add(Actions.Attack)
                        End If
                    End If


                Case AITargetMode.FollowAndAttackOnce

                    LookAtEntity(E, .Target)

                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)

                    If .CTool IsNot Nothing Then
                        If TargetDistace > .CTool.MaxAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Forward)
                        ElseIf TargetDistace < .CTool.MinAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Backward)
                        Else
                            .CurrActionLst.Add(Actions.Attack)
                            .ReleaseTartget()
                        End If
                    End If




                Case AITargetMode.Follow

                    LookAtEntity(E, .Target)


                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)

                    If TargetDistace > .eType.Width + 100 Then
                        LookAtEntity(E, .Target)
                        .CurrActionLst.Add(Actions.Forward)
                    ElseIf TargetDistace < .eType.Width Then
                        LookAtEntity(E, .Target)
                        .CurrActionLst.Add(Actions.Backward)
                    End If

                Case AITargetMode.Look

                    LookAtEntity(E, .Target)
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .eType.LookingAtTimeout Then
                        .PickNewRandomMovement()
                    End If

                Case AITargetMode.RunFromIt
                    LookOutFromEntity(E, .Target)
                    .CurrActionLst.Add(Actions.Forward)

                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .PickNewRandomMovement()
                    End If

                Case AITargetMode.None
                    .CRandomMovementTime += 1
                    If .CRandomMovementTime > .InRandomMovementTime Then
                        .PickNewRandomMovement()
                    End If

            End Select




        End With

    End Sub




    Private Shared Sub DoCurrentAITartgetMode_DoNotPickNewRndMotion(E As Human)
        With E


            Select Case .TargetMode
                Case AITargetMode.FollowAndKill

                    LookAtEntity(E, .Target)



                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)

                    If .CTool IsNot Nothing Then
                        If TargetDistace > .CTool.MaxAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Forward)
                        ElseIf TargetDistace < .CTool.MinAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Backward)
                        Else
                            .CurrActionLst.Add(Actions.Attack)
                        End If
                    End If

                Case AITargetMode.FollowAndAttackOnce

                    LookAtEntity(E, .Target)

                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)
                    If .CTool IsNot Nothing Then
                        If TargetDistace > .CTool.MaxAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Forward)
                        ElseIf TargetDistace < .CTool.MinAttackingDistance Then
                            LookAtEntity(E, .Target)
                            .CurrActionLst.Add(Actions.Backward)
                        Else
                            .CurrActionLst.Add(Actions.Attack)
                            .ReleaseTartget(False)
                        End If
                    End If



                Case AITargetMode.Follow

                    LookAtEntity(E, .Target)


                    Dim TargetDistace = Vector3.Distance(.Target.Position, .Position)

                    If TargetDistace > .eType.Width + 100 Then
                        LookAtEntity(E, .Target)
                        .CurrActionLst.Add(Actions.Forward)
                    ElseIf TargetDistace < .eType.Width Then
                        LookAtEntity(E, .Target)
                        .CurrActionLst.Add(Actions.Backward)
                    End If

                Case AITargetMode.Look
                    LookAtEntity(E, .Target)


                Case AITargetMode.RunFromIt
                    LookOutFromEntity(E, .Target)
                    .CurrActionLst.Add(Actions.Forward)


                Case AITargetMode.None


            End Select




        End With

    End Sub
End Class
