
Imports System.ComponentModel
Imports System.Deployment
Imports Microsoft.Win32
Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics




#Region "Entity"



Public Class Entity


#Region "Vars"

    Public Name As String = "Model0"
    Public ICode As Integer

    Public eType As EntityType


    Public Health As Integer = 200
    Public MaxHealth As Integer = 500

    Public Strength As Single = 1
    Public Weight As Single = 0.3

    Public XP As Integer = 0


    Public Position As Vector3 = Vector3.Zero


    Public Accelaration As New Vector3(3.0F, 3.0F, 3.4F)
    Public RotationAccelaration As New Vector3(0.02F, 0.03F, 0.02F)

    Public RotationVelocityReducingFactor As New Vector3(0.01F, 0.01F, 0.01F)
    Public ModelVelocityReducingFactor As Vector3 = New Vector3(0.7F, 0.6F, 0.7F)

    Public ModelRotation As Matrix = Matrix.Identity
    Public ModelRotationY As Matrix = Matrix.Identity

    Public Velocity As New Vector3(0)
    Public MaxVelocity As New Vector3(0.5F, 0.5F, 2.0F)

    Public Jumping As Boolean = False

    Public MovedFB As Boolean = False
    Public WalkingFw As Boolean = False



    Public HeadRotation As Matrix = Matrix.Identity

    Public NeededBodyRotation As Matrix = Matrix.Identity
    Public NeededBodyRotationChanged As Boolean = True
    Public NeededBodyRotationGainingSpeed As Single = 0.4

    Public FacingDirection As Vector3 = New Vector3(0, 0, 1)

    Public FallingSpeed As Single = 0



    Public RotationVelocity As Vector3 = Vector3.Zero


    Public BodyParts As New List(Of ePart)(8)
    Public Height As Vector3 = New Vector3(0, 120, 0)


    Public BlockEnv As Ground.BlockEnvironment

    Public CurrentChunk As New Chunk
    Public CurrentBlock As DBlock


    Public DualFacingDirectionsFB As Direction
    Public DualFacingDirectionsLR As Direction

    Public TrappedCount As Integer = 0

    Public OnGround As Boolean = False



    Public IsDead As Boolean = False
    Public TargetLockedAIs As New List(Of Entity)

    Public Enemies As New List(Of Entity)
    Public Friendlies As New List(Of Entity)



    Public CollitionHierarchy As eCollitionHierarchy ''

    Public HighestTatget As Boolean = False
    Public HighestTatgetE As Entity
    Public HighestTargetMode As AITargetMode


    Public Target As Entity
    Public TargetMode As AITargetMode
    Public NoTarget As Boolean = True


    Public CRandomMovement As Actions
    Public InRandomMovement As Boolean = False
    Public CRandomMovementTime As Integer
    Public InRandomMovementTime As Integer

    Public CurrActionLst As New List(Of Actions)

    Public LastActionTime As Double = 0

    Public Tools As New List(Of Tool)
    Public CTool As Tool

    Public Property RotationY() As Single
    Public Property RotationX() As Single
    Public Property RotationZ() As Single


    Public NoAI As Boolean = False
    Public DelTargetScan As Action(Of Entity)
    Public DelUpdateAI As Action(Of Entity)

    Public CloseByRange As Single = 1000

#End Region

#Region "Overridables"
    Public Overridable Sub Update()

        '  eType.MethUpdate.Invoke(Me, Nothing)


    End Sub

    Public Overridable Sub Draw()

    End Sub

    Public Sub ExpensiveUpdate()

        If DelTargetScan IsNot Nothing Then DelTargetScan(Me)

    End Sub


    Public Overridable Sub Load(eType As EntityType)
        Me.eType = eType
    End Sub



    Public Sub New(eT As EntityType)
        eType = eT
    End Sub


    Public Sub New()

    End Sub




    Public Overridable Sub GiveTool(T As Tool, Place As Integer)

    End Sub


    Public Overridable Sub DoCurrentActions()
        For Each Act In CurrActionLst
            If Controls.Go(Me, Act) Then If InRandomMovement Then PickNewRandomMovement()

        Next



        'Attack
        If CurrActionLst.Contains(Actions.Attack) Then
            If CTool IsNot Nothing Then

                If LastActionTime + 500 < NowGameTime And CTool.Owner.LockedeA = False Then


                    For Each eA In CTool.Owner.CurrentAnimations
                        eA.Reset()
                    Next
                    CTool.Owner.Revert()

                    CTool.Use(RandomOf({Tool.Action.PrimaryAttack1, Tool.Action.PrimaryAttack2, Tool.Action.ShortAttack}))
                    'CTool.Use(Tool.Action.ShortAttack)

                    LastActionTime = NowGameTime

                End If

            End If

        End If

        CurrActionLst.Clear()
    End Sub

#End Region


    Public Sub ShotHit(Hater As Entity)


        CheckAndLockTargetForAttack(Hater)

        Dim Helpers = NearEntities(500, EntityRelationMode.Friends)


        For Each e In Helpers
            e.CheckAndLockTargetForAttack(Hater)
        Next

        If Helpers.Count = 0 Then

        End If

    End Sub


    Public Function LockTarget(at As Entity, Mode As AITargetMode, Optional ForHowLong As Integer = 600) As Boolean
        If Not at = Me Then
            If Not at.IsDead Then
                NoTarget = False
                Target = at
                TargetMode = Mode


                If Not at.TargetLockedAIs.Contains(Me) Then at.TargetLockedAIs.Add(Me)

                InRandomMovement = False
                InRandomMovementTime = ForHowLong
                CRandomMovementTime = 0
                Return True
            End If
        End If

        Return False
    End Function






    Public Function CheckAndLockTargetForAttack(at As Entity) As AITargetMode
        If Not at = Me Then
            If Not at.IsDead Then
                Dim Rel = ChekRelation(at)


                If eType.TargetSelectingRulesForDamage.ContainsKey(Rel) Then
                    Dim TTargetMode = eType.TargetSelectingRulesForDamage(Rel)
                    If TTargetMode <> AITargetMode.None Then
                        LockTarget(at, TTargetMode)
                    End If

                    Return TTargetMode

                End If

            End If

        End If

        Return Nothing
    End Function


    Public Function CheckAndLockTargetForSee(at As Entity) As AITargetMode
        If Not at = Me Then
            If Not at.IsDead Then
                Dim Rel = ChekRelation(at)


                If eType.TargetSelectingRulesForDetection.ContainsKey(Rel) Then
                    Dim TTargetMode = eType.TargetSelectingRulesForDetection(Rel)
                    If TTargetMode <> AITargetMode.None Then
                        LockTarget(at, TTargetMode)
                    End If

                    Return TTargetMode

                End If

            End If

        End If

        Return Nothing
    End Function





    Public Sub ReleaseTartget(Optional PickNewRndMovement As Boolean = True)
        NoTarget = True
        If Target IsNot Nothing Then
            Target.TargetLockedAIs.Remove(Me)
            TargetLockedAIs.Remove(Target)
        End If

        Target = Nothing


        If HighestTatget AndAlso Not HighestTatgetE.IsDead Then
            LockTarget(HighestTatgetE, HighestTargetMode)
            PickNewRndMovement = False
        End If

        If PickNewRndMovement Then PickNewRandomMovement()


    End Sub





    Public Sub DefendHere()

        PickNewRandomMovement(Actions.Null, Integer.MaxValue)
        NoTarget = False

#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        DelTargetScan = AIs.AttackWhenCloseBy
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.

    End Sub



    Public Function ChekRelation(e As Entity) As EntityRelationMode


        For Each eni In Enemies
            If e = eni Then
                Return EntityRelationMode.Enemy
            End If
        Next
        For Each eni In Friendlies
            If e = eni Then
                Return EntityRelationMode.Friends
            End If
        Next


        For Each eni In eType.EtFriendlies
            If e.eType.Equals(eni) Then
                Return EntityRelationMode.TypeFriends
            End If
        Next

        For Each eni In eType.EtEnemies
            If e.eType.Equals(eni) Then
                Return EntityRelationMode.TypeEnemies
            End If
        Next




        Return EntityRelationMode.Unknowen
    End Function



    Public Sub BeFriends(e As Entity)
        BeUnknown(e)

        If Not Friendlies.Contains(e) Then
            Friendlies.Add(e)
        End If

        If Not e.Friendlies.Contains(Me) Then
            e.Friendlies.Add(Me)
        End If
    End Sub

    Public Sub BeEnemies(e As Entity)
        BeUnknown(e)

        If Not Enemies.Contains(e) Then
            Enemies.Add(e)
        End If

        If Not e.Enemies.Contains(Me) Then
            e.Enemies.Add(Me)
        End If
    End Sub

    Public Sub BeUnknown(e As Entity)
        If Enemies.Contains(e) Then Enemies.Remove(e)
        If Friendlies.Contains(e) Then Friendlies.Remove(e)

        If e.Enemies.Contains(e) Then e.Enemies.Remove(e)
        If e.Friendlies.Contains(e) Then e.Friendlies.Remove(e)

    End Sub





#Region "Commander Related"
    Public Function NearestEntity() As Entity
        Dim Nearest As Entity = Nothing
        Dim NearestDistance As Single = Single.PositiveInfinity

        For Each e In Ground.CStack.eList
            If Not e = Me Then

                Dim CDistance = Vector3.DistanceSquared(Position, e.Position)
                If CDistance < NearestDistance Then
                    NearestDistance = CDistance
                    Nearest = e
                End If

            End If
        Next


        Return Nearest


    End Function


    Public Function NearEntities(radius As Single) As Entity()
        Dim Nearests As New List(Of Entity)
        Dim r = radius * radius

        For Each e In Ground.CStack.eList
            If e <> Me Then


                If Vector3.DistanceSquared(Position, e.Position) < r Then
                    Nearests.Add(e)
                End If

            End If
        Next


        Return Nearests.ToArray


    End Function


    Public Function NearEntities(radius As Single, Relation As EntityRelationMode) As Entity()
        Dim Nearests As New List(Of Entity)
        Dim r = radius * radius

        For Each e In Ground.CStack.eList
            If e <> Me Then

                If Vector3.DistanceSquared(Position, e.Position) < r AndAlso e.ChekRelation(Me) = Relation Then
                    Nearests.Add(e)
                End If

            End If
        Next


        Return Nearests.ToArray


    End Function

    Public Function FacingEntities(Distance As Single, FieldOfViewDegrees As Single) As Entity()
        Dim FacingEs As New List(Of Entity)

        Dim ViewFrust As New BoundingFrustum(Matrix.CreateLookAt(Position, Position + HeadRotation.Forward, HeadRotation.Up) * Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfViewDegrees), 2, 1, Distance))

        For Each e In Ground.CStack.eList
            If e <> Me Then

                If ViewFrust.Intersects(New BoundingSphere(e.Position, 100)) Then
                    FacingEs.Add(e)
                End If

            End If
        Next


        Return FacingEs.ToArray


    End Function


    Public Function FacingEntity(Length As Single) As Entity


        Dim R As New Ray(Position + Height, HeadRotation.Forward)

        For Each e In Ground.CStack.eList
            If e <> Me Then

                If e.CollitionHierarchy?.GetCollided(R, Length) IsNot Nothing Then
                    Return e
                End If

            End If
        Next



        Return Nothing
    End Function

    Public Sub Hire(Owner As Entity)
        BeFriends(Owner)
        LockTarget(Owner, AITargetMode.Follow)

        HighestTatget = True
        HighestTatgetE = Owner
        HighestTargetMode = AITargetMode.Follow

        For Each e In Owner.Friendlies
            e.BeFriends(Me)
        Next
    End Sub


    Public Sub Go(Act As Actions)

        If Not CurrActionLst.Contains(Act) Then CurrActionLst.Add(Act)

    End Sub

    Public Sub Go(Destination As Vector3)

        NoTarget = False
        Dim OriginalAI = DelUpdateAI
#Disable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.
        DelUpdateAI = AIs.Worker
#Enable Warning BC42016 ' Implicit conversion from 'Action(Of Human)' to 'Action(Of Entity)'; this conversion may fail because 'Entity' is not derived from 'Human', as required for the 'In' generic parameter 'T' in 'Delegate Sub Action(Of In T)(obj As T)'.



        LookAtPosition(Me, Destination)

        While NeededBodyRotationChanged
            Delay(10)
        End While



        Dim LastDistance As Double = Double.MaxValue
        Dim CurrDist As Double = Double.MaxValue - 1

        Dim AccelerationSq = 50 * 50

        Dim LastUpdateCount = 0L

        Do Until CurrDist < AccelerationSq

            '    Controls.Go(Me, Actions.Forward)
            LastUpdateCount = CurrUpdateCount
            Go(Actions.Forward)

            While LastUpdateCount = CurrUpdateCount
                Delay(10)
            End While


            CurrDist = Vector3.DistanceSquared(Position, Destination)

            If CurrDist > LastDistance Then
                'Look at again
                LookAtPosition(Me, Destination)

                While NeededBodyRotationChanged
                    Delay(10)
                End While
            End If

            LastDistance = CurrDist

        Loop



        NoTarget = True
        DelUpdateAI = OriginalAI

    End Sub

#End Region




    Public Sub PickNewRandomMovement()
        If Not NoAI Then

            Dim RNDOut = CRND.Next(0, 4)


            If RNDOut = 0 Then
                CRandomMovement = Actions.Forward
                InRandomMovementTime = CRND.Next(200)
            ElseIf RNDOut = 1 Then
                CRandomMovement = Actions.RotateClockwiseY
                InRandomMovementTime = CRND.Next(30)
            ElseIf RNDOut = 2 Then
                CRandomMovement = Actions.RotateAntiClockwiseY
                InRandomMovementTime = CRND.Next(30)
            Else
                CRandomMovement = Actions.Null
                InRandomMovementTime = CRND.Next(60)
            End If

            InRandomMovement = True
            CRandomMovementTime = 0

            NoTarget = True

        End If

    End Sub


    Public Sub PickNewRandomMovement(Act As Actions, MaxTime As Integer, MinTime As Integer)



        CRandomMovement = Act
        InRandomMovementTime = CRND.Next(MinTime, MaxTime)


        InRandomMovement = True
        CRandomMovementTime = 0
        NoTarget = True



    End Sub


    Public Sub PickNewRandomMovement(Act As Actions, Time As Integer)



        CRandomMovement = Act
        InRandomMovementTime = Time

        InRandomMovement = True
        CRandomMovementTime = 0
        NoTarget = True



    End Sub
    Public Overridable Sub Kill()
        Health = -1

        IsDead = True

        'on error Resume Next
        For n = 0 To Friendlies.Count - 1
            Friendlies(n).Friendlies.Remove(Me)
        Next

        For n = 0 To Enemies.Count - 1
            Enemies(n).Enemies.Remove(Me)
        Next


    End Sub



#Region "Stack related"

    ''' <summary>
    ''' Player Only
    ''' </summary>
    Public Sub ChunkOutOfStackRange()

        ''on error Resume Next

        Dim RealChunkIndex = Ground.ChunkIndexOfPosition(Position)


        'If CurrentChunk.Index <> RealChunkIndex Then

        Dim BNotOutOfRange = True

        If Ground.CStack.ChunkRangeMax.Z <= RealChunkIndex.Z Then
            Position.Z = ((Ground.CStack.ChunkRangeMax.Z) * Chunk.VolumeI) - 50.0F
            BNotOutOfRange = False
        ElseIf RealChunkIndex.Z <= Ground.CStack.ChunkRangeMin.Z Then
            Position.Z = ((Ground.CStack.ChunkRangeMin.Z) * Chunk.VolumeI) + 50.0F
            BNotOutOfRange = False
        End If


        If Ground.CStack.ChunkRangeMax.X <= RealChunkIndex.X Then
            Position.X = ((Ground.CStack.ChunkRangeMax.X) * Chunk.VolumeI) - 50.0F
            BNotOutOfRange = False
        ElseIf RealChunkIndex.X <= Ground.CStack.ChunkRangeMin.X Then
            Position.X = ((Ground.CStack.ChunkRangeMin.X) * Chunk.VolumeI) + 50.0F
            BNotOutOfRange = False
        End If

        If BNotOutOfRange Then
            'Chunk still loading
            Loader.LoadAndReplaceChunkTempory(RealChunkIndex)

            'Threading.Thread.Sleep(1000)

            'Position += (Ground.CStack.ChunkRangeMiddle - RealChunkIndex) * 50

            Velocity *= -5

        End If


        'To stop AIs from hanging with edges
        InRandomMovement = False

        'End If
    End Sub



    Public Function CheckOutOfStackRange() As Boolean

        Dim RealChunkIndex = Ground.ChunkIndexOfPosition(Position)

        If Ground.CStack.ChunkRangeMax.Z <= RealChunkIndex.Z Then
            Return True
        ElseIf RealChunkIndex.Z <= Ground.CStack.ChunkRangeMin.Z Then
            Return True

        ElseIf Ground.CStack.ChunkRangeMax.X <= RealChunkIndex.X Then
            Return True
        ElseIf RealChunkIndex.X <= Ground.CStack.ChunkRangeMin.X Then
            Return True
        End If

        Return False
    End Function






    Public Function CheckOutOfWorld() As Boolean

        Dim RealChunkIndex = Ground.ChunkIndexOfPosition(Position)

        If Loader.MaxWorldBorders.Z <= RealChunkIndex.Z Then
            Return True
        ElseIf RealChunkIndex.Z <= 0 Then
            Return True

        ElseIf Loader.MaxWorldBorders.X <= RealChunkIndex.X Then
            Return True
        ElseIf RealChunkIndex.X <= 0 Then
            Return True
        End If

        Return False
    End Function



    Public Sub PutInsideOfTheWorld()

        Dim RealChunkIndex = Ground.ChunkIndexOfPosition(Position)


        If Loader.MaxWorldBorders.X <= RealChunkIndex.X Then
            Position.X = (Loader.MaxWorldBorders.X * 8 * 50) - 100.0F

        ElseIf RealChunkIndex.X <= Ground.CStack.ChunkRangeMin.X Then
            Position.X = 10 * 50

        End If


        If Loader.MaxWorldBorders.Z <= RealChunkIndex.Z Then
            Position.Z = (Loader.MaxWorldBorders.Z * 8 * 50) - 100.0F

        ElseIf RealChunkIndex.Z <= Ground.CStack.ChunkRangeMin.Z Then
            Position.Z = 10 * 50

        End If




    End Sub


#End Region




    ''' <summary>
    ''' Last ICode
    ''' </summary>
    Public Shared LICode As Short = 0

    Public Shared Operator =(Left As Entity, Right As Entity) As Boolean
        Return Left IsNot Nothing AndAlso Right IsNot Nothing AndAlso Left.ICode = Right.ICode
    End Operator

    Public Shared Operator <>(Left As Entity, Right As Entity) As Boolean
        Return Not (Left IsNot Nothing AndAlso Right IsNot Nothing AndAlso Left.ICode = Right.ICode)
    End Operator



    Public Overrides Function Equals(obj As Object) As Boolean
        Return obj IsNot Nothing AndAlso ICode = DirectCast(obj, Entity).ICode
    End Function

End Class

#End Region










#Region "EntityTypes"

Public Class EntityType
    Public ID As Short
    Public Model As Model
    Public Transforms As Matrix

    Public IsHuman As Boolean = False
    Public IsPlayer As Boolean = False

    Public T As Type


    Public DelTargetScan As Action(Of Entity)
    Public UpdateAI As Action(Of Entity)

    Public EtEnemies As New List(Of EntityType)
    Public EtFriendlies As New List(Of EntityType)

    Public TargetSelectingRulesForDamage As New Dictionary(Of EntityRelationMode, AITargetMode)
    Public TargetSelectingRulesForDetection As New Dictionary(Of EntityRelationMode, AITargetMode)


    Public LookingAtTimeout As Integer = 50

    Public Width As Single

    Public Sub Load(M As Model, ID As Short)

        If M IsNot Nothing Then

            Me.Model = New Model(Game.GraphicsDevice, New List(Of ModelBone), New List(Of ModelMesh))
            CloneObject(M, Model) 'New Model(Game.GraphicsDevice, M.Bones.ToList, M.Meshes.ToList)
            Me.Model.Tag = ID
        End If




        Me.ID = ID

    End Sub

    Public Sub SetMethods(TType As Type)
        T = TType

    End Sub


    Public Sub CreateBasicTargetingRules()

        TargetSelectingRulesForDamage.Clear()
        TargetSelectingRulesForDamage.Add(EntityRelationMode.Enemy, AITargetMode.FollowAndKill)
        TargetSelectingRulesForDamage.Add(EntityRelationMode.TypeEnemies, AITargetMode.FollowAndKill)

        TargetSelectingRulesForDamage.Add(EntityRelationMode.Friends, AITargetMode.None)
        TargetSelectingRulesForDamage.Add(EntityRelationMode.TypeFriends, AITargetMode.None)

        TargetSelectingRulesForDamage.Add(EntityRelationMode.Unknowen, AITargetMode.FollowAndKill)


        TargetSelectingRulesForDetection.Clear()
        TargetSelectingRulesForDetection.Add(EntityRelationMode.Enemy, AITargetMode.FollowAndKill)
        TargetSelectingRulesForDetection.Add(EntityRelationMode.TypeEnemies, AITargetMode.FollowAndKill)

        TargetSelectingRulesForDetection.Add(EntityRelationMode.Friends, AITargetMode.None)
        TargetSelectingRulesForDetection.Add(EntityRelationMode.TypeFriends, AITargetMode.None)

        TargetSelectingRulesForDetection.Add(EntityRelationMode.Unknowen, AITargetMode.None)

    End Sub

End Class




Public Class EntityTypes
    Public Shared Property Human1 As EntityType
    Public Shared Property Civilian As EntityType
    Public Shared Property Guard As EntityType
    Public Shared Property Murderer As EntityType

    Public Shared Property Player1 As EntityType

    Public Shared Property Arrow1 As EntityType

    Public Shared Property WayPoint As EntityType


    Public Shared Lst As New List(Of EntityType)

End Class





#End Region




Public Enum AITargetMode
    None
    FollowAndKill
    FollowAndAttackOnce
    Follow
    Look
    RunFromIt
End Enum

Public Enum EntityRelationMode
    Unknowen = 0

    Friends = 2
    Enemy = 4

    TypeFriends = 8
    TypeEnemies = 16

End Enum


