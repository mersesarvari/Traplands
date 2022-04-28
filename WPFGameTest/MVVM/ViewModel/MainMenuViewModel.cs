using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.Core;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Services;
using WPFGameTest.MVVM.Stores;

namespace WPFGameTest.MVVM.ViewModel
{
    public class MainmenuViewModel:ViewModelBase
    {
        public ICommand NavigateMultiGameMenuCommand { get; }
        public ICommand NavigateLevelEditorCommand { get; }
        public ICommand NavigateLevelManagerCommand { get; }

        public MainmenuViewModel( 
            INavigationService multiMenuNavigationService,
            INavigationService levelEditorNavigationService,
            INavigationService levelManagerService
            )
        {
            NavigateMultiGameMenuCommand = new NavigateCommand(multiMenuNavigationService);
            NavigateLevelEditorCommand=new NavigateCommand(levelEditorNavigationService);
            NavigateLevelManagerCommand = new NavigateCommand(levelManagerService);
        }
    }
}
