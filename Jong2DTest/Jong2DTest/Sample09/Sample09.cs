using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Jong2DTest
{
    /* 
        ※ 반드시 최소한 2번 까지는 진행해보고, Sample09-1 코드를 확인해보세요.
        (1번 처리한 내용이 들어가있습니다. 직접 해보세요!!)

        과제 : 
        1. 새로 만든 BoundingBox 클래스의 내용을 분석해보자
        2. 소년이 공에 닿으면, 공을 없애보자!
        3. 큰 공을 새로 만들어, 작은 공과 다른 BoundingBox를 가지도록 해보고 충돌체크를 테스트 해보자
        3-1. 큰 공과 작은 공을 하나의 동일 클래스(Ball)을 사용한 상태에서, 이미지와 BoundingBox만 다르게 유지해보자
        4. 원 충돌체크를 해보자
        5. 공이 모두 사라지면, 이제 이미지는 필요없다! 이미지 리소스를 해제해보자!
        6. 2번까지 진행하거나, 모두 완료했으면 Sample09-1을 확인해보자
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

            GameObjects.Add(new Text(100, 300, "Sample9")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(20, 80));

            Random r = new Random();
            foreach(var i in Enumerable.Range(0, 30))
            {
                GameObjects.Add(new Ball(r.Next(50, 750), 100));
            }

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
