using Game.Helpers;
using Game.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class Bird : DynamicObject
    {
        private Animation animActive;
        private Animation animIdle;
        private Animation animFlying;

        private Vector2f Velocity;

        public Bird(Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size, hitboxOffset)
        {
            Tag = "Bird";
            animIdle = new Animation(Resource.GetSpritesheet("bird_idle"), 0.1f);
            animFlying = new Animation(Resource.GetSpritesheet("bird_flying"), 0.1f);
            animActive = animIdle;
            Velocity = new Vector2f();
        }

        public void FlyAway(int dir)
        {
            Tag = "Default";
            Velocity = new Vector2f(dir * 350, RandomGenerator.IntInRange(-250, -150));
            animActive = animFlying;
        }

        public override void Update(float deltaTime)
        {
            animActive.Play(this, deltaTime);

            MoveY(Velocity.Y * deltaTime, null);
            MoveX(Velocity.X * deltaTime, null);
        }
    }
}
