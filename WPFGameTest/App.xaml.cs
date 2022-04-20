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

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //_navigationStore.CurrentViewModel = new MainmenuViewModel(_navigationBarViewModel, _navigationStore);
            INavigationService<MainmenuViewModel> mainMenuNavigationService = CreateMainMenuNavigationService();
            mainMenuNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private INavigationService<MainmenuViewModel> CreateMainMenuNavigationService()
        {
            return new NavigationService<MainmenuViewModel>(
                _navigationStore,
                () => new MainmenuViewModel(
                    CreateMultiMenuNavigationService(),
                    CreateLevelEditorNavigationService(),
                    CreateSingleGameNavigationService())
                );
        }
        private INavigationService<MultiplayerGameMenuViewModel> CreateMultiMenuNavigationService()
        {
            return new NavigationService<MultiplayerGameMenuViewModel>(
                _navigationStore, 
                () => new MultiplayerGameMenuViewModel(
                    CreateMainMenuNavigationService(),
                    CreateLobbyNavigationService(),
                    CreateMultiGameNavigationService())
                );
        }

        private INavigationService<LevelEditorViewModel> CreateLevelEditorNavigationService()
        {
            return new NavigationService<LevelEditorViewModel>(
                _navigationStore,
                () => new LevelEditorViewModel(CreateMainMenuNavigationService())
                );
        }
        private INavigationService<SingleplayerGameViewModel> CreateSingleGameNavigationService()
        {
            return new NavigationService<SingleplayerGameViewModel>(
                _navigationStore,
                () => new SingleplayerGameViewModel(CreateMainMenuNavigationService())
                );
        }

        private INavigationService<LobbyViewModel> CreateLobbyNavigationService()
        {
            return new NavigationService<LobbyViewModel>(
                _navigationStore,
                ()=>new LobbyViewModel(
                    CreateMultiMenuNavigationService()
                    )
                );
        }
        private INavigationService<MultiplayerGameViewModel> CreateMultiGameNavigationService()
        {
            return new NavigationService<MultiplayerGameViewModel>(
                _navigationStore,
                () => new MultiplayerGameViewModel(
                    CreateMultiMenuNavigationService()
                    )
                );
        }



    }
}
