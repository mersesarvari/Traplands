using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGameTest.Helpers
{
    public static class Time
    {
        private static Stopwatch sw = new Stopwatch();

        public static float DeltaTime { get { return (float)sw.Elapsed.TotalSeconds; } }
        public static void Tick()
        {
            sw.Restart();
        }
    }
}
