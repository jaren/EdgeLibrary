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
        SimpleMove GetLatestMoveFrom(bool topTeam, int gameId);

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
        Dictionary<int, GameManager> GetJoinableGames();
    }
}
