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
using WPFGameTest.Renderer;

namespace WPFGameTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        GameState currentState;
        public static double _Width;
        public static double _Height;

        public MainWindow()
        {
            InitializeComponent();
            _Width = this.Width;
            //Renderer.InvalidateVisual();

            Resource.AddImage("MainMenu_Bg", "menu_bg.jpg");
            Resource.AddImage("Game_Bg", "background.png");
            Resource.AddImage("Player", "player.png");
            Resource.AddImage("Coin", "coin.png");
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
            



            // Background music
            AudioManager.Init();

            // Setting up the initial game state
            currentState = new MainMenu(GameWindow);

            // Game loop
            CompositionTargetEx.Rendering += MainLoop;
        }

        public void MainLoop(object sender, RenderingEventArgs e)
        {
            currentState.Update();

            if (currentState.NeedChange)
            {
                ChangeState();
            }

            Time.Tick();
        }

        public void ChangeState()
        {
            switch (currentState.State)
            {
                case GameStates.MainMenu:
                    currentState = new MainMenu(GameWindow);
                    break;
                case GameStates.ClientTest:
                    currentState = new ClientTest(GameWindow);
                    break;
                case GameStates.Play:
                    currentState = new PlayState(GameWindow);
                    break;
                case GameStates.Editor:
                    currentState = new EditorState(GameWindow);
                    break;
                case GameStates.Lobby:
                    currentState = new LobbyState(GameWindow);
                    break;
                case GameStates.Multiplayer:
                    currentState = new MultiplayerState(GameWindow);
                    break;
                case GameStates.Exit:
                    Environment.Exit(0);
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat) Input.pressedKey = e.Key;
            Input.heldKeys[(int)e.Key] = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            Input.releasedKey = e.Key;
            Input.heldKeys[(int)e.Key] = false;
        }

        private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = (int)e.NewSize.Width;
            this.Height = (int)e.NewSize.Height;
        }
    }
}
