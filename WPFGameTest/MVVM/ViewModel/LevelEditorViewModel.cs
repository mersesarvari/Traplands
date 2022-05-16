using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Logic;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Mvvm.Input;
using WPFGameTest.Logic;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Game.Models;

namespace Game.MVVM.ViewModel
{
    public class LevelEditorViewModel : ViewModelBase
    {
        ILevelEditor logic;

        private ObservableCollection<EditorElement> elements;
        public ObservableCollection<EditorElement> Elements
        {
            get
            {
                return elements;
            }
            set
            {
                SetProperty(ref elements, value);
            }
        }

        private EditorElement selectedObject;
        public EditorElement SelectedObject
        {
            get { return selectedObject; }
            set
            {
                SetProperty(ref selectedObject, value);
                logic.SelectElement(value);
            }
        }

        public CannonRect SelectedCannon
        {
            get { return logic.SelectedCannon; }
        }

        private string levelName;
        public string LevelName
        {
            get { return levelName; }
            set
            {
                SetProperty(ref levelName, value);
                (SaveLevel as RelayCommand).NotifyCanExecuteChanged();
            }
        }

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
                (LoadLevel as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public void SetupLogic(FrameworkElement renderTarget, ScrollViewer camera)
        {
            logic.Init(renderTarget, camera);
        }

        public IGameModel GetLogic()
        {
            return logic;
        }

        public ICommand SaveLevel { get; set; }
        public ICommand ExitWithoutSaving { get; set; }
        public ICommand FlipCannon { get; set; }
        public ICommand LoadLevel { get; set; }

        public LevelEditorViewModel(INavigationService mainMenuNavigationService)
        {
            this.logic = new LevelEditor(Messenger);
            MainWindow.game = logic;
            SaveLevel = new RelayCommand(() => { logic.SaveLevel(LevelName); mainMenuNavigationService.Navigate(); }, () => !string.IsNullOrWhiteSpace(LevelName));
            ExitWithoutSaving = new RelayCommand(() => { mainMenuNavigationService.Navigate(); });
            FlipCannon = new RelayCommand(() => { logic.FlipCannon(SelectedCannon); }, () => SelectedCannon != null);
            LoadLevel = new RelayCommand(() => { logic.LoadLevel(SelectedLevel); LevelName = SelectedLevel.Name; }, () => SelectedLevel != null);

            Levels = LevelManager.LevelList();

            Elements = new ObservableCollection<EditorElement>();
            logic.LoadElements(Elements);

            Messenger.Register<LevelEditorViewModel, string, string>(this, "CannonSelected", (recepient, msg) =>
            {
                OnPropertyChanged(nameof(SelectedCannon));
                (FlipCannon as RelayCommand).NotifyCanExecuteChanged();
            });
        }
    }
}
