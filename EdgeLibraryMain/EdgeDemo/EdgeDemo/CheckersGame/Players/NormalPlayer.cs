using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame.Players
{
    //A player used by the person playing the game
    public class NormalPlayer : Player
    {
        //Has the first square been clicked
        public bool SelectedFirstSquare;

        //The square to start moving from
        private Square startSquare;

        //The current move
        private Move CurrentMove;

        //The square that is currently moused over
        public Square MousedOverSquare;
        public Square PreviousMousedOverSquare;

        //The possible moves on this turn
        private Dictionary<Piece, List<Move>> PossibleMoves;

        public NormalPlayer()
        {
            //Subscribes to input
            Input.OnMouseMove += Input_OnMouseMove;
            Input.OnKeyPress += Input_OnKeyPress;
            Input.OnKeyRelease += Input_OnKeyRelease;
            Input.OnClick += Input_OnClick;
            Input.OnReleaseClick += Input_OnReleaseClick;
        }

        public override void ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            PossibleMoves = possibleMoves;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        private void Input_OnMouseMove(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            //Gets the square clicked
            PreviousMousedOverSquare = MousedOverSquare;
            MousedOverSquare = BoardManager.Board.GetSquareMousedOver();

            if (MousedOverSquare != PreviousMousedOverSquare)
            {
                if (SelectedFirstSquare)
                {
                    //ClearPossibleSquarePaths(PreviousMousedOverSquare);
                    ClearSquareNumberPaths(PreviousMousedOverSquare);
                    DrawSquareNumberPath(MousedOverSquare);
                }
                else
                {
                    ClearPossibleSquarePaths(PreviousMousedOverSquare);
                    DrawPossibleSquarePaths(MousedOverSquare);
                }
            }
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

                    ClearPossibleSquarePaths(startSquare);
                    if (startSquare != null)
                    {
                        foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                        {
                            foreach (Square square in possibleMove.SquarePath)
                            {
                                square.Color = square.DefaultColor;
                                square.SquareNumber.Text = "";
                            }
                        }
                    }

                    ResetMove();
                }
            }
        }
        private void Input_OnKeyPress(Keys key) { }

        //Sets the starting square to the moused over square
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
                    BoardManager.Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).Color = BoardManager.Board.GetSquareAt(possiblePiece.X, possiblePiece.Y).DefaultColor;
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

        //Sets the last square to the moused over square
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

                    ClearPossibleSquarePaths(CurrentMove.StartSquare);
                    ClearSquareNumberPaths(CurrentMove.FinishSquare);

                    //Run move
                    ExecuteMove();

                    //Checks for the game end
                    if (CheckEndGame())
                    {
                        EndGame();
                    }

                    //Updates info
                    TopTeamTurn = !TopTeamTurn;
                    TeamText = TopTeamTurn ? Config.Player1Name + ": " : Config.Player2Name + ": ";

                    //Resets move
                    ResetMove();


                    if (Config.ThisGameType == Config.GameType.Online)
                    {
                        #region WebServiceConnection
                        //try
                        //{
                        CheckersServiceClient WebService = new CheckersServiceClient();
                        ////Send Move to Web Service

                        WebService.AddMove(Move.ConvertAndSend(CurrentMove), Config.ThisGameID);
                        Move RemoteMove = null;
                        int loop = 0;

                        while (RemoteMove == null)
                        {
                            if (loop == 0)
                            {
                                //TODO: Add loading text so user thinks something is happening
                                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(TopTeamTurn, Config.ThisGameID));

                                if (recievedMove != null)
                                {
                                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(TopTeamTurn, Config.ThisGameID));
                                    break;
                                }
                            }
                            else if (loop == 120)
                            {
                                loop = -1;
                            }

                            loop++;
                        }

                        //Duplicate This Function

                        //Set the current move and subscribe to it

                        CurrentMove = RemoteMove;

                        CurrentMove.OnComplete += CurrentMove_OnCompleteSquare;

                        ClearPossibleSquarePaths(CurrentMove.StartSquare);
                        ClearSquareNumberPaths(CurrentMove.FinishSquare);

                        //Run move
                        ExecuteMove();

                        //Checks for the game end
                        if (CheckEndGame())
                        {
                            EndGame();
                        }

                        //Updates info
                        TopTeamTurn = !TopTeamTurn;
                        TeamText = TopTeamTurn ? Config.Player1Name + ": " : Config.Player2Name + ": ";

                        //Resets move
                        ResetMove();
                        //}
                        //catch(Exception e)
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Multiplayer Connection Error!\nGame will now close.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        //    System.Windows.Forms.MessageBox.Show("Detailed Error Below:\n" + e, "Error");
                        //}
                        #endregion WebServiceConnection
                    }


                    break;
                }
            }
        }

        //Draws the square lines for a certain move
        private void DrawSquareLines(Move move)
        {
            Square square = move.StartSquare;

            for (int i = 0; i < move.SquarePath.Count - 1; i++)
            {
                square.SquareLines.Add(
                    new Sprite(Config.SquareTexture, Vector2.Lerp(move.SquarePath[i].Position, move.SquarePath[i + 1].Position, 0.5f))
                    {
                        Scale = new Vector2(Config.SquareLineThickness,
                        (float)Math.Sqrt((move.SquarePath[i].Position.X - move.SquarePath[i + 1].Position.X) * (move.SquarePath[i].Position.X - move.SquarePath[i + 1].Position.X)
                        + (move.SquarePath[i].Position.Y - move.SquarePath[i + 1].Position.Y) * (move.SquarePath[i].Position.Y - move.SquarePath[i + 1].Position.Y))),

                        Rotation = -1 * (float)Math.Atan2((move.SquarePath[i].Position.Y - move.SquarePath[i + 1].Position.Y), (move.SquarePath[i].Position.X - move.SquarePath[i + 1].Position.X)),

                        Color = Config.SquareLineColor
                    });
            }
        }

        //Draws all the possible square paths for the moused over square
        private void DrawPossibleSquarePaths(Square square)
        {
            if (square != null)
            {
                foreach (Piece piece in PossibleMoves.Keys)
                {
                    if (square.OccupyingPiece == piece)
                    {
                        foreach (Move move in PossibleMoves[piece])
                        {
                            DrawSquareLines(move);
                        }
                    }
                }
            }
        }
        //Clears the square paths for a certain square
        private void ClearPossibleSquarePaths(Square square)
        {
            if (square != null)
            {
                square.SquareLines.Clear();
            }
        }

        //Draws the square numbers and path for the current moused over square
        private void DrawSquareNumberPath(Square endSquare)
        {
            foreach (Move move in PossibleMoves[startSquare.OccupyingPiece])
            {
                if (move.FinishSquare == endSquare)
                {
                    for (int i = 0; i < move.SquarePath.Count; i++)
                    {
                        move.SquarePath[i].SquareNumber.Text = i.ToString();
                    }

                    foreach (Square square in move.JumpedSquares)
                    {
                        square.OccupyingPiece.ShowX = true;
                    }
                }
            }
        }

        //Clears the possible square paths for a certain square
        private void ClearSquareNumberPaths(Square endSquare)
        {
            if (Config.ThisGameType != Config.GameType.Online || (Config.IsHost && !TopTeamTurn) || (!Config.IsHost && TopTeamTurn))
            {
                foreach (Move move in PossibleMoves[startSquare.OccupyingPiece])
                {
                    if (move.FinishSquare == endSquare)
                    {
                        foreach (Square square in move.SquarePath)
                        {
                            square.SquareNumber.Text = "";
                        }

                        foreach (Square square in move.JumpedSquares)
                        {
                            square.OccupyingPiece.ShowX = false;
                        }
                    }
                }
            }
        }


        //Resets the move at the end of the turn
        public void ResetMove()
        {
            //If you can't move, switches teams
            if (PossibleMoves.Count == 0)
            {
                BoardManager.MessageSprite.Display("You have passed your turn\nYou have no possible moves");
            }

            if (PossibleMoves.ContainsKey(CurrentMove.Piece))
            {
                //Resets all of the square colors
                //It uses the finish square's occupying piece because the piece has already been moved
                foreach (Move possibleMove in PossibleMoves[CurrentMove.Piece])
                {
                    foreach (Square square in possibleMove.SquarePath)
                    {
                        square.Color = square.DefaultColor;
                    }
                }
            }

            //Resets the info
            SelectedFirstSquare = false;
        }
    }
}
