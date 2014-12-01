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
        public Dictionary<DateTime, object[]> Moves = new Dictionary<DateTime, object[]>();

        public void addMove(object[] moveInfo)
        {
            Moves.Add(DateTime.Now, moveInfo);
        }

        public List<object[]> GetMovesAfter(DateTime time)
        {
            List<object[]> moves = new List<object[]>();
            foreach (DateTime dt in Moves.Keys)
            {
                if (dt > time)
                {
                    moves.Add(Moves[dt]);
                }
            }

            return moves;
        }
    }
}
