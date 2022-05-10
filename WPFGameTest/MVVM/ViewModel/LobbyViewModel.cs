using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ICommand StartGame { get; }
        public ICommand SetMap { get; }


    private List<User> users;

        public List<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }



        public LobbyViewModel(INavigationService multiMenuNavigationService)
        {
            var l = MultiLogic.locals;
            Users = l.lobby.Users;
            StartGame = new RelayCommand(
                () => { MultiLogic.StartGame(MultiLogic.locals.user.Id); }
                );
            SetMap = new RelayCommand(
                () => { MultiLogic.SetMap(); }
                );
            NavigateMultiMenuCommand = new NavigateCommand(multiMenuNavigationService);


            


            

        }
    }
}
