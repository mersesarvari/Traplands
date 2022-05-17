using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Helpers
{
    public static class CompositionTargetEx
    {
        private static TimeSpan last = TimeSpan.Zero;
        private static event EventHandler<RenderingEventArgs> frameUpdating;

        public static event EventHandler<RenderingEventArgs> Rendering
        {
            add
            {
                if (frameUpdating == null)
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                frameUpdating += value;
            }
            remove
            {
                frameUpdating -= value;
                if (frameUpdating == null)
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }

        static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs args = (RenderingEventArgs)e;
            if (args.RenderingTime == last)
                return;
            last = args.RenderingTime; 
            frameUpdating(sender, args);
        }
    }
}
