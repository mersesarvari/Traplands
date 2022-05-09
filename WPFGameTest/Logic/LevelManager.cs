using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Game.Helpers;
using Game.Logic;
using Game.Models;
using Game.Renderer;
using WPFGameTest.Logic;

namespace Game
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
        Finish,
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
        Trap_Waypoint,
        Moving_Trap
    }

    public static class LevelManager
    {
        private static Dictionary<string, Level> levels = new Dictionary<string, Level>();

        private static Level currentLevel = null;
        public static Level CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }

        public static List<Level> LevelList()
        {
            List<Level> list = new List<Level>();

            foreach (var pairs in levels)
            {
                list.Add(pairs.Value);
            }

            return list;
        }

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

        public static void Save(string key, int[,] Map, Dictionary<Coordinate, EditorElement> editorElements)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Levels/");
            string filePath = path + key + ".lvl";

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                for (int i = 0; i < Map.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.GetLength(1); j++)
                    {
                        if (Map[j, i] == (int)ObjectType.Trap_Waypoint)
                        {
                            EditorElement element = null;
                            editorElements.TryGetValue(new Coordinate(j, i), out element);

                            WaypointRect waypointRect = (WaypointRect)element;
                            sw.Write(Map[j, i] + ":" + waypointRect.Waypoint.GroupID + ":" + waypointRect.Waypoint.ID + ";");
                        }
                        else
                        {
                            sw.Write(Map[j, i] + ";");
                        }
                    }
                    sw.WriteLine();
                }
            }

            string[] fileLines = File.ReadAllLines(filePath);
            Level value = new Level(key, fileLines);
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

        public static void LoadLevels()
        {
            // Get all levels in the Levels folder
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Levels"), "*.lvl");

            // Go through each level file
            foreach (var path in lvls)
            {
                string name = Path.GetFileNameWithoutExtension(path); // Use the filename without extension as name of the level
                string[] lines = File.ReadAllLines(path); // Read line by line

                Level level = new Level(name, lines);

                levels.Add(name, level);
            }
        }
    }
}
