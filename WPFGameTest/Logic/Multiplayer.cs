using Game.Helpers;
using Game.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Game.Logic
{
    public struct InputPayload
    {
        public bool pressingRight;
        public bool pressingUp;
        public bool pressingLeft;
        public bool pressingDash;
    }

    public class Multiplayer : IMultiplayer
    {
        public Player Player { get; set; }
        public List<User> Players { get; set; }
        public List<GameObject> Solids { get; set; }
        public List<GameObject> Interactables { get; set; }

        string localID;

        InputPayload input;

        private Level currentLevel;

        private Vector2 spawnPoint;

        // Fixed tickrate variables
        const int tickRate = 60;
        float timer;
        float minTimeBetweenTicks;

        RenderData renderData;

        public Multiplayer()
        {
            spawnPoint = new Vector2();
            Players = new List<User>();
            Solids = new List<GameObject>();
            Interactables = new List<GameObject>();
            Player = new Player("01", "Player1", spawnPoint, new Vector2(ObjectData.PLAYER_WIDTH, ObjectData.PLAYER_HEIGHT), 8);

            localID = MultiLogic.locals.user.Id;

            minTimeBetweenTicks = 1 / tickRate;

            renderData = new RenderData();

            User user = new User(MultiLogic.locals.user.Id, MultiLogic.locals.user.Username, renderData);
        }

        public void LoadLevel(string levelName)
        {
            currentLevel = LevelManager.GetLevel(levelName);
            currentLevel.Load();

            spawnPoint = currentLevel.SpawnPoint;
            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;
            Player = new Player(MultiLogic.locals.user.Id, MultiLogic.locals.user.Username, spawnPoint, new Vector2(ObjectData.PLAYER_WIDTH, ObjectData.PLAYER_HEIGHT), 8);
            ;
            renderData = new RenderData();
            renderData.Transform = Player.Transform;
            renderData.Fill = Player.Fill;

            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;

            GameObject.SetSolids(Solids);
            GameObject.SetInteractables(Interactables);
            GameObject.SetPlayers(new List<GameObject> { Player });
        }

        public void LoadPlayers(List<User> players)
        {
            Players = players;
        }

        public void UpdatePlayer(User playerToUpdate)
        {
            User player = Players.FirstOrDefault(x => x.Id == playerToUpdate.Id);
            playerToUpdate.RenderData = playerToUpdate.RenderData;
        }

        private void UpdateRenderData()
        {
            renderData.Transform = Player.Transform;
            renderData.Fill = Player.Fill;
        }

        public void ProcessInput()
        {
            if (Input.GetKeyPressed(Key.Escape))
            {
                //Paused = !Paused;
            }

            Player.ProcessInput();
        }

        public void Update(float deltaTime)
        {
            foreach (var obj in Interactables)
            {
                obj.Update(deltaTime);
            }

            Player.Update(deltaTime);

            UpdateRenderData();

            MultiLogic.locals.user.RenderData = renderData;

            var serialized = JsonConvert.SerializeObject(MultiLogic.locals.user);

            MultiLogic.locals.client.SendCommandToServer("MOVE", localID, serialized, false);

            CameraController.Instance.UpdateCamera(Player.Transform.Position);
        }
    }
}
