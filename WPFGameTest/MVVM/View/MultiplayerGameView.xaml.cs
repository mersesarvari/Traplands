using System;
using System.Collections.Generic;
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

namespace Game.MVVM.View
{
    /// <summary>
    /// Interaction logic for MultiplayerGameView.xaml
    /// </summary>
    public partial class MultiplayerGameView : UserControl
    {
        public MultiplayerGameView()
        {
            InitializeComponent();

            MainWindow.renderer = Renderer;
            Renderer.SetupModel(MainWindow.game);

            MainWindow.SetupCamera(MainCamera);
        }
    }
}
