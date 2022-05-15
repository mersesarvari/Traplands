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

        public MultiLogic(INavigationService lobbyService, INavigationService gameService, INavigationService multimenuService, INavigationService menuService)
        {
            locals = new(lobbyService, gameService, multimenuService, menuService);
            //this.messenger = messenger;            
        }
        public MultiLogic(INavigationService lobbyService, INavigationService gameService, INavigationService multimenuService, INavigationService menuService, IMessenger messenger)
        {
            locals = new(lobbyService, gameService, multimenuService, menuService);
            //this.messenger = messenger;
        }

        public static void Disconnect(string id)
        {
            locals.client.DisconnectFromServer(id);
        }
        public static void JoinLobby(Locals locals, string username,string lobbycode)
        {
            try
            {
                if (locals.client.Connected())
                {
                    locals.client.SendCommandToServer("JOINLOBBY", locals.user.Id, lobbycode);
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
        public static void CreateLobby(Locals locals, string username, int currenttick)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("CREATELOBBY", locals.user.Id, locals.user.Id);
                var checker = MultiLogic.locals;                
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }       
        public static void StartGame(Lobby lobby, string username)
        {
            foreach (var user in lobby.Users)
            {
                user.RenderData = new RenderData();
            }
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("STARTGAME", locals.lobby.LobbyId, JsonConvert.SerializeObject(lobby));
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
        }
        public static void LeaveGame(Lobby lobby, string userId)
        {
            locals.client.SendCommandToServer("LEAVEGAME", userId, JsonConvert.SerializeObject(lobby));
        }
        public static void SetMap(string mapname)
        {
            //SETTING MAP FOR THE GAME
            //locals.lobby.Map =;
        }
        public void SendMessage(Message message)
        {
            if (locals.client.Connected())
            {
                locals.client.SendCommandToServer("SENDMESSAGE", locals.lobby.LobbyId, JsonConvert.SerializeObject(message));
            }
            else
            {
                MessageBox.Show("You are not connected to the server");
            }
            
        }






    }
}
