using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Models
{
    public static class Locals
    {        
        public static Lobby lobby = new();
        public static Client client= new();
        public static User user=new();
        public static bool Connected=false;

        public static void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            //client.userCreatedLobbyEvent += Client_userCreatedLobbyEvent;
            //client.userJoinedLobbyEvent += Client_userJoinedLobbyEvent;
        }

        private static void Client_userJoinedLobbyEvent()
        {
            //MessageBox.Show("USER JOINED LOBBY RESPONSE ARRIVED");
            /*
            //This method is handling the JoinResponse from the server
            var msgname = Locals.client.PacketReader.ReadMessage();
            var msg = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show(msg, msgname);
            */
        }

        private static void Client_userCreatedLobbyEvent()
        {
            
            //MessageBox.Show("USER CREATED LOBBY RESPONSE ARRIVED");
            //This method is handling the JoinResponse from the server
            var msgname = Locals.client.PacketReader.ReadMessage();
            var msg = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show(msg, msgname);
            
        }


        //Server-Client Methods
        #region Server-Client methods
        //Nem hívódik meg valamiért
        private static void UserConnected()
        {
            Locals.user.Username = Locals.client.PacketReader.ReadMessage();
            Locals.user.Id = Locals.client.PacketReader.ReadMessage();
            //Setting up the timer <==> Sync with the server
            //GameTimer.Tick = int.Parse(Locals.client.PacketReader.ReadMessage());
            var u = Locals.user;
            MessageBox.Show($"Connection was succesfull: "+Locals.user.Id);
        }
        private static void UserDisconnected()
        {
            var uid = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show("User disconnected");
        }


        #endregion
        
    }
}
