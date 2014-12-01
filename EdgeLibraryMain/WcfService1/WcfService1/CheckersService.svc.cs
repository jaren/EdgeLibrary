using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckersService
{
    public class CheckersService : ICheckersService
    {
        public short[] movePiece(short pieceId, short destX, short destY)
        {
            return new short[] { pieceId, destX, destY };
        }
    }
}
