using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Logic;
using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Microsoft.Toolkit.Mvvm.Input;

namespace Game.MVVM.ViewModel
{
    public class MultiplayerGameViewModel : ViewModelBase
    {
        Multiplayer logic;

        public string GameState { get { return logic.GameOver ? "Finish" : "Paused"; } }

        public float LevelTimeElapsed { get { return logic.LevelTimer; } }

        public bool GamePaused { get { return logic.Paused; } }

        public bool GameOver { get { return logic.GameOver; } }

        public double TransitionAlpha { get { return logic.TransitionAlpha; } }

        public bool Transitioning { get { return logic.Transitioning; } }

        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand ResumeGame { get; set; }
        public ICommand DisconnectFromServer { get; set; }
        public ICommand LeaveGame { get; set; }

        public MultiplayerGameViewModel(INavigationService multiMenuNavigationService)
        {
            logic = new Multiplayer();
            MainWindow.game = logic;
            logic.SetMessenger(Messenger);
            logic.LoadLevel("Level 1");
            //Ez a sor ami nem engedi elindulni a programot
            logic.LoadPlayers();
            
            NavigateMultiMenuCommand = new NavigateCommand(multiMenuNavigationService);

            ResumeGame = new RelayCommand(() => { logic.Paused = false; }, () => !GameOver);

            LeaveGame = new RelayCommand(() => 
            { 
                Lobby.Leave(MultiLogic.locals.lobby, MultiLogic.locals.user.Id);
            });

            DisconnectFromServer = new RelayCommand(() =>
            {
                Client.Disconnect(MultiLogic.locals.user.Id);
                multiMenuNavigationService.Navigate();
            });

            //Messenger.Register<MultiplayerGameViewModel, string, string>(this, "LevelTimerUpdate", (recepient, msg) =>
            //{
            //    OnPropertyChanged(nameof(LevelTimeElapsed));
            //});

            //Messenger.Register<MultiplayerGameViewModel, string, string>(this, "GamePaused", (recepient, msg) =>
            //{
            //    OnPropertyChanged(nameof(GamePaused));
            //    OnPropertyChanged(nameof(GameOver));
            //    OnPropertyChanged(nameof(GameState));
            //    (ResumeGame as RelayCommand).NotifyCanExecuteChanged();
            //});

            //Messenger.Register<MultiplayerGameViewModel, string, string>(this, "Transition", (recepient, msg) =>
            //{
            //    OnPropertyChanged(nameof(TransitionAlpha));
            //    OnPropertyChanged(nameof(Transitioning));
            //});
        }
    }
}
