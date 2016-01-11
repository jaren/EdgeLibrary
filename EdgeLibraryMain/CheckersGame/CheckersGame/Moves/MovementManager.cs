using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CheckersGame
{
    public static class MovementManager
    {
        /*
         * All code sections in this class named "BottomTeam" check for jumps for all pieces on the
         * bottom team OR kinged pieces on the top team. Code sections named "TopTeam" do exactly the opposite.
         * 
         * All code sections named "UpwardMovement" check for possible moves for pieces on the bottom team OR
         * for kinged pieces on the top team. Code sections named "DownwardMovement" do the exact opposite.
         */

        /*
         * General math used in all of the below functions:
         * 
         * If a piece is in the top row or the left column, it cannot move up and to the left
         * If a piece is in the top row or the right column, it cannot move up and to the right
         * If a piece is in the bottom row or the left column, it cannot move down and to the left
         * If a piece is in the bottom row or the right column, it cannot move down and to the right
         * 
         * If a piece is in the top two rows or the left two columns, it cannot jump up and to the left
         * etc.
         */

        public static Piece GetCorrespondingPiece(Piece p)
        {
            foreach (Square s in BoardManager.Instance.Board.Squares)
            {
                if (s.Location() == p.Location())
                {
                    return s.OccupyingPiece;
                }
            }

            return null;
        }

        public static Dictionary<Piece, List<Move>> GeneratePlayerMoves(bool player1, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            Dictionary<Piece, List<Move>> Moves = new Dictionary<Piece, List<Move>>();

            Dictionary<Piece, List<Move>> jumps = PlayerCanMultiJumpTo(player1, board); //Single jumps
            foreach (Piece piece in jumps.Keys)
            {
                Moves.Add(GetCorrespondingPiece(piece), jumps[piece]);
            }

            Dictionary<Piece, List<Square>> moves = new Dictionary<Piece, List<Square>>();
            //If a player cannot jump
            if (Moves.Count == 0)
            {
                moves = PlayerCanMoveTo(player1, board);
                foreach (KeyValuePair<Piece, List<Square>> move in moves)
                {
                    List<Move> moveList = new List<Move>();
                    foreach (Square square in move.Value)
                    {
                        moveList.Add(new Move(new List<Square>() { board.Squares[move.Key.X, move.Key.Y], square }));
                    }
                    Moves.Add(GetCorrespondingPiece(move.Key), moveList);
                }
            }

            return Moves;
        }

        public static Dictionary<Piece, List<Square>> PlayerCanJumpTo(bool player1, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            //Creates a dictionary of pieces that can jump and where they can jump to - only single jumps
            Dictionary<Piece, List<Square>> toReturn = new Dictionary<Piece, List<Square>>();

            foreach (Square square in board.Squares)
            {
                Piece piece = square.OccupyingPiece;
                List<Square> validJumps = new List<Square>();

                if (piece != null && piece.Player1 == player1)
                {
                    #region Player 2
                    if (!piece.Player1 || piece.King)
                    {
                        if (piece.X > 1 && piece.Y > 1)
                        {
                            Square topLeft = board.Squares[square.X - 1, square.Y - 1];
                            Square topLeftTopLeft = board.Squares[square.X - 1, square.Y - 1];

                            if ((topLeft.OccupyingPiece != null && topLeft.OccupyingPiece.Player1 != piece.Player1) && board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(topLeftTopLeft);
                            }
                        }
                        if (piece.X < board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = board.Squares[square.X + 2, square.Y - 2];

                            if ((topRight.OccupyingPiece != null && topRight.OccupyingPiece.Player1 != piece.Player1) && board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(topRightTopRight);
                            }
                        }
                    }
                    #endregion Player 2
                    #region Player 1
                    if (piece.Player1 || piece.King)
                    {
                        if (piece.X > 1 && piece.Y < board.Size - 2)
                        {
                            Square bottomLeft = board.Squares[square.X - 1, square.Y + 1];
                            Square bottomLeftBottomLeft = board.Squares[square.X - 2, square.Y + 2];
                            if ((bottomLeft.OccupyingPiece != null && bottomLeft.OccupyingPiece.Player1 != piece.Player1) && board.Squares[bottomLeftBottomLeft.X, bottomLeftBottomLeft.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(bottomLeftBottomLeft);
                            }
                        }
                        if (piece.X < board.Size - 2 && piece.Y < board.Size - 2)
                        {
                            Square bottomRight = board.Squares[square.X + 1, square.Y + 1];
                            Square bottomRightBottomRight = board.Squares[square.X + 2, square.Y + 2];
                            if ((bottomRight.OccupyingPiece != null && bottomRight.OccupyingPiece.Player1 != piece.Player1) && board.Squares[bottomRightBottomRight.X, bottomRightBottomRight.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(bottomRightBottomRight);
                            }
                        }
                    }
                    #endregion Player 1

                    if (validJumps.Count > 0)
                    {
                        toReturn.Add(piece, validJumps);
                    }
                }
            }

            return toReturn;
        }

        //Returns a list of squares a specific piece can jump to
        public static List<Square> PieceCanJumpTo(Piece piece, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            List<Square> toReturn = new List<Square>();
            Square square;

            foreach (Square s in board.Squares)
            {
                if (s.OccupyingPiece == piece || piece.Fake)
                {
                    if (piece.Fake)
                    {
                        square = board.Squares[piece.X, piece.Y];
                    }
                    else
                    {
                        square = s;
                    }

                    #region BottomTeam
                    if (!piece.Player1 || piece.King)
                    {
                        if (piece.X > 1 && piece.Y > 1)
                        {
                            Square topLeft = board.Squares[square.X - 1, square.Y - 1];
                            Square topLeftTopLeft = board.Squares[square.X - 2, square.Y - 2];

                            if ((topLeft.OccupyingPiece != null && (topLeft.OccupyingPiece.Player1 != piece.Player1)) && board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(topLeftTopLeft);
                            }
                        }
                        if (piece.X < board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = board.Squares[square.X + 2, square.Y - 2];

                            if ((topRight.OccupyingPiece != null && (topRight.OccupyingPiece.Player1 != piece.Player1)) && board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(topRightTopRight);
                            }
                        }
                    }
                    #endregion BottomTeam
                    #region TopTeam
                    if (piece.Player1 || piece.King)
                    {
                        if (piece.X > 1 && piece.Y < board.Size - 2)
                        {
                            Square bottomLeft = board.Squares[square.X - 1, square.Y + 1];
                            Square bottomLeftBottomLeft = board.Squares[square.X - 2, square.Y + 2];
                            if ((bottomLeft.OccupyingPiece != null && bottomLeft.OccupyingPiece.Player1 != piece.Player1) && board.Squares[bottomLeftBottomLeft.X, bottomLeftBottomLeft.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(bottomLeftBottomLeft);
                            }
                        }
                        if (piece.X < board.Size - 2 && piece.Y < board.Size - 2)
                        {
                            Square bottomRight = board.Squares[square.X + 1, square.Y + 1];
                            Square bottomRightBottomRight = board.Squares[square.X + 2, square.Y + 2];
                            if ((bottomRight.OccupyingPiece != null && bottomRight.OccupyingPiece.Player1 != piece.Player1) && board.Squares[bottomRightBottomRight.X, bottomRightBottomRight.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(bottomRightBottomRight);
                            }
                        }
                    }
                    #endregion TopTeam

                    break;
                }
            }

            return toReturn;
        }

        public static List<Move> GenerateNewListOfJumps(Piece piece)
        {
            List<Move> CompletedJumpSequences = new List<Move>();
            Board fakeBoard = (Board)BoardManager.Instance.Board.Clone();
            PieceCanMultiJumpTo(fakeBoard.Squares[piece.X, piece.Y].OccupyingPiece, fakeBoard, CompletedJumpSequences);
            return CompletedJumpSequences;
        }

        private static void PieceCanMultiJumpTo(Piece piece, Board FakeBoard, List<Move> JumpSequences, Move CurrentMove = null)
        {
            foreach (Square jump in PieceCanJumpTo(piece, FakeBoard))
            {
                Move MoveToRun;

                if (CurrentMove == null)
                {
                    CurrentMove = new Move(new List<Square>() { FakeBoard.Squares[piece.X, piece.Y], jump }, new List<Square>() { FakeBoard.GetSquareBetween(FakeBoard.Squares[piece.X, piece.Y], jump) });
                    MoveToRun = CurrentMove.SwitchBoards(BoardManager.Instance.Board);
                }
                else
                {
                    CurrentMove.SquarePath.Add(jump);
                    CurrentMove.JumpedSquares.Add(FakeBoard.GetSquareBetween(FakeBoard.Squares[piece.X, piece.Y], jump));
                    MoveToRun = new Move(new List<Square>() { CurrentMove.SquarePath[CurrentMove.SquarePath.Count - 2], CurrentMove.SquarePath.Last() }, new List<Square>() { CurrentMove.JumpedSquares.Last() });
                }

                MoveToRun.SwitchBoards(FakeBoard).RunMove(FakeBoard);

                PieceCanMultiJumpTo(CurrentMove.SquarePath.Last().OccupyingPiece, FakeBoard, JumpSequences, CurrentMove);
            }

            if (CurrentMove != null && !JumpSequences.Contains(CurrentMove))
            {
                JumpSequences.Add(CurrentMove.SwitchBoards(BoardManager.Instance.Board));
            }
        }

        public static Dictionary<Piece, List<Move>> PlayerCanMultiJumpTo(bool topTeam, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            Dictionary<Piece, List<Move>> multiJumps = new Dictionary<Piece, List<Move>>();

            foreach (Square square in board.Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    if (square.OccupyingPiece.Player1 == topTeam)
                    {
                        List<Move> validJumps = GenerateNewListOfJumps(square.OccupyingPiece);
                        if (validJumps.Count != 0)
                        {
                            multiJumps.Add(square.OccupyingPiece, validJumps);
                        }
                    }
                }
            }
            return multiJumps;
        }

        public static List<Square> GetJumpsFromSquare(Piece jumpingPiece, Square originSquare)
        {
            List<Square> jumps = new List<Square>();
            Piece fakePiece = new Piece("none", Vector2.Zero, Color.White, 0f, jumpingPiece.Player1);
            fakePiece.X = originSquare.X;
            fakePiece.Y = originSquare.Y;
            fakePiece.King = jumpingPiece.King;
            fakePiece.Fake = true;

            return PieceCanJumpTo(fakePiece);
        }

        public static Dictionary<Piece, List<Square>> PlayerCanMoveTo(bool topTeam, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            Dictionary<Piece, List<Square>> moves = new Dictionary<Piece, List<Square>>();

            foreach (Square square in board.Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    if (square.OccupyingPiece.Player1 == topTeam)
                    {
                        List<Square> validMoves = PieceCanMoveTo(square.OccupyingPiece);
                        if (validMoves.Count != 0)
                        {
                            moves.Add(GetCorrespondingPiece(square.OccupyingPiece), validMoves);
                        }
                    }
                }
            }
            return moves;
        }

        //Returns a list of squares a specific piece can move to
        public static List<Square> PieceCanMoveTo(Piece piece, Board board = null)
        {
            //Must be set here because BoardManager.Instance.Board is not a compile-time constant
            if (board == null)
            {
                board = BoardManager.Instance.Board;
            }

            List<Square> toReturn = new List<Square>();
            Square square;

            foreach (Square s in board.Squares)
            {
                if (s.OccupyingPiece == piece)
                {
                    square = s;
                    #region UpwardMovement
                    if (!piece.Player1 || piece.King)
                    {
                        if (piece.X > 0 && piece.Y > 0)
                        {
                            Square topLeft = board.Squares[square.X - 1, square.Y - 1];

                            if (topLeft.OccupyingPiece == null)
                            {
                                toReturn.Add(topLeft);
                            }
                        }
                        if (piece.X < board.Size - 1 && piece.Y > 0)
                        {
                            Square topRight = board.Squares[square.X + 1, square.Y - 1];

                            if (topRight.OccupyingPiece == null)
                            {
                                toReturn.Add(topRight);
                            }
                        }
                    }
                    #endregion UpwardMovement
                    #region DownwardMovement
                    if (piece.Player1 || piece.King)
                    {
                        if (piece.X > 0 && piece.Y < board.Size - 1)
                        {
                            Square bottomLeft = board.Squares[square.X - 1, square.Y + 1];

                            if (bottomLeft.OccupyingPiece == null)
                            {
                                toReturn.Add(bottomLeft);
                            }
                        }
                        if (piece.X < board.Size - 1 && piece.Y < board.Size - 1)
                        {
                            Square bottomRight = board.Squares[square.X + 1, square.Y + 1];

                            if (bottomRight.OccupyingPiece == null)
                            {
                                toReturn.Add(bottomRight);
                            }
                        }
                    }
                    #endregion DownwardMovement
                    break;
                }
            }

            return toReturn;
        }
    }
}
