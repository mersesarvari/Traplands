using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public string Color { get; set; }

        public Player(Socket client)
        {
            Id = client.UID.ToString();
            Username = client.Username;
            Color = client.Color;
        }

        [JsonConstructor]
        public Player(string id, string username, RenderData renderData, string Color)
        {
            Id = id;
            Username = username;
            RenderData = renderData;
            this.Color = Color;
        }
    }

    public class RenderData
    {
        public string FileName { get; set; }
        public int ImageIndex { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double ScaleX { get; set; }
    }
}
