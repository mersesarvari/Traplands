using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Game.Helpers;
using Game.Logic;
using Game.Renderer;
using WPFGameTest.Logic;

namespace Game.Models
{
    public class Level
    {
        public string Name { get; set; }
        public Vector2 SpawnPoint { get; set; }
        public Vector2 FinishPoint { get; set; }
        public List<GameObject> Props { get; private set; }
        public List<GameObject> Solids { get; private set; }
        public List<GameObject> Interactables { get; private set; }
        public string ImagePath { get; set; }
        public float BestTime { get; set; }

        public int OrderIndex { get; set; }

        private string[] file;
        private string filePath;

        public Level(string name, string[] file)
        {
            Name = name;
            SpawnPoint = new Vector2();
            FinishPoint = new Vector2();
            Solids = new List<GameObject>();
            Interactables = new List<GameObject>();
            Props = new List<GameObject>();
            this.file = file;
            filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Levels/" + name + ".lvl");
            BestTime = float.Parse(file[0]);
        }

        public void AddNewBestTime(float time)
        {
            BestTime = time;
            file[0] = time.ToString();
        }

        public void Save()
        {
            File.WriteAllLines(filePath, file);
        }

        public void Load()
        {
            Interactables.Clear();
            Solids.Clear();
            Props.Clear();

            List<List<Waypoint>> waypoints = new List<List<Waypoint>>();

            BestTime = float.Parse(file[0]);

            for (int i = 1; i < file.Length; i++)
            {
                string[] splitLine = file[i].Split(';'); // Split each row into single objectIDs(string)

                for (int j = 0; j < splitLine.Length; j++)
                {
                    if (splitLine[j].Length != 0) // If it has a value
                    {
                        int currentObject = -1;
                        string[] splitBlock = splitLine[j].Split(':');
                        bool facingRight = false;

                        if (splitBlock.Length > 1)
                        {
                            if (splitBlock.Length == 2)
                            {
                                currentObject = int.Parse(splitBlock[0]);
                                facingRight = int.Parse(splitBlock[1]) == 1 ? true : false;
                            }
                            else
                            {
                                currentObject = int.Parse(splitBlock[0]);
                                int id = int.Parse(splitBlock[2]);
                                int groupId = int.Parse(splitBlock[1]);

                                Waypoint waypoint = new Waypoint(id, groupId, new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i));
                                bool foundGroup = false;
                                foreach (var list in waypoints)
                                {
                                    Waypoint wp = list[0];
 
                                    if (wp.GroupID == groupId)
                                    {
                                        list.Add(waypoint);
                                        foundGroup = true;
                                        break;
                                    }
                                }

                                if (!foundGroup)
                                {
                                    List<Waypoint> newWaypointList = new List<Waypoint>();
                                    newWaypointList.Add(waypoint);
                                    waypoints.Add(newWaypointList);
                                }
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
                                Props.Add(obj);
                                break;
                            case ObjectType.Finish:
                                FinishPoint = new Vector2(obj.Transform.Position.X, obj.Transform.Position.Y - 9);
                                obj.Tag = "Finish";
                                Interactables.Add(obj);
                                break;
                            case ObjectType.Spike:
                                obj.Tag = "Trap";
                                Interactables.Add(obj);
                                break;
                            case ObjectType.Cannon:
                                obj = new Cannon(new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i),
                                    new Vector2(ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT), facingRight ? Direction.Right : Direction.Left, false);
                                obj.SetDefaultSprite(Resource.GetImage(facingRight ? "Cannon_Right" : "Cannon_Left"));
                                Interactables.Add(obj);
                                Solids.Add(obj);
                                break;
                            case ObjectType.Trap_Spike:
                                obj = new TrapBlock(new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i),
                                                    new Vector2(ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT));
                                Interactables.Add(obj);
                                Solids.Add(obj);
                                break;
                            case > ObjectType.Grass_First and < ObjectType.Grass_Last:
                                if (e == ObjectType.Grass_Top_Center || e == ObjectType.Grass_Top_Left || e == ObjectType.Grass_Top_Right)
                                {
                                    if (RandomGenerator.IntInRange(0,100) > 95)
                                    {
                                        Bird bird = new Bird(new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i - 16), new Vector2(16, 16));
                                        Interactables.Add(bird);
                                    }
                                    if (RandomGenerator.IntInRange(0, 100) > 85)
                                    {
                                        DynamicObject dyn = new DynamicObject(new Vector2(ObjectData.BLOCK_WIDTH * j, ObjectData.BLOCK_HEIGHT * i - 16), new Vector2(32, 16));
                                        if (RandomGenerator.IntInRange(0, 2) == 0)
                                        {
                                            dyn.SetDefaultSprite(Resource.GetImage("Flowers"));
                                        }
                                        else
                                        {
                                            dyn.SetDefaultSprite(Resource.GetImage("Grass_Props"));
                                        }
                                        Props.Add(dyn);
                                    }
                                }
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
                List<Waypoint> orderedList = list.OrderBy(x => x.ID).ToList();
                MovingTrap movingTrap = new MovingTrap(orderedList[0].Position, new Vector2(ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT));
                movingTrap.SetDefaultSprite(Resource.GetImage("Spike"));
                movingTrap.SetWaypoints(orderedList);

                Interactables.Add(movingTrap);
            }
        }

        public void LoadToEditor(LevelEditor editor)
        {
            List<List<Waypoint>> waypoints = new List<List<Waypoint>>();

            for (int i = 1; i < file.Length; i++)
            {
                string[] splitLine = file[i].Split(';'); // Split each row into single objectIDs(string)

                for (int j = 0; j < splitLine.Length; j++)
                {
                    if (splitLine[j].Length != 0) // If it has a value
                    {
                        int currentObject = -1;
                        string[] splitBlock = splitLine[j].Split(':');
                        bool facingRight = false;

                        if (splitBlock.Length > 1)
                        {
                            if (splitBlock.Length == 2)
                            {
                                currentObject = int.Parse(splitBlock[0]);
                                facingRight = int.Parse(splitBlock[1]) == 1 ? true : false;
                            }
                            else
                            {
                                currentObject = int.Parse(splitBlock[0]);
                                int id = int.Parse(splitBlock[2]);
                                int groupId = int.Parse(splitBlock[1]);

                                Waypoint waypoint = new Waypoint(id, groupId, new Vector2(j, i));
                                bool foundGroup = false;
                                foreach (var list in waypoints)
                                {
                                    Waypoint wp = list[0];

                                    if (wp.GroupID == groupId)
                                    {
                                        list.Add(waypoint);
                                        foundGroup = true;
                                        break;
                                    }
                                }

                                if (!foundGroup)
                                {
                                    List<Waypoint> newWaypointList = new List<Waypoint>();
                                    newWaypointList.Add(waypoint);
                                    waypoints.Add(newWaypointList);
                                }
                            }
                        }

                        if (currentObject == -1) currentObject = int.Parse(splitLine[j]); // Convert from string to int

                        EditorElement element = new EditorElement();
                        ObjectType e = (ObjectType)currentObject;
                        element.Type = e;

                        switch (e)
                        {
                            case ObjectType.Cannon:
                                element = new CannonRect(facingRight);
                                break;
                            default:
                                break;
                        }

                        editor.PlaceSingleTile(new Vector2(j, i), element);
                    }
                }
            }

            int counter = 0;

            foreach (var list in waypoints)
            {
                List<Waypoint> orderedList = list.OrderBy(x => x.ID).ToList();
                Trace.WriteLine(orderedList.Count);
                WaypointGroup wpg = new WaypointGroup(counter);
                wpg.SetupCollections(editor.Lines, editor.Rectangles);
                counter++;

                for (int i = 0; i < orderedList.Count; i++)
                {
                    WaypointRect wpr = wpg.AddWaypoint(orderedList[i].Position, editor.Grid.CellSize);
                    editor.AddTile(wpr, orderedList[i].Position);
                }

                for (int j = 0; j < wpg.WaypointRects.Count - 1; j++)
                {
                    wpg.ConnectWaypoints(wpg.WaypointRects[j], wpg.WaypointRects[j + 1]);
                }

                editor.WaypointGroups.Add(wpg);
            }
        }
    }
}
