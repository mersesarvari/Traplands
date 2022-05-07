using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.MVVM.Commands;
using Game.MVVM.Services;

namespace Game.MVVM.ViewModel
{
    public class NavigationBarViewModel:ViewModelBase
    {
        public ICommand NavigateMultiGameMenuCommand { get; }
        public ICommand NavigateLevelEditorCommand { get; }
        public ICommand NavigateSingleGameCommand { get; }

        public NavigationBarViewModel(
            INavigationService multiMenuNavigationService, 
            INavigationService levelEditorNavigationService, 
            INavigationService singleGameNavigationService)
        {
            NavigateMultiGameMenuCommand = new NavigateCommand(multiMenuNavigationService);
            NavigateLevelEditorCommand = new NavigateCommand(levelEditorNavigationService);
            NavigateSingleGameCommand = new NavigateCommand(singleGameNavigationService);
        }
    }
}
