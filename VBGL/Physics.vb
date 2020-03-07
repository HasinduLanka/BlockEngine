

Imports Microsoft.Xna.Framework


Public Module PhysicsFuncs




    Public CRND As New Random(RND.Next)

    Public Sub LookAtEntity(H As Entity, Target As Entity)

        Dim RotChange = Matrix.Invert(Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(H.Position, Target.Position, H.ModelRotation.Up))


        Dim RotChangeY = Matrix.Invert(Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(New Vector3(H.Position.X, Target.Position.Y, H.Position.Z), Target.Position, H.ModelRotation.Up))


        H.HeadRotation = RotChange
        H.NeededBodyRotation = RotChangeY
        H.NeededBodyRotationChanged = True


    End Sub


    Public Sub LookAtPosition(H As Entity, Target As Vector3)

        Dim RotChange = Matrix.Invert(Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(H.Position, Target, H.ModelRotation.Up))


        Dim RotChangeY = Matrix.Invert(Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(New Vector3(H.Position.X, Target.Y, H.Position.Z), Target, H.ModelRotation.Up))


        H.HeadRotation = RotChange
        H.NeededBodyRotation = RotChangeY
        H.NeededBodyRotationChanged = True


    End Sub


    Public Sub LookOutFromEntity(H As Entity, Target As Entity)

        Dim RotChange = (Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(H.Position, Target.Position, H.ModelRotation.Up))


        Dim RotChangeY = (Matrix.CreateTranslation(H.Position) * Matrix.CreateLookAt(New Vector3(H.Position.X, Target.Position.Y, H.Position.Z), Target.Position, H.ModelRotation.Up))


        H.HeadRotation = RotChange
        H.NeededBodyRotation = RotChangeY
        H.NeededBodyRotationChanged = True


    End Sub


    Public Function CreateLookAtPosition(Position As Vector3, Target As Vector3, Up As Vector3) As Matrix

        Return Matrix.Invert(Matrix.CreateTranslation(Position) * Matrix.CreateLookAt(Position, Target, Up))



    End Function





    Public Function RNDVec3(Max As Single, Min As Single) As Vector3
        Return New Vector3(CRND.Next(CInt(Min), CInt(Max)) + CSng(CRND.NextDouble), CRND.Next(CInt(Min), CInt(Max)) + CSng(CRND.NextDouble), CRND.Next(CInt(Min), CInt(Max)) + CSng(CRND.NextDouble))
    End Function


    Public Function RNDVec3(MaxX As Single, MinX As Single, MaxY As Single, MinY As Single, MaxZ As Single, MinZ As Single) As Vector3
        Return New Vector3(CRND.Next(CInt(MinX), CInt(MaxX)) + CSng(CRND.NextDouble), CRND.Next(CInt(MinY), CInt(MaxY)) + CSng(CRND.NextDouble), CRND.Next(CInt(MinZ), CInt(MaxZ)) + CSng(CRND.NextDouble))
    End Function

    Public Function RNDIntVec3(Max As Single, Min As Single) As Vector3
        Return New Vector3(CRND.Next(CInt(Min), CInt(Max + 1)), CRND.Next(CInt(Min), CInt(Max + 1)), CRND.Next(CInt(Min), CInt(Max + 1)))
    End Function






    Public Function RandomOf(Of T)(Choices As T()) As T

        Return Choices(CRND.Next(0, Choices.Count))


    End Function



    Public Function DivideAndTruncVector3(V As IntVector3, D As Single) As IntVector3
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
		Return New IntVector3(Int(V.X / D), Int(V.Y / D), Int(V.Z / D))
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
	End Function






End Module


Public Enum Direction
	Forward
	Backward
	Left
	Right
	Up
	Down
	None
End Enum




Public Structure IntVector3

	Public X As Integer
	Public Y As Integer
	Public Z As Integer


	Public Sub New(X As Integer, Y As Integer, Z As Integer)
		Me.X = X
		Me.Y = Y
		Me.Z = Z
	End Sub

	Public Sub New(AllXYZAs As Integer)
		X = AllXYZAs
		Y = AllXYZAs
		Z = AllXYZAs
	End Sub




	Public Shared Operator *(ByVal L As IntVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X * R.X, L.Y * R.Y, L.Z * R.Z)
	End Operator


	Public Shared Operator *(ByVal L As IntVector3, ByVal R As Integer) As IntVector3
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
		Return New IntVector3(Math.Truncate(L.X * R), Math.Truncate(L.Y * R), Math.Truncate(L.Z * R))
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Decimal' to 'Integer'.
	End Operator
	Public Shared Operator /(ByVal L As IntVector3, ByVal R As Integer) As IntVector3
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
		Return New IntVector3(Math.Truncate(L.X / R), Math.Truncate(L.Y / R), Math.Truncate(L.Z / R))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
	End Operator



	Public Shared Operator +(ByVal L As IntVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X + R.X, L.Y + R.Y, L.Z + R.Z)
	End Operator

	Public Shared Operator -(ByVal L As IntVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X - R.X, L.Y - R.Y, L.Z - R.Z)
	End Operator
	Public Shared Operator /(ByVal L As IntVector3, ByVal R As IntVector3) As IntVector3
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
		Return New IntVector3(Math.Truncate(L.X / R.X), Math.Truncate(L.Y / R.Y), Math.Truncate(L.Z / R.Z))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
	End Operator

	Public Shared Operator Mod(ByVal L As IntVector3, ByVal R As Integer) As IntVector3
		Return New IntVector3(L.X Mod R, L.Y Mod R, L.Z Mod R)
	End Operator


	Public Shared Widening Operator CType(v As IntVector3) As Vector3
		Return New Vector3(v.X, v.Y, v.Z)
	End Operator




	Public Shared Function FromV3Truncated(V3 As Vector3) As IntVector3
#Disable Warning BC42016 ' Implicit conversion
		Return New IntVector3(Math.Truncate(V3.X), Math.Truncate(V3.Y), Math.Truncate(V3.Z))
#Enable Warning BC42016 ' Implicit conversion
	End Function

	Public Shared Function TruncateV3(V3 As Vector3) As Vector3
#Disable Warning BC42016 ' Implicit conversion 
		Return New Vector3(Math.Truncate(V3.X), Math.Truncate(V3.Y), Math.Truncate(V3.Z))
#Enable Warning BC42016 ' Implicit conversion
	End Function

	Public Shared Function RoundV3(V3 As Vector3) As Vector3
#Disable Warning BC42016 ' Implicit conversion 
		Return New Vector3(Math.Round(V3.X), Math.Round(V3.Y), Math.Round(V3.Z))

	End Function

	Public Shared Function FromV3Rounded(V3 As Vector3) As IntVector3
#Disable Warning BC42016 ' Implicit conversion
		Return New IntVector3(Math.Round(V3.X), Math.Round(V3.Y), Math.Round(V3.Z))
#Enable Warning BC42016 ' Implicit conversion
	End Function

	Public Shared Function FromV3Rounded(X As Single, Y As Single, Z As Single) As IntVector3

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
		Return New IntVector3(Math.Round(X), Math.Round(Y), Math.Round(Z))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion
	End Function

	'Public Shared Widening Operator CType(v As Vector3) As IntVector3
	'    Throw New NotImplementedException()
	'End Operator

	Public Overrides Function ToString() As String
		Return "X=" + X.ToString + ", Y=" + Y.ToString + ", Z=" + Z.ToString
	End Function


	Public Shared Operator <>(ByVal left As IntVector3, ByVal right As IntVector3) As Boolean
		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return False
		Else
			Return True
		End If

	End Operator


	Public Shared Operator =(ByVal left As IntVector3, ByVal right As IntVector3) As Boolean

		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return True
		Else
			Return False
		End If
	End Operator

	Public Overrides Function Equals(obj As Object) As Boolean
		Dim R = DirectCast(obj, IntVector3)
		If X = R.X AndAlso Y = R.Y AndAlso Z = R.Z Then
			Return True
		Else
			Return False
		End If
	End Function



	Public Shared Operator <>(ByVal left As IntVector3, ByVal right As BVector3) As Boolean
		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return False
		Else
			Return True
		End If

	End Operator

	Public Shared Operator =(ByVal left As IntVector3, ByVal right As BVector3) As Boolean

		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return True
		Else
			Return False
		End If
	End Operator








	Public Shared Up As New IntVector3(0, 1, 0)
	Public Shared Down As New IntVector3(0, -1, 0)

	Public Shared Right As New IntVector3(1, 0, 0)
	Public Shared Left As New IntVector3(-1, 0, 0)

	Public Shared Back As New IntVector3(0, 0, 1)
	Public Shared Foward As New IntVector3(0, 0, -1)


	Public Shared Zero As New IntVector3(0, 0, 0)
	Public Shared One As New IntVector3(1, 1, 1)
	Public Shared MinusOne As New IntVector3(-1, -1, -1)

End Structure


Public Class BVector3
	Public X As Byte
	Public Y As Byte
	Public Z As Byte

	Public Sub New()
	End Sub

	Public Sub New(X As Byte, Y As Byte, Z As Byte)
		Me.X = X
		Me.Y = Y
		Me.Z = Z
	End Sub

	Public Sub New(AllXYZAs As Byte)
		X = AllXYZAs
		Y = AllXYZAs
		Z = AllXYZAs
	End Sub




	Public Shared Operator *(ByVal L As BVector3, ByVal R As BVector3) As BVector3
		Return New BVector3(L.X * R.X, L.Y * R.Y, L.Z * R.Z)
	End Operator

	Public Shared Operator *(ByVal L As BVector3, ByVal R As Integer) As BVector3

#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
		Return New BVector3(L.X * R, L.Y * R, L.Z * R)
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
	End Operator


	Public Shared Operator +(ByVal L As BVector3, ByVal R As BVector3) As BVector3
		Return New BVector3(L.X + R.X, L.Y + R.Y, L.Z + R.Z)
	End Operator

	Public Shared Operator -(ByVal L As BVector3, ByVal R As BVector3) As BVector3
		Return New BVector3(L.X - R.X, L.Y - R.Y, L.Z - R.Z)
	End Operator
	Public Shared Operator /(ByVal L As BVector3, ByVal R As BVector3) As BVector3
		Return New BVector3(CByte(L.X / R.X), CByte(L.Y / R.Y), CByte(L.Z / R.Z))
	End Operator




	Public Shared Operator *(ByVal L As BVector3, ByVal R As Vector3) As BVector3
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
		Return New BVector3(L.X * R.X, L.Y * R.Y, L.Z * R.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
	End Operator

	Public Shared Operator +(ByVal L As BVector3, ByVal R As Vector3) As BVector3
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
		Return New BVector3(L.X + R.X, L.Y + R.Y, L.Z + R.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
	End Operator

	Public Shared Operator -(ByVal L As BVector3, ByVal R As Vector3) As BVector3
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
		Return New BVector3(L.X - R.X, L.Y - R.Y, L.Z - R.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
	End Operator
	Public Shared Operator /(ByVal L As BVector3, ByVal R As Vector3) As BVector3
		Return New BVector3(CByte(L.X / R.X), CByte(L.Y / R.Y), CByte(L.Z / R.Z))
	End Operator




	Public Shared Operator *(ByVal L As BVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X * R.X, L.Y * R.Y, L.Z * R.Z)
	End Operator

	Public Shared Operator +(ByVal L As BVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X + R.X, L.Y + R.Y, L.Z + R.Z)
	End Operator

	Public Shared Operator -(ByVal L As BVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(L.X - R.X, L.Y - R.Y, L.Z - R.Z)
	End Operator
	Public Shared Operator /(ByVal L As BVector3, ByVal R As IntVector3) As IntVector3
		Return New IntVector3(CByte(L.X / R.X), CByte(L.Y / R.Y), CByte(L.Z / R.Z))
	End Operator





	Public Shared Widening Operator CType(v As BVector3) As IntVector3
		Return New IntVector3(v.X, v.Y, v.Z)
	End Operator
	Public Shared Widening Operator CType(v As BVector3) As Vector3
		Return New Vector3(v.X, v.Y, v.Z)
	End Operator

	Public Shared Widening Operator CType(v As IntVector3) As BVector3
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
		Return New BVector3(v.X, v.Y, v.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Integer' to 'Byte'.
	End Operator

	Public Shared Widening Operator CType(v As Vector3) As BVector3
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
		Return New BVector3(v.X, v.Y, v.Z)
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Byte'.
	End Operator


#Disable Warning BC42016 ' Implicit conversion
	Public Shared Function FromV3Truncated(V3 As Vector3) As BVector3
		Return New BVector3(Math.Truncate(V3.X), Math.Truncate(V3.Y), Math.Truncate(V3.Z))
	End Function


	Public Shared Function FromV3Rounded(V3 As Vector3) As BVector3
		Return New BVector3(Math.Round(V3.X), Math.Round(V3.Y), Math.Round(V3.Z))
	End Function


	Public Shared Function FromIntV3(V3 As IntVector3) As BVector3
		Return New BVector3(CByte(V3.X), CByte(V3.Y), CByte(V3.Z))
	End Function



#Enable Warning BC42016 ' Implicit conversion


	Public Overrides Function ToString() As String
		Return "X=" + X.ToString + ", Y=" + Y.ToString + ", Z=" + Z.ToString
	End Function


	Public Shared Operator <>(ByVal left As BVector3, ByVal right As BVector3) As Boolean
		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return False
		Else
			Return True
		End If

	End Operator

	Public Shared Operator =(ByVal left As BVector3, ByVal right As BVector3) As Boolean

		If left.X = right.X AndAlso left.Y = right.Y AndAlso left.Z = right.Z Then
			Return True
		Else
			Return False
		End If
	End Operator



	Public Overrides Function Equals(obj As Object) As Boolean
		Dim R = DirectCast(obj, BVector3)
		If X = R.X AndAlso Y = R.Y AndAlso Z = R.Z Then
			Return True
		Else
			Return False
		End If
	End Function



End Class


Public Enum RacingCameraAngle
	Back
	Inside
End Enum





Public Module Funcs


	Public RND As New Random

	Public Function RndPosition(min As Vector2, max As Vector2) As Vector2
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Disable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
		Return New Vector2(RND.Next(min.X, max.X), RND.Next(min.Y, max.Y))
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
#Enable Warning BC42016 ' Implicit conversion from 'Single' to 'Integer'.
	End Function


	Public Function VLerp(A As Vector2, B As Vector2, Amount As Single) As Vector2
		Return A + ((B - A) * Amount)
	End Function

#Disable Warning BC42016 ' Implicit conversion 
	Public Function TruncateV2(V2 As Vector2) As Vector2
		Return New Vector2(Math.Truncate(V2.X), Math.Truncate(V2.Y))
	End Function

	Public Function RoundV2(V2 As Vector2) As Vector2
		Return New Vector2(Math.Round(V2.X), Math.Round(V2.Y))
	End Function
#Enable Warning BC42016 ' Implicit conversion


	Public Function ListDif(F As Integer, L As Integer, nOfVal As Integer) As Integer()
		Dim Out(nOfVal - 1) As Integer
		Dim FLDif = L - F
		Dim Dif = FLDif / nOfVal

		For X = 1 To nOfVal '- 1
			Out(X - 1) = F + CInt(X * Dif)

			'Out(X - 1) = (L * (X / nOfVal)) + (F * (1 - (X / nOfVal)))


		Next

		Return Out
	End Function


	Public Function ListDifSqured(F As Integer, L As Integer, nOfVal As Integer) As Integer()
		Dim Out(nOfVal - 1) As Integer
		Dim FLDif = L - F
		Dim Dif = FLDif / nOfVal

		For X = 1 To nOfVal '- 1
			'Out(X - 1) = F + CInt(X * Dif)

			'Out(X - 1) = (L * (X / nOfVal)) + (F * (1 - (X / nOfVal)))

#Disable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.
			Out(X - 1) = Math.Sqrt((L * L * (X / nOfVal)) + (F * F * (1 - (X / nOfVal))))
#Enable Warning BC42016 ' Implicit conversion from 'Double' to 'Integer'.

		Next

        Return Out
    End Function


    Public Function Contains(A() As IntVector3, Count As Integer, Item As IntVector3) As Boolean

        For n = 0 To Count - 1
            If A(n) = Item Then
                Return True
            End If
        Next

        Return False
    End Function
    Public Function Contains(A As List(Of IntVector3), Item As IntVector3) As Boolean

        For n = 0 To A.Count - 1
            If A(n) = Item Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Function Contains(A() As Chunk, Count As Integer, Item As Chunk) As Boolean

        For n = 0 To Count - 1
            If A(n) = Item Then
                Return True
            End If
        Next

        Return False
    End Function

End Module







Public Class Physics

    Public Shared YZero As New Vector3(1, 0, 1)
    Public Shared HalfXYAndYZero As New Vector3(0.5, 0, 0.5)

    Public Shared Function FindStandedVec3OfDirection(D As Vector3) As Vector3
        Dim X = D.X
        Dim Y = D.Y
        Dim Z = D.Z

        Dim aX = Math.Abs(D.X)
        Dim aY = Math.Abs(D.Y)
        Dim aZ = Math.Abs(D.Z)

        Dim O As New Vector3

        If aX > aY Then
            If aZ > aX Then 'Z
                If Z > 0 Then
                    O = Vector3.Backward
                Else
                    O = Vector3.Forward
                End If

            ElseIf aX > aZ Then 'X
                If X > 0 Then
                    O = Vector3.Right
                Else
                    O = Vector3.Left
                End If
            End If

        Else 'Y>

            If aZ > aY Then 'Z
                If Z > 0 Then
                    O = Vector3.Backward
                Else
                    O = Vector3.Forward
                End If

            ElseIf aY > aZ Then 'Y
                If Y > 0 Then
                    O = Vector3.Up
                Else
                    O = Vector3.Down
                End If
            End If

        End If

        Return O
    End Function

    Public Shared Function FindUnitDirectionOfDirection(D As Vector3) As Direction
        Dim X = D.X
        Dim Y = D.Y
        Dim Z = D.Z

        Dim aX = Math.Abs(D.X)
        Dim aY = Math.Abs(D.Y)
        Dim aZ = Math.Abs(D.Z)

        Dim O As New Direction

        If aX > aY Then
            If aZ > aX Then 'Z
                If Z > 0 Then
                    O = Direction.Backward
                Else
                    O = Direction.Forward
                End If

            ElseIf aX > aZ Then 'X
                If X > 0 Then
                    O = Direction.Right
                Else
                    O = Direction.Left
                End If
            End If

        Else 'Y>

            If aZ > aY Then 'Z
                If Z > 0 Then
                    O = Direction.Backward
                Else
                    O = Direction.Forward
                End If

            ElseIf aY > aZ Then 'Y
                If Y > 0 Then
                    O = Direction.Up
                Else
                    O = Direction.Down
                End If
            End If

        End If

        Return O
    End Function

    Public Shared Function Find2DUnitDirectionOfDirection(D As Vector3) As Direction
        Dim X = D.X
        Dim Z = D.Z

        Dim aX = Math.Abs(D.X)
        Dim aZ = Math.Abs(D.Z)

        Dim O As New Direction


        If aZ > aX Then 'Z
            If Z > 0 Then
                O = Direction.Backward
            Else
                O = Direction.Forward
            End If

        ElseIf aX > aZ Then 'X
            If X > 0 Then
                O = Direction.Right
            Else
                O = Direction.Left
            End If
        End If



        Return O
    End Function

    ''' <summary>
    ''' 0 = Backward or Forward, 1 = Right or Left
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Find2DDualDirectionsOfDirection(D As Vector3) As Direction()
        Dim X = D.X
        Dim Z = D.Z

        Dim O(1) As Direction

        If Z > 0 Then
            O(0) = Direction.Backward
        Else
            O(0) = Direction.Forward

        End If

        'If Z > 0 Then
        '    O(0) = Direction.Backward
        'ElseIf Z < 0 Then
        '    O(0) = Direction.Forward
        'Else
        '    O(0) = Direction.None
        'End If

        If X > 0 Then
            O(1) = Direction.Right
        Else
            O(1) = Direction.Left
        End If

        'If X > 0 Then
        '    O(1) = Direction.Right
        'ElseIf X < 0 Then
        '    O(1) = Direction.Left
        'Else
        '    O(1) = Direction.None
        'End If

        Return O
    End Function


    Public Shared Function GetAxisByChar(Rotaion As Matrix, C As Char) As Vector3
        Dim Direction As Vector3

        If C = "D" Then
            Direction = Rotaion.Down
        ElseIf C = "U" Then
            Direction = Rotaion.Up
        ElseIf C = "F" Then
            Direction = Rotaion.Forward
        ElseIf C = "B" Then
            Direction = Rotaion.Backward
        ElseIf C = "L" Then
            Direction = Rotaion.Left
        ElseIf C = "R" Then
            Direction = Rotaion.Right
        End If

        Return Direction
    End Function


    Public Shared Function GetAxisByChar(Rotaion As Matrix, C As Char, RelativePos As Vector3) As Vector3
        Dim Direction As Vector3

        If C = "D" Then
            Direction = Rotaion.Down
        ElseIf C = "U" Then
            Direction = Rotaion.Up
        ElseIf C = "F" Then
            Direction = Rotaion.Forward
        ElseIf C = "B" Then
            Direction = Rotaion.Backward
        ElseIf C = "L" Then
            Direction = Rotaion.Left

        ElseIf C = "R" Then
            Direction = Rotaion.Right

        End If


        Direction += Vector3.Transform(RelativePos, Rotaion)


        Return Direction
    End Function




    Public Shared Function GetBigerToOne(V As Vector3) As Vector3

        V.Normalize()

        Dim X = V.X
        Dim Y = V.Y
        Dim Z = V.Z

        Dim aX = Math.Abs(V.X)
        Dim aY = Math.Abs(V.Y)
        Dim aZ = Math.Abs(V.Z)

        Dim O As New Vector3

        If aX > aY Then
            If aZ > aX Then 'Z
                If Z > 0 Then
                    'O = Vector3.Backward
                    O.X = (1 / Z) * X
                    O.Y = (1 / Z) * Y

                    O.Z = 1



                Else
                    'O = Vector3.Forward
                    O.X = (-1 / Z) * X
                    O.Y = (-1 / Z) * Y

                    O.Z = -1
                End If

            ElseIf aX > aZ Then 'X
                If X > 0 Then
                    'O = Vector3.Right
                    O.Z = (1 / X) * Z
                    O.Y = (1 / X) * Y

                    O.X = 1
                Else
                    'O = Vector3.Left
                    O.Z = (-1 / X) * Z
                    O.Y = (-1 / X) * Y

                    O.X = -1
                End If
            End If

        Else 'Y>

            If aZ > aY Then 'Z
                If Z > 0 Then
                    'O = Vector3.Backward
                    O.X = (1 / Z) * X
                    O.Y = (1 / Z) * Y

                    O.Z = 1
                Else
                    'O = Vector3.Forward
                    O.X = (-1 / Z) * X
                    O.Y = (-1 / Z) * Y

                    O.Z = -1
                End If

            ElseIf aY > aZ Then 'Y
                If Y > 0 Then
                    'O = Vector3.Up
                    O.X = (1 / Y) * X
                    O.Z = (1 / Y) * Z

                    O.Y = 1
                Else
                    'O = Vector3.Down
                    O.X = (-1 / Y) * X
                    O.Z = (-1 / Y) * Z

                    O.Y = -1
                End If
            End If

        End If


        Return O

    End Function



End Class


