using System;
using System.Collections.Generic;
using System.IO;
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

namespace Game.MVVM.View
{
    /// <summary>
    /// Interaction logic for LevelManagerView.xaml
    /// </summary>
    public partial class LevelManagerView : UserControl
    {
        public LevelManagerView()
        {
            InitializeComponent();
            ImageBrush img = new ImageBrush(new BitmapImage(new Uri(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Graphics\", "menu_bg.jpg"), UriKind.Relative)));
            MainGrid.Background = img;
        }
    }
}
