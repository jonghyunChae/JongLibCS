using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;

namespace Jong2DTest
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;

        static void HandleEvents()
        {
            var events = Context.GetGameEvents();
            foreach (GameEvent e in events)
            {
                Console.WriteLine(e.Type);
                switch (e.Type)
                {
                        // 키보드 처리
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        {
                            Console.WriteLine(e.Key);
                            if (e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                            {
                                // 우측 이동
                            }
                            else if (e.Key == SDL.SDL_Keycode.SDLK_LEFT)
                            {
                                // 왼쪽 이동
                            }
                            else if (e.Key == SDL.SDL_Keycode.SDLK_ESCAPE)
                            {
                                // 종료 처리
                            }
                        }
                        break;
                    case SDL.SDL_EventType.SDL_MOUSEMOTION:
                        {
                            int x = e.x;
                            int y = Program.SCREEN_HEIGHT - e.y;
                            Console.WriteLine($"{x}, {y}");
                        }
                        break;
                    case SDL.SDL_EventType.SDL_QUIT:
                        {
                            // 종료 처리
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Font font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", 16);
            Image grass = Context.LoadImage(@"Resources\grass.png");
            Image character = Context.LoadImage(@"Resources\run_animation.png");

            var pos = new Vector2D(100, 80);
            int frame = 0;
            while (true)
            {
                Context.ClearWindow();

                font.Render(100, 300, "Sample2", new Color(100, 25, 25));
                grass.Render(Program.SCREEN_WIDTH / 2, 30);
                character.ClipRender(new Rectangle(frame * 100, 0, 100, 100), pos);

                frame = (frame + 1) % 8;

                Context.UpdateWindow();

                pos.x += 1;
                Context.Delay(0.2);
                //SDL.SDL_Delay(200);
                HandleEvents();
            }

            Context.CloseWindow();
        }
    }
}
