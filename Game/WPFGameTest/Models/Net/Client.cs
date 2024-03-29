﻿using Game.Logic;
using Game.MVVM.Services;
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
        public PacketReader packetReader;
        //public static List<MovementPackage> MovementHistory=new List<MovementPackage>();
        public event Action connectedEvent;
        public event Action userDisconnectedEvent;
        public event Action userJoinedLobbyEvent;
        public event Action updateUserData;
        public event Action gameStartedEvent;
        public event Action gameLeftEvent;
        public event Action messageRecievedEvent;
        public event Action gameFinishedEvent;
        public event Action lobbyCreatedEvent;

        public Client()
        {
            _client = new TcpClient();
        }

        private void ReadPacket()
        {
            Task.Run(() =>
            {
                while (_client.Connected)
                {
                    try
                    {
                        var opcode = packetReader.ReadByte();
                        switch (opcode)
                        {
                            case 10:
                                userDisconnectedEvent?.Invoke();
                                break;
                            case 1:
                                connectedEvent?.Invoke();
                                break;
                            case 2:
                                userJoinedLobbyEvent?.Invoke();
                                break;
                            case 3:
                                lobbyCreatedEvent?.Invoke();
                                break;
                            case 4:
                                updateUserData?.Invoke();
                                break;
                            case 5:
                                gameStartedEvent?.Invoke();
                                break;
                            case 6:
                                gameLeftEvent?.Invoke();
                                break;
                            case 7:
                                messageRecievedEvent?.Invoke();
                                break;
                            case 9:
                                gameFinishedEvent?.Invoke();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"[Error]: readpacket threw an error: \n {ex.Message}");
                        break;
                    }
                }
            });
        }
        public void ConnectToServer(string username, string color)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 5000);
                packetReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(0);
                    connectPacket.WriteMessage(username);
                    connectPacket.WriteMessage(color);
                    _client.Client.Send(connectPacket.GetPacketbytes());
                }
                ReadPacket();
            }
        }
        public bool Connected()
        {
            return _client.Connected;
        }
        public static void Disconnect(string id)
        {
            MultiLogic.locals.client.DisconnectFromServer(id);
        }
        public void DisconnectFromServer(string guid)
        {
            if (_client.Connected)
            {
                packetReader = new PacketReader(_client.GetStream());
                if (!string.IsNullOrEmpty(guid))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOptCode(10);
                    connectPacket.WriteMessage(guid);
                    _client.Client.Send(connectPacket.GetPacketbytes());
                }
            }
        }
        public void SendCommandToServer(string commandname, string executor, string command)
        {
            if (_client.Connected)
            {
                //Trace.WriteLine($"Sending Command: {commandname} \n {executor} \n {command}");
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOptCode(4);
                messagePacket.WriteMessage(commandname);
                messagePacket.WriteMessage(executor);
                messagePacket.WriteMessage(command);
                _client.Client.Send(messagePacket.GetPacketbytes());
            }
        }

    }
}
