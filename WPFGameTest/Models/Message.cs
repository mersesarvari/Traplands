using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class Message
    {
        public string ownerid { get; set; }
        public string message { get; set; }

        public Message(string ownerid, string message)
        {
            this.ownerid = ownerid;
            this.message = message;
        }
    }
}
