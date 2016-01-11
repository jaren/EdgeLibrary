using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using CheckersGame.CheckersService;
using Microsoft.Xna.Framework;

namespace CheckersGame
{
    public class BoardManager3D : BoardManager
    {
        public BoardManager3D() : base()
        {
            Components.Remove(Board);
            Board = new Board3D(Config.BoardSize, Config.SquareScale3D, Config.SquareDistance, Config.SquareColor1, Config.SquareColor2, Config.BorderSize3D, Config.BoardHeight3D, Config.BorderColor, Config.PieceScale3D, Config.PieceColor1, Config.PieceColor2);
            Components.Add(Board);

            Components.Remove(DebugSprite);
            DebugSprite = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 150)) { Color = Color.Goldenrod, CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false, Include2D = false, Include3D = true };
            Components.Add(DebugSprite);
        }

        public static void ResetInstance3D()
        {
            Instance = new BoardManager3D();
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
        }
    }
}
