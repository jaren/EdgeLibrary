using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using EdgeDemo.CheckersGame;

namespace CheckersService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICheckersService
    {
        [OperationContract]
        void SendMoveData(Move move);

        [OperationContract]
        Move RecieveMoveData();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        //[DataMember]
    }
}
