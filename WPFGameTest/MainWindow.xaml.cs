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
using Client.Models;
using Game.Models;
using Newtonsoft.Json;
using Client;

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
            /*
            Locals.client = new Client.Client();            
            Locals.user = new User();
            */
            Locals.client.connectedEvent += UserConnected;
            Locals.client.userDisconnectedEvent += UserDisconnected;
            Locals.client.userJoinedLobbyEvent += UserJoinedLobbyResponse;
            //Locals.client.ConnectToServer("eventtester");
            _Width = this.Width;
            _Height = this.Height;
            #region commented
            /*
            Resource.AddImage("MainMenu_Bg", "menu_bg.jpg");
            Resource.AddImage("Game_Bg", "background.png");
            Resource.AddImage("Player", "player.png");
            Resource.AddImage("Coin", "coin.png");
            Resource.AddImage("Spawn", "orbs_hud.png");
            Resource.AddImage("Spike", "spike.png");
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
            */
            #endregion
            LevelManager.LoadLevels();

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


        //Server-Client Methods
        #region Server-Client methods
        //Nem hívódik meg valamiért
        private void UserConnected()
        {

            Locals.user.Username = Locals.client.PacketReader.ReadMessage();
            Locals.user.Id = Locals.client.PacketReader.ReadMessage();
            //Setting up the timer <==> Sync with the server
            GameTimer.Tick = int.Parse(Locals.client.PacketReader.ReadMessage());
            var u = Locals.user;
            timer.Start(500);
        }
        private void UserDisconnected()
        {
            var uid = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show("User disconnected");
        }

        public void UserJoinedLobbyResponse()
        {
            //This method is handling the JoinResponse from the server
            var msg = Locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];


                if (status != "ERROR" && status != "Success")
                {
                    Locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }

        #endregion
    }
}
