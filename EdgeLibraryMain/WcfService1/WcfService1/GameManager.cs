using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Drawing;

namespace CheckersService
{
    [DataContract]
    public class GameManager
    {
        public enum GameState
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
        public GameState State { get; set; }

        public GameManager(string hostTeamName)
        {
            HostTeamName = hostTeamName;
            OtherTeamName = "Waiting For Players";
            GameStartTime = DateTime.UtcNow;
            State = GameState.WaitingForPlayers;
        }
    }
}