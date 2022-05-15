using Server;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace Server // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server.Start("127.0.0.1", 5000, 500);
        }       
        
    }
}