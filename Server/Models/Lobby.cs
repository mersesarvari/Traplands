﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public string OwnerId { get; set; }
        public List<Player> Users { get; set; }
        public List<string> Messages { get; set; }
        public Map Map { get; set; }

        public Lobby(string ownerid)
        {
            LobbyId = ownerid;
            Users = new List<Player>();
            Messages = new List<string>();
            OwnerId = ownerid;

        }

        public static void Create(string userid)
        {
            var alreadyexists = Server.lobbies.Where(x => x.LobbyId == userid.ToString()).FirstOrDefault();
            if (alreadyexists == null)
            {
                Server.lobbies.Add(new Lobby(userid));
                Console.WriteLine("[INFO] Lobby Created succesfully");
                Server.SendResponse(userid.ToString(), "CREATELOBBY","Success");
            }

        }
        public static void Join(string userid, string lobbyid)
        {
            try
            {
                //Lobby exists and Found.
                Lobby currentlobby = Server.lobbies.Where(x => x.LobbyId.ToString() == lobbyid).FirstOrDefault();
                //Player is already added to that lobby
                if (currentlobby != null)
                {
                    Player alreadyadded = currentlobby.Users.Where(y => y.Id == userid).FirstOrDefault();
                    if (alreadyadded == null && currentlobby != null)
                    {
                        //Adding User to a pecific lobby
                        currentlobby.Users.Add(Server.FindUserById(userid));
                        var l = Server.lobbies;
                        //Sneding CODE and LOBBY INFO Back to the Client

                        Console.WriteLine($"[INFO]: {Server.FindUserById(userid).Username} Joined a lobby");
                        //Sending lobby information to all connected player
                        foreach (var item in currentlobby.Users)
                        {
                            try
                            {
                                //Server.SendMessage(3, item.Id, "JOINLOBBY/" + JsonConvert.SerializeObject(currentlobby));
                                Server.BroadcastResponse("JOINLOBBY",JsonConvert.SerializeObject(currentlobby));
                                ;
                            }
                            catch (Exception ex)
                            {

                                throw new Exception(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        //Print information
                        Console.WriteLine("[Error]: User cannot join this lobby!");
                        //Server.SendResponse(userid, "JOINLOBBY","ERROR");
                    }
                }
                
            }
            catch (Exception e)
            {

                throw new Exception("[ERROR] at Lobby.Join: "+e.Message);
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
        public void SetMap(Map map)
        {
            Map = map;
        }        
        public Game StartGame(List<Player> players, Map _map)
        {            
            return new Game(LobbyId,_map, players);
        }
    }
}
