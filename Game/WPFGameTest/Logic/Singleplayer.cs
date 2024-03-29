﻿using System.Collections.Generic;
using Game.Models;
using Game.Helpers;
using Game.Renderer;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Windows;
using System.Windows.Input;

namespace Game.Logic
{
    public abstract class GameplayBase : IGameplayBase
    {
        protected IMessenger messenger;

        // Game objects
        public Player Player { get; set; }
        public List<GameObject> Props { get; set; }
        public List<GameObject> Solids { get; set; }
        public List<GameObject> Interactables { get; set; }

        // Fixed tickrate variables
        protected const int tickRate = 50;
        protected float timer;
        protected float minTimeBetweenTicks;

        protected bool paused;
        public bool Paused
        {
            get => paused;
            set
            {
                paused = value;
                messenger.Send("Game state changed", "GamePaused");
                if (value) levelTimer.Stop();
                else levelTimer.Start();
            }
        }

        protected bool gameOver;
        public bool GameOver
        {
            get => gameOver;
            set
            {
                gameOver = value;
                messenger.Send("Game state changed", "GameOver");
                if (value)
                {
                    Paused = value;
                    levelTimer.Stop();
                }
                else levelTimer.Start();
            }
        }

        // Transition between levels
        protected Transition transition;
        protected bool transitioning;
        public bool Transitioning
        {
            get => transitioning;
            set
            {
                transitioning = value;
                messenger.Send("Transitioning", "Transition");
                if (value) levelTimer.Stop();
                else
                {
                    if (!Paused) levelTimer.Start();
                }
            }
        }

        public double TransitionAlpha { get => transition.Alpha; }

        // Timer for level completition time
        protected Stopwatch levelTimer;
        public float LevelTimer { get { return (float)levelTimer.Elapsed.TotalSeconds; } }

        public GameplayBase()
        {
            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");

            levelTimer = new Stopwatch();

            transition = new Transition();

            transition.OnTransitionEnd += () => { Transitioning = false; };

            minTimeBetweenTicks = 1f / tickRate;
        }

        public abstract void ProcessInput();
        public abstract void Update(float deltaTime);
    }

    public class Singleplayer : GameplayBase, ISingleplayer
    {
        public Action OnFinishPointReachedEvent;

        public int DeathCount { get; set; }

        private List<Level> campaignLevels;
        private int currentLevelIndex;

        // Level data
        private Vector2 spawnPoint;
        private Level currentLevel;

        private bool firstLoad;

        public Singleplayer(IMessenger messenger)
        {
            this.messenger = messenger;

            firstLoad = true;

            currentLevelIndex = 0;

            campaignLevels = LevelManager.CampaignLevels;
        }

        public void SetLevel(string key)
        {
            currentLevel = LevelManager.GetLevel(key);

            currentLevel.Load();

            spawnPoint = currentLevel.SpawnPoint;
            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;
            Props = currentLevel.Props;
            Player = new Player("01", "Player1", "", spawnPoint, new Vector2(ObjectData.PLAYER_WIDTH, ObjectData.PLAYER_HEIGHT), 8);
            DeathCount = 0;

            messenger.Send("Player died", "PlayerDied");

            Player.OnDeath += () =>
            {
                DeathCount++;
                messenger.Send("Player died", "PlayerDied");
            };

            if (currentLevel == campaignLevels[0])
            {
                Player.OnFinishPointReached += OnFinishPointReached;

                if (firstLoad)
                {
                    transition.OnTransitionMiddle += () =>
                    {
                        currentLevelIndex++;
                        if (currentLevelIndex >= campaignLevels.Count)
                        {
                            GameOver = true;
                        }
                        else
                        {
                            SetLevel(campaignLevels[currentLevelIndex].Name);
                        }
                    };

                    firstLoad = false;
                }
            }
            else
            {
                Player.OnFinishPointReached += OnFinishPointReached;

                if (firstLoad)
                {
                    transition.OnTransitionMiddle += () =>
                    {
                        GameOver = true;
                    };

                    firstLoad = false;
                }
            }

            GameObject.SetProps(Props);
            GameObject.SetSolids(Solids);
            GameObject.SetInteractables(Interactables);
            GameObject.SetPlayers(new List<GameObject> { Player });

            for (int i = Interactables.Count - 1; i >= 0; i--)
            {
                Interactables[i].Start();
            }

            levelTimer.Restart();
            if (!Paused) levelTimer.Start();
            else levelTimer.Stop();
        }

        public void OnFinishPointReached()
        {
            levelTimer.Stop();
            float newTime = (float)levelTimer.Elapsed.TotalSeconds;

            if (currentLevel.BestTime == 0 || newTime <= currentLevel.BestTime)
            {
                currentLevel.AddNewBestTime((float)levelTimer.Elapsed.TotalSeconds);
                SaveLevel();
            }

            Transitioning = true;
        }

        public void SaveLevel()
        {
            currentLevel.Save();
        }

        public override void ProcessInput()
        {
            if (Input.GetKeyPressed(Key.Escape) && !GameOver)
            {
                Paused = !Paused;
            }

            if (Input.GetKeyPressed(Key.Space))
            {
                Trace.WriteLine(1 / Time.DeltaTime);
            }

            if (!Paused && !Transitioning)
            {
                Player.ProcessInput();
            }
        }

        public override void Update(float deltaTime)
        {
            if (!Paused)
            {
                messenger.Send("Update elapsed time", "LevelTimerUpdate");

                timer += deltaTime;

                foreach (var obj in Interactables)
                {
                    obj.Update(deltaTime);
                }

                Player.Update(deltaTime);

                while (timer >= minTimeBetweenTicks)
                {
                    timer -= minTimeBetweenTicks;
                    Player.LateUpdate();
                }

                CameraController.Instance.UpdateCamera(Player.Transform.Position);
            }

            if (Transitioning)
            {
                transition.Update(deltaTime);
                messenger.Send("Transitioning", "Transition");
            }
        }
    }
}
