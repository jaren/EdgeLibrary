using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdgeDemo.CheckersGame.Players
{
    //A player used by the remote person playing the game
    public class WebPlayer : Player
    {
        //Move everything service related here?
        public WebPlayer()
        {

        }

        public override void ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
