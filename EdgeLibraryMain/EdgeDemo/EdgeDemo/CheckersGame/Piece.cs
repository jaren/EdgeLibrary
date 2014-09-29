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

        public Piece(string textureName, Vector2 position, Color color, float size)
            : base(textureName, position)
        {
            Color = color;
            Size = size;
            King = false;
        }

        public bool Move(int x, int y)
        {
            //Checks if the player can jump but isn't
            if (Board.Instance.TeamCanJump(TopTeam) &&
                //Checks if the player jumped by checking the distance it moved
                ((x == X + 1 && y == Y + 1) || (x == X - 1 && y == Y + 1) ||
                (x == X + 1 && y == Y - 1) || (x == X - 1 && y == Y - 1)))
            {
                return false;
            }

            //Checks if a piece isn't moving in a correct direction
            if (!King && ((TopTeam && y >= Y) || (!TopTeam && y <= Y)))
            {
                return false;
            }

            //Checks if a piece trying to move off of the board
            if (x > Board.Instance.Size || y > Board.Instance.Size)
            {
                return false;
            }

            return true;
            //Eventually this will call the web service's mvoe function
            //Something like this:
            //CheckersService.move(short pieceId, short destX, short destY)
        }
    }
}
