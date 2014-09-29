using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class BoardManager : Sprite
    {
        public Board Board;
        public Sprite StatusSprite;

        //Used for each move
        public bool TopTeamTurn;
        public bool SelectedFirstSquare;

        public Color TopColor = Color.Gray;
        public Color BottomColor = Color.DarkGray;

        public BoardManager() : base("", Vector2.Zero)
        {
            Board = new Board("Pixel", EdgeGame.WindowSize / 2, 8, 64, 0, Color.SaddleBrown, Color.Tan, 5, Color.DarkGoldenrod, "Checkers", 50, TopColor, BottomColor);
            Board.AddToGame();

            StatusSprite = new TextSprite("ComicSans-20", "Top Team: Select a square to move from", Vector2.Zero);
            StatusSprite.CenterAsOrigin = false;
            StatusSprite.AddToGame();
        }

        public bool Move(int startX, int startY, int finishX, int finishY)
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

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
