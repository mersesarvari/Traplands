using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Core;
using Game.MVVM.Stores;
using Game.Models;
using Newtonsoft.Json;
using Game.Logic;

namespace Game.MVVM.ViewModel
{    
    public class MainWindowViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;            
        }
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
