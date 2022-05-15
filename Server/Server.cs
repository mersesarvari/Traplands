using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server
{
    public static class Server
    {
        public static List<Lobby> lobbies= new List<Lobby>();
        public static List<Socket> clients = new List<Socket>();
        public static List<Player> players = new List<Player>();
        private static TcpListener listener;

        public static void Start(string ip, int port, int tickinterval)
        {            
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();            

            Console.WriteLine($"Server started at: {ip}:{port}");
            //Kliens fogadás
            while (true)
            {
                var client = new Socket(listener.AcceptTcpClient());
                var user = new Player(client);
                clients.Add(client);
                players.Add(user);
                Console.WriteLine($"Client Added: {client.UID} CLIENTS: {clients.Count}");

                /* Send back Username and Id to the current client */
                SendConnection(client);
            }
        }
        public static void Stop()
        {
            listener.Stop();
        }
        public static Player FindUserById(string id)
        {
            return players.Where(x => x.Id == id).FirstOrDefault();
        }
        public static Socket FindClient(string userid)
        {
            return clients.Where(x => x.UID.ToString() == userid).FirstOrDefault();
        }
        #region Server Responses to Client
        public static void BroadcastConnection()
        {
            foreach (var client in clients)
            {
                foreach (var clnt in clients)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOptCode(1);
                    broadcastPacket.WriteMessage(clnt.Username);
                    broadcastPacket.WriteMessage(clnt.UID.ToString());
                    client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
                }
            }
        }
        public static void SendConnection(Socket client)
        {
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.WriteOptCode(1);
            broadcastPacket.WriteMessage(client.Username);
            broadcastPacket.WriteMessage(client.UID.ToString());
            broadcastPacket.WriteMessage(JsonConvert.SerializeObject(Server.lobbies));
            client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
        }
        public static void BroadcastDisconnect(string uid)
        {                   
            foreach (var client in clients)
            {                
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOptCode(10);
                packetBuilder.WriteMessage(uid.ToString());
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
                clients.Remove(Server.FindClient(uid.ToString()));
                players.Remove(Server.FindUserById(uid.ToString()));
                Console.WriteLine("Current user count " + clients.Count);
            }
        }            
        public static void BroadcastResponse(byte opcode, string messagename, string message)
        {
            Console.WriteLine($"[BroadCastMessage(3)] : {message}");
            foreach (var client in clients)
            {
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOptCode(opcode);
                packetBuilder.WriteMessage(messagename);
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
            }
        }
        /// <summary>
        /// Sending respomse to a client
        /// </summary>
        /// <param name="opcode"> the opcode that the clinet can categorize
        /// <param name="client"> the client who have to recieve the message</param>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        public static void SendResponse(byte opcode, Socket client, string message)
        {
            if (client == null)
            {
                throw new Exception("client was null");
            }

            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteOptCode(opcode);
            packetBuilder.WriteMessage(message);

            try
            {
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
                //Console.WriteLine("SendResponse: "+client.TCP.GetHashCode()+"|"+opcode);
                //Console.WriteLine($"[Response]: {FindUserById(userid).Username} - [{3}]:{message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CommandManager(string commandname, string executor, string command)
        {
            switch (commandname)
            {
                case "CREATELOBBY":
                    Lobby.Create(executor);
                    Lobby.Join(executor, command);
                    break;
                case "JOINLOBBY":
                    Lobby.Join(executor, command);
                    break;
                case "STARTGAME":
                    Game.Start(executor, command);
                    break;
                case "MOVE":
                    Game.Move(executor, command);
                    break;
                case "LEAVEGAME":
                    Game.LeaveGame(executor, command);
                    break;
                case "SENDMESSAGE":
                    Message.SendMessageToLobby(executor, command);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
