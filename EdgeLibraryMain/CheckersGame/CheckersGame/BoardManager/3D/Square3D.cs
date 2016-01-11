using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace CheckersGame
{
    public class Square3D : Square
    {
        public Square3D(SpriteModel model, Vector3 scale, Color color) : base("", Vector2.Zero, scale.X, color)
        {

        }

        public override void DrawObject(GameTime gameTime)
        {
        }
    }
}
