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
        public WebPlayer(int thisGameID)
        {
            ThisGameID = thisGameID;
        }

        CheckersServiceClient WebService = new CheckersServiceClient();
        private Thread waitForMoveThread;
        public int ThisGameID;
        private Move PreviousMove;

        public void CheckForRemoteMove()
        {
            Move RemoteMove = PreviousMove;

            do
            {
                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, ThisGameID));

                if (recievedMove != null)
                {
                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, ThisGameID));
                    break;
                }

                Thread.Sleep(1000);
            } while (RemoteMove == PreviousMove);

            base.SendMove(RemoteMove);
        }

        public override bool ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            if (!base.ReceivePreviousMove(move, possibleMoves))
            {
                return false;
            }


            WebService.AddMove(Move.ConvertAndSend(move), ThisGameID);

            PreviousMove = move;
            waitForMoveThread = new Thread(CheckForRemoteMove);
            waitForMoveThread.Start();
            //TODO: Switch to waiting for other player screen here.

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
