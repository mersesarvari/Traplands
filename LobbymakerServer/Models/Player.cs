using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbymakerServer
{
    public class Player
    {
        public string Id { get; set; }
        public string  Username { get; set; }

        public Player(_Client client)
        {
            Id = client.UID.ToString();
            Username = client.Username;
        }
    }
}
