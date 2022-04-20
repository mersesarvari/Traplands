﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Services;
using WPFGameTest.MVVM.Stores;

namespace WPFGameTest.MVVM.ViewModel
{
    public class MultiplayerGameMenuViewModel:ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public MultiplayerGameMenuViewModel(NavigationService<MainmenuViewModel> mainMenuNavigationService, 
            NavigationService<LobbyViewModel> lobbyNavigationService, 
            NavigationService<MultiplayerGameViewModel> multiGameNavigationService)
        {
            NavigateMainMenuCommand = new NavigateCommand<MainmenuViewModel>(mainMenuNavigationService);

            NavigateLobbyCommand = new NavigateCommand<LobbyViewModel>(lobbyNavigationService);

            NavigateMultiGameCommand = new NavigateCommand<MultiplayerGameViewModel>(multiGameNavigationService);
        }
    }
}
