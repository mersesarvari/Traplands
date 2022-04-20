using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<NavigationStore>();

            services.AddSingleton<INavigationService<MainmenuViewModel>>(s => CreateMainMenuNavigationService(s));

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });

            _serviceProvider=services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService<MainmenuViewModel> mainMenuNavigationService = 
                _serviceProvider.GetRequiredService <INavigationService<MainmenuViewModel>>();
            mainMenuNavigationService.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }

        private INavigationService<MainmenuViewModel> CreateMainMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MainmenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => new MainmenuViewModel(
                    CreateMultiMenuNavigationService(serviceProvider),
                    CreateLevelEditorNavigationService(serviceProvider),
                    CreateSingleGameNavigationService(serviceProvider))
                );
        }
        private INavigationService<MultiplayerGameMenuViewModel> CreateMultiMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MultiplayerGameMenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => new MultiplayerGameMenuViewModel(
                    CreateMainMenuNavigationService(serviceProvider),
                    CreateLobbyNavigationService(serviceProvider),
                    CreateMultiGameNavigationService(serviceProvider))
                );
        }

        private INavigationService<LevelEditorViewModel> CreateLevelEditorNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LevelEditorViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
                () => new LevelEditorViewModel(CreateMainMenuNavigationService(serviceProvider))
                );
        }
        private INavigationService<SingleplayerGameViewModel> CreateSingleGameNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<SingleplayerGameViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => new SingleplayerGameViewModel(CreateMainMenuNavigationService(serviceProvider))
                );
        }

        private INavigationService<LobbyViewModel> CreateLobbyNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LobbyViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                ()=>new LobbyViewModel(
                    CreateMultiMenuNavigationService(serviceProvider)
                    )
                );
        }
        private INavigationService<MultiplayerGameViewModel> CreateMultiGameNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MultiplayerGameViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => new MultiplayerGameViewModel(
                    CreateMultiMenuNavigationService(serviceProvider)
                    )
                );
        }



    }
}
