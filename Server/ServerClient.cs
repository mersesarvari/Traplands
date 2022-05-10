using System.Net.Sockets;

namespace Server
{
    public class ServerClient
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient TCP { get; set; }

        PacketReader _packetReader;
        
        public ServerClient(TcpClient client)
        {
            TCP = client;
            UID = Guid.NewGuid();            
            //Console.WriteLine("Current games:"+games.Count);
            _packetReader = new PacketReader(TCP.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();


            Console.WriteLine($"[{DateTime.Now}]: Client has connected: {Username}");
            Task.Run(() => Process());
        }        
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 4:
                            var negy_commandname = _packetReader.ReadMessage();
                            var negy_executor = _packetReader.ReadMessage();
                            var negy_command = _packetReader.ReadMessage();
                            Command.CommandManager(negy_commandname, negy_executor, negy_command);
                            break;
                        case 10:
                            var dc = _packetReader.ReadMessage();
                            Server.BroadcastDisconnect(dc);
                            break;
                        default:
                            Console.WriteLine($"[INFO] Recieving invalid opcode from the client({opcode})!");
                            break;
                    }                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[{UID}]: Disconnected!" + e.Message);
                    Server.BroadcastDisconnect(UID.ToString());
                    TCP.Close();
                    break;
                }
            }
        }
        public Player ConvertClientToUser(ServerClient client)
        {
            return Server.players.FirstOrDefault(x => x.Id == client.UID.ToString());
        }


    }
}
