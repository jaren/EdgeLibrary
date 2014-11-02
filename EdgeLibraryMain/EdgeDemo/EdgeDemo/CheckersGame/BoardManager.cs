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
        public TextSprite CaptureSprite;
        public TextSprite ExtraSprite;
        public TextSprite DebugSprite;

        //Used for each move
        public bool TopTeamTurn;
        public bool SelectedFirstSquare;
        private Square startSquare;
        private Move CurrentMove;
        private Dictionary<Piece, List<Move>> PossibleMoves;
        private string TeamText;

        public BoardManager()
            : base("", Vector2.Zero)
        {
            Board = new Board(Config.SquareTexture, EdgeGame.WindowSize / 2, Config.BoardSize, Config.SquareSize, Config.SquareDistance, Config.Color1, Config.Color2, Config.BorderSize, Config.BorderColor, Config.PieceTexture, Config.PieceSize, Config.TopColor, Config.BottomColor);
            Board.AddToGame();

            DebugSprite = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 300)) { Color = Color.Goldenrod, CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            DebugSprite.AddToGame();

            TeamText = Config.TopTeamName + ": ";

            StatusSprite = new TextSprite(Config.StatusFont, TeamText + Config.SelectSquare1Message, Vector2.Zero) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            StatusSprite.AddToGame();

            CaptureSprite = new TextSprite(Config.StatusFont, "Top Team Captures: 0\nBottom Team Captures: 0", new Vector2(0, 50)) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            CaptureSprite.AddToGame();

            ExtraSprite = new TextSprite(Config.StatusFont, "Current Move ID at Start: \n Current Move ID at Finish:", new Vector2(0, 150)) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            ExtraSprite.AddToGame();

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
            CurrentMove.RunMove();

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

            DebugSprite.Text += "\n2D Camera Scale: " + EdgeGame.Camera.Scale;

            if (Input.KeyJustPressed(Config.MoveCancelKey))
            {
                if (SelectedFirstSquare)
                {
                    foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                    {
                        possibleMove.SquarePath[0].Color = possibleMove.SquarePath[0].DefaultColor;
                    }

                    resetMove();
                }
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
                            startSquare = square;

                            foreach (Piece possiblePiece in PossibleMoves.Keys)
                            {
                                Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).DefaultColor;
                            }

                            square.Color = Config.Square1SelectColor;
                            SelectedFirstSquare = true;
                            StatusSprite.Text = TeamText + Config.SelectSquare2Message;

                            foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                            {
                                possibleMove.SquarePath[possibleMove.SquarePath.Count - 1].Color = Config.Square2SelectColor;
                            }
                        }
                        else
                        {
                            StatusSprite.Text = TeamText + Config.SelectSquare1MessageFailed;
                        }
                    }
                    else
                    {
                        foreach (Move move in PossibleMoves[startSquare.OccupyingPiece])
                        {
                            if (move.SquarePath[move.SquarePath.Count - 1] == square)
                            {
                                CurrentMove = move;
                                CurrentMove.OnComplete += CurrentMove_OnCompleteSquare;

                                Move();

                                TopTeamTurn = !TopTeamTurn;
                                TeamText = TopTeamTurn ? Config.TopTeamName + ": " : Config.BottomTeamName + ": ";
                                resetMove();
                            }
                        }
                    }
                }
            }
        }

        void CurrentMove_OnCompleteSquare(List<Square> squarePath, List<Square> jumpedSquares, int index)
        {
            Board.CapturePiece(jumpedSquares[index].OccupyingPiece);
            CaptureSprite.Text = "Top Team Captures: " + Board.TopTeamCaptures + "\nBottom Team Captures: " + Board.BottomTeamCaptures;
        }

        public void resetMove()
        {
            if (PossibleMoves != null && CurrentMove != null)
            {
                if (PossibleMoves.Count == 0)
                {
                    TopTeamTurn = !TopTeamTurn;
                    TeamText = TopTeamTurn ? Config.TopTeamName + ": " : Config.BottomTeamName + ": ";
                    StatusSprite.Text = TeamText + Config.PassMessage;
                    resetMove();
                }

                //It uses the finish square's occupying piece because the piece has already been moved
                foreach (Move possibleMove in PossibleMoves[CurrentMove.Piece])
                {
                    foreach(Square square in possibleMove.SquarePath)
                    {
                        square.Color = square.DefaultColor;
                    }
                }
            }

            PossibleMoves = MovementManager.GenerateTeamMoves(TopTeamTurn);

            foreach (Piece possiblePiece in PossibleMoves.Keys)
            {
                Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Config.Square1SelectColor;
            }

            SelectedFirstSquare = false;
            StatusSprite.Text = TeamText + Config.SelectSquare1Message;
        }
    }
}
