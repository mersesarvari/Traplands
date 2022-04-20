using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFGameTest.MVVM.Stores;
using WPFGameTest.MVVM.ViewModel;

namespace WPFGameTest.MVVM.Commands
{
    public class SaveAndExitCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public SaveAndExitCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            MessageBox.Show("Map saved");

            //Navigate to the MainMenu page
            _navigationStore.CurrentViewModel = new MainmenuViewModel(_navigationStore);

        }
    }
}
