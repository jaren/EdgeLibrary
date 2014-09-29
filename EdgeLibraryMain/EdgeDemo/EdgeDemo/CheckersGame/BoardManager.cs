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

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
