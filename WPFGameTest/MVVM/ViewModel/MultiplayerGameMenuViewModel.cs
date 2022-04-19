using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Stores;

namespace WPFGameTest.MVVM.ViewModel
{
    public class MultiplayerGameMenuViewModel:ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }
        public ICommand NavigateLobbyCommand { get; }
        public ICommand NavigateMultiGameCommand { get; }
        public MultiplayerGameMenuViewModel(NavigationStore navigationStore)
        {
            NavigateMainMenuCommand = new NavigateCommand<MainmenuViewModel>
                (navigationStore, () => new MainmenuViewModel(navigationStore));

            NavigateLobbyCommand = new NavigateCommand<LobbyViewModel>
                (navigationStore, () => new LobbyViewModel(navigationStore));

            NavigateMultiGameCommand = new NavigateCommand<MultiplayerGameViewModel>
                (navigationStore, () => new MultiplayerGameViewModel(navigationStore));
        }
    }
}
