using EdgeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class Piece : Sprite
    {
        public bool King;
        public bool TopTeam;

        public float Size
        {
            set
            {
                Scale = new Vector2(value / Width, value / Height);
            }
        }

        public int X;
        public int Y;

        public Piece(string textureName, Vector2 position, Color color, float size, bool topTeam)
            : base(textureName, position)
        {
            Color = color;
            Size = size;
            TopTeam = topTeam;
            King = false;
        }
    }
}
