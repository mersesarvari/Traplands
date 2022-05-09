using System.Collections.Generic;
using Game.Models;
using Game.Helpers;
using Game.Renderer;
using System.Windows.Media;
using System.Diagnostics;

namespace Game.Logic
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

        public Singleplayer()
        {
            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");

            minTimeBetweenTicks = 1f / tickRate;
        }

        public void SetLevel(string key)
        {
            currentLevel = LevelManager.GetLevel(key);

            currentLevel.Load();

            spawnPoint = currentLevel.SpawnPoint;
            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;
            Player = new Player("01", "Player1", spawnPoint, new Vector2(44, 44), 8);

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
