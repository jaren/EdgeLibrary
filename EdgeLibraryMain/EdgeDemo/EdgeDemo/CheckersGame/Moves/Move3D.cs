using EdgeDemo.CheckersService;
using EdgeLibrary;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame
{
    public class Move3D
    {
        public string ID;
        public List<Square3D> SquarePath;
        public List<Square3D> JumpedSquares;
        public Piece3D Piece;
        public int MoveIndex;
        public Square3D StartSquare
        {
            get
            {
                return SquarePath[0];
            }
        }
        public Square3D FinishSquare
        {
            get
            {
                return SquarePath[SquarePath.Count - 1];
            }
        }
        public delegate void MoveEvent(List<Square3D> squarePath, List<Square3D> jumpedSquares, int index);
        public event MoveEvent OnComplete;

        public Move3D(List<Square3D> squarePath, List<Square3D> jumpedSquares = null)
        {
            ID = this.GenerateID();

            SquarePath = squarePath;
            //TODO: IMPLEMENT WITH 3D Piece = StartSquare.OccupyingPiece;
            JumpedSquares = jumpedSquares == null ? new List<Square3D>() : jumpedSquares;
            
            MoveIndex = 0;
        }

        ASequence3D MoveSequence;
        public void RunMove(Board3D boardToRunOn)
        {
            if(boardToRunOn == null)
            {
                boardToRunOn = BoardManager3D.Board;
            }

            List<Action3D> Moves = new List<Action3D>();
            foreach (Square3D square in SquarePath)
            {
                //TODO: IMPLEMENT WITH 3D Moves.Add(new AMoveTo3D(square.Position, Config.CheckerMoveSpeed));
            }
            Moves.RemoveAt(0);
            MoveSequence = new ASequence3D(Moves);
            MoveSequence.OnActionTransition += MoveSequence_OnTransition;

            //TODO: IMPLEMENT WITH 3D Piece.AddAction(MoveSequence);

            //TODO: IMPLEMENT WITH 3D SquarePath[0].OccupyingPiece = null;
            //TODO: IMPLEMENT WITH 3D SquarePath[SquarePath.Count - 1].SetPiece(Piece);

            //Temporary workaround
            foreach (Square3D square in JumpedSquares)
            {
                //TODO: IMPLEMENT WITH 3D boardToRunOn.CapturePiece(square.OccupyingPiece);
            }
            if (boardToRunOn == BoardManager3D.Board)
            {
                //TODO: IMPLEMENT WITH 3D BoardManager3D.CaptureSprite.Text = "Top Team Captures: " + boardToRunOn.TopTeamCaptures + "\nBottom Team Captures: " + boardToRunOn.BottomTeamCaptures;
            }
            //********************

            /*TODO: IMPLEMENT WITH 3D if ((Piece.Player1 && SquarePath[SquarePath.Count - 1].Y == Config.BoardSize - 1) || (!Piece.Player1 && SquarePath[SquarePath.Count - 1].Y == 0))
            {
                Piece.King = true;
            }
             */
        }

        void MoveSequence_OnTransition(ASequence3D sequence, Action3D action, Sprite3D sprite, GameTime gameTime)
        {
            OnComplete(SquarePath, JumpedSquares, MoveIndex++);
        }

        //Moves a move from one board to another
        public Move SwitchBoards(Board3D board)
        {
            /* TODO: IMPLEMENT WITH 3D 
            Move3D newMove = new Move3D(SquarePath);
            newMove.SquarePath = new List<Square3D>();
            foreach (Square3D square in SquarePath)
            {
                newMove.SquarePath.Add(board.Squares[square.X, square.Y]);
            }
            foreach (Square3D square in JumpedSquares)
            {
                newMove.JumpedSquares.Add(board.Squares[square.X, square.Y]);
            }
            newMove.Piece = board.Squares[SquarePath[0].X, SquarePath[0].Y].OccupyingPiece;
            return newMove;
             */
            return null;
        }
    }
}
