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
        void addMove(object[] moveInfo);

        [OperationContract]
        List<object[]> GetMovesAfter(DateTime time);
    }
}
