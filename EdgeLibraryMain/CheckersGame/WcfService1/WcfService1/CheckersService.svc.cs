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
        public Dictionary<int, GameState> Games = new Dictionary<int, GameState>();

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

        public SimpleMove GetLatestMoveFrom(int gameId)
        {
            if (Games.Count >= gameId + 1 && Games[gameId].MoveList.Count > 0)
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
            GameState Game = new GameState(hostTeamName);
            Games.Add(Games.Count,Game);
            return Games.Count - 1;
        }

        public void JoinGame(int gameId, string otherTeamName)
        {
            Games[gameId].OtherTeamName = otherTeamName;
            Games[gameId].GameInfo = GameState.State.InProgress;
        }

        public void Disconnect(int gameId, bool host)
        {
            if (host)
            {
                Games[gameId].GameInfo = GameState.State.HostDisconnected;
            }
            else
            {
                Games[gameId].GameInfo = GameState.State.PlayerDisconnected;
            }
        }

        public void EndGame(int gameId)
        {
            Games[gameId].GameInfo = GameState.State.Ended;
        }

        public Dictionary<int, GameState> GetSpecificGames(GameState.State state)
        {
            Dictionary<int, GameState> SpecificGames = new Dictionary<int, GameState>();

            foreach (int gameId in Games.Keys)
            {
                GameState game = Games[gameId];
                if (game.GameInfo == state)
                {
                    SpecificGames.Add(gameId,game);
                }
            }

            return SpecificGames;
        }

        public List<GameState> GetAllGames()
        {
            return Games.Values.ToList();
        }
    }
}
