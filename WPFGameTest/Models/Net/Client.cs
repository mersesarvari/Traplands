using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Models
{
    public class Client
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public static List<MovementPackage> MovementHistory=new List<MovementPackage>();

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
            _client = new TcpClient();
        }        

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
                            case 1:
                                MessageBox.Show("connectedEvent INVOKED");
                                connectedEvent?.Invoke();
                                break;
                            case 9:
                                userCommandSentEvent?.Invoke();
                                break;
                            case 5:
                                messageRecievedEvent?.Invoke();
                                break;
                            case 10:
                                userDisconnectedEvent?.Invoke();
                                break;
                            case 11:
                                userCreatedLobbyEvent?.Invoke();
                                break;
                            case 6:
                                MessageBox.Show("userJoinedLobbyEvent INVOKED");
                                userJoinedLobbyEvent?.Invoke();                                
                                break;
                            case 13:
                                userJoinedGameEvent?.Invoke();
                                break;
                            case 17:
                                //MessageBox.Show("userMovedEvent Invoked");
                                userMovedEvent?.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"[ERROR]: {e.Message}");                     
                    }
                    
                }
            });
        }
        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 5000);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketbytes());
                }
                ReadPacket();
            }
        }
        public bool Connected()
        { 
            return _client.Connected;
        }
        public void DisconnectFromServer(string guid)
        {
            if (_client.Connected)
            {
                PacketReader = new PacketReader(_client.GetStream());
                if (!string.IsNullOrEmpty(guid))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(10);
                    connectPacket.WriteMessage(guid);
                    _client.Client.Send(connectPacket.GetPacketbytes());
                }
                ReadPacket();
            }
        }
        public void SendMessageToServer(string message)
        { 
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOptCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketbytes());
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
            _client.Client.Send(messagePacket.GetPacketbytes());
        }




    }
}
