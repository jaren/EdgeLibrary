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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class CheckersService : ICheckersService
    {
        public List<Move> moveHistory = new List<Move>();

        public void SendMoveData(Move move)
        {
            moveHistory.Add(move);
        }

        public Move RecieveMoveData()
        {
            return moveHistory[moveHistory.Count - 1];
        }
    }
}
