using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Piece3D : Piece
    {
        public Piece3D(SpriteModel model, Color color, Vector3 scale, bool topTeam) : base("", Vector2.Zero, color, scale.X, topTeam)
        {

        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}
