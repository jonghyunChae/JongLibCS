using System;
using SDL2;
namespace Jong2D
{
    public abstract class Sound : Resource
    {
        protected IntPtr sound { get; set; }
        internal Sound(IntPtr sound)
        {
            this.sound = sound;
        }

        public void Stop()
        {
            SDL_mixer.Mix_HaltMusic();
        }

        public void Pause()
        {
            SDL_mixer.Mix_PauseMusic();
        }

        public void Resume()
        {
            SDL_mixer.Mix_ResumeMusic();
        }

        public abstract void Play(int repeat = 1);
        public void PlayRepeat()
        {
            this.Play(-1);
        }

        public abstract void SetVolume(int value);
        public abstract int GetVolume();
    }
}
