using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Game.Helpers;
using Game.Renderer;

namespace Game.Logic
{
    public class EditorRect
    {
        public IntRect Rect { get; set; }
        public Brush Fill { get; set; }
    }

    public class LevelEditor : ILevelEditor
    {
        public LevelGrid Grid { get; set; }
        public List<EditorRect> Rectangles { get; set; }
        public List<Line> Lines { get; set; }
        public EditorRect PreviewRect { get; set; }

        private struct Coordinate
        {
            public int X;
            public int Y;
            public Coordinate(int x = 0, int y = 0)
            {
                X = x;
                Y = y;
            }
        }

        // Visuals
        private FrameworkElement renderTarget;

        // Camera
        public double Zoom { get; set; }

        private double mouseX = 0;
        private double mouseY = 0;
        private Vector2 dir = new Vector2();
        private Vector2f cameraPos = new Vector2f(0, 0);
        private Vector2 actualPos = new Vector2();
        private int moveSpeed = 500;

        // Placing tiles
        private Dictionary<Coordinate, EditorRect> rects = new Dictionary<Coordinate, EditorRect>();
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

        public LevelEditor(FrameworkElement renderTarget, ScrollViewer camera)
        {
            this.renderTarget = renderTarget;
            Grid = new LevelGrid(100, 100, 44, 44);
            Rectangles = new List<EditorRect>();
            Lines = Grid.Lines;
            Zoom = 0.5;

            renderTarget.LayoutTransform = new ScaleTransform(Zoom, Zoom);

            camera.PreviewMouseMove += (o, e) =>
            {
                // Get mouse position relative to the render target (world)
                mouseX = Mouse.GetPosition(this.renderTarget).X;
                mouseY = Mouse.GetPosition(this.renderTarget).Y;

                // Get the rounded position for the Grid-like placement
                actualPos = new Vector2(Grid.CellSize.X * (int)(mouseX / Grid.CellSize.X), Grid.CellSize.Y * (int)(mouseY / Grid.CellSize.Y));
            };
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

            if (Input.GetMouseButton(Mouse.LeftButton))
            {
                PlaceSingleTile();
            }
        }

        public void Update(float deltaTime)
        {
            cameraPos.Y += dir.Y * moveSpeed * deltaTime;
            cameraPos.X += dir.X * moveSpeed * deltaTime;

            CameraController.Instance.UpdateEditorCamera(cameraPos);
        }

        private void PlaceSingleTile()
        {
            Vector2 matrixPos = new Vector2(actualPos.X / Grid.CellSize.X, actualPos.Y / Grid.CellSize.Y);

            if (Grid.Map[matrixPos.X, matrixPos.Y] == 0)
            {
                EditorRect rect = new EditorRect();
                rect.Rect = new IntRect(actualPos.X, actualPos.Y, Grid.CellSize.X, Grid.CellSize.Y);
                rect.Fill = new SolidColorBrush(Colors.LightBlue);

                Grid.Map[matrixPos.X, matrixPos.Y] = 1;
                rects.Add(new Coordinate(matrixPos.X, matrixPos.Y), rect);
                Rectangles.Add(rect);
            }
        }
    }
}
