Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Human
    Inherits Entity



    Public GetingTargetBodyRotationLast As Single = 0
    Public GetingTargetBodyRotationLastDir As Boolean = False 'T=C  F=AC



    Public DualFacingDirections(1) As Direction


    Public Overrides Sub Update()



        If Not IsDead Then

            If (Not eType.IsPlayer) AndAlso CheckOutOfStackRange() Then
                Exit Sub
            End If


            Position += Velocity
            Velocity *= ModelVelocityReducingFactor

            If Velocity.LengthSquared < 0.02F Then Velocity = Vector3.Zero


            If CheckOutOfWorld() Then
                PutInsideOfTheWorld()
            End If


            FacingDirection = HeadRotation.Forward
            FacingDirection.Normalize()



            Dim RealChunkIndex As IntVector3


            DualFacingDirections = Physics.Find2DDualDirectionsOfDirection(FacingDirection)



            OnGround = False


            BlockEnv = Ground.GetBlockEnvironment(Position - Ground.BlockSizeHalfYonlyV3, True)

            If IsNothing(BlockEnv) Then


                ChunkOutOfStackRange()
                BlockEnv = Ground.GetBlockEnvironment(Position - Ground.BlockSizeHalfYonlyV3, True)

                If IsNothing(BlockEnv) Then

                    If eType.IsPlayer Then

                        While IsNothing(BlockEnv)
                            ChunkOutOfStackRange()
                            BlockEnv = Ground.GetBlockEnvironment(Position - Ground.BlockSizeHalfYonlyV3, True)
                        End While

                    Else
                        Exit Sub
                    End If

                End If


            End If









            CurrentBlock = BlockEnv.CurrentBlock



            CurrentChunk = CurrentBlock.Chunk

            DualFacingDirectionsFB = DualFacingDirections(0)
            DualFacingDirectionsLR = DualFacingDirections(1)



            If Not BlockEnv.LegsBlock.IsAir Then


                If TrappedCount > 10 Then
                    Position.Y += 100
                    BodyParts(iBody).Hurten(20)

                    TrappedCount = 0


                    Update()
                    Exit Sub

                Else

                    TrappedCount += 1
                End If

            Else
                TrappedCount = 0
            End If






            If Not CurrentBlock.RealBlock.IsAir Then


                Position.Y = (CurrentBlock.Index * Ground.BlockSize).Y + Ground.BlockSize
                'OnGround = True

                If Velocity.Y < -10 Then
                    BodyParts(iLKnee).Hurten(Velocity.Y * -2)
                    BodyParts(iRKnee).Hurten(Velocity.Y * -2)
                    'BodyParts(iBody).Hurten(FallingSpeed * 2)
                    Health -= CInt(Velocity.Y * -2)
                End If

                FallingSpeed = 0

            Else


                FallingSpeed += Weight
                Velocity.Y -= FallingSpeed

            End If





            'Detect collitions when falling fast
            If Velocity.Y < -10 Then

                If Ground.GetIsAirDistanceInTheDirection(Position, Vector3.Down, -2, False) > 20 Then



                    FallingSpeed = 0
                    Velocity.Y = 0

                    'Position.Y -= LandingDistance


                    BlockEnv = Ground.GetBlockEnvironment(Position - Ground.BlockSizeYonlyV3)

                    CurrentBlock = BlockEnv.CurrentBlock



                    DualFacingDirectionsFB = DualFacingDirections(0)
                    DualFacingDirectionsLR = DualFacingDirections(1)

                    DualFacingDirectionsFB = DualFacingDirections(0)
                    DualFacingDirectionsLR = DualFacingDirections(1)

                    CurrentChunk = CurrentBlock.Chunk




                    Position.Y = (CurrentBlock.Index * Ground.BlockSize).Y + Ground.BlockSize
                    OnGround = True


                End If


            End If


            If eType.IsPlayer Then
                RealChunkIndex = Ground.ChunkIndexOfPosition(Position - Ground.BlockSizeYonlyV3)

                If CurrentChunk.Index.X <> RealChunkIndex.X OrElse CurrentChunk.Index.Z <> RealChunkIndex.Z Then
                    CheckOutOfStackRange()
                End If

            End If


            If Not CurrentBlock.RealBlock.IsAir Then
                OnGround = True
            End If




            If MovedFB Then
                ResumeWalking()

            Else
                PuaseWalking()

            End If
            MovedFB = False



            If Not NoAI Then
                If DelUpdateAI IsNot Nothing Then DelUpdateAI(Me)
            End If



            If NeededBodyRotationChanged Then

                If ModelRotation.Forward <> NeededBodyRotation.Forward Then

                    If Vector3.Distance(NeededBodyRotation.Forward, ModelRotation.Forward) > GetingTargetBodyRotationLast Then
                        If GetingTargetBodyRotationLastDir Then 'CW
                            GetingTargetBodyRotationLastDir = False 'ACW

                        Else 'ACW
                            GetingTargetBodyRotationLastDir = True
                        End If
                    End If

                    GetingTargetBodyRotationLast = Vector3.Distance(ModelRotation.Forward, NeededBodyRotation.Forward)

                    If GetingTargetBodyRotationLastDir = False Then
                        'Anti ClockWise
#Disable Warning BC42016 ' Implicit conversion
                        RotationVelocity.Y += (Vector3.Distance(NeededBodyRotation.Forward, ModelRotation.Forward) * 0.2)

                    Else
                        'ClockWise
                        RotationVelocity.Y -= (Vector3.Distance(NeededBodyRotation.Forward, ModelRotation.Forward) * 0.2)
#Enable Warning BC42016 ' Implicit conversion
                    End If

                    If Vector3.Distance(NeededBodyRotation.Forward, ModelRotation.Forward) < 0.001 Then
                        NeededBodyRotationChanged = False
                    End If
                End If
            End If







            RotationY = MathHelper.WrapAngle(RotationVelocity.Y)
            RotationX = MathHelper.WrapAngle(RotationVelocity.X)
            RotationZ = MathHelper.WrapAngle(RotationVelocity.Z)

            ModelRotation *= Matrix.Identity
            HeadRotation *= Matrix.Identity

            If RotationY <> 0 Then
                ModelRotation *= Matrix.CreateFromAxisAngle(Vector3.Up, RotationY)
            End If
            If RotationX <> 0 Then
                ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Right, RotationX)
            End If
            If RotationZ <> 0 Then
                ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Forward, RotationZ)
            End If

            ModelRotationY *= Matrix.CreateRotationY(RotationY)

            Dim RotationYCHange = Matrix.CreateFromAxisAngle(ModelRotation.Up, RotationY)



            BodyParts(iHead).Rotation = HeadRotation
            BodyParts(iBody).Rotation = ModelRotation


            BodyParts(iLHand).RotateAsChild(ModelRotation.Up, RotationY)
            BodyParts(iRHand).RotateAsChild(ModelRotation.Up, RotationY)


            BodyParts(iLLeg).RotateAsChild(RotationYCHange)
            BodyParts(iRLeg).RotateAsChild(RotationYCHange)




            BodyParts(iHead).OriginalRotation = HeadRotation * BodyParts(iHead).DefualtRotation
            BodyParts(iBody).OriginalRotation = ModelRotation * BodyParts(iBody).DefualtRotation

            BodyParts(iRLeg).OriginalRotation = ModelRotation * BodyParts(iRLeg).DefualtRotation
            BodyParts(iLLeg).OriginalRotation = ModelRotation * BodyParts(iLLeg).DefualtRotation

            BodyParts(iRKnee).OriginalRotation = ModelRotation * BodyParts(iRKnee).DefualtRotation
            BodyParts(iLKnee).OriginalRotation = ModelRotation * BodyParts(iLKnee).DefualtRotation

            If Not IsNothing(CTool) Then
                CTool.Rotate(RotationYCHange)
                CTool.Update()
                CTool.OriginalRotation = CTool.DefualtRotation * ModelRotation
            End If







            FacingDirection = HeadRotation.Forward
            FacingDirection.Normalize()




            'Collition Checking...
            CollitionHierarchy.UpdateAllSpheres(Position)




            Dim MeWidthSq = eType.Width * eType.Width * 2
            Dim IsAttacking = (CTool IsNot Nothing) AndAlso CTool.Attacking
            For Each e In Ground.CStack.eList
                If e IsNot Me Then


                    If IsAttacking Then

                        Dim CN = e.CollitionHierarchy?.GetCollided(CTool.R, CTool.Length)
                        If Not IsNothing(CN) Then


                            Dim Dir = (e.Position - Position)
                            Dir.Y = 0
                            Dir.Normalize()
                            CTool.RewardsToVictim.Reward(e, CN.RRewardMultiplier)
                            CN.eP.Hurten(-1 * CTool.RewardsToVictim.Health)

                            '  e.Position += Dir * 10

                            Controls.Go(e, Dir, 15)
                            e.ShotHit(Me)


                        End If

                    End If
                    'Next






                    'Keeping Distance to other entities
                    Dim DistanceSquaredToE = Vector3.DistanceSquared(e.Position, Position)
                    If DistanceSquaredToE < MeWidthSq Then

                        'dir.Normalize()
                        Dim Dir = (Position - e.Position) * Physics.YZero
                        Controls.Go(Me, Dir, 0.1)

                        '   Position += (Position - e.Position) * Physics.HalfXYAndYZero
                        ' e.Position += (e.Position - Position) * Physics.HalfXYAndYZero
                    End If

                End If
            Next




            RotationY = 0
            RotationX = 0
            RotationZ = 0



            RotationVelocity.Y *= RotationVelocityReducingFactor.Y
            RotationVelocity.X *= RotationVelocityReducingFactor.X
            RotationVelocity.Z *= RotationVelocityReducingFactor.Z








            If Health < 1 Then
                Kill()
            End If

        Else
            '____________________________________________Dead_________________________________________

            RotationY = MathHelper.WrapAngle(RotationVelocity.Y)
            RotationX = MathHelper.WrapAngle(RotationVelocity.X)
            RotationZ = MathHelper.WrapAngle(RotationVelocity.Z)



            If RotationX <> 0 Then
                ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Right, RotationX)
                Dim RotationXCHange = Matrix.CreateFromAxisAngle(ModelRotation.Right, RotationX)




                BodyParts(iHead).Rotation = ModelRotation
                BodyParts(iHead).RelativePosition = Vector3.Transform(BodyParts(iHead).RelativePosition, RotationXCHange)
                BodyParts(iBody).Rotation = ModelRotation
                BodyParts(iBody).RelativePosition = Vector3.Transform(BodyParts(iBody).RelativePosition, RotationXCHange)

                'BodyParts(iLHand).RotateAsChild(ModelRotation.Right, RotationY)
                BodyParts(iLHand).Rotation = ModelRotation
                BodyParts(iLHand).RelativePosition = Vector3.Transform(BodyParts(iLHand).RelativePosition, RotationXCHange)
                BodyParts(iLLeg).Rotation = ModelRotation
                BodyParts(iLLeg).RelativePosition = Vector3.Transform(BodyParts(iLLeg).RelativePosition, RotationXCHange)


                'BodyParts(iRHand).RotateAsChild(ModelRotation.Right, RotationY)
                BodyParts(iRHand).Rotation = ModelRotation
                BodyParts(iRHand).RelativePosition = Vector3.Transform(BodyParts(iRHand).RelativePosition, RotationXCHange)
                BodyParts(iRLeg).Rotation = ModelRotation
                BodyParts(iRLeg).RelativePosition = Vector3.Transform(BodyParts(iRLeg).RelativePosition, RotationXCHange)



                BodyParts(iRKnee).Rotation = ModelRotation
                BodyParts(iRKnee).RelativePosition = Vector3.Transform(BodyParts(iRKnee).RelativePosition, RotationXCHange)
                BodyParts(iLKnee).Rotation = ModelRotation
                BodyParts(iLKnee).RelativePosition = Vector3.Transform(BodyParts(iLKnee).RelativePosition, RotationXCHange)

                If CTool IsNot Nothing Then
                    CTool.Rotation = ModelRotation
                    CTool.RelativePosition = Vector3.Transform(CTool.RelativePosition, RotationXCHange)
                End If







                RotationY = 0
                RotationX = 0
                RotationZ = 0



                RotationVelocity.Y *= 0.95F
                RotationVelocity.X *= 0.95F
                RotationVelocity.Z *= 0.95F


            End If






        End If


        Exit Sub







    End Sub












    Public Overrides Sub Draw()



        If Not eType.IsPlayer Then

            Dim AmbColor As New Vector3(0)

            For m = 0 To 7

                Dim mesh As ModelMesh = eType.Model.Meshes(m)

                Dim World = Matrix.Multiply(Matrix.Multiply(eType.Transforms, BodyParts(m).Rotation), Matrix.CreateTranslation(BodyParts(m).RelativePosition + Position))

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
                AmbColor.X = BodyParts(m).Hurt / 25
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Single'.
                BodyParts(m).Hurten(-1)



                For Each effect As BasicEffect In mesh.Effects
                    effect.AmbientLightColor = AmbColor
                    effect.Projection = projectionMatrix
                    effect.View = viewMatrix

                    effect.World = World

                Next
                mesh.Draw()

            Next

            If Not IsNothing(CTool) Then


                For Each mesh As ModelMesh In CTool.Model.Meshes

                    Dim World = CTool.Transforms(mesh.ParentBone.Index) * CTool.Rotation * Matrix.CreateTranslation(CTool.RelativePosition + Position)

                    For Each effect As BasicEffect In mesh.Effects
                        effect.Projection = projectionMatrix
                        effect.View = viewMatrix
                        effect.World = World
                    Next
                    mesh.Draw()


                Next
            End If




        End If
    End Sub









    Public Const iHead As Integer = 5
    Public Const iBody As Integer = 3

    Public Const iLHand As Integer = 1
    Public Const iRHand As Integer = 6

    Public Const iLLeg As Integer = 2
    Public Const iRLeg As Integer = 7

    Public Const iRKnee As Integer = 0
    Public Const iLKnee As Integer = 4


    Public Overrides Sub Load(eT As EntityType)
        eType = eT
        'Transforms = TTransforms


        HeadRotation = Matrix.Identity

        NeededBodyRotation = Matrix.Identity
        NeededBodyRotationChanged = True



        MovedFB = False
        WalkingFw = False



        GetingTargetBodyRotationLast = 0
        GetingTargetBodyRotationLastDir = False 'T=C  F=AC


        DelUpdateAI = eT.UpdateAI
        DelTargetScan = eT.DelTargetScan



        ' 0 - Head , 1 - LHand , 2 - LLeg , 3 - LKnee , 4 - Body , 5 - RHand , 6 - RLeg , 7 - RKnee
        ' 0 - RKnee  , 1 - LHand , 2 - LLeg , 3 - Body, 4 - LKnee, 5 - Head , 6 - RHand  ,7 - RLeg



        BodyParts = New List(Of ePart) From {
            New ePart("RightKnee", Matrix.Identity, New Vector3(11, 52, -5.7) * 0.825, Me, 0),
            New ePart("LeftHand", Matrix.Identity, New Vector3(-22, 139, 4) * 0.825, Me, 1),
            New ePart("LeftLeg", Matrix.Identity, New Vector3(-9, 98, -5.7) * 0.825, Me, 2),
            New ePart("Body", Matrix.Identity, New Vector3(0, 95, 0) * 0.825, Me, 3),
            New ePart("LeftKnee", Matrix.Identity, New Vector3(-11, 52, -5.7) * 0.825, Me, 4),
            New ePart("Head", Matrix.Identity, New Vector3(0, 151, 0.4) * 0.825, Me, 5),
            New ePart("RightHand", Matrix.Identity, New Vector3(22, 139, 4) * 0.825, Me, 6),
            New ePart("RightLeg", Matrix.Identity, New Vector3(10, 98, -5.7) * 0.825, Me, 7)
        }






        'BodyParts(iRHand).Rotaion *= Matrix.CreateFromAxisAngle(BodyParts(iRHand).Rotaion.Forward, 0.9)
        'BodyParts(iLHand).Rotaion *= Matrix.CreateFromAxisAngle(BodyParts(iRHand).Rotaion.Forward, -0.9)


        BodyParts(iRKnee).eAChildRotationGradient = 2
        BodyParts(iLKnee).eAChildRotationGradient = 2

        BodyParts(iRLeg).Children.Add(BodyParts(iRKnee))
        BodyParts(iLLeg).Children.Add(BodyParts(iLKnee))

        BodyParts(iRLeg).Length = 35
        BodyParts(iLLeg).Length = 35

        BodyParts(iRLeg).ChildDirection = "D".Chars(0)
        BodyParts(iLLeg).ChildDirection = "D".Chars(0)



        BodyParts(iRHand).Length = 50
        BodyParts(iLHand).Length = 50

        BodyParts(iRHand).ChildDirection = "D".Chars(0)
        BodyParts(iLHand).ChildDirection = "D".Chars(0)



        CollitionHierarchy = eCollitionHierarchy.CreateNewHumanHierarchy(Me)


        If eType.IsPlayer Then
            DirectCast(Me, Player).LoadPlayer()
        End If

        StartWalking()

    End Sub

    Public WalkingAnimation As ePAnimationChain
    Public FakeWA As New ePAnimation
    Public WalkingAnimationL As ePAnimation
    Public WalkingAnimationR As ePAnimation
    Public WalkingAnimationHL As ePAnimation
    Public WalkingAnimationHR As ePAnimation
#Disable Warning BC42016 ' Implicit conversion

    Public StandStraitAnimations As ePAnimationChain






    Public Sub StartWalking()

        WalkingAnimation = New ePAnimationChain
        FakeWA = New ePAnimation
        WalkingAnimationL = New ePAnimation
        WalkingAnimationR = New ePAnimation
        WalkingAnimationHL = New ePAnimation
        WalkingAnimationHR = New ePAnimation






        Dim FakeBodyPart = New ePart(Me)
        FakeWA = New ePAnimation(FakeBodyPart, New Vector3, {-0.4, 0.8}, True, {20, 20}, New Axis("L", False, FakeBodyPart)) With {
            .IsFakeTimer = True
        }

        WalkingAnimationL = New ePAnimation(BodyParts(iLLeg), New Vector3, {-0.4, 0.8}, True, {20, 20}, New Axis("L", False, BodyParts(iLLeg)), 2, True)

        WalkingAnimationR = New ePAnimation(BodyParts(iRLeg), New Vector3, {-0.8, 0.4}, True, {20, 20}, New Axis("R", False, BodyParts(iRLeg)), 2, True)

        FakeWA.Parellel_eAs.Add(WalkingAnimationR)
        FakeWA.Parellel_eAs.Add(WalkingAnimationL)


        'If BodyParts(iLHand).LockedeA = False Then
        WalkingAnimationHL = New ePAnimation(BodyParts(iLHand), New Vector3, {0.6, -0.6}, True, {20, 20}, New Axis("L", False, BodyParts(iLHand)), 0, True)
        '    BodyParts(iLHand).LockedeA = True
        'End If
        'If BodyParts(iRHand).LockedeA = False Then
        WalkingAnimationHR = New ePAnimation(BodyParts(iRHand), New Vector3, {0.6, -0.6}, True, {20, 20}, New Axis("R", False, BodyParts(iRHand)), 0, True)
        '    BodyParts(iRHand).LockedeA = True
        'End If


        FakeWA.Parellel_eAs.Add(WalkingAnimationHL)
        FakeWA.Parellel_eAs.Add(WalkingAnimationHR)



        'eARLeg.Parellel_eAs.Add(eALLeg)
        WalkingAnimation = New ePAnimationChain(New List(Of ePAnimation) From {FakeWA}, True)
        'WalkingAnimation.Reset_LockedeA_After_eA.AddRange({BodyParts(iRHand), BodyParts(iLHand)})
        WalkingAnimation.Start()
        WalkingAnimation.Pause()



        Dim eALLeg1 As New ePAnimation(BodyParts(iLLeg), New Vector3, {(WalkingAnimationL.CAngle) * -1}, False, {15}, New Axis("L", False, BodyParts(iLLeg)))
        Dim eARLeg2 As New ePAnimation(BodyParts(iRLeg), New Vector3, {(WalkingAnimationR.CAngle) * -1}, False, {15}, New Axis("R", False, BodyParts(iRLeg)))

        eARLeg2.Parellel_eAs.Add(eALLeg1)
        'StandStraitAnimations.Start()
        StandStraitAnimations = New ePAnimationChain(New List(Of ePAnimation) From {eARLeg2}, False) With {
            .Name = "Begining game1",
            .Done = True
        }

    End Sub
#Enable Warning BC42016 ' Implicit conversion

    Public Sub ResumeWalking()
        If WalkingFw = False Then

            If StandStraitAnimations.Done Then
                WalkingAnimation.RResume()
                WalkingFw = True
            Else
                WalkingFw = False
            End If
        End If
    End Sub

    Public Sub PuaseWalking()
        If WalkingFw Then
            If StandStraitAnimations.Done Then
                WalkingAnimation.Pause()
                WalkingFw = False
                StandStrait()
            End If
        End If
    End Sub




#Disable Warning BC42016 ' Implicit conversion
    Public Sub StandStrait()

        StandStraitAnimations = Nothing

        Dim FakeBP As New ePart(Me)

        Dim eAFBP As New ePAnimation(FakeBP, New Vector3, {(WalkingAnimationL.CAngle) * -1}, False, {11}, New Axis("L", False, FakeBP))

        Dim eALLeg As New ePAnimation(BodyParts(iLLeg), New Vector3, {(WalkingAnimationL.CAngle) * -1}, False, {11}, New Axis("L", False, BodyParts(iLLeg)))
        Dim eARLeg As New ePAnimation(BodyParts(iRLeg), New Vector3, {(WalkingAnimationR.CAngle) * -1}, False, {11}, New Axis("R", False, BodyParts(iRLeg)))


        eAFBP.Parellel_eAs.Add(eALLeg)
        eAFBP.Parellel_eAs.Add(eARLeg)



        StandStraitAnimations = New ePAnimationChain(New List(Of ePAnimation) From {eAFBP}, False) With {
            .Done = False,
            .Reset_eAC_After_eA = WalkingAnimation
        }



        If BodyParts(iLHand).LockedeA = False Then
            Dim eALHand = New ePAnimation(BodyParts(iLHand), New Vector3, {(WalkingAnimationHL.CAngle) * -1}, False, {11}, New Axis("L", False, BodyParts(iLHand)))
            eAFBP.Parellel_eAs.Add(eALHand)
            'BodyParts(iLHand).LockedeA = True
            'StandStraitAnimations.Reset_LockedeA_After_eA.Add(BodyParts(iLHand))
            StandStraitAnimations.Revert_After_eA.Add(BodyParts(iLHand))

        End If

        If BodyParts(iRHand).LockedeA = False Then
            Dim eARHand = New ePAnimation(BodyParts(iRHand), New Vector3, {(WalkingAnimationHR.CAngle) * -1}, False, {11}, New Axis("R", False, BodyParts(iRHand)))
            eAFBP.Parellel_eAs.Add(eARHand)
            'BodyParts(iRHand).LockedeA = True
            'StandStraitAnimations.Reset_LockedeA_After_eA.Add(BodyParts(iRHand))
            StandStraitAnimations.Revert_After_eA.Add(BodyParts(iRHand))
        End If


        'StandStraitAnimations.Reset_LockedeA_After_eA.AddRange({BodyParts(iRHand), BodyParts(iLHand)})

        StandStraitAnimations.Revert_After_eA.AddRange({BodyParts(iLLeg), BodyParts(iRLeg), BodyParts(iLKnee), BodyParts(iRKnee)})

        'StandStraitAnimations.Revert_After_eA.AddRange({BodyParts(iLKnee), BodyParts(iRKnee)})

        '  WalkingAnimation.Reset()

        StandStraitAnimations.Start()


        WalkingFw = False

    End Sub

    Public Sub StandStraitInstantly()


        StandStraitAnimations.Reset()
        StandStraitAnimations.Done = True

        WalkingFw = False

        WalkingAnimationR.Reset()
        BodyParts(iRLeg).Revert()

        WalkingAnimationL.Reset()
        BodyParts(iLLeg).Revert()


        BodyParts(iRKnee).Revert()

        BodyParts(iLKnee).Revert()


        WalkingAnimationHL.Reset()
        BodyParts(iLHand).Revert()

        WalkingAnimationHR.Reset()
        BodyParts(iRHand).Revert()

        ResumeWalking()



    End Sub




#Enable Warning BC42016 ' Implicit conversion

    Public Sub New(eT As EntityType)
        MyBase.New(eT)

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
        LICode += 1
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Short'.
        ICode = LICode


        RotationX = 0
        RotationY = 0
        RotationZ = 0



        CurrentBlock = New DBlock(New Block)



    End Sub


    Public Sub New()
        MyBase.New

        ICode = CShort(LICode + 1)
        LICode += CShort(1)

        RotationX = 0
        RotationY = 0
        RotationZ = 0

        CurrentBlock = New DBlock(New Block)




    End Sub


    Public Overrides Sub Kill()
        Health = -1

        IsDead = True


        'eCollitionHierarchy.eCollitionHierarchies.Remove(CollitionHierarchy)
        Dim n = 0
        While n < ePAnimation.LstAnimation.Count
            If ePAnimation.LstAnimation(n).eP.Parent = Me Then
                ePAnimation.LstAnimation(n).Reset()
            End If
            n += 1
        End While

        WalkingAnimation.Reset()
        StandStraitAnimations.Reset()


        RotationVelocity.X = 0.08

        Position += New Vector3(0, 10, 0)



        On Error Resume Next
        For n = 0 To Friendlies.Count - 1
            Friendlies(n).Friendlies.Remove(Me)
        Next

        For n = 0 To Enemies.Count - 1
            Enemies(n).Enemies.Remove(Me)
        Next

        'Game1.DeadEntities.Add(Me)

    End Sub


    ''' <summary>
    ''' Place: -1 = LHand, 1 = RHand, 0 = Inventory
    ''' </summary>
    ''' <param name="T"></param>
    ''' <param name="Place"></param>
    Public Overrides Sub GiveTool(T As Tool, Place As Integer)
        Dim Sword1 = T
        With Sword1
            .Rotation = T.OriginalRotation
            .Parent = Me
            .Charter = Me
            .ChildRotationGradient = 0
        End With
        CTool = Sword1


        If Place = -1 Then
            Sword1.Owner = BodyParts(iLHand)
            Sword1.RelativePositionFromParent = New Vector3(-0.15, 0.13, -0.25)
            Sword1.Side = -1
        End If
        If Place = 1 Then
            Sword1.Owner = BodyParts(iRHand)
            Sword1.RelativePositionFromParent = New Vector3(0.1, 0.1, -0.15)
            Sword1.Side = 1
        End If


        Sword1.Owner.Children.Add(Sword1)

    End Sub















    Public Shared Function AddNewHuman(eT As EntityType, NName As String, PPosition As Vector3, CCCTool As Tool, CCCToolSide As Integer) As Human
        Dim H1 As New Human(eT)
        H1.Load(eT)
        H1.Name = NName
        H1.eType.IsHuman = True
        H1.Position = PPosition




        If CCCTool IsNot Nothing Then H1.GiveTool(CCCTool, CCCToolSide)

        Return H1

    End Function



    Public Sub AddToTheCurrentStack()
        Ground.CStack.eList.Add(Me)
    End Sub


End Class



