using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFGameTest.Renderer
{
    public class ResourceManager<T> where T : class
    {
        private Dictionary<string, T> resources;

        public ResourceManager()
        {
            resources = new Dictionary<string, T>();
        }

        public void Add(string key, T value)
        {
            resources.Add(key, value);
        }

        public T Get(string key)
        {
            T resource = default(T);
            resources.TryGetValue(key, out resource);

            return resource;
        }
    }

    public static class Resource
    {
        private static ResourceManager<ImageSource> images = new ResourceManager<ImageSource>();
        private static ResourceManager<AudioClip> sounds = new ResourceManager<AudioClip>();

        public static void AddImage(string key, string fileName)
        {
            ImageSource imageSource = new BitmapImage(new Uri(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"Graphics\", fileName), UriKind.Relative));
            images.Add(key, imageSource);
        }

        public static ImageSource GetImage(string key)
        {
            return images.Get(key);
        }

        public static void AddSound(string key, AudioClip value)
        {
            sounds.Add(key, value);
        }

        public static AudioClip GetSound(string key)
        {
            return sounds.Get(key);
        }
    }
}
