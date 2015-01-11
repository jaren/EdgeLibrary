using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class SortedMove
    {
        public Move Move;
        public int PiecesTaken;
        public int PiecesLostNext;

        public SortedMove(Move move, int piecesTaken, int piecesLostNext)
        {
            Move = move;
            PiecesTaken = piecesTaken;
            PiecesLostNext = piecesLostNext;
        }
    }
}
