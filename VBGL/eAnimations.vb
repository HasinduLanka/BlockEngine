#Region "ePart"

Imports Microsoft.Xna.Framework

Public Class ePart
    Public Name As String = ""
    Public Rotation As Matrix = Matrix.Identity
    Public RelativePosition As Vector3
    Public Parent As Entity
    Public Charter As Human
    Public Hurt As Byte = 0

    Public Index As Integer

    Public Children As New List(Of ePart)
    Public Length As Single
    Public ChildDirection As Char
    Public eAChildRotationGradient As Single 'Only using to rotate when animations
    Public ChildRotationGradient As Single 'Using to rotate when not animations
    Public RelativePositionFromParent As Vector3 = Vector3.Zero

    Public OriginalRotation As Matrix = Matrix.Identity
    Public DefualtRotation As Matrix = Matrix.Identity
    Public OriginalRelativePosition As Vector3 = Vector3.Zero

    Public LockedeA As Boolean = False
    Public CurrentAnimations As New List(Of ePAnimation)

    Public Sub New(NName As String, RRotaion As Matrix, RRelativePosition As Vector3, PParent As Entity, Index As Integer)
        Name = NName
        Rotation = RRotaion
        RelativePosition = RRelativePosition
        Parent = PParent

        OriginalRotation = RRotaion
        DefualtRotation = RRotaion
        OriginalRelativePosition = RRelativePosition

        Me.Index = Index

    End Sub

    Public Sub New(PParent As Entity)
        Parent = PParent
    End Sub


    Public Sub Hurten(Damage As Single)
        If Damage < 0 Then
            If Hurt > 0 Then
                If Hurt + Damage > 0 Then
                    Hurt -= CByte(Damage * -1)
                Else
                    Hurt = 0
                End If
            End If
        Else
            If Hurt < 255 - CByte(Damage) Then
                Hurt += CByte(Damage)
            Else
                Hurt = 255
            End If
        End If


    End Sub


    Public Sub Rotate(Axis As Vector3, Angle As Single)


        Rotation *= Matrix.CreateFromAxisAngle(Axis, Angle)


        If Children.Count > 0 Then
            For Each Ch In Children


                Ch.Rotation *= Matrix.CreateFromAxisAngle(Axis, Angle * Ch.ChildRotationGradient)


                Ch.RelativePosition = RelativePosition + (Axis * Ch.RelativePositionFromParent * Length)
            Next


        End If
    End Sub

    Public Sub RotateAsAnimation(Axis As Vector3, Angle As Single)


        Rotation *= Matrix.CreateFromAxisAngle(Axis, Angle)



        If Children.Count > 0 Then
            For Each Ch In Children



                Ch.Rotation *= Matrix.CreateFromAxisAngle(Axis, Angle * Ch.eAChildRotationGradient)

                Ch.RelativePosition = RelativePosition + (Physics.GetAxisByChar(Rotation, ChildDirection, Ch.RelativePositionFromParent) * Length)
            Next
        End If
    End Sub

    Public Sub RotateAsChild(RotationChange As Matrix)

        Rotation *= RotationChange
        RelativePosition = Vector3.Transform(RelativePosition, RotationChange)


        If Children.Count > 0 Then
            For Each Ch In Children

                Ch.Rotation *= RotationChange

                Ch.RelativePosition = RelativePosition + (Physics.GetAxisByChar(Rotation, ChildDirection, Ch.RelativePositionFromParent) * Length)
            Next


        End If
    End Sub

    Public Sub RotateAsChild(Axis As Vector3, Angle As Single)
        Dim RotationCHange = Matrix.CreateFromAxisAngle(Axis, Angle)

        Rotation *= RotationCHange
        RelativePosition = Vector3.Transform(RelativePosition, RotationCHange)


        If Children.Count > 0 Then
            For Each Ch In Children

                Ch.Rotation *= Matrix.CreateFromAxisAngle(Axis, Angle * Ch.ChildRotationGradient)

                Ch.RelativePosition = RelativePosition + (Physics.GetAxisByChar(Rotation, ChildDirection, Ch.RelativePositionFromParent) * Length)
            Next


        End If
    End Sub



    Public Sub Rotate(RotationChange As Matrix)

        Rotation *= RotationChange

        ' RelativePosition = Vector3.Transform(RelativePosition, RotationCHange)

        If Children.Count > 0 Then
            For Each Ch In Children

                Ch.Rotation *= RotationChange

                Dim ChildDirectionM = Physics.GetAxisByChar(Rotation, ChildDirection, Ch.RelativePositionFromParent) * Ch.RelativePositionFromParent
                Ch.RelativePosition = RelativePosition + (ChildDirectionM * Length)

            Next
        End If
    End Sub




    Public Sub Revert()

        'Rotation = OriginalRotation * Parent.ModelRotation
        Rotation = OriginalRotation

        'RelativePosition = OriginalRelativePosition


    End Sub



End Class

#End Region


#Region "ePAnimations"
Public Class ePAnimationChain




    Public CurrentAnimation As ePAnimation
    Public iCurrentAnimation As Integer = 0
    Public AnimationList As New List(Of ePAnimation)
    Public Loopable As Boolean
    Public Playing As Boolean = False
    Public Done As Boolean = False
    Public Name As String

    'Public Tool1 As Tool
    'Public RotationGradient As Single

    Public Reset_eAC_After_eA As ePAnimationChain
    Public Reset_LockedeA_After_eA As New List(Of ePart)
    Public Revert_After_eA As New List(Of ePart)

    Public Reset_N_Start_eAC_After_eA As ePAnimationChain




    Public Sub Next_eA()
        CurrentAnimation.Pause()

        If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
            For Each eA In CurrentAnimation.Parellel_eAs
                eA.Pause()
                If eA.CAngle <> eA.DAngle Then
                    eA.eP.RotateAsAnimation(eA.Axis, eA.DAngle - eA.CAngle)
                    eA.CAngle = eA.DAngle
                End If

            Next
        End If


        iCurrentAnimation += 1
        If iCurrentAnimation = AnimationList.Count Then
            If Loopable Then
                iCurrentAnimation = 0


                CurrentAnimation = AnimationList(iCurrentAnimation)
                CurrentAnimation.RResume()


                If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
                    For Each eA In CurrentAnimation.Parellel_eAs
                        eA.RResume()
                    Next
                End If


            Else
                For Each eA In AnimationList

                    If Not IsNothing(eA.Parellel_eAs) Then
                        For Each eA1 In eA.Parellel_eAs

                            If eA1.CAngle <> eA1.DAngle Then
                                eA1.eP.RotateAsAnimation(eA1.Axis, eA1.DAngle - eA1.CAngle)
                                eA1.CAngle = eA1.DAngle
                            End If
                            eA1.Destroy()
                        Next
                    End If



                    If eA.CAngle <> eA.DAngle Then
                        eA.eP.RotateAsAnimation(eA.Axis, eA.DAngle - eA.CAngle)
                        eA.CAngle = eA.DAngle
                    End If
                    eA.Destroy()
                Next
                iCurrentAnimation = 0



                If Not IsNothing(Reset_eAC_After_eA) Then
                    For Each eAA In Reset_eAC_After_eA.AnimationList
                        If Not IsNothing(eAA.Parellel_eAs) Then
                            For Each eAP In eAA.Parellel_eAs
                                eAP.Reset()
                            Next
                        End If
                    Next


                    Reset_eAC_After_eA.Reset()

                End If

                If Not IsNothing(Reset_LockedeA_After_eA) Then
                    For Each eA In Reset_LockedeA_After_eA
                        eA.LockedeA = False
                    Next
                End If

                If Not IsNothing(Revert_After_eA) Then
                    For Each eA In Revert_After_eA
                        eA.Revert()
                    Next
                End If

                If Not IsNothing(Reset_N_Start_eAC_After_eA) Then
                    Reset_N_Start_eAC_After_eA.Reset()
                    Reset_N_Start_eAC_After_eA.Start()
                End If

                RaiseEvent Completed(Me, New EventArgs)

                'If Not IsNothing(Tool1) Then Tool1.AttackCompleted(Me, New EventArgs)


                Done = True
                Exit Sub
            End If
        Else

            CurrentAnimation = AnimationList(iCurrentAnimation)
            CurrentAnimation.RResume()


            If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
                For Each eA In CurrentAnimation.Parellel_eAs
                    eA.RResume()
                Next
            End If
        End If




    End Sub

    Public Sub New()

    End Sub

    Public Sub New(AAnimationList As List(Of ePAnimation), LLoopable As Boolean)
        AnimationList.AddRange(AAnimationList)
        'AAnimationList.Clear()

        For Each eA In AnimationList
            eA.ePAnimationChain1 = Me

        Next


        Loopable = LLoopable
        CurrentAnimation = AnimationList(0)
        Done = False

    End Sub


    Public Sub Reset()
        CurrentAnimation = AnimationList(0)
        Done = False

        For Each eA In AnimationList
            eA.Reset()

            If Not IsNothing(eA.Parellel_eAs) Then
                For Each eA1 In eA.Parellel_eAs
                    eA1.Reset()
                Next
            End If
        Next

    End Sub

    Public Sub Start()
        CurrentAnimation.RResume()
        Playing = True

        'If Not IsNothing(Tool1) Then
        '    Tool1.eAChildRotationGradient = RotationGradient
        '    'Tool1.Owner.eAChildRotationGradient = RotationGradient
        '    Dim a = Me
        'End If





        If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
            For Each eA In CurrentAnimation.Parellel_eAs
                eA.RResume()
            Next
        End If


    End Sub

    Public Sub Pause()
        If Playing Then

            CurrentAnimation.Pause()
            Playing = False


            If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
                For Each eA In CurrentAnimation.Parellel_eAs
                    eA.Pause()
                Next
            End If

        End If
    End Sub

    Public Sub RResume()
        If Playing = False Then
            CurrentAnimation.RResume()
            Playing = True

            If Not IsNothing(CurrentAnimation.Parellel_eAs) Then
                For Each eA In CurrentAnimation.Parellel_eAs
                    eA.RResume()
                Next
            End If

        End If

    End Sub


	Public Event Completed(sender As ePAnimationChain, ByVal e As EventArgs)





	Public Function Clone(eeP As ePart) As ePAnimationChain
        Dim C As New ePAnimationChain()



        'AnimationList.AddRange(AAnimationList)

        For Each eA In AnimationList

            Dim NeweA = eA.Clone(eeP)
            NeweA.ePAnimationChain1 = Me
            NeweA.Axis.Owner_ePart = eeP

            NeweA.ChildRotationGradient = eA.ChildRotationGradient

            For Each PeA In NeweA.Parellel_eAs
                PeA.ePAnimationChain1 = Me
                PeA.Axis.Owner_ePart = eeP


            Next

            C.AnimationList.Add(NeweA)


        Next



        C.Loopable = Loopable


        C.iCurrentAnimation = 0
        C.CurrentAnimation = C.AnimationList(0)
        'C.RotationGradient = RotationGradient

        Return C
    End Function








End Class


Public Class ePAnimation


    Public Shared LstAnimation As New List(Of ePAnimation)

    Public eP As ePart

    Public DPos As Vector3 'Destination Position
    Public DAngle As Single  'Destination angle 
    Public CAngle As Single = 0 'Current angle
    Public Time As Single = 0.04
    Public Speed As Single
    Public Axis As Axis
    Public Name As String

    Public ChildRotationGradient As Single

    Public GotTrack As Boolean = False

    Public DAngleLst As New List(Of Single)
    Public TimeLst As New List(Of Single)
    Public CDAngleI As Integer = 0
    Public Looping As Boolean = False

    Public IsRefiningEdges As Boolean = False


    Public GetingTargetLstDir As Boolean = False 'T=CW  F=ACW

    Public ePAnimationChain1 As ePAnimationChain
    Public Parellel_eAs As New List(Of ePAnimation)

    Public IsFakeTimer As Boolean = False

    Public Playing As Boolean = False

    Public TotalCAngleChange As Single


    Public Sub Animate()
#Disable Warning BC42016 ' Implicit conversion

        For Each Child In eP.Children
            Child.eAChildRotationGradient = ChildRotationGradient
        Next

        Dim Completed As Boolean = False

        If DAngle >= 0 Then

            If Math.Abs(DAngle - CAngle) < 0.001 Then
                Completed = True
            End If

            If Speed > 0 Then
                If CAngle > DAngle Then
                    Completed = True
                End If
            Else
                If CAngle < DAngle Then
                    Completed = True
                End If
            End If


        ElseIf DAngle < 0 Then '-DAngle

            If Math.Abs(CAngle - DAngle) < 0.001 Then

                Completed = True
            End If

            If Speed < 0 Then
                If CAngle < DAngle Then
                    Completed = True
                End If
            Else
                If CAngle > DAngle Then
                    Completed = True
                End If
            End If

        End If


        If Completed Then
            eP.RotateAsAnimation(Axis, DAngle - CAngle)



            If IsRefiningEdges Then
                eP.Rotation = eP.Parent.ModelRotation * Matrix.CreateFromAxisAngle(Axis, DAngle)
            End If

            CAngle = DAngle

            If CDAngleI = DAngleLst.Count - 1 Then
                If Looping Then
                    CDAngleI = 0
                    DAngle = DAngleLst(CDAngleI)
                    Time = TimeLst(CDAngleI)
                    Speed = (DAngle - CAngle) / Time



                    'Next Animaton
                    If Not IsNothing(ePAnimationChain1) Then
                        ePAnimationChain1.Next_eA()
                    End If
                Else

                    'Next Animation
                    If Not IsNothing(ePAnimationChain1) Then
                        ePAnimationChain1.Next_eA()
                    End If
                    'Remove
                    Pause()
                    'eP.CurrentAnimations.Remove(Me)
                End If
            Else
                If GotTrack = False Then
                    GotTrack = True
                    'CDAngleI = 0
                    'DAngle = DAngleLst(CDAngleI)
                    'Time = TimeLst(CDAngleI)
                    'Speed = (DAngle) / Time

                Else

                    CDAngleI += 1


                    DAngle = DAngleLst(CDAngleI)
                    Time = TimeLst(CDAngleI)
                    Speed = (DAngle - CAngle) / Time

                End If
            End If
        End If




        If DAngle > CAngle Then
            If GetingTargetLstDir Then 'CW
                GetingTargetLstDir = False 'ACW

            Else 'ACW
                GetingTargetLstDir = True 'CW

            End If
        End If


        If GetingTargetLstDir = False Then
            'Anti ClockWise
            eP.RotateAsAnimation(Axis, Speed)
            CAngle += Speed
            TotalCAngleChange += Speed
        Else
            'ClockWise
            eP.RotateAsAnimation(Axis, Speed)
            CAngle += Speed
            TotalCAngleChange += Speed

        End If



#Enable Warning BC42016 ' Implicit conversion
    End Sub



    Public Sub Pause()
        If Playing Then
            ePAnimation.LstAnimation.Remove(Me)
            Playing = False
        End If


    End Sub

    Public Sub RResume()

        If Playing = False Then
            'eP.ChildRotationGradient = eAChildRotationGradient
            ePAnimation.LstAnimation.Add(Me)
            Playing = True

        End If
    End Sub


    'Public Sub Start()


    ' End Sub

    Public Sub Destroy()
        ePAnimation.LstAnimation.Remove(Me)
        eP.CurrentAnimations.Remove(Me)
    End Sub

    Public Sub New()

    End Sub



    Public Sub New(eeP As ePart, DDPos As Vector3, DDAngles() As Single, LLooping As Boolean, TTimes() As Single, AAxis As Axis)

        CAngle = 0
        CDAngleI = 0

        eP = eeP
        DPos = DDPos



        For Each DDAngle In DDAngles
            DAngleLst.Add(DDAngle)
        Next


        DAngle = DAngleLst(0)
        Looping = LLooping
        For Each TTime In TTimes
            TimeLst.Add(TTime)
        Next
        Time = TimeLst(0)
        Axis = AAxis



        eP.CurrentAnimations.Add(Me)



        Speed = (DAngle) / Time

        'ePAnimation.LstAnimation.Add(Me)


    End Sub


    Public Sub New(eeP As ePart, DDPos As Vector3, DDAngles() As Single, LLooping As Boolean, TTimes() As Single, AAxis As Axis, ChildRotationGradient As Single, RefineAnimationEdges As Boolean)

        CAngle = 0
        CDAngleI = 0

        eP = eeP
        DPos = DDPos

        Me.ChildRotationGradient = ChildRotationGradient

        IsRefiningEdges = RefineAnimationEdges

        For Each DDAngle In DDAngles
            DAngleLst.Add(DDAngle)
        Next


        DAngle = DAngleLst(0)
        Looping = LLooping
        For Each TTime In TTimes
            TimeLst.Add(TTime)
        Next
        Time = TimeLst(0)
        Axis = AAxis



        eP.CurrentAnimations.Add(Me)



        Speed = (DAngle) / Time

        'ePAnimation.LstAnimation.Add(Me)


    End Sub



    Public Sub New(eeP As ePart, DDPos As Vector3, DDAngles() As Single, LLooping As Boolean, TTimes() As Single, AAxis As Axis, ChildRotationGradient As Single)

        CAngle = 0
        CDAngleI = 0

        eP = eeP
        DPos = DDPos

        Me.ChildRotationGradient = ChildRotationGradient



        For Each DDAngle In DDAngles
            DAngleLst.Add(DDAngle)
        Next


        DAngle = DAngleLst(0)
        Looping = LLooping
        For Each TTime In TTimes
            TimeLst.Add(TTime)
        Next
        Time = TimeLst(0)
        Axis = AAxis



        eP.CurrentAnimations.Add(Me)



        Speed = (DAngle) / Time

        'ePAnimation.LstAnimation.Add(Me)


    End Sub



    'Public Sub RefineDestinationAngles()
    '    IsUsingDRotation = True

    '    DRotation = Matrix.CreateFromAxisAngle(Axis, DAngleLst.Last)


    'End Sub





    Public Sub Reset()
        Pause()

        CAngle = 0
        CDAngleI = 0
        DAngle = DAngleLst(0)
        Time = TimeLst(0)

        Speed = (DAngle) / Time
    End Sub


    Public Sub AddStep(DDAngle As Single, TTime As Single)
        DAngleLst.Add(DDAngle)
        TimeLst.Add(TTime)

    End Sub


    'Public Sub Revert(TTime As Single)
    '    Pause()

    '    eP.Rotation = eP.OriginalRotation
    '    eP.RelativePosition = eP.OriginalRelativePosition




    '    'Dim FakeBP As New ePart("FakeBP", Matrix.Identity, Vector3.One, eP.Parent)

    '    'Dim eAFBP As New ePAnimation(FakeBP, DPos, {(CAngle) * -1}, False, {TTime}, Axis)

    '    'Dim eALLeg As New ePAnimation(eP, DPos, {(CAngle) * -1}, False, {TTime}, Axis)

    '    'eAFBP.Parellel_eAs.Add(eALLeg)
    '    'Dim RevertingAnimation = New ePAnimationChain(New List(Of ePAnimation) From {eAFBP}, False)
    '    'RevertingAnimation.Reset_eAC_After_eA = Me

    '    'RevertingAnimation.Start()

    'End Sub





    Public Function Clone(eeP As ePart) As ePAnimation

        Dim o As ePAnimation


        If IsFakeTimer Then
            o = New ePAnimation(New ePart(New Entity(EntityTypes.WayPoint)), DPos, DAngleLst.ToArray, Looping, TimeLst.ToArray, Axis)
        Else
            o = New ePAnimation(eeP, DPos, DAngleLst.ToArray, Looping, TimeLst.ToArray, Axis)
        End If




        For Each Ani In Parellel_eAs
            Dim ClA = Ani.Clone(eeP)

            ClA.ChildRotationGradient = Ani.ChildRotationGradient

            o.Parellel_eAs.Add(ClA)

        Next


        Return o

    End Function


End Class


#End Region