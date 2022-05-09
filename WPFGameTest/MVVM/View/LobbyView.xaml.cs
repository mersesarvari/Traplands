using Game.Models;
using Newtonsoft.Json;
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
    /// Interaction logic for LobbyView.xaml
    /// </summary>
    public partial class LobbyView : UserControl
    {
        public LobbyView()
        {
            Locals.client.userJoinedLobbyEvent += UserJoinedLobbyResponse;
            InitializeComponent();

        }

        public static void UserJoinedLobbyResponse()
        {
            //This method is handling the JoinResponse from the server
            var msg = Locals.client.PacketReader.ReadMessage();
            if (msg.Contains('/') && msg.Split('/')[0] == "JOINLOBBY")
            {
                var status = msg.Split('/')[1];


                if (status != "ERROR" && status != "Success")
                {
                    Locals.lobby = JsonConvert.DeserializeObject<Lobby>(status);
                    ;
                }
            }
            else
            {
                MessageBox.Show("Response Message format is bad:" + msg);
            }
        }
    }
}
