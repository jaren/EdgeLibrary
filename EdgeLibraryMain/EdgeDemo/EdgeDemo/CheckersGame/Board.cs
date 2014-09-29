using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Board : Sprite
    {
        public Square[,] Squares;
        public int Size;
        public Border Border;

        public Board(string squareTexture, Vector2 position, int size, float squareSize, Color color1, Color color2, Color borderColor, float squareDistance = 0, float borderSize = 10)
            : base(squareTexture, position)
        {
            float totalSquareDistance = squareDistance * (size - 1);

            Border = new Border(squareTexture, position, borderSize, squareSize*size + totalSquareDistance, borderColor);

            Vector2 topLeft = new Vector2(position.X - (squareSize * size - squareSize + totalSquareDistance) / 2, position.Y - (squareSize * size - squareSize + totalSquareDistance) / 2);

            Size = size;
            Squares = new Square[size, size];
            bool team1 = true;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    team1 = !team1;
                    Squares[x, y] = new Square(squareTexture, new Vector2(topLeft.X + (squareSize + squareDistance)*x, topLeft.Y + (squareSize+squareDistance)*y), squareSize, team1 ? color1 : color2);
                }

                if (size % 2 == 0)
                {
                    team1 = !team1;
                }
            }
            Instance = this;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Square piece in Squares)
            {
                piece.Draw(gameTime);
            }

            Border.Draw(gameTime);
            Color = Color.White;
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Border.Update(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public bool TeamCanJump(bool team)
        {
            //TODO: Check if team can move one of their pieces to jump one of the other team's pieces
            return false;
        }

        public Piece GetPieceAt(int x, int y)
        {
            return Squares[x, y].OccupyingPiece;
        }

        public static Board Instance;
    }

    public enum Direction
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
