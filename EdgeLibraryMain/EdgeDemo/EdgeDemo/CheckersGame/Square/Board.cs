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
        public List<Piece> CapturedPieces;
        public int TopTeamCaptures;
        public int BottomTeamCaptures;
        public int Size;
        public Border Border;

        public Square mousedOverSquare;

        public float CompleteSize;
        public float SquareSize;

        public Rectangle BoardArea;

        public Board(string squareTexture, Vector2 position, int size, float squareSize, float squareDistance, Color color1, Color color2, float borderSize, Color borderColor, string pieceTexture, float pieceSize, Color pieceColor1, Color pieceColor2)
            : base(squareTexture, position)
        {
            if (size < 4)
            {
                throw new ArgumentException("Board size must be greater than three");
            }

            float totalSquareDistance = squareDistance * (size - 1);

            Border = new Border(squareTexture, position, borderSize, squareSize * size + totalSquareDistance, borderColor);

            CapturedPieces = new List<Piece>();
            TopTeamCaptures = 0;
            BottomTeamCaptures = 0;

            Vector2 topLeft = new Vector2(position.X - (squareSize * size - squareSize + totalSquareDistance) / 2, position.Y - (squareSize * size - squareSize + totalSquareDistance) / 2);
            CompleteSize = (Position.X - topLeft.X) * 2 + squareSize + totalSquareDistance;

            BoardArea = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)(squareSize * size - squareSize + totalSquareDistance), (int)(squareSize * size - squareSize + totalSquareDistance));

            SquareSize = squareSize;

            Size = size;
            Squares = new Square[size, size];
            bool hasPiece = true;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    hasPiece = !hasPiece;
                    Squares[x, y] = new Square(squareTexture, new Vector2(topLeft.X + (squareSize + squareDistance) * x, topLeft.Y + (squareSize + squareDistance) * y), squareSize, hasPiece ? color1 : color2) { X = x, Y = y, TopLeft = new Vector2(topLeft.X + x * squareSize - (squareSize / 2), topLeft.Y + y * squareSize - (squareSize / 2)) };

                    if (hasPiece && (y < ((size - 3) / 2 + 1) || y > ((size - 3) / 2 + 2)))
                    {
                        bool topTeam = y < ((size - 3) / 2 + 1);
                        Squares[x, y].SetPiece(new Piece(pieceTexture, Squares[x, y].Position, topTeam ? pieceColor1 : pieceColor2, pieceSize, topTeam));
                    }
                }

                if (size % 2 == 0)
                {
                    hasPiece = !hasPiece;
                }
            }
        }

        public void MoveBoard(Vector2 position)
        {
            Vector2 topLeft = new Vector2(Position.X - CompleteSize / 2, Position.Y - CompleteSize / 2);

            Position = position;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Squares[x, y].TopLeft = topLeft + new Vector2(x * SquareSize, y * SquareSize);
                }
            }
        }

        public bool MovePiece(int startX, int startY, int finishX, int finishY)
        {
            if (Squares[startX, startY].OccupyingPiece != null && Squares[finishX, finishY].OccupyingPiece == null)
            {
                Squares[finishX, finishY].SetPiece(Squares[startX, startY].OccupyingPiece);
                Squares[startX, startY].OccupyingPiece = null;

                if ((Squares[finishX, finishY].OccupyingPiece.Player1 && finishY == Config.BoardSize - 1) || (!Squares[finishX, finishY].OccupyingPiece.Player1 && finishY == 0))
                {
                    Squares[finishX, finishY].OccupyingPiece.King = true;
                }

                Squares[finishX, finishY].OccupyingPiece.AddAction(new AMoveTo(Squares[finishX, finishY].Position, Config.CheckerMoveSpeed));

                return true;
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            //To make sure the pieces get drawn on top of the squares
            foreach(Square square in Squares)
            {
                square.Draw(gameTime);
            }

            foreach(Square square in Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    square.OccupyingPiece.Draw(gameTime);
                }

                foreach (Sprite sprite in square.SquareLines)
                {
                    sprite.Draw(gameTime);
                }

                square.SquareNumber.Draw(gameTime);
            }

            Border.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Border.Update(gameTime);

            foreach (Square square in Squares)
            {
                square.Update(gameTime);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public bool CapturePiece(Piece piece)
        {
            foreach(Square square in Squares)
            {
                if (square.OccupyingPiece == piece)
                {
                    square.SetPiece(null);
                    CapturedPieces.Add(piece);
                    piece.AddAction(new AColorChange(new ColorChangeIndex(Config.CheckerFadeOutSpeed, piece.Color, Color.Transparent)));
                    if (piece.Player1)
                    {
                        BottomTeamCaptures++;
                    }
                    else
                    {
                        TopTeamCaptures++;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool CheckForMouseOver()
        {
            return Input.MousePosition.X > Position.X - (CompleteSize * EdgeGame.Camera.Scale / 2) && Input.MousePosition.X < Position.X + CompleteSize / 2
                && Input.MousePosition.Y > Position.Y - (CompleteSize * EdgeGame.Camera.Scale / 2) && Input.MousePosition.Y < Position.Y + CompleteSize / 2;
        }

        public Square GetSquareMousedOver()
        {
            if (CheckForMouseOver())
            {
                //Vector2 topLeft = new Vector2(BoardArea.Left, BoardArea.Top);
                //float modifiedX = (Input.MousePosition.X - topLeft.X - EdgeGame.Camera.Position.X + EdgeGame.WindowSize.X/2)/EdgeGame.Camera.Scale;
                //float modifiedY = (Input.MousePosition.Y - topLeft.Y - EdgeGame.Camera.Position.Y + EdgeGame.WindowSize.Y/2)/EdgeGame.Camera.Scale;

                //This was made with zooming in mind. Zooming was removed and this may be overcomplicated
                Vector2 topLeft = new Vector2(Position.X - (CompleteSize * EdgeGame.Camera.Scale / 2), Position.Y - (CompleteSize * EdgeGame.Camera.Scale / 2));
                float modifiedX = Input.MousePosition.X - topLeft.X - EdgeGame.Camera.Position.X + EdgeGame.WindowSize.X / 2;
                float modifiedY = Input.MousePosition.Y - topLeft.Y - EdgeGame.Camera.Position.Y + EdgeGame.WindowSize.Y / 2;

                Vector2 modifiedPosition = new Vector2(modifiedX - (modifiedX % (SquareSize * EdgeGame.Camera.Scale)) + topLeft.X, modifiedY - (modifiedY % (SquareSize * EdgeGame.Camera.Scale)) + topLeft.Y);
                Vector2 finalPosition = new Vector2(modifiedPosition.X / EdgeGame.Camera.Scale, modifiedPosition.Y / EdgeGame.Camera.Scale);

                foreach (Square square in Squares)
                {
                    if (square.TopLeft == modifiedPosition)
                    {
                        mousedOverSquare = square;
                        return square;
                    }
                }
            }
            return null;
        }

        public Piece GetPieceAt(int x, int y)
        {
            return Squares[x, y].OccupyingPiece;
        }

        public Square GetSquareAt(int x, int y)
        {
            return Squares[x, y];
        }

        public Square GetSquareBetween(Square origin, Square destination)
        {
            if(Math.Abs(destination.X - origin.X) != 1 || Math.Abs(destination.Y - origin.Y) != 1)
            {
                return Squares[(destination.X + origin.X) / 2, (destination.Y + origin.Y) / 2];
            }

            return null;
        }
    }

    public enum Direction
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
