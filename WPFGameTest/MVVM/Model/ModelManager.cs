//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WPFGameTest.Core;
//using WPFGameTest.MVVM.ViewModel;

//namespace WPFGameTest.MVVM.Model
//{
//    internal class ModelManager:ObservableObject
//    {
//        public  LobbyViewModel LobbyMV { get; set; }
//        public  MainmenuViewModel MainMenuVM { get; set; }
//        public  MultiplayerGameMenuViewModel MultiplayerGameMenuVM { get; set; }
//        public  MultiplayerGameViewModel MultiplayerGameVM { get; set; }
//        public  SingleplayerGameViewModel SingleplayerGameVM { get; set; }

//        private static object _currentView;

//        public object CurrentView
//        {
//            get { return _currentView; }
//            set
//            {
//                _currentView = value;
//                OnPropertyChanged();
//            }
//        }

//        public ModelManager()
//        {
//            LobbyMV = new LobbyViewModel();
//            MainMenuVM = new MainmenuViewModel();
//            MultiplayerGameMenuVM = new MultiplayerGameMenuViewModel();
//            MultiplayerGameVM = new MultiplayerGameViewModel();
//            SingleplayerGameVM = new SingleplayerGameViewModel();
//        }
//    }
//}
