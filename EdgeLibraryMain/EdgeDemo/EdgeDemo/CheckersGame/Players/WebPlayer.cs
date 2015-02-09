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
        /// <summary>
        /// If the web player is Player2 (the joiner) use this overload
        /// </summary>
        /// <param name="hostTeamName">Team name of the hosting team</param>
        public WebPlayer(string hostTeamName)
        {
            IsHost = false;
            ThisGameID = WebService.CreateGame(hostTeamName);
        }
        /// <summary>
        /// If the web player is Player1 (the host) use this overload
        /// </summary>
        /// <param name="gameId">ID of the joined game</param>
        /// <param name="player2name">The name of the joining team</param>
        public WebPlayer(int gameId, string player2name)
        {
            ThisGameID = gameId;
            IsHost = true;
            WebService.JoinGame(gameId, player2name);
        }

        CheckersServiceClient WebService = new CheckersServiceClient();
        private Thread waitForMoveThread;
        Move PreviousMove = new Move(null, null);
        public int ThisGameID;
        public bool IsHost;

        public void CheckForRemoteMove()
        {
            Move RemoteMove = PreviousMove;

            while (RemoteMove == PreviousMove)
            {
                //TODO: Add loading text so user thinks something is happening
                Move recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));

                if (recievedMove != null)
                {
                    RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(Config.ThisGameID));
                    do
                    {
                        recievedMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(ThisGameID));

                        if (recievedMove != null)
                        {
                            RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(ThisGameID));
                            break;
                        }

                        Thread.Sleep(1000);
                    } while (RemoteMove == PreviousMove);

                    base.SendMove(RemoteMove);
                }
            }
        }

        public override bool ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            base.ReceivePreviousMove(move, possibleMoves);
            PreviousMove = move;
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
