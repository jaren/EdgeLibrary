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
        public WebPlayer(string hostTeamName, bool createGame = true)
        {
            IsHost = false;
            if (createGame)
            {
                ThisGameID = WebService.CreateGame(hostTeamName);
                while (WebService.GetSpecificGames(GameState.State.WaitingForPlayers).ContainsKey(ThisGameID))
                {
                    //Waiting For Another Player...
                }

                TeamName = hostTeamName;
            }
        }
        /// <summary>
        /// If the web player is Player1 (the host) use this overload
        /// </summary>
        /// <param name="gameId">ID of the joined game</param>
        /// <param name="player2name">The name of the joining team</param>
        public WebPlayer(int gameId, string player2name, bool joinGame = true)
        {
            ThisGameID = gameId;
            IsHost = true;
            if (joinGame)
            {
                WebService.JoinGame(gameId, player2name);
            }

            TeamName = player2name;
        }

        CheckersServiceClient WebService = new CheckersServiceClient();
        private Thread waitForMoveThread;
        Move PreviousMove = null;
        public int ThisGameID;
        public bool IsHost;
        public string TeamName;

        public void CheckForRemoteMove()
        {
            SimpleMove ProcessedPreviousMove = Move.ConvertAndSend(PreviousMove);
            Move RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(ThisGameID));
            //TODO: Add loading text so user thinks something is happening
            while(ProcessedPreviousMove.ID == RemoteMove.ID)
            {
                Thread.Sleep(1000);
                RemoteMove = Move.ConvertAndRecieve(WebService.GetLatestMoveFrom(ThisGameID));
            }

            base.SendMove(RemoteMove);
        }

        public override bool ReceivePreviousMove(Move move, Dictionary<Piece, List<Move>> possibleMoves)
        {
            if (move != null)
            {
                base.ReceivePreviousMove(move, possibleMoves);
                PreviousMove = move;
                if (!base.ReceivePreviousMove(move, possibleMoves))
                {
                    return false;
                }

                WebService.AddMove(Move.ConvertAndSend(move), ThisGameID);

                PreviousMove = move;
            }
            CheckForRemoteMove();
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
