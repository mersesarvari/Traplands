using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
    public class LobbyViewModel : ViewModelBase
    {
        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand SetMapCommand { get; }
        public ICommand SendMessageCommand { get; set; }

        public bool NotLobbyOwner { get { return MultiLogic.locals.lobby.LobbyId == MultiLogic.locals.user.Id ? false : true; } }

        private List<User> users;
        public List<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public Level SelectedLevel { get; set; }

        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set { SetProperty(ref messageText, value); }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
            }
        }

        public LobbyViewModel(INavigationService game, INavigationService menu)
        {
            MultiLogic.locals.RegisterLobbyViewMessenger(Messenger);
            SelectedLevel = LevelManager.GetLevel("Level 1");
            var l = MultiLogic.locals;
            Users = l.lobby.Users;

            Messages = new ObservableCollection<Message>(MultiLogic.locals.lobby.Messages);
            MultiLogic.locals.SetupCollection(Messages);

            StartGameCommand = new RelayCommand(
                () => 
                {
                    Lobby.Start(MultiLogic.locals.lobby, MultiLogic.locals.user.Username);
                }
                );
            SetMapCommand = new RelayCommand(
                () => { MultiLogic.SetMap("MAP1"); }
                );

            SendMessageCommand = new RelayCommand(
                () => { Message msg = new Message(MultiLogic.locals.user.Id, MessageText); Lobby.SendMessage(msg); Trace.WriteLine(MessageText); }
                );

            NavigateMultiMenuCommand = new NavigateCommand(menu);


            Messenger.Register<LobbyViewModel, string, string>(this, "GameStarted", (recepient, msg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    game.Navigate();
                });
            });

            Messenger.Register<LobbyViewModel, string, string>(this, "MessageRecieved", (recepient, msg) =>
            {
                OnPropertyChanged(nameof(Messages));
                MessageText = "";
                OnPropertyChanged(nameof(MessageText));
            });
        }
    }
}
