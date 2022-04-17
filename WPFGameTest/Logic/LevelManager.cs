using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFGameTest.Helpers;
using WPFGameTest.Logic;
using WPFGameTest.Models;
using WPFGameTest.Renderer;

namespace WPFGameTest
{
    public static class ObjectData
    {
        public const int BLOCK_WIDTH = 36, BLOCK_HEIGHT = 36;
        public const int PLAYER_WIDTH = 44, PLAYER_HEIGHT = 44;
    }

    public enum ObjectType
    {
        None = 0,
        Spawn,
        Spike,
        Grass_First,
        Grass_Top_Center,
        Grass_Top_Right,
        Grass_Top_Left,
        Grass_Top_Right_Bottom,
        Grass_Top_Left_Bottom,
        Grass_Bottom_Center,
        Grass_Bottom_Right,
        Grass_Bottom_Left,
        Grass_Mid_Right,
        Grass_Mid_Left,
        Grass_Mid_Right_Left,
        Grass_Under,
        Grass_Last,
    }

    public static class LevelManager
    {
        private static Dictionary<string, Level> levels = new Dictionary<string, Level>();

        public static ImageBrush GetCorrectTileImage(ObjectType obj)
        {
            return new ImageBrush(Resource.GetImage(obj.ToString()));
        }

        private static Level Get(string key)
        {
            Level level;
            levels.TryGetValue(key, out level);

            return level;
        }

        public static void Save(string key, Level value, Canvas canvas)
        {
            levels.Add(key, value);

            // Save map to image

            //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, PixelFormats.Pbgra32);
            //renderTargetBitmap.Render(canvas);

            //PngBitmapEncoder pngImage = new PngBitmapEncoder();
            //pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            //using (Stream fileStream = File.Create(key + ".png"))
            //{
            //    pngImage.Save(fileStream);
            //}
        }

        public static Level GetLevel(string key)
        {
            Level level = Get(key);

            if (level == null)
            {
                return null;
            }

            return level;
        }
    }
}
