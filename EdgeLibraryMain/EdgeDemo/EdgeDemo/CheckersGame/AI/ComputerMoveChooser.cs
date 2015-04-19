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

        private Board Board; //Used for choosing next moves without disturbing default board

        public ComputerMoveChooser(int difficulty = 1, int difficultyFluctuation = 1)
        {
            Difficulty = difficulty;
            DifficultyFluctuation = difficultyFluctuation;
        }

        public Move ChooseMove(List<Move> possibleMoves, Board board)
        {
            /* Move sorting order
             * - Number of pieces taken
             * - Number of pieces possible to be lost in the next move
             */

            List<SortedMove> sortedMoves = new List<SortedMove>();
            int piecesLostNextIfNotMoved = 0;
            int piecesTaken = 0;
            int piecesLostNext = 0;

            foreach(Move move in possibleMoves)
            {
                //Gets the number of pieces taken
                piecesTaken = move.JumpedSquares.Count;

                //Clones the board into the fake board
                //Warning: This is very expensive to do every move
                Board = (Board)board.Clone();

                //Transfers the move to the fake board
                //Runs the transferred move on the fake board to find how many pieces can be taken next move
                move.SwitchBoards(Board).RunMove(Board);

                //Generates the possible moves for the next team
                Dictionary<Piece, List<Move>> possibleNextMoves;
                if (Config.DrawIn3D)
                {
                    possibleNextMoves = MovementManager.GeneratePlayerMoves(!BoardManager.Player1Turn, Board);
                }
                else
                {
                    possibleNextMoves = MovementManager3D.GeneratePlayerMoves(!BoardManager3D.Player1Turn, Board);
                }

                foreach(Piece possibleNextPiece in possibleNextMoves.Keys)
                {
                    //Loops through all the moves and chooses the one with the most pieces captured (lost)
                    foreach(Move possibleNextMove in possibleNextMoves[possibleNextPiece])
                    {
                        if (possibleNextMove.JumpedSquares.Count > piecesLostNext)
                        {
                            piecesLostNext = possibleNextMove.JumpedSquares.Count;
                        }
                    }
                }

                //Finds out how many pieces will be lost if piece is NOT moved
                //Warning: Very expensive
                Board = (Board)board.Clone();

                if (Config.DrawIn3D)
                {
                    possibleNextMoves = MovementManager.GeneratePlayerMoves(!BoardManager.Player1Turn, Board);
                }
                else
                {
                    possibleNextMoves = MovementManager3D.GeneratePlayerMoves(!BoardManager3D.Player1Turn, Board);
                }

                foreach (Piece possibleNextPiece in possibleNextMoves.Keys)
                {
                    //Loops through all the moves and chooses the one with the most pieces captured (lost)
                    foreach (Move possibleNextMove in possibleNextMoves[possibleNextPiece])
                    {
                        if (possibleNextMove.JumpedSquares.Count > piecesLostNextIfNotMoved)
                        {
                            piecesLostNextIfNotMoved = possibleNextMove.JumpedSquares.Count;
                        }
                    }
                }

                //Adds the move with extra information to sortedMoves
                sortedMoves.Add(new SortedMove(move, piecesLostNextIfNotMoved, piecesTaken, piecesLostNext));
            }

            Board = null;

            //Sorts the moves
            sortedMoves.OrderBy(x => x.LostIfNotMoved).ThenByDescending(x => x.PiecesTaken).ThenBy(x => x.PiecesLostNext);

            //Adds a random element to the move choosing - all moves with equal 'value' are randomly shuffled
            //Organizes all moves of equal value into smaller lists
            List<List<SortedMove>> DividedSortedMoves = new List<List<SortedMove>>();
            int currentIndex = 0;
            SortedMove previousMove = sortedMoves[0];
            DividedSortedMoves.Add(new List<SortedMove>());
            foreach(SortedMove move in sortedMoves)
            {
                if ((move.LostIfNotMoved != previousMove.LostIfNotMoved) || (move.PiecesTaken != previousMove.PiecesTaken) || (move.PiecesLostNext != previousMove.PiecesLostNext))
                {
                    DividedSortedMoves[currentIndex].Add(previousMove);

                    previousMove = move;
                    currentIndex++;

                    DividedSortedMoves.Add(new List<SortedMove>());
                }
                else
                {
                    DividedSortedMoves[currentIndex].Add(move);
                    previousMove = move;
                }
            }

            //Shuffles the equal moves and adds them back into sortedMoves
            sortedMoves = new List<SortedMove>();
            foreach(List<SortedMove> dividedSortedMoveList in DividedSortedMoves)
            {
                dividedSortedMoveList.Shuffle<SortedMove>();
                sortedMoves.AddRange(dividedSortedMoveList);
            }

            //Chooses a move based on Difficulty and DifficultyFluctuation
            //If indexToChoose is -1, then math is broken
            int indexToChoose = Difficulty <= 0 ? 0 : Difficulty == 1 ? sortedMoves.Count / 2 : Difficulty >= 2 ? sortedMoves.Count - 1 : -1;
            indexToChoose = RandomTools.RandomInt(indexToChoose - DifficultyFluctuation, indexToChoose + DifficultyFluctuation);
            indexToChoose = indexToChoose < 0 ? 0 : indexToChoose > sortedMoves.Count - 1 ? sortedMoves.Count - 1 : indexToChoose;

            return sortedMoves[indexToChoose].Move;
        }
    }

}
