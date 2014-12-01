using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CheckersService
{
    [DataContract]
    public class SimpleMove
    {
        [DataMember]
        Dictionary<int, KeyValuePair<int, int>> SquarePath { get; set; }
        [DataMember]
        Dictionary<int, KeyValuePair<int, int>> JumpedSquares { get; set; }
        [DataMember]
        KeyValuePair<int, int> StartSquare { get; set; }
        [DataMember]
        string ID { get; set; }

        public SimpleMove(Dictionary<int, KeyValuePair<int, int>> squarePath, Dictionary<int, KeyValuePair<int, int>> jumpedSquares, KeyValuePair<int, int> startSquare, string id)
        {
            SquarePath = squarePath;
            JumpedSquares = jumpedSquares;
            StartSquare = startSquare;
            ID = id;
        }
    }
}