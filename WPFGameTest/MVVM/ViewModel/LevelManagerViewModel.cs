using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFGameTest.Models;
using WPFGameTest.MVVM.Commands;
using WPFGameTest.MVVM.Services;

namespace WPFGameTest.MVVM.ViewModel
{
    public class LevelManagerViewModel:ViewModelBase
    {
        private Dictionary<string, Level> levels;
        public Dictionary<string, Level> Levels
        {
            get { return levels; }
        }

        private KeyValuePair<string, Level> selectedLevel;

        public KeyValuePair<string, Level> SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                if (value.Key!=null && value.Value!=null)
                {
                    selectedLevel = new KeyValuePair<string, Level>(value.Key, value.Value);
                }
                OnPropertyChanged();
                //(NavigateSingleGameCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand NavigateSingleGameCommand { get; set; }
        public LevelManagerViewModel(INavigationService singleGameNavigationService)
        {
            NavigateSingleGameCommand = new NavigateCommand(singleGameNavigationService);
            levels = LevelManager.Levels;
            //NavigateSingleGameCommand = new RelayCommand(
            //   () => { new NavigateCommand(singleGameNavigationService); },
            //   () => selectedLevel.Key != null
            //   );
        }
    }
}
