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
        public void Connect()
        {
            Locals.client.ConnectToServer(Locals.user.Username);
        }

        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public void JoinLobby(string lobbycode, int currenttick)
        {
            if (Locals.user.Username != null && Locals.user.Id != null)
            {
                Locals.client.SendCommandToServer("JOINLOBBY", Locals.user.Id, lobbycode, currenttick);
            }
            else
            {
                throw new Exception("You are already joined a lobby!");
            }
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public void CreateLobby(int currenttick)
        {
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
