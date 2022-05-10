using Game.Models;
using Game.MVVM.Commands;
using Game.MVVM.Services;
using Game.MVVM.View;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Logic
{
    public class MultiLogic
    {

        public static Locals locals;
        public IMessenger messenger;

        public MultiLogic(IMessenger messenger)
        {
            locals = new(messenger);
            this.messenger = messenger;            
        }

        public void Disconnect(string id)
        {
            locals.client.DisconnectFromServer(id);
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
                    service.Navigate();
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
                Thread.Sleep(1500);
                service.Navigate();
                var checker = MultiLogic.locals;
                ;
                
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }

        public void StartGame(INavigationService service, Locals locals, string username, int currenttick)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("StartGame", locals.user.Id, locals.user.Id, false);
                Thread.Sleep(1500);
                //Implementálás
                service.Navigate();
                ;

            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }




    }
}
