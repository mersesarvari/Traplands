using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using WPFGameTest.Models;
using WPFGameTest.Helpers;
using WPFGameTest.Renderer;

namespace WPFGameTest.Logic
{
    public enum GameStates
    {
        None = 0,
        MainMenu,
        ClientTest,
        Play,
        Editor,
        Multiplayer,
        Lobby,
        Exit
    }

    public abstract class GameState
    {
        public event Action OnStateExit;
        public bool NeedChange { get; private set; }
        public GameStates State { get; private set; }

        protected Canvas canvas;
        protected ScrollViewer camera;
        protected Grid grid;
        protected MainWindow mainWindow;

        public GameState(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            NeedChange = false;

            // Add grid to window
            grid = new Grid();
            mainWindow.Content = grid;

            // Canvas setup
            canvas = new Canvas();
            //Removing this because we dont need that canvas size in everywhere
            /*
            canvas.Width = ObjectData.BLOCK_WIDTH * 100;
            canvas.Height = ObjectData.BLOCK_HEIGHT * 100;
            */

            // Camera (ScrollViewer) setup
            camera = new ScrollViewer();
            camera.Width = mainWindow.Width;
            camera.Height = mainWindow.Width;
            camera.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            camera.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            this.mainWindow.SizeChanged += (s, e) =>
            {
                camera.Width = e.NewSize.Width;
                camera.Height = e.NewSize.Height;
            };

            camera.Content = canvas;
            grid.Children.Add(camera);
            CameraController.Instance.Init(camera);

        }

        public virtual void Update() { }

        protected void ChangeState(GameStates newState)
        {
            OnStateExit?.Invoke();
            State = newState;
            NeedChange = true;
        }
    }
    public class MainMenu : GameState
    {
        private float transitionTime = 1f;
        private float currentTime = 0;
        private Rectangle blackScreen;
        private bool startTransition = false;
        private GameStates newState;
        AudioClip selectAudio;
        public MainMenu(MainWindow mainWindow) : base(mainWindow)
        {
            camera.Background = new ImageBrush(Resource.GetImage("MainMenu_Bg"));
            AudioManager.SetBackgroundMusic("27-Dark Fantasy Studio- Silent walk.wav");

            selectAudio = new AudioClip("ui_reward.wav");
            selectAudio.Volume = 0.1;

            RectButton clientButton = new RectButton(canvas, 200, 50, new SolidColorBrush(Colors.Gray));
            clientButton.SetText("Client Test");
            clientButton.SetTextSize(26);
            clientButton.SetPosition(412, 200);
            clientButton.AddOnClickEvent((s, e) => { selectAudio.Play(); StartTransition(GameStates.ClientTest); });
            clientButton.AddOnMouseEnterEvent((s, e) => { clientButton.Border.Fill = new SolidColorBrush(Colors.MediumAquamarine); });
            clientButton.AddOnMouseLeaveEvent((s, e) => { clientButton.Border.Fill = new SolidColorBrush(Colors.Gray); });

            RectButton playButton = new RectButton(canvas, 200, 50, new SolidColorBrush(Colors.Gray));
            playButton.SetText("Singleplayer");
            playButton.SetTextSize(26);
            playButton.SetPosition(412, 300);
            playButton.AddOnClickEvent((s, e) => { selectAudio.Play(); StartTransition(GameStates.Play); });
            playButton.AddOnMouseEnterEvent((s, e) => { playButton.Border.Fill = new SolidColorBrush(Colors.MediumAquamarine); });
            playButton.AddOnMouseLeaveEvent((s, e) => { playButton.Border.Fill = new SolidColorBrush(Colors.Gray); });
            
            RectButton multiplayerButton = new RectButton(canvas, 200, 50, new SolidColorBrush(Colors.Gray));
            multiplayerButton.SetText("Multiplayer");
            multiplayerButton.SetTextSize(26);
            multiplayerButton.SetPosition(412, 400);
            multiplayerButton.AddOnClickEvent((s, e) => { selectAudio.Play(); StartTransition(GameStates.Multiplayer); });
            multiplayerButton.AddOnMouseEnterEvent((s, e) => { multiplayerButton.Border.Fill = new SolidColorBrush(Colors.MediumAquamarine); });
            multiplayerButton.AddOnMouseLeaveEvent((s, e) => { multiplayerButton.Border.Fill = new SolidColorBrush(Colors.Gray); });
            
            RectButton editorButton = new RectButton(canvas, 200, 50, new SolidColorBrush(Colors.Gray));
            editorButton.SetText("Level Editor");
            editorButton.SetTextSize(26);
            editorButton.SetPosition(412, 500);
            editorButton.AddOnClickEvent((s, e) => { selectAudio.Play(); StartTransition(GameStates.Editor); });
            editorButton.AddOnMouseEnterEvent((s, e) => { editorButton.Border.Fill = new SolidColorBrush(Colors.MediumAquamarine); });
            editorButton.AddOnMouseLeaveEvent((s, e) => { editorButton.Border.Fill = new SolidColorBrush(Colors.Gray); });

            RectButton exitButton = new RectButton(canvas, 200, 50, new SolidColorBrush(Colors.Gray));
            exitButton.SetText("Exit");
            exitButton.SetTextSize(26);
            exitButton.SetPosition(412, 600);
            exitButton.AddOnClickEvent((s, e) => { ChangeState(GameStates.Exit); });
            exitButton.AddOnMouseEnterEvent((s, e) => { exitButton.Border.Fill = new SolidColorBrush(Colors.IndianRed); });
            exitButton.AddOnMouseLeaveEvent((s, e) => { exitButton.Border.Fill = new SolidColorBrush(Colors.Gray); });

            blackScreen = new Rectangle();
            blackScreen.Width = canvas.Width;
            blackScreen.Height = canvas.Height;
        }
        private void StartTransition(GameStates newState)
        {
            this.newState = newState;
            blackScreen.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            canvas.Children.Add(blackScreen);
            startTransition = true;
        }
        private void Transition()
        {
            float ratio = currentTime / transitionTime;
            byte a = (byte)(255 * ratio);

            double volume = AudioManager.defaultVolume - ratio * AudioManager.defaultVolume;

            if (currentTime < transitionTime)
            {
                currentTime += Time.DeltaTime;
                blackScreen.Fill = new SolidColorBrush(Color.FromArgb(a, 0, 0, 0));
                AudioManager.backgroundMusic.Volume = volume;
            }
            else
            {
                ChangeState(newState);
            }
        }
        public override void Update()
        {
            if (Input.GetKeyPressed(Key.Escape))
            {
                ChangeState(GameStates.Exit);
            }

            if (startTransition)
            {
                Transition();
            }
        }
    }
    public class ClientTest : GameState
    {
        private Player player;
        private Player player2;

        public ClientTest(MainWindow mainWindow) : base(mainWindow)
        {
            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");
            camera.Background = new ImageBrush(Resource.GetImage("Game_Bg"));

            List<StaticObject> solids = new List<StaticObject>();

            if (LevelManager.Load("test", canvas))
            {
                solids = LevelManager.Solids;
            }
            else
            {
                StaticObject s1 = new StaticObject(new Vector2(300, 950), new Vector2(50, 50));
                StaticObject s2 = new StaticObject(new Vector2(0, 1000), new Vector2(5000, 50));
                solids.Add(s1);
                solids.Add(s2);
                canvas.Children.Add(s1.Element);
                canvas.Children.Add(s2.Element);
            }

            player = new Player("001", "Player1",new Vector2(100, 0), new Vector2(44, 44), 8);
            player.SetDefaultSprite(Resource.GetImage("Player"));
            player.SetSolids(solids);
            canvas.Children.Add(player.Element);

            player2 = new Player("002", "Player2",new Vector2(100, 0), new Vector2(44, 44), 8);
            player2.SetDefaultSprite(Resource.GetImage("Grass_Under"));
            player2.Element.Fill.Opacity = 0.2;
            canvas.Children.Add(player2.Element);

            TestClient.Instance.Init(player);
            TestServer.Instance.Init(player2);
        }

        public override void Update()
        {
            //player2.Element.Fill.Opacity = 0.2;

            if (Input.GetKeyPressed(Key.Escape))
            {
                ChangeState(GameStates.MainMenu);
            }

            TestClient.Instance.Update();
            TestServer.Instance.Update();

            CameraController.Instance.UpdateCamera(player.Transform.Position);
        }
    }
    public class PlayState : GameState
    {
        private Player player;
        private Stopwatch timer;

        public PlayState(MainWindow mainWindow) : base(mainWindow)
        {
            /*
            * 
            * TODO:
            * Combine blocks to have one big hitbox with multiple rectangles (images)
            * Implement a Spatial Hash Grid!!!!!!!!!!!!!!!!!!!!
            * Object pooling on Canvas Rectangle Elements!!!!!!!!!!!!!!!!!
            * Store input in order to replay player movement
            * 
            */
            //Setting back Canvas size to normal
            canvas.Width = ObjectData.BLOCK_WIDTH * 100;
            canvas.Height = ObjectData.BLOCK_HEIGHT * 100;
            timer = new Stopwatch();
            timer.Start();

            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");
            camera.Background = new ImageBrush(Resource.GetImage("Game_Bg"));

            player = new Player("001", "Player1",new Vector2(100, 0), new Vector2(44, 44), 8);
            player.SetDefaultSprite(Resource.GetImage("Player"));


            canvas.Children.Add(player.Element);

            if (LevelManager.Load("test", canvas))
            {
                player.SetSolids(LevelManager.Solids);
            }
            else
            {
                List<StaticObject> solids = new List<StaticObject>();

                StaticObject s1 = new StaticObject(new Vector2(300, 950), new Vector2(50, 50));
                StaticObject s2 = new StaticObject(new Vector2(0, 1000), new Vector2(5000, 50));
                solids.Add(s1);
                solids.Add(s2);
                canvas.Children.Add(s1.Element);
                canvas.Children.Add(s2.Element);
                player.SetSolids(solids);
            }
        }

        private void SimulateInput(Key key)
        {
            Input.pressedKey = key;
        }

        public override void Update()
        {
            if (Input.GetKeyPressed(Key.Escape))
            {
                ChangeState(GameStates.MainMenu);
            }

            if (Input.GetKeyPressed(Key.P)) // Show fps(?)
            {
                Trace.WriteLine(1 / Time.DeltaTime);
            }

            //if (timer.Elapsed.TotalSeconds >= 5)
            //{
            //    SimulateInput(Key.W);
            //}

            player.ProcessInput();
            player.Update(Time.DeltaTime);

            CameraController.Instance.UpdateCamera(player.Transform.Position); // Apply modifications to the "camera" like following the player or shaking
        }
    }
    public class EditorState : GameState
    {
        private struct Coordinate
        {
            public int X;
            public int Y;
            public Coordinate(int x = 0, int y = 0)
            {
                X = x;
                Y = y;
            }
        }
        #region Variables
        // Visuals
        private LevelGrid levelGrid;
        private Rectangle previewRect;
        private ImageBrush previewImage;
        private int firstRectIndex;

        // Camera
        public double Zoom = 0.5;

        private double mouseX = 0;
        private double mouseY = 0;
        private Vector2f cameraPos = new Vector2f(0, 0);
        private Vector2 actualPos = new Vector2();
        private float moveSpeed = 500;

        // Placing tiles
        private Dictionary<Coordinate, Rectangle> rects = new Dictionary<Coordinate, Rectangle>();
        private ObjectType selectedType = ObjectType.Grass_Top_Center;
        private bool isDeleting = false;
        private bool canPlace = false;

        private Vector2[] dirs =
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(-1,0),
            new Vector2(0,-1),
            new Vector2(-1,-1),
            new Vector2(1,-1),
            new Vector2(-1,1)
        };
        #endregion
        public EditorState(MainWindow mainWindow) : base(mainWindow)
        {
            // Initial setup of the state
            camera.Background = new ImageBrush(Resource.GetImage("Game_Bg"));
            AudioManager.SetBackgroundMusic("27-Dark Fantasy Studio- Silent walk.wav");
            levelGrid = new LevelGrid(canvas, 100, 100, ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT);
            firstRectIndex = canvas.Children.Count - 1;

            // Set up editor panel for customizing settings
            Canvas editorPanel = new Canvas();
            editorPanel.HorizontalAlignment = HorizontalAlignment.Right;
            editorPanel.Background = new SolidColorBrush(Colors.Black);
            editorPanel.Width = 250;
            editorPanel.Background.Opacity = 0.25;

            editorPanel.MouseMove += (s, e) => // Prevent user from placing tiles while mouse is over the editor panel
            {
                canPlace = false;
            };

            for (int i = 0; i < 3; i++) // Place 9 "checkboxes" in a 3x3 pattern (not useful atm)
            {
                for (int j = 0; j < 3; j++)
                {
                    RectCheckbox rc = new RectCheckbox(editorPanel, 25, 25, new SolidColorBrush(Colors.LightSkyBlue));
                    rc.SetPosition(i * 26, 200 + j * 26);
                }
            }

            editorPanel.Children.Add(new TextBox
            {
                Width = 150,
                AcceptsReturn = false,
                BorderThickness = new Thickness(0),
                Background =
                new SolidColorBrush(Colors.Black),
                Opacity = 0.5,
                Foreground = new SolidColorBrush(Colors.White)
            });

            grid.Children.Add(editorPanel);

            // Set up mouse hover image (tile or red square when deleting)
            previewImage = new ImageBrush(Resource.GetImage("Grass_Top_Center"));
            previewRect = new Rectangle();
            previewRect.Width = ObjectData.BLOCK_WIDTH;
            previewRect.Height = ObjectData.BLOCK_HEIGHT;
            previewRect.Fill = previewImage;
            previewRect.Opacity = 0.5;
            canvas.Children.Add(previewRect);

            // When moving the mouse in the scrollview update and calculate actual position on the "grid"
            camera.PreviewMouseMove += (s, e) =>
            {
                // Get mouse position relative to canvas (world)
                mouseX = Mouse.GetPosition(canvas).X;
                mouseY = Mouse.GetPosition(canvas).Y;
                // Get the rounded position for the levelGrid-like placement
                actualPos = new Vector2(levelGrid.CellSize.X * (int)(mouseX / levelGrid.CellSize.X), levelGrid.CellSize.Y * (int)(mouseY / levelGrid.CellSize.Y));

                Canvas.SetLeft(previewRect, actualPos.X);
                Canvas.SetTop(previewRect, actualPos.Y);

                canPlace = true;
            };

            camera.PreviewMouseWheel += Camera_MouseWheel;

            // Setting up scrollviewer for zooming (0.5 by default)
            ScaleTransform scale = new ScaleTransform(Zoom, Zoom);
            canvas.LayoutTransform = scale;

            // On exiting this state (not needed at this build, we remake every canvas and scrollviewer anyway)
            OnStateExit += () =>
            {
                scale = new ScaleTransform(1, 1);
                canvas.LayoutTransform = scale;
                camera.PreviewMouseWheel -= Camera_MouseWheel;
            };

            canvas.PreviewMouseDown += (s, e) => { canPlace = true; };
            canvas.PreviewMouseUp += (s, e) => { canPlace = false; };
        }
        

        // Zooming in the scrollviewer with mousewheel
        private void Camera_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Zoom += 0.1;
            }
            else
            {
                Zoom -= 0.1;
            }

            canvas.LayoutTransform = new ScaleTransform(Zoom, Zoom);
            e.Handled = true;
        }

        public override void Update()
        {
            // Bandaid fix to saving and going back to the Main Menu
            if (Input.GetKeyPressed(Key.Escape))
            {
                Level level = new Level(levelGrid.Map);
                LevelManager.Save("test", level, canvas);
                ChangeState(GameStates.MainMenu);
            }

            // Update camera position
            if (Input.GetKey(Key.W))
            {
                cameraPos.Y -= moveSpeed * Time.DeltaTime;
            }
            else if (Input.GetKey(Key.S))
            {
                cameraPos.Y += moveSpeed * Time.DeltaTime;
            }

            if (Input.GetKey(Key.D))
            {
                cameraPos.X += moveSpeed * Time.DeltaTime;
            }
            else if (Input.GetKey(Key.A))
            {
                cameraPos.X -= moveSpeed * Time.DeltaTime;
            }

            // Deleting with LShift
            if (Input.GetKey(Key.LeftShift))
            {
                // Red square as overlapping image instead of a tile
                previewRect.Fill = new SolidColorBrush(Colors.Red);
                isDeleting = true;
            }
            else
            {
                previewRect.Fill = previewImage;
                isDeleting = false;
            }

            if (Input.GetKeyPressed(Key.P))
            {
                //Trace.WriteLine(" camera width: " + camera.Width);
                //Trace.WriteLine(cameraPos.X + " " + cameraPos.Y);
                //Trace.WriteLine(camera.HorizontalOffset + " - " + camera.VerticalOffset);
                Trace.WriteLine(1 / Time.DeltaTime);
            }

            // If the mouse is not over the editor panel and we are pressing the Left button
            if (canPlace && Input.GetMouseButton(Mouse.LeftButton))
            {
                PlaceTile();
            }

            SetCameraInBoundries();
            CameraController.Instance.UpdateEditorCamera(cameraPos);
        }

        private void SetCameraInBoundries()
        {
            // Setting max and minimum zoom
            if (Zoom > 1)
            {
                Zoom = 1;
            }
            else if (Zoom < 0.25)
            {
                Zoom = 0.3;
            }
            // Keeping the camera position in boundaries
            if (cameraPos.X < camera.Width / 2)
            {
                cameraPos.X = (int)(camera.Width / 2);
            }
            else if (cameraPos.X > (int)((canvas.Width - camera.Width / 2) * Zoom))
            {
                cameraPos.X = (int)((canvas.Width - camera.Width / 2) * Zoom);
            }

            if (cameraPos.Y < camera.Height / 2)
            {
                cameraPos.Y = (int)(camera.Height / 2);
            }
            else if (cameraPos.Y > (int)((canvas.Height - camera.Height / 2) * Zoom))
            {
                cameraPos.Y = (int)((canvas.Height - camera.Height / 2) * Zoom);
            }
        }

        private Rectangle Get(Coordinate cord)
        {
            Rectangle rect;
            rects.TryGetValue(cord, out rect);

            return rect;
        }

        private void PlaceTile()
        {
            // Get the matrix position to place the correct value into the levelGrid's Map
            Vector2 matrixPos = new Vector2(actualPos.X / levelGrid.CellSize.X, actualPos.Y / levelGrid.CellSize.Y);

            if (levelGrid.Map[matrixPos.X, matrixPos.Y] != 0) // If the cell is not empty
            {
                if (isDeleting) // Delete
                {
                    // Delete tile and update it's neighbours
                    DeleteTile(matrixPos.X, matrixPos.Y);
                    UpdateTile(matrixPos.X + 1, matrixPos.Y);
                    UpdateTile(matrixPos.X - 1, matrixPos.Y);
                    UpdateTile(matrixPos.X, matrixPos.Y + 1);
                    UpdateTile(matrixPos.X, matrixPos.Y - 1);
                }

                return;
            }

            if (isDeleting)
                return;

            // Place tiles in a 3x3 pattern (changeable in the dirs Vector[])
            foreach (Vector2 dir in dirs)
            {
                // Only place tiles if it's a correct matrix position
                if (matrixPos.X + dir.X >= 0 && 
                    matrixPos.Y + dir.Y >= 0 &&
                    matrixPos.X + dir.X < levelGrid.Map.GetLength(0) && 
                    matrixPos.Y + dir.Y < levelGrid.Map.GetLength(1)) 
                {
                    if (levelGrid.Map[matrixPos.X + dir.X, matrixPos.Y + dir.Y] != 0) // If any of the desired cells is not empty just contiune to the next
                    {
                        continue;
                    }

                    Rectangle rect = new Rectangle();
                    rect.Width = ObjectData.BLOCK_WIDTH;
                    rect.Height = ObjectData.BLOCK_HEIGHT;

                    levelGrid.Map[matrixPos.X + dir.X, matrixPos.Y + dir.Y] = (int)selectedType;
                    rects.Add(new Coordinate(matrixPos.X + dir.X, matrixPos.Y + dir.Y), rect);

                    // Update tile and it's neighbours
                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y);
                    UpdateTile(matrixPos.X + dir.X + 1, matrixPos.Y + dir.Y);
                    UpdateTile(matrixPos.X + dir.X - 1, matrixPos.Y + dir.Y);
                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y + 1);
                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y - 1);

                    Canvas.SetLeft(rect, actualPos.X + ObjectData.BLOCK_WIDTH * dir.X);
                    Canvas.SetTop(rect, actualPos.Y + ObjectData.BLOCK_WIDTH * dir.Y);

                    // Insert rectangle after with the right index behind the preview
                    // to make sure we see the preview after the cell is occupied
                    canvas.Children.Insert(firstRectIndex, rect);
                }
            }
        }

        private void DeleteTile(int x, int y)
        {
            Coordinate cord = new Coordinate(x, y);
            Rectangle rectToDelete = Get(cord);
            rects.Remove(cord);
            levelGrid.Map[x, y] = 0;
            if (rectToDelete != null)
            {
                canvas.Children.Remove(rectToDelete);
            }
        }

        private void UpdateTile(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return;
            }

            Rectangle rect = Get(new Coordinate(x, y));

            if (rect is null)
                return;

            bool right = false, left = false, top = false, bottom = false;
            ObjectType type = GetCorrectTile(right, left, top, bottom);

            if (x < 1 || y < 1)
            {
                rect.Fill = new ImageBrush(Resource.GetImage(type.ToString()));
                return;
            }

            if (levelGrid.Map[x + 1, y] > (int)ObjectType.Grass_First &&
                levelGrid.Map[x + 1, y] < (int)ObjectType.Grass_Last)
            {
                right = true;
            }
            if (levelGrid.Map[x - 1, y] > (int)ObjectType.Grass_First &&
                levelGrid.Map[x - 1, y] < (int)ObjectType.Grass_Last)
            {
                left = true;
            }
            if (levelGrid.Map[x, y - 1] > (int)ObjectType.Grass_First &&
                levelGrid.Map[x, y - 1] < (int)ObjectType.Grass_Last)
            {
                top = true;
            }
            if (levelGrid.Map[x, y + 1] > (int)ObjectType.Grass_First &&
                levelGrid.Map[x, y + 1] < (int)ObjectType.Grass_Last)
            {
                bottom = true;
            }

            type = GetCorrectTile(right, left, top, bottom);
            rect.Fill = new ImageBrush(Resource.GetImage(type.ToString()));
            levelGrid.Map[x, y] = (int)type;
        }

        private ObjectType GetCorrectTile(bool right, bool left, bool top, bool bottom)
        {
            ObjectType obj = ObjectType.Grass_Top_Center;

            if (right && !left && !top && !bottom) // Right
            {
                obj = ObjectType.Grass_Top_Left_Bottom;
            }
            else if (right && left && !top && !bottom) // Right + Left
            {
                obj = ObjectType.Grass_Top_Center;
            }
            else if (right && !left && top && !bottom) // Right + Top
            {
                obj = ObjectType.Grass_Bottom_Left;
            }
            else if (right && !left && !top && bottom) // Right + Bottom
            {
                obj = ObjectType.Grass_Top_Left;
            }
            else if (right && !left && top && bottom) // Right + Bottom + Top
            {
                obj = ObjectType.Grass_Mid_Left;
            }
            else if (right && left && !top && bottom) // Right + Left + Bottom
            {
                obj = ObjectType.Grass_Top_Center;
            }
            else if (right && left && top && !bottom) // Right + Left + Top
            {
                obj = ObjectType.Grass_Bottom_Center;
            }
            else if (left && !right && !top && !bottom) // Left
            {
                obj = ObjectType.Grass_Top_Right_Bottom;
            }
            else if (left && !right && top && !bottom) // Left + Top
            {
                obj = ObjectType.Grass_Bottom_Right;
            }
            else if (left && !right && !top && bottom) // Left + Bottom
            {
                obj = ObjectType.Grass_Top_Right;
            }
            else if (left && !right && top && bottom) // Left + Top + Bottom
            {
                obj = ObjectType.Grass_Mid_Right;
            }
            else if (top && !left && !right && !bottom) // Top
            {
                obj = ObjectType.Grass_Bottom_Center;
            }
            else if (top && !left && !right && bottom) // Top + Bottom
            {
                obj = ObjectType.Grass_Mid_Right_Left;
            }
            else if (top && left && right && bottom) // Right + Left + Top + Bottom
            {
                obj = ObjectType.Grass_Under;
            }

            return obj;
        }
    }
    //TODO: Summary
    /// <summary>
    /// TODO:
    /// This function will inicialize the lobby with the player list, start button and map selector
    /// </summary>
    public class LobbyState : GameState
    {

        public LobbyState(MainWindow mainWindow) : base(mainWindow)
        {

        }
    }
    //TODO: Summary
    /// <summary>
    /// TODO: Multiplayr state
    /// This function will inicialize the elements for the Multiplayer Game creation of Lobby Join Panel
    /// </summary>
    public class MultiplayerState : GameState
    {
        private float transitionTime = 1f;
        private float currentTime = 0;
        private Rectangle blackScreen;
        private bool startTransition = false;
        private GameStates newState;
        AudioClip selectAudio;

        public MultiplayerState(MainWindow mainWindow) : base(mainWindow)
        {
            camera.Background = new ImageBrush(Resource.GetImage("MainMenu_Bg"));
            AudioManager.SetBackgroundMusic("27-Dark Fantasy Studio- Silent walk.wav");

            selectAudio = new AudioClip("ui_reward.wav");
            selectAudio.Volume = 0.1;


            //Adding elements Dinamically
            #region Elements
            
            TextBox lobbycodebox = new TextBox();
            lobbycodebox.Width = 100;
            lobbycodebox.Height = 30;

            //Set canvas Size to window Size Need to Set this Back on Game
            canvas.Width = mainWindow.Width;
            canvas.Height = mainWindow.Height;
            double left = (mainWindow.Width - lobbycodebox.Width) / 2;
            double top = (mainWindow.Height - lobbycodebox.Height) / 2;
            Canvas.SetLeft(lobbycodebox, left);
            Canvas.SetTop(lobbycodebox, top);
            canvas.Children.Add(lobbycodebox);

            #endregion
            /*
            blackScreen = new Rectangle();
            blackScreen.Width = canvas.Width;
            blackScreen.Height = canvas.Height;
            */


        }

        private void StartTransition(GameStates newState)
        {
            this.newState = newState;
            blackScreen.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            canvas.Children.Add(blackScreen);
            startTransition = true;
        }
        private void Transition()
        {
            float ratio = currentTime / transitionTime;
            byte a = (byte)(255 * ratio);

            double volume = AudioManager.defaultVolume - ratio * AudioManager.defaultVolume;

            if (currentTime < transitionTime)
            {
                currentTime += Time.DeltaTime;
                blackScreen.Fill = new SolidColorBrush(Color.FromArgb(a, 0, 0, 0));
                AudioManager.backgroundMusic.Volume = volume;
            }
            else
            {
                ChangeState(newState);
            }
        }
        public override void Update()
        {
            if (Input.GetKeyPressed(Key.Escape))
            {
                ChangeState(GameStates.Exit);
            }

            if (startTransition)
            {
                Transition();
            }
        }
    }
}

