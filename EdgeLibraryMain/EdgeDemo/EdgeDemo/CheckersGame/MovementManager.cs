using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
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

        public static Dictionary<Piece, List<Move>> GenerateTeamMoves(bool topTeam)
        {
            Dictionary<Piece, List<Move>> Moves = new Dictionary<Piece, List<Move>>();

            Dictionary<Piece, List<Move>> jumps = TeamCanMultiJumpTo(topTeam); //Single jumps
            foreach (Piece piece in jumps.Keys)
            {
                Moves.Add(piece, jumps[piece]);
            }

            Dictionary<Piece, List<Square>> moves = new Dictionary<Piece, List<Square>>();
            //If a team cannot jump
            if (Moves.Count == 0)
            {
                moves = TeamCanMoveTo(topTeam);
                foreach(KeyValuePair<Piece, List<Square>> move in moves)
                {
                    List<Move> moveList = new List<Move>();
                    foreach(Square square in move.Value)
                    {
                        moveList.Add(new Move(new List<Square>() { Board.Squares[move.Key.X, move.Key.Y], square }));
                    }
                    Moves.Add(move.Key, moveList);
                }
            }

            return Moves;
        }

        public static bool TeamCanJump(bool topTeam)
        {
            //Returns whether a team can jump or not
            //Almost unnecessary as TeamCanJumpTo returns more information
            foreach (Square square in Board.Squares)
            {
                Piece piece = square.OccupyingPiece;
                if (piece != null && piece.TopTeam == topTeam)
                {
                    #region BottomTeam
                    if (!piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y > 1)
                        {
                            Square topLeft = Board.Squares[square.X - 1, square.Y - 1];
                            Square topLeftTopLeft = Board.Squares[square.X - 1, square.Y - 1];

                            if (topLeft.OccupyingPiece != null && topLeft.OccupyingPiece.TopTeam != piece.TopTeam)
                            {
                                if (Board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                                {
                                    return true;
                                }
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = Board.Squares[square.X + 2, square.Y - 2];

                            if (topRight.OccupyingPiece != null && topRight.OccupyingPiece.TopTeam != piece.TopTeam)
                            {
                                if (Board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    #endregion BottomTeam
                    #region TopTeam
                    if (piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y < Board.Size - 2)
                        {
                            Square bottomLeft = Board.Squares[square.X - 1, square.Y + 1];
                            Square bottomLeftBottomLeft = Board.Squares[square.X - 2, square.Y + 2];
                            if (bottomLeft.OccupyingPiece != null && bottomLeft.OccupyingPiece.TopTeam != piece.TopTeam)
                            {
                                if (Board.Squares[bottomLeftBottomLeft.X, bottomLeftBottomLeft.Y].OccupyingPiece == null)
                                {
                                    return true;
                                }
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y < Board.Size - 2)
                        {
                            Square bottomRight = Board.Squares[square.X + 1, square.Y + 1];
                            Square bottomRightBottomRight = Board.Squares[square.X + 2, square.Y + 2];
                            if (bottomRight.OccupyingPiece != null && bottomRight.OccupyingPiece.TopTeam != piece.TopTeam)
                            {
                                if (Board.Squares[bottomRightBottomRight.X, bottomRightBottomRight.Y].OccupyingPiece == null)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    #endregion TopTeam
                }
            }
            return false;
        }

        public static Dictionary<Piece, List<Square>> TeamCanJumpTo(bool topTeam)
        {
            Dictionary<Piece, List<Square>> toReturn = new Dictionary<Piece, List<Square>>();
            //Creates a dictionary of pieces that can jump and where they can jump to - only single jumps

            foreach (Square square in Board.Squares)
            {
                Piece piece = square.OccupyingPiece;
                List<Square> validJumps = new List<Square>();

                if (piece != null && piece.TopTeam == topTeam)
                {
                    #region BottomTeam
                    if (!piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y > 1)
                        {
                            Square topLeft = Board.Squares[square.X - 1, square.Y - 1];
                            Square topLeftTopLeft = Board.Squares[square.X - 1, square.Y - 1];

                            if ((topLeft.OccupyingPiece != null && topLeft.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(topLeftTopLeft);
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = Board.Squares[square.X + 2, square.Y - 2];

                            if ((topRight.OccupyingPiece != null && topRight.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(topRightTopRight);
                            }
                        }
                    }
                    #endregion BottomTeam
                    #region TopTeam
                    if (piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y < Board.Size - 2)
                        {
                            Square bottomLeft = Board.Squares[square.X - 1, square.Y + 1];
                            Square bottomLeftBottomLeft = Board.Squares[square.X - 2, square.Y + 2];
                            if ((bottomLeft.OccupyingPiece != null && bottomLeft.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[bottomLeftBottomLeft.X, bottomLeftBottomLeft.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(bottomLeftBottomLeft);
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y < Board.Size - 2)
                        {
                            Square bottomRight = Board.Squares[square.X + 1, square.Y + 1];
                            Square bottomRightBottomRight = Board.Squares[square.X + 2, square.Y + 2];
                            if ((bottomRight.OccupyingPiece != null && bottomRight.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[bottomRightBottomRight.X, bottomRightBottomRight.Y].OccupyingPiece == null)
                            {
                                validJumps.Add(bottomRightBottomRight);
                            }
                        }
                    }
                    #endregion TopTeam

                    if (validJumps.Count > 0)
                    {
                        toReturn.Add(piece, validJumps);
                    }
                }
            }

            return toReturn;
        }

        public static List<Square> JumpsFromSquare(Piece jumpingPiece, int jumpToX, int jumpToY)
        {
            List<Square> toReturn = new List<Square>();

            Piece ghostPiece = jumpingPiece;
            ghostPiece.X = jumpToX;
            ghostPiece.Y = jumpToY;

            return PieceCanJumpTo(ghostPiece);
        }

        public static List<Square> PieceCanJumpTo(Piece piece)
        {
            //Returns a list of squares a specific piece can jump to

            List<Square> toReturn = new List<Square>();
            Square square;

            foreach (Square s in Board.Squares)
            {
                if (s.OccupyingPiece == piece)
                {
                    square = s;

                    #region BottomTeam
                    if (!piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y > 1)
                        {
                            Square topLeft = Board.Squares[square.X - 1, square.Y - 1];
                            Square topLeftTopLeft = Board.Squares[square.X - 2, square.Y - 2];

                            if ((topLeft.OccupyingPiece != null && (topLeft.OccupyingPiece.TopTeam != piece.TopTeam)) && Board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(topLeftTopLeft);
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = Board.Squares[square.X + 2, square.Y - 2];

                            if ((topRight.OccupyingPiece != null && (topRight.OccupyingPiece.TopTeam != piece.TopTeam)) && Board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(topRightTopRight);
                            }
                        }
                    }
                    #endregion BottomTeam
                    #region TopTeam
                    if (piece.TopTeam || piece.King)
                    {
                        if (piece.X > 1 && piece.Y < Board.Size - 2)
                        {
                            Square bottomLeft = Board.Squares[square.X - 1, square.Y + 1];
                            Square bottomLeftBottomLeft = Board.Squares[square.X - 2, square.Y + 2];
                            if ((bottomLeft.OccupyingPiece != null && bottomLeft.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[bottomLeftBottomLeft.X, bottomLeftBottomLeft.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(bottomLeftBottomLeft);
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y < Board.Size - 2)
                        {
                            Square bottomRight = Board.Squares[square.X + 1, square.Y + 1];
                            Square bottomRightBottomRight = Board.Squares[square.X + 2, square.Y + 2];
                            if ((bottomRight.OccupyingPiece != null && bottomRight.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[bottomRightBottomRight.X, bottomRightBottomRight.Y].OccupyingPiece == null)
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

        public static List<Move> PieceCanMultiJumpTo(Piece piece)
        {
            List<Move> MultiJumps = new List<Move>();
            List<Square> originalJumps = PieceCanJumpTo(piece);

            //NOTES: Ask for client input even if there is only one jump available: This will not involve fake anythings

            foreach(Square square in originalJumps)
            {
                //Temporary function - only checks for single jumps
                MultiJumps.Add(new Move(new List<Square>() { Board.Squares[piece.X, piece.Y], square }, new List<Square>() { Board.GetSquareBetween(Board.Squares[piece.X, piece.Y], square) }));
            }

            return MultiJumps;
        }

        public static Move CheckForMoreMoves(Piece piece)
        {
            //TODO: Complete
        }

        public static Dictionary<Piece, List<Move>> TeamCanMultiJumpTo(bool topTeam)
        {
            Dictionary<Piece, List<Move>> multiJumps = new Dictionary<Piece, List<Move>>();

            foreach(Square square in Board.Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    if (square.OccupyingPiece.TopTeam == topTeam)
                    {
                        List<Move> validJumps = PieceCanMultiJumpTo(square.OccupyingPiece);
                        if (validJumps.Count != 0)
                        {
                            multiJumps.Add(square.OccupyingPiece, validJumps);
                        }
                    }
                }
            }
            return multiJumps;
        }

        public static Dictionary<Piece, List<Square>> TeamCanMoveTo(bool topTeam)
        {
            Dictionary<Piece, List<Square>> moves = new Dictionary<Piece, List<Square>>();

            foreach (Square square in Board.Squares)
            {
                if (square.OccupyingPiece != null)
                {
                    if (square.OccupyingPiece.TopTeam == topTeam)
                    {
                        List<Square> validMoves = PieceCanMoveTo(square.OccupyingPiece);
                        if (validMoves.Count != 0)
                        {
                            moves.Add(square.OccupyingPiece, validMoves);
                        }
                    }
                }
            }
            return moves;
        }

        public static List<Square> PieceCanMoveTo(Piece piece)
        {
            //Returns a list of squares a specific piece can move to

            List<Square> toReturn = new List<Square>();
            Square square;

            foreach (Square s in Board.Squares)
            {
                if (s.OccupyingPiece == piece)
                {
                    square = s;
                    #region UpwardMovement
                    if (!piece.TopTeam || piece.King)
                    {
                        if (piece.X > 0 && piece.Y > 0)
                        {
                            Square topLeft = Board.Squares[square.X - 1, square.Y - 1];

                            if (topLeft.OccupyingPiece == null)
                            {
                                toReturn.Add(topLeft);
                            }
                        }
                        if (piece.X < Board.Size - 1 && piece.Y > 0)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];

                            if (topRight.OccupyingPiece == null)
                            {
                                toReturn.Add(topRight);
                            }
                        }
                    }
                    #endregion UpwardMovement
                    #region DownwardMovement
                    if (piece.TopTeam || piece.King)
                    {
                        if (piece.X > 0 && piece.Y < Board.Size - 1)
                        {
                            Square bottomLeft = Board.Squares[square.X - 1, square.Y + 1];

                            if (bottomLeft.OccupyingPiece == null)
                            {
                                toReturn.Add(bottomLeft);
                            }
                        }
                        if (piece.X < Board.Size - 1 && piece.Y < Board.Size - 1)
                        {
                            Square bottomRight = Board.Squares[square.X + 1, square.Y + 1];

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
