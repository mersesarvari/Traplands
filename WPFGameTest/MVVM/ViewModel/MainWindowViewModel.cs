using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.Core;
using WPFGameTest.MVVM.Stores;

namespace WPFGameTest.MVVM.ViewModel
{    
    public class MainWindowViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        ////public MainWindowViewModel Instance;

        //public event EventHandler ViewChanged;
        //LobbyViewModel LobbyMV { get; set; }
        //MainmenuViewModel MainMenuVM { get; set; }
        //MultiplayerGameMenuViewModel MultiplayerGameMenuVM { get; set; }
        //MultiplayerGameViewModel MultiplayerGameVM { get; set; }
        //SingleplayerGameViewModel SingleplayerGameVM { get; set; }

        //public ICommand ViewChangeCommand { get; set; } 

        //object _currentView=new LobbyViewModel();

        //public object CurrentView
        //{
        //    get { return _currentView; }
        //    set {
        //        _currentView = value;
        //        //OnPropertyChanged();
        //    }
        //}


        //public MainWindowViewModel()
        //{
        //    CurrentView = LobbyMV;
        //    ViewChangeCommand = new RelayCommand(
        //        () => ShowLobby(),
        //        () => true
        //    );

        //    //Instance = this;
        //    LobbyMV = new LobbyViewModel();
        //    MainMenuVM = new MainmenuViewModel();
        //    MultiplayerGameMenuVM = new MultiplayerGameMenuViewModel();
        //    MultiplayerGameVM = new MultiplayerGameViewModel();
        //    SingleplayerGameVM = new SingleplayerGameViewModel();
        //    //Instance.CurrentView = MainMenuVM;


        //}

        //public void ShowLobby()
        //{
        //    //CurrentView = LobbyMV;
        //    CurrentView = LobbyMV;
        //    ViewChanged?.Invoke(this, null);
        //    (ViewChangeCommand as RelayCommand).NotifyCanExecuteChanged();
        //}


    }
}
