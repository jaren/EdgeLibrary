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
        public Dictionary<int,GameManager> Games = new Dictionary<int,GameManager>();

        public bool AddMove(SimpleMove move, int gameId)
        {
            if (Games.Count >= gameId + 1)
            {
                Games[gameId].MoveList.Add(move);
                return true;
            }
            else
            {
                return false;
            }
        }

        public SimpleMove GetLatestMoveFrom(bool topTeam, int gameId)
        {
            if (Games.Count >= gameId + 1 && Games[gameId].MoveList.Count > 0 && Games[gameId].MoveList[Games[gameId].MoveList.Count - 1].TopTeam == topTeam)
            {
                return Games[gameId].MoveList[Games[gameId].MoveList.Count - 1];
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

        public Dictionary<int,GameManager> GetJoinableGames()
        {
            Dictionary<int, GameManager> JoinableGames = new Dictionary<int, GameManager>();

            foreach (int gameId in Games.Keys)
            {
                if (Games[gameId].State == GameManager.GameState.WaitingForPlayers)
                {
                    JoinableGames.Add(gameId,Games[gameId]);
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
