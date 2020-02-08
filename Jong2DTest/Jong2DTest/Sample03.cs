using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;
using System.Collections.Generic;

namespace Jong2DTest
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;
        private static bool CloseGame { get; set; }
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
                                CloseGame = true;
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
                            CloseGame = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static List<IResource> Resources = new List<IResource>();
        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Context.OnClosed += Close;      // 종료 이벤트 등록

            // 리소스 생성
            Font font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", 16);
            Image grass = Context.LoadImage(@"Resources\grass.png");
            Image character = Context.LoadImage(@"Resources\run_animation.png");
            Music music = Context.LoadMusic(@"Resources\background.mp3");
            music.PlayRepeat();

            Resources.Add(font);
            Resources.Add(grass);
            Resources.Add(character);
            Resources.Add(music);

            // 게임 루프
            var pos = new Vector2D(100, 80);
            int frame = 0;
            CloseGame = false;
            while (CloseGame == false)
            {
                // 이벤트 처리
                HandleEvents();

                pos.x += 2;

                Context.ClearWindow();

                font.Render(100, 300, "Sample2", new Color(100, 25, 25));
                grass.Render(Program.SCREEN_WIDTH / 2, 30);
                character.ClipRender(new Rectangle(frame * 100, 0, 100, 100), pos);

                frame = (frame + 1) % 8;

                Context.UpdateWindow();

                Context.Delay(0.1);
            }

            Context.CloseWindow();
        }

        static void Close()
        {
            Console.WriteLine("Close!");
            foreach (var resource in Resources)
            {
                resource.Dispose();
            }
        }
    }
}
