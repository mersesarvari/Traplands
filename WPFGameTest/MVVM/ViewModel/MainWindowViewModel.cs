using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Core;
using Game.MVVM.Stores;
using Game.Models;
using Newtonsoft.Json;

namespace Game.MVVM.ViewModel
{    
    public class MainWindowViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            Locals.client.zeroopcodeEvent += ZeroOpcodeEventHandler;
            Locals.client.connectedEvent += UserConnected;
            Locals.client.userDisconnectedEvent += UserDisconnected;
            Locals.client.userJoinedLobbyEvent += UserJoinedLobbyResponse;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void ZeroOpcodeEventHandler()
        {
            Console.Write("Zero Opcode called");
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        //Server-Client Methods
        #region Server-Client methods
        //Nem hívódik meg valamiért
        private void UserConnected()
        {

            Locals.user.Username = Locals.client.PacketReader.ReadMessage();
            Locals.user.Id = Locals.client.PacketReader.ReadMessage();
            //Setting up the timer <==> Sync with the server
            //GameTimer.Tick = int.Parse(Locals.client.PacketReader.ReadMessage());
            var u = Locals.user;
        }
        private void UserDisconnected()
        {
            var uid = Locals.client.PacketReader.ReadMessage();
            //MessageBox.Show("User disconnected");
        }

        public static void UserJoinedLobbyResponse()
        {
            //MessageBox.Show("USER JOINED LOBBY RESPONSE ARRIVED");
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
                //this is doing the navigation

            }
            else
            {
                //MessageBox.Show("Response Message format is bad:" + msg);
            }
        }

        #endregion
    }
}
