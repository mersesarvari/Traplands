using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WPFGameTest.Models
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public Player Owner { get; set; }
        public List<Player> Players { get; set; }
        public List<string> Messages { get; set; }
        //TODO: Map class to declare what a map is
        //public Map Map { get; set; }

        public Lobby(string ownerid)
        {
            LobbyId = ownerid;
            Players = new List<Player>();
            Messages = new List<string>();

        }
        public Lobby()
        {

        }

    }
}
