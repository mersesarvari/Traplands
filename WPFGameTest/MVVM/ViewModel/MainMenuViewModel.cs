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

        public MainmenuViewModel(NavigationBarViewModel navigationBarViewModel, NavigationStore navigationStore)
        {
            NavigateMultiGameMenuCommand = new NavigateCommand<MultiplayerGameMenuViewModel>(new NavigationService<MultiplayerGameMenuViewModel>
                (navigationStore, ()=>new MultiplayerGameMenuViewModel(navigationStore)));
            NavigateSingleGameCommand = new NavigateCommand<SingleplayerGameViewModel>(new NavigationService<SingleplayerGameViewModel>
                (navigationStore, () => new SingleplayerGameViewModel(navigationStore)));
            NavigateLevelEditorCommand=new NavigateCommand<LevelEditorViewModel>(new NavigationService<LevelEditorViewModel>
                (navigationStore, () => new LevelEditorViewModel(navigationStore)));

            NavigationBarViewModel = navigationBarViewModel;
        }
    }
}
