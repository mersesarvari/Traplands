using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Game.Helpers;
using Game.Models;
using Game.Renderer;
using Game;
using Game.Logic;

namespace WPFGameTest.Logic
{
    public struct Coordinate
    {
        public int X;
        public int Y;
        public Coordinate(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
    }

    public abstract class EditorElement : ObservableObject
    {
        public Rectangle Rectangle { get; set; }
        public Vector2 Position { get; set; }
        public string Name { get; set; }
        public ObjectType Type { get; set; }
    }

    public class WaypointGroup : EditorElement
    {
        public int ID { get; set; }
        public List<WaypointRect> WaypointRects { get; set; }

        public List<Line> Lines { get; set; }

        private List<Line> lineList;
        private List<EditorElement> rectElements;

        public WaypointGroup(int id)
        {
            WaypointRects = new List<WaypointRect>();
            Lines = new List<Line>();
            ID = id;

            Rectangle = new Rectangle();
            Rectangle.Fill = new SolidColorBrush(Colors.AntiqueWhite);
            Position = new Vector2();
            Name = "Moving trap waypoint";
            Type = ObjectType.Trap_Waypoint;
        }

        public void SetupCollections(List<Line> lineList, List<EditorElement> rectElements)
        {
            this.lineList = lineList;
            this.rectElements = rectElements;
        }

        public Line AddLine(WaypointRect first, WaypointRect second)
        {
            Line line = new Line();
            line.X1 = first.Position.X + first.Rectangle.Width / 2;
            line.Y1 = first.Position.Y + first.Rectangle.Height / 2;
            line.X2 = second.Position.X + second.Rectangle.Width / 2;
            line.Y2 = second.Position.Y + second.Rectangle.Height / 2;

            line.Stroke = new SolidColorBrush(Colors.Yellow);
            line.StrokeThickness = 2;

            Lines.Add(line);

            return line;
        }

        public WaypointRect AddWaypoint(Vector2 position, Vector2 size)
        {
            Waypoint wp = new Waypoint(WaypointRects.Count, ID, position);

            WaypointRect rect = new WaypointRect();
            rect.Rectangle = new Rectangle();
            rect.Rectangle.Fill = new SolidColorBrush(Colors.OrangeRed);
            rect.Rectangle.Fill.Opacity = 0.5;
            rect.Rectangle.Width = size.X;
            rect.Rectangle.Height = size.Y;
            rect.Position = position;

            rect.IdLabel = new Label();
            rect.IdLabel.Content = wp.ID;
            rect.Waypoint = wp;

            rect.ParentGroup = this;

            WaypointRects.Add(rect);

            return rect;
        }

        public void ConnectWaypoints(WaypointRect from, WaypointRect to)
        {
            if (from.Next == null && to.Prev == null)
            {
                from.SetNext(to);
                to.SetPrev(from);

                WaypointGroup toGroup = to.ParentGroup;

                if (this != toGroup)
                {
                    if ((from.Prev != null || from.Prev == null) && to.Next == null)
                    {
                        toGroup.WaypointRects.Remove(to);
                        WaypointRects.Add(to);
                        to.ParentGroup = this;
                    }
                    else if (from.Prev != null && to.Next != null)
                    {
                        foreach (WaypointRect rect in toGroup.WaypointRects)
                        {
                            WaypointRects.Add(rect);
                            rect.ParentGroup = this;
                        }

                        toGroup.WaypointRects.Clear();
                    }
                    else if (from.Prev == null && to.Next != null)
                    {
                        WaypointRects.Remove(from);
                        toGroup.WaypointRects.Insert(0, from);
                        from.ParentGroup = toGroup;
                        toGroup.UpdateWaypoints();
                    }
                }
            }

            UpdateWaypoints();
        }

        public void UpdateWaypoints()
        {
            int count = 0;

            foreach (WaypointRect rect in WaypointRects)
            {
                rect.Waypoint.GroupID = ID;
                rect.Waypoint.ID = count++;
                rect.IdLabel.Content = rect.Waypoint.ID;
            }

            RegenerateLines();
        }

        public void DeleteWaypoint(int id)
        {
            WaypointRect toDelete = WaypointRects[id];

            if (id != 0 && id != WaypointRects.Count - 1)
            {
                return;
            }

            if (toDelete.Prev != null)
            {
                toDelete.Prev.SetNext(null);
            }

            if (toDelete.Next != null)
            {
                toDelete.Next.SetPrev(null);
            }

            rectElements.Remove(WaypointRects[id]);
            WaypointRects.Remove(WaypointRects[id]);

            UpdateWaypoints();
        }

        public void RegenerateLines()
        {
            foreach (var line in Lines)
            {
                lineList.Remove(line);
            }

            Lines.Clear();

            foreach (var rect in WaypointRects)
            {
                if (rect.Next != null)
                {
                    lineList.Add(AddLine(rect, rect.Next));
                }
            }
        }
    }

    public class BlockRect : EditorElement
    {
        public BlockRect()
        {
            Rectangle = new Rectangle();
            Position = new Vector2();
            Rectangle.Fill = new SolidColorBrush(Colors.Blue);
            Name = "Block";
            Type = ObjectType.Grass_Top_Center;
        }
    }

    public class TrapRect : EditorElement
    {
        public TrapRect()
        {
            Rectangle = new Rectangle();
            Position = new Vector2();
            Rectangle.Fill = new SolidColorBrush(Colors.DarkRed);
            Name = "Trap";
            Type = ObjectType.Spike;
        }
    }

    public class SpawnPoint : EditorElement
    {
        public SpawnPoint()
        {
            Rectangle = new Rectangle();
            Position = new Vector2();
            Rectangle.Fill = new SolidColorBrush(Colors.Yellow);
            Name = "Spawn point";
            Type = ObjectType.Spawn;
        }
    }

    public class FinishPoint : EditorElement
    {
        public FinishPoint()
        {
            Rectangle = new Rectangle();
            Position = new Vector2();
            Rectangle.Fill = new SolidColorBrush(Colors.Pink);
            Name = "Finish point";
            Type = ObjectType.Finish;
        }
    }

    public class WaypointRect : EditorElement
    {
        public WaypointRect Next { get; set; }
        public WaypointRect Prev { get; set; }
        public WaypointGroup ParentGroup { get; set; }
        public Waypoint Waypoint { get; set; }
        public Label IdLabel { get; set; }
        public bool Start { get; set; }

        public WaypointRect()
        {
            Rectangle = new Rectangle();
            Rectangle.Fill = new SolidColorBrush(Colors.AntiqueWhite);
            Position = new Vector2();
            Name = "Moving trap waypoint";
            Type = ObjectType.Trap_Waypoint;
        }

        public void Delete()
        {
            ParentGroup.DeleteWaypoint(Waypoint.ID);
        }

        public void SetNext(WaypointRect next)
        {
            Next = next;
            Waypoint.Next = next == null ? null : Next.Waypoint;
        }

        public void SetPrev(WaypointRect prev)
        {
            Prev = prev;
            Waypoint.Prev = prev == null ? null : Prev.Waypoint;
        }
    }

    public class LevelEditor : ILevelEditor
    {
        public LevelGrid Grid { get; set; }
        public List<EditorElement> Rectangles { get; set; }
        public List<Line> Lines { get; set; }
        public EditorElement PreviewRect { get; set; }

        public bool Paused { get; set; }

        // Visuals
        private FrameworkElement renderTarget;
        private ScrollViewer camera;

        private EditorElement currentElement;

        private Line previewLine;

        // Camera
        private double Zoom { get; set; }

        private double mouseX = 0;
        private double mouseY = 0;
        private Vector2 dir = new Vector2();
        private Vector2f cameraPos = new Vector2f(0, 0);
        private Vector2 actualPos = new Vector2();
        private int moveSpeed = 500;

        // Placing tiles
        private Dictionary<Coordinate, EditorElement> rects = new Dictionary<Coordinate, EditorElement>();
        private bool isDeleting = false;

        private Vector2[] dirs =
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(-1,0),
            new Vector2(0,-1),
            new Vector2(-1,-1),
            new Vector2(1,-1),
            new Vector2(-1,1)
        };

        // Might be temp
        List<WaypointGroup> WaypointGroups;
        WaypointRect selected;
        WaypointRect releasedOn;
        ObjectType selectedElement;
        bool painting;
        Vector2 spawnPosition;
        Vector2 finishPosition;

        public LevelEditor()
        {
            Grid = new LevelGrid(100, 100, ObjectData.BLOCK_WIDTH, ObjectData.BLOCK_HEIGHT);
            Rectangles = new List<EditorElement>();
            WaypointGroups = new List<WaypointGroup>();
            Lines = Grid.Lines;
            Zoom = 0.5;

            previewLine = new Line();
            previewLine.StrokeThickness = 0;
            previewLine.Stroke = new SolidColorBrush(Colors.Yellow);
            Lines.Add(previewLine);

            spawnPosition = new Vector2(0, 0);
            finishPosition = new Vector2(1, 0);

            Grid.Map[spawnPosition.X, spawnPosition.Y] = (int)ObjectType.Spawn;
            Grid.Map[finishPosition.X, finishPosition.Y] = (int)ObjectType.Finish;

            SpawnPoint spawn = new SpawnPoint();
            spawn.Rectangle.Width = ObjectData.BLOCK_WIDTH;
            spawn.Rectangle.Height = ObjectData.BLOCK_HEIGHT;
            spawn.Position = new Vector2(spawnPosition.X * ObjectData.BLOCK_WIDTH, spawnPosition.Y * ObjectData.BLOCK_HEIGHT);
            rects.Add(new Coordinate(spawnPosition.X, spawnPosition.Y), spawn);
            Rectangles.Add(spawn);

            FinishPoint finish = new FinishPoint();
            finish.Rectangle.Width = ObjectData.BLOCK_WIDTH;
            finish.Rectangle.Height = ObjectData.BLOCK_HEIGHT;
            finish.Position = new Vector2(finishPosition.X * ObjectData.BLOCK_WIDTH, finishPosition.Y * ObjectData.BLOCK_HEIGHT);
            rects.Add(new Coordinate(finishPosition.X, finishPosition.Y), finish);
            Rectangles.Add(finish);
        }

        public void ProcessInput()
        {
            // Update camera position
            if (Input.GetKey(Key.W))
            {
                dir.Y = -1;
            }
            else if (Input.GetKey(Key.S))
            {
                dir.Y = 1;
            }
            else
            {
                dir.Y = 0;
            }

            if (Input.GetKey(Key.D))
            {
                dir.X = 1;
            }
            else if (Input.GetKey(Key.A))
            {
                dir.X = -1;
            }
            else
            {
                dir.X = 0;
            }

            if (Input.GetKey(Key.LeftShift))
            {
                isDeleting = true;
            }
            else
            {
                isDeleting = false;
            }

            if (Input.GetMouseButtonReleased(Mouse.LeftButton))
            {
                if (selected != null) MouseReleased();
            }

            if (Input.GetKeyReleased(Key.Space))
            {
                CheckInfo();
            }
        }

        public void Update(float deltaTime)
        {
            cameraPos.Y += dir.Y * moveSpeed * deltaTime;
            cameraPos.X += dir.X * moveSpeed * deltaTime;

            if (selected != null)
            {
                previewLine.StrokeThickness = 2;
                previewLine.X2 = mouseX;
                previewLine.Y2 = mouseY;
            }
            else
            {
                previewLine.StrokeThickness = 0;
            }

            if (painting)
            {
                if (!isDeleting)
                {
                    PlaceSingleTile();
                }
                else
                {
                    DeleteTile();
                }
            }

            CameraController.Instance.UpdateEditorCamera(cameraPos);
            SetCameraInBoundries();
        }

        private void SetCameraInBoundries()
        {
            // Setting max and minimum zoom
            if (Zoom >= 1)
            {
                Zoom = 1;
            }
            else if (Zoom <= 0.25)
            {
                Zoom = 0.25;
            }

            // Keeping the camera position in boundaries
            if (cameraPos.X < camera.Width / 2)
            {
                cameraPos.X = (int)(camera.Width / 2);
            }
            else if (cameraPos.X > (int)((renderTarget.Width - camera.Width / 2) * Zoom))
            {
                cameraPos.X = (int)((renderTarget.Width - camera.Width / 2) * Zoom);
            }

            if (cameraPos.Y < camera.Height / 2)
            {
                cameraPos.Y = (int)(camera.Height / 2);
            }
            else if (cameraPos.Y > (int)((renderTarget.Height - camera.Height / 2) * Zoom))
            {
                cameraPos.Y = (int)((renderTarget.Height - camera.Height / 2) * Zoom);
            }
        }

        private void PlaceSingleTile()
        {
            Vector2 matrixPos = new Vector2(actualPos.X / Grid.CellSize.X, actualPos.Y / Grid.CellSize.Y);

            if (selected == null)
            {
                if (Grid.Map[matrixPos.X, matrixPos.Y] == 0)
                {
                    switch (selectedElement)
                    {
                        case ObjectType.Trap_Waypoint:
                            WaypointGroup wpg = null;

                            foreach (var group in WaypointGroups)
                            {
                                if (group.WaypointRects.Count == 0)
                                {
                                    wpg = group;
                                }
                            }

                            if (wpg == null)
                            {
                                wpg = new WaypointGroup(WaypointGroups.Count);
                            }

                            wpg.SetupCollections(Lines, Rectangles);
                            WaypointRect rect = wpg.AddWaypoint(actualPos, Grid.CellSize);

                            WaypointGroups.Add(wpg);

                            AddTile(rect, matrixPos);
                            break;
                        case ObjectType.Grass_Top_Center:
                            foreach (Vector2 dir in dirs)
                            {
                                // Only place tiles if it's a correct matrix position
                                if (matrixPos.X + dir.X >= 0 &&
                                    matrixPos.Y + dir.Y >= 0 &&
                                    matrixPos.X + dir.X < Grid.Map.GetLength(0) &&
                                    matrixPos.Y + dir.Y < Grid.Map.GetLength(1))
                                {
                                    if (Grid.Map[matrixPos.X + dir.X, matrixPos.Y + dir.Y] != 0) // If any of the desired cells is not empty just contiune to the next
                                    {
                                        continue;
                                    }

                                    BlockRect block = new BlockRect();
                                    block.Rectangle.Width = ObjectData.BLOCK_WIDTH;
                                    block.Rectangle.Height = ObjectData.BLOCK_HEIGHT;

                                    AddTile(block, new Vector2(matrixPos.X + dir.X, matrixPos.Y + dir.Y));

                                    // Update tile and it's neighbours
                                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y);
                                    UpdateTile(matrixPos.X + dir.X + 1, matrixPos.Y + dir.Y);
                                    UpdateTile(matrixPos.X + dir.X - 1, matrixPos.Y + dir.Y);
                                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y + 1);
                                    UpdateTile(matrixPos.X + dir.X, matrixPos.Y + dir.Y - 1);
                                }
                            }
                            break;
                        case ObjectType.Spike:
                            TrapRect spike = new TrapRect();
                            spike.Rectangle.Width = ObjectData.BLOCK_WIDTH;
                            spike.Rectangle.Height = ObjectData.BLOCK_HEIGHT;
                            AddTile(spike, matrixPos);
                            break;
                        case ObjectType.Spawn:
                            ReplaceTile(Get(new Coordinate(spawnPosition.X, spawnPosition.Y)), spawnPosition, matrixPos);
                            spawnPosition = matrixPos;
                            break;
                        case ObjectType.Finish:
                            ReplaceTile(Get(new Coordinate(finishPosition.X, finishPosition.Y)), finishPosition, matrixPos);
                            finishPosition = matrixPos;
                            break;
                        default:
                            Trace.WriteLine("Nothing is selected.");
                            break;
                    }
                }
                else
                {
                    if (Grid.Map[matrixPos.X, matrixPos.Y] == (int)ObjectType.Trap_Waypoint)
                    {
                        selected = (Get(new Coordinate(matrixPos.X, matrixPos.Y)) as WaypointRect);
                        previewLine.X1 = actualPos.X + Grid.CellSize.X / 2;
                        previewLine.Y1 = actualPos.Y + Grid.CellSize.Y / 2;
                    }
                    else if (Grid.Map[matrixPos.X, matrixPos.Y] == (int)ObjectType.None) // Cannon later?
                    {
                        //selectedCannon = (Get(new Coordinate(matrixPos.X, matrixPos.Y)) as CannonRect);
                    }
                }
            }
        }

        private EditorElement Get(Coordinate cord)
        {
            EditorElement rect;
            rects.TryGetValue(cord, out rect);

            return rect;
        }

        private void MouseReleased()
        {
            Vector2 matrixPos = new Vector2(actualPos.X / Grid.CellSize.X, actualPos.Y / Grid.CellSize.Y);

            if (Grid.Map[matrixPos.X, matrixPos.Y] == (int)ObjectType.Trap_Waypoint) // Waypoint test
            {
                releasedOn = (Get(new Coordinate(matrixPos.X, matrixPos.Y)) as WaypointRect);

                if (selected == releasedOn)
                {
                    return;
                }

                WaypointGroup fromGroup = selected.ParentGroup;

                fromGroup.ConnectWaypoints(selected, releasedOn);
            }

            selected = null;
        }

        private void DeleteFromCollections(Vector2 matrixPos)
        {
            EditorElement element = Get(new Coordinate(matrixPos.X, matrixPos.Y));
            Grid.Map[matrixPos.X, matrixPos.Y] = 0;
            Rectangles.Remove(element);
            rects.Remove(new Coordinate(matrixPos.X, matrixPos.Y));
        }

        private void DeleteTile()
        {
            Vector2 matrixPos = new Vector2(actualPos.X / Grid.CellSize.X, actualPos.Y / Grid.CellSize.Y);

            if (Grid.Map[matrixPos.X, matrixPos.Y] == 0)
            {
                return;
            }

            ObjectType type = Get(new Coordinate(matrixPos.X, matrixPos.Y)).Type;

            switch (type)
            {
                case ObjectType.Trap_Waypoint:
                    (Get(new Coordinate(matrixPos.X, matrixPos.Y)) as WaypointRect).Delete();
                    Grid.Map[matrixPos.X, matrixPos.Y] = 0;
                    rects.Remove(new Coordinate(matrixPos.X, matrixPos.Y));
                    break;
                case ObjectType.Grass_Top_Center:
                    DeleteFromCollections(matrixPos);
                    UpdateTile(matrixPos.X + 1, matrixPos.Y);
                    UpdateTile(matrixPos.X - 1, matrixPos.Y);
                    UpdateTile(matrixPos.X, matrixPos.Y + 1);
                    UpdateTile(matrixPos.X, matrixPos.Y - 1);
                    break;
                case ObjectType.Spike:
                    DeleteFromCollections(matrixPos);
                    break;
                default:
                    break;
            }
        }

        public void CheckInfo()
        {
            Vector2 matrixPos = new Vector2(actualPos.X / Grid.CellSize.X, actualPos.Y / Grid.CellSize.Y);
            if (Grid.Map[matrixPos.X, matrixPos.Y] == 1)
            {
                Waypoint next = (Get(new Coordinate(matrixPos.X, matrixPos.Y)) as WaypointRect).Waypoint;

                Trace.WriteLine(next.GroupID + ": " + next.Prev.ID + " - " + next.Next.ID);
            }
        }

        public void Init(FrameworkElement renderTarget, ScrollViewer camera)
        {
            this.renderTarget = renderTarget;
            this.camera = camera;

            renderTarget.LayoutTransform = new ScaleTransform(Zoom, Zoom);

            camera.PreviewMouseMove += (o, e) =>
            {
                // Get mouse position relative to the render target (world)
                mouseX = Mouse.GetPosition(renderTarget).X;
                mouseY = Mouse.GetPosition(renderTarget).Y;

                // Get the rounded position for the Grid-like placement
                actualPos = new Vector2(Grid.CellSize.X * (int)(mouseX / Grid.CellSize.X), Grid.CellSize.Y * (int)(mouseY / Grid.CellSize.Y));
            };

            // Set mouse scrolling as zoom
            camera.PreviewMouseWheel += (o, e) =>
            {
                e.Handled = true;

                if (e.Delta > 0)
                {
                    Zoom += 0.05;
                }
                else
                {
                    Zoom -= 0.05;
                }

                renderTarget.LayoutTransform = new ScaleTransform(Zoom, Zoom);
            };

            camera.PreviewMouseDown += (o, e) =>
            {
                painting = true;
            };

            camera.PreviewMouseUp += (o, e) =>
            {
                painting = false;
            };
        }

        private IList<EditorElement> elements;

        public void LoadElements(IList<EditorElement> elements)
        {
            this.elements = elements;

            elements.Add(new WaypointGroup(WaypointGroups.Count));
            elements.Add(new SpawnPoint());
            elements.Add(new FinishPoint());
            elements.Add(new BlockRect());
            elements.Add(new TrapRect());
        }

        public void SaveLevel(string levelName)
        {
            LevelManager.Save(levelName, Grid.Map, rects);
        }

        public void SelectElement(EditorElement element)
        {
            selectedElement = element.Type;
            Trace.WriteLine(element.Type.ToString());
        }

        private void ReplaceTile(EditorElement element, Vector2 from, Vector2 to)
        {
            Coordinate coordFrom = new Coordinate(from.X, from.Y);
            Coordinate coordTo = new Coordinate(to.X, to.Y);

            Grid.Map[from.X, from.Y] = 0;
            Grid.Map[to.X, to.Y] = (int)element.Type;

            rects.Remove(coordFrom);
            rects.Add(coordTo, element);

            element.Position = new Vector2(to.X * ObjectData.BLOCK_WIDTH, to.Y * ObjectData.BLOCK_HEIGHT);
        }

        private void AddTile(EditorElement element, Vector2 to)
        {
            Coordinate coordTo = new Coordinate(to.X, to.Y);

            Grid.Map[to.X, to.Y] = (int)element.Type;

            rects.Add(coordTo, element);

            Rectangles.Add(element);

            element.Position = new Vector2(to.X * ObjectData.BLOCK_WIDTH, to.Y * ObjectData.BLOCK_HEIGHT);
        }

        private void UpdateTile(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return;
            }

            EditorElement rect = Get(new Coordinate(x, y));

            if (rect is null)
                return;

            if (Grid.Map[x, y] < (int)ObjectType.Grass_First ||
                Grid.Map[x, y] > (int)ObjectType.Grass_Last)
            {
                return;
            }

            bool right = false, left = false, top = false, bottom = false;
            ObjectType type = GetCorrectTile(right, left, top, bottom);

            if (x < 1 || y < 1)
            {
                rect.Rectangle.Fill = new ImageBrush(Resource.GetImage(type.ToString()));
                return;
            }

            if (Grid.Map[x + 1, y] > (int)ObjectType.Grass_First &&
                Grid.Map[x + 1, y] < (int)ObjectType.Grass_Last)
            {
                right = true;
            }
            if (Grid.Map[x - 1, y] > (int)ObjectType.Grass_First &&
                Grid.Map[x - 1, y] < (int)ObjectType.Grass_Last)
            {
                left = true;
            }
            if (Grid.Map[x, y - 1] > (int)ObjectType.Grass_First &&
                Grid.Map[x, y - 1] < (int)ObjectType.Grass_Last)
            {
                top = true;
            }
            if (Grid.Map[x, y + 1] > (int)ObjectType.Grass_First &&
                Grid.Map[x, y + 1] < (int)ObjectType.Grass_Last)
            {
                bottom = true;
            }

            type = GetCorrectTile(right, left, top, bottom);
            rect.Rectangle.Fill = new ImageBrush(Resource.GetImage(type.ToString()));
            Grid.Map[x, y] = (int)type;
        }

        private ObjectType GetCorrectTile(bool right, bool left, bool top, bool bottom)
        {
            ObjectType obj = ObjectType.Grass_Top_Center;

            if (right && !left && !top && !bottom) // Right
            {
                obj = ObjectType.Grass_Top_Left_Bottom;
            }
            else if (right && left && !top && !bottom) // Right + Left
            {
                obj = ObjectType.Grass_Top_Center;
            }
            else if (right && !left && top && !bottom) // Right + Top
            {
                obj = ObjectType.Grass_Bottom_Left;
            }
            else if (right && !left && !top && bottom) // Right + Bottom
            {
                obj = ObjectType.Grass_Top_Left;
            }
            else if (right && !left && top && bottom) // Right + Bottom + Top
            {
                obj = ObjectType.Grass_Mid_Left;
            }
            else if (right && left && !top && bottom) // Right + Left + Bottom
            {
                obj = ObjectType.Grass_Top_Center;
            }
            else if (right && left && top && !bottom) // Right + Left + Top
            {
                obj = ObjectType.Grass_Bottom_Center;
            }
            else if (left && !right && !top && !bottom) // Left
            {
                obj = ObjectType.Grass_Top_Right_Bottom;
            }
            else if (left && !right && top && !bottom) // Left + Top
            {
                obj = ObjectType.Grass_Bottom_Right;
            }
            else if (left && !right && !top && bottom) // Left + Bottom
            {
                obj = ObjectType.Grass_Top_Right;
            }
            else if (left && !right && top && bottom) // Left + Top + Bottom
            {
                obj = ObjectType.Grass_Mid_Right;
            }
            else if (top && !left && !right && !bottom) // Top
            {
                obj = ObjectType.Grass_Bottom_Center;
            }
            else if (top && !left && !right && bottom) // Top + Bottom
            {
                obj = ObjectType.Grass_Mid_Right_Left;
            }
            else if (top && left && right && bottom) // Right + Left + Top + Bottom
            {
                obj = ObjectType.Grass_Under;
            }

            return obj;
        }
    }
}
