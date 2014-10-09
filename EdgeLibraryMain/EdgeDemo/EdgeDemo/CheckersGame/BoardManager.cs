using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame
{
    public class BoardManager : Sprite
    {
        public Board Board;
        public TextSprite StatusSprite;

        //Used for each move
        public bool TopTeamTurn;
        public bool SelectedFirstSquare;
        private int StartX;
        private int StartY;
        private int FinishX;
        private int FinishY;
        private List<Vector2> PossibleMoves;

        public BoardManager()
            : base("", Vector2.Zero)
        {
            Board = new Board(Config.SquareTexture, EdgeGame.WindowSize/2, Config.BoardSize, Config.SquareSize, Config.SquareDistance, Config.Color1, Config.Color2, Config.BorderSize, Config.BorderColor, Config.PieceTexture, Config.PieceSize, Config.TopColor, Config.BottomColor);
            Board.AddToGame();

            DebugText debug = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 75)) { Color = Color.Goldenrod, CenterAsOrigin = false };
            debug.AddToGame();

            StatusSprite = new TextSprite(Config.StatusFont, Config.TopTeamName + " " + Config.SelectSquare1Message, Vector2.Zero);
            StatusSprite.CenterAsOrigin = false;
            StatusSprite.AddToGame();
        }

        public void GeneratePossibleMoves()
        {
            PossibleMoves = new List<Vector2>();

            Piece movePiece = Board.GetPieceAt(StartX, StartY);

            if (movePiece == null)
            {
            }

            if (Board.GetPieceAt(FinishX, FinishY) != null)
            {
            }

            if (Board.GetSquareAt(FinishX, FinishY).FakeSquare)
            {
            }

            //Checks if the player can jump but isn't
            if (Movement.TeamCanJump(movePiece.TopTeam) && //NOTE: This should probably be replaced with if(Board.TeamCanJumpTo(movePiece.topTeam).Count > 0 && ... (also the returned dictionary should be assigned to a variable)
                //Checks if the player jumped by checking the distance it moved
                ((FinishX == StartX + 1 && FinishY == StartY + 1) || (FinishX == StartX - 1 && FinishY == StartY + 1) ||
                (FinishX == StartX + 1 && FinishY == StartY - 1) || (FinishX == StartX - 1 && FinishY == StartY - 1)))
            {
            }

            //Checks if a piece isn't moving in a correct direction
            if (!movePiece.King && ((movePiece.TopTeam && FinishY >= StartY) || (!movePiece.TopTeam && FinishY <= StartY)))
            {
            }

            //Checks if a piece trying to move off of the board
            if (FinishX > Board.Size || FinishY > Board.Size)
            {
            }
        }

        public bool Move()
        {
            if (PossibleMoves.Contains(new Vector2(FinishX, FinishY)))
            {
                //Move here
                //Eventually this will call the web service's move function
                //Something like this:
                //CheckersService.move(short pieceId, short destX, short destY)
                return true;
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Input.KeyJustPressed(Config.MoveCancelKey))
            {
                resetMove();
            }

            if (Input.JustLeftClicked())
            {
                Square square = Board.GetSquareClicked();

                if (square != null)
                {
                    if (!SelectedFirstSquare)
                    {
                        StartX = square.X;
                        StartY = square.Y;
                        square.Color = Color.DarkGreen;
                        SelectedFirstSquare = true;
                        StatusSprite.Text = Config.SelectSquare2Message;
                        GeneratePossibleMoves();
                    }
                    else
                    {
                        FinishX = square.X;
                        FinishY = square.Y;

                        if (Move())
                        {
                            TopTeamTurn = !TopTeamTurn;
                            resetMove();
                        }
                    }
                }
            }
        }

        public void resetMove()
        {
            Board.GetSquareAt(StartX, StartY).Color = Board.GetSquareAt(StartX, StartY).DefaultColor;
            Board.GetSquareAt(FinishX, FinishY).Color = Board.GetSquareAt(FinishX, FinishY).DefaultColor;
            SelectedFirstSquare = false;
            StatusSprite.Text = Config.SelectSquare1Message;
        }
    }
}
