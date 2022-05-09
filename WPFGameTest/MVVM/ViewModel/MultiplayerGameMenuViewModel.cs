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

        private bool connected;

        public bool Connected
        {
            get { return Locals.Connected; }
            set { Locals.Connected = value; }
        }

        public bool ConnectionCheck()
        {
            if (Locals.Connected)
            {
                return true;
            }
            else return false;
            ;
        }


        public MultiplayerGameMenuViewModel(INavigationService mainMenuNavigationService, INavigationService lobbyNavigationService, INavigationService multiGameNavigationService)
        {
            Locals.client.userJoinedLobbyEvent += UserJoinedLobbyResponse;
            Username = "PLAYER";
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
            NavigateLobbyCommand = new NavigateCommand(lobbyNavigationService);
            NavigateMultiGameCommand = new NavigateCommand(multiGameNavigationService);

            ConnectServerCommand = new RelayCommand(
                () => MultiLogic.ConnectToServer(Username)
                );

            JoinLobbyCommand = new RelayCommand(
                () => MultiLogic.JoinLobby(lobbyNavigationService, Username, LobbyCode)
                ); ;
            CreateLobbyCommand = new RelayCommand(
                () => MultiLogic.CreateLobby(lobbyNavigationService, Username, 0)
                );

            //NavigateLobbyCommand = new NavigateToLobbyCommand(lobbyNavigationService, Locals.lobby);
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
    }
}
