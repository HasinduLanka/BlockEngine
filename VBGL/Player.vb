


Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports Microsoft.Xna.Framework.Input

Public Class Player : Inherits Human



    Public CamPosType As RacingCameraAngle = RacingCameraAngle.Inside

    Public ControlsList As New List(Of Controls.Control)

    Public InsiderViewBodyPartes As New List(Of ePart)

    Public InsiderHandXRotation As Single = 0
    Public InsiderHandXCurrRotation As Single = 0

    Public ArrowLastTime As Long
    Public PlaceBlockLastTime As Long
    Public SelectedBlockType As BlockType

    Public ManMovedOnce As Boolean = False

    Public Money As Integer = 500


    Public Sub UpdateMan(controllerState As KeyboardState, MouseState As MouseState)


        If Not IsDead Then


            FacingDirection = HeadRotation.Forward




            For Each C As Controls.Control In ControlsList
                Dim A As Actions = C.Action

                If C.IsKeyControl Then


                    Dim K As Keys = C.Key
                    If controllerState.IsKeyDown(K) Then

                        Controls.Go(Me, A)

                        Act(A)

                    End If



                Else


                    Dim MouseKey As Controls.Control.MouseKeys = C.MouseControl


                    Select Case MouseKey

                        Case Controls.Control.MouseKeys.LeftClick
                            If MouseState.LeftButton = ButtonState.Pressed Then
                                Act(A)
                            End If

                        Case Controls.Control.MouseKeys.RightClick
                            If MouseState.RightButton = ButtonState.Pressed Then
                                Act(A)
                            End If

                        Case Controls.Control.MouseKeys.WheelPress
                            If MouseState.MiddleButton = ButtonState.Pressed Then
                                Act(A)
                            End If

                        Case Controls.Control.MouseKeys.WheelUp
                            If MouseState.ScrollWheelValue > MouseWheelValue Then
                                Act(A)
                                MouseWheelValue = MouseState.ScrollWheelValue
                            End If

                        Case Controls.Control.MouseKeys.WheelDown
                            If MouseState.ScrollWheelValue < MouseWheelValue Then
                                Act(A)
                                MouseWheelValue = MouseState.ScrollWheelValue
                            End If


                    End Select



                End If



            Next



        End If


        If MovedFB Then
            ManMovedOnce = True
        Else
            If ManMovedOnce Then
                Dim afgfgfgf = Me
            End If
        End If


    End Sub

#Enable Warning BC42016 ' Implicit conversion



    Public Sub Act(A As Actions)

        Select Case A
            Case Actions.Attack
                ActAttack()

            Case Actions.PlaceBlock
                ActPlaceBlock()

            Case Actions.BreakBlock
                ActBreakBlock()

            Case Actions.ChangeBlock
                ActChangeBlockType()


            Case Actions.C1
                ActC1()

            Case Actions.C2
                ActC2()

            Case Actions.C3
                ActC3()

            Case Actions.Interact
                ActInteract()

        End Select


    End Sub




    Public Sub ActInteract()


        If Money >= 100 Then


            Dim FacingE = FacingEntity(1000)

            If Not IsNothing(FacingE) Then
                If Not FacingE.ChekRelation(Me) = EntityRelationMode.Friends Then

                    Money -= 100
                    FacingE.Hire(Me)

                End If

            End If
        End If

    End Sub





    Public Sub ActAttack()


        If LastActionTime + 500 < NowGameTime Then
            If CTool.Owner.LockedeA = False Then


                For Each eA In CTool.Owner.CurrentAnimations
                    eA.Reset()
                Next
                CTool.Owner.Revert()

                CTool.Use(
RandomOf({Tool.Action.PrimaryAttack1, Tool.Action.PrimaryAttack2, Tool.Action.ShortAttack}))
                'CTool.Use(RandomOf({Tool.Action.ShortAttack}))


                LastActionTime = NowGameTime

            End If
        End If

    End Sub




    Public Sub ActC1()

        For Each e In Friendlies
            If Not e.IsDead Then
                e.LockTarget(Player1, AITargetMode.Follow,)
            End If
        Next



    End Sub

    Public Sub ActC2()
        Dim FacingE = FacingEntity(500)

        If Not IsNothing(FacingE) Then

            For Each FE In Friendlies
                FE.LockTarget(FacingE, AITargetMode.FollowAndKill)
            Next
        End If
    End Sub


    Public Sub ActC3()

        Dim FacingE = FacingEntity(500)

        If Not IsNothing(FacingE) Then

            If FacingE.Friendlies.Contains(Me) Then
                FacingE.Kill()
                Player1.Health = 1000
                ActArrowThrow()
            End If
        End If


    End Sub


    Public Sub ActArrowThrow()
        If ArrowLastTime + 100 < NowGameTime Then
            Dim Ar1 As New Arrow(EntityTypes.Arrow1) With {
                .Position = Me.Position + Height
            }

            Ar1.ModelRotation = CreateLookAtPosition(Ar1.Position, Ar1.Position + FacingDirection, HeadRotation.Up)

            Ar1.Velocity = HeadRotation.Forward * 5


            Ar1.CollitionHierarchy = eCollitionHierarchy.CreateNewHierarchyForArrow(Ar1)

            Ar1.AddToTheCurrentStack()




            ArrowLastTime = NowGameTime
        End If
    End Sub


    Public Sub ActPlaceBlock()

        If PlaceBlockLastTime + 250 < NowGameTime Then

            Dim LookingBlock = Ground.GetBeforeBlockInTheDirection(Position + Height, HeadRotation.Forward, 1000, False)
            '+ (FacingDirection * -20)
            If Not IsNothing(LookingBlock) Then


                Ground.SetBlock(LookingBlock, SelectedBlockType)


                PlaceBlockLastTime = NowGameTime

            End If



        End If


    End Sub

    Public Sub ActBreakBlock()

        If PlaceBlockLastTime + 250 < NowGameTime Then


            Dim LookingBlock = Ground.GetBlockInTheDirection(Position + Height, HeadRotation.Forward, 1000, False)

            If Not IsNothing(LookingBlock) Then
                If LookingBlock.Chunk.Index.Y > 0 Then

                    Ground.BreakBlock(LookingBlock)

                    PlaceBlockLastTime = NowGameTime

                End If
            End If

        End If


    End Sub

    Public Sub ActChangeBlockType()

        If PlaceBlockLastTime + 250 < NowGameTime Then

            Dim SBTID = SelectedBlockType.ID + 1

            If SBTID > BlockType.BTCount Then
                SBTID = 0
                SelectedBlockType = BlockType.PlaceHolder
            Else
                SelectedBlockType = BlockType.BTList(SBTID)

            End If



            PlaceBlockLastTime = NowGameTime

            Board.SelectedToolAndBlock = "Tool:" & CTool.Name & "; Block:" & SelectedBlockType.Name

        End If




    End Sub





    Public Sub DrawMan()




        Dim BPRHand = BodyParts(iRHand)

        BPRHand.OriginalRotation = ModelRotationY * Matrix.CreateFromAxisAngle(ModelRotationY.Left, InsiderHandXRotation - MathHelper.PiOver4)
        BPRHand.Rotation *= Matrix.CreateFromAxisAngle(ModelRotation.Left, InsiderHandXCurrRotation)

        Dim BPLHand = BodyParts(iLHand)
        BPLHand.OriginalRotation = ModelRotationY
        'BPLHand.Rotation *= Matrix.CreateFromAxisAngle(ModelRotation.Left, InsiderHandXCurrRotation)

        InsiderHandXCurrRotation = 0



        If CamPosType = RacingCameraAngle.Back Then






            'Draw the model, a model can have multiple meshes, so loop
            Dim m = 0
            For Each mesh As ModelMesh In eType.Model.Meshes
                'This is where the mesh orientation is set
                Dim BP = BodyParts(m)
                For Each effect As BasicEffect In mesh.Effects
                    effect.AmbientLightColor = New Vector3(CSng(BP.Hurt / 25), 0, 0)

                    effect.Projection = projectionMatrix
                    effect.View = viewMatrix

                    effect.World = Matrix.Multiply(Matrix.Multiply(eType.Transforms, BodyParts(m).Rotation), Matrix.CreateTranslation(BodyParts(m).RelativePosition + Position))

                    'BodyParts(m).Rotation.Backward.Normalize()
                    'BodyParts(m).Rotation.Up.Normalize()
                    'BodyParts(m).Rotation.Right.Normalize()

                    'BodyParts(m).Rotation.Forward.Normalize()
                    'BodyParts(m).Rotation.Down.Normalize()
                    'BodyParts(m).Rotation.Left.Normalize()
                Next
                'Draw the mesh, will use the effects set above.
                mesh.Draw()
                m += 1
            Next


            If Not IsNothing(CTool) Then

                Dim b = 0
                For Each mesh As ModelMesh In CTool.Model.Meshes
                    For Each effect As BasicEffect In mesh.Effects
                        effect.Projection = projectionMatrix
                        effect.View = viewMatrix
                        Dim MeshTransform = CTool.Rotation * Matrix.CreateTranslation(CTool.RelativePosition + Position)
                        effect.World = CTool.Transforms(mesh.ParentBone.Index) * MeshTransform
                    Next
                    mesh.Draw()
                    b += 1
                Next
            End If



            '_________________________________________________________________________________

        ElseIf CamPosType = RacingCameraAngle.Inside Then



            'Draw the model, a model can have multiple meshes, so loop


            'For Each eP In InsiderViewBodyPartes


            '____________RHand




            'Dim RHRot As New Matrix

            'RHRot = BPRHand.Rotaion



            Dim meshRHand = eType.Model.Meshes(iRHand)


            For Each effect As BasicEffect In meshRHand.Effects

                effect.AmbientLightColor = New Vector3(CSng(BPRHand.Hurt / 25), 0, 0)
                effect.Projection = projectionMatrix
                effect.View = viewMatrix
                'Transforms(meshRHand.ParentBone.Index)
                effect.World = eType.Transforms * BPRHand.Rotation * Matrix.CreateTranslation(BPRHand.RelativePosition + Position)

            Next


            meshRHand.Draw()






            Dim meshLHand = eType.Model.Meshes(iLHand)


            For Each effect As BasicEffect In meshLHand.Effects

                effect.AmbientLightColor = New Vector3(CSng(BPLHand.Hurt / 25), 0, 0)
                effect.Projection = projectionMatrix
                effect.View = viewMatrix
                'Transforms(meshLHand.ParentBone.Index)
                effect.World = eType.Transforms * BPLHand.Rotation * Matrix.CreateTranslation(BPLHand.RelativePosition + Position)

            Next


            meshLHand.Draw()




            If Not IsNothing(CTool) Then

                Dim b = 0
                For Each meshC As ModelMesh In CTool.Model.Meshes
                    For Each effect As BasicEffect In meshC.Effects
                        effect.Projection = projectionMatrix
                        effect.View = viewMatrix
                        Dim MeshTransform = CTool.Rotation * Matrix.CreateTranslation(CTool.RelativePosition + Position)
                        effect.World = CTool.Transforms(meshC.ParentBone.Index) * MeshTransform
                    Next
                    meshC.Draw()
                    b += 1
                Next
            End If







        End If



        For Each BP In BodyParts
            BP.Hurten(-1)
        Next



    End Sub




    Public Sub LoadPlayer()
        ''InsiderViewBodyPartes.AddRange({BodyParts(iLHand), BodyParts(iRHand)})
        BodyParts(iRHand).Rotation *= Matrix.CreateFromAxisAngle(ModelRotation.Left, -MathHelper.PiOver4)
        BodyParts(iRHand).OriginalRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Left, -MathHelper.PiOver4)
    End Sub










    Public Sub New(eT As EntityType)
        MyBase.New(eT)
    End Sub


    Public Sub New()
        MyBase.New
    End Sub

End Class

