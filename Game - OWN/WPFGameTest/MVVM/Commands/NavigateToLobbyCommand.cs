using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Game.MVVM.ViewModel;

namespace Game.MVVM.Commands
{


    public class NavigateToLobbyCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateToLobbyCommand(INavigationService navigationService, Lobby Lobby)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
