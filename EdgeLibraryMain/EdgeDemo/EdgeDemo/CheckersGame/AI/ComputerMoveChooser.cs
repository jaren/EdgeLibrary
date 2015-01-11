using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeLibrary;

namespace EdgeDemo.CheckersGame
{
    //Not a very 'intelligent' AI, only chooses best possible move based on current board
    public class ComputerMoveChooser
    {
        /* How the computer should choose moves
         * 0 - Chooses worst possible move
         * 1 - Chooses middle move
         * 2 - Chooses best move
         */
        public int Difficulty = 2;

        /* Maximum number of moves to skip when choosing a move
         * If the move setup was like this:
         *  - Move A
         *  - Move B
         *  - Move C
         *  - Move D
         *  - Move E
         *  - Move F
         *  
         * If the difficulty was 1 and fluctuation was 0, move C or D could be chosen
         * If the difficulty was 1 and fluctuation was 1, move B, C, D, or E could be chosen
         * If the difficulty was 1 and fluctuation was 2, any move could be chosen
         */
        public int DifficultyFluctuation;

        //How many milliseconds to wait before choosing a move
        public float MoveWait;
        public float MoveWaitFluctuation;

        public ComputerMoveChooser(int difficulty = 1, int difficultyFluctuation = 1, float moveWait = 1000, float moveWaitFluctuation = 500)
        {
            Difficulty = difficulty;
            DifficultyFluctuation = difficultyFluctuation;
            MoveWait = moveWait;
            MoveWaitFluctuation = moveWaitFluctuation;
        }

        public Move ChooseMove(List<Move> possibleMoves)
        {
            /* Move sorting order
             * - Number of pieces taken
             * - Number of pieces possible to be lost in the next move
             * - Random
             */

            List<SortedMove> sortedMoves = new List<SortedMove>();
            int piecesTaken = 0;
            int piecesLostNext = 0;
            foreach(Move move in possibleMoves)
            {
                //Add generating stuff here
                sortedMoves.Add(new SortedMove(move, piecesTaken, piecesLostNext));
            }
            sortedMoves.OrderByDescending(x => x.PiecesTaken).ThenBy(x => x.PiecesLostNext);

            //If indexToChoose is -1, then math is broken
            int indexToChoose = Difficulty <= 0 ? 0 : Difficulty == 1 ? sortedMoves.Count / 2 : Difficulty >= 2 ? sortedMoves.Count - 1 : -1;
            indexToChoose = RandomTools.RandomInt(indexToChoose - DifficultyFluctuation, indexToChoose + DifficultyFluctuation);
            indexToChoose = indexToChoose < 0 ? 0 : indexToChoose > sortedMoves.Count - 1 ? sortedMoves.Count - 1;

            return sortedMoves[indexToChoose].Move;
        }
    }

}
