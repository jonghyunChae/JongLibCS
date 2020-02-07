using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace Jong2D
{
    public class Context
    {
        public static int canvas_width { get; private set; }
        public static int canvas_height { get; private set; }

        private static IntPtr window { get; set; }
        internal static IntPtr renderer { get; set; }
        private static Font debug_font { get; set; }
        private static bool lattice_on { get; set; }
        public static bool debug_log { get; set; }

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

        public static string GetTitle(double fps)
        {
            string caption = $"Canvas ({canvas_width.ToString()} x {canvas_height.ToString()}) {fps} FPS"; //.encode('UTF-8')
            return caption;
        }

        public static string SDLError => SDL.SDL_GetError();
        public static void Open_Canvas(int width, int height, bool sync = false)
        {
            canvas_width = width;
            canvas_height = height;

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

            if (lattice_on)
            {
                Log("lattice_on");
            }

            Clear_Canvas_Now();

            Log("Open Canvas Success!");
            //debug_font = Load_Font("ConsolaMalgun.TTF", 16);
        }

        public static void Close_Canvas()
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

        public static void Show_Lattice()
        {
            lattice_on = true;
            Clear_Canvas();
            Update_Canvas();
        }

        public static void Hide_Lattice()
        {
            lattice_on = false;
            Clear_Canvas();
            Update_Canvas();
        }

        public static void Clear_Canvas()
        {
            SDL.SDL_SetRenderDrawColor(renderer, 200, 200, 210, 255);
            SDL.SDL_RenderClear(renderer);

            if (lattice_on)
            {
                Console.WriteLine("Clear_Canvas lattice_on");

                SDL.SDL_SetRenderDrawColor(renderer, 180, 180, 180, 255);
                for (int x = 0; x < canvas_width; x += 10)
                {
                    SDL.SDL_RenderDrawLine(renderer, x, 0, x, canvas_height);
                }
                for (int y = canvas_height - 1; y >= 0; y -= 10)
                {
                    SDL.SDL_RenderDrawLine(renderer, 0, y, canvas_width, y);
                }
                SDL.SDL_SetRenderDrawColor(renderer, 160, 160, 160, 255);

                for (int x = 0; x < canvas_width; x += 100)
                {
                    SDL.SDL_RenderDrawLine(renderer, x, 0, x, canvas_height);
                }
                for (int y = canvas_height - 1; y < canvas_width; y -= 100)
                {
                    SDL.SDL_RenderDrawLine(renderer, 0, y, canvas_width, y);
                }
            }
        }

        public static void Clear_Canvas_Now()
        {
            Clear_Canvas();
            Update_Canvas();
            Clear_Canvas();
            Update_Canvas();
        }

        public static void Update_Canvas() => SDL.SDL_RenderPresent(renderer);
        public static void Show_Cursor() => SDL.SDL_ShowCursor(SDL.SDL_ENABLE);
        public static void Hide_Cursor() => SDL.SDL_ShowCursor(SDL.SDL_DISABLE);

        public static Font Load_Font(string name, int size = 20)
        {
            try
            {
                var font = new Font(name, size);
                return font;
            }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }
        }

        private static DateTime CurTime { get; set; }
        public static void Print_FPS()
        {
            var dt = DateTime.UtcNow - CurTime;
            CurTime += dt;

            var caption = GetTitle(1.0 / dt.TotalMilliseconds);
            SDL.SDL_SetWindowTitle(window, caption);
        }

        internal static SDL.SDL_Rect To_SDL_Rect(int x, int y, int width, int height)
        {
            var sdl_rect = new SDL.SDL_Rect();
            sdl_rect.x = x;
            sdl_rect.y = y;
            sdl_rect.w = width;
            sdl_rect.h = height;
            return sdl_rect;
        }

        public string ToUTF8(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            var encoded = Encoding.UTF8.GetString(bytes);
            return encoded;
        }
    }
}
