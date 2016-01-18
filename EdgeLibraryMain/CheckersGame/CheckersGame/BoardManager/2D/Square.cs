using EdgeLibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CheckersGame
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

        public string BoardID;

        private float Size;

        public Square(string texture, Vector2 position, float size, Color color, string boardID)
            : base(texture, position)
        {
            Size = size;
            Scale = new Vector2(size / Width, size / Height);
            DefaultColor = color;
            Color = color;
            BoardID = boardID;

            SquareNumber = new TextSprite(Config.SquareFont, "", Position) { CenterAsOrigin = true, Color = Config.SquareNumberColor };
            SquareLines = new List<Sprite>();
        }

        public Vector2 Location()
        {
            return new Vector2(X, Y);
        }

        public bool CheckForClick()
        {
            return Input.MousePosition.X > Position.X - Size / 2 && Input.MousePosition.X < Position.X + Size / 2
            && Input.MousePosition.Y > Position.Y - Size / 2 && Input.MousePosition.Y < Position.Y + Size / 2;
        }

        public override void DrawObject(GameTime gameTime)
        {
            base.DrawObject(gameTime);
        }

        public override void UpdateObject(GameTime gameTime)
        {
            base.UpdateObject(gameTime);

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
                piece.BoardID = BoardID;
            }

            OccupyingPiece = piece;
        }

        public override object Clone()
        {
            //Squares aren't the same here Object.ReferenceEquals(this, square)
            Square square = (Square)base.Clone();
            if (OccupyingPiece != null)
            {
                square.SetPiece((Piece)OccupyingPiece.Clone());
            }
            else
            {
                square.SetPiece(null);
            }
            return square;
        }
    }
}
