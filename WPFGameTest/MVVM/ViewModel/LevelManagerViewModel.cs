using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using System.Collections.ObjectModel;

namespace Game.MVVM.ViewModel
{
    public class LevelManagerViewModel : ViewModelBase
    {
        private List<Level> levels;
        public List<Level> Levels
        {
            get { return levels; }
            set { SetProperty(ref levels, value); }
        }

        private Level selectedLevel;
        public Level SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                SetProperty(ref selectedLevel, value);
                LevelManager.CurrentLevel = SelectedLevel;
                (NavigateSingleGameCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand NavigateSingleGameCommand { get; set; }
        public LevelManagerViewModel(INavigationService singleGameNavigationService)
        {
            Levels = LevelManager.LevelList();
            LevelManager.CurrentLevel = SelectedLevel;
            NavigateSingleGameCommand =
               new RelayCommand(
               () => { singleGameNavigationService.Navigate(); },
               () => selectedLevel != null
               );
        }
    }
}
