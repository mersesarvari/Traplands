using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server
{
    public class Player
    {
        public string Id { get; set; }
        public string  Username { get; set; }

        public Player(ServerClient client)
        {
            Id = client.UID.ToString();
            Username = client.Username;
        }
        [JsonConstructor]
        public Player()
        {

        }
    }
}
