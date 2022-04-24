using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFGameTest.Helpers;
using WPFGameTest.Logic;

namespace WPFGameTest.MVVM.View
{
    /// <summary>
    /// Interaction logic for SingleplayerGameView.xaml
    /// </summary>
    public partial class SingleplayerGameView : UserControl
    {
        public SingleplayerGameView()
        {
            InitializeComponent();

            MainWindow.game = new Singleplayer();
            MainWindow.renderer = Renderer;
            Renderer.SetupModel(MainWindow.game);

            MainWindow.SetupCamera(MainCamera);
        }
    }
}
