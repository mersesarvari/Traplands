using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;
using Newtonsoft.Json;

namespace Server
{
    public class Game
    {
        public string Id;
        public Map Map { get; set; }
        public List<Player> Players { get; set; }
        public Player LobbyHost { get; set; }

        public Game(string ownerid, Map map, List<Player> players)
        {
            Id = ownerid;
            Players = players;
            Map = map;
        }
        public string getGameId()
        {
            return Id.ToString();
        }

        static int gametesterX = 200;
        static int gametesterY = 200;
        public static void Move(int tick , string ownerid, char key)
        {
            if (key=='W')
            {
                gametesterY-=4;
            }            
            if (key == 'A')
            {
                gametesterX-=4;
            }
            if (key == 'S')
            {
                gametesterY+=4;
            }
            if (key == 'D')
            {
                gametesterX+=4;
            }
            int[] coords = new int[2];
            Random r = new Random();
            coords[0] = gametesterX;
            coords[1] = gametesterY;
            //Console.WriteLine($"[GAME CLASS] : Sending back to client: {JsonConvert.SerializeObject(new MovementPackage(tick,coords[0], coords[1]))}");
            Server.SendMessage(17, ownerid, "MOVE/" + JsonConvert.SerializeObject(new MovementPackage(tick, coords[0], coords[1])));

        }

    }
}
