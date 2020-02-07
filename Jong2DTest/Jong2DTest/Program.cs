using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using System.Threading;

namespace Jong2DTest
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 640;
        private const int SCREEN_HEIGHT = 480;

        static void Main(string[] args)
        {
            Context.Open_Canvas(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT, false);
            var font = Context.Load_Font(@"C:\Users\user\source\repos\SDLTest2\SDLTest2\ConsolaMalgun.TTF", 16);
            font.Draw(100, 100, "채종현", 100, 25, 25);

            bool quit = false;
            while (!quit)
            {
                SDL.SDL_Event e;
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    //User requests quit
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quit = true;
                    }

                    Context.Update_Canvas();
                    Thread.Sleep(1);
                }
            }
            Context.Close_Canvas();
        }
    }
}
