using System;
using System.Collections.Generic;
using Game.Helpers;
using Game.Models;

namespace Game.Logic
{
    public class Waypoint
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public Vector2 Position { get; set; }
        public Waypoint Next { get; set; }
        public Waypoint Prev { get; set; }

        public Waypoint(int id, int groupId, Vector2 position)
        {
            ID = id;
            GroupID = groupId;
            Position = position;
        }
    }

    public class MovingTrap : DynamicObject
    {
        private static int groupID = 0;

        public int MoveSpeed { get; set; }
        public int GroupID { get; private set; }
        public List<Waypoint> Waypoints { get; set; }

        private Action OnWaypointReached;
        private int xDir, yDir;

        Waypoint currentWp;

        bool goingForward;

        public MovingTrap(Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size, hitboxOffset)
        {
            // Basic information
            Tag = "Trap";
            GroupID = groupID++;
            MoveSpeed = 100;
            goingForward = true;

            // Fire action when reaching next waypoint
            // Sets up the next location to go to
            OnWaypointReached += () =>
            {
                if (goingForward)
                {
                    if (currentWp.Next != null)
                    {
                        currentWp = currentWp.Next;
                    }
                    else
                    {
                        if (currentWp.Prev != null)
                        {
                            currentWp = currentWp.Prev;
                            goingForward = false;
                        }
                    }
                }
                else
                {
                    if (currentWp.Prev != null)
                    {
                        currentWp = currentWp.Prev;
                    }
                    else
                    {
                        if (currentWp.Next != null)
                        {
                            currentWp = currentWp.Next;
                            goingForward = true;
                        }
                    }
                }
            };
        }

        public void SetWaypoints(List<Waypoint> waypoints, bool loopBack = false)
        {
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                waypoints[i].Next = waypoints[i + 1];
                waypoints[i + 1].Prev = waypoints[i];
            }

            if (loopBack)
            {
                waypoints[0].Prev = waypoints[waypoints.Count - 1];
                waypoints[waypoints.Count - 1].Next = waypoints[0];
            }

            this.Waypoints = waypoints;
            currentWp = waypoints[0];
        }

        public void AddWaypoint(Vector2 position)
        {
            Waypoint wp = new Waypoint(Waypoints.Count, GroupID, position);

            Waypoint prev = Waypoints[Waypoints.Count - 1];
            prev.Next = wp;
            wp.Prev = prev;

            Waypoints.Add(wp);
        }

        public override void Update(float deltaTime)
        {
            Move(currentWp, deltaTime);
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
                    if (Transform.Position.X != currentWp.Position.X)
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
                    if (Transform.Position.Y != currentWp.Position.Y)
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
