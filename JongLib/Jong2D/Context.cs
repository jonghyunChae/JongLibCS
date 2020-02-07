using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using SDL2;

namespace Jong2D
{
    public static partial class Context
    {
        public static bool debug_log { get; set; }

        public static string screen_title { get; set; }
        public static int screen_width { get; private set; }
        public static int screen_height { get; private set; }

        private static IntPtr window { get; set; }
        internal static IntPtr renderer { get; set; }
        private static Font debug_font { get; set; }
        private static bool lattice_on { get; set; }

        static Context()
        {
            lattice_on = true;
            debug_log = true;
        }

        public static void Log(string str)
        {
            if (debug_log)
            {
                Console.WriteLine(str);
            }
        }

        public static string GetTitle(int fps)
        {
            string caption = $"{screen_title} ({screen_width.ToString()} x {screen_height.ToString()}) {fps.ToString()} FPS"; //.encode('UTF-8')
            return caption;
        }

        public static string SDLError => SDL.SDL_GetError();
        public static void CreateWindow(int width, int height, string title = "Jong2D", bool sync = false)
        {
            screen_title = title;
            screen_width = width;
            screen_height = height;

            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                throw new Exception($"SDL could not initialize! SDL_Error: { SDLError}");
            }

            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_TIF | SDL_image.IMG_InitFlags.IMG_INIT_WEBP);
            if (SDL_ttf.TTF_Init() == -1)
            {
                throw new Exception($"TTF Init Fail {SDLError}");
            }
                        
            /* 
            Mix_Init(MIX_INIT_MP3 | MIX_INIT_OGG)
            Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, MIX_DEFAULT_CHANNELS, 1024)
            Mix_Volume(-1, 128)
            Mix_VolumeMusic(128)
            */

            window = SDL.SDL_CreateWindow(GetTitle(1000), SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            if (window == IntPtr.Zero)
            {
                throw new Exception("CreateWindow Fail");
            }

            var render_flag = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
            if (sync)
            {
                render_flag = render_flag | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
            }
            renderer = SDL.SDL_CreateRenderer(Context.window, -1, render_flag);
            //SDL_ShowCursor(SDL_DISABLE)

            ClearWindowNow();

            Log("Open screen Success!");
            //debug_font = Load_Font("ConsolaMalgun.TTF", 16);
        }

        public static void CloseWindow()
        {
            /*
              Mix_HaltMusic()
    Mix_HaltChannel(-1)
    Mix_CloseAudio()
    Mix_Quit()
             */

            SDL_ttf.TTF_Quit();
            SDL_image.IMG_Quit();

            SDL.SDL_DestroyWindow(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        public static void ShowLattice()
        {
            lattice_on = true;
            ClearWindow();
            UpdateWindow();
        }

        public static void HideLattice()
        {
            lattice_on = false;
            ClearWindow();
            UpdateWindow();
        }

        public static void ClearWindow()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 200, 200, 210, 255);
            SDL.SDL_RenderClear(renderer);

            if (lattice_on)
            {
                SDL.SDL_SetRenderDrawColor(renderer, 180, 180, 180, 255);

                for (int x = 0; x < screen_width; x += 10)
                {
                    SDL.SDL_RenderDrawLine(renderer, x, 0, x, screen_height);
                }

                for (int y = screen_height - 1; y >= 0; y -= 10)
                {
                    SDL.SDL_RenderDrawLine(renderer, 0, y, screen_width, y);
                }
                SDL.SDL_SetRenderDrawColor(renderer, 160, 160, 160, 255);

                for (int x = 0; x < screen_width; x += 100)
                {
                    SDL.SDL_RenderDrawLine(renderer, x, 0, x, screen_height);
                }

                for (int y = screen_height - 1; y >= 0; y -= 100)
                {
                    SDL.SDL_RenderDrawLine(renderer, 0, y, screen_width, y);
                }
            }
            
        }

        public static void ClearWindowNow()
        {
            ClearWindow();
            UpdateWindow();
            ClearWindow();
            UpdateWindow();
        }

        public static void UpdateWindow() => SDL.SDL_RenderPresent(renderer);
        public static void ShowCursor() => SDL.SDL_ShowCursor(SDL.SDL_ENABLE);
        public static void HideCursor() => SDL.SDL_ShowCursor(SDL.SDL_DISABLE);

        public static void Delay(double sec) => SDL.SDL_Delay((uint)(sec * 1000));

        private static int frame { get; set; }
        private static long lastTick { get; set; }
        public static void PrintFPS()
        {
            var now = DateTime.Now.Ticks;
            var delta = DateTime.Now.Ticks - lastTick;
            if (delta >= TimeSpan.TicksPerSecond)
            {
                var caption = GetTitle(frame);
                SDL.SDL_SetWindowTitle(window, caption);

                frame = 0;
                lastTick = now;
            }

            ++frame;
        }

        public static string ToUTF8(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            var encoded = Encoding.UTF8.GetString(bytes);
            return encoded;
        }
    }
}
