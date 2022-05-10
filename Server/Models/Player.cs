using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public RenderData RenderData { get; set; }

        public Player(ServerClient client)
        {
            Id = client.UID.ToString();
            Username = client.Username;
        }

        [JsonConstructor]
        public Player(string id, string username, RenderData renderData)
        {
            Id = id;
            Username = username;
            RenderData = renderData;
        }
    }

    public class RenderData
    {
        string Fill { get; set; }
        Transform Transform { get; set; }
    }
}
