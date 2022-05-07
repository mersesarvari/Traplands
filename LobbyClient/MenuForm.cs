using System.Net.Sockets;
using Client.Models;
using Newtonsoft.Json;

namespace Client
{
    public partial class MenuForm : Form
    {
        public bool up, down, left, right=false;

        public int[] currentpos = new int[2] { 22, 22 };
        GameTimer timer = new GameTimer();
        public MenuForm()
        {
            InitializeComponent();
            groupBox4.Enabled = false;
            groupBox6.Enabled = false;
            Locals.client = new Client();
            Locals.user = new User();
            Locals.user.Username = "PLAYER";
            Locals.client.connectedEvent += UserConnected;
            Locals.client.userDisconnectedEvent += UserDisconnected;
            Locals.client.userJoinedLobbyEvent += UserJoinedLobbyResponse;
            Locals.client.userMovedEvent += MoveByServer;
            GameTimer.tickEvent += ShowTimerTick;
            label2.Text = $"{Locals.user.Username}";
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;

        }


        #region Button management
        //Connect
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect"/*&& textBox1.Text.Trim() != ""*/)
            {
                try
                {
                    if (textBox1.Text.Trim() != "")
                    {
                        Locals.user.Username = textBox1.Text;
                    }                    
                    label2.Text = $"{Locals.user.Username}";
                    Locals.client.ConnectToServer(Locals.user.Username);
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("[Error]  You have to set a username");
            }
        }
        //Join a game
        private void button2_Click(object sender, EventArgs e)
        {
            if (Locals.user.Username != null&& Locals.user.Id!=null)
            {
                Locals.client.SendCommandToServer("JOINLOBBY",  Locals.user.Id, codeBox.Text.Trim(), GameTimer.Tick);
            }
            else
            {
                MessageBox.Show("[Error] You are not connected");
            }
        }

        //Create a game
        private void button3_Click(object sender, EventArgs e)
        {
            if (Locals.user.Username != null)
            {
                Locals.client.SendCommandToServer("CREATELOBBY", Locals.user.Id, "NULL", GameTimer.Tick);
                Locals.client.SendCommandToServer("JOINLOBBY", Locals.user.Id, Locals.user.Id, GameTimer.Tick);
            }
            else
            {
                MessageBox.Show("[Error] You have to set your username");
            }
        }
        #endregion
        public void UserJoinedLobbyResponse()
        {
            //This method is handling the JoinResponse from the server
            var msg = Locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/')&& msg.Split('/')[0] == "JOINLOBBY")
            {                
                var status = msg.Split('/')[1];

                
                if (status != "ERROR" && status!="Success")
                {
                    Locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                    ;
                    if (Locals.lobby !=null)
                    {
                        //LobbyForm lobbyForm = new LobbyForm();
                        //lobbyForm.RefreshListBox(Locals.lobby);
                        //lobbyForm.ShowDialog();
                        SetupLobby();

                    }
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }

        private void UserDisconnected()
        {
            var uid = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show("User disconnected");
        }

        private void MessageRecieved()
        {
            var msg = Locals.client.PacketReader.ReadMessage();
            MessageBox.Show($"[Message] Recieved message: {msg}");
        }

        private void UserConnected()
        {
            Locals.user.Username= Locals.client.PacketReader.ReadMessage();
            Locals.user.Id = Locals.client.PacketReader.ReadMessage();
            //Setting up the timer <==> Sync with the server
            GameTimer.Tick = int.Parse(Locals.client.PacketReader.ReadMessage());
            timer.Start(500);

            button4.BackColor = Color.Green;
            button4.Invoke((MethodInvoker)(() => button4.Text = "Online"));
            textBox3.Invoke((MethodInvoker)(() => textBox3.Text = Locals.user.Id));
            
            //MessageBox.Show($"[Message] User Added: {Username} - Id: {id}");
        }

        public void SetupLobby()
        {
            groupBox4.Invoke((MethodInvoker)(() => groupBox4.Enabled = true));
            groupBox4.Invoke((MethodInvoker)(() => lobby_username_label.Text = Locals.user.Username));
            groupBox4.Invoke((MethodInvoker)(() => lobby_id_label.Text = Locals.user.Id));
            groupBox4.Invoke((MethodInvoker)(() => lobby_id_box.Text = Locals.lobby.LobbyId));
        }

        private void lobby_start_button_Click(object sender, EventArgs e)
        {
            ;
            groupBox4.Invoke((MethodInvoker)(() => groupBox6.Enabled = true));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (up)
            {
                
                SendingMoveCommandToServer(Keys.W);
                MoveLocal(Keys.W);
            }
            if (left)
            {
                
                SendingMoveCommandToServer(Keys.A);
                MoveLocal(Keys.A);
            }
            if (down)
            {
                
                SendingMoveCommandToServer(Keys.S);
                MoveLocal(Keys.S);
            }
            if (right)
            {
                
                SendingMoveCommandToServer(Keys.D);
                MoveLocal(Keys.D);
            }
        }
        #region ButtonManagement
        private void game_up_button_MouseDown(object sender, MouseEventArgs e)
        {
            up = true;
        }

        private void game_up_button_MouseUp(object sender, MouseEventArgs e)
        {
            up = false;
        }
        private void game_right_button_MouseDown(object sender, MouseEventArgs e)
        {
            right = true;
        }

        private void game_right_button_MouseUp(object sender, MouseEventArgs e)
        {
            right=false;
        }

        private void game_down_button_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
        }

        private void game_down_button_MouseUp(object sender, MouseEventArgs e)
        {
            down=false;
        }

        private void game_left_button_MouseDown(object sender, MouseEventArgs e)
        {
            left = true;
        }

        private void game_left_button_MouseUp(object sender, MouseEventArgs e)
        {
            left = false;
        }

        #endregion

        public void SendingMoveCommandToServer(Keys key)
        {
            //OPCODE is 9 for commands
            Locals.client.SendCommandToServer("MOVE", Locals.user.Id, key.ToString(),GameTimer.Tick);
        }
        public void MoveByServer()
        {
            var msg = Locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "MOVE")
            {
                var status = msg.Split('/')[1];
                if (status != "ERROR" && status != "Success")
                {
                    MovementPackage mp = JsonConvert.DeserializeObject<MovementPackage>(status);
                    if (Locals.lobby != null && Client.MovementHistory!=null)
                    {
                        List<MovementPackage> localhistory = Client.MovementHistory;
                        foreach (var item in localhistory)
                        {
                            if (item.Timestamp == mp.Timestamp && item.Vertical == mp.Vertical && item.Horizontal == mp.Horizontal)
                            {
                                Client.MovementHistory.Remove(item);
                                break;
                            }
                            else if(item.Timestamp == mp.Timestamp && (item.Vertical != mp.Vertical || item.Horizontal != mp.Horizontal))
                            {
                                Invoke((MethodInvoker)(() => game_item.Left = mp.Horizontal));
                                Invoke((MethodInvoker)(() => game_item.Top = mp.Vertical));
                                break;
                            }
                        }
                        Client.MovementHistory = localhistory;
                        //MessageBox.Show("Position changed by the server");
                    }
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
            
        }

        public void MoveLocal(Keys direction)
        {
            int[] current = new int[2] { game_item.Left, game_item.Top };
            switch (direction)
            {
                case Keys.W:
                    current[1] -= 4;
                    break;
                case Keys.S:
                    current[1] += 4;
                    break;
                case Keys.A:
                    current[0] -= 4;
                    break;
                case Keys.D:
                    current[0] += 4;
                    break;
                default:
                    break;
            }
            Client.MovementHistory.Add(new MovementPackage(GameTimer.Tick, current[0], current[1]));
            this.Invoke((MethodInvoker)(() => game_item.Left = current[0]));
            this.Invoke((MethodInvoker)(() => game_item.Top = current[1]));
            Invoke((MethodInvoker)(() => label10.Text = $"Current object coords: [{game_item.Left}, {game_item.Top}"));
        }

        public void codetizenhetiscalled()
        {
            MessageBox.Show("Code 17 is Called");
        }

        public void ShowTimerTick()
        {
            Invoke((MethodInvoker)(() => label9.Text = "Current Tick: "+GameTimer.Tick.ToString()));
        }
    }
}