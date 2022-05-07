using Client;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class MultiLogic
    {
        public static void Connect(string username)
        {
            Locals.user.Username = username;
            Locals.client.ConnectToServer(Locals.user.Username);
        }

        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public static void JoinLobby(string username,string lobbycode, int currenttick)
        {
            Connect(username);
            Locals.client.SendCommandToServer("JOINLOBBY", Locals.user.Id, lobbycode, currenttick);
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public static void CreateLobby(string username, int currenttick)
        {
            Connect(username);
            if (Locals.user.Username != null)
            {
                Locals.client.SendCommandToServer("CREATELOBBY", Locals.user.Id, "NULL", currenttick);
                Locals.client.SendCommandToServer("JOINLOBBY", Locals.user.Id, Locals.user.Id, currenttick);
            }
            else
            {
                throw new Exception("You have to set your username");
            }
        }


    }
}
