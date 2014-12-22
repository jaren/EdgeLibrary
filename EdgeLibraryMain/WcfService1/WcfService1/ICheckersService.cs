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
        void AddMove(SimpleMove move);

        [OperationContract]
        SimpleMove GetLatestMoveFrom(bool topTeam);

        [OperationContract]
        int CreateGame(string hostTeamName);

        [OperationContract]
        void JoinGame(int gameId, string otherTeamName);

        [OperationContract]
        void Disconnect(int gameId, bool isHost);

        [OperationContract]
        void EndGame(int gameId);
    }
}
