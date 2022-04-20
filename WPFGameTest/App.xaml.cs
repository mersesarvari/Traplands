using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFGameTest.MVVM.Services;
using WPFGameTest.MVVM.Stores;
using WPFGameTest.MVVM.ViewModel;

namespace WPFGameTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationBarViewModel _navigationBarViewModel;

        public App(NavigationStore navigationStore, NavigationBarViewModel navigationBarViewModel)
        {
            _navigationStore = navigationStore;
            _navigationBarViewModel = new NavigationBarViewModel(
                CreateMultiMenuNavigationService(),
                CreateLevelEditorNavigationService(),
                CreateSingleGameNavigationService()
                );
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new MainmenuViewModel(_navigationBarViewModel, _navigationStore);    


            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private NavigationService<MultiplayerGameMenuViewModel> CreateMultiMenuNavigationService()
        {
            return new NavigationService<MultiplayerGameMenuViewModel>(
                _navigationStore, 
                () => new MultiplayerGameMenuViewModel(_navigationStore)
                );
        }

        private NavigationService<LevelEditorViewModel> CreateLevelEditorNavigationService()
        {
            return new NavigationService<LevelEditorViewModel>(
                _navigationStore,
                () => new LevelEditorViewModel(_navigationStore)
                );
        }
        private NavigationService<SingleplayerGameViewModel> CreateSingleGameNavigationService()
        {
            return new NavigationService<SingleplayerGameViewModel>(
                _navigationStore,
                () => new SingleplayerGameViewModel(_navigationStore)
                );
        }

        
    }
}
