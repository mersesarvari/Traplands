using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class User
    {
        public string Id { get; set; }
        public string  Username { get; set; }

        public User()
        {
        }
        public User(string id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}
