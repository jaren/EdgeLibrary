using CheckersGame.CheckersService;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersGame
{
    public class Move
    {
        public string ID;
        public List<Square> SquarePath;
        public List<Square> JumpedSquares;
        public Piece Piece;
        public int MoveIndex;
        public Square StartSquare
        {
            get
            {
                return SquarePath[0];
            }
        }
        public Square FinishSquare
        {
            get
            {
                return SquarePath[SquarePath.Count - 1];
            }
        }
        public delegate void MoveEvent(List<Square> squarePath, List<Square> jumpedSquares, int index);
        public event MoveEvent OnComplete;

        public Move(List<Square> squarePath, List<Square> jumpedSquares = null)
        {
            ID = this.GenerateID();

            SquarePath = squarePath;
            Piece = StartSquare.OccupyingPiece;
            JumpedSquares = jumpedSquares == null ? new List<Square>() : jumpedSquares;
            
            MoveIndex = 0;
        }

        /// <summary>
        /// Converts a move into a SimpleMove for sending to the web service
        /// </summary>
        /// <param name="move">The Move to Convert</param>
        public static SimpleMove ConvertAndSend(Move move)
        {
            SimpleMove newMove = new SimpleMove();

            //Square Path
            //Jumped Squares
            //Start Square
            //Move ID

            Dictionary<int, KeyValuePair<int, int>> path = new Dictionary<int, KeyValuePair<int, int>>();
            for (int i = 0; i < move.SquarePath.Count; i++)
            {
                path.Add(i, new KeyValuePair<int, int>(move.SquarePath[i].X, move.SquarePath[i].Y));
            }

            newMove.SquarePath = path;

            Dictionary<int, KeyValuePair<int, int>> jumped = new Dictionary<int, KeyValuePair<int, int>>();
            if (move.JumpedSquares != null)
            {
                for (int i = 0; i < move.JumpedSquares.Count; i++)
                {
                    jumped.Add(i, new KeyValuePair<int, int>(move.JumpedSquares[i].X, move.JumpedSquares[i].Y));
                }
            }
            else
            {
                jumped = null;
            }

            newMove.JumpedSquares = jumped;

            newMove.ID = move.ID;

            newMove.StartSquare = new KeyValuePair<int, int>(move.StartSquare.X, move.StartSquare.Y);

            newMove.Player1 = move.Piece.Player1;

            return newMove;
        }

        /// <summary>
        /// Converts a SimpleMove from the web service into a Move
        /// </summary>
        /// <param name="moveInfo">An object array of move data produced by the ConvertAndSend function</param>
        public static Move ConvertAndRecieve(SimpleMove moveInfo, Board board = null)
        {
            //Board needs to be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            if (moveInfo != null)
            {
                Dictionary<int, KeyValuePair<int, int>> path = moveInfo.SquarePath;
                Dictionary<int, KeyValuePair<int, int>> jumped = moveInfo.JumpedSquares;
                KeyValuePair<int, int> start = moveInfo.StartSquare;

                List<Square> squarePath = new List<Square>();
                foreach (KeyValuePair<int, int> square in path.Values)
                {
                    squarePath.Add(board.Squares[square.Key, square.Value]);
                }

                List<Square> jumpedSquares = new List<Square>();
                if (jumped != null)
                {
                    foreach (KeyValuePair<int, int> square in jumped.Values)
                    {
                        jumpedSquares.Add(board.Squares[square.Key, square.Value]);
                    }
                }

                Square startSquare = board.Squares[start.Key, start.Value];
                Piece piece = startSquare.OccupyingPiece;

                Move decodedMove = new Move(squarePath, jumpedSquares);
                decodedMove.ID = moveInfo.ID;
                decodedMove.Piece = decodedMove.StartSquare.OccupyingPiece;
                return decodedMove;
            }
            else
            {
                return null;
            }
        }

        ASequence MoveSequence;
        public void RunMove(Board boardToRunOn)
        {
            if(boardToRunOn == null)
            {
                boardToRunOn = BoardManager.Instance.Board;
            }

            List<Action> Moves = new List<Action>();
            foreach (Square square in SquarePath)
            {
                Moves.Add(new AMoveTo(square.Position, Config.CheckerMoveSpeed));
            }
            Moves.RemoveAt(0);
            MoveSequence = new ASequence(Moves);
            MoveSequence.OnActionTransition += MoveSequence_OnTransition;

            Piece.AddAction(MoveSequence);

            SquarePath[0].OccupyingPiece = null;
            SquarePath[SquarePath.Count - 1].SetPiece(Piece);

            //Temporary workaround
            foreach (Square square in JumpedSquares)
            {
                boardToRunOn.CapturePiece(square.OccupyingPiece);
            }
            if (boardToRunOn == BoardManager.Instance.Board)
            {
                BoardManager.Instance.CaptureSprite.Text = "Top Team Captures: " + boardToRunOn.TopTeamCaptures + "\nBottom Team Captures: " + boardToRunOn.BottomTeamCaptures;
            }
            //********************

            if ((Piece.Player1 && SquarePath[SquarePath.Count - 1].Y == Config.BoardSize - 1) || (!Piece.Player1 && SquarePath[SquarePath.Count - 1].Y == 0))
            {
                Piece.King = true;
            }
        }

        void MoveSequence_OnTransition(ASequence sequence, Action action, Sprite sprite, GameTime gameTime)
        {
            OnComplete(SquarePath, JumpedSquares, MoveIndex++);
        }

        //Moves a move from one board to another
        public Move SwitchBoards(Board board)
        {
            Move newMove = new Move(SquarePath);
            newMove.SquarePath = new List<Square>();
            foreach (Square square in SquarePath)
            {
                newMove.SquarePath.Add(board.Squares[square.X, square.Y]);
            }
            foreach (Square square in JumpedSquares)
            {
                newMove.JumpedSquares.Add(board.Squares[square.X, square.Y]);
            }
            newMove.Piece = board.Squares[SquarePath[0].X, SquarePath[0].Y].OccupyingPiece;
            return newMove;
        }
    }
}
