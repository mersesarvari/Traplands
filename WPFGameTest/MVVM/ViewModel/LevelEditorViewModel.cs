using Microsoft.Toolkit.Mvvm.ComponentModel;
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
    public class LevelEditorViewModel:ViewModelBase
    {
        ObservableObject selected;

        public ICommand NavigateMainMenuCommand { get; }
        public ICommand SaveAndExitCommand { get; }

        private ILevelEditor logic;

        public LevelEditorViewModel(INavigationService mainMenuNavigationService)
        {
            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);

            SaveAndExitCommand = new SaveAndExitCommand(mainMenuNavigationService);
        }
    }
}
