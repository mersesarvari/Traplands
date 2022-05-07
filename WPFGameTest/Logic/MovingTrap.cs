using System;
using System.Collections.Generic;
using Game.Helpers;
using Game.Models;

namespace Game.Logic
{
    public class MovingTrap : DynamicObject
    {
        public class Waypoint
        {
            public int ID { get; private set; }
            public int GroupID { get; private set; }
            public Vector2 Position { get; set; }

            public Waypoint(int id, int groupId, Vector2 position)
            {
                ID = id;
                GroupID = groupId;
                Position = position;
            }
        }

        private static int groupID = 0;

        public int MoveSpeed { get; set; }
        public int GroupID { get; private set; }
        public List<Waypoint> Waypoints { get; set; }

        private Action OnWaypointReached;
        private int nextWaypoint;
        private bool goingForward;
        private int xDir, yDir;

        public MovingTrap(Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size, hitboxOffset)
        {
            // Basic information
            GroupID = groupID++;
            MoveSpeed = 100;
            goingForward = true;

            // Set up first waypoint on the created location
            Waypoints = new List<Waypoint>();
            Waypoints.Add(new Waypoint(Waypoints.Count, GroupID, position));

            // Fire action when reaching next waypoint
            // Sets up the next location to go to
            OnWaypointReached += () =>
            {
                if (nextWaypoint == Waypoints.Count - 1)
                {
                    goingForward = false;
                }
                else if (nextWaypoint == 0)
                {
                    goingForward = true;
                }

                nextWaypoint += goingForward ? 1 : -1;
            };
        }

        public void AddWaypoint(Vector2 position)
        {
            Waypoints.Add(new Waypoint(Waypoints.Count, GroupID, position));
        }

        public override void Update(float deltaTime)
        {
            if (Waypoints.Count > 1) Move(Waypoints[nextWaypoint], deltaTime);
        }

        public override void MoveX(float amount, Action onCollision)
        {
            xRemainder += amount;
            int move = (int)Math.Round(xRemainder, MidpointRounding.ToEven);

            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (Transform.Position.X != Waypoints[nextWaypoint].Position.X)
                    {
                        Transform.Position.X += sign;
                        Hitbox.X += sign;
                        move -= sign;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public override void MoveY(float amount, Action onCollision)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder, MidpointRounding.ToEven);

            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign(move);

                while (move != 0)
                {
                    if (Transform.Position.Y != Waypoints[nextWaypoint].Position.Y)
                    {
                        Transform.Position.Y += sign;
                        Hitbox.Y += sign;
                        move -= sign;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void Move(Waypoint next, float deltaTime)
        {
            if (Transform.Position.X > next.Position.X)
            {
                xDir = -1;
            }
            else if (Transform.Position.X < next.Position.X)
            {
                xDir = 1;
            }
            else
            {
                xDir = 0;
            }

            if (Transform.Position.Y > next.Position.Y)
            {
                yDir = -1;
            }
            else if (Transform.Position.Y < next.Position.Y)
            {
                yDir = 1;
            }
            else
            {
                yDir = 0;
            }

            if (xDir == 0 && yDir == 0)
            {
                OnWaypointReached?.Invoke();
            }

            MoveY(MoveSpeed * yDir * deltaTime, null);
            MoveX(MoveSpeed * xDir * deltaTime, null);
        }
    }
}
