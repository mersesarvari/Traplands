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
using Game.Helpers;
using Game.Logic;
using Game.Renderer;
using Game.MVVM.ViewModel;
using Game.Models;
using Newtonsoft.Json;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static double _Width;
        public static double _Height;

        public static IGameModel game = null;
        public static RendererBase renderer = null;
        GameTimer timer= new GameTimer();


        public MainWindow()
        {
            
            InitializeComponent();
            
            _Width = this.Width;
            _Height = this.Height;
            #region commented

            #endregion

            // Game loop
            CompositionTargetEx.Rendering += MainLoop;
        }

        public void MainLoop(object sender, RenderingEventArgs e)
        {
            if (game != null && renderer != null)
            {
                game.ProcessInput(); // Get input

                game.Update(Time.DeltaTime); // Update game state and objects

                renderer.InvalidateVisual(); // Render game
            }
            Time.Tick();
        }

        public static void SetupCamera(ScrollViewer camera)
        {
            camera.Width = _Width;
            camera.Height = _Height;
            camera.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            camera.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            camera.Background = new ImageBrush(Resource.GetImage("Game_Bg"));

            CameraController.Instance.Init(camera);
        }

        private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _Width = e.NewSize.Width;
            _Height = e.NewSize.Height;

            CameraController.Instance.UpdateCameraView(_Width, _Height);
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat) Input.pressedKey = e.Key;
            Input.heldKeys[(int)e.Key] = true;
        }

        private void GameWindow_KeyUp(object sender, KeyEventArgs e)
        {
            Input.releasedKey = e.Key;
            Input.heldKeys[(int)e.Key] = false;
        }

       
        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Images
            Resource.AddImage("MainMenu_Bg", "menu_bg.jpg");
            Resource.AddImage("Game_Bg", "background.png");
            Resource.AddImage("Player", "player.png");
            Resource.AddImage("Coin", "coin.png");
            Resource.AddImage("Spawn", "wooden_plate.png");
            Resource.AddImage("Spike", "spike.png");
            Resource.AddImage("Finish", "door.png");
            Resource.AddImage("Grass_Top_Center", "grass_topcenter.png");
            Resource.AddImage("Grass_Top_Right", "grass_topright.png");
            Resource.AddImage("Grass_Top_Left", "grass_topleft.png");
            Resource.AddImage("Grass_Under", "grass_under.png");
            Resource.AddImage("Grass_Mid_Left", "grass_midleft.png");
            Resource.AddImage("Grass_Mid_Right", "grass_midright.png");
            Resource.AddImage("Grass_Bottom_Center", "grass_bottomcenter.png");
            Resource.AddImage("Grass_Bottom_Right", "grass_bottomright.png");
            Resource.AddImage("Grass_Bottom_Left", "grass_bottomleft.png");
            Resource.AddImage("Grass_Mid_Right_Left", "grass_midrightleft.png");
            Resource.AddImage("Grass_Top_Right_Bottom", "grass_toprightbottom.png");
            Resource.AddImage("Grass_Top_Left_Bottom", "grass_topleftbottom.png");

            // Spritesheets
            Resource.AddImage("player_idle", "player_idle.png");
            Resource.AddImage("player_run", "player_run.png");
            Resource.AddImage("player_jumpup", "player_jumpup.png");
            Resource.AddImage("player_jumpdown", "player_jumpdown.png");
            Resource.AddImage("player_attack", "player_attack.png");
            Resource.AddImage("player_death", "player_death.png");

            Resource.AddSpriteSheet("player_idle","player_idle.png", 4, 48, 48);
            Resource.AddSpriteSheet("player_run", "player_run.png", 6, 48, 48);
            Resource.AddSpriteSheet("player_jumpup", "player_jumpup.png", 3, 48, 48);
            Resource.AddSpriteSheet("player_jumpdown", "player_jumpdown.png", 3, 48, 48);
            Resource.AddSpriteSheet("player_attack", "player_attack.png", 4, 48, 48);
            Resource.AddSpriteSheet("player_dash", "player_run.png", 2, 48, 48);
            Resource.AddSpriteSheet("player_death", "player_death.png", 8, 48, 48);

            LevelManager.LoadLevels();
        }
    }
}
