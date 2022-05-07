using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using Game.MVVM.ViewModel;

namespace Game.MVVM.Commands
{
    public class SaveAndExitCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public SaveAndExitCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            MessageBox.Show("Map saved");

            //Navigate to the MainMenu page
            _navigationService.Navigate();

        }
    }
}
