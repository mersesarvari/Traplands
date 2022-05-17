using Game.Helpers;
using Game.Models;
using Game.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Logic
{
    public class Spike : DynamicObject
    {
        Animation Anim;

        AudioClip raise;

        bool isActive;

        ImageBrush defaultSprite; 
        ImageBrush spikeOutSprite;

        float spikeOutTime;
        float spikeOutLeft;

        public Spike(Vector2 position, Vector2 size, int hitboxOffset = 0) : base(position, size, hitboxOffset)
        {
            Tag = "Default";
            isActive = false;
            Anim = new Animation(Resource.GetSpritesheet("trap_spikes"), 0.05f);
            raise = new AudioClip("knife-sharpen.wav");
            raise.Volume = 0.1;
            defaultSprite = Resource.GetSpriteSheetImage("trap_spikes", 1);
            spikeOutSprite = Resource.GetSpriteSheetImage("trap_spikes", 4);

            SetDefaultSprite(defaultSprite.ImageSource);

            spikeOutTime = 0.5f;
            spikeOutLeft = spikeOutTime;

            Anim.OnAnimationStart += () =>
            {
                Anim.Reset();
            };

            Anim.OnAnimationOver += () =>
            {
                Fill = defaultSprite;
                isActive = false;
            };

            Anim.OnAnimationImage[2] += () =>
            {
                Tag = "Trap";
                raise.Play();
            };

            Anim.OnAnimationImage[4] += () =>
            {
                isActive = false;
                Fill = spikeOutSprite;
            };

            Anim.OnAnimationImage[5] += () =>
            {
                Tag = "Default";
            };
        }

        public void Activate()
        {
            isActive = true;
        }

        public override void Update(float deltaTime)
        {
            if (!isActive && Fill == spikeOutSprite)
            {
                if (spikeOutLeft > 0)
                {
                    spikeOutLeft -= deltaTime;
                }
                else
                {
                    spikeOutLeft = spikeOutTime;
                    isActive = true;
                }
            }

            if (isActive)
                Anim.Play(this, deltaTime);
        }
    }

    public class TrapBlock : SolidObject
    {
        private float coolDown;
        private float coolDownLeft;
        Spike spike;

        public TrapBlock(Vector2 position, Vector2 size, bool grabbable = true) : base(position, size, grabbable)
        {
            SetDefaultSprite(Resource.GetImage("Stone"));
            coolDown = 2f;
            coolDownLeft = coolDown;

            spike = new Spike(new Vector2(position.X, position.Y - size.Y + 10), size);
        }

        public override void Start()
        {
            interactables.Add(spike);
        }

        public void ActivateSpike()
        {
            spike.Activate();
        }

        public override void Update(float deltaTime)
        {
            if (coolDownLeft > 0)
            {
                coolDownLeft -= deltaTime;
            }
            else
            {
                coolDownLeft = coolDown;
                ActivateSpike();
            }
        }
    }
}
