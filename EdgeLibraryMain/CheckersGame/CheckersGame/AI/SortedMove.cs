using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersGame
{
    public class SortedMove
    {
        public Move Move;
        public int LostIfNotMoved;
        public int PiecesTaken;
        public int PiecesLostNext;

        public SortedMove(Move move, int lostIfNotMoved, int piecesTaken, int piecesLostNext)
        {
            Move = move;
            LostIfNotMoved = lostIfNotMoved;
            PiecesTaken = piecesTaken;
            PiecesLostNext = piecesLostNext;
        }
    }
}
