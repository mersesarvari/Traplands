using System;
using System.IO;
using System.Windows.Media;

namespace WPFGameTest.Renderer
{
    public static class AudioManager
    {
        public static AudioClip backgroundMusic = new AudioClip();
        public static double defaultVolume = 0.04;

        public static void Init()
        {
            backgroundMusic.Volume = defaultVolume;
            backgroundMusic.Looping = true;
        }

        public static void Play()
        {
            backgroundMusic.Play();
        }

        public static void SetBackgroundMusic(string audioSource)
        {
            if (backgroundMusic.Source != new Uri(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Audio\", audioSource), UriKind.Relative))
            {
                backgroundMusic.Source = new Uri(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Audio\", audioSource), UriKind.Relative);
            }
            else
            {
                backgroundMusic.Stop();
            }
            
            backgroundMusic.Volume = defaultVolume;
            backgroundMusic.Play();
        }

        public static void Play(string audioSource)
        {
            AudioClip audioClip = new AudioClip(audioSource);
            audioClip.GetMediaPlayer().MediaEnded += (object sender, EventArgs e) => { audioClip.Stop(); audioClip.Close(); };
            audioClip.Play();
        }
    }

    public class AudioClip
    {
        private readonly MediaPlayer mediaPlayer;

        public bool CanOverlap { get; set; }
        public bool Playing { get; private set; }
        public double Volume { get { return mediaPlayer.Volume; } set { mediaPlayer.Volume = value; } }
        public Uri Source { get { return mediaPlayer.Source; } set { mediaPlayer.Open(value); } }
        public bool Looping
        {
            get { return looping; }
            set 
            {
                if (value)
                {
                    mediaPlayer.MediaEnded += (object sender, EventArgs e) =>
                    {
                        mediaPlayer.Play();
                    };
                }
                else
                {
                    if (Looping)
                    {
                        mediaPlayer.MediaEnded -= (object sender, EventArgs e) =>
                        {
                            mediaPlayer.Play();
                        };
                    }
                }

                looping = value;
            }
        }

        private bool looping;

        public AudioClip(string source)
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += (object sender, EventArgs e) => { Playing = false; mediaPlayer.Position = TimeSpan.Zero; mediaPlayer.Stop(); };
            Source = new Uri(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Audio\", source), UriKind.Relative);
            CanOverlap = true;
            Playing = false;
            looping = false;
        }

        public AudioClip()
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += (object sender, EventArgs e) => { Playing = false; mediaPlayer.Position = TimeSpan.Zero; mediaPlayer.Stop(); };
            CanOverlap = true;
            Playing = false;
            looping = false;
        }

        public MediaPlayer GetMediaPlayer()
        {
            return mediaPlayer;
        }

        public void Play()
        {
            if (!CanOverlap && Playing)
            {
                return;
            }

            mediaPlayer.Play();
            Playing = true;
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        public void Close()
        {
            mediaPlayer.Close();
        }
    }
}
