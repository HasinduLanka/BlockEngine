



'Public Sub DrawGround()


'    'Dim GThr As New Threading.Thread(AddressOf DrawMesh)
'    'GThr.Start(BT.Mesh)

'End Sub


'Public Sub DrawMesh(M As ModelMesh)
'    M.Draw()
'End Sub





'    Public Class ePAnimation
'        Public Shared ss.LstAnimation As New List(Of ePAnimation)
'        'Public Shared LstAddingAnimation As New List(Of ePAnimation)
'        'Public Shared LstRemovingAnimation As New List(Of ePAnimation)

'        Public eP As ePart
'        Public DPos As Vector3 'Destination Position
'        Public DRot As Matrix 'Destination Rotation
'        Public Time As Single = 0.04
'        Public Speed As Single
'        Public Axis As Vector3

'        Public GetingTargetLstSng As Single = 0
'        Public GetingTargetLstDir As Boolean = False 'T=CW  F=ACW

'        Public ePAnimationChain1 As ePAnimationChain
'        Public Parellel_eAs As New List(Of ePAnimation)

'        Public Sub Animate()
'#Disable Warning BC42016 ' Implicit conversion


'            If Vector3.Distance(DRot.Forward, eP.Rotaion.Forward) < 0.05 Then
'                eP.Rotaion = DRot
'                'Remove
'                ss.LstAnimation.Remove(Me)
'                ' eP.CurrentAnimations.Remove(Me)
'                If Not IsNothing(ePAnimationChain1) Then
'                    ePAnimationChain1.Next_eA()
'                End If

'            Else

'                If Vector3.Distance(DRot.Forward, eP.Rotaion.Forward) > GetingTargetLstSng Then
'                    If GetingTargetLstDir = True Then 'CW
'                        GetingTargetLstDir = False 'ACW

'                    Else 'ACW
'                        GetingTargetLstDir = True 'CW

'                    End If
'                End If

'                GetingTargetLstSng = Vector3.Distance(DRot.Forward, eP.Rotaion.Forward)

'                If GetingTargetLstDir = False Then
'                    'Anti ClockWise
'                    eP.RotateWithoutOptimizingAnimations(Axis, Speed)

'                Else
'                    'ClockWise
'                    eP.RotateWithoutOptimizingAnimations(Axis, -Speed)


'                End If

'            End If



'#Enable Warning BC42016 ' Implicit conversion
'        End Sub

'        Public Sub Pause()

'            ss.LstAnimation.Remove(Me)

'        End Sub

'        Public Sub RResume()
'            ss.LstAnimation.Add(Me)
'        End Sub


'        Public Sub Start()

'            If eP.Rotaion.Forward <> DRot.Forward Then

'                Speed = Vector3.Distance(DRot.Forward, eP.Rotaion.Forward) / Time

'                ss.LstAnimation.Add(Me)

'            Else
'                If Not IsNothing(ePAnimationChain1) Then
'                    ePAnimationChain1.Next_eA()
'                End If
'            End If

'        End Sub

'        Public Sub Destroy()
'            ss.LstAnimation.Remove(Me)
'            eP.CurrentAnimations.Remove(Me)
'        End Sub

'        Public Sub New()
'        End Sub

'        Public Sub New(eeP As ePart, DDPos As Vector3, DDRot As Matrix, TTime As Single, AAxis As Vector3)
'            eP = eeP
'            DPos = DDPos
'            DRot = DDRot
'            Time = TTime
'            Axis = AAxis
'            eP.CurrentAnimations.Add(Me)
'        End Sub


'    End Class












'    Public Sub Update2(controllerState As KeyboardState)
'        FacingDirection = HeadRotation.Forward



'        If NoTarget = False Then
'            If IsTargetShip = True Then
'                Controls.GetTarget(Me, TargetS)
'            ElseIf IsTargetShip = False Then
'                Controls.GetTarget(Me, Target)
'            End If
'        End If


'        Position += ModelVelocity
'        ModelVelocity *= ModelVelocityReducingFactor



'        OnGround = False

'        CurrentBlock = Ground.GetBlock(Position - New Vector3(0, Ground.BlockSize, 0))
'        CurrentChunk = CurrentBlock.Chunk

'        If CurrentBlock.IsAir = False Then

'            Position.Y = CurrentBlock.RealPosition.Y + Ground.BlockSize
'            OnGround = True

'        End If



'        If OnGround = False Then
'            Position.Y -= (Ground.Gravity * Weight).Y

'        End If


'#Region "Keys"

'        Dim CurActionLst As New List(Of Actions)
'        For Each C As Controls.Control In ControlsList
'            Dim K As Keys = C.Key
'            Dim A As Actions = C.Action

'            If controllerState.IsKeyDown(K) Then
'                Controls.Go(Me, A)


'                If A = Actions.C1 Then
'                    If GotTargetTime < Game1.NowGameTime - 500 Then

'                        If NoTarget = True Then

'                            Dim TG As New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z)
'                            Dim TGDirection As Vector3 = FacingDirection
'                            Dim TGFoundTarget As Boolean = False


'                            '1 check & 2 check & 3 check
'                            For CheckID As Integer = 1 To 3


'                                For i As Single = 0 To 500 '500x10=5000 (render distance)
'                                    TG += TGDirection * 10
'                                    Dim r As Single

'                                    If CheckID = 1 Then
'                                        ' For r As Single = 5 To 20 '5 to 20 -> 50 to 200
'                                        r = i * 3
'                                        If r < 50 Then
'                                            r = 50
'                                        End If
'                                    ElseIf CheckID = 2 Then
'                                        ' Distance things
'                                        r = i * 4
'                                        If r < 100 Then
'                                            r = 20
'                                        End If
'                                    ElseIf CheckID = 3 Then
'                                        ' Very closer things
'                                        r = i * 10
'                                        If r < 60 Then
'                                            r = 100
'                                        End If

'                                    End If

'                                    'If r > 500 Then
'                                    '    r = 500
'                                    'End If

'                                    Dim BS As New BoundingSphere(TG, r)


'                                    'Ships 
'                                    Dim PVTn2 As Integer = 0
'                                    For Each S As Player In Game1.Ships

'                                        If Not S.Name = Me.Name Then

'                                            Dim SBS As BoundingSphere = BS
'                                            'If SBS.Radius > 300 Then
'                                            '    SBS.Radius = 300
'                                            'End If


'                                            If SBS.Intersects(S.MBoundingSphere) Then
'                                                NoTarget = False
'                                                IsTargetShip = True
'                                                TargetS = S
'                                                TGFoundTarget = True
'                                                Exit For
'                                            End If
'                                        End If


'                                    Next


'                                    'Obstacleoids
'                                    For Each M As Obstacle In Game1.Obst
'                                        If BS.Intersects(M.MBoundingSphere) Then
'                                            NoTarget = False
'                                            IsTargetShip = False
'                                            Target = M
'                                            TGFoundTarget = True
'                                            Exit For
'                                        End If
'                                    Next









'                                    'Next

'                                    If TGFoundTarget = True Then
'                                        Exit For
'                                    End If
'                                Next
'                            Next

'                            If NoTarget = False Then
'                                If IsTargetShip = True Then
'                                    Controls.GetTarget(Me, TargetS)
'                                ElseIf IsTargetShip = False Then
'                                    Controls.GetTarget(Me, Target)
'                                End If
'                            End If


'                        Else
'                            NoTarget = True
'                        End If
'                        GotTargetTime = CSng(Game1.NowGameTime)
'                    End If
'                End If

'                If A = Actions.C2 Then
'                    'Dim B As New Block

'                    'B.Model = BlockType.Sand.Model
'                    'B.Transforms = BlockType.Sand.Transforms

'                    'B.Position = Position

'                    ' Ground.BlockLis.Add(B)

'                    MsgBox(Physics.FindStandedVec3OfDirection(ModelRotation.Forward).ToString)

'                    '  Position = New Vector3(10, 100, 10)



'                End If


'                If A = Actions.C3 Then

'                    'man rotation
'                    If NoTarget = False Then
'                        If IsTargetShip = True Then
'                            Controls.GetTarget(Me, TargetS)
'                        ElseIf IsTargetShip = False Then
'                            Controls.GetTarget(Me, Target)
'                        End If
'                    End If


'                    'Fire''''''''''''''
'                    If Game1.NowGameTime - LastBulletTime > BulletFrequency Then


'                        Dim DBullet As Integer



'                        While Controls.Bullets.Count > BulletMagazineSize
'                            Dim CHn = 0
'                            For Each B1 As Bullet In Controls.Bullets
'                                If B1.Owner.Equals(Me) Then
'                                    Exit For
'                                    DBullet = CHn
'                                End If
'                                CHn += 1
'                            Next


'                            Controls.Bullets.RemoveAt(DBullet)


'                        End While








'                        Dim B As New Bullet

'                        B.ModelVelocity = BulletVelocity


'                        B.Position = Me.Position ' + (New Vector3(X, Y, Z) * 130)


'                        If NoTarget = True Then

'                            B.Direction = ModelRotation.Forward


'                        Else
'                            Dim R As Vector3
'                            If IsTargetShip = True Then
'                                R = TargetS.Position - Me.Position
'                            ElseIf IsTargetShip = False Then
'                                R = Target.Position - Me.Position
'                            End If

'                            While R.X > 1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While
'                            While R.Y > 1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While
'                            While R.Z > 1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While

'                            While R.X < -1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While
'                            While R.Y < -1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While
'                            While R.Z < -1
'                                R.X /= 2
'                                R.Y /= 2
'                                R.Z /= 2
'                            End While



'                            B.Direction = R




'                        End If


'                        B.Owner = Me


'                        B.Model = Game1.BulletModel.Model
'                        B.Transforms = Game1.BulletModel.Transforms
'                        B.Transforms2 = Game1.BulletModel.Transforms2

'                        'If RX >= 90 And RX <= 270 Then
'                        'Else

'                        LastBulletTime = Game1.NowGameTime
'                        Controls.Bullets.Add(B)
'                        'End If

'                        'If controllerState.IsKeyDown(Keys.P) Then
'                        '    Dim PauseNow = 1
'                        'End If

'                    End If
'                End If

'                If A = Actions.C4 Then
'                    If GotTargetTime < Game1.NowGameTime - 200 Then
'                        If Me.CamPosType = RacingCameraAngle.Back Then
'                            Me.CamPosType = RacingCameraAngle.Inside
'                        ElseIf Me.CamPosType = RacingCameraAngle.Inside Then
'                            Me.CamPosType = RacingCameraAngle.Back
'                        End If
'                        GotTargetTime = CSng(Game1.NowGameTime)
'                    End If

'                End If
'                If A = Actions.C5 Then
'                    'Select the last hited object
'                    Try
'                        If IsLastHitS = True Then
'                            TargetS = LastHitS
'                            Controls.GetTarget(Me, TargetS)
'                        Else
'                            Target = LastHitM
'                            Controls.GetTarget(Me, Target)
'                        End If
'                    Catch ex As Exception

'                    End Try
'                End If

'            End If
'        Next







'        If (controllerState.IsKeyDown(Keys.F11)) Then
'            MsgBox("  X " + Position.X.ToString + "  Y " + Position.Y.ToString + "  Z " + Position.Y.ToString)
'            MsgBox(RotationY)
'        End If



'#End Region








'        'If ModelRotation.Forward <> NeededBodyRotation.Forward Then
'        '    ModelRotation += (NeededBodyRotation - ModelRotation) * 0.1
'        'End If



'        'If Math.Abs(ModelVelocity.X) < 0.00001 Then
'        '    ModelVelocity.X = 0
'        'End If


'        'If Math.Abs(ModelVelocity.Y) < 0.00001 Then
'        '    ModelVelocity.Y = 0
'        'End If

'        'If Math.Abs(ModelVelocity.Z) < 0.00001 Then
'        '    ModelVelocity.Z = 0
'        'End If




'        RotationY = RotationVelocity.Y
'        RotationX = RotationVelocity.X
'        RotationZ = RotationVelocity.Z


'        While RotationY >= MathHelper.TwoPi
'            RotationY -= MathHelper.TwoPi
'        End While
'        While RotationY < 0
'            RotationY += MathHelper.TwoPi
'        End While


'        While RotationX >= MathHelper.TwoPi
'            RotationX -= MathHelper.TwoPi
'        End While
'        While RotationX < 0
'            RotationX += MathHelper.TwoPi
'        End While


'        While RotationZ >= MathHelper.TwoPi
'            RotationZ -= MathHelper.TwoPi
'        End While
'        While RotationZ < 0
'            RotationZ += MathHelper.TwoPi
'        End While


'        If RotationY <> 0 Then
'            ModelRotation *= Matrix.CreateFromAxisAngle(Vector3.Up, RotationY)
'        End If
'        If RotationX <> 0 Then
'            ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Right, RotationX)
'        End If
'        If RotationZ <> 0 Then
'            ModelRotation *= Matrix.CreateFromAxisAngle(Vector3.Forward, RotationZ)
'        End If



'        If (controllerState.IsKeyDown(Keys.F12)) Then
'            Dim fjdbgjf = 4
'            'Position = Vector3.Zero
'            'ModelVelocity = Vector3.Zero

'            'RotationX = 0
'            'RotationY = 0
'            'RotationZ = 0
'        End If

'        RotationY = 0
'        RotationX = 0
'        RotationZ = 0



'        RotationVelocity.Y *= RotationVelocityReducingFactor.Y
'        RotationVelocity.X *= RotationVelocityReducingFactor.X
'        RotationVelocity.Z *= RotationVelocityReducingFactor.Z







'        'Bounding sphere
'        MBoundingSphere = New BoundingSphere(Position, BoundingSphereRadius *
'                          BoundingSphereScale)
'        'HeadPosition = Position + New Vector3(0, 166, 0)

'        FacingDirection = HeadRotation.Forward


'    End Sub









'Public Sub StartWalking()
'    BodyParts(iLLeg).ChildRotationGradient = 2
'    Dim eALegBack As New ePAnimation(BodyParts(iLLeg), New Vector3, Matrix.CreateFromAxisAngle(Vector3.Left, 0.4) * BodyParts(iLLeg).Rotaion, 40, BodyParts(iLLeg).Rotaion.Left)
'    Dim eALegForward As New ePAnimation(BodyParts(iLLeg), New Vector3, Matrix.CreateFromAxisAngle(Vector3.Left, -0.4) * BodyParts(iLLeg).Rotaion, 40, BodyParts(iLLeg).Rotaion.Left)
'    Dim Ch_eALst1 As New List(Of ePAnimation) From {eALegForward, eALegBack}

'    Dim Ch_eA1 As New ePAnimationChain(Ch_eALst1, True)

'    BodyParts(iRLeg).ChildRotationGradient = 2
'    Dim eARLegBack As New ePAnimation(BodyParts(iRLeg), New Vector3, Matrix.CreateFromAxisAngle(Vector3.Left, 0.4) * BodyParts(iLLeg).Rotaion, 40, BodyParts(iRLeg).Rotaion.Left)
'    Dim eARLegForward As New ePAnimation(BodyParts(iRLeg), New Vector3, Matrix.CreateFromAxisAngle(Vector3.Left, -0.4) * BodyParts(iLLeg).Rotaion, 40, BodyParts(iRLeg).Rotaion.Left)

'    eALegBack.Parellel_eAs.Add(eARLegForward)
'    eALegForward.Parellel_eAs.Add(eARLegBack)

'    WalkingAnimations = Ch_eA1

'    Ch_eA1.Start()
'    Ch_eA1.Pause()

'End Sub


'public class block 
'Public ModelRotation As Matrix = Matrix.Identity
'Private Mrotation As Single

'Public MrotationY As Single
'Public MrotationX As Single
'Public MrotationZ As Single

'Public RotationVelocity As Vector3 = Vector3.Zero

'Public RotationVelocityReducingFactor As Vector3 = New Vector3(0.6F, 0.3F, 0.6F)
'Public ModelVelocityReducingFactor As Vector3 = New Vector3(0.97F, 0.97F, 0.97F)




'Public Property RotationY() As Single
'    Get
'        Return MrotationY
'    End Get
'    Set


'        Dim newVal As Single = Value
'        While newVal >= MathHelper.TwoPi
'            newVal -= MathHelper.TwoPi
'        End While
'        While newVal < 0
'            newVal += MathHelper.TwoPi
'        End While

'        If Not MrotationY = newVal Then

'            MrotationY = newVal
'            ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Up, MrotationY)
'        End If

'    End Set
'End Property

'Public Property RotationX() As Single
'    Get
'        Return MrotationX
'    End Get
'    Set

'        Dim newVal As Single = Value
'        While newVal >= MathHelper.TwoPi
'            newVal -= MathHelper.TwoPi
'        End While
'        While newVal < 0
'            newVal += MathHelper.TwoPi
'        End While


'        If Not MrotationX = newVal Then
'            MrotationX = newVal
'            ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Right, MrotationX)
'        End If

'    End Set
'End Property

'Public Property RotationZ() As Single
'    Get
'        Return MrotationZ
'    End Get
'    Set
'        Dim newVal As Single = Value
'        While newVal >= MathHelper.TwoPi
'            newVal -= MathHelper.TwoPi
'        End While
'        While newVal < 0
'            newVal += MathHelper.TwoPi
'        End While

'        If Not MrotationZ = newVal Then
'            MrotationZ = newVal
'            ModelRotation *= Matrix.CreateFromAxisAngle(ModelRotation.Forward, MrotationZ)
'        End If
'    End Set
'End Property



'    Public Sub Move(A As Actions, Distance As Single)


'        MBoundingBox = New BoundingBox(Me.RealPosition, Me.RealPosition + New Vector3(100, 100, 100))



'#Region "Keys"



'        Dim CurActionLst As New List(Of Actions)



'        If A = Actions.Forward Then
'            RealPosition.Z += Distance
'        End If
'        If A = Actions.Backward Then
'            RealPosition.Z -= Distance
'        End If
'        If A = Actions.Left Then
'            RealPosition.X += Distance
'        End If
'        If A = Actions.Right Then
'            RealPosition.X -= Distance
'        End If


'        If A = Actions.Up Then
'            RealPosition.Y += Distance
'        End If
'        If A = Actions.Down Then
'            RealPosition.Y -= Distance
'        End If


'#End Region

'    End Sub
'end class




'Public Shared Function FindSurfaceBlocks() As Regester
'    Dim O As New Regester


'    For Each G In ChunkList
'        For Each J In G
'            For Each CH In J

'                Dim X = CH.Index.X
'                Dim Y = CH.Index.Y
'                Dim Z = CH.Index.Z
'                If CH.IsAir = False Then

'                    Dim Added = False

'                    Try
'                        If ChunkList(X)(Y)(Z + 1).IsAir = True Then
'                            '  SurfaceChunks.Indexes.Add(CH.Position)

'                            If Added = False Then


'                                SurfaceChunksForward.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Forward)
'                                Added = True


'                            End If

'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X)(Y)(Z - 1).IsAir = True Then
'                            If Added = False Then

'                                SurfaceChunksBackward.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Backward)
'                                Added = True




'                            End If
'                        End If

'                    Catch ex As Exception

'                    End Try


'                    Try
'                        If ChunkList(X)(Y + 1)(Z).IsAir = True Then
'                            If Added = False Then
'                                SurfaceChunksUp.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Up)

'                                Added = True



'                                Dim PC As Integer = 0
'                                For Each CX In CH.BlockList(0)
'                                    For Each CZ In CX
'                                        If CZ.IsAir = False Then
'                                            PC += 1
'                                        End If
'                                    Next
'                                Next


'                                If Not PC = 8 * 8 Then
'                                    SurfaceChunksUp.Chunks.Add(ChunkList(X)(Y - 1)(Z))
'                                    'SurfaceChunks.Relations.Add(Vector3.Up)
'                                End If

'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X)(Y - 1)(Z).IsAir = True Then
'                            If Added = False Then
'                                SurfaceChunksDown.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Down)
'                                Added = True



'                                Dim PC As Integer = 0
'                                For Each CX In CH.BlockList(9)
'                                    For Each CZ In CX
'                                        If CZ.IsAir = False Then PC += 1
'                                    Next
'                                Next


'                                If Not PC = 8 * 8 Then
'                                    SurfaceChunksDown.Chunks.Add(ChunkList(X)(Y + 1)(Z))
'                                    'SurfaceChunks.Relations.Add(Vector3.Down)
'                                End If

'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try





'                    Try
'                        If ChunkList(X + 1)(Y)(Z).IsAir = True Then
'                            If Added = False Then
'                                SurfaceChunksRight.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Right)
'                                Added = True

'                                'Dim PC As Integer = 0
'                                'For Each CY In CH.BlockList
'                                '    For Each CX In CY
'                                '        For Each CZ In CX
'                                '            If CZ.Index.Z = 9 Then
'                                '                If CZ.IsAir = False Then PC += 1
'                                '            End If
'                                '        Next
'                                '    Next
'                                'Next


'                                'If Not PC =8*8 Then
'                                '    SurfaceChunks.Chunks.Add(ChunkList(X)(Y + 1)(Z))
'                                '    SurfaceChunks.Relations.Add(Vector3.Up)
'                                'End If

'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X - 1)(Y)(Z).IsAir = True Then
'                            If Added = False Then
'                                SurfaceChunksLeft.Indexes.Add(CH.Position)
'                                'SurfaceChunks.Relations.Add(Vector3.Left)
'                                Added = True
'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try

'                End If
'            Next
'        Next
'    Next


'    Return O
'End Function




















'Ground.RenderingChunks.Add(Ground.ChunkList(0))
'Ground.RenderingChunks.Add(Ground.ChunkList(1))
'Ground.RenderingChunks.Add(Ground.ChunkList(2))
'Ground.RenderingChunks.Add(Ground.ChunkList(3))

'For Each CHX In Ground.ChunkList
'    For Each CHY In CHX
'        For Each CH In CHY
'            If CH.IsAir = False Then
'                If Vector3.Distance(CH.Position, SS.p1.player.Position) < Ground.ChunkRenderDistance Then
'                    'If Vector3.Distance( CH.Position, SS.p1.player.Position) < Ground.ChunkRenderDistance Then

'                    Dim BF As New BoundingFrustum(SS.P1.viewMatrix * SS.P1.projectionMatrix)

'                    If BF.Intersects(New BoundingBox(CH.Position, CH.Position + Chunk.Volume)) Then
'                        Ground.RenderingChunks.Add(CH)
'                    End If

'                End If
'            End If
'        Next
'    Next
'Next


'Dim n = 0
'For Each CH In Ground.SurfaceChunks.Chunks
'    If Vector3.Distance(CH.Position, SS.p1.player.Position) < Ground.ChunkRenderDistance Then


'        Dim BF As New BoundingFrustum(SS.P1.viewMatrix * SS.P1.projectionMatrix)

'        If BF.Intersects(New BoundingBox(CH.Position, CH.Position + Chunk.Volume)) Then

'            Dim FOrB = False
'            Dim UOrD = False
'            Dim LOrR = False

'            Dim OPass = False

'            Dim ORF = CH.Position - SS.p1.player.Position

'            If ORF.X >= 0 Then
'                LOrR = True

'                If Ground.SurfaceChunks.Relations(n).X < 0 Then
'                    OPass = True
'                End If

'            Else
'                LOrR = False
'                If Ground.SurfaceChunks.Relations(n).X > 0 Then
'                    OPass = True
'                End If

'            End If

'            If ORF.Y >= 0 Then
'                UOrD = True
'                If Ground.SurfaceChunks.Relations(n).Y > 0 Then
'                    OPass = True
'                End If
'                'OPass = True
'            Else
'                UOrD = False
'                If Ground.SurfaceChunks.Relations(n).Y > 0 Then
'                    OPass = True
'                End If
'                ' OPass = True
'            End If

'            If ORF.Z >= 0 Then
'                FOrB = False
'                If Ground.SurfaceChunks.Relations(n).Z < 0 Then
'                    OPass = True
'                End If
'            Else
'                FOrB = True
'                If Ground.SurfaceChunks.Relations(n).Z > 0 Then
'                    OPass = True
'                End If
'            End If




'            If OPass = True Then
'                Ground.RenderingChunks.Add(CH)
'            End If
'        End If
'    End If
'    n += 1
'Next




'Public Shared Function FindSurfaceBlocks() As Regester
'    Dim O As New Regester


'    For Each G In ChunkList
'        For Each J In G
'            For Each CH In J

'                Dim X = CH.Index.X
'                Dim Y = CH.Index.Y
'                Dim Z = CH.Index.Z
'                If CH.IsAir = False Then

'                    Dim Added = False
'                    Dim R As New Vector3

'                    Try
'                        If ChunkList(X)(Y)(Z + 1).IsAir = True Then
'                            '  SurfaceChunks.Indexes.Add(CH.Position)
'                            R += Vector3.Backward
'                            If Added = False Then


'                                'SurfaceChunks.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Forward)

'                                Added = True


'                                Dim CB As Integer = 0
'                                For CX = Chunk.Size.X - 1 To 0
'                                    For CY = Chunk.Size.Y - 1 To 0
'                                        For CZ = Chunk.Size.Z - 1 To 0
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Y Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X)(Y)(Z - 1))
'                                    SurfaceChunks.Relations.Add(Vector3.Backward)

'                                End If
'                            End If

'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X)(Y)(Z - 1).IsAir = True Then
'                            R += Vector3.Forward
'                            If Added = False Then

'                                'SurfaceChunks.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Backward)

'                                Added = True



'                                Dim CB As Integer = 0
'                                For CX = Chunk.Size.X - 1 To 0
'                                    For CY = Chunk.Size.Y - 1 To 0
'                                        For CZ = Chunk.Size.Z - 1 To 0
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Y Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X)(Y)(Z + 1))
'                                    SurfaceChunks.Relations.Add(Vector3.Forward)

'                                End If


'                            End If
'                        End If

'                    Catch ex As Exception

'                    End Try


'                    Try
'                        If ChunkList(X)(Y + 1)(Z).IsAir = True Then
'                            R += Vector3.Up
'                            If Added = False Then
'                                ''SurfaceChunks.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Up)

'                                Added = True



'                                Dim CB As Integer = 0

'                                For CX = 0 To Chunk.Size.X - 1
'                                    For CZ = 0 To Chunk.Size.Z - 1
'                                        For CY = 0 To Chunk.Size.Y - 1
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Z Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X)(Y - 1)(Z))
'                                    SurfaceChunks.Relations.Add(Vector3.Up)

'                                End If

'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X)(Y - 1)(Z).IsAir = True Then
'                            R += Vector3.Down
'                            If Added = False Then
'                                'SurfaceChunks.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Down)
'                                Added = True



'                                Dim CB As Integer = 0

'                                For CX = Chunk.Size.X - 1 To 0
'                                    For CZ = Chunk.Size.Z - 1 To 0
'                                        For CY = Chunk.Size.Y - 1 To 0
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Z Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X)(Y + 1)(Z))
'                                    SurfaceChunks.Relations.Add(Vector3.Down)

'                                End If

'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try





'                    Try
'                        If ChunkList(X + 1)(Y)(Z).IsAir = True Then
'                            R += Vector3.Right
'                            If Added = False Then
'                                'SurfaceChunks.Chunks.Add(CH)
'                                'SurfaceChunks.Relations.Add(Vector3.Right)
'                                Added = True


'                                Dim CB As Integer = 0

'                                For CY = Chunk.Size.Y - 1 To 0
'                                    For CZ = Chunk.Size.Z - 1 To 0
'                                        For CX = Chunk.Size.X - 1 To 0
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Z Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X - 1)(Y)(Z))
'                                    SurfaceChunks.Relations.Add(Vector3.Right)

'                                End If


'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try

'                    Try
'                        If ChunkList(X - 1)(Y)(Z).IsAir = True Then
'                            R += Vector3.Left
'                            If Added = False Then
'                                SurfaceChunks.Indexes.Add(CH.Position)
'                                'SurfaceChunks.Relations.Add(Vector3.Left)
'                                Added = True


'                                Dim CB As Integer = 0
'                                For CZ = Chunk.Size.Z - 1 To 0
'                                    For CY = Chunk.Size.Y - 1 To 0
'                                        For CX = Chunk.Size.X - 1 To 0
'                                            If CH.BlockList(CY)(CX)(CZ).IsAir = False Then
'                                                CB += 1
'                                                Exit For
'                                            End If
'                                        Next
'                                    Next
'                                Next


'                                If Not CB = Chunk.Size.X * Chunk.Size.Z Then
'                                    SurfaceChunks.Chunks.Add(ChunkList(X + 1)(Y)(Z))
'                                    SurfaceChunks.Relations.Add(Vector3.Left)

'                                End If
'                            End If
'                        End If
'                    Catch ex As Exception

'                    End Try




'                    If Added = True Then
'                        SurfaceChunks.Chunks.Add(CH)
'                        SurfaceChunks.Relations.Add(R)
'                    End If
'                End If
'            Next
'        Next
'    Next


'    Return O
'End Function




'''' <summary>
'''' Finds Upper blocks of a BL
'''' </summary>
'''' <param name="BL"></param>
'''' Y X Z
'''' <returns></returns>
''Public Shared Function FindSurfaceBlocks(BL As List(Of List(Of List(Of Boolean)))) As List(Of List(Of List(Of SurfaceBlockRegi)))
''    Dim O As New List(Of List(Of List(Of SurfaceBlockRegi)))
''    Dim Ynn = 0
''    For Each YB In BL
''        O.Add(New List(Of List(Of SurfaceBlockRegi)))
''        Dim Xn = 0
''        For Each XB In YB
''            O(Ynn).Add(New List(Of SurfaceBlockRegi))
''            Dim Zn = 0
''            For Each ZZB In XB
''                O(Ynn)(Xn).Add(New SurfaceBlockRegi(New Vector3(Xn, Ynn, Zn), Vector3.Zero))
''                Zn += 1
''            Next
''            Xn += 1
''        Next
''        Ynn += 1
''    Next



''    Dim Yn = 0
''    For Each YB In BL
''        Dim Xn = 0
''        For Each XB In YB
''            Dim Zn = 0
''            For Each ZZB In XB

''                Dim Pos = New Vector3(Xn, Yn, Zn)
''                O(Pos.Y)(Pos.X)(Pos.Z).Place = Pos
''                'Console.WriteLine("Loading...")



''                'Up
''                Try
''                    If BL(Pos.Y + 1)(Pos.X)(Pos.Z) = False Then
''                        O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, 1, 0)

''                    End If
''                Catch ex As Exception

''                End Try
''                'Down
''                Try
''                    If BL(Pos.Y - 1)(Pos.X)(Pos.Z) = False Then
''                        If O(Pos.Y)(Pos.X)(Pos.Z).Relation.Y = 1 Then
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, 1, 0) '=2
''                        Else
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, -2, 0) '=-1
''                        End If

''                    End If
''                Catch ex As Exception

''                End Try




''                'Right
''                Try
''                    If BL(Pos.Y)(Pos.X + 1)(Pos.Z) = False Then
''                        O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(1, 0, 0)
''                    End If
''                Catch ex As Exception

''                End Try
''                'Left
''                Try
''                    If BL(Pos.Y)(Pos.X - 1)(Pos.Z) = False Then
''                        If O(Pos.Y)(Pos.X)(Pos.Z).Relation.X = 1 Then
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(1, 0, 0) '=2
''                        Else
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(-2, 0, 0) '=-1
''                        End If

''                    End If
''                Catch ex As Exception

''                End Try


''                'Backward
''                Try
''                    If BL(Pos.Y)(Pos.X)(Pos.Z + 1) = False Then
''                        O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, 0, 1)
''                    End If
''                Catch ex As Exception

''                End Try
''                'Foward
''                Try
''                    If BL(Pos.Y)(Pos.X)(Pos.Z - 1) = False Then
''                        If O(Pos.Y)(Pos.X)(Pos.Z).Relation.Z = 1 Then
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, 0, 1) '=2
''                        Else
''                            O(Pos.Y)(Pos.X)(Pos.Z).Relation += New Vector3(0, 0, -2) '=-1
''                        End If
''                    End If
''                Catch ex As Exception

''                End Try
''                Zn += 1
''            Next
''            Xn += 1
''        Next
''        Yn += 1
''    Next

''    Return O
''End Function








'#Region "IsCollided"




'    Public Shared Function IsCollided(Object1 As Player, Object2 As Player) As Boolean



'        Dim B1 As BoundingSphere = Object1.MBoundingSphere
'        Dim B2 As BoundingSphere = Object2.MBoundingSphere

'        If B1.Intersects(B2) Then
'            Return True
'        Else
'            Return False
'        End If

'    End Function



'    Public Shared Function IsCollided(Object1 As Bullet, Object2 As Player) As Boolean


'        Dim B1 As BoundingSphere = Object1.MBoundingSphere

'        Dim B2 As BoundingSphere = Object2.MBoundingSphere


'        If B1.Intersects(B2) Then
'            Return True
'        Else
'            Return False
'        End If

'    End Function
'    Public Shared Function IsCollided(Object1 As Player, Object2 As Bullet) As Boolean


'        Dim B1 As BoundingSphere = Object1.MBoundingSphere
'        Dim B2 As BoundingSphere = Object2.MBoundingSphere

'        If B1.Intersects(B2) Then
'            Return True
'        Else
'            Return False
'        End If

'    End Function










'#End Region










'If Controls.Bullets.Count > 0 Then


'    For Each B As Bullet In Controls.Bullets
'        B.Update()

'        Dim PVTBn As Integer = 0


'        If Controls.IsCollided(B, Player) Then
'            B.Owner.LastHitS = Player
'            B.Owner.IsLastHitS = True

'            B.Health -= Player.HitDamage
'            Player.Health -= B.HitDamage

'            If B.Health <= 0 Then
'                DBullets.Add(B)
'            End If
'            If Player.Health <= 0 Then
'                DShips.Add(Player)
'            End If
'            BackgroundColour = Color.Orange
'        End If

'        PVTBn += 1


'    Next
'End If




'Ground GetblockwithEnvironment
'Facing directional Block X
'If FacingDirections(0) = Direction.Forward Then
'If BPZ = 0 Then
'CPZ -= 1
'BPZ = 7
'Else
'BPZ -= 1

'End If

'CH = Ground.ChunkList(CPX)(CPY)(CPZ)
'o(2) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
'o(2).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)
'o(2).Chunk = CH


'ElseIf FacingDirections(0) = Direction.Backward Then
'If BPZ = 7 Then
'CPZ += 1
'BPZ = 0
'Else
'BPZ += 1
'End If

'CH = Ground.ChunkList(CPX)(CPY)(CPZ)
'o(2) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
'o(2).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)
'o(2).Chunk = CH







'ElseIf FacingDirections(0) = Direction.Left Then
'If BPX = 0 Then
'CPX -= 1
'BPX = 7
'Else
'BPX -= 1

'End If

'CH = Ground.ChunkList(CPX)(CPY)(CPZ)
'o(2) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
'o(2).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)
'o(2).Chunk = CH


'ElseIf FacingDirections(0) = Direction.Right Then
'If BPX = 7 Then
'CPX += 1
'BPX = 0
'Else
'BPX += 1

'End If

'CH = Ground.ChunkList(CPX)(CPY)(CPZ)
'o(2) = DBlock.FromBlock(CH.BlockList(BPX)(BPY)(BPZ))
'o(2).RealBlock.CPosition = New Vector3(BPX, BPY, BPZ)
'o(2).Chunk = CH







'End If









'If M.NextUpperBlockX.IsAir Then
'    If M.NextUpperBlockZ.IsAir Then
'        'Both Upper Ways NotBlocked


'        If M.NextBlockX.IsAir Then
'            If M.NextBlockZ.IsAir Then
'                'Way Clear

'                M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward
'                M.ManMovedFB = True

'            Else

'                'X Unblocked. Z Blocked

'                M.MonoFacingDirection = Physics.Find2DUnitDirectionOfDirection(M.FacingDirection)

'                If M.MonoFacingDirection = Direction.Left OrElse M.MonoFacingDirection = Direction.Right Then
'                    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Right
'                    M.ManMovedFB = True

'                ElseIf M.MonoFacingDirection = Direction.Forward OrElse M.MonoFacingDirection = Direction.Backward Then
'                    Go(M, Actions.Up2)
'                End If

'            End If

'        Else
'            'X Blocked

'            If M.NextBlockZ.IsAir Then
'                'X Blocked. Z Unlocked

'                M.MonoFacingDirection = Physics.Find2DUnitDirectionOfDirection(M.FacingDirection)

'                If M.MonoFacingDirection = Direction.Left OrElse M.MonoFacingDirection = Direction.Right Then
'                    Go(M, Actions.Up2)

'                ElseIf M.MonoFacingDirection = Direction.Forward OrElse M.MonoFacingDirection = Direction.Backward Then
'                    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Backward
'                    M.ManMovedFB = True
'                End If

'            Else
'                'Both lower ways blocked

'                Go(M, Actions.Up2)

'            End If

'        End If






'    Else

'        'Z Up is Blocked. X Up is NotBlocked
'        'Sliding on wall
'        If M.NextUpperBlockX.IsAir Then
'            If M.NextBlockX.IsAir Then
'                M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Right
'                M.ManMovedFB = True
'            Else
'                'Lower unblocked. Upper Blocked
'                Go(M, Actions.Up2)
'                M.ManMovedFB = True
'            End If
'        End If


'        'If M.DualFacingDirectionsLR = Direction.Left Then
'        '    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Right
'        '    M.ManMovedFB = True
'        'Else
'        '    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Right
'        '    M.ManMovedFB = True
'        'End If



'    End If

'Else
'    'X up Blocked

'    If M.NextUpperBlockZ.IsAir Then

'        'Z up NotBlocked, X up Blocked
'        'Sliding on wall


'        If M.NextUpperBlockZ.IsAir Then
'            If M.NextBlockZ.IsAir Then
'                M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Backward
'                M.ManMovedFB = True
'            Else
'                'Lower unblocked. Upper Blocked
'                Go(M, Actions.Up2)
'                M.ManMovedFB = True
'            End If
'        End If


'        'If M.DualFacingDirectionsFB = Direction.Forward Then
'        '    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Backward
'        '    M.ManMovedFB = True
'        'Else
'        '    M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Forward * Vector3.Backward
'        '    M.ManMovedFB = True
'        'End If

'    End If

'End If










'ElseIf A = Actions.Backward Then
'Dim BR As Block

'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Backward) + Vector3.Up)
'If IsNothing(BR) Then
'M.Position = (Physics.RNDVec3(2000, 50, 100, 50, 2000, 50))
'M.CurrentBlock = Ground.GetBlock(M.Position - New Vector3(0, Ground.BlockSize, 0))
'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Backward) + Vector3.Up)
'End If
'If BR.IsAir Then
'M.ModelVelocity += M.Accelaration.Z * M.ModelRotation.Backward
'Else
'Go(M, Actions.Up2)
'End If
'M.ManMovedFB = True
'End If



'If A = Actions.Left Then
'Dim BR As Block

'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Left) + Vector3.Up)
'If IsNothing(BR) Then
'M.Position = (Physics.RNDVec3(2000, 50, 100, 50, 2000, 50))
'M.CurrentBlock = Ground.GetBlock(M.Position - New Vector3(0, Ground.BlockSize, 0))
'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Left) + Vector3.Up)

'End If
'If BR.IsAir Then
'M.ModelVelocity += M.Accelaration.X * M.ModelRotation.Left
'Else
'Go(M, Actions.Up2)
'End If

'ElseIf A = Actions.Right Then
'Dim BR As Block

'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Right) + Vector3.Up)
'If IsNothing(BR) Then
'M.Position = (Physics.RNDVec3(2000, 50, 100, 50, 2000, 50))
'M.CurrentBlock = Ground.GetBlock(M.Position - New Vector3(0, Ground.BlockSize, 0))
'BR = Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Right) + Vector3.Up)
'End If
'If BR.IsAir Then
'M.ModelVelocity += M.Accelaration.X * M.ModelRotation.Right
'Else
'Go(M, Actions.Up2)
'End If
'End If

'If A = Actions.Up Then
''  If Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Up) + Vector3.Up).IsAir = True Then
'If M.OnGround Then
'M.ModelVelocity += 50 * M.ModelRotation.Up
'End If
'End If
'If A = Actions.Up2 Then
''  If Ground.GetBlockRealatedTo(M.CurrentBlock, Physics.FindStandedVec3OfDirection(M.ModelRotation.Up) + Vector3.Up).IsAir = True Then
'If M.OnGround Then
'M.ModelVelocity += 30 * M.ModelRotation.Up
'End If
'End If

'If A = Actions.RotateClockwiseY Then
''M.RotationVelocity.Y -= M.RotationAccelaration.Y
'Physics.LookAt(M, New Entity(EntityTypes.WayPoint) With {.Position = M.Position + M.HeadRotation.Forward + New Vector3(0.1, 0, 0)})
'End If
'If A = Actions.RotateAntiClockwiseY Then
'Physics.LookAt(M, New Entity(EntityTypes.WayPoint) With {.Position = M.Position + M.HeadRotation.Forward + New Vector3(-0.1, 0, 0)})
'End If

















'Dim instanceVertexBuffer As DynamicVertexBuffer
'Private Sub DrawModelHardwareInstancing(Mesh As ModelMesh, Transforms As Matrix, instances() As Matrix, view As Matrix, projection As Matrix)
'    If instances.Length <> 0 Then
'        Return
'    End If

'    ' If we have more instances than room in our vertex buffer, grow it to the neccessary size.
'    If (instanceVertexBuffer Is Nothing) OrElse (instances.Length > instanceVertexBuffer.VertexCount) Then
'        If instanceVertexBuffer IsNot Nothing Then
'            instanceVertexBuffer.Dispose()
'        End If

'        instanceVertexBuffer = New DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration, instances.Length, BufferUsage.None)
'    End If

'    ' Transfer the latest instance transform matrices into the instanceVertexBuffer.
'    instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard)

'    Dim MeshVertexBufferBinding(1) As VertexBufferBinding

'    Dim effect As Effect
'    For Each meshPart As ModelMeshPart In Mesh.MeshParts
'        MeshVertexBufferBinding(0) = New VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0)
'        MeshVertexBufferBinding(1) = New VertexBufferBinding(instanceVertexBuffer, 0, 1)
'        ' Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
'        GraphicsDevice.SetVertexBuffers(MeshVertexBufferBinding)


'        GraphicsDevice.Indices = meshPart.IndexBuffer

'        ' Set up the instance rendering effect.
'        effect = meshPart.Effect

'        effect.CurrentTechnique = effect.Techniques("HardwareInstancing")

'        effect.Parameters("World").SetValue(Transforms)
'        effect.Parameters("View").SetValue(view)
'        effect.Parameters("Projection").SetValue(projection)

'        ' Draw all the instance copies in a single call.
'        For Each pass As EffectPass In effect.CurrentTechnique.Passes
'            pass.Apply()
'            GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
'                                                               meshPart.StartIndex, meshPart.PrimitiveCount,
'                               instances.Length)
'        Next




'    Next

'End Sub





'Dim BTArr() As Matrix

'Select Case BT1.CurInstancesArrI
'Case 0
'BTArr = MArr1
'Case 1
'BTArr = MArr2
'Case 2
'BTArr = MArr3
'Case 3
'BTArr = MArr4
'Case 4
'BTArr = MArr5
'Case 5
'BTArr = MArr6
'Case 6
'BTArr = MArr7
'Case 7
'BTArr = MArr8
'Case 8
'BTArr = MArr9
'Case 9
'BTArr = MArr10
'Case 10
'BTArr = MArr11
'Case 11
'BTArr = MArr12
'Case 12
'BTArr = MArr13
'Case 13
'BTArr = MArr14
'Case 14
'BTArr = MArr15
'Case 15
'BTArr = MArr16
'Case 16
'BTArr = MArr17
'Case 17
'BTArr = MArr18
'Case 18
'BTArr = MArr19
'Case 19
'BTArr = MArr20
'End Select


'If Player.FacingDirection.X > 0 Then
'    LD.X = 0.5
'ElseIf Player.FacingDirection.X < 0 Then
'    LD.X = -1.5
'Else
'    LD.X = 0
'End If
'If Player.FacingDirection.Y > 0 Then
'    LD.Y = 0.5
'ElseIf Player.FacingDirection.Y < 0 Then
'    LD.Y = -1.5
'Else
'    LD.Y = 0
'End If
'If Player.FacingDirection.Z > 0 Then
'    LD.Z = 0.5
'ElseIf Player.FacingDirection.Z < 0 Then
'    LD.Z = -1.5
'Else
'    LD.Y = 0
'End If



'Private Sub DrawModelHardwareInstancing(Mesh As ModelMesh, Transforms As Matrix, instances() As Matrix, view As Matrix, projection As Matrix)


'    ' If we have more instances than room in our vertex buffer, grow it to the neccessary size.
'    If (instanceVertexBuffer Is Nothing) OrElse (instances.Length > instanceVertexBuffer.VertexCount) Then
'        If instanceVertexBuffer IsNot Nothing Then
'            instanceVertexBuffer.Dispose()
'        End If

'        instanceVertexBuffer = New DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration, instances.Length, BufferUsage.None)
'    End If

'    ' Transfer the latest instance transform matrices into the instanceVertexBuffer.
'    instanceVertexBuffer.SetData(instances, 0, instances.Length, SetDataOptions.Discard)

'    Dim MeshVertexBufferBinding(1) As VertexBufferBinding

'    Dim effect As Effect
'    For Each meshPart As ModelMeshPart In Mesh.MeshParts
'        MeshVertexBufferBinding(0) = New VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0)
'        MeshVertexBufferBinding(1) = New VertexBufferBinding(instanceVertexBuffer, 0, 1)
'        ' Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
'        GraphicsDevice.SetVertexBuffers(MeshVertexBufferBinding)


'        GraphicsDevice.Indices = meshPart.IndexBuffer

'        ' Set up the instance rendering effect.
'        effect = meshPart.Effect

'        effect.CurrentTechnique = effect.Techniques("HardwareInstancing")

'        effect.Parameters("World").SetValue(Transforms)
'        effect.Parameters("View").SetValue(view)
'        effect.Parameters("Projection").SetValue(projection)

'        ' Draw all the instance copies in a single call.
'        For Each pass As EffectPass In effect.CurrentTechnique.Passes
'            pass.Apply()
'            GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
'                                                               meshPart.StartIndex, meshPart.PrimitiveCount,
'                               instances.Length)
'        Next




'    Next

'End Sub



'Public IVB32 As DynamicVertexBuffer
'Public MA32(31) As Matrix

'Public IVB64 As DynamicVertexBuffer
'Public MA64(63) As Matrix

'Public IVB128 As DynamicVertexBuffer
'Public MA128(127) As Matrix

'Public IVB256 As DynamicVertexBuffer
'Public MA256(255) As Matrix

'Public IVB512 As DynamicVertexBuffer
'Public MA512(511) As Matrix

'Public IVB1024 As DynamicVertexBuffer
'Public MA1024(1023) As Matrix

'Public IVB2048 As DynamicVertexBuffer
'Public MA2048(2047) As Matrix

'Public IVB4096 As DynamicVertexBuffer
'Public MA4096(4095) As Matrix

'Public IVB8192 As DynamicVertexBuffer
'Public MA8192(8191) As Matrix

'Public IVB16384 As DynamicVertexBuffer
'Public MA16384(16383) As Matrix

'Public IVB32768 As DynamicVertexBuffer
'Public MA32768(32767) As Matrix
















''DrawModelHardwareInstancing(BT1.Mesh, BT1.Transform, BT1.BlockInstances, viewMatrix, projectionMatrix)
''Private Sub DrawModelHardwareInstancing(Mesh As ModelMesh, Transforms As Matrix, instances() As Matrix, view As Matrix, projection As Matrix)


'' If we have more instances than room in our vertex buffer, grow it to the neccessary size.
'If (instanceVertexBuffer Is Nothing) OrElse (BT1.BlockInstances.Length > instanceVertexBuffer.VertexCount) Then
'If instanceVertexBuffer IsNot Nothing Then
'instanceVertexBuffer.Dispose()
'End If

'instanceVertexBuffer = New DynamicVertexBuffer(GraphicsDevice, instanceVertexDeclaration, BT1.BlockInstances.Length, BufferUsage.WriteOnly)
'End If

'' Transfer the latest instance transform matrices into the instanceVertexBuffer.
'instanceVertexBuffer.SetData(BT1.BlockInstances, 0, BT1.BlockInstances.Length - 1, SetDataOptions.Discard)

'Dim MeshVertexBufferBinding(1) As VertexBufferBinding

'Dim effect As Effect
'For Each meshPart As ModelMeshPart In BT1.Mesh.MeshParts
'MeshVertexBufferBinding(0) = New VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset, 0)
'MeshVertexBufferBinding(1) = New VertexBufferBinding(instanceVertexBuffer, 0, 1)
'' Tell the GPU to read from both the model vertex buffer plus our instanceVertexBuffer.
'GraphicsDevice.SetVertexBuffers(MeshVertexBufferBinding)


'GraphicsDevice.Indices = meshPart.IndexBuffer

'' Set up the instance rendering effect.
'effect = meshPart.Effect

'effect.CurrentTechnique = effect.Techniques("HardwareInstancing")

'effect.Parameters("World").SetValue(BT1.Transform)
'effect.Parameters("View").SetValue(viewMatrix)
'effect.Parameters("Projection").SetValue(projectionMatrix)

'effect.Parameters("LightDirection1").SetValue(LD1)
'effect.Parameters("LightDirection2").SetValue(LD2)


'effect.Parameters("DiffuseLight").SetValue(DiffuseColor)
'effect.Parameters("AmbientLight").SetValue(AmbientLightColor)


'' Draw all the instance copies in a single call.
'For Each pass As EffectPass In effect.CurrentTechnique.Passes
'pass.Apply()
'GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
'                                                               meshPart.StartIndex, meshPart.PrimitiveCount,
'                               BT1.BlockInstances.Length)
'Next




'Next


'Public Shared Function GetBlockInTheDirection(Pos As Vector3, Dir As Vector3, MaxDistance As Integer, IsAir As Boolean) As DBlock



'    Dim RootI = New IntVector3(Math.Truncate((Pos.X / BlockSize) + 0.5), Math.Truncate((Pos.Y / BlockSize) + 3.5), Math.Truncate((Pos.Z / BlockSize) + 0.5))

'    Dim RootChI = RootI / Chunk.Size

'    RootChI.X = Math.Truncate(RootChI.X)
'    RootChI.Y = Math.Truncate(RootChI.Y)
'    RootChI.Z = Math.Truncate(RootChI.Z)





'    Dim CPos = RootI - (RootChI * Chunk.Size)


'    'Dir.Normalize()

'    Dir = Physics.GetBigerToOne(Dir)



'    Dim n = 0

'    Dim NCPosInt As New IntVector3
'    Dim NCPosSingle As Vector3 = CPos
'    Dim O As DBlock = Nothing
'    Dim Found = False

'    Dim CH = CStack.ChunkList(RootChI.X)(RootChI.Y)(RootChI.Z)
'    Dim NCHIndex = RootChI
'    'Dim PvtNCHIndex As New IntVector3

'    While Found = False
'        If n < MaxDistance Then
'            NCPosSingle += Dir

'            If NCPosSingle.X >= 8 Then
'                NCPosSingle.X -= 8
'                NCHIndex.X += 1
'            End If
'            If NCPosSingle.Y >= 8 Then
'                NCPosSingle.Y -= 8
'                NCHIndex.Y += 1
'            End If
'            If NCPosSingle.Z >= 8 Then
'                NCPosSingle.Z -= 8
'                NCHIndex.Z += 1
'            End If



'            If NCPosSingle.X < 0 Then
'                NCPosSingle.X += 8
'                NCHIndex.X -= 1
'            End If
'            If NCPosSingle.Y < 0 Then
'                NCPosSingle.Y += 8
'                NCHIndex.Y -= 1
'            End If
'            If NCPosSingle.Z < 0 Then
'                NCPosSingle.Z += 8
'                NCHIndex.Z -= 1
'            End If


'            NCPosInt.X = Math.Round(NCPosSingle.X)
'            NCPosInt.Y = Math.Round(NCPosSingle.Y)
'            NCPosInt.Z = Math.Round(NCPosSingle.Z)



'            'If NCPosInt.X > 7 Then
'            '    NCPosInt.X = 0
'            '    PvtNCHIndex.X = NCHIndex.X - 1
'            'Else
'            '    PvtNCHIndex.X = NCHIndex.X
'            'End If

'            'If NCPosInt.Y > 7 Then
'            '    NCPosInt.Y = 0
'            '    PvtNCHIndex.Y = NCHIndex.Y - 1
'            'Else
'            '    PvtNCHIndex.Y = NCHIndex.Y
'            'End If

'            'If NCPosInt.Z > 7 Then
'            '    NCPosInt.Z = 0
'            '    PvtNCHIndex.Z = NCHIndex.Z - 1
'            'Else
'            '    PvtNCHIndex.Z = NCHIndex.Z
'            'End If




'            If NCPosInt.X > 7 Then
'                NCPosInt.X = 7
'            End If

'            If NCPosInt.Y > 7 Then
'                NCPosInt.Y = 7

'            End If

'            If NCPosInt.Z > 7 Then
'                NCPosInt.Z = 7

'            End If




'            CH = CStack.ChunkList(NCHIndex.X)(NCHIndex.Y)(NCHIndex.Z)
'            'CH = CStack.ChunkList(NCHIndex.X)(NCHIndex.Y)(NCHIndex.Z)


'            If CH.AirGrid(NCPosInt.X)(NCPosInt.Y)(NCPosInt.Z) = IsAir Then
'                O = DBlock.FromBlock(CH.BlockList(NCPosInt.X)(NCPosInt.Y)(NCPosInt.Z))
'                O.Chunk = CH

'                Found = True
'                Return O
'                Exit Function
'            End If


'            n += 1
'        Else
'            Found = True
'            Return Nothing

'            Exit Function
'        End If
'    End While


'    Return O
'End Function













'Dim PL = B.Chunk.SurfaceBlocks.ToList
'PL.Remove(B.RealBlock)
'B.Chunk.SurfaceBlocks = PL.ToArray
'B.Chunk.FilledSB -= 1



'Dim PK = B.Chunk.BIDList.ToList

'Dim i = -1
'For n = 0 To B.Chunk.BIDFilledI - 1
'    Dim BB = B.Chunk.BIDList(n)
'    If BB(0) = B.Index.X AndAlso BB(1) = B.Index.Y AndAlso BB(2) = B.Index.Z Then
'        i = n
'    End If
'Next

'PK.RemoveAt(i)
'B.Chunk.BIDList = PK.ToArray


'Dim PJ = B.Chunk.BlockTranslations.ToList
'PJ.RemoveAt(i)
'B.Chunk.BlockTranslations = PJ.ToArray

'B.Chunk.BIDFilledI -= 1

'B.Chunk.CountForBlockTypes(OldBID) -= 1





'Public Shared Sub SetBlockVisible(B As DBlock)



'    If Not SurfaceChunks.Contains(B.Chunk) Then
'        SurfaceChunks(FilledSFC) = B.Chunk
'        FilledSFC += 1
'    End If


'    If Not B.RealBlock.SurfaceRelation Then


'        B.Chunk.SurfaceBlocks(B.Chunk.FilledSB) = B.RealBlock
'        B.Chunk.FilledSB += 1


'        Dim BBlock(4) As Byte

'        BBlock(0) = B.RealBlock.CPosition.X
'        BBlock(1) = B.RealBlock.CPosition.Y
'        BBlock(2) = B.RealBlock.CPosition.Z
'        BBlock(3) = CByte(B.RealBlock.BID Mod 256)
'        BBlock(4) = CByte(Math.Truncate(B.RealBlock.BID / 256))

'        B.Chunk.BIDList(B.Chunk.BIDFilledI) = BBlock

'        Dim BWorld = Matrix.CreateTranslation(New Vector3(BBlock(0) * 50, BBlock(1) * 50, BBlock(2) * 50) + B.Chunk.Position)
'        Array.Resize(B.Chunk.BlockTranslations, B.Chunk.BIDFilledI + 1)
'        B.Chunk.BlockTranslations(B.Chunk.BIDFilledI) = BWorld
'        B.Chunk.CountForBlockTypes(B.RealBlock.BID) += 1

'        B.Chunk.BIDFilledI += 1



'    End If

'End Sub


'BodyParts = New List(Of ePart)
'BodyParts.Add(New ePart("RightKnee", Matrix.Identity, New Vector3(11, 52, -5.7), Me, 0))
'BodyParts.Add(New ePart("LeftHand", Matrix.Identity, New Vector3(-22, 139, 4), Me, 1))
'BodyParts.Add(New ePart("LeftLeg", Matrix.Identity, New Vector3(-9, 98, -5.7), Me, 2))
'BodyParts.Add(New ePart("Body", Matrix.Identity, New Vector3(0, 95, 0), Me, 3))
'BodyParts.Add(New ePart("LeftKnee", Matrix.Identity, New Vector3(-11, 52, -5.7), Me, 4))
'BodyParts.Add(New ePart("Head", Matrix.Identity, New Vector3(0, 151, 0.4), Me, 5))
'BodyParts.Add(New ePart("RightHand", Matrix.Identity, New Vector3(22, 139, 4), Me, 6))
'BodyParts.Add(New ePart("RightLeg", Matrix.Identity, New Vector3(10, 98, -5.7), Me, 7))



'If TimeOfTheDay > 1.54 Then
'TimeDirection = -0.005
'ElseIf TimeOfTheDay > 1.3 AndAlso TimeOfTheDay <= 1.5 Then
'TimeDirection = 0.001
'ElseIf TimeOfTheDay > 0.8 AndAlso TimeOfTheDay <= 1.3 Then
'TimeDirection = 0.005
'ElseIf TimeOfTheDay > 0.4 AndAlso TimeOfTheDay <= 0.8 Then
'TimeDirection = 0.01
'End If










'If BAX > 0 Then

''Right
'If BA(BAX - 1)(BAZ) < y Then
''SetBlockVisibleButNotTranslation(B, Ch)
'End If

'Else
''SetBlockVisibleButNotTranslation(B, Ch)
'End If

'If BAX < HeightMap.Size - 1 Then

''Left
'If BA(BAX + 1)(BAZ) < y Then
''SetBlockVisibleButNotTranslation(B, Ch)
'End If


'End If


'If BAZ > 0 Then

''forward
'If BA(BAZ)(BAZ - 1) < y Then
'SetBlockVisibleButNotTranslation(B, Ch)
'End If

'Else
''SetBlockVisibleButNotTranslation(B, Ch)
'End If

'If BAZ < HeightMap.Size - 1 Then

''Back
'If BA(BAZ)(BAZ + 1) < y Then
'SetBlockVisibleButNotTranslation(B, Ch)
'End If


'End If
















'Dim FBlocks = Ground.GetFacingIsAir(E.Position, Physics.Find2DDualDirectionsOfDirection(E.ModelRotation.Forward), E.ModelRotation.Forward)
'Dim MonoFacingDirection As Direction = Physics.Find2DUnitDirectionOfDirection(E.ModelRotation.Forward)


'If IsNothing(FBlocks) Then
'If E.CheckOutOfStackRange Then E.PutOutOfStackRange()
'                Return True
'Exit Function
'End If


'If FBlocks(2) Then
'If FBlocks(3) Then
''Both Upper Ways NotBlocked


'If FBlocks(0) Then
'If FBlocks(1) Then
''Way Clear

'If FBlocks(4) Then
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward
'E.MovedFB = True

'Else 'XZ blocked
'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Right
'E.MovedFB = True

'Else
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Backward
'E.MovedFB = True

'End If

'End If




'Else

''X Unblocked. Z Blocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Right
'E.MovedFB = True


'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If

'End If

'Else
''X Blocked

'If FBlocks(1) Then
''X Blocked. Z Unlocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then
'Go(E, Actions.Up2)
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Backward
'E.MovedFB = True

'End If

'Else
''Both lower ways blocked

'Go(E, Actions.Up2)
'E.MovedFB = True

'End If

'End If






'Else

''Z Up is Blocked. X Up is NotBlocked
''Sliding on wall
'If FBlocks(2) Then
'If FBlocks(0) Then
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Right
'E.MovedFB = True
'IsPathBlocked = True
'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If





'End If

'Else
''X up Blocked

'If FBlocks(3) Then

''Z up NotBlocked, X up Blocked
''Sliding on wall


'If FBlocks(3) Then
'If FBlocks(1) Then
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Forward * Vector3.Backward
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If



'End If

'End If





'ElseIf A = Actions.Backward Then

'Dim FBlocks = Ground.GetFacingIsAir(E.Position, Physics.Find2DDualDirectionsOfDirection(E.ModelRotation.Forward * -1), E.ModelRotation.Forward * -1)

'Dim MonoFacingDirection As Direction = Physics.Find2DUnitDirectionOfDirection(E.ModelRotation.Backward)


'If IsNothing(FBlocks) Then
'If E.CheckOutOfStackRange Then E.PutOutOfStackRange()
'                Return True
'Exit Function
'End If




'If FBlocks(2) Then
'If FBlocks(3) Then
''Both Upper Ways NotBlocked


'If FBlocks(0) Then
'If FBlocks(1) Then
''Way Clear


'If FBlocks(4) Then
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward
'E.MovedFB = True

'Else 'XZ blocked
'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Right
'E.MovedFB = True

'Else
'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Backward
'E.MovedFB = True

'End If

'End If

'Else

''X Unblocked. Z Blocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Right
'E.MovedFB = True


'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then
'Go(E, Actions.Up2)

'End If

'End If

'Else
''X Blocked

'If FBlocks(1) Then
''X Blocked. Z Unlocked

'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then
'Go(E, Actions.Up2)
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Backward
'E.MovedFB = True


'End If

'Else
''Both lower ways blocked

'Go(E, Actions.Up2)
'E.MovedFB = True

'End If

'End If






'Else

''Z Up is Blocked. X Up is NotBlocked
''Sliding on wall
'If FBlocks(2) Then
'If FBlocks(0) Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Right
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If





'End If

'Else
''X up Blocked

'If FBlocks(3) Then

''Z up NotBlocked, X up Blocked
''Sliding on wall


'If FBlocks(3) Then
'If FBlocks(1) Then

'E.Velocity += E.Accelaration.Z * E.ModelRotation.Backward * Vector3.Backward
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If



'End If

'End If




'End If






'If A = Actions.Left Then




'Dim FBlocks = Ground.GetFacingIsAir(E.Position, Physics.Find2DDualDirectionsOfDirection(E.ModelRotation.Left), E.ModelRotation.Left)

'Dim MonoFacingDirection As Direction = Physics.Find2DUnitDirectionOfDirection(E.ModelRotation.Left)


'If IsNothing(FBlocks) Then
'If E.CheckOutOfStackRange Then E.PutOutOfStackRange()
'                Return True
'Exit Function
'End If




'If FBlocks(2) Then
'If FBlocks(3) Then
''Both Upper Ways NotBlocked


'If FBlocks(0) Then
'If FBlocks(1) Then
''Way Clear


'If FBlocks(4) Then
'E.Velocity += E.Accelaration.X * E.ModelRotation.Left
'E.MovedFB = True

'Else 'XZ blocked
'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Right
'E.MovedFB = True

'Else
'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Backward
'E.MovedFB = True

'End If

'End If


'Else

''X Unblocked. Z Blocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then


'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2


'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Right
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If

'End If

'Else
''X Blocked

'If FBlocks(1) Then
''X Blocked. Z Unlocked

'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then
'Go(E, Actions.Up2)
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2

'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Backward
'E.MovedFB = True
'End If

'Else
''Both lower ways blocked

'Go(E, Actions.Up2)
'E.MovedFB = True

'End If

'End If






'Else

''Z Up is Blocked. X Up is NotBlocked
''Sliding on wall
'If FBlocks(2) Then
'If FBlocks(0) Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2


'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Right
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If





'End If

'Else
''X up Blocked

'If FBlocks(3) Then

''Z up NotBlocked, X up Blocked
''Sliding on wall


'If FBlocks(3) Then
'If FBlocks(1) Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2

'E.Velocity += E.Accelaration.X * E.ModelRotation.Left * Vector3.Backward
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If



'End If

'End If






'ElseIf A = Actions.Right Then





'Dim FBlocks = Ground.GetFacingIsAir(E.Position, Physics.Find2DDualDirectionsOfDirection(E.ModelRotation.Right), E.ModelRotation.Right)

'Dim MonoFacingDirection = Physics.Find2DUnitDirectionOfDirection(E.ModelRotation.Right)



'If IsNothing(FBlocks) Then
'If E.CheckOutOfStackRange Then E.PutOutOfStackRange()
'                Return True
'Exit Function
'End If




'If FBlocks(2) Then
'If FBlocks(3) Then
''Both Upper Ways NotBlocked


'If FBlocks(0) Then
'If FBlocks(1) Then
''Way Clear


'If FBlocks(4) Then
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right
'E.MovedFB = True

'Else 'XZ blocked
'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Right
'E.MovedFB = True

'Else
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Backward
'E.MovedFB = True

'End If

'End If

'Else

''X Unblocked. Z Blocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Right
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If

'End If

'Else
''X Blocked

'If FBlocks(1) Then
''X Blocked. Z Unlocked


'If MonoFacingDirection = Direction.Left OrElse MonoFacingDirection = Direction.Right Then
'Go(E, Actions.Up2)
'E.MovedFB = True

'ElseIf MonoFacingDirection = Direction.Forward OrElse MonoFacingDirection = Direction.Backward Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Backward
'E.MovedFB = True

'End If

'Else
''Both lower ways blocked

'Go(E, Actions.Up2)
'E.MovedFB = True

'End If

'End If






'Else

''Z Up is Blocked. X Up is NotBlocked
''Sliding on wall
'If FBlocks(2) Then
'If FBlocks(0) Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Right
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If





'End If

'Else
''X up Blocked

'If FBlocks(3) Then

''Z up NotBlocked, X up Blocked
''Sliding on wall


'If FBlocks(3) Then
'If FBlocks(1) Then

'E.Position += ((E.CurrentBlock.Index + Vector3.Up) - (E.Position / Ground.BlockSize)) * 2
'E.Velocity += E.Accelaration.X * E.ModelRotation.Right * Vector3.Backward
'E.MovedFB = True
'IsPathBlocked = True

'Else
''Lower unblocked. Upper Blocked
'Go(E, Actions.Up2)
'E.MovedFB = True
'End If
'End If



'End If

'End If