using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame.Players
{
    //A player used by the person playing the game
    public class NormalPlayer : Player
    {
        public NormalPlayer()
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
