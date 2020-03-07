
#Region "eCollitions"



Imports Microsoft.Xna.Framework

Public Class eCollitionHierarchy
    'Public Shared eCollitionHierarchies As New List(Of eCollitionHierarchy)

    Public Owner As Entity
    Public UpperMostNodes As New List(Of eCollitionNode)

    Public AllNodes As New List(Of eCollitionNode)

    Public Sub AddNode(Node As eCollitionNode)
        Node.CollitionHierarchy = Me
        AllNodes.Add(Node)
        UpperMostNodes.Add(Node)
    End Sub

    Public Sub UpdateAllSpheres(Pos As Vector3)
        For Each Node In AllNodes
            If Node.Type1 = eCollitionNode.Type.BoundingSphere Then
                Node.Update(Pos)
            End If
        Next
    End Sub

    Public Sub New(OOWner As Entity)
        'eCollitionHierarchies.Add(Me)
        Owner = OOWner
    End Sub




    Public Function IsCollided(BS As BoundingSphere) As Boolean
        Dim IsC = False
        For Each N In UpperMostNodes
            If N.IsCollided(BS) Then
                IsC = True
            End If
        Next

        Return IsC
    End Function

    Public Function IsCollided(R As Ray, RL As Single) As Boolean
        Dim IsC = False
        For Each N In UpperMostNodes
            If N.IsCollided(R, RL) Then
                IsC = True
            End If
        Next

        Return IsC
    End Function


    Public Function GetCollided(R As Ray, RL As Single) As eCollitionNode
        Dim IsC As eCollitionNode = Nothing

        For Each N In UpperMostNodes
            Dim CCN = N.GetCollided(R, RL)
            If Not IsNothing(CCN) Then
                IsC = CCN
            End If

        Next

        Return IsC
    End Function






    Public Shared Function CreateNewHierarchyForArrow(A1 As Arrow) As eCollitionHierarchy
        A1.CollitionHierarchy = New eCollitionHierarchy(A1)

        Dim CN = New eCollitionNode(Vector3.Zero, 10.0F, New ePart(A1))
        A1.CollitionHierarchy.AddNode(CN)


        Return A1.CollitionHierarchy
    End Function




    Public Shared Function CreateNewHumanHierarchy(H1 As Human) As eCollitionHierarchy
        H1.CollitionHierarchy = New eCollitionHierarchy(H1)

        Dim CN = New eCollitionNode(New Vector3(0, 76.0F, 0), 80.0F, New ePart(H1))

        H1.CollitionHierarchy.AddNode(CN)
        CN.AddLowerNode(New eCollitionNode(New Vector3(0, 164, 0), 15, H1.BodyParts(Human.iHead)) With
                        {.RRewardMultiplier = New RewardSetMultiplier(4, 1, 1)}) 'Head


        Dim BodyNode = New eCollitionNode(New Vector3(0, 125, 0), 25, H1.BodyParts(Human.iBody))

        CN.AddLowerNode(BodyNode)

        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(10, 140, 2), 10, H1.BodyParts(Human.iBody))) 'R Chest
        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 140, 2), 10, H1.BodyParts(Human.iBody))) 'L Chest

        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(9, 130, 1), 10, H1.BodyParts(Human.iBody))) 'RB Chest
        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(-9, 130, 1), 10, H1.BodyParts(Human.iBody))) 'LB Chest
        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(0, 130, 0), 10, H1.BodyParts(Human.iBody))) 'MB Chest


        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(6, 120, -3), 9, H1.BodyParts(Human.iBody))) 'RU Stomach
        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(-6, 120, -3), 9, H1.BodyParts(Human.iBody))) 'LU Stomach


        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(5, 110, -3), 9, H1.BodyParts(Human.iBody))) 'R Stomach
        BodyNode.AddLowerNode(New eCollitionNode(New Vector3(-5, 110, -3), 9, H1.BodyParts(Human.iBody))) 'L Stomach



        Dim LegsNode = New eCollitionNode(New Vector3(0, 50, 0), 35, H1.BodyParts(Human.iLLeg))

        CN.AddLowerNode(LegsNode)


        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(0, 95, -1), 12, H1.BodyParts(Human.iLLeg))) 'Hips


        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(10, 78, -2), 7, H1.BodyParts(Human.iLLeg))) 'L Leg1
        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 78, -2), 7, H1.BodyParts(Human.iRLeg))) 'R Leg1


        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(10, 63, -2), 7, H1.BodyParts(Human.iLLeg))) 'L Leg1
        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 63, -2), 7, H1.BodyParts(Human.iRLeg))) 'R Leg1

        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(10, 48, 0), 7, H1.BodyParts(Human.iLLeg))) 'L Leg2
        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 48, 0), 7, H1.BodyParts(Human.iRLeg))) 'R Leg2

        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(10, 33, 2), 7, H1.BodyParts(Human.iLKnee))) 'L Leg2
        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 33, 2), 7, H1.BodyParts(Human.iRKnee))) 'R Leg2

        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(10, 18, 0), 7, H1.BodyParts(Human.iLKnee))) 'L Leg2
        LegsNode.AddLowerNode(New eCollitionNode(New Vector3(-10, 18, 0), 7, H1.BodyParts(Human.iRKnee))) 'R Leg2


        H1.CollitionHierarchy.UpdateAllSpheres(H1.Position)




        Return H1.CollitionHierarchy
    End Function




End Class

Public Class eCollitionNode
    Public Type1 As Type

    Public BS As BoundingSphere
    Public BSR As Single
    Public BB As BoundingBox
    Public BF As BoundingFrustum
    Public RR As Ray
    Public RRLength As Single
    Public PL As Plane

    Public CollitionHierarchy As eCollitionHierarchy
    Public UpperNode As eCollitionNode
    Public LowerNodes As New List(Of eCollitionNode)


    Public eP As ePart

    'Public B As New Bullet
    Public RRewardMultiplier As New RewardSetMultiplier(1, 1, 1)

    ''' <summary>
    ''' Bounding Spheare node
    ''' </summary>
    ''' 
    Public RealativePos As Vector3
    Public Sub New(RealativePos As Vector3, R As Single, eP As ePart)
        Type1 = Type.BoundingSphere
        BS = New BoundingSphere(RealativePos, R)
        Me.RealativePos = RealativePos
        BSR = R
        Me.eP = eP
    End Sub



    ''' <summary>
    ''' Bounding Box node
    ''' </summary>
    ''' 
    Public RealativePosMin As Vector3
    Public RealativePosMax As Vector3
    Public Sub New(Min As Vector3, Max As Vector3, eP As ePart)
        Type1 = Type.BoundingBox
        BB = New BoundingBox(Min, Max)
        RealativePosMax = Max
        RealativePosMin = Min
        Me.eP = eP
    End Sub

    ''' <summary>
    ''' Ray node
    ''' </summary>
    Public Sub New(Start As Vector3, Direction As Vector3, KeepThisTrueForRays As Boolean, eP As ePart)
        Type1 = Type.Ray
        RR = New Ray(Start, Direction)
        RealativePos = Start
        Me.eP = eP
    End Sub

    ''' <summary>
    ''' Bounding Fustrum node
    ''' </summary>
    Public Sub New(M As Matrix, eP As ePart)
        Type1 = Type.BoundingFrustum
        BF = New BoundingFrustum(M)
        Me.eP = eP
    End Sub

    ''' <summary>
    ''' Plane node
    ''' </summary>
    Public RP1 As Vector3
    Public RP2 As Vector3
    Public RP3 As Vector3
    Public Sub New(P1 As Vector3, P2 As Vector3, P3 As Vector3, eP As ePart)
        Type1 = Type.Plane
        PL = New Plane(P1, P2, P3)
        RP1 = P1
        RP2 = P2
        RP3 = P3
        Me.eP = eP
    End Sub


    Public Enum Type
        BoundingSphere
        BoundingBox
        Ray
        BoundingFrustum
        Plane
    End Enum



    Public Sub AddLowerNode(LN As eCollitionNode)

        LN.CollitionHierarchy = CollitionHierarchy
        CollitionHierarchy.AllNodes.Add(LN)
        LN.UpperNode = Me

        LowerNodes.Add(LN)
    End Sub






    Public Function IsCollided(BBS As BoundingSphere) As Boolean
        Dim IsC As Boolean = False

        If Type1 = Type.BoundingSphere Then
            If BS.Intersects(BBS) Then

                If LowerNodes.Count > 0 Then
                    For Each LN In LowerNodes
                        If LN.IsCollided(BBS) Then
                            IsC = True
                        End If
                    Next
                Else
                    IsC = True
                End If

            Else
                IsC = False
            End If

        ElseIf Type1 = Type.BoundingBox Then
            If BB.Intersects(BBS) Then
                IsC = True
            Else
                IsC = False
            End If

        ElseIf Type1 = Type.Ray Then
            If RR.Intersects(BBS).HasValue Then

                If LowerNodes.Count > 0 Then
                    For Each LN In LowerNodes
                        If LN.IsCollided(BBS) Then
                            IsC = True
                        End If
                    Next
                Else
                    IsC = True
                End If

            Else
                IsC = False
            End If
        ElseIf Type1 = Type.BoundingFrustum Then
            If BF.Intersects(BBS) Then
                IsC = True
            Else
                IsC = False
            End If
        ElseIf Type1 = Type.Plane Then
            If PL.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
                IsC = True
            Else
                IsC = False
            End If

        Else
            IsC = False
        End If




        Return IsC
    End Function


    Public Function IsCollided(BBS As BoundingBox) As Boolean
        If Type1 = Type.BoundingSphere Then
            If BS.Intersects(BBS) Then
                Return True
            Else
                Return False
            End If

        ElseIf Type1 = Type.BoundingBox Then
            If BB.Intersects(BBS) Then
                Return True
            Else
                Return False
            End If

        ElseIf Type1 = Type.Ray Then
            If RR.Intersects(BBS).HasValue Then
                Return True
            Else
                Return False
            End If
        ElseIf Type1 = Type.BoundingFrustum Then
            If BF.Intersects(BBS) Then
                Return True
            Else
                Return False
            End If
        ElseIf Type1 = Type.Plane Then
            If PL.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
                Return True
            Else
                Return False
            End If

        Else
            Return False
        End If





    End Function

    'Public Function IsCollided(BBS As BoundingFrustum) As Boolean
    '    If Type1 = Type.BoundingSphere Then
    '        If BS.Intersects(BBS) Then
    '            Return True
    '        Else
    '            Return False
    '        End If

    '    ElseIf Type1 = Type.BoundingBox Then
    '        If BB.Intersects(BBS) Then
    '            Return True
    '        Else
    '            Return False
    '        End If

    '    ElseIf Type1 = Type.Ray Then
    '        If RR.Intersects(BBS).HasValue Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    ElseIf Type1 = Type.BoundingFrustum Then
    '        If BF.Intersects(BBS) Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    ElseIf Type1 = Type.Plane Then
    '        If PL.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
    '            Return True
    '        Else
    '            Return False
    '        End If

    '    Else
    '        Return False
    '    End If





    'End Function

    Public Function IsCollided(BBS As Ray, RL As Single) As Boolean
        Dim IsC As Boolean = False


        If Type1 = Type.BoundingSphere Then
            If BS.Intersects(BBS).HasValue Then
                If Vector3.Distance(BS.Center, BBS.Position) < RL Then

                    If LowerNodes.Count > 0 Then
                        For Each LN In LowerNodes

                            If LN.IsCollided(BBS, RL) Then
                                IsC = True
                            End If
                        Next
                    Else
                        IsC = True
                    End If

                End If
            Else
                IsC = False
            End If

        ElseIf Type1 = Type.BoundingBox Then
            If BB.Intersects(BBS).HasValue Then
                IsC = True
            Else
                IsC = False
            End If

        ElseIf Type1 = Type.Ray Then
            IsC = False

        ElseIf Type1 = Type.BoundingFrustum Then
            If BF.Intersects(BBS).HasValue Then
                IsC = True
            Else
                IsC = False
            End If
        ElseIf Type1 = Type.Plane Then
            IsC = False
        Else
            IsC = False
        End If



        Return IsC

    End Function

    Public Function GetCollided(BBS As Ray, RL As Single) As eCollitionNode
        Dim IsC As eCollitionNode = Nothing


        If Type1 = Type.BoundingSphere Then
            If BS.Intersects(BBS).HasValue Then
                If Vector3.Distance(BS.Center, BBS.Position) < RL Then

                    If LowerNodes.Count > 0 Then
                        For Each LN In LowerNodes
                            Dim CCN = LN.GetCollided(BBS, RL)
                            If Not IsNothing(CCN) Then
                                IsC = CCN
                            End If
                        Next
                    Else
                        IsC = Me
                    End If

                End If
            Else
                IsC = Nothing
            End If

        ElseIf Type1 = Type.BoundingBox Then
            If BB.Intersects(BBS).HasValue Then
                IsC = Me
            Else
                IsC = Nothing
            End If

        ElseIf Type1 = Type.Ray Then
            IsC = Nothing

        ElseIf Type1 = Type.BoundingFrustum Then
            If BF.Intersects(BBS).HasValue Then
                IsC = Me
            Else
                IsC = Nothing
            End If
        ElseIf Type1 = Type.Plane Then
            IsC = Me
        Else
            IsC = Nothing
        End If



        Return IsC



    End Function


    Public Function IsCollided(BBS As Plane) As Boolean
        Dim IsC As Boolean = False
        If Type1 = Type.BoundingSphere Then
            If BS.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
                IsC = True
            Else
                IsC = False
            End If

        ElseIf Type1 = Type.BoundingBox Then
            If BB.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
                IsC = True
            Else
                IsC = False
            End If

        ElseIf Type1 = Type.Ray Then
            If RR.Intersects(BBS).HasValue Then
                IsC = True
            Else
                IsC = False
            End If
        ElseIf Type1 = Type.BoundingFrustum Then
            If BF.Intersects(BBS) = PlaneIntersectionType.Intersecting Then
                IsC = True
            Else
                IsC = False
            End If
        ElseIf Type1 = Type.Plane Then
            IsC = False

        Else
            IsC = False
        End If



        Return IsC

    End Function




    ''' <summary>
    ''' Bounding Spheare node
    ''' </summary>
    Public Sub Update(Pos As Vector3)
        BS = New BoundingSphere(Pos + RealativePos, BSR)

    End Sub

    ''' <summary>
    ''' Bounding Box node
    ''' </summary>
    Public Sub Update(Min As Vector3, Max As Vector3)
        BB = New BoundingBox(Min + RealativePosMin, Max + RealativePosMax)
    End Sub

    ''' <summary> 
    ''' Ray node 
    ''' </summary>
    Public Sub Update(Start As Vector3, KeepThisTrueForRays As Boolean)
        RR = New Ray(Start + RealativePos, RR.Direction)
    End Sub

    ''' <summary>
    ''' Bounding Fustrum node
    ''' </summary>
    Public Sub Update(M As Matrix)
        BF = New BoundingFrustum(M)
    End Sub

    ''' <summary>
    ''' Plane node
    ''' </summary>
    Public Sub Update(P1 As Vector3, P2 As Vector3, P3 As Vector3)
        PL = New Plane(P1, P2, P3)
    End Sub


End Class



#End Region