using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;

namespace Game.MVVM.ViewModel
{
    public class MultiplayerGameMenuViewModel:ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public MultiplayerGameMenuViewModel(INavigationService mainMenuNavigationService, INavigationService lobbyNavigationService, INavigationService multiGameNavigationService)
        {
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
            NavigateLobbyCommand = new NavigateCommand(lobbyNavigationService);
            NavigateMultiGameCommand = new NavigateCommand(multiGameNavigationService);
        }

        

    }
}
