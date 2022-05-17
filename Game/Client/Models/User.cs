using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class User
    {
        public string Id { get; set; }
        public string  Username { get; set; }
        float movespeed;

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
