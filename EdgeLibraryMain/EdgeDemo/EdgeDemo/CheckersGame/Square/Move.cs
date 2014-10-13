using EdgeLibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class Move
    {
        public List<Square> SquarePath;
        public List<Square> JumpedSquares;
        public Piece Piece;

        public delegate void MoveEvent(List<Square> squarePath, List<Square> jumpedSquares);
        public event MoveEvent OnComplete = delegate { };

        public Move(List<Square> squarePath, List<Square> jumpedSquares = null)
        {
            SquarePath = squarePath;
            JumpedSquares = jumpedSquares;

            Piece = SquarePath[0].OccupyingPiece;
        }

        public void RunMove()
        {
            List<Action> Moves = new List<Action>();
            foreach(Square square in SquarePath)
            {
                Moves.Add(new AMoveTo(square.Position, Config.CheckerMoveSpeed));
            }
            ASequence MoveSequence = new ASequence(Moves);
            MoveSequence.OnFinish += MoveSequence_OnFinish;

            Piece.AddAction(MoveSequence);

            SquarePath[0].OccupyingPiece = null;
            SquarePath[SquarePath.Count - 1].SetPiece(Piece);

            if ((Piece.TopTeam && SquarePath[SquarePath.Count - 1].Y == Config.BoardSize - 1) || (!Piece.TopTeam && SquarePath[SquarePath.Count - 1].Y == 0))
            {
                Piece.King = true;
            }
        }

        void MoveSequence_OnFinish(Action action, GameTime gameTime, Sprite sprite)
        {
            OnComplete(SquarePath, JumpedSquares);
        }
    }
}
