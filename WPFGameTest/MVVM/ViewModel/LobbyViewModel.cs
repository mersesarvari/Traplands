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
            MessageBox.Show("LobbyViewModel was opened");
            NavigateMultiMenuCommand= new NavigateCommand(multiMenuNavigationService);
            var l = MultiLogic.locals;
            Users = l.lobby.Users;
            

        }
    }
}
