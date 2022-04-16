using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameTest.Core;

namespace WPFGameTest.MVVM.ViewModel
{    
    internal class MainWindowViewModel:ObservableObject
    {
        public static MainWindowViewModel Instance;
        public LobbyViewModel LobbyMV { get; set; }
        public MainmenuViewModel MainMenuVM { get; set; }
        public MultiplayerGameMenuViewModel MultiplayerGameMenuVM { get; set; }
        public MultiplayerGameViewModel MultiplayerGameVM { get; set; }
        public SingleplayerGameViewModel SingleplayerGameVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            LobbyMV = new LobbyViewModel();
            MainMenuVM = new MainmenuViewModel();
            MultiplayerGameMenuVM = new MultiplayerGameMenuViewModel();
            MultiplayerGameVM = new MultiplayerGameViewModel();
            SingleplayerGameVM = new SingleplayerGameViewModel();   
            CurrentView = MainMenuVM;

        }

        public void ChangeCurrentView()
        { 
            Instance.CurrentView = CurrentView;
        }


    }
}
