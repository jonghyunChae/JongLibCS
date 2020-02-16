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
        1. 컴퓨터가 느리다고 가정하여, 게임 루프 내에 Thread.Sleep을 다양하게 줘보자 (1초:1000ms 등..)
        2. Sleep이 높은 상태에서 공이 땅을 뚫고 갔을 경우 뚫지 못하도록 수정해보자
        3. 캐릭터가 공이랑 닿을 경우에도 공이 그 자리에서 멈추도록 해보자
        4. 
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

            GameObjects.Add(new Text(100, 300, "Sample9-2")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30, Program.SCREEN_WIDTH, 60));
            GameObjects.Add(new Boy(20, 80));

            Random r = new Random();
            foreach(var i in Enumerable.Range(0, 30))
            {
                GameObjects.Add(new Ball(r.Next(50, 750), r.Next(400, 500)));
            }

            // 게임 루프
            DateTime current_time = DateTime.Now;
            CloseGame = false;
            while (CloseGame == false)
            {
                DateTime now = DateTime.Now;
                double frame_time = (DateTime.Now - current_time).TotalSeconds;
                current_time = now;

                HandleEvents(frame_time);

                Update(frame_time);

                Render();

                //Thread.Sleep(1000);
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
