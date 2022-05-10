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
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace Game.MVVM.ViewModel
{
    public class MultiplayerGameMenuViewModel:ViewModelBase
    {
        private MultiLogic logic;
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public ICommand JoinLobbyCommand { get; set;}
        public ICommand CreateLobbyCommand { get; set; }
        public ICommand SetUsernameCommand { get; set; }
        public ICommand Disconnect { get; set; }
        public ICommand ConnectServerCommand { get; set; }

        private string lobbyCode;

        public string LobbyCode
        {
            get { return lobbyCode; }
            set { lobbyCode = value; }
        }
        private string username="PLAYER";

        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }

        private string userid;

        public string UserID
        {
            get { return userid; }
            set { SetProperty(ref userid, value); }
        }



        public MultiplayerGameMenuViewModel(INavigationService mainMenuNavigationService, INavigationService lobbyNavigationService, INavigationService multiGameNavigationService)
        {
            Locals locals = new Locals(Messenger);
            MultiLogic logic = new MultiLogic(Messenger);
            MultiLogic.locals = locals;
            locals.RegisterEvents();
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
            NavigateLobbyCommand = new NavigateCommand(lobbyNavigationService);
            NavigateMultiGameCommand = new NavigateCommand(multiGameNavigationService);
            ConnectServerCommand = new RelayCommand(
                () => locals.client.ConnectToServer(Username),
                () => !locals.Connected
                );
            JoinLobbyCommand = new RelayCommand(
                () => logic.JoinLobby(lobbyNavigationService, locals,Username, LobbyCode)
                );
            CreateLobbyCommand = new RelayCommand(
                () => logic.CreateLobby(lobbyNavigationService,locals, Username, 0)
                );
            Disconnect = new RelayCommand(
                () => { logic.Disconnect(MultiLogic.locals.user.Id); }
                );
            Messenger.Register<MultiplayerGameMenuViewModel, string, string>(this, "SetUser", (recepient, msg) =>
            {
                UserID = MultiLogic.locals.user.Id;
                Username = MultiLogic.locals.user.Username;
                (Disconnect as RelayCommand).NotifyCanExecuteChanged();
            });

        }
        public void UserJoinedLobbyResponse()
        {
            MessageBox.Show("USER JOINED LOBBY RESPONSE ARRIVED");
            //This method is handling the JoinResponse from the server
            var msg = MultiLogic.locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];


                if (status != "ERROR" && status != "Success")
                {
                    MultiLogic.locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }
    }
}
