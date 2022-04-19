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
        public MultiplayerGameMenuViewModel(NavigationStore navigationStore)
        {
            NavigateMainMenuCommand = new NavigateMainMenuCommand(navigationStore);
        }
    }
}
