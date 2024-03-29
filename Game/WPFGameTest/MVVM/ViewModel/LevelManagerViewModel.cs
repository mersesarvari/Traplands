﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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
                OnPropertyChanged(nameof(SelectedLevel));
                LevelManager.CurrentLevel = SelectedLevel;
                (NavigateSingleGameCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand NavigateSingleGameCommand { get; set; }
        public ICommand NavigateMenuCommand { get; set; }
        public ICommand PlayCampaign { get; set; }

        public LevelManagerViewModel(INavigationService singleGameNavigationService, INavigationService menuNavigationService)
        {
            Levels = LevelManager.LevelList();
            LevelManager.CurrentLevel = SelectedLevel;

            NavigateSingleGameCommand =
               new RelayCommand(
               () => { singleGameNavigationService.Navigate(); },
               () => selectedLevel != null
               );

            PlayCampaign =
               new RelayCommand(
               () => { LevelManager.CurrentLevel = LevelManager.GetLevel("Level 0"); singleGameNavigationService.Navigate(); }
               );

            NavigateMenuCommand =
               new RelayCommand(
               () => { menuNavigationService.Navigate(); }
               );
        }
    }
}
