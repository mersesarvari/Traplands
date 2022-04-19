using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameTest.MVVM.Stores;
using WPFGameTest.MVVM.ViewModel;

namespace WPFGameTest.MVVM.Commands
{
    public class NavigateMultiGameMenuCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateMultiGameMenuCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new MultiplayerGameMenuViewModel(_navigationStore);
        }
    }
}
