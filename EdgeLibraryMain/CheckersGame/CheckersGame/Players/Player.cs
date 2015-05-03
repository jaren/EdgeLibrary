using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersGame
{
    //The base for all other 'players'
    public class Player
    {
        public delegate void PlayerMoveEvent(Move move);
        public event PlayerMoveEvent OnRunMove;

        public bool CanMove = false;

        public string Name;

        public Player(string name)
        {
            Name = name;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }

        //The player gets send the previous move and possible moves - if there are no possible moves, return false
        public virtual bool ReceivePreviousMove(Move previousMove, Dictionary<Piece, List<Move>> possibleMoves)
        {
            if (possibleMoves.Count == 0) 
            {
                return false;
            } 

            CanMove = true;
            return true; 
        }

        //The player sends a move to be executed by BoardManager - only works if it is 
        protected void SendMove(Move move)
        {
            if (CanMove)
            {
                if (OnRunMove != null)
                {
                    OnRunMove(move);
                }

                CanMove = false;
            }
        }
    }
}
