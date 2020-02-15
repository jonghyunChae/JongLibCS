using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Jong2DTest
{
    /* 
        과제 :
        1. 왼쪽 끝부터 오른쪽 끝까지 가는데, 10초 걸리도록 설계해보자
        2. 애니메이션 frame을 모두 수행하는데 걸리는 시간을 다양하게 바꿔보자
    */

    class Program
    {
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;
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
                if (obj is IControllable)
                {
                    var target = obj as IControllable;
                    target.EventHandle(e, frame_time);
                }
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

            GameObjects.Add(new Text(100, 300, "Sample8-2")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(150, 80));

            // 게임 루프
            DateTime current_time = DateTime.Now;
            CloseGame = false;
            while (CloseGame == false)
            {
                DateTime now = DateTime.Now;
                double frame_time = (DateTime.Now - current_time).TotalSeconds;
                double frame_rate = 1.0 / frame_time;
                //Console.WriteLine(frame_rate.ToString("0.00000#") + " fps \t " + frame_time.ToString("0.00000#") + " sec");
                current_time = now;

                // 거리 = 경과시간 * 속도
                // 위치 = 초기 위치 + 거리
                // 다음 프레임 = 현재 프레임 + v△t (v: 속도, △t : 시간 차이)
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
