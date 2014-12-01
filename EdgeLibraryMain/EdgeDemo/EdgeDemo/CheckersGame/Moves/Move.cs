using EdgeLibrary;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
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
            JumpedSquares = jumpedSquares == null ? new List<Square>() : jumpedSquares;

            Piece = SquarePath[0].OccupyingPiece;

            MoveIndex = 0;
        }

        /// <summary>
        /// Converts a move into a collection of integers for sending to the web service
        /// </summary>
        /// <param name="move">The Move to Convert</param>
        public static object[] ConvertAndSend(Move move)
        {
            //Square Path
            //Jumped Squares
            //Start Square
            //Move ID

            Dictionary<int, KeyValuePair<int, int>> path = new Dictionary<int, KeyValuePair<int, int>>();
            for (int i = 0; i < move.SquarePath.Count; i++)
            {
                path.Add(i, new KeyValuePair<int, int>(move.SquarePath[i].X, move.SquarePath[i].Y));
            }

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

            string moveID = move.ID;

            KeyValuePair<int, int> start = new KeyValuePair<int, int>(move.StartSquare.X,move.StartSquare.Y);

            object[] moveInfo = new object[4]
                {
                    path,
                    jumped,
                    start,
                    moveID
                };

            return moveInfo;
        }

        /// <summary>
        /// Converts a collection of integers from the web service into a move
        /// </summary>
        /// <param name="moveInfo">An object array of move data produced by the ConvertAndSend function</param>
        public static Move ConvertAndRecieve(object[] moveInfo)
        {
            Dictionary<int, KeyValuePair<int, int>> path = (Dictionary<int, KeyValuePair<int, int>>)moveInfo[0];
            Dictionary<int, KeyValuePair<int, int>> jumped = (Dictionary<int, KeyValuePair<int, int>>)moveInfo[1];
            KeyValuePair<int, int> start = (KeyValuePair<int, int>)moveInfo[2];
            
            List<Square> squarePath = new List<Square>();
            foreach(KeyValuePair<int,int> square in path.Values)
            {
                squarePath.Add(Board.Squares[square.Key, square.Value]);
            }

            List<Square> jumpedSquares = new List<Square>();
            if(jumped != null)
            {
                foreach(KeyValuePair<int,int> square in jumped.Values)
                {
                    jumpedSquares.Add(Board.Squares[square.Key, square.Value]);
                }
            }

            Square startSquare = Board.Squares[start.Key, start.Value];
            Piece piece = startSquare.OccupyingPiece;

            Move decodedMove = new Move(squarePath, jumpedSquares);
            decodedMove.ID = (string)moveInfo[3];
            return decodedMove;
        }

        ASequence MoveSequence;
        public void RunMove()
        {
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
                BoardManager.Board.CapturePiece(square.OccupyingPiece);
            }
            BoardManager.CaptureSprite.Text = "Top Team Captures: " + Board.TopTeamCaptures + "\nBottom Team Captures: " + Board.BottomTeamCaptures;
            //********************

            if ((Piece.TopTeam && SquarePath[SquarePath.Count - 1].Y == Config.BoardSize - 1) || (!Piece.TopTeam && SquarePath[SquarePath.Count - 1].Y == 0))
            {
                Piece.King = true;
            }
        }

        void MoveSequence_OnTransition(ASequence sequence, Action action, Sprite sprite, GameTime gameTime)
        {
            OnComplete(SquarePath, JumpedSquares, MoveIndex++);
        }
    }
}
