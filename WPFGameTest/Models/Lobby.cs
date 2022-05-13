using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Game.Models
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
        public string Map { get; set; }
        public Lobby()
        {

        }        
        public Lobby(string ownerid)
        {
            LobbyId = ownerid;
            Users = new List<User>();
            Messages = new List<Message>();

        }
        [JsonConstructor]
        public Lobby(string LobbyId, List<User> Users, List<Message> Messages, string map)
        {
            this.LobbyId = LobbyId;
            this.Users = Users;
            this.Messages = Messages;
            this.Map = map;
        }
    }
}
