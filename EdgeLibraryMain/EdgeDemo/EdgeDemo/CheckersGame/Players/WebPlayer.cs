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
<<<<<<< HEAD
        Move PreviousMove = new Move(null, null);
=======
        public int ThisGameID;
        private Move PreviousMove;
>>>>>>> origin/master

        public void CheckForRemoteMove()
        {
            Move RemoteMove = PreviousMove;

<<<<<<< HEAD
            while (RemoteMove == PreviousMove)
            {
                //TODO: Add loading text so user thinks something is happening
                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));

                if (recievedMove != null)
                {
                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));
=======
            do
            {
                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, ThisGameID));

                if (recievedMove != null)
                {
                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, ThisGameID));
>>>>>>> origin/master
                    break;
                }

                Thread.Sleep(1000);
            } while (RemoteMove == PreviousMove);

            base.SendMove(RemoteMove);
        }

        public override bool ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
<<<<<<< HEAD
            base.ReceivePreviousMove(move, possibleMoves);
            PreviousMove = move;
=======
            if (!base.ReceivePreviousMove(move, possibleMoves))
            {
                return false;
            }

>>>>>>> origin/master

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
