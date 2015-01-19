using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckersService
{
    [ServiceContract]
    public interface ICheckersService
    {
        [OperationContract]
        bool AddMove(SimpleMove move, int gameId);

        [OperationContract]
        SimpleMove GetLatestMoveFrom(bool player1, int gameId);

        [OperationContract]
        int CreateGame(string hostTeamName);

        [OperationContract]
        void JoinGame(int gameId, string otherTeamName);

        [OperationContract]
        void Disconnect(int gameId, bool isHost);

        [OperationContract]
        void EndGame(int gameId);

        [OperationContract]
        List<GameManager> GetAllGames();

        [OperationContract]
        Dictionary<int, GameManager> GetSpecificGames(bool waitingForPlayers = false, bool inProgress = false, bool ended = false, bool hostDisconnect = false, bool playerDisconnect = false);
    }
}
