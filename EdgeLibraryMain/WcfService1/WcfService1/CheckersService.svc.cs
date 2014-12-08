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
        public List<SimpleMove> Moves = new List<SimpleMove>();

        public void AddMove(Dictionary<int,KeyValuePair<int,int>> movePath, Dictionary<int,KeyValuePair<int,int>> jumpedSquares, KeyValuePair<int,int> startSquare, string id, bool topTeam)
        {
            Moves.Add(new SimpleMove(movePath,jumpedSquares,startSquare,id,topTeam));
        }

        public SimpleMove GetLatestMoveFrom(bool topTeam)
        {
            if (Moves[Moves.Count - 1].TopTeam = topTeam)
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
