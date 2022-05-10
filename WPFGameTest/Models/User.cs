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
        public RenderData RenderData { get; set; }

        public User()
        {

        }

        public User(string id, string username, RenderData renderData)
        {
            Id = id;
            Username = username;
            RenderData = renderData;
        }
    }
}
