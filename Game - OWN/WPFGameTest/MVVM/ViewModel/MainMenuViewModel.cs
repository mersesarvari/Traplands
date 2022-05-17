using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Game.Core;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Game.Renderer;
using Microsoft.Toolkit.Mvvm.Input;

namespace Game.MVVM.ViewModel
{
    public class MainmenuViewModel:ViewModelBase
    {
        public ICommand NavigateMultiGameMenuCommand { get; }
        public ICommand NavigateLevelEditorCommand { get; }
        public ICommand NavigateLevelManagerCommand { get; }
        public ICommand Exit { get; set; }

        public MainmenuViewModel( 
            INavigationService multiMenuNavigationService,
            INavigationService levelEditorNavigationService,
            INavigationService levelManagerService
            )
        {
            AudioManager.SetBackgroundMusic("27-Dark Fantasy Studio- Silent walk.wav");

            MainWindow.game = null;

            NavigateMultiGameMenuCommand = new NavigateCommand(multiMenuNavigationService);
            NavigateLevelEditorCommand=new NavigateCommand(levelEditorNavigationService);
            NavigateLevelManagerCommand = new NavigateCommand(levelManagerService);

            Exit = new RelayCommand(() => Application.Current.Shutdown());
        }
    }
}
