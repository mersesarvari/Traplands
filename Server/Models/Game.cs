using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Game
    {
        //public List<Player> Player { get; set; }

        internal static void Move(string executor, string command)
        {
            //Console.WriteLine("MOVE CALLED");
            Player data = JsonConvert.DeserializeObject<Player>(command);
            foreach (var item in Server.players)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(4, currentclient, JsonConvert.SerializeObject(data));
                    //Console.WriteLine($"[({2})Response to: {item.Id}]");
                }
                else
                {
                    throw new Exception("curentclient was null");
                }

            }
        }
        //executor is clientID and command is LobbyID
        public static void LeaveGame(string executor, string command)
        {
            var currentclient = Server.FindClient(executor);
            Lobby lobby = JsonConvert.DeserializeObject<Lobby>(command);
            if (lobby != null)
            {
                lobby.Users.Remove(Server.FindUserById(executor));
                Console.WriteLine($"[LeftGame]: {executor}");
                Server.SendResponse(6, currentclient, executor);
            }
        }
    }
}
