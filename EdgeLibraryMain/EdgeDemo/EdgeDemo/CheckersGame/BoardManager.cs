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
        private Piece MovementPiece;
        private Square StartSquare;
        private Dictionary<Piece, List<Square>> PossibleMoves;
        private string TeamText;

        public BoardManager()
            : base("", Vector2.Zero)
        {
            Board = new Board(Config.SquareTexture, EdgeGame.WindowSize / 2, Config.BoardSize, Config.SquareSize, Config.SquareDistance, Config.Color1, Config.Color2, Config.BorderSize, Config.BorderColor, Config.PieceTexture, Config.PieceSize, Config.TopColor, Config.BottomColor);
            Board.AddToGame();

            DebugText debug = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 75)) { Color = Color.Goldenrod, CenterAsOrigin = false };
            debug.AddToGame();

            TeamText = Config.TopTeamName + ": ";

            StatusSprite = new TextSprite(Config.StatusFont, TeamText + Config.SelectSquare1Message, Vector2.Zero);
            StatusSprite.CenterAsOrigin = false;
            StatusSprite.AddToGame();

            resetMove();
        }

        /*
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
            if (MovementManager.TeamCanJump(movePiece.TopTeam) && //NOTE: This should probably be replaced with if(Board.TeamCanJumpTo(movePiece.topTeam).Count > 0 && ... (also the returned dictionary should be assigned to a variable)
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
         */

        public void Move()
        {
            //Move here
            //Eventually this will call the web service's move function
            //Something like this:
            //CheckersService.move(short pieceId, short destX, short destY)
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
                        if (square.OccupyingPiece != null && PossibleMoves.Keys.Contains(square.OccupyingPiece))
                        {
                            MovementPiece = square.OccupyingPiece;
                            StartSquare = square;
                            StartX = square.X;
                            StartY = square.Y;

                            foreach (Piece possiblePiece in PossibleMoves.Keys)
                            {
                                Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).DefaultColor;
                            }

                            square.Color = Config.Square1SelectColor;
                            SelectedFirstSquare = true;
                            StatusSprite.Text = TeamText + Config.SelectSquare2Message;

                            foreach (Square possibleSquare in PossibleMoves[MovementPiece])
                            {
                                possibleSquare.Color = Config.Square2SelectColor;
                            }
                        }
                        else
                        {
                            StatusSprite.Text = TeamText + Config.SelectSquare1MessageFailed;
                        }
                    }
                    else
                    {
                        if (PossibleMoves[MovementPiece].Contains(square))
                        {
                            FinishX = square.X;
                            FinishY = square.Y;

                            Move();

                            TopTeamTurn = !TopTeamTurn;
                            TeamText = TopTeamTurn ? Config.TopTeamName + ": " : Config.BottomTeamName + ": ";
                            resetMove();
                        }
                    }
                }
            }
        }

        public void resetMove()
        {
            if (PossibleMoves != null)
            {
                foreach (Square possibleSquare in PossibleMoves[MovementPiece])
                {
                    possibleSquare.Color = possibleSquare.DefaultColor;
                }

                StartSquare.Color = StartSquare.DefaultColor;
                SelectedFirstSquare = false;
                StatusSprite.Text = TeamText + Config.SelectSquare1Message;
            }

            PossibleMoves = MovementManager.TeamCanMoveTo(TopTeamTurn);

            foreach (Piece possiblePiece in PossibleMoves.Keys)
            {
                Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Config.Square1SelectColor;
            }
        }
    }
}
