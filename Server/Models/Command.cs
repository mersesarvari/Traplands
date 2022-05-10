using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Command
    {
        public static void CommandManager(string commandname,string executor, string command)
        {
            switch (commandname)
            {
                case "CREATELOBBY":
                    Lobby.Create(executor);
                    Lobby.Join(executor, command);
                    break;
                case "STARTGAME":
                    Lobby.Start(executor, command);
                    break;
                case "MOVE":
                    Game.Move(executor,command);
                    break;
                default:
                    break;
            }
        }

    }
}
