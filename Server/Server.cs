using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Logic;

namespace Server
{
    public class Server
    {
        public static GameTimer timer = new GameTimer();
        public static List<Lobby> lobbies= new List<Lobby>();
        public static List<Game> games=new List<Game>();
        private static List<ServerClient> clients = new List<ServerClient>();
        public static List<Player> players = new List<Player>();
        private static TcpListener listener;

        #region
        /// <summary>
        /// Method to Start the server
        /// </summary>
        /// <param name="ip"> Ip of our server</param>
        /// <param name="port">Port of our server</param>
        /// <param name="tickinterval"> Interval of the server timer in Miliseconds</param>
        public static void Start(string ip, int port, int tickinterval)
        {            
            //var ip = IPAddress.Parse("127.0.0.1") + ":5000";
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();            

            Console.WriteLine($"Server started at: {ip}:{port}");
            //Kliens fogadás
            while (true)
            {
                var client = new ServerClient(listener.AcceptTcpClient());
                var user = new Player(client);
                clients.Add(client);
                players.Add(user);

                /* Send back Username and Id to the current client */
                SendConnection(client); ;
            }
        }
        /// <summary>
        /// Method to stop the Server
        /// </summary>
        public static void Stop()
        {
            listener.Stop();
        }
        public static Player FindUserById(string id)
        {
            return players.Where(x => x.Id == id).FirstOrDefault();
        }
        public static ServerClient FindClient(string userid)
        {
            return clients.Where(x => x.UID.ToString() == userid).FirstOrDefault();
        }
        #endregion serverMethods

        #region Server Responses to Client
        static void BroadcastConnection()
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
        static void SendConnection(ServerClient client)
        {
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.WriteOptCode(1);
            broadcastPacket.WriteMessage(client.Username);
            broadcastPacket.WriteMessage(client.UID.ToString());
            broadcastPacket.WriteMessage(GameTimer.Tick.ToString());
            client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
        }
        public static void BroadcastDisconnect(string uid)
        {
            ServerClient disconnectedClient = clients.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            clients.Remove(disconnectedClient);
            players.Remove(disconnectedClient.ConvertClientToUser(disconnectedClient));
            foreach (var client in clients)
            {
                Console.WriteLine("User count after removing user: " + players.Count);
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOptCode(10);
                broadcastPacket.WriteMessage(uid.ToString());
                client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
            }
            //BroadcastMessage($"[{disconnectedUser.Username}] Disconnected!");       

        }
        
        public static void BroadcastResponse(byte opcode, string messagename, string message)
        {
            Console.WriteLine($"[BroadCastMessage(3)] : {message}");
            foreach (var client in clients)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOptCode(opcode);
                msgPacket.WriteMessage(messagename);
                msgPacket.WriteMessage(message);
                client.TCP.Client.Send(msgPacket.GetPacketbytes());
            }
        }
        public static void SendResponse(byte opcode, string messagename, string userid, string message)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOptCode(opcode);
            msgPacket.WriteMessage(messagename);
            msgPacket.WriteMessage(message);
            var client = clients.Where(x => x.UID.ToString() == userid).FirstOrDefault();
            if (client != null)
            {
                client.TCP.Client.Send(msgPacket.GetPacketbytes());
                //Console.WriteLine($"[Response]: {FindUserById(userid).Username} - ({3}){messagename}:{message}");
            }
            else
            {
                throw new Exception("client doesnt exists");
            } 
        }
        public static void MovePlayer(Game game,MovementPackage movement)
        {            
            foreach (var player in game.Players)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOptCode(6);
                msgPacket.WriteMessage(JsonConvert.SerializeObject(movement));
                var clnt = FindClient(player.Id);
                clnt.TCP.Client.Send(msgPacket.GetPacketbytes());
            }
        }
        #endregion

    }
}
