using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Drawing;

namespace CheckersService
{
    [DataContract]
    public class GameState
    {
        public enum State
        {
            WaitingForPlayers,
            InProgress,
            Ended,
            HostDisconnected,
            PlayerDisconnected
        }

        [DataMember]
        public string HostTeamName { get; set; }
        [DataMember]
        public string OtherTeamName { get; set; }
        [DataMember]
        public DateTime GameStartTime { get; set; }
        [DataMember]
        public State GameInfo { get; set; }
        [DataMember]
        public List<SimpleMove> MoveList { get; set; }

        public GameState(string hostTeamName)
        {
            HostTeamName = hostTeamName;
            OtherTeamName = "Waiting For Players";
            GameStartTime = DateTime.UtcNow;
            GameInfo = State.WaitingForPlayers;
            MoveList = new List<SimpleMove>();
        }
    }
}