using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.Core;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Stores;

namespace WPFGameTest.MVVM.ViewModel
{
    internal class MainmenuViewModel:ViewModelBase
    {
        public ICommand NavigateMultiGameMenuCommand { get; }
        public ICommand NavigateLevelEditorCommand { get; }
        public ICommand NavigateSingleGameCommand { get; }

        public MainmenuViewModel(NavigationStore navigationStore)
        {
            NavigateMultiGameMenuCommand = new NavigateCommand<MultiplayerGameMenuViewModel>
                (navigationStore, ()=>new MultiplayerGameMenuViewModel(navigationStore));
            NavigateSingleGameCommand = new NavigateCommand<SingleplayerGameViewModel>
                (navigationStore, () => new SingleplayerGameViewModel(navigationStore));
            NavigateLevelEditorCommand=new NavigateCommand<LevelEditorViewModel>
                (navigationStore, () => new LevelEditorViewModel(navigationStore));
        }
    }
}
