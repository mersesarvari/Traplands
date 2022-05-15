using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Game.MVVM.ViewModel;
using Game.Models;
using Game.Logic;
using Ninject;

namespace Game
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
            //Saját rész

            services.AddSingleton<INavigationService>(s => CreateMainMenuNavigationService(s));

            services.AddTransient<MultiLogic>(logic => new MultiLogic(
                CreateLobbyNavigationService(logic),
                CreateMultiGameNavigationService(logic),
                CreateMultiMenuNavigationService(logic),
                CreateMainMenuNavigationService(logic)
                ));
            //Locals Meghat'roy'sa
            services.AddTransient<MainmenuViewModel>(s => new MainmenuViewModel(
                CreateMultiMenuNavigationService(s),
                CreateLevelEditorNavigationService(s),
                CreateLevelManagerNavigationService(s)
                ));
            services.AddTransient<MultiplayerGameMenuViewModel>(s => new MultiplayerGameMenuViewModel(                
                CreateLobbyNavigationService(s),
                CreateMultiGameNavigationService(s),
                CreateMultiMenuNavigationService(s),
                CreateMainMenuNavigationService(s)
                ));

            services.AddTransient<LevelEditorViewModel>(s => new LevelEditorViewModel(
                CreateMainMenuNavigationService(s)));
            services.AddTransient<LevelManagerViewModel>(s => new LevelManagerViewModel(
                CreateSingleGameNavigationService(s),
                CreateMainMenuNavigationService(s)));
            services.AddTransient<SingleplayerGameViewModel>(s => new SingleplayerGameViewModel(
                CreateMainMenuNavigationService(s)));
            services.AddTransient<LobbyViewModel>(s => new LobbyViewModel(
                CreateMultiGameNavigationService(s), 
                CreateMultiMenuNavigationService(s)));
            services.AddTransient<MultiplayerGameViewModel>(s => new MultiplayerGameViewModel(
                CreateMultiMenuNavigationService(s)));

            services.AddSingleton<MainWindowViewModel>();            
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
            

            _serviceProvider=services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService mainMenuNavigationService = 
                _serviceProvider.GetRequiredService <INavigationService>();
            mainMenuNavigationService.Navigate();
            

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Visibility = Visibility.Visible;
            //MainWindow.Show();
            MainWindow.Title = "Traplands";
            
            base.OnStartup(e);
        }
        #region Navigatons
        private INavigationService CreateMainMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MainmenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<MainmenuViewModel>());
        }
        private INavigationService CreateMultiMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MultiplayerGameMenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<MultiplayerGameMenuViewModel>());
        }

        private INavigationService CreateLevelEditorNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LevelEditorViewModel>(
               serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LevelEditorViewModel>());
        }
        private INavigationService CreateLevelManagerNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LevelManagerViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LevelManagerViewModel>());
        }
        private INavigationService CreateSingleGameNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<SingleplayerGameViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SingleplayerGameViewModel>());
        }
        private INavigationService CreateLobbyNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LobbyViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<LobbyViewModel>());
        }
        private INavigationService CreateMultiGameNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<MultiplayerGameViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<MultiplayerGameViewModel>());
        }
        #endregion


    }
}
