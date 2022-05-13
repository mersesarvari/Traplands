﻿using Game.Helpers;
using Game.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
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
    public class Multiplayer : GameplayBase, IMultiplayer
    {
        public List<User> Players { get; set; }

        string localID;

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

        public void SetMessenger(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void LoadLevel(string levelName)
        {
            currentLevel = LevelManager.GetLevel(levelName);
            currentLevel.Load();

            spawnPoint = currentLevel.SpawnPoint;
            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;
            Player = new Player(MultiLogic.locals.user.Id, MultiLogic.locals.user.Username, spawnPoint, new Vector2(ObjectData.PLAYER_WIDTH, ObjectData.PLAYER_HEIGHT), 8);
            
            renderData = new RenderData();
            renderData.Position = Player.Transform.Position;
            renderData.Size = Player.Transform.Size;
            renderData.ScaleX = Player.Transform.ScaleTransform.ScaleX;
            renderData.CenterX = Player.Transform.ScaleTransform.CenterX;
            renderData.CenterY = Player.Transform.ScaleTransform.CenterY;
            renderData.ImageIndex = Player.AnimActive.GetCurrentImageIndex();
            renderData.FileName = Player.AnimActive.SpritesheetName;

            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;

            GameObject.SetSolids(Solids);
            GameObject.SetInteractables(Interactables);
            GameObject.SetPlayers(new List<GameObject> { Player });
        }

        public void LoadPlayers(List<User> players)
        {
            if (!Paused && messenger != null)
            {
                messenger.Send("Update elapsed time", "LevelTimerUpdate");
            }

            foreach (User user in players)
            {
                if (user.Id == localID)
                {
                    continue;
                }
                else
                {
                    Players.Add(user);
                }
            }
        }

        public void NotifyPlayerLeft(string username)
        {
            User player = Players.FirstOrDefault(x => x.Username == username);
            Players.Remove(player);
        }

        public void UpdatePlayer(User playerToUpdate)
        {
            User player = Players.FirstOrDefault(x => x.Id == playerToUpdate.Id);
            if (player != null)
                playerToUpdate.RenderData = playerToUpdate.RenderData;
        }

        private void UpdateRenderData()
        {
            renderData.Position = Player.Transform.Position;
            renderData.Size = Player.Transform.Size;
            renderData.ScaleX = Player.Transform.ScaleTransform.ScaleX;
            renderData.CenterX = Player.Transform.ScaleTransform.CenterX;
            renderData.CenterY = Player.Transform.ScaleTransform.CenterY;
            renderData.ImageIndex = Player.AnimActive.GetCurrentImageIndex();
            renderData.FileName = Player.AnimActive.SpritesheetName;
        }

        public override void ProcessInput()
        {
            if (Input.GetKeyPressed(Key.Escape))
            {
                Paused = !Paused;
            }

            Player.ProcessInput();
        }

        public override void Update(float deltaTime)
        {
            foreach (var obj in Interactables)
            {
                obj.Update(deltaTime);
            }

            Player.Update(deltaTime);

            UpdateRenderData();

            MultiLogic.locals.user.RenderData = renderData;

            var serialized = JsonConvert.SerializeObject(MultiLogic.locals.user);
            
            MultiLogic.locals.client.SendCommandToServer("MOVE", localID, serialized);

            CameraController.Instance.UpdateCamera(Player.Transform.Position);
        }
    }
}
