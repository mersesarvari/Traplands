using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Logic;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;

namespace Game.MVVM.ViewModel
{
    public class SingleplayerGameViewModel : ViewModelBase
    {
        public ICommand NavigateMainMenuCommand { get; }

        public SingleplayerGameViewModel(INavigationService mainMenuNavigationService)
        {
            MainWindow.game = new Singleplayer();
            (MainWindow.game as ISingleplayer).SetLevel(LevelManager.CurrentLevel.Name);
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);
        }
    }
}
