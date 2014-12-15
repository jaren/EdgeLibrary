using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckersService
{
    [ServiceBehavior(InstanceContextMode= InstanceContextMode.Single)]
    public class CheckersService : ICheckersService
    {
        public List<SimpleMove> Moves = new List<SimpleMove>();

        public void AddMove(SimpleMove move)
        {
            Moves.Add(move);
        }

        public SimpleMove GetLatestMoveFrom(bool topTeam)
        {
            if (Moves.Count > 0 && Moves[Moves.Count - 1].TopTeam == topTeam)
            {
                return Moves[Moves.Count - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
