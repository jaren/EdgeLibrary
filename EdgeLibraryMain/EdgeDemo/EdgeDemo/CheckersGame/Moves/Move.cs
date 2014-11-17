using EdgeLibrary;
using Microsoft.Xna.Framework;
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

        ASequence MoveSequence;
        public void RunMove()
        {
            List<Action> Moves = new List<Action>();
            foreach(Square square in SquarePath)
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
