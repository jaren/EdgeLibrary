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
        short[] movePiece(short pieceID, short destX, short destY);
    }
}
