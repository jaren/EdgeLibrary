using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckersService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CheckersService : ICheckersService
    {
        Dictionary<DateTime, object[]> moves = new Dictionary<DateTime, object[]>();

        public void SendMove (object[] move)
        {
            moves.Add(DateTime.Now, move);
        }

        public List<object[]> RecieveMove(DateTime checkAfter)
        {
            List<object[]> movesList = null;

            foreach(DateTime d in moves.Keys)
            {
                if(d > checkAfter)
                {
                    if(movesList == null)
                    {
                        movesList = new List<object[]>();
                    }

                    movesList.Add(moves[d]);
                }
            }

            return movesList;
        }
    }
}
