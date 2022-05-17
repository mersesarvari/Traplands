using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Game
    {
        public string Id;
        public Map Map { get; set; }
        public List<User> Players { get; set; }
        public User LobbyHost { get; set; }

        public Game(string ownerid, Map map , List<User> players)
        {
            Id = ownerid;
            Players = players;
            Map = map;
        }
        public string getGameId()
        {
            return Id.ToString();
        }

    }
}
