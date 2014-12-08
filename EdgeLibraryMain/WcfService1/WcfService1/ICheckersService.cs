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
        void AddMove(Dictionary<int, KeyValuePair<int, int>> movePath, Dictionary<int, KeyValuePair<int, int>> jumpedSquares, KeyValuePair<int, int> startSquare, string id, bool topTeam);

        [OperationContract]
        SimpleMove GetLatestMoveFrom(bool topTeam);
    }
}
