using System;
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
    public class LobbyViewModel:ViewModelBase
    {
        public ICommand NavigateMultiMenuCommand { get; }
        public LobbyViewModel(NavigationService<MultiplayerGameMenuViewModel> multiMenuNavigationService)
        {
            NavigateMultiMenuCommand= new NavigateCommand<MultiplayerGameMenuViewModel>(multiMenuNavigationService);
        }
    }
}
