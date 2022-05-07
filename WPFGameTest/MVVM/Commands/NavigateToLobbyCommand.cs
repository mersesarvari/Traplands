using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFGameTest.Models;
using WPFGameTest.MVVM.Services;
using WPFGameTest.MVVM.Stores;
using WPFGameTest.MVVM.ViewModel;

namespace WPFGameTest.MVVM.Commands
{
    public class NavigateToLobbyCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateToLobbyCommand(INavigationService navigationService, Lobby lobby)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
