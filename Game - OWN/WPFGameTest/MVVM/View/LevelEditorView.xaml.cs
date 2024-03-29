﻿using Game.Logic;
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
using System.Windows.Shapes;

namespace Game.MVVM.View
{
    /// <summary>
    /// Interaction logic for LevelEditorView.xaml
    /// </summary>
    public partial class LevelEditorView : UserControl
    {
        public LevelEditorView()
        {
            InitializeComponent();

            (MainWindow.game as ILevelEditor).Init(Renderer, MainCamera);
            MainWindow.renderer = Renderer;
            Renderer.SetupModel(MainWindow.game);

            MainWindow.SetupCamera(MainCamera);
        }
    }
}
