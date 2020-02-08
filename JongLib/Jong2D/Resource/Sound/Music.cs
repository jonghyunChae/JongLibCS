using System;
using SDL2;
namespace Jong2D
{
    public class Music : Jong2D.Sound
    {
        internal Music(IntPtr sound) : base(sound)
        {
        }

        public override void SetVolume(int value)
        {
            SDL_mixer.Mix_VolumeMusic(value);
        }

        public override int GetVolume()
        {
            return SDL_mixer.Mix_VolumeMusic(-1);
        }

        public override void Play(int repeat = 1)
        {
            SDL_mixer.Mix_PlayMusic(this.sound, repeat);
        }

        public override void Close()
        {
            if (this.sound != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeMusic(this.sound);
                this.sound = IntPtr.Zero;
            }
        }
    }

    public static partial class Context
    {
        public static Music LoadMusic(string name)
        {
            IntPtr data = SDL_mixer.Mix_LoadMUS(name);
            if (data == IntPtr.Zero)
            {
                string msg = $"Error Load Music!! {name}";
                Log(msg);
                throw new Exception(msg);
            }

            return new Music(data);
        }
    }
}
