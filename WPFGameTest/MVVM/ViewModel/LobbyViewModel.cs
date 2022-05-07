using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client;
using Client.Models;
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
            
            //Load Users
            LoadItems();
            NavigateMultiMenuCommand= new NavigateCommand(multiMenuNavigationService);


        }

        public void LoadItems()
        {
            var t = Locals.user;
            foreach (var item in Locals.lobby.Users)
            {
                if (!Locals.lobby.Users.Contains(item))
                {
                    Users.Add(item);
                }
            }
        }

        public static void UserJoinedLobbyResponse()
        {
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
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }
    }
}
