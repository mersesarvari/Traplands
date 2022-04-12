using System.Diagnostics;
using System.Windows.Input;
using WPFGameTest.Helpers;
using WPFGameTest.Models;

namespace WPFGameTest.Logic
{
    public interface IPlayerState
    {
        public void Enter();
        public PlayerState ProcessInput();
        public void Update(float deltaTime);
        public void Exit();
    }

    public abstract class PlayerState : IPlayerState
    {
        protected Player player;

        public PlayerState(Player player)
        {
            this.player = player;
        }

        public abstract void Enter();
        public abstract PlayerState ProcessInput();
        public abstract void Update(float deltaTime);
        public abstract void Exit();
    }

    public class PlayerDefault : PlayerState
    {
        public PlayerDefault(Player player) : base(player)
        {

        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override PlayerState ProcessInput()
        {
            if (Input.GetKeyPressed(Key.LeftShift))
            {
                if (player.CooldownLeft == 0 && player.DashesLeft > 0)
                {
                    player.DashesLeft--;
                    player.CooldownLeft = player.DashCooldown;

                    return new PlayerDashing(player);
                }
            }

            if (Input.GetKey(Key.A))
            {
                player.Dir = -1;
            }
            else if (Input.GetKey(Key.D))
            {
                player.Dir = 1;
            }
            else
            {
                player.Dir = 0;
            }

            if (player.WallGrabbing && !player.Grounded)
            {
                return new PlayerWallgrabbing(player);
            }

            return null;
        }

        public override void Update(float deltaTime)
        {
            if (player.Dir == 1) // Going right
            {
                if (!player.FacingRight)
                {
                    player.Flip();
                }

                if (player.Velocity.X < 0) // If we were going left before decelerate 2 times faster than accelerate
                {
                    player.Velocity.X += player.Acceleration * 2 * deltaTime;
                }
                else if (player.Velocity.X < player.MoveSpeed) // If we have 0 or positive velocity just accelerate the default way
                {
                    player.Velocity.X += player.Acceleration * deltaTime;
                }
                else
                {
                    player.Velocity.X = player.MoveSpeed;
                }
            }
            else if (player.Dir == -1)
            {
                if (player.FacingRight)
                {
                    player.Flip();
                }

                if (player.Velocity.X > 0)
                {
                    player.Velocity.X -= player.Acceleration * 2 * deltaTime;
                }
                else if (player.Velocity.X > -player.MoveSpeed)
                {
                    player.Velocity.X -= player.Acceleration * deltaTime;
                }
                else
                {
                    player.Velocity.X = -player.MoveSpeed;
                }
            }
            else // If we have no intention of moving just decelerate the velocity
            {
                // Between -20 and 20 horizontal velocity just reset to 0
                // otherwise the character is going to jiggle left and right until it reaches 0
                if (player.Velocity.X < -20)
                {
                    player.Velocity.X += player.Acceleration * 2 * deltaTime;
                }
                else if (player.Velocity.X > 20)
                {
                    player.Velocity.X -= player.Acceleration * 2 * deltaTime;
                }
                else
                {
                    player.Velocity.X = 0;
                }
            }

            player.MoveY(player.Velocity.Y * deltaTime, () => { player.Velocity.Y = 0; });
            player.MoveX(player.Velocity.X * deltaTime, player.GrabWall);
        }
    }

    public class PlayerOnGround : PlayerDefault
    {
        public PlayerOnGround(Player player) : base(player)
        {

        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override PlayerState ProcessInput()
        {
            if (Input.GetKeyPressed(Key.W))
            {
                player.JumpSound.Play();
                player.Velocity.Y = -player.JumpSpeed;
                player.JumpsLeft--;

                return new PlayerInAir(player);
            }

            if (!player.Grounded)
            {
                player.JumpsLeft--;
                return new PlayerInAir(player);
            }

            return base.ProcessInput();
        }

        public override void Update(float deltaTime)
        {
            player.DashesLeft = player.MaxDashes;

            if (player.Dir != 0)
            {
                player.AnimActive = player.AnimRunning;
            }
            else
            {
                player.AnimActive = player.AnimIdle;
            }

            base.Update(deltaTime);
        }
    }

    public class PlayerInAir : PlayerDefault
    {
        public PlayerInAir(Player player) : base(player)
        {

        }

        public override PlayerState ProcessInput()
        {
            if (Input.GetKeyPressed(Key.W))
            {
                if (player.JumpsLeft > 0 || player.CoyoteTimeLeft > 0)
                {
                    player.Velocity.Y = -player.JumpSpeed;
                    player.JumpsLeft--;
                }
            }

            if (Input.GetKey(Key.W))
            {
                player.IsJumping = true;
            }
            else
            {
                player.IsJumping = false;
            }

            if (!player.Grounded)
            {
               return base.ProcessInput();
            }
            else
            {
                player.JumpsLeft = player.MaxJumps;
                player.CoyoteTimeLeft = player.CoyoteTime;

                return new PlayerOnGround(player);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            player.CoyoteTimeLeft -= deltaTime;

            if (player.CoyoteTimeLeft > 0)
            {
                player.CoyoteTimeLeft -= deltaTime;
            }

            if (player.Velocity.Y > 0)
            {
                player.AnimActive = player.AnimJumpdown;
            }
            else
            {
                player.AnimActive = player.AnimJumpup;
            }

            if (player.IsJumping && player.Velocity.Y < 0)
            {
                player.Velocity.Y += Physics.Gravity * deltaTime;
            }
            else
            {
                player.Velocity.Y += Physics.Gravity * 2 * deltaTime;
            }

            if (player.Velocity.Y > Physics.FallClamp)
            {
                player.Velocity.Y = Physics.FallClamp;
            }
        }
    }

    public class PlayerWallgrabbing : PlayerState
    {
        public PlayerWallgrabbing(Player player) : base(player)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override PlayerState ProcessInput()
        {
            if (Input.GetKeyPressed(Key.W))
            {
                player.JumpSound.Play();
                player.Velocity.Y = -player.JumpSpeed;
                player.Velocity.X = player.FacingRight ? -player.MoveSpeed * 1.25f : player.MoveSpeed * 1.25f;

                return new PlayerInAir(player);
            }

            if (player.Grounded || !player.WallGrabbing)
            {
                return new PlayerOnGround(player);
            }

            return null;
        }

        public override void Update(float deltaTime)
        {
            if (player.Velocity.Y < 0)
            {
                player.Velocity.Y += Physics.Gravity * deltaTime;
            }
            else
            {
                player.Velocity.Y = Physics.WallSlideSpeed;
            }

            player.MoveY(player.Velocity.Y * deltaTime, () => { player.Velocity.Y = 0; });
            player.MoveX(0 * deltaTime, null);
        }
    }

    public class PlayerDashing : PlayerState
    {
        public PlayerDashing(Player player) : base(player)
        {

        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override PlayerState ProcessInput()
        {
            return null;
        }

        public override void Update(float deltaTime)
        {
            player.AnimActive = player.AnimDash;
            player.MoveY(0, null);
            player.MoveX(player.Velocity.X * deltaTime, () => { player.Velocity.X = 0; });
        }
    }
}
