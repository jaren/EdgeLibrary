using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Square3D : Square
    {
        public Square3D(SpriteModel model, Vector3 scale, Color color) : base("", Vector2.Zero, scale.X, color)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
