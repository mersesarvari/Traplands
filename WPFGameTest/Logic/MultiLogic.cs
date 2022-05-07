using Client;
using Client.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.View;
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
            Locals.Connected = true;
            var l = Locals.user;
            var c = Locals.Connected;
            ;
        }
        public static void ConnectToServer(string username)
        {            
            Connect(username);
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public static NavigateToLobbyCommand JoinLobby(INavigationService service, string username,string lobbycode, int currenttick)
        {
            //ConnectToServer(username);
            try
            {
                if (Locals.client.Connected())
                {
                    var l = Locals.user;
                    Locals.client.SendCommandToServer("JOINLOBBY", Locals.user.Id, lobbycode, currenttick);
                    return new NavigateToLobbyCommand(service, Locals.lobby);
                }
                else return null;
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public static void CreateLobby(INavigationService service, string username, int currenttick)
        {
            //ConnectToServer(username);
            var d = Locals.user;
            ;
            try
            {
                if (Locals.client.Connected())
                {
                    Locals.client.SendCommandToServer("CREATELOBBY", Locals.user.Id, "NULL", currenttick);
                    JoinLobby(service, username, Locals.user.Id, currenttick);
                    LobbyView lobby = new LobbyView();
                    
                }
                else
                {
                    throw new Exception("You are not connected to the server");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}
