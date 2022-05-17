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
        }
        public MultiLogic(INavigationService lobbyService, INavigationService gameService, INavigationService multimenuService, INavigationService menuService, IMessenger messenger)
        {
            locals = new(lobbyService, gameService, multimenuService, menuService);
        }        
     
        
        public static void SetMap(string mapname)
        {
            //SETTING MAP FOR THE GAME
            //locals.lobby.Map =;
        }
        
    }
}
