using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class Transition
    {
        public Action OnTransitionMiddle;
        public Action OnTransitionEnd;

        public double Alpha { get; set; }

        private float durationLeft;
        private float Duration { get; set; }

        private bool fadeIn = true;

        public Transition()
        {
            Duration = 0.5f;
            durationLeft = Duration;

            OnTransitionMiddle += () =>
            {
                fadeIn = false;
                durationLeft = Duration;
                Alpha = 1;
            };

            OnTransitionEnd += () =>
            {
                fadeIn = true;
                durationLeft = Duration;
                Alpha = 0;
            };

            Alpha = 0;
        }

        public void Update(float deltaTime)
        {
            durationLeft -= deltaTime;
            float ratio = durationLeft / Duration;

            if (fadeIn)
            {
                Alpha = (1 - ratio);
            }
            else
            {
                Alpha = ratio;
            }

            if (durationLeft <= 0)
            {
                if (fadeIn)
                {
                    OnTransitionMiddle?.Invoke();
                }
                else
                {
                    OnTransitionEnd?.Invoke();
                }
            }
        }
    }
}
