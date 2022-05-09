using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Helpers;
using Game.Logic;
using Game.Renderer;

namespace Game.Models
{
    public class Level
    {
        public string Name { get; set; }
        public Vector2 SpawnPoint { get; set; }
        public Vector2 FinishPoint { get; set; }
        public List<GameObject> Solids { get; private set; }
        public List<GameObject> Interactables { get; private set; }
        public string ImagePath { get; set; }
        string[] File { get; set; }

        public Level(string name, string[] file)
        {
            Name = name;
            File = file;
            SpawnPoint = new Vector2();
            FinishPoint = new Vector2();
            Solids = new List<GameObject>();
            Interactables = new List<GameObject>();
        }

        public void Load()
        {
            List<List<Waypoint>> waypoints = new List<List<Waypoint>>();

            for (int i = 0; i < File.Length; i++)
            {
                string[] splitLine = File[i].Split(';'); // Split each row into single objectIDs(string)

                for (int j = 0; j < splitLine.Length; j++)
                {
                    if (splitLine[j].Length != 0) // If it has a value
                    {
                        int currentObject = -1;
                        string[] splitBlock = splitLine[j].Split(':');
                        if (splitBlock.Length > 1)
                        {
                            currentObject = int.Parse(splitBlock[0]);
                            int id = int.Parse(splitBlock[2]);
                            int groupId = int.Parse(splitBlock[1]);

                            Waypoint waypoint = new Waypoint(id, groupId, new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i));
                            bool foundGroup = false;
                            foreach (var list in waypoints)
                            {
                                Waypoint wp = list.FirstOrDefault();
                                if (wp != null)
                                {
                                    if (wp.GroupID == groupId)
                                    {
                                        list.Add(waypoint);
                                        foundGroup = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundGroup)
                            {
                                List<Waypoint> newWaypointList = new List<Waypoint>();
                                newWaypointList.Add(waypoint);
                                waypoints.Add(newWaypointList);
                            }
                        }

                        if (currentObject == -1) currentObject = int.Parse(splitLine[j]); // Convert from string to int

                        ObjectType e = (ObjectType)currentObject;
                        SolidObject obj = new SolidObject
                        (
                            new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i),
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
                            case ObjectType.Finish:
                                FinishPoint = new Vector2(obj.Transform.Position.X, obj.Transform.Position.Y - 9);
                                obj.Tag = "Finish";
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

            foreach (var list in waypoints)
            {
                list.OrderBy(x => x.ID);

                MovingTrap movingTrap = new MovingTrap(list[0].Position, new Vector2(ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT));
                movingTrap.SetWaypoints(list);

                Interactables.Add(movingTrap);
            }
        }
    }
}
