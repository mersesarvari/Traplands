using LobbyClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyClient
{
    public class User
    {
        public string Id { get; set; }
        public string  Username { get; set; }
        float movespeed;
        CharacterController controller;

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
