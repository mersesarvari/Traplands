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
using Newtonsoft.Json;

namespace Game.MVVM.ViewModel
{
    public class LobbyViewModel:ViewModelBase
    {

        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand NavigateGameCommand { get; }


        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }


        public LobbyViewModel(INavigationService multiMenuNavigationService)
        {
            MessageBox.Show("LobbyViewModel was opened");
            NavigateMultiMenuCommand= new NavigateCommand(multiMenuNavigationService);
            var l = MultiLogic.locals;
            ;

        }
    }
}
