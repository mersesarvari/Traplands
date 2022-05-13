using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Message
    {
        public string ownerid { get; set; }
        public string message { get; set; }

        public Message(string ownerid, string message)
        {
            this.ownerid = ownerid;
            this.message = message;
        }
        //Executor is the lobby here
        public static void SendMessageToLobby(string executor, string command)
        {
            var currentlobby = Server.lobbies.Where(x => x.LobbyId == executor).FirstOrDefault();
            Message message = JsonConvert.DeserializeObject<Message>(command);
            currentlobby.Messages.Add(message);
            foreach (var item in currentlobby.Users)
            {
                var currentclient = Server.FindClient(item.Id);
                if (currentclient != null)
                {
                    Server.SendResponse(7, currentclient, JsonConvert.SerializeObject(message));
                }
                else
                {
                    throw new Exception("curentclient was null");
                }
            }

        }
    }
}
