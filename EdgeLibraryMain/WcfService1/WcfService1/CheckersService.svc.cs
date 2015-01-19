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

        public SimpleMove GetLatestMoveFrom(bool player1, int gameId)
        {
            if (Games.Count >= gameId + 1 && Games[gameId].MoveList.Count > 0 && Games[gameId].MoveList[Games[gameId].MoveList.Count - 1].Player1 == player1)
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

        public Dictionary<int,GameManager> GetSpecificGames(bool waitingForPlayers = false, bool inProgress = false, bool ended = false, bool hostDisconnect = false, bool playerDisconnect = false)
        {
            Dictionary<int, GameManager> SpecificGames = new Dictionary<int, GameManager>();

            foreach (int gameId in Games.Keys)
            {
                GameManager game = Games[gameId];
                if (waitingForPlayers && game.State == GameManager.GameState.WaitingForPlayers)
                {
                    SpecificGames.Add(gameId,game);
                }
                else if (inProgress && game.State == GameManager.GameState.InProgress)
                {
                    SpecificGames.Add(gameId, game);
                }
                else if (ended && game.State == GameManager.GameState.Ended)
                {
                    SpecificGames.Add(gameId, game);
                }
                else if (hostDisconnect && game.State == GameManager.GameState.HostDisconnected)
                {
                    SpecificGames.Add(gameId, game);
                }
                else if (playerDisconnect && game.State == GameManager.GameState.PlayerDisconnected)
                {
                    SpecificGames.Add(gameId, game);
                }
            }

            return SpecificGames;
        }

        public List<GameManager> GetAllGames()
        {
            return Games.Values.ToList();
        }
    }
}
