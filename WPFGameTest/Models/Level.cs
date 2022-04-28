using System.Collections.Generic;
using WPFGameTest.Helpers;
using WPFGameTest.Renderer;

namespace WPFGameTest.Models
{
    public class Level
    {
        public int[,] Map { get; private set; }
        public string Name { get; set; }
        public Vector2 SpawnPoint { get; private set; }
        public List<GameObject> Solids { get; private set; }
        public List<GameObject> Interactables { get; private set; }
        public string ImagePath { get; set; }

        public Level(int[,] map)
        {
            Map = map;
            SpawnPoint = new Vector2();
            Solids = new List<GameObject>();
            Interactables = new List<GameObject>();
        }

        public void Load()
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
                            break;
                        case ObjectType.Spike:
                            obj.Tag = "Trap";
                            Interactables.Add(obj);
                            break;
                        case > ObjectType.Grass_First and < ObjectType.Grass_Last:
                            Solids.Add(obj);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
