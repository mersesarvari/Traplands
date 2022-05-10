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
using Newtonsoft.Json;

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

        public static void Disconnect(string id)
        {
            locals.client.DisconnectFromServer(id);
        }
        //A tickes rész átírandó arra amit a rendes gameban is használunk..
        public static void JoinLobby(INavigationService service,Locals locals, string username,string lobbycode)
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
        public static void CreateLobby(INavigationService service, Locals locals, string username, int currenttick)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("CREATELOBBY", locals.user.Id, locals.user.Id, false);
                Thread.Sleep(1500);
                service.Navigate();
                var checker = MultiLogic.locals;                
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }


        //SENDING INFORMATION: MAP, PLAYERLIST, 
        public static void StartGame(INavigationService service, Lobby lobby, string username)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("STARTGAME", locals.lobby.LobbyId, JsonConvert.SerializeObject(lobby), false);
                Thread.Sleep(1000);
                //Implementálás
                service.Navigate();
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }
        //Selecting map from the servers database
        public static void SetMap(string mapname)
        {
            //SETTING MAP FOR THE GAME
            //locals.lobby.Map =;
        }






    }
}
