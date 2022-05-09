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

        public ICommand SaveLevel { get; set; }

        public void SetupLogic(FrameworkElement renderTarget, ScrollViewer camera)
        {
            logic.Init(renderTarget, camera);
        }

        public IGameModel GetLogic()
        {
            return logic;
        }

        public static bool IsInDesignerMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop,
                typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public ICommand NavigateMainMenuCommand { get; }
        public ICommand SaveAndExitCommand { get; }

        public LevelEditorViewModel(INavigationService mainMenuNavigationService)
        {
            this.logic = new LevelEditor();
            MainWindow.game = logic;
            SaveLevel = new RelayCommand(() => { logic.SaveLevel(LevelName); mainMenuNavigationService.Navigate(); }, () => !string.IsNullOrWhiteSpace(LevelName));

            Elements = new ObservableCollection<EditorElement>();
            logic.LoadElements(Elements);

            NavigateMainMenuCommand = new NavigateCommand(mainMenuNavigationService);

            SaveAndExitCommand = new SaveAndExitCommand(mainMenuNavigationService);
        }
    }
}
