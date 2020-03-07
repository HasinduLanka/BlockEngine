Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class Arrow : Inherits Entity


    Public Sub New(eT As EntityType)
        MyBase.New(eT)

        ICode = CShort(LICode + 1)
        LICode += CShort(1)

        RotationX = 0
        RotationY = 0
        RotationZ = 0

        CurrentBlock = New DBlock(New Block)


    End Sub

    Public Sub AddToTheCurrentStack()
        Ground.CStack.eList.Add(Me)
    End Sub




    Public Shadows Sub Update()
        Position += Velocity

    End Sub


    Public Shadows Sub Draw()

        'Draw the model, a model can have multiple meshes, so loop
        Dim m = 0



        For Each mesh As ModelMesh In eType.Model.Meshes
            Dim W = eType.Transforms * ModelRotation * Matrix.CreateTranslation(Position)

            'This is where the mesh orientation is set
            For Each effect As BasicEffect In mesh.Effects

                effect.Projection = projectionMatrix
                effect.View = viewMatrix

                effect.World = W

            Next
            'Draw the mesh, will use the effects set above.
            mesh.Draw()
            m += 1
        Next

    End Sub

End Class
