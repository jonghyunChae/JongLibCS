using Jong2D;
using Jong2D.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jong2DTest.Sample13
{
    public class ResourceAttribute : Attribute
    {
        public string path { get; set; }
        public ResourceAttribute(string resource_path)
        {
            path = resource_path;
        }
    }

    public static class ResourceFactory
    {
        static Dictionary<string, IResource> Resources = new Dictionary<string, IResource>();

        public static void Reset(Type type)
        {
            Reset(type.ToString());

        }

        public static void Reset(string key)
        {
            if (Resources.TryGetValue(key, out IResource value))
            {
                value.Dispose();
                Resources.Remove(key);
            }
        }

        public static void ResetAll()
        {
            foreach (var resource in Resources.Values)
            {
                resource.Dispose();
            }
            Resources.Clear();
        }

        public static Image CreateImage(string key, string path)
        {
            IResource img;
            if (Resources.TryGetValue(key, out img) == false)
            {
                img = Context.LoadImage(path);
                Resources[key] = img;
            }
            return img as Image;
        }

        public static Image CreateImage(IGameObject obj)
        {
            var type = obj.GetType();
            return CreateByAttribute(type, Context.LoadImage) as Image;
        }

        public static Music CreateMusic(string key, string path)
        {
            IResource music;
            if (Resources.TryGetValue(key, out music) == false)
            {
                music = Context.LoadMusic(path);
                Resources[key] = music;
            }
            return music as Music;
        }

        public static Music CreateMusic(IGameObject obj)
        {
            var type = obj.GetType();
            return CreateByAttribute(type, Context.LoadMusic) as Music;
        }

        public static Font CreateFont(string key, string path, int font_size = 20)
        {
            IResource font;
            if (Resources.TryGetValue(key, out font) == false)
            {
                font = Context.LoadFont(path, font_size);
                Resources[key] = font;
            }
            return font as Font;
        }

        public static Font CreateFont(IGameObject obj, int font_size = 20)
        {
            var type = obj.GetType();
            Func<string, IResource> creator = (string path) =>
            {
                return Context.LoadFont(path, font_size);
            };
            return CreateByAttribute(type, creator) as Font;
        }

        private static IResource CreateByAttribute(Type type, Func<string, IResource> creator)
        {
            var key = type.ToString();
            if (Resources.ContainsKey(key) == false)
            {
                var attr = type.GetCustomAttributes(typeof(ResourceAttribute), true).FirstOrDefault();
                if (attr == null)
                {
                    throw new Exception($"{key} is not defined ResourceAttribute");
                }
                var resourceAttr = attr as ResourceAttribute;
                Resources[key] = creator(resourceAttr.path);
            }
            return Resources[key];
        }
    }
}
