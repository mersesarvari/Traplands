using System.Collections.Generic;
using System.Windows.Controls;
using WPFGameTest.Helpers;
using WPFGameTest.Renderer;

namespace WPFGameTest.Models
{
    public class Level
    {
        public int[,] Map { get; private set; }
        public Vector2 SpawnPoint { get; private set; }
        public List<Entity> Solids { get; private set; }
        public List<Entity> Interactables { get; private set; }

        public Level(int[,] map)
        {
            Map = map;
            SpawnPoint = new Vector2();
            Solids = new List<Entity>();
            Interactables = new List<Entity>();
        }

        public void Load(Canvas canvas)
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    ObjectType e = (ObjectType)Map[i, j];
                    SolidObject obj = new SolidObject
                    (
                        new Vector2(ObjectData.BLOCK_WIDTH * i, ObjectData.BLOCK_HEIGHT * j),
                        new Vector2(ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT),
                        Resource.GetImage(e.ToString()),
                        true
                    );
                    switch (e)
                    {
                        case ObjectType.Spawn:
                            SpawnPoint = new Vector2(obj.Transform.Position.X, obj.Transform.Position.Y - 9);
                            obj.Tag = "Spawn";
                            canvas.Children.Add(obj.Element);
                            break;
                        case ObjectType.Spike:
                            obj.Tag = "Trap";
                            Interactables.Add(obj);
                            canvas.Children.Add(obj.Element);
                            break;
                        case > ObjectType.Grass_First and < ObjectType.Grass_Last:
                            Solids.Add(obj);
                            canvas.Children.Add(obj.Element);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
