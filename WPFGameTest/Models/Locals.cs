using Game.Logic;
using Game.MVVM.View;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Models
{
    public class Locals
    {
        IMessenger messenger;
        public Lobby lobby;
        public Client client;
        public User user;
        public bool Connected=false;

        public List<string>Maps=new List<string>();
        public void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            client.userDisconnectedEvent += UserDisconnected;
            client.userJoinedLobbyEvent += Client_userJoinedLobbyEvent;
            client.updateUserData += UpdateUser;
            client.gameStartedEvent += GameStarted;
        }

        public Locals(IMessenger messenger)
        {
            Maps.Add("MAP1");
            Maps.Add("MAP2");
            Maps.Add("MAP3");
            client = new Client();
            lobby = new Lobby();
            user = new User();
            this.messenger = messenger;
        }
        
        public void GameStarted()
        {
            var msg = this.client.PacketReader.ReadMessage();
            
            MultiLogic.locals.lobby = JsonConvert.DeserializeObject<Lobby>(msg);
            MainWindow.game = new Multiplayer();
            (MainWindow.game as Multiplayer).LoadLevel("Level 1");
            (MainWindow.game as Multiplayer).LoadPlayers(MultiLogic.locals.lobby.Users);
            MessageBox.Show("GageStartedEventStarted");
            //Have to ADD NAVIGATION

        }

        public void UpdateUser()
        {
            //This method is handling the JoinResponse from the server
            var msg = this.client.PacketReader.ReadMessage();
            var L = JsonConvert.DeserializeObject<User>(msg);

            (MainWindow.game as Multiplayer).UpdatePlayer(L);

            //Trace.WriteLine($"Lobby was set in multilogic");
        }

        public void Client_userJoinedLobbyEvent()
        {
            ;
            //This method is handling the JoinResponse from the server
            var msg = this.client.PacketReader.ReadMessage();            
            var L = JsonConvert.DeserializeObject<Lobby>(msg);
            MultiLogic.locals.lobby= L;
            Trace.WriteLine($"Lobby was set in multilogic");
            MessageBox.Show("Lobby refreshed");

        }
        //Server-Client Methods
        #region Server-Client methods
        private void UserConnected()
        {

            this.user.Username = client.PacketReader.ReadMessage();
            this.user.Id = client.PacketReader.ReadMessage();
            Trace.WriteLine($"[Connected] :{this.user.Id}");
            this.Connected = true;
            //messenger.Send("User Connected", "SetUser");
        }


        private void UserDisconnected()
        {
            ;
            var uid = client.PacketReader.ReadMessage();
            Trace.WriteLine($"[Disconnected]");
        }


        #endregion
        
    }
}
