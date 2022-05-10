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
        public Lobby lobby;
        public Client client;
        public User user;
        public bool Connected=false;



        public Locals()
        {
            client = new Client();
            lobby = new Lobby();
            user = new User();
        }
        public void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            client.userJoinedLobbyEvent += Client_userJoinedLobbyEvent;
        }

        public void Client_userJoinedLobbyEvent()
        {
            //This method is handling the JoinResponse from the server
            var msg = this.client.PacketReader.ReadMessage();
            MessageBox.Show(msg);
            
        }
        //Server-Client Methods
        #region Server-Client methods
        //Nem hívódik meg valamiért
        private void UserConnected()
        {
            this.user.Username = client.PacketReader.ReadMessage();
            this.user.Id = client.PacketReader.ReadMessage();
            var u = this.user;
            MessageBox.Show($"Connection was succesfull: "+this.user.Id);
            this.Connected = true;
            ;
        }
        private void UserDisconnected()
        {
            var uid = client.PacketReader.ReadMessage();
            MessageBox.Show("User disconnected");
        }


        #endregion
        
    }
}
