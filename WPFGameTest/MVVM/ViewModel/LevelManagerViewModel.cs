using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Services;

namespace WPFGameTest.MVVM.ViewModel
{
    public class LevelManagerViewModel:ViewModelBase
    {
        public ICommand NavigateSingleGameCommand { get; }
        public LevelManagerViewModel(INavigationService singleGameNavigationService)
        {
            NavigateSingleGameCommand = new NavigateCommand(singleGameNavigationService);
        }
    }
}
