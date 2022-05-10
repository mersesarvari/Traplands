using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Server.Models
{
    public class Game
    {
        public List<Player> Player { get; set; }

        internal static void Move(string executor, string command)
        {
            Console.WriteLine("MOVE CALLED");
            var data = JsonConvert.DeserializeObject<Player>(command);
            ;
        }
    }
}
