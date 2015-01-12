using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame.Players
{
    //A player used by the computer
    public class ComputerPlayer : Player
    {
        public ComputerPlayer()
        {

        }

        public override Move SendMove(Dictionary<Piece, List<Move>> possibleMoves)
        {
            return null;
        }

        public override void ReceiveMove(Move move)
        {
        }
    }
}
