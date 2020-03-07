Imports System.Xml
Imports Microsoft.Xna.Framework

<Serializable>
Public Class XEntity

    Public TypeID As Short
    Public Name As String
    Public ICode As Integer

    Public Health As Integer = 200
    Public MaxHealth As Integer = 500


    Public Strength As Single = 1
    Public Weight As Single = 0.3
    Public XP As Integer = 0

    Public Position As Vector3 = Vector3.Zero

    Public Accelaration As New Vector3(3.0F, 3.0F, 3.4F)
    Public RotationAccelaration As New Vector3(0.02F, 0.03F, 0.02F)


    Public ModelVelocity As New Vector3
    Public MaxVelocity As New Vector3(0.5F, 0.5F, 2.0F)

    Public Jumping As Boolean = False

    Public MovedFB As Boolean = False
    Public WalkingFw As Boolean = False


    Public FacingDirection As Vector3 = New Vector3(0, 0, 1)
    Public FallingSpeed As Single = 0

    Public RotationVelocity As Vector3 = Vector3.Zero

    Public IsDead As Boolean = False

    'ICodes
    Public TargetLockedAIs() As Integer '
    Public Enemies() As Integer '
    Public Friendlies() As Integer '

    Public OnGround As Boolean = False

    Public HighestTatget As Boolean = False
    Public HighestTatgetE As Integer '
    Public HighestTargetMode As AITargetMode


    Public Target As Integer '
    Public TargetMode As AITargetMode
    Public NoTarget As Boolean = True


    Public InRandomMovement As Boolean = False
    Public InRandomMovementTime As Integer
    Public CRandomMovement As Actions
    Public CRandomMovementTime As Integer

    Public Tools() As Integer
    Public CTool As Integer


    Public Sub New()

    End Sub

    Public Sub New(e As Entity)

        With e
            TypeID = .eType.ID
            Name = .Name
            ICode = .ICode

            Health = .Health
            MaxHealth = .MaxHealth

            Strength = .Strength
            Weight = .Weight
            XP = .XP

            Position = .Position

            Accelaration = .Accelaration
            RotationAccelaration = .RotationAccelaration


            ModelVelocity = .Velocity
            MaxVelocity = .MaxVelocity

            Jumping = .Jumping

            MovedFB = .MovedFB
            WalkingFw = .WalkingFw


            FacingDirection = .FacingDirection
            FallingSpeed = .FallingSpeed

            RotationVelocity = .RotationVelocity

            IsDead = .IsDead

            Tools = New Integer(.Tools.Count) {}
            For i = 0 To .Tools.Count - 1
                Tools(i) = .Tools(i).Index
            Next

            If .CTool IsNot Nothing Then
                CTool = .CTool.Index
            Else
                CTool = -1
            End If

            TargetLockedAIs = New Integer(.TargetLockedAIs.Count - 1) {}
            For i = 0 To .TargetLockedAIs.Count - 1
                TargetLockedAIs(i) = .TargetLockedAIs(i).ICode
            Next

            Enemies = New Integer(.Enemies.Count - 1) {}
            For i = 0 To .Enemies.Count - 1
                Enemies(i) = .Enemies(i).ICode
            Next

            Friendlies = New Integer(.Friendlies.Count - 1) {}
            For i = 0 To .Friendlies.Count - 1
                Friendlies(i) = .Friendlies(i).ICode
            Next


            OnGround = .OnGround

            HighestTatget = .HighestTatget
            If .HighestTatgetE IsNot Nothing Then HighestTatgetE = .HighestTatgetE.ICode
            HighestTargetMode = .HighestTargetMode

            If .Target IsNot Nothing Then Target = .Target.ICode
            TargetMode = .TargetMode
            NoTarget = .NoTarget

            InRandomMovement = .InRandomMovement
            InRandomMovementTime = .InRandomMovementTime
            CRandomMovement = .CRandomMovement
            CRandomMovementTime = .CRandomMovementTime

        End With

    End Sub



    Public Shared Function NewXE(e As Entity, Position As Vector3) As XEntity
        Dim Out = New XEntity(e) With {
            .Position = Position
        }

        Return Out

    End Function



    Public Function GetEntity() As Entity
        Dim eType = EntityTypes.Lst(TypeID)
        Dim e As Entity
		'e = Activator.CreateInstance(eType.T)
#Disable Warning BC42016 ' Implicit conversion from 'Object' to 'Entity'.
		e = eType.T.GetConstructor({GetType(EntityType)}).Invoke({eType})
#Enable Warning BC42016 ' Implicit conversion from 'Object' to 'Entity'.
		'  e.eType = eType
		e.Load(eType)


		With e
			.InRandomMovement = InRandomMovement
			.InRandomMovementTime = InRandomMovementTime
			.CRandomMovement = CRandomMovement
			.CRandomMovementTime = CRandomMovementTime
		End With




		'If eType.IsHuman Then

		'    Dim RealE = DirectCast(e, Human)

		'    RealE.StandStrait()
		'    e.CollitionHierarchy = eCollitionHierarchy.CreateNewHumanHierarchy(e)


		'ElseIf eType.ID = EntityTypes.Arrow1.ID Then
		'    e.CollitionHierarchy = eCollitionHierarchy.CreateNewHierarchyForArrow(e)

		'End If


		SetEntitySettings(e)




		Return e
	End Function



	Public Sub SetEntitySettings(ByRef e As Entity)
		With e

			.Name = Name
			.ICode = ICode

			.Health = Health
			.MaxHealth = MaxHealth

			.Strength = Strength
			.Weight = Weight
			.XP = XP

			.Position = Position

			.Accelaration = Accelaration
			.RotationAccelaration = RotationAccelaration


			.Velocity = ModelVelocity
			.MaxVelocity = MaxVelocity

			.Jumping = Jumping

			.MovedFB = MovedFB
			.WalkingFw = WalkingFw


			.FacingDirection = FacingDirection
			.FallingSpeed = FallingSpeed

			.RotationVelocity = RotationVelocity

			.IsDead = IsDead




			.OnGround = OnGround

			.HighestTatget = HighestTatget
			'.HighestTatgetE = HighestTatgetE.ICode
			.HighestTargetMode = HighestTargetMode

			'.Target = Target.ICode
			.TargetMode = TargetMode
			.NoTarget = NoTarget


			If CTool > -1 Then .GiveTool(Tool.Buy(CTool), 1)

		End With
	End Sub



	Public Sub GenerateRelationships(ByRef e As Entity, ByRef EList As List(Of Entity))
		e.TargetLockedAIs = New List(Of Entity)
		e.Enemies = New List(Of Entity)
		e.Friendlies = New List(Of Entity)

		For Each exE In EList

			For Each eIcode In TargetLockedAIs
				If exE.ICode = eIcode Then
					e.TargetLockedAIs.Add(exE)
				End If
			Next

			For Each eIcode In Enemies
				If exE.ICode = eIcode Then
					e.Enemies.Add(exE)
				End If
			Next
			For Each eIcode In Friendlies
				If exE.ICode = eIcode Then
					e.Friendlies.Add(exE)
				End If
			Next


			If HighestTatgetE = exE.ICode Then
				e.HighestTatgetE = exE
			End If

			If Target = exE.ICode Then
				e.Target = exE
			End If

		Next



	End Sub


	Public Shared Sub Save(Path As String, ELst As List(Of XEntity))

		Dim XWriter As New Xml.Serialization.XmlSerializer(GetType(List(Of XEntity)))
		Dim StreamWriter As New IO.StreamWriter(Path)

		Try
			XWriter.Serialize(StreamWriter, ELst)
			StreamWriter.Close()
		Catch ex As Exception
			StreamWriter.Close()

			Save(Path & "Backup", ELst)

			My.Computer.FileSystem.CopyFile(Path & "Backup", Path, True)
		End Try



		My.Computer.FileSystem.CopyFile(Path, Path & "Backup", True)

	End Sub


	Public Shared Function Load(Path As String) As List(Of XEntity)

		Dim Out As List(Of XEntity)

		Dim XWriter As New Xml.Serialization.XmlSerializer(GetType(List(Of XEntity)))
		Dim StreamReader As New IO.StreamReader(Path)

		Try
#Disable Warning BC42016 ' Implicit conversion from 'Object' to 'List(Of XEntity)'.
			Out = XWriter.Deserialize(StreamReader)
#Enable Warning BC42016 ' Implicit conversion from 'Object' to 'List(Of XEntity)'.
			StreamReader.Close()


		Catch ex As Exception
            Out = New List(Of XEntity)
            StreamReader.Close()

            'Restore backup
            If My.Computer.FileSystem.FileExists(Path & "Backup") Then
                My.Computer.FileSystem.CopyFile(Path & "Backup", Path, True)
                Out = Load(Path)

            End If
        End Try


        Return Out
    End Function
End Class