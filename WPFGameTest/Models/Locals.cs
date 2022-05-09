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
        public static Lobby lobby;
        public static Client client=new Client();
        public static User user=new User();
        public static bool Connected=false;

        public static void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            client.userCreatedLobbyEvent += Client_userCreatedLobbyEvent;
            client.userCreatedLobbyEvent += Client_userCreatedLobbyEvent1;
            client.zeroopcodeEvent += Client_zeroopcodeEvent;
        }

        private static void Client_zeroopcodeEvent()
        {
            try
            {
                
                //This method is handling the JoinResponse from the server
                var msg = Locals.client.PacketReader.ReadMessage();
                ;
                if (msg.Contains('/') && msg.Split('/')[0] == "CREATELOBBY")
                {
                    var status = msg.Split('/')[1];
                    Locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                    MessageBox.Show(msg);
                    MessageBox.Show("ZERO CODE EVENT OCCURED");
                    //this is doing the navigation

                }
                else
                {
                    MessageBox.Show("Response Message format is bad:" + msg);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private static void Client_userCreatedLobbyEvent1()
        {
            MessageBox.Show("Lobby Created Succesfully");
        }

        private static void Client_userCreatedLobbyEvent()
        {
            MessageBox.Show("Lobby was created succesfully");
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

        public static void UserJoinedLobbyResponse()
        {
            MessageBox.Show("USER JOINED LOBBY RESPONSE ARRIVED");
            //This method is handling the JoinResponse from the server
            var msg = Locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];


                if (status != "ERROR" && status != "Success")
                {
                    Locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                    ;
                }
                //this is doing the navigation
                
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }

        #endregion
        
    }
}
