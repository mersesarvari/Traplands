using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameTest.Models
{
    public class Level
    {
        public int[,] Map { get; private set; }

        public Level(int[,] map)
        {
            Map = map;
        }
    }
}
