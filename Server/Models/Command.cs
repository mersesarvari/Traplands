using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Command
    {
        public static void CommandManager(string commandname,string executor, string command)
        {
            switch (commandname)
            {
                case "CREATELOBBY":
                    Lobby.Create(executor);
                    break;
                case "JOINLOBBY":
                    Lobby.Join(executor, command);
                    ;
                    break;
                case "MOVE":
                    //Game.Move(tick,executor,command[0]);
                    break;
                default:
                    break;
            }
        }

    }
}
