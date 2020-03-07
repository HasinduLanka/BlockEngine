


'Imports System.Collections.Generic
'    Imports System.Text
'    Imports Microsoft.Xna.Framework
'    Imports Microsoft.Xna.Framework.Content
'    Imports Microsoft.Xna.Framework.Graphics
'    Imports Microsoft.Xna.Framework.Input


'Public Class Bullet

'    Public BoundingSphereScale As Single = 1.0F
'    Public BoundingSphereRadius As Single = 10.0F
'    Public MBoundingSphere As BoundingSphere

'    Public Model As Model
'    Public Transforms As Matrix()
'    Public Scale As Matrix
'    'Public Transforms2 As Matrix()

'    Public Health As Integer = 10
'    Public HitDamage As Integer = 10

'    Public Direction As New Vector3

'    'Position of the model in world space
'    Public Position As Vector3 = Vector3.Zero

'    'Velocity of the model, applied each frame to the model's position
'    Public Velocity As Vector3 = Vector3.Zero

'    Public ModelRotation As Matrix = Matrix.Identity
'    Private Mrotation As Single

'    Public ModelVelocity As Single

'    Public Property ControlsList As New List(Of Controls.Control)

'    Public Owner As Player

'    Public OldRndP As Integer = 1

'    Public ModelRotationY As Matrix = Matrix.Identity
'    Public ModelRotationX As Matrix = Matrix.Identity
'    Public ModelRotationZ As Matrix = Matrix.Identity
'    Public RotationVelocity As Vector3 = Vector3.Zero

'    Public RotationVelocityReducingFactor As Vector3 = New Vector3(0.2F, 0.2F, 0.2F)
'    Public ModelVelocityReducingFactor As Vector3 = New Vector3(0.2F, 0.2F, 0.2F)




'    Public Property RotationY() As Single
'        Get
'            Return Mrotation
'        End Get
'        Set
'            Dim newVal As Single = Value
'            While newVal >= MathHelper.TwoPi
'                newVal -= MathHelper.TwoPi
'            End While
'            While newVal < 0
'                newVal += MathHelper.TwoPi
'            End While

'            If Mrotation <> newVal Then
'                Mrotation = newVal
'                ModelRotationY = Matrix.CreateRotationY(Mrotation)
'            End If
'        End Set
'    End Property

'    Public Property RotationX() As Single
'        Get
'            Return Mrotation
'        End Get
'        Set
'            Dim newVal As Single = Value
'            While newVal >= MathHelper.TwoPi
'                newVal -= MathHelper.TwoPi
'            End While
'            While newVal < 0
'                newVal += MathHelper.TwoPi
'            End While

'            If Mrotation <> newVal Then
'                Mrotation = newVal
'                ModelRotationX = Matrix.CreateRotationY(Mrotation)
'            End If
'        End Set
'    End Property

'    Public Property RotationZ() As Single
'        Get
'            Return Mrotation
'        End Get
'        Set
'            Dim newVal As Single = Value
'            While newVal >= MathHelper.TwoPi
'                newVal -= MathHelper.TwoPi
'            End While
'            While newVal < 0
'                newVal += MathHelper.TwoPi
'            End While

'            If Mrotation <> newVal Then
'                Mrotation = newVal
'                ModelRotationZ = Matrix.CreateRotationY(Mrotation)
'            End If
'        End Set
'    End Property



'    Public Sub New()
'        'Model = Game1.BulletModel
'        'Transforms = Game1.BulletTransforms
'        Controls.Bullets.Add(Me)
'    End Sub





'    Public Sub Update()
'        'Bounding sphere
'        MBoundingSphere = New BoundingSphere(Position, BoundingSphereRadius *
'                          BoundingSphereScale)



'        ' Finally, add this vector to our velocity.
'        'Direction.Normalize()
'        '    Velocity += ModelRotation.Forward * 1.0F * RotationVector
'        'Position += ModelVelocity * Direction
'        '   MsgBox(ModelVelocity.ToString + "     " + Direction.ToString)
'        'RotationY += RotationVelocity.Y
'        'RotationX += RotationVelocity.X
'        'RotationZ += RotationVelocity.Z


'    End Sub




'    Public Sub Draw()
'        'Draw the model, a model can have multiple meshes, so loop
'        For Each mesh As ModelMesh In Model.Meshes
'            'This is where the mesh orientation is set
'            For Each effect As BasicEffect In mesh.Effects
'                effect.Projection = SS.projectionMatrix
'                effect.View = SS.viewMatrix
'                effect.World = Scale * ModelRotation * Matrix.CreateTranslation(Position)



'            Next
'            'Draw the mesh, will use the effects set above.
'            mesh.Draw()
'        Next
'    End Sub

'End Class

