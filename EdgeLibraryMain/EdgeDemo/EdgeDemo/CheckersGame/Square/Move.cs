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

        public delegate void MoveEvent(List<Square> squarePath, List<Square> jumpedSquares);
        public event MoveEvent OnComplete = delegate { };

        public Move(List<Square> squarePath, List<Square> jumpedSquares = null)
        {
            SquarePath = squarePath;
            JumpedSquares = jumpedSquares;
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

            SquarePath[0].OccupyingPiece.AddAction(MoveSequence);
        }

        void MoveSequence_OnFinish(Action action, GameTime gameTime, Sprite sprite)
        {
            OnComplete(SquarePath, JumpedSquares);
        }
    }
}
