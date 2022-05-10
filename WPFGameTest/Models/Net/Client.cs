using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public event Action userJoinedLobbyEvent;
        public event Action userJoinedGameEvent;
        public event Action userMovedEvent;
        public event Action updateUserData;

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
                        Trace.WriteLine($"Recieving from the server ({opcode})");
                        switch (opcode)
                        {
                            case 0:
                                userDisconnectedEvent?.Invoke();
                                break;
                            case 1:
                                connectedEvent?.Invoke();
                                break;
                            case 2:
                                userJoinedLobbyEvent?.Invoke();                             
                                break;
                            case 4:
                                updateUserData?.Invoke();
                                break;
                            default:
                                //MessageBox.Show($"Recieved unknown message ({opcode})");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);                   
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
        
        public void SendCommandToServer(string commandname, string executor, string command, bool waitforresponse)
        {
            if (_client.Connected)
            {
                if (waitforresponse)
                {
                    PacketReader = new PacketReader(_client.GetStream());
                }                
                Trace.WriteLine($"Sending Command: {commandname} \n {executor} \n {command}");
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOptCode(4);
                messagePacket.WriteMessage(commandname);
                messagePacket.WriteMessage(executor);
                messagePacket.WriteMessage(command);
                _client.Client.Send(messagePacket.GetPacketbytes());
                if(waitforresponse)
                {
                    ReadPacket();
                }
                
            }
        }        




    }
}
