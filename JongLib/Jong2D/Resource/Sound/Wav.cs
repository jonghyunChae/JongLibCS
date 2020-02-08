using System;
using SDL2;

namespace Jong2D
{
    public class Wav : Jong2D.Sound
    {
        internal Wav(IntPtr sound) : base(sound)
        {
        }

        public override void SetVolume(int value)
        {
            SDL_mixer.Mix_VolumeChunk(this.sound, value);
        }

        public override int GetVolume()
        {
            return SDL_mixer.Mix_VolumeChunk(this.sound, -1);
        }

        public override void Play(int repeat = 1)
        {
            int n = -1; 
            if (repeat > 0)
            {
                n = repeat - 1;
            }

            SDL_mixer.Mix_PlayChannel(-1, this.sound, n);
        }

        public override void Close()
        {
            if (this.sound != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeChunk(this.sound);
                this.sound = IntPtr.Zero;
            }
        }
    }

    public static partial class Context
    {
        public static Wav LoadWav(string name)
        {
            IntPtr data = SDL_mixer.Mix_LoadWAV(name);
            if (data == IntPtr.Zero)
            {
                string msg = $"Error Load Wav!! {name}";
                Log(msg);
                throw new Exception(msg);
            }

            return new Wav(data);
        }
    }
}
