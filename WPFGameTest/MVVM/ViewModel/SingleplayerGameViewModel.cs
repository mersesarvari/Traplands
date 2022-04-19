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
    public class SingleplayerGameViewModel:ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }
        public SingleplayerGameViewModel(NavigationStore navigationStore)
        {
            NavigateMainMenuCommand = new NavigateCommand<MainmenuViewModel>
                (navigationStore, () => new MainmenuViewModel(navigationStore));
        }
    }
}
