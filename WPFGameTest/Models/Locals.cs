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
        IMessenger lobbyViewMessenger;
        IMessenger multiViewMessenger;


        public Lobby lobby;
        public List<Lobby> Lobbies;
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
            client.gameLeftEvent += GameLeft;
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
            Lobbies = new List<Lobby>();
    }

        public void RegisterMessenger(IMessenger messenger)
        {
            multiViewMessenger = messenger;
        }
        public void GameStarted()
        {
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();
            MultiLogic.locals.lobby = JsonConvert.DeserializeObject<Lobby>(msg);
            lobbyViewMessenger.Send("Game started", "GameStarted");
        }

        public void UpdateUser()
        {
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();            
            var L = JsonConvert.DeserializeObject<User>(msg);
            (MainWindow.game as Multiplayer).UpdatePlayer(L);
        }

        public void Client_userJoinedLobbyEvent()
        {
            //This method is handling the JoinResponse from the server
            var msg = MultiLogic.locals.client.packetReader.ReadMessage();            
            var L = JsonConvert.DeserializeObject<Lobby>(msg);
            MultiLogic.locals.lobby= L;
            MultiLogic.locals.Lobbies.Add(L);
            Trace.WriteLine($"Lobby was set in multilogic");
            //MessageBox.Show("User Joined The Lobby");
            lobbyService.Navigate();

        }
        private void UserConnected()
        {
            MultiLogic.locals.user.Username = MultiLogic.locals.client.packetReader.ReadMessage();
            MultiLogic.locals.user.Id = MultiLogic.locals.client.packetReader.ReadMessage();
            var lobbiesstring = MultiLogic.locals.client.packetReader.ReadMessage();
            MultiLogic.locals.Lobbies = JsonConvert.DeserializeObject<List<Lobby>>(lobbiesstring);

            Trace.WriteLine($"[Connected] :{this.user.Id}");
            MultiLogic.locals.Connected = true;

            multiViewMessenger.Send("User connected", "UserConnected");
        }
        private void UserDisconnected()
        {

            var userid = MultiLogic.locals.client.packetReader.ReadMessage();
            Trace.WriteLine($"[Disconnected]");
            Application.Current.Dispatcher.Invoke((Action)delegate {
                menuService.Navigate();

            });

        }        
        private void GameLeft()
        {
            var uid = MultiLogic.locals.client.packetReader.ReadMessage();
            Trace.WriteLine($"[Left game]");
            menuService.Navigate();
        }

        public void MessageRecieved()
        {
            var UserName = MultiLogic.locals.client.packetReader.ReadMessage();
            var Message = MultiLogic.locals.client.packetReader.ReadMessage();
            lobby.Messages.Add()
            Trace.WriteLine($"[Connected] :{this.user.Id}");
            this.Connected = true;
        }
    }
}
