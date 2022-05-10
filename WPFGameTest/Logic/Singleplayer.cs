using System.Collections.Generic;
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
    public class Singleplayer : ISingleplayer
    {
        public Action OnFinishPointReachedEvent;

        private IMessenger messenger;

        // Game objects
        public Player Player { get; set; }
        public List<GameObject> Solids { get; set; }
        public List<GameObject> Interactables { get; set; }

        private List<Level> campaignLevels;
        private int currentLevelIndex;

        // Level data
        private Vector2 spawnPoint;
        private Level currentLevel;

        // Fixed tickrate variables
        const int tickRate = 60;
        float timer;
        float minTimeBetweenTicks;

        // Game state
        private bool paused;
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

        public bool GameOver { get; set; }

        // Transition between levels
        private Transition transition;
        private bool transitioning;
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
        private Stopwatch levelTimer;
        public float LevelTimer { get { return (float)levelTimer.Elapsed.TotalSeconds; } }

        public Singleplayer(IMessenger messenger)
        {
            this.messenger = messenger;

            campaignLevels = LevelManager.CampaignLevels;

            AudioManager.SetBackgroundMusic("26-Dark Fantasy Studio- Playing in water.wav");

            levelTimer = new Stopwatch();

            minTimeBetweenTicks = 1f / tickRate;

            transition = new Transition();

            transition.OnTransitionMiddle += () => { SetLevel(campaignLevels[currentLevelIndex++].Name); };
            transition.OnTransitionEnd += () => { Transitioning = false; };
        }

        public void SetLevel(string key)
        {
            currentLevel = LevelManager.GetLevel(key);

            currentLevel.Load();

            spawnPoint = currentLevel.SpawnPoint;
            Solids = currentLevel.Solids;
            Interactables = currentLevel.Interactables;
            Player = new Player("01", "Player1", spawnPoint, new Vector2(ObjectData.PLAYER_WIDTH, ObjectData.PLAYER_HEIGHT), 8);

            Player.OnFinishPointReached += OnFinishPointReached;

            GameObject.SetSolids(Solids);
            GameObject.SetInteractables(Interactables);
            GameObject.SetPlayers(new List<GameObject> { Player });

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
            }

            Transitioning = true;
        }

        public void SaveLevel()
        {
            currentLevel.Save();
        }

        public void ProcessInput()
        {
            if (Input.GetKeyPressed(Key.Escape))
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

        public void Update(float deltaTime)
        {
            if (!Paused)
            {
                messenger.Send("Update elapsed time", "LevelTimerUpdate");

                timer += deltaTime;

                while (timer >= minTimeBetweenTicks)
                {
                    timer -= minTimeBetweenTicks;
                }

                //foreach (var obj in Interactables)
                //{
                //    obj.Update(deltaTime);
                //}

                for (int i = Interactables.Count - 1; i > -1; i--)
                {
                    Interactables[i].Update(deltaTime);
                    if (Interactables[i].NeedToRemove)
                    {
                        Interactables.RemoveAt(i);
                    }
                }

                //Interactables.RemoveAll(x => x.NeedToRemove);

                Player.Update(deltaTime);

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
