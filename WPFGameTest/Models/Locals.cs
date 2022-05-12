using Game.Logic;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.View;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Game.Models
{
    public class Locals
    {
        IMessenger messenger;
        public Lobby lobby;
        public Client client;
        public User user;
        public bool Connected=false;
        
        INavigationService lobbyService;
        INavigationService gameService;
        INavigationService multimenuService;
        INavigationService menuService;
        IMultiplayer multiplayer;
        

        public List<string>Maps=new List<string>();
        public void RegisterEvents()
        {
            client.connectedEvent += UserConnected;
            client.userDisconnectedEvent += UserDisconnected;
            client.userJoinedLobbyEvent += Client_userJoinedLobbyEvent;
            client.updateUserData += UpdateUser;
            client.gameStartedEvent += GameStarted;
        }

        public Locals(INavigationService lobbyService, INavigationService gameService, INavigationService multimenuService, INavigationService menuService)
        {
            this.lobbyService = lobbyService;
            this.gameService = gameService;
            this.multimenuService = multimenuService;
            this.menuService = menuService;


            Maps.Add("MAP1");
            Maps.Add("MAP2");
            Maps.Add("MAP3");
            client = new Client();
            lobby = new Lobby();
            user = new User();
        }
        
        public void GameStarted()
        {
            ;
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();            
            MultiLogic.locals.lobby = JsonConvert.DeserializeObject<Lobby>(msg);
            ;
            bool firstcall = true;
            Application.Current.Dispatcher.Invoke((Action)delegate {
                MainWindow.game = new Multiplayer();
                (MainWindow.game as Multiplayer).LoadLevel("Level 1");
                //Ez a sor ami nem engedi elindulni a programot
                //(MainWindow.game as Multiplayer).LoadPlayers(MultiLogic.locals.lobby.Users);

            });
            /*
            MainWindow.game = new Multiplayer();
            (MainWindow.game as Multiplayer).LoadLevel("Level 1");
            (MainWindow.game as Multiplayer).LoadPlayers(MultiLogic.locals.lobby.Users);  
            */
            //LevelManager.LoadLevels();
            gameService.Navigate();

        }

        public void UpdateUser()
        {
            //This method is handling the JoinResponse from the server
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();
            /*
            var L = JsonConvert.DeserializeObject<User>(msg);
            (MainWindow.game as Multiplayer).UpdatePlayer(L);
            */

            //Trace.WriteLine($"Lobby was set in multilogic");
        }

        public void Client_userJoinedLobbyEvent()
        {
            //This method is handling the JoinResponse from the server
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();            
            var L = JsonConvert.DeserializeObject<Lobby>(msg);
            MultiLogic.locals.lobby= L;            
            Trace.WriteLine($"Lobby was set in multilogic");
            //MessageBox.Show("User Joined The Lobby");
            lobbyService.Navigate();

        }
        private void UserConnected()
        {
            MultiLogic.locals.user.Username = MultiLogic.locals.client.packetReader.ReadMessage();
            MultiLogic.locals.user.Id = MultiLogic.locals.client.packetReader.ReadMessage();
            Trace.WriteLine($"[Connected] :{this.user.Id}");
            this.Connected = true;
        }
        private void UserDisconnected()
        {
            var uid = MultiLogic.locals.client.packetReader.ReadMessage();
            Trace.WriteLine($"[Disconnected]");
            menuService.Navigate();
        }
        
    }
}
