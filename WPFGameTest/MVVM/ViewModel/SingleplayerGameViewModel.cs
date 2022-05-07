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
    public class SingleplayerGameViewModel:ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }
        public SingleplayerGameViewModel(INavigationService mainMenuNavigationService)
        {
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
        }
    }
}
