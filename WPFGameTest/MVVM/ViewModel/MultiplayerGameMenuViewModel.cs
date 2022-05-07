using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Logic;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Microsoft.Toolkit.Mvvm.Input;

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



        public MultiplayerGameMenuViewModel(INavigationService mainMenuNavigationService, INavigationService lobbyNavigationService, INavigationService multiGameNavigationService)
        {
            Username = "PLAYER";
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
            NavigateLobbyCommand = new NavigateCommand(lobbyNavigationService);
            NavigateMultiGameCommand = new NavigateCommand(multiGameNavigationService);

            JoinLobbyCommand = new RelayCommand(
                () => MultiLogic.JoinLobby(Username,LobbyCode, 0));
            CreateLobbyCommand = new RelayCommand(
                () => MultiLogic.CreateLobby(Username, 0));

        }

        

    }
}
