using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class LobbyViewModel:ViewModelBase
    {
        private MultiLogic logic;
        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand SetMapCommand { get; }


        private List<User> users;

        public List<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public Level SelectedLevel { get; set; }

        public LobbyViewModel(INavigationService game, INavigationService menu)
        {
            MultiLogic.locals.RegisterEvents();
            SelectedLevel = LevelManager.GetLevel("Level 1");
            var l = MultiLogic.locals;
            Users = l.lobby.Users;
            StartGameCommand = new RelayCommand(
                () => 
                {
                    MultiLogic.StartGame(MultiLogic.locals.lobby, MultiLogic.locals.user.Username);
                }
                );
            SetMapCommand = new RelayCommand(
                () => { MultiLogic.SetMap("MAP1"); }
                );
            NavigateMultiMenuCommand = new NavigateCommand(menu);
        }
    }
}
