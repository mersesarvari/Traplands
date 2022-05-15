using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        public List<string> Maps;
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public ICommand JoinLobbyCommand { get; set;}
        public ICommand CreateLobbyCommand { get; set; }
        public ICommand ConnectServerCommand { get; set; }
        public ICommand RefreshLobbies { get; set; }

        public bool IsConnected { get { return MultiLogic.locals.Connected; } }
        public bool NotConnected { get { return !MultiLogic.locals.Connected; } }

        public string LobbyCode
        {
            get { return SelectedLobby == null ? "0" : SelectedLobby.LobbyId; }
        }

        private string username = "Name";

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

        private List<Lobby> lobbies;
        public List<Lobby> Lobbies
        {
            get { return lobbies; }
            set 
            {
                SetProperty(ref lobbies, value);
            }
        }

        private Lobby selectedLobby;
        public Lobby SelectedLobby
        {
            get { return selectedLobby; }
            set
            {
                SetProperty(ref selectedLobby, value);
                OnPropertyChanged(nameof(SelectedLobby));
                (JoinLobbyCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        private List<SolidColorBrush> colors;
        public List<SolidColorBrush> ColorList
        {
            get { return colors; }
            set
            {
                SetProperty(ref colors, value);
            }
        }

        private SolidColorBrush selectedColor;
        public SolidColorBrush SelectedColor
        {
            get { return selectedColor; }
            set
            {
                SetProperty(ref selectedColor, value);
                OnPropertyChanged(nameof(SelectedLobby));
            }
        }
        public MultiplayerGameMenuViewModel(INavigationService lobbyService, INavigationService gameService, INavigationService multimenuService, INavigationService menuService)
        {
            if (MultiLogic.locals == null)
            {
                MultiLogic.locals = new Locals(lobbyService, gameService, multimenuService, menuService);
                MultiLogic.locals.RegisterEvents();
                MultiLogic.locals.RegisterMultiViewMessenger(Messenger);
            }

            ColorList = new List<SolidColorBrush>();
            ColorList.Add(new SolidColorBrush(Colors.Black));
            ColorList.Add(new SolidColorBrush(Colors.IndianRed));
            ColorList.Add(new SolidColorBrush(Colors.LightBlue));
            ColorList.Add(new SolidColorBrush(Colors.ForestGreen));
            ColorList.Add(new SolidColorBrush(Colors.Purple));
            ColorList.Add(new SolidColorBrush(Colors.Yellow));
            ColorList.Add(new SolidColorBrush(Colors.Orange));

            SelectedColor = ColorList[0];

            Lobbies = MultiLogic.locals.Lobbies;
            
            NavigateMainMenuCommand = new NavigateCommand(menuService);
            NavigateLobbyCommand = new NavigateCommand(lobbyService);
            NavigateMultiGameCommand = new NavigateCommand(gameService);

            OnPropertyChanged(nameof(Lobbies));
            OnPropertyChanged(nameof(IsConnected));
            OnPropertyChanged(nameof(NotConnected));

            ConnectServerCommand = new RelayCommand(
                () =>
                {
                    MultiLogic.locals.client.ConnectToServer(Username, SelectedColor.Color.ToString());
                    Thread.Sleep(300);
                    Lobbies = MultiLogic.locals.Lobbies;
                    Trace.WriteLine(SelectedColor.Color.ToString());
                },
                () => !IsConnected
                );
            JoinLobbyCommand = new RelayCommand(
                () => Lobby.Join(MultiLogic.locals, Username, LobbyCode),
                () => SelectedLobby != null
                );
            CreateLobbyCommand = new RelayCommand(
                () => 
                { 
                    Lobby.Create(MultiLogic.locals, Username, 0);
                    Thread.Sleep(300);
                    Lobbies = MultiLogic.locals.Lobbies;
                }
                );

            RefreshLobbies = new RelayCommand(
                () =>
                {
                    Lobbies = MultiLogic.locals.Lobbies;
                    OnPropertyChanged(nameof(Lobbies));
                }
                );

            Messenger.Register<MultiplayerGameMenuViewModel, string, string>(this, "SetUser", (recepient, msg) =>
            {
                UserID = MultiLogic.locals.user.Id;
                Username = MultiLogic.locals.user.Username;
            });

            Messenger.Register<MultiplayerGameMenuViewModel, string, string>(this, "UserConnected", (recepient, msg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    OnPropertyChanged(nameof(Lobbies));
                    OnPropertyChanged(nameof(IsConnected));
                    OnPropertyChanged(nameof(NotConnected));
                    (ConnectServerCommand as RelayCommand).NotifyCanExecuteChanged();
                });
            });
        }
    }
}
