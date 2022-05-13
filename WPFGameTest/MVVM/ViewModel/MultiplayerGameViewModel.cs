﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Logic;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Microsoft.Toolkit.Mvvm.Input;

namespace Game.MVVM.ViewModel
{
    public class MultiplayerGameViewModel : ViewModelBase
    {
        IMultiplayer logic;

        public string GameState { get { return logic.GameOver ? "Finish" : "Paused"; } }

        public float LevelTimeElapsed { get { return logic.LevelTimer; } }

        public bool GamePaused { get { return logic.Paused; } }

        public bool GameOver { get { return logic.GameOver; } }

        public double TransitionAlpha { get { return logic.TransitionAlpha; } }

        public bool Transitioning { get { return logic.Transitioning; } }

        public ICommand NavigateMultiMenuCommand { get; }
        public ICommand DisconnectFromServer { get; set; }
        public ICommand LeaveGame { get; set; }

        public MultiplayerGameViewModel(INavigationService multiMenuNavigationService)
        {
            this.logic = (MainWindow.game as Multiplayer);
            (MainWindow.game as Multiplayer).SetMessenger(Messenger);
            NavigateMultiMenuCommand = new NavigateCommand(multiMenuNavigationService);

            LeaveGame = new RelayCommand(() => 
            { 
                MultiLogic.LeaveGame(MultiLogic.locals.lobby, MultiLogic.locals.user.Id);
            });

            DisconnectFromServer = new RelayCommand(() =>
            {
                MultiLogic.Disconnect(MultiLogic.locals.user.Id);
                multiMenuNavigationService.Navigate();
            });
        }
    }
}
