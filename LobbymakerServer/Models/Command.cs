using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbymakerServer
{
    public class Command
    {
        private static void CheckCommandId(string commandname,string executor, string command, int tick)
        {
            switch (commandname)
            {
                case "CREATELOBBY":
                    Lobby.Create(executor);
                    break;
                case "JOINLOBBY":
                    Lobby.Join(executor, command);
                    break;
                case "MOVE":
                    Game.Move(tick,executor,command[0]);
                    break;
                default:
                    break;
            }
        }
        public static void CommandManager(string command)
        {
            //Console.WriteLine("Current command:"+command);
            if (command.Contains('/'))
            {
                string[] cmdparts = command.Split('/');
                if (cmdparts.Length >= 4)
                {
                    CheckCommandId(cmdparts[0], cmdparts[1], cmdparts[2], int.Parse(cmdparts[3]));
                }
                else 
                {
                    CheckCommandId(cmdparts[0], cmdparts[1], cmdparts[2], -1);
                }
                
            }
            else
            {
                throw new Exception("Invalid command in Command class");
            }
        }

    }
}
