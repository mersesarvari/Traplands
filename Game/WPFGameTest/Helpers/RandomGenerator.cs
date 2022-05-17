using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Helpers
{
    public static class RandomGenerator
    {
        private static Random rng = new Random();

        public static int IntInRange(int min, int max)
        {
            return rng.Next(min, max);
        }
    }
}
