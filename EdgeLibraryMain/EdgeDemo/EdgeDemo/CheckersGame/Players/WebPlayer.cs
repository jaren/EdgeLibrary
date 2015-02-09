using EdgeDemo.CheckersService;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EdgeDemo.CheckersGame
{
    //A player used by the remote person playing the game
    public class WebPlayer : Player
    {
        //Move everything service related here?
        public WebPlayer()
        {

        }

        CheckersServiceClient WebService = new CheckersServiceClient();
        private Thread waitForMoveThread;
        Move PreviousMove = new Move(null, null);

        public void CheckForRemoteMove()
        {
            Move RemoteMove = null;

            while (RemoteMove == PreviousMove)
            {
                //TODO: Add loading text so user thinks something is happening
                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));

                if (recievedMove != null)
                {
                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));
                    break;
                }

                Thread.Sleep(250);
            }

            base.SendMove(RemoteMove);
        }

        public override void ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            base.ReceivePreviousMove(move, possibleMoves);
            PreviousMove = move;

            WebService.AddMove(Move.ConvertAndSend(move), Config.ThisGameID);

            waitForMoveThread = new Thread(CheckForRemoteMove);
            waitForMoveThread.Start();
            BoardManager.MessageSprite.Display("Waiting For the Other Player...");
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
