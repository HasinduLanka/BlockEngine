Imports Microsoft.Xna.Framework


'Public Class GameTypes

'    Public Shared ReadOnly Property SplitScreenFreeMode As GameType
'        Get
'            Dim GT As New GameType
'            GT.IsFree = True
'            GT.Name = "SSF"
'            GT.WinMode = "death"

'            Return GT
'        End Get
'    End Property

'    Public Shared ReadOnly Property SinglePlayerFreeMode As GameType
'        Get
'            Dim GT As New GameType
'            GT.IsFree = True
'            GT.Name = "SPF"
'            GT.WinMode = "death"

'            Return GT
'        End Get
'    End Property

'End Class

'Public Class ObstacleTypes
'    Public Shared ReadOnly Property Metor As ObstacleTypes
'        Get
'            Dim Out As New ObstacleTypes
'            Return Out
'        End Get
'    End Property

'    Public Shared ReadOnly Property Crate As ObstacleTypes
'        Get
'            Dim Out As New ObstacleTypes
'            Return Out
'        End Get
'    End Property


'    Public Shared ReadOnly Property Wall As ObstacleTypes
'        Get
'            Dim Out As New ObstacleTypes
'            Return Out
'        End Get
'    End Property
'End Class



Public Class RewardSetMultiplier
    Public Health As Single = 1
    Public Strength As Single = 1

    Public XP As Single = 1

    Public Sub New(HHealth As Single, SStrength As Single, XXP As Single)
        Health = HHealth
        Strength = SStrength
        XP = XXP
    End Sub
End Class



Public Class RewardSet
    Public Health As Integer = 0
    Public Strength As Single = 0
    Public Weight As Single = 0
	Public Accelaration As Vector3 = Vector3.Zero

	Public XP As Integer = 0


    Public Sub Reward(e As Entity)
        e.Health = Math.Min(e.Health + Health, e.MaxHealth)
        e.Strength += Strength
        e.Accelaration += Accelaration
        e.Weight += Weight
        e.XP += XP
    End Sub

    Public Sub Reward(e As Entity, Multiplier As RewardSetMultiplier)
        e.Health += CInt(Health * Multiplier.Health)
        e.Health = Math.Min(e.Health, e.MaxHealth)
        e.Strength += Strength * Multiplier.Strength
        e.Accelaration += Accelaration
        e.Weight += Weight
        e.XP += XP * CInt(Multiplier.XP)
    End Sub


    Public Sub New(HHealth As Integer)
        Health = HHealth
    End Sub

End Class




Public Class Axis
    Public AxisChar As Char
    Public Offset As Vector3 = Vector3.Zero
    Public Gradient As Vector3 = Vector3.One

    Public OwnerIsEntity As Boolean

    Public OwnerEntity As Entity
    Public Owner_ePart As ePart

    Public IsFromWorldCords As Boolean = False

    Public Sub New()
    End Sub

#Disable Warning BC42016 ' Implicit conversion
    Public Sub New(AAxisChar As Char, OOffset As Vector3, GGradient As Vector3, OOwnerIsEntity As Boolean, OOwner As Object)
        AxisChar = AAxisChar
        Offset = OOffset
        Gradient = GGradient
        OwnerIsEntity = OOwnerIsEntity
        If OwnerIsEntity Then
            OwnerEntity = OOwner
        Else
            Owner_ePart = OOwner
        End If
    End Sub
#Enable Warning BC42016 ' Implicit conversion

#Disable Warning BC42016 ' Implicit conversion
    Public Sub New(AAxisChar As Char, OOffset As Vector3, OOwnerIsEntity As Boolean, OOwner As Object)
        AxisChar = AAxisChar
        Offset = OOffset
        OwnerIsEntity = OOwnerIsEntity
        If OwnerIsEntity Then
            OwnerEntity = OOwner
        Else
            Owner_ePart = OOwner
        End If
    End Sub

#Disable Warning BC42016 ' Implicit conversion
    Public Sub New(AAxisChar As String, GGradient As Vector3, OOwnerIsEntity As Boolean, OOwner As Object)
        AxisChar = AAxisChar
        Gradient = GGradient
        OwnerIsEntity = OOwnerIsEntity
        If OwnerIsEntity Then
            OwnerEntity = OOwner
        Else
            Owner_ePart = OOwner
        End If
    End Sub
#Enable Warning BC42016 ' Implicit conversion


#Disable Warning BC42016 ' Implicit conversion
    Public Sub New(AAxisChar As Char, OOwnerIsEntity As Boolean, OOwner As Object)
        AxisChar = AAxisChar
        OwnerIsEntity = OOwnerIsEntity
        If OwnerIsEntity Then
            OwnerEntity = OOwner
        Else
            Owner_ePart = OOwner
        End If
    End Sub
#Enable Warning BC42016 ' Implicit conversion


#Disable Warning BC42016 ' Implicit conversion
    ''' <summary>
    ''' Gets from World Cords
    ''' </summary>
    ''' <param name="AAxisChar"></param>
    Public Sub New(AAxisChar As Char)
        AxisChar = AAxisChar
        IsFromWorldCords = True

    End Sub
#Enable Warning BC42016 ' Implicit conversion




    'Public ReadOnly Property V3() As Vector3
    '    Get
    '        Dim Out As Vector3
    '        If OwnerIsEntity Then
    '            Out = Physics.GetAxisByChar(OwnerEntity.ModelRotation, AxisChar)
    '        Else
    '            Out = Physics.GetAxisByChar(Owner_ePart.Rotaion, AxisChar)
    '        End If

    '        Out += Offset
    '        Out *= Gradient

    '        Return Out
    '    End Get
    'End Property




    Public Shared Widening Operator CType(v As Axis) As Vector3
        Dim Out As New Vector3
        If v.IsFromWorldCords = False Then

            If v.OwnerIsEntity Then
                Out = Physics.GetAxisByChar(v.OwnerEntity.ModelRotation, v.AxisChar)
            Else
                Out = Physics.GetAxisByChar(v.Owner_ePart.Rotation, v.AxisChar)
            End If

            Out += v.Offset
            Out *= v.Gradient
        Else

            Dim C = v.AxisChar

            If C = "D" Then
                Out = Vector3.Down
            ElseIf C = "U" Then
                Out = Vector3.Up
            ElseIf C = "F" Then
                Out = Vector3.Forward
            ElseIf C = "B" Then
                Out = Vector3.Backward
            ElseIf C = "L" Then
                Out = Vector3.Left
            ElseIf C = "R" Then
                Out = Vector3.Right
            End If

            Out += v.Offset
            Out *= v.Gradient
        End If
        Return Out
    End Operator
End Class



