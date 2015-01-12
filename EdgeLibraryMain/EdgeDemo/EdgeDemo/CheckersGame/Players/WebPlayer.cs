using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame.Players
{
    //A player used by the remote person playing the game
    public class WebPlayer : Player
    {
        public WebPlayer()
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
