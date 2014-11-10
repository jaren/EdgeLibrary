using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame
{
    /// <summary>
    /// This class manages the board and updates it
    /// </summary>
    public class BoardManager : Sprite
    {
        //The main board - static so other classes can access it
        public static Board Board;

        //Displays which team should move and the move state
        public TextSprite StatusSprite;

        //Displays how many captures each team has
        public static TextSprite CaptureSprite;

        //Displays other stuff
        public TextSprite ExtraSprite;

        //Displays debug info
        public TextSprite DebugSprite;

        //Is it the top team's turn?
        public bool TopTeamTurn;

        //Team has selected the starting square?
        public bool SelectedFirstSquare;

        //The square to start moving from
        private Square startSquare;

        //The current move
        private Move CurrentMove;

        //The square that is currently moused over
        public Square MousedOverSquare;
        
        //The possible moves on this turn
        private Dictionary<Piece, List<Move>> PossibleMoves;

        //Text for the current team
        private string TeamText;

        public BoardManager()
            : base("", Vector2.Zero)
        {
            //Initializing the board
            Board = new Board(Config.SquareTexture, EdgeGame.WindowSize / 2, Config.BoardSize, Config.SquareSize, Config.SquareDistance, Config.Color1, Config.Color2, Config.BorderSize, Config.BorderColor, Config.PieceTexture, Config.PieceSize, Config.TopColor, Config.BottomColor);
            Board.AddToGame();

            //Initializing the debug sprite
            DebugSprite = new DebugText(Config.DebugFont, new Vector2(0, EdgeGame.WindowSize.Y - 300)) { Color = Color.Goldenrod, CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            DebugSprite.AddToGame();

            //Initializing the teamtext
            TeamText = Config.TopTeamName + ": ";

            //Initializing status sprite
            StatusSprite = new TextSprite(Config.StatusFont, TeamText + Config.SelectSquare1Message, Vector2.Zero) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            StatusSprite.AddToGame();

            //Initializing capture sprite
            CaptureSprite = new TextSprite(Config.StatusFont, "Top Team Captures: 0\nBottom Team Captures: 0", new Vector2(0, 50)) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            CaptureSprite.AddToGame();

            //Initializing extra sprite
            ExtraSprite = new TextSprite(Config.StatusFont, "Current Move ID at Start: \n Current Move ID at Finish:", new Vector2(0, 150)) { CenterAsOrigin = false, FollowsCamera = false, ScaleWithCamera = false };
            ExtraSprite.AddToGame();

            //Initializing move
            ResetMove();

            //Subscribes to input
            Input.OnMouseMove += Input_OnMouseMove;
            Input.OnKeyPress += Input_OnKeyPress;
            Input.OnKeyRelease += Input_OnKeyRelease;
            Input.OnClick += Input_OnClick;
            Input.OnReleaseClick += Input_OnReleaseClick;
        }

        //Starts the current move and sends it to the webservice
        public void Move()
        {
            CurrentMove.RunMove();

            //Eventually this will call the web service's move function
            //Something like this:
            //CheckersService.move(short pieceId, short destX, short destY)
        }

        //Necessary override to not draw the BoardManager
        public override void Draw(GameTime gameTime) { }

        //Updates the board
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Updates the debug information
            DebugSprite.Text += "\n2D Camera Scale: " + EdgeGame.Camera.Scale;
        }

        private void Input_OnMouseMove(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            //Gets the square clicked
            MousedOverSquare = Board.GetSquareClicked();
        }

        private void Input_OnReleaseClick(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (MousedOverSquare != null)
            {
                //If the first square hasn't selected, try to select the first square
                if (!SelectedFirstSquare)
                {
                    SetFirstSquare();
                }
                //If the first square was selected already, complete the move
                else
                {
                    SetLastSquare();
                }
            }
        }
        private void Input_OnClick(Vector2 mousePosition, Vector2 previousMousePosition) { }

        private void Input_OnKeyRelease(Keys key) 
        {
            //Cancels the move is the cancel key was pressed
            if (key == Config.MoveCancelKey)
            {
                //If the first square was selected, reset the move
                if (SelectedFirstSquare)
                {
                    foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                    {
                        possibleMove.SquarePath[0].Color = possibleMove.SquarePath[0].DefaultColor;
                    }

                    ResetMove();
                }
            }
        }
        private void Input_OnKeyPress(Keys key) { }

        //Called after each 'segment' of the move completes
        public void CurrentMove_OnCompleteSquare(List<Square> squarePath, List<Square> jumpedSquares, int index)
        {
            //Captures the piece and updates the capture sprite
            Board.CapturePiece(jumpedSquares[index].OccupyingPiece);
            CaptureSprite.Text = "Top Team Captures: " + Board.TopTeamCaptures + "\nBottom Team Captures: " + Board.BottomTeamCaptures;
        }

        //Sets the starting square
        private void SetFirstSquare()
        {
            //Checks if the square is valid
            if (MousedOverSquare.OccupyingPiece != null && PossibleMoves.Keys.Contains(MousedOverSquare.OccupyingPiece))
            {
                //Sets the start square
                startSquare = MousedOverSquare;

                //Resets the color of the possible start squares
                foreach (Piece possiblePiece in PossibleMoves.Keys)
                {
                    Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).DefaultColor;
                }

                //Colors the start square
                MousedOverSquare.Color = Config.Square1SelectColor;

                //Updates info
                SelectedFirstSquare = true;
                StatusSprite.Text = TeamText + Config.SelectSquare2Message;

                //Colors the possible end squares
                foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                {
                    possibleMove.SquarePath[possibleMove.SquarePath.Count - 1].Color = Config.Square2SelectColor;
                }
            }
            //If the square isn't valid, change the message
            else
            {
                StatusSprite.Text = TeamText + Config.SelectSquare1MessageFailed;
            }
        }

        //Sets the last square
        private void SetLastSquare()
        {
            //Find the correct finish square
            foreach (Move move in PossibleMoves[startSquare.OccupyingPiece])
            {
                if (move.SquarePath[move.SquarePath.Count - 1] == MousedOverSquare)
                {
                    //Set the current move and subscribe to it
                    CurrentMove = move;
                    CurrentMove.OnComplete += CurrentMove_OnCompleteSquare;

                    //Run move
                    Move();

                    //Updates info
                    TopTeamTurn = !TopTeamTurn;
                    TeamText = TopTeamTurn ? Config.TopTeamName + ": " : Config.BottomTeamName + ": ";

                    //Resets move
                    ResetMove();
                }
            }
        }

        //Resets the move
        public void ResetMove()
        {
            //If the possible moves have been generated, reset the info
            if (PossibleMoves != null && CurrentMove != null)
            {
                //If you can't move, switches teams
                if (PossibleMoves.Count == 0)
                {
                    //Reversees turns, updates team text, updates sprite, and re-calls ResetMove
                    TopTeamTurn = !TopTeamTurn;
                    TeamText = TopTeamTurn ? Config.TopTeamName + ": " : Config.BottomTeamName + ": ";
                    StatusSprite.Text = TeamText + Config.PassMessage;

                    //Resets the move again, which will generate new squares for the other team
                    PossibleMoves = null; 
                    ResetMove();
                }

                //Resets all of the square colors
                //It uses the finish square's occupying piece because the piece has already been moved
                foreach (Move possibleMove in PossibleMoves[CurrentMove.Piece])
                {
                    foreach(Square square in possibleMove.SquarePath)
                    {
                        square.Color = square.DefaultColor;
                    }
                }
            }

            //Generates new moves
            PossibleMoves = MovementManager.GenerateTeamMoves(TopTeamTurn);

            //Sets the color of the possible starting squares
            foreach (Piece possiblePiece in PossibleMoves.Keys)
            {
                Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = Config.Square1SelectColor;
            }

            //Resets the info
            SelectedFirstSquare = false;
            StatusSprite.Text = TeamText + Config.SelectSquare1Message;
        }
    }
}
