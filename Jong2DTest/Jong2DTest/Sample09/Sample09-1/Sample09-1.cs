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
    과제 : 
    1. 샘플코드 보다 충돌체크의 효율을 높이기 위해 더 좋은 방법을 생각해보고 적용해보자!
       - 충돌체크 횟수를 줄일 수 있는 방법은 없을까?
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

            var collidables = GameObjects
                .Where(x => x is ICollidable)
                .Select(x => x as ICollidable)
                .ToList();

            foreach (var src in collidables)
            {
                foreach (var target in collidables)
                {
                    Collision.AABBCollision(src, target);
                }
            }

            foreach (var remove in RemoveList)
            {
                GameObjects.Remove(remove);
            }
            RemoveList.Clear();
        }

        static List<IGameObject> GameObjects = new List<IGameObject>();
        public static List<IResource> Resources = new List<IResource>();
        public static List<IGameObject> RemoveList = new List<IGameObject>();
        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Context.OnClosed += Close;      // 종료 이벤트 등록

            // 리소스 생성
            Music music = Context.LoadMusic(@"Resources\background.mp3");
            Resources.Add(music);

            music.PlayRepeat();

            GameObjects.Add(new Text(100, 300, "Sample9-1")
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
