using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using Game.Models;
using Game.Renderer;

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
        public static Dictionary<string, Level> Levels
        {
            get {
                return levels;
            }
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

        public static void Save(string key, Level value)
        {
            levels.Add(key, value);

            using (StreamWriter sw = new StreamWriter(key + ".lvl"))
            {
                for (int i = 0; i < value.Map.GetLength(0); i++)
                {
                    for (int j = 0; j < value.Map.GetLength(1); j++)
                    {
                        sw.Write(value.Map[j, i] + ";");
                    }
                    sw.WriteLine();
                }
            }

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
                string[] lines = File.ReadAllLines(path); // Read line by line
                int[,] levelMap = new int[100, 100];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] splitLine = lines[i].Split(';'); // Split each row into single objectIDs(string)

                    for (int j = 0; j < splitLine.Length; j++)
                    {
                        if (splitLine[j].Length != 0) // If it has a value
                        {
                            int currentObject = int.Parse(splitLine[j]); // Convert from string to int
                            levelMap[j, i] = currentObject;
                        }
                    }
                }

                Level level = new Level(levelMap);
                string name = Path.GetFileNameWithoutExtension(path); // Use the filename without extension as name of the level

                if (Get(name) == null)
                {
                    levels.Add(name, level);
                }
            }
        }
    }
}
