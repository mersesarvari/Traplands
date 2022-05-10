using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Models
{

    public class RenderData
    {
        public string FileName { get; set; }
        public int ImageIndex { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double ScaleX { get; set; }
    }
}
