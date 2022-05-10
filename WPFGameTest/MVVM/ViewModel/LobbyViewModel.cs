using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Newtonsoft.Json;

namespace Game.MVVM.ViewModel
{
    public class LobbyViewModel:ViewModelBase
    {
        public Locals locals { get; set; }

        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand NavigateGameCommand { get; }


        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; }
        }


        public LobbyViewModel(INavigationService multiMenuNavigationService, Locals locals)
        {
            this.locals = locals;
            //Load Users
            LoadItems();
            NavigateMultiMenuCommand= new NavigateCommand(multiMenuNavigationService);


        }

        public void LoadItems()
        {
            var t = locals.user;
            foreach (var item in locals.lobby.Users)
            {
                if (!locals.lobby.Users.Contains(item))
                {
                    Users.Add(item);
                }
            }
        }

        public void UserJoinedLobbyResponse()
        {
            //This method is handling the JoinResponse from the server
            var msg = locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];
                if (status != "ERROR" && status != "Success")
                {
                    locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                    ;
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }
    }
}
