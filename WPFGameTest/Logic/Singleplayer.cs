using System.Collections.Generic;
using WPFGameTest.Models;
using WPFGameTest.Helpers;
using WPFGameTest.Renderer;
using System.Windows.Media;

namespace WPFGameTest.Logic
{
    public class Singleplayer : ISingleplayer
    {
        // Game objects
        public Player Player { get; set; }
        public List<GameObject> Solids { get; set; }
        public List<GameObject> Interactables { get; set; }

        // Level data
        private Vector2 spawnPoint;
        private Level currentLevel;

        // Fixed tickrate variables
        const int tickRate = 60;
        float timer;
        float minTimeBetweenTicks;

        // Game state
        public bool Paused { get; set; }

        MovingTrap mt;
        MovingTrap mt2;

        public Singleplayer()
        {
            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");

            minTimeBetweenTicks = 1f / tickRate;

            LevelManager.LoadLevels();

            // Get map (or null)
            currentLevel = LevelManager.GetLevel("tests");

            // Load entities
            if (currentLevel is not null)
            {
                currentLevel.Load();

                spawnPoint = currentLevel.SpawnPoint;
                Solids = currentLevel.Solids;
                Interactables = currentLevel.Interactables;
                Player = new Player("0", "Player1", spawnPoint, new Vector2(44, 44), 8);
            }
            else
            {
                Solids = new List<GameObject>();
                Interactables = new List<GameObject>();
                Player = new Player("0", "Player1", new Vector2(0, 0), new Vector2(44, 44), 8);
                Solids.Add(new SolidObject(new Vector2(0, 500), new Vector2(1000, 500)));
            }

            mt = new MovingTrap(new Vector2(100, 400), new Vector2(44, 44));
            mt.Fill = new ImageBrush(Resource.GetImage("SpikeSingle"));
            mt.Tag = "Trap";

            mt.AddWaypoint(new Vector2(200, 400));
            mt.AddWaypoint(new Vector2(200, 444));
            mt.AddWaypoint(new Vector2(100, 444));
            mt.AddWaypoint(new Vector2(100, 400));

            Interactables.Add(mt);

            mt2 = new MovingTrap(new Vector2(200, 400), new Vector2(44, 44));
            mt2.Tag = "Trap";

            mt2.AddWaypoint(new Vector2(300, 400));
            mt2.AddWaypoint(new Vector2(300, 444));
            mt2.AddWaypoint(new Vector2(200, 500));
            mt2.AddWaypoint(new Vector2(200, 300));

            Interactables.Add(mt2);

            Player.SetSolids(Solids);
            Player.SetInteractables(Interactables);
        }

        public void ProcessInput()
        {
            Player.ProcessInput();
        }

        public void Update(float deltaTime)
        {
            timer += deltaTime;

            while (timer >= minTimeBetweenTicks)
            {
                timer -= minTimeBetweenTicks;
            }

            foreach (var obj in Interactables)
            {
                obj.Update(deltaTime);
            }

            Player.Update(deltaTime);

            CameraController.Instance.UpdateCamera(Player.Transform.Position);
        }
    }
}
