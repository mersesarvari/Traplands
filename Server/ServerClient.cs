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
                        //USER Command
                        case 4:
                            var ucmd = _packetReader.ReadMessage();
                            //Console.WriteLine("Command recieved:"+ucmd);
                            Command.CommandManager(ucmd);
                            //Server.SendMessage(3, ucmd.Split('/')[1], "Proba");
                            ;
                            break;
                        //Messgae
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            //Console.WriteLine($"[{DateTime.Now}]: Message recieved from {Username} {msg}");
                            Server.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        //Game Command
                        /*
                        case 7:
                            var gcmd = _packetReader.ReadMessage();
                            Server.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {gcmd}");
                            break;
                        */
                        case 10:
                            var dc = _packetReader.ReadMessage();
                            Server.BroadcastDisconnect(dc);
                            break;
                        case 2:
                            Console.WriteLine("Test recieved from the client");
                            //Server.Tes();
                            Server.BroadcastMessage("test was succesfull");
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
        public Player ConvertClientTouser(_Client client)
        {
            return Server.players.FirstOrDefault(x => x.Id == client.UID.ToString());
        }
    }
}
