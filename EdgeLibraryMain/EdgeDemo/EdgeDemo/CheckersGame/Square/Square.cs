using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;

namespace EdgeDemo.CheckersGame
{
    public class Square : Sprite
    {
        public bool FakeSquare;
        public Piece OccupyingPiece;
        public Color DefaultColor;
        public TextSprite SquareNumber;
        public List<Sprite> SquareLines;

        public Vector2 TopLeft;

        public int X;
        public int Y;

        private float Size;

        public Square(string texture, Vector2 position, float size, Color color)
            : base(texture, position)
        {
            Size = size;
            Scale = new Vector2(size/Width, size/Height);
            DefaultColor = color;
            Color = color;

            SquareNumber = new TextSprite(Config.SquareFont, "", Position) { CenterAsOrigin = true, Color = Config.SquareNumberColor };
            SquareLines = new List<Sprite>();
        }

        public bool CheckForClick()
        {
            return Input.MousePosition.X > Position.X - Size / 2 && Input.MousePosition.X < Position.X + Size / 2
            && Input.MousePosition.Y > Position.Y - Size / 2 && Input.MousePosition.Y < Position.Y + Size / 2;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach(Sprite sprite in SquareLines)
            {
                sprite.Draw(gameTime);
            }
            SquareNumber.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (OccupyingPiece != null)
            {
                OccupyingPiece.Update(gameTime);
            }

            SquareNumber.Update(gameTime);
        }

        public void SetPiece(Piece piece)
        {
            if (piece != null)
            {
                piece.X = X;
                piece.Y = Y;
            }

            OccupyingPiece = piece;
        }
    }
}
