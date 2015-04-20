using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Board3D : Board
    {
        //Most of the arguments passed to the base constructor SHOULD still work when switched with any value because they're not used in a 3D class
        public Board3D(int size, Vector3 squareScale, float squareDistance, Color color1, Color color2, float borderSize, float boardHeight, Color borderColor, Vector3 pieceScale, Color pieceColor1, Color pieceColor2) : base("", Vector2.Zero, size, 0, squareDistance, color1, color2, borderSize, borderColor, "", 0, pieceColor1, pieceColor2)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}
