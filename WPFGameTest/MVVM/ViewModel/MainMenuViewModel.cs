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
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ICommand NavigateMultiGameMenuCommand { get; }
        public ICommand NavigateLevelEditorCommand { get; }
        public ICommand NavigateSingleGameCommand { get; }

        public MainmenuViewModel(NavigationBarViewModel navigationBarViewModel, 
            NavigationService<MultiplayerGameMenuViewModel> multiMenuNavigationService,
            NavigationService<LevelEditorViewModel> levelEditorNavigationService,
            NavigationService<SingleplayerGameViewModel> singleGameNavigationService)
        {
            NavigateMultiGameMenuCommand = new NavigateCommand<MultiplayerGameMenuViewModel>(multiMenuNavigationService);
            NavigateLevelEditorCommand=new NavigateCommand<LevelEditorViewModel>(levelEditorNavigationService);
            NavigateSingleGameCommand = new NavigateCommand<SingleplayerGameViewModel>(singleGameNavigationService);

            NavigationBarViewModel = navigationBarViewModel;
        }
    }
}
