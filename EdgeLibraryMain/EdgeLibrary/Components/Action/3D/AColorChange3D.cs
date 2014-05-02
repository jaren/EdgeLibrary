using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EdgeLibrary
{
    //Changes the color of a sprite using a ColorChangeIndex
    public class AColorChange3D : Action3D
    {
        public ColorChangeIndex ColorIndex;
        public VertexPositionColor Vertex;
        protected int VertexIndex;
        protected bool hasSetVertex;

        public AColorChange3D(int vertexIndex, Color start, Color finish, float time) : this(vertexIndex, new ColorChangeIndex(time, start, finish)) {}
        public AColorChange3D(int vertexIndex, ColorChangeIndex index)
        {
            ColorIndex = index;
            VertexIndex = vertexIndex;
            hasSetVertex = false;
        }

        //Changes the sprite's color based on the color change index
        protected override void UpdateAction(GameTime gameTime, Sprite3D sprite)
        {
            if (!hasSetVertex) { Vertex = ((PrimitiveModel)sprite.Model).Vertices[VertexIndex]; hasSetVertex = true; }

            Vertex.Color = ColorIndex.Update(gameTime);

            if (sprite.Model is PrimitiveModel)
            {
                ((PrimitiveModel)sprite.Model).SetVertexAt(Vertex, VertexIndex);
            }
            else
            {
                throw new InvalidCastException("The sprite's model is not supported by this action");
            }

            if (ColorIndex.HasFinished)
            {
                Stop(gameTime, sprite);
            }
        }

        public override Action3D Clone()
        {
            return new AColorChange3D(VertexIndex, ColorIndex);
        }
    }
}
