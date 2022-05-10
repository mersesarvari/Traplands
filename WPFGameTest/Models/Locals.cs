using Game.Logic;
using Game.MVVM.View;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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



        public Locals(IMessenger messenger)
        {

            client = new Client();
            lobby = new Lobby();
            user = new User();
            this.messenger = messenger;
        }
        public void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            client.userDisconnectedEvent += UserDisconnected;
            client.userJoinedLobbyEvent += Client_userJoinedLobbyEvent;
        }

        public void Client_userJoinedLobbyEvent()
        {
            ;
            //This method is handling the JoinResponse from the server
            var msg = this.client.PacketReader.ReadMessage();            
            var L = JsonConvert.DeserializeObject<Lobby>(msg);
            MultiLogic.locals.lobby= L;
            ;
            MessageBox.Show(msg);
            
        }
        //Server-Client Methods
        #region Server-Client methods
        private void UserConnected()
        {

            this.user.Username = client.PacketReader.ReadMessage();
            this.user.Id = client.PacketReader.ReadMessage();
            MessageBox.Show($"Connection was succesfull: "+this.user.Id);
            this.Connected = true;
            //messenger.Send("User Connected", "SetUser");
        }


        private void UserDisconnected()
        {
            ;
            var uid = client.PacketReader.ReadMessage();
            MessageBox.Show("User disconnected");
        }


        #endregion
        
    }
}
