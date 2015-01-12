using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    //The base for all other 'players'
    public virtual class Player
    {
        //The player gets the possible moves and sends out to the board
        public virtual Move SendMove(Dictionary<Piece, List<Move>> possibleMoves) { return null; }

        //The player gets sent the previous move
        public virtual void ReceiveMove(Move move) { }
    }
}
