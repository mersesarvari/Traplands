using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Game.Models;
using Game.Net.IO;

namespace Game.Net
{
    public class Client
    {
        TcpClient client;
        private PacketReader PacketReader;

        //public static List<MovementPackage> MovementHistory=new List<MovementPackage>();
        public static List<Player> PlayerHistory = new List<Player>();

        public event Action connectedEvent;
        public event Action userDisconnectedEvent;
        public event Action messageRecievedEvent;
        public event Action userCommandSentEvent;
        public event Action userCreatedLobbyEvent;
        public event Action userJoinedLobbyEvent;
        public event Action userJoinedGameEvent;
        public event Action userMovedEvent;

        public Client()
        {
            client = new TcpClient();
        }        
        //Client recieving datafrom the Server (OPCODE if for identifying)
        private void ReadPacket()
        {
            Task.Run(() => {
                while (true)
                {
                    try
                    {
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)
                        {
                            //ServerSendingConnection
                            case 1:
                                connectedEvent?.Invoke();
                                break;
                            //userCommand
                            case 9:
                                userCommandSentEvent?.Invoke();
                                break;
                            //Message
                            case 5:
                                messageRecievedEvent?.Invoke();
                                break;
                            //Disconnect
                            case 10:
                                userDisconnectedEvent?.Invoke();
                                break;
                            //Lobby was Created
                            case 11:
                                userCreatedLobbyEvent?.Invoke();
                                break;
                            //User Joined the Lobby
                            case 12:
                                userJoinedLobbyEvent?.Invoke();                                
                                break;
                            //User joined the game
                            case 13:
                                userJoinedGameEvent?.Invoke();
                                break;
                            case 17:
                            //User Moved
                                userMovedEvent?.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        //Showing an exception or something
                        MessageBox.Show("Unable to process server response", "Server Response Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
            });
        }
        public void ConnectToServer(string username)
        {
            if (!client.Connected)
            {
                client.Connect("127.0.0.1", 5000);
                PacketReader = new PacketReader(client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(0);
                    connectPacket.WriteMessage(username);
                    client.Client.Send(connectPacket.GetPacketbytes());
                }
                ReadPacket();
            }
        }
        public void DisconnectFromServer(string guid)
        {
            if (client.Connected)
            {
                PacketReader = new PacketReader(client.GetStream());
                if (!string.IsNullOrEmpty(guid))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(10);
                    connectPacket.WriteMessage(guid);
                    client.Client.Send(connectPacket.GetPacketbytes());
                }
                ReadPacket();
            }
        }
        public void SendMessageToServer(string message)
        { 
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOptCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketbytes());
        }
        public void SendCommandToServer(string commandname, string executor, string command ,int tick)
        {
            string formattedcommand = "";
            string splitter = "";
            if ((command == null || command.Trim()==""))
            {
                command = "NULL";
            }
            formattedcommand = commandname + "/"+ executor + "/" + command+"/"+tick;
            //MessageBox.Show("Sending Command: " + formattedcommand);
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOptCode(9);
            messagePacket.WriteMessage(formattedcommand);
            client.Client.Send(messagePacket.GetPacketbytes());
        }
    }
}
