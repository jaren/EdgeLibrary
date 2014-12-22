using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckersService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CheckersService : ICheckersService
    {
        public List<SimpleMove> Moves = new List<SimpleMove>();
        public Dictionary<int,GameManager> Games = new Dictionary<int,GameManager>();

        public void AddMove(SimpleMove move)
        {
            Moves.Add(move);
        }

        public SimpleMove GetLatestMoveFrom(bool topTeam)
        {
            if (Moves.Count > 0 && Moves[Moves.Count - 1].TopTeam == topTeam)
            {
                return Moves[Moves.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public int CreateGame(string hostTeamName)
        {
            GameManager Game = new GameManager(hostTeamName);
            Games.Add(Games.Count,Game);
            return Games.Count - 1;
        }

        public void JoinGame(int gameId, string otherTeamName)
        {
            Games[gameId].OtherTeamName = otherTeamName;
            Games[gameId].State = GameManager.GameState.InProgress;
        }

        public void Disconnect(int gameId, bool isHost)
        {
            if (isHost)
            {
                Games[gameId].State = GameManager.GameState.HostDisconnected;
            }
            else
            {
                Games[gameId].State = GameManager.GameState.PlayerDisconnected;
            }
        }

        public void EndGame(int gameId)
        {
            Games[gameId].State = GameManager.GameState.Ended;
        }

        public List<GameManager> GetJoinableGames()
        {
            List<GameManager> JoinableGames = new List<GameManager>();

            foreach (int gameId in Games.Keys)
            {
                if (Games[gameId].State == GameManager.GameState.WaitingForPlayers)
                {
                    JoinableGames.Add(Games[gameId]);
                }
            }

            return JoinableGames;
        }

        public List<GameManager> GetAllGames()
        {
            return Games.Values.ToList();
        }
    }
}
