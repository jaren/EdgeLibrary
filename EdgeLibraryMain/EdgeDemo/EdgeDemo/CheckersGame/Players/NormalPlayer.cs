using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EdgeDemo.CheckersGame
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
            Input.OnReleaseRightClick += Input_OnReleaseRightClick;
            Input.OnClick += Input_OnClick;
            Input.OnReleaseClick += Input_OnReleaseClick;
        }

        public override bool ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            if (!base.ReceivePreviousMove(move, possibleMoves))
            {
                return false;
            }

            PossibleMoves = possibleMoves;

            DrawPossibleStartSquares(PossibleMoves);

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }



        private void Input_OnMouseMove(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (CanMove)
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
        }

        private void Input_OnReleaseClick(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (CanMove)
            {
                //WORKAROUND**********
                Input.ClickCount++;
                //********************

                if (MousedOverSquare != null && /*WORKAROUND*/ Input.ClickCount == 1)
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

        }
        private void Input_OnClick(Vector2 mousePosition, Vector2 previousMousePosition) { if (CanMove) { } }

        private void Input_OnReleaseRightClick(Vector2 mousePosition, Vector2 previousMousePosition)
        {
            if (CanMove)
            {
                //If the first square was selected, reset the move
                if (SelectedFirstSquare)
                {
                    //Clears the possible square paths and resets the colors and numbers of all squares in all possible moves
                    ClearPossibleSquarePaths(startSquare);

                    foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                    {
                        foreach (Square square in possibleMove.SquarePath)
                        {
                            square.Color = square.DefaultColor;
                            square.SquareNumber.Text = "";
                        }
                    }

                    DrawPossibleStartSquares(PossibleMoves);

                    SelectedFirstSquare = false;
                }
            }
        }
        private void Input_OnKeyPress(Keys key) { if (CanMove) { } }

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

                //Colors the possible end squares
                foreach (Move possibleMove in PossibleMoves[startSquare.OccupyingPiece])
                {
                    possibleMove.SquarePath[possibleMove.SquarePath.Count - 1].Color = Config.Square2SelectColor;
                }
            }
            //If the square isn't valid, change the message
            else
            {
                BoardManager.MessageSprite.Display("Invalid square");
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
                    //Set the current move
                    CurrentMove = move;

                    ClearPossibleSquarePaths(CurrentMove.StartSquare);
                    ClearSquareNumberPaths(CurrentMove.FinishSquare);
                    ClearPossibleStartSquares(PossibleMoves);

                    //Run move
                    SendMove(CurrentMove);

                    //Resets move
                    ResetMove();

                    // TODO - Possibly move because of Players

                    //CurrentMove.OnComplete += CurrentMove_OnCompleteSquare;

                    //ClearPossibleSquarePaths(CurrentMove.StartSquare);
                    //ClearSquareNumberPaths(CurrentMove.FinishSquare);

                    ////Run move
                    //ExecuteMove();

                    ////Checks for the game end
                    //if (BoardManager.CheckEndGame())
                    //{
                    //    BoardManager.EndGame();
                    //}

                    ////Updates info
                    //BoardManager.Player1Turn = !BoardManager.Player1Turn;
                    //TeamText = BoardManager.Player1Turn ? Config.Player1Name + ": " : Config.Player2Name + ": ";

                    //Resets move
                    //ResetMove();



                    break;
                }
            }
        }

        //Draws the possible starting squares
        private void DrawPossibleStartSquares(Dictionary<Piece, List<Move>> moves)
        {
            foreach (Piece piece in moves.Keys)
            {
                moves[piece][0].StartSquare.Color = Config.Square1SelectColor;
            }
        }

        //Clears the possible starting squares
        private void ClearPossibleStartSquares(Dictionary<Piece, List<Move>> moves)
        {
            foreach (Piece piece in moves.Keys)
            {
                moves[piece][0].StartSquare.Color = moves[piece][0].StartSquare.DefaultColor;
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
            //TODO - Fix because of players
            //if (Config.ThisGameType != Config.GameType.Online || (Config.IsHost && !TopTeamTurn) || (!Config.IsHost && TopTeamTurn))
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
