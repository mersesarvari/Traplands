using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbymakerServer.Models
{
    public class MovementPackage
    {
        public int Horizontal;
        public int Vertical;
        public int Timestamp;
        public MovementPackage(int tick, int horizontal, int vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Timestamp = tick;
        }
    }
}
