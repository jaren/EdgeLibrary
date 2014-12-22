using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WcfService1
{
    public class Game
    {
        [DataMember]
        public string HostTeamName { get; set; }

    }
}