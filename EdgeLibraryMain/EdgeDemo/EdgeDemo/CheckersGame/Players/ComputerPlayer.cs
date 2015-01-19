using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    //A player used by the computer
    public class ComputerPlayer : Player
    {
        private ComputerMoveChooser MoveChooser;

        public ComputerPlayer(int difficulty = 1, int difficultyFluctuation = 1, float moveWait = 1000, float moveWaitFluctuation = 500)
        {
            MoveChooser = new ComputerMoveChooser(difficulty, difficultyFluctuation, moveWait, moveWaitFluctuation);
        }

        public override void ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            List<Move> moves = new List<Move>();
            foreach(Piece piece in possibleMoves.Keys)
            {
                moves.AddRange(possibleMoves[piece]);
            }

            SendMove(MoveChooser.ChooseMove(moves, BoardManager.Board));
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
