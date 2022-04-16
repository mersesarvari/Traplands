using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFGameTest.Helpers;

namespace WPFGameTest.Logic
{
    public sealed class CameraController
    {
        private static readonly CameraController instance = new CameraController();

        private ScrollViewer camera;
        private float shakeDuration = 0.1f;
        private float shakePower = 2f;
        private Random rnd;
        private bool isShaking;
        private float currentShakeTime = 0;

        static CameraController() { }

        private CameraController() { }

        public static CameraController Instance
        {
            get
            {
                return instance;
            }
        }

        public void Init(ScrollViewer camera)
        {
            rnd = new Random();
            this.camera = camera;
        }

        public void Shake()
        {
            isShaking = true;
        }

        public void Shake(float duration, float power)
        {
            Shake();
            shakeDuration = duration;
            shakePower = power;
        }

        public void UpdateCamera(Vector2 position)
        {
            int offsetX = (int)(position.X - WindowController.WIDTH / 2);
            int offsetY = (int)(position.Y - WindowController.HEIGHT / 1.7f);

            if (isShaking)
            {
                if (currentShakeTime < shakeDuration)
                {
                    currentShakeTime += Time.DeltaTime;
                }
                else
                {
                    isShaking = false;
                    currentShakeTime = 0;
                }

                offsetX = (int)(position.X - WindowController.WIDTH / 2 + rnd.NextDouble() * (shakePower * 2) + -shakePower);
                offsetY = (int)(position.Y - WindowController.HEIGHT / 1.7f + rnd.NextDouble() * (shakePower * 2) + -shakePower);
            }

            camera.ScrollToHorizontalOffset(offsetX);
            camera.ScrollToVerticalOffset(offsetY);
        }

        public void UpdateEditorCamera(Vector2f position)
        {
            int offsetX = (int)(position.X - WindowController.WIDTH / 2);
            int offsetY = (int)(position.Y - WindowController.HEIGHT / 2);

            camera.ScrollToHorizontalOffset(offsetX);
            camera.ScrollToVerticalOffset(offsetY);
        }
    }
}
