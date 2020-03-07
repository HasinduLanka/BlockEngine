Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input

Public Class Controls




    Public Const KeyUnAssigned As Keys = Keys.Pa1
    Public Class Control

        Public Key As Keys
        Public MouseControl As MouseKeys

        Public IsKeyControl As Boolean = True

        Public Property Action As Actions



        Public Sub New(KeyOfAction As Keys, RelatedAction As Actions)
            Key = KeyOfAction
            Action = RelatedAction
        End Sub

        Public Sub New(MouseKey As MouseKeys, RelatedAction As Actions)
            MouseControl = MouseKey
            IsKeyControl = False

            Action = RelatedAction
        End Sub



        Public Enum MouseKeys

            LeftClick
            RightClick

            WheelUp
            WheelDown

            WheelPress

        End Enum

    End Class


    Public Shared Function NewControlList(Forward As Keys, Backward As Keys, Left As Keys, Right As Keys, Up As Keys, Down As Keys) As List(Of Controls.Control)
        Dim Out As New List(Of Controls.Control) From {
            New Control(Forward, Actions.Forward),
            New Control(Backward, Actions.Backward),
            New Control(Left, Actions.Left),
            New Control(Right, Actions.Right),
            New Control(Up, Actions.Up)
        }

        Return Out
    End Function







    Public Shared V3_0X As New Vector3(0, 1, 1)
    Public Shared V3_0Y As New Vector3(1, 0, 1)
    Public Shared V3_0Z As New Vector3(1, 1, 0)

    Public Shared V3_1X As New Vector3(1, 0, 0)
    Public Shared V3_1Y As New Vector3(0, 1, 0)
    Public Shared V3_1Z As New Vector3(0, 0, 1)

	''' <summary>
	''' Returns IsPathBlocked
	''' </summary>
	Public Shared Function Go(E As Entity, A As Actions) As Boolean
		Dim IsPathBlocked = False

		Select Case A

			Case Actions.Forward
				Return Go(E, E.ModelRotationY.Forward, E.Accelaration.Z)

			Case Actions.Backward
				Return Go(E, E.ModelRotationY.Backward, E.Accelaration.X)

			Case Actions.Right
				Return Go(E, E.ModelRotationY.Right, E.Accelaration.X)

			Case Actions.Left
				Return Go(E, E.ModelRotationY.Left, E.Accelaration.X)

			Case Actions.Up
				'  If Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Up) + Vector3.Up).IsAir = True Then
				If E.OnGround Then
					E.Velocity += 50 * E.ModelRotation.Up
					E.FallingSpeed = 0
				End If

			Case Actions.Up2
				'  If Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Up) + Vector3.Up).IsAir = True Then
				If E.OnGround Then
					E.Velocity += 28 * E.ModelRotation.Up
					E.FallingSpeed = 0
				End If

			Case Actions.RotateClockwiseY
				'   E.RotationVelocity.Y -= E.RotationAccelaration.Y

				Dim Rot = Matrix.CreateFromAxisAngle(Vector3.Up, -0.1F)
				E.HeadRotation *= Rot
				E.NeededBodyRotation *= Rot
				E.NeededBodyRotationChanged = True
			Case Actions.RotateAntiClockwiseY
				'E.RotationVelocity.Y += E.RotationAccelaration.Y
				'LookAtEntity(E, New Entity(EntityTypes.WayPoint) With {.Position = E.Position + E.HeadRotation.Forward + New Vector3(-0.1, 0, 0)})

				Dim Rot = Matrix.CreateFromAxisAngle(Vector3.Up, 0.1F)
				E.HeadRotation *= Rot
				E.NeededBodyRotation *= Rot
				E.NeededBodyRotationChanged = True
		End Select


		Return IsPathBlocked

	End Function





	''' <summary>
	''' Returns IsPathBlocked
	''' </summary>
	Public Shared Function Go(E As Entity, Dir As Vector3, Accelaration As Single) As Boolean
        Dim IsPathBlocked As Boolean = False

        Dim FBlocks = Ground.GetFacingIsAir(E.Position, Physics.Find2DDualDirectionsOfDirection(Dir), Dir)
        Dim MonoFacingDirection As Direction = Physics.Find2DUnitDirectionOfDirection(Dir)


        If IsNothing(FBlocks) Then

            Return True
            Exit Function
        End If


        If FBlocks(2) Then
            If FBlocks(3) Then
                'Both Upper Ways NotBlocked


                If FBlocks(0) Then
                    If FBlocks(1) Then
                        'Way Clear

                        If FBlocks(4) Then
                            E.Velocity += Accelaration * Dir
                            E.MovedFB = True

                        Else 'XZ blocked
                            If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

                                E.Velocity += Accelaration * Dir * Vector3.Right
                                E.MovedFB = True

                            Else
                                E.Velocity += Accelaration * Dir * Vector3.Backward
                                E.MovedFB = True

                            End If

                        End If




                    Else

                        'X Unblocked. Z Blocked


                        If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

                            E.Velocity += Accelaration * Dir * Vector3.Right
                            E.MovedFB = True


                        ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then
                            Go(E, Actions.Up2)
                            E.MovedFB = True
                        End If

                    End If

                Else
                    'X Blocked

                    If FBlocks(1) Then
                        'X Blocked. Z Unlocked


                        If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then
                            Go(E, Actions.Up2)
                            E.MovedFB = True

                        ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then

                            E.Velocity += Accelaration * Dir * Vector3.Backward
                            E.MovedFB = True

                        End If

                    Else
                        'Both lower ways blocked

                        Go(E, Actions.Up2)
                        E.MovedFB = True

                    End If

                End If






            Else

                'Z Up is Blocked. X Up is NotBlocked
                'Sliding on wall
                If FBlocks(2) Then
                    If FBlocks(0) Then
                        E.Velocity += Accelaration * Dir * Vector3.Right
                        E.MovedFB = True
                        IsPathBlocked = True
                    Else
                        'Lower unblocked. Upper Blocked
                        Go(E, Actions.Up2)
                        E.MovedFB = True
                    End If
                End If





            End If

        Else
            'X up Blocked

            If FBlocks(3) Then

                'Z up NotBlocked, X up Blocked
                'Sliding on wall


                If FBlocks(3) Then
                    If FBlocks(1) Then
                        E.Velocity += Accelaration * Dir * Vector3.Backward
                        E.MovedFB = True
                        IsPathBlocked = True

                    Else
                        'Lower unblocked. Upper Blocked
                        Go(E, Actions.Up2)
                        E.MovedFB = True
                    End If
                End If



            End If

        End If

        Return IsPathBlocked
    End Function



End Class






Public Enum Actions
    Null

    Forward
    Backward

    Left
    Right

    Up
    Up2

    RotateClockwiseY
    RotateAntiClockwiseY



    Attack

    C1
    C2
    C3
    C4
    C5
    C6

    PlaceBlock
    BreakBlock
    ChangeBlock


    Interact



End Enum