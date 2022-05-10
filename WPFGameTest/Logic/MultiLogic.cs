using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Logic
{
    public class MultiLogic
    {
        private Locals locals;
        public void Connect(Locals locals, string username)
        {
            this.locals = locals;
            locals.user.Username = username;
            locals.client.ConnectToServer(locals.user.Username);
            this.locals.Connected = true;
            
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public void JoinLobby(INavigationService service,Locals locals, string username,string lobbycode)
        {
            try
            {
                if (locals.client.Connected())
                {
                    locals.client.SendCommandToServer("JOINLOBBY", locals.user.Id, lobbycode, true);
                    //Valahogyan meg kéne kapnom, hogy sikeres volt e a csatlakozás. Valószínűleg eventen keresztül
                    //service.Navigate();
                }
                else
                {
                    MessageBox.Show("You are not connected");               
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public void CreateLobby(INavigationService service, Locals locals, string username, int currenttick)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("CREATELOBBY", locals.user.Id, locals.user.Id, false);
                JoinLobby(service, locals, username, locals.user.Id);                
                ;
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }


    }
}
