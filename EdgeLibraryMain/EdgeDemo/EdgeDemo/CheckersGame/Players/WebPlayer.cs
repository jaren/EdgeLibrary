using EdgeDemo.CheckersService;
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

        CheckersServiceClient WebService = new CheckersServiceClient();

        public static void SendAndRecieve()
        {
            //Send Move to Web Service


            //Duplicate This Function

            //Set the current move and subscribe to it
        }

        public override void ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            WebService.AddMove(Move.ConvertAndSend(move), Config.ThisGameID);
            Move RemoteMove = null;
            int loop = 0;

            while (RemoteMove == null)
            {
                if (loop == 0)
                {
                    //TODO: Add loading text so user thinks something is happening
                    Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, Config.ThisGameID));

                    if (recievedMove != null)
                    {
                        RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(BoardManager.Player1Turn, Config.ThisGameID));
                        break;
                    }
                }
                else if (loop == Config.WebServiceCheck)
                {
                    loop = -1;
                }

                loop++;
            }

            base.SendMove(RemoteMove);

        }

        public override void Draw(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
