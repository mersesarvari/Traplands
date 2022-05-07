using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logic
{
    public class GameTimer
    {
        private Timer _timer = null;
        public static int Tick = 0;
        public void Start(int milliseconds)
        {
            // Create a Timer object that knows to call our TimerCallback
            // method once every 2000 milliseconds.
            _timer = new Timer(TimerCallback, null, 0, milliseconds);
            // Wait for the user to hit <Enter>
        }

        private static void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.
            //Console.WriteLine("In TimerCallback: " + DateTime.Now);
            if (Tick < 1024)
            {
                Tick++;
            }
            else 
            {
                Tick = 0;            
            }
            Console.WriteLine($"TickNumber: {Tick}");
        }


    }
}
