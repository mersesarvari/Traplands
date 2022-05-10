using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Game.Logic;
using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace Game.MVVM.ViewModel
{
    public class MultiplayerGameMenuViewModel:ViewModelBase
    {
        public Locals locals;
        public MultiLogic logic;
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public ICommand JoinLobbyCommand { get; set;}
        public ICommand CreateLobbyCommand { get; set; }
        public ICommand SetUsernameCommand { get; set; }

        public ICommand ConnectServerCommand { get; set; }

        private string lobbyCode;

        public string LobbyCode
        {
            get { return lobbyCode; }
            set { lobbyCode = value; }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }


        public MultiplayerGameMenuViewModel(INavigationService mainMenuNavigationService, INavigationService lobbyNavigationService, INavigationService multiGameNavigationService, Locals locals, MultiLogic logic)
        {
            this.locals = locals;
            Username = "PLAYER";
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
            NavigateLobbyCommand = new NavigateCommand(lobbyNavigationService);
            NavigateMultiGameCommand = new NavigateCommand(multiGameNavigationService);
            ConnectServerCommand = new RelayCommand(
                () => logic.Connect(locals, Username),
                () => !locals.Connected
                ) ;
            JoinLobbyCommand = new RelayCommand(
                () => logic.JoinLobby(lobbyNavigationService, locals,Username, LobbyCode)
                ); ;
            CreateLobbyCommand = new RelayCommand(
                () => logic.CreateLobby(lobbyNavigationService,locals, Username, 0)
                );

            //NavigateLobbyCommand = new NavigateToLobbyCommand(lobbyNavigationService, Locals.lobby);
        }
        public void UserJoinedLobbyResponse()
        {
            MessageBox.Show("USER JOINED LOBBY RESPONSE ARRIVED");
            //This method is handling the JoinResponse from the server
            var msg = locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];


                if (status != "ERROR" && status != "Success")
                {
                    locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }
    }
}
