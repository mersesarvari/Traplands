using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Game.Logic;
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

        public static void Join(Locals locals, string username, string lobbycode)
        {
            try
            {
                if (locals.client.Connected())
                {
                    locals.client.SendCommandToServer("JOINLOBBY", locals.user.Id, lobbycode);
                }
                else
                {
                    MessageBox.Show("You are not connected");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Create(Locals locals, string username, int currenttick)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("CREATELOBBY", locals.user.Id, locals.user.Id);
                //var checker = MultiLogic.locals;
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }
        public static void Start(Lobby lobby, string username)
        {
            foreach (var user in lobby.Users)
            {
                user.RenderData = new RenderData();
            }
            if (MultiLogic.locals.client.Connected())
            {
                MultiLogic.locals.client.SendCommandToServer("STARTGAME", MultiLogic.locals.lobby.LobbyId, JsonConvert.SerializeObject(lobby));
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }
        public static void Leave(Lobby lobby, string userId)
        {
            MultiLogic.locals.client.SendCommandToServer("LEAVEGAME", userId, JsonConvert.SerializeObject(lobby));
        }
        public void SendMessage(Message message)
        {
            if (MultiLogic.locals.client.Connected())
            {
                MultiLogic.locals.client.SendCommandToServer("SENDMESSAGE", MultiLogic.locals.lobby.LobbyId, JsonConvert.SerializeObject(message));
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }

        }
    }
}
