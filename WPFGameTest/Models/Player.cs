using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Game.Helpers;
using Game.Logic;
using Game.Renderer;

namespace Game.Models
{
    public class Player : DynamicObject
    {
        private PlayerState State { get; set; }

        public Action OnFinishPointReached;

        public Vector2 Spawn { get; set; }

        // Player data from the server
        public string PlayerName { get; set; }
        public string PlayerID { get; set; }
        public string PlayerColor { get; set; }

        // Movement attributes
        public int Dir { get; set; }
        public int MoveSpeed { get; set; }
        public int JumpSpeed { get; set; }
        public Vector2f Velocity { get; set; }
        public int MaxDashes { get; set; }
        public int DashesLeft { get; set; }
        public bool WallGrabbing { get; set; }
        public float Acceleration { get; private set; }

        public float CoyoteTime { get; private set; }
        public float CoyoteTimeLeft { get; set; }
        public float MaxJumps { get; private set; }
        public float JumpsLeft { get; set; }
        public bool IsJumping { get; set; }

        // Animations
        public Animation AnimActive { get; set; }

        public Animation AnimIdle { get; private set; }
        public Animation AnimRunning { get; private set; }
        public Animation AnimJumpup { get; private set; }
        public Animation AnimJumpdown { get; private set; }
        public Animation AnimAttack { get; private set; }
        public Animation AnimDash { get; private set; }
        public Animation AnimDeath { get; private set; }

        // Sound effects
        public AudioClip JumpSound { get; private set; }

        private AudioClip attackSound;
        private AudioClip dashSound;
        private AudioClip footstepSound;

        // Dash
        public float DashCooldown { get; private set; }
        public float CooldownLeft { get; set; }

        public Player(string id, string name, string color, Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size, hitboxOffset)
        {
            // Setting up player data from the server
            PlayerID = id;
            PlayerName = name;
            PlayerColor = color;

            State = new PlayerOnGround(this);

            Spawn = new Vector2(position.X, position.Y);

            // Setting up default values
            Tag = "Player";
            Velocity = new Vector2f();
            MoveSpeed = 300;
            JumpSpeed = 700;
            Acceleration = 2000;
            MaxDashes = 1;
            DashesLeft = MaxDashes;
            DashCooldown = 0.5f;
            CooldownLeft = 0;
            CoyoteTime = 0.1f;
            CoyoteTimeLeft = CoyoteTime;
            MaxJumps = 1;
            JumpsLeft = MaxJumps;

            #region Animations
            // Animations + sound effects
            AnimIdle = new Animation(Resource.GetSpritesheet("player_idle"), 0.2f);
            AnimRunning = new Animation(Resource.GetSpritesheet("player_run"), 0.1f);
            AnimJumpup = new Animation(Resource.GetSpritesheet("player_jumpup"), 0.1f);
            AnimJumpdown = new Animation(Resource.GetSpritesheet("player_jumpdown"), 0.1f);
            AnimAttack = new Animation(Resource.GetSpritesheet("player_attack"), 0.1f);
            AnimDash = new Animation(Resource.GetSpritesheet("player_dash"), 0.05f);
            AnimDeath = new Animation(Resource.GetSpritesheet("player_death"), 0.05f);

            AnimActive = AnimIdle;

            JumpSound = new AudioClip("jump_new.wav");
            JumpSound.Volume = 1;

            attackSound = new AudioClip("swoosh.wav");
            attackSound.Volume = 0.25;

            dashSound = new AudioClip("dash.wav");
            dashSound.Volume = 0.5;

            footstepSound = new AudioClip("footstep.wav");
            footstepSound.Volume = 0.3;

            AnimRunning.AddAudio(new AudioClip[] { null, null, footstepSound, null, null, footstepSound });

            // Animation events
            AnimAttack.OnAnimationStart += () => { Dir = 0; attackSound.Play(); };
            AnimAttack.OnAnimationOver += () => { State = new PlayerOnGround(this); };

            AnimDeath.OnAnimationStart += () => { AnimDeath.Reset(); Velocity = new Vector2f(0, 0); CameraController.Instance.Shake(0.1f, 15); };
            AnimDeath.OnAnimationOver += () => { State = new PlayerOnGround(this); Respawn(); MoveSpeed = 400; };

            AnimDash.OnAnimationStart += () =>
            {
                AnimDash.Reset();
                CameraController.Instance.Shake(0.1f, 5);
                Fill.Opacity = 0.5;
                Dir = FacingRight ? 1 : -1;
                MoveSpeed = 1500;
                Velocity.X = Dir * MoveSpeed;
                dashSound.Play();
                // JumpsLeft = MaxJumps; Reset jumps, maybe adding it later
            };

            AnimDash.OnAnimationOver += () =>
            {
                Fill.Opacity = 1;
                MoveSpeed = 400;
                Velocity.X = Dir * MoveSpeed;
                Velocity.Y = 0;
                if (Grounded) State = new PlayerOnGround(this);
                else State = new PlayerInAir(this);
            };
            #endregion

            // Set player state to idle upon reaching the finish
            OnFinishPointReached += () =>
            {
                Velocity.X = Dir * 100;
                Velocity.Y = Velocity.Y > 0 ? Velocity.Y : -Velocity.Y;
                xRemainder = 0;
                yRemainder = 0;
                Dir = 0;
                State = new PlayerOnGround(this);
            };
        }

        public override void MoveY(float amount, Action onCollision)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder, MidpointRounding.ToEven);

            yRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0)
            {
                GameObject obj;
                IntRect tempRect = new IntRect
                {
                    X = Hitbox.X,
                    Y = Hitbox.Y + sign,
                    Width = Hitbox.Width,
                    Height = Hitbox.Height
                };

                if (Physics.IsColliding(interactables, tempRect, out obj))
                {
                    if (obj.Tag == "Trap")
                    {
                        Die();
                    }
                    else if (obj.Tag == "Finish")
                    {
                        OnFinishPointReached?.Invoke();
                    }
                }

                if (!Physics.IsColliding(solids, tempRect, out obj))
                {
                    //  We don't collide with anyting solid
                    Transform.Position.Y += sign;
                    Hitbox.Y += sign;
                    move -= sign;
                    Grounded = false;
                }
                else
                {
                    // Colliding with solid
                    if (sign == 1) // If we collided below us
                    {
                        Grounded = true;
                    }

                    onCollision?.Invoke();
                    break;
                }
            }

            if (Grounded) // If we collided below us check if we are still colliding
            {
                if (WallGrabbing)
                {
                    WallGrabbing = false;
                }

                int belowAmount = 1;

                IntRect tempRect = new IntRect
                {
                    X = Hitbox.X,
                    Y = Hitbox.Y + belowAmount,
                    Width = Hitbox.Width,
                    Height = Hitbox.Height
                };

                if (!Physics.IsColliding(solids, tempRect))
                {
                    Grounded = false;
                }
            }
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
                    GameObject obj;

                    IntRect tempRect = new IntRect
                    {
                        X = Hitbox.X + sign,
                        Y = Hitbox.Y,
                        Width = Hitbox.Width,
                        Height = Hitbox.Height
                    };

                    if (Physics.IsColliding(interactables, tempRect, out obj))
                    {
                        if (obj.Tag == "Trap")
                        {
                            Die();
                        }
                        else if (obj.Tag == "Finish")
                        {
                            // Show window with time and option
                            OnFinishPointReached?.Invoke();
                        }
                    }

                    if (!Physics.IsColliding(solids, tempRect, out obj))
                    {
                        //  We don't collide with anyting solid
                        Transform.Position.X += sign;
                        Hitbox.X += sign;
                        move -= sign;
                    }
                    else
                    {
                        // Colliding with solid
                        onCollision?.Invoke();
                        break;
                    }
                }
            }

            if (xRemainder < 0 || Grounded)
            {
                WallGrabbing = false;
            }

            if (WallGrabbing)
            {
                int sign = FacingRight ? 1 : -1;
                IntRect tempRect = new IntRect
                {
                    X = Hitbox.X + sign,
                    Y = Hitbox.Y,
                    Width = Hitbox.Width,
                    Height = Hitbox.Height
                };

                if (!Physics.IsColliding(solids, tempRect))
                {
                    // We are not colliding with anyting solid
                    WallGrabbing = false;
                }
            }
        }

        public void Die()
        {
            State = new PlayerDead(this);
        }

        public void Respawn()
        {
            SetPosition(new Vector2(Spawn.X, Spawn.Y));
        }

        public void GrabWall()
        {
            if (!Grounded)
            {
                WallGrabbing = true;
            }
        }

        public void SetPosition(Vector2 newPos)
        {
            Transform.Position = newPos;
            Hitbox.X = newPos.X + HitboxOffset;
            Hitbox.Y = newPos.Y;
        }

        public void ProcessInput()
        {
            PlayerState state = State.ProcessInput();

            if (state is not null)
            {
                State = state;
            }
        }

        public override void Update(float deltaTime)
        {
            State.Update(deltaTime);

            if (CooldownLeft > 0)
            {
                CooldownLeft -= Time.DeltaTime;
            }
            else
            {
                CooldownLeft = 0;
            }

            if (AnimActive != null)
            {
                AnimActive.Play(this, deltaTime);
            }

            base.Update(deltaTime);
        }

        //public void LateUpdate()
        //{
        //    GameObject obj;
        //    if (Physics.IsColliding(interactables, Hitbox, out obj))
        //    {
        //        if (obj.Tag == "Trap")
        //        {
        //            Trace.WriteLine("This is called");
        //            Die();
        //            CameraController.Instance.Shake(0.1f, 10);
        //        }
        //    }
        //}
    }
}
