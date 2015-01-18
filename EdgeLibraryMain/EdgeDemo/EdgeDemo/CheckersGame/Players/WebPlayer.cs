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

        public static void SendAndRecieve ()
        {
            if (Config.ThisGameType == Config.GameType.Online)
            {
                #region WebServiceConnection

                CheckersServiceClient WebService = new CheckersServiceClient();

                //Send Move to Web Service

                WebService.AddMove(Move.ConvertAndSend(CurrentMove), Config.ThisGameID);
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
                    else if (loop == 120)
                    {
                        loop = -1;
                    }

                    loop++;
                }

                //Duplicate This Function

                //Set the current move and subscribe to it

                CurrentMove = RemoteMove;
                #endregion WebServiceConnection
            }
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
