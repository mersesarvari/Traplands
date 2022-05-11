using System;
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
    public class SingleplayerGameViewModel : ViewModelBase
    {
        private ISingleplayer logic;

        public ICommand NavigateMainMenuCommand { get; set; }
        public ICommand ResumeGame { get; set; }

        public float LevelTimeElapsed { get { return logic.LevelTimer; } }

        public bool GamePaused { get { return logic.Paused; } }

        public bool GameOver { get { return logic.GameOver; } }

        public double TransitionAlpha { get { return logic.TransitionAlpha; } }

        public bool Transitioning { get { return logic.Transitioning; } }

        public SingleplayerGameViewModel(INavigationService mainMenuNavigationService)
        {
            logic = new Singleplayer(Messenger);
            MainWindow.game = logic;
            (MainWindow.game as ISingleplayer).SetLevel(LevelManager.CurrentLevel.Name);

            NavigateMainMenuCommand = new RelayCommand(() => { logic.SaveLevel(); mainMenuNavigationService.Navigate(); });
            ResumeGame = new RelayCommand(() => { logic.Paused = false; }, () => !GameOver);

            Messenger.Register<SingleplayerGameViewModel, string, string>(this, "LevelTimerUpdate", (recepient, msg) =>
            {
                OnPropertyChanged(nameof(LevelTimeElapsed));
            });

            Messenger.Register<SingleplayerGameViewModel, string, string>(this, "GamePaused", (recepient, msg) =>
            {
                OnPropertyChanged(nameof(GamePaused));
                OnPropertyChanged(nameof(GameOver));
                (ResumeGame as RelayCommand).NotifyCanExecuteChanged();
            });

            Messenger.Register<SingleplayerGameViewModel, string, string>(this, "Transition", (recepient, msg) =>
            {
                OnPropertyChanged(nameof(TransitionAlpha));
                OnPropertyChanged(nameof(Transitioning));
            });
        }
    }
}
