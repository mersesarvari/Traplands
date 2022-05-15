using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public List<Player> Users { get; set; }
        public List<Message> Messages { get; set; }

        public string Map;

        public Lobby(string ownerid)
        {
            LobbyId = ownerid;
            Users = new List<Player>();
            Messages = new List<Message>();

        }
        //Constructor for the JSON Serialization
        [JsonConstructor]
        public Lobby(string LobbyId, List<Player> Users, List<Message> Messages, string map)
        {
            this.LobbyId = LobbyId;
            this.Users = Users;
            this.Messages = Messages;
            this.Map = map;
        }
    
        public static void Create(string userid)
        {
            var alreadyexists = Server.lobbies.Where(x => x.LobbyId == userid.ToString()).FirstOrDefault();
            if (alreadyexists == null)
            {
                Server.lobbies.Add(new Lobby(userid));
                Console.WriteLine($"[LOBBY CREATED] : {userid}");
            }

        }
        public static void Join(string userid, string lobbyid)
        {
            Lobby currentlobby = Server.lobbies.Where(x => x.LobbyId.ToString() == lobbyid).FirstOrDefault();
            if (currentlobby != null)
            {

                bool alreadyadded = currentlobby.Users.Where(y => y.Id == userid).FirstOrDefault() != null;
                if (alreadyadded == false && currentlobby != null)
                {
                    currentlobby.Users.Add(Server.FindUserById(userid));
                    foreach (var item in currentlobby.Users)
                    {
                        Console.WriteLine($"[User in the current lobby]: {item.Username}");
                    }
                    Console.WriteLine($"[INFO]: {Server.FindUserById(userid).Username} Joined a lobby");
                    //SENDING RESPONSE TP ALL PLAYER IN THE LOBBY
                    foreach (var item in currentlobby.Users)
                    {
                        var currentclient = Server.FindClient(item.Id);
                        if (currentclient != null)
                        {
                            Server.SendResponse(2, currentclient, JsonConvert.SerializeObject(currentlobby));
                        }
                        else
                        {
                            throw new Exception("curentclient was null");
                        }  
                    }
                }
                else
                {
                    Console.WriteLine("[Error]: User cannot join this lobby!");
                }
            }
        }
        public static void Leave(Player user, string lobbyid)
        {
            //Ez most minden lobbyból törli az illetőt ami nem jó
            var currentlobby = Server.lobbies.Where(x => x.LobbyId.ToString() == lobbyid).FirstOrDefault();
            if (currentlobby != null)
            {
                if (currentlobby.Users.Contains(user))
                {
                    currentlobby.Users.Remove(user);
                }
                else
                {
                    throw new Exception("You cant delete user from this lobby because that user is not part of that lobby");
                }
            }
            else
            {
                throw new Exception("You cant delete user from this lobby because this lobby doesnt exists");
            }
        }        
        public void SetMap(string map)
        {
            Map = map;
        }
        public static void AddRealUsers(Lobby lobby)
        {
            var old = Server.lobbies.FirstOrDefault(x => x.LobbyId == lobby.LobbyId);
            for (int i = 0; i < lobby.Users.Count; i++)
            {
                lobby.Users[i] = old.Users[i];
            }
        }
    }
}
