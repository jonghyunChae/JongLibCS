using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Jong2DTest
{
    class Program
    {
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 480;
        private static bool CloseGame { get; set; }
        static void HandleEvents(double frame_time)
        {
            var events = Context.GetGameEvents();
            foreach (GameEvent e in events)
            {
                switch (e.Type)
                {
                    // 키보드 처리
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        {
                            if (e.Key == SDL.SDL_Keycode.SDLK_ESCAPE)
                                CloseGame = true;
                        }
                        break;
                    case SDL.SDL_EventType.SDL_QUIT:
                        CloseGame = true;
                        break;
                    default:
                        break;
                }
            }

            foreach (GameEvent e in events)
            {
                EventHandle(e, frame_time);
            }
        }

        static void EventHandle(GameEvent e, double frame_time)
        {
            foreach (var obj in GameObjects)
            {
                obj.EventHandle(e, frame_time);
            }
        }

        static void Render()
        {
            Context.ClearWindow();
            foreach (var obj in GameObjects)
            {
                obj.Render();
            }
            Context.UpdateWindow();
        }

        static void Update(double frame_time)
        {
            foreach (var obj in GameObjects)
            {
                obj.Update(frame_time);
            }
        }

        static List<IGameObject> GameObjects = new List<IGameObject>();
        public static List<IResource> Resources = new List<IResource>();
        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Context.OnClosed += Close;      // 종료 이벤트 등록

            // 리소스 생성
            Music music = Context.LoadMusic(@"Resources\background.mp3");
            Resources.Add(music);

            music.PlayRepeat();

            // 배경은 항상 먼저 그려야한다. 
            GameObjects.Add(BackGround.Instance);
            GameObjects.Add(new Text(100, 300, "Sample11")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(BackGround.Width/ 2, 80));

            // 게임 루프
            DateTime current_time = DateTime.Now;
            CloseGame = false;
            while (CloseGame == false)
            {
                DateTime now = DateTime.Now;
                double frame_time = (now - current_time).TotalSeconds;
                if (frame_time <= 0)
                {
                    continue;
                }
                current_time = now;

                HandleEvents(frame_time);

                Update(frame_time);

                Render();
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
