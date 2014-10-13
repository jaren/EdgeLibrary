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
            //Creates a dictionary of pieces that can jump and where they can jump to

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
                            Square topLeftTopLeft = Board.Squares[square.X - 1, square.Y - 1];

                            if ((topLeft.OccupyingPiece != null && topLeft.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[topLeftTopLeft.X, topLeftTopLeft.Y].OccupyingPiece == null)
                            {
                                toReturn.Add(topLeftTopLeft);
                            }
                        }
                        if (piece.X < Board.Size - 2 && piece.Y > 1)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];
                            Square topRightTopRight = Board.Squares[square.X + 2, square.Y - 2];

                            if ((topRight.OccupyingPiece != null && topRight.OccupyingPiece.TopTeam != piece.TopTeam) && Board.Squares[topRightTopRight.X, topRightTopRight.Y].OccupyingPiece == null)
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

        public static Dictionary<Piece, List<Square>> TeamCanMoveTo(bool topTeam)
        {
            //Returns a list of all pieces that can move and all places they can move to
            Dictionary<Piece, List<Square>> toReturn = new Dictionary<Piece, List<Square>>();

            foreach (Square square in Board.Squares)
            {
                Piece piece = square.OccupyingPiece;
                List<Square> validMovements = new List<Square>();

                if (piece != null && piece.TopTeam == topTeam)
                {
                    #region UpwardMovement
                    if (!piece.TopTeam || piece.King)
                    {
                        if (piece.X > 0 && piece.Y > 0)
                        {
                            Square topLeft = Board.Squares[square.X - 1, square.Y - 1];

                            if (topLeft.OccupyingPiece == null)
                            {
                                validMovements.Add(topLeft);
                            }
                        }
                        if (piece.X < Board.Size - 1 && piece.Y > 0)
                        {
                            Square topRight = Board.Squares[square.X + 1, square.Y - 1];

                            if (topRight.OccupyingPiece == null)
                            {
                                validMovements.Add(topRight);
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
                                validMovements.Add(bottomLeft);
                            }
                        }
                        if (piece.X < Board.Size - 1 && piece.Y < Board.Size - 1)
                        {
                            Square bottomRight = Board.Squares[square.X + 1, square.Y + 1];

                            if (bottomRight.OccupyingPiece == null)
                            {
                                validMovements.Add(bottomRight);
                            }
                        }
                    }
                    #endregion DownwardMovement

                    if (validMovements.Count > 0)
                    {
                        toReturn.Add(piece, validMovements);
                    }
                }
            }

            return toReturn;
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
