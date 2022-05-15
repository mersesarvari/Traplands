using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class LobbyViewModel:ViewModelBase
    {
        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand SetMapCommand { get; }

        public bool NotLobbyOwner { get { return MultiLogic.locals.lobby.LobbyId == MultiLogic.locals.user.Id ? false : true; } }

        private List<User> users;

        public List<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public Level SelectedLevel { get; set; }

        public LobbyViewModel(INavigationService game, INavigationService menu)
        {
            MultiLogic.locals.RegisterLobbyViewMessenger(Messenger);
            SelectedLevel = LevelManager.GetLevel("Level 1");
            var l = MultiLogic.locals;
            Users = l.lobby.Users;
            StartGameCommand = new RelayCommand(
                () => 
                {
                    Lobby.Start(MultiLogic.locals.lobby, MultiLogic.locals.user.Username);
                }
                );
            SetMapCommand = new RelayCommand(
                () => { MultiLogic.SetMap("MAP1"); }
                );
            NavigateMultiMenuCommand = new NavigateCommand(menu);


            Messenger.Register<LobbyViewModel, string, string>(this, "GameStarted", (recepient, msg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    game.Navigate();
                });
            });
        }
    }
}
