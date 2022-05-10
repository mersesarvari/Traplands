using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Game.Helpers;
using Game.Models;

namespace Game.Renderer
{
    public class SpriteSheet
    {
        public List<Int32Rect> Regions { get; private set; }
        public BitmapImage Image { get; private set; }

        public CroppedBitmap ImageWithIndex(int index)
        {
            return new CroppedBitmap(Image, Regions[index]);
        }

        public SpriteSheet(string fileName, int numOfImages, int imageWidth, int imageHeight)
        {
            Regions = new List<Int32Rect>();
            Image = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Graphics\", fileName), UriKind.Relative));
            for (int i = 0; i < numOfImages; i++)
            {
                Int32Rect r = new Int32Rect(i * imageWidth, 0, imageWidth, imageHeight);
                Regions.Add(r);
            }
        }
    }

    public class Animation
    {
        public delegate void AnimationEvent();
        public event AnimationEvent OnAnimationOver;
        public event AnimationEvent OnAnimationStart;

        public float TimeBetweenImages { get; set; }
        public bool IsOver { get; set; }
        public bool Looping { get; set; }

        private SpriteSheet spriteSheet;
        private AudioClip[] audioClips;

        private int imageIndex = 0;
        private float timeToNewImage = 0;
        private bool hasAudio = false;
        private bool firstFrame = true;


        public Animation(string filePath, int numOfImages, int imageWidth, int imageHeight, float timeBetweenImages)
        {
            spriteSheet = new SpriteSheet(filePath, numOfImages, imageWidth, imageHeight);
            TimeBetweenImages = timeBetweenImages;

            OnAnimationStart += () => { firstFrame = false; };
            OnAnimationOver += () => { IsOver = true; firstFrame = true; };
        }

        public Animation(SpriteSheet spriteSheet, float timeBetweenImages)
        {
            this.spriteSheet = spriteSheet;
            TimeBetweenImages = timeBetweenImages;

            OnAnimationStart += () => { firstFrame = false; };
            OnAnimationOver += () => { IsOver = true; firstFrame = true; };
        }

        public int GetCurrentImageIndex()
        {
            return imageIndex;
        }

        public void AddAudio(AudioClip[] audioClips)
        {
            if (audioClips.Length != spriteSheet.Regions.Count)
            {
                return;
            }

            this.audioClips = audioClips;
            hasAudio = true;
        }

        public void Reset()
        {
            OnAnimationOver?.Invoke();
            imageIndex = 0;
        }

        public void Play(GameObject user, float deltaTime)
        {
            user.Fill = new ImageBrush(new CroppedBitmap(spriteSheet.Image, spriteSheet.Regions[imageIndex]));

            if (imageIndex == 0 && firstFrame)
            {
                OnAnimationStart?.Invoke();
            }

            if (timeToNewImage < TimeBetweenImages)
            {
                timeToNewImage += deltaTime;
            }
            else
            {
                timeToNewImage = 0;

                if (imageIndex == spriteSheet.Regions.Count - 1)
                {
                    imageIndex = 0;
                    OnAnimationOver?.Invoke();
                }
                else
                {
                    imageIndex++;
                }

                if (hasAudio)
                {
                    if (audioClips[imageIndex] != null)
                    {
                        audioClips[imageIndex].Play();
                    }
                }
            }
        }
    }
}
