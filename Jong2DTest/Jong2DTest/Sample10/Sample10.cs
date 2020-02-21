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
        1. 마우스 위치에 도달할 수 있을지 판단해보고, 도달 가능한 경우에만 공을 발사해보자
        (힌트 : 캐릭터와 마우스 사이의 거리를 구하고 비교 해봅니다.)
        (예1 : 마우스까지 거리가 100이고, 공은 80까지 날아가면 발사하지 않음)
        (예2 : 마우스까지 거리가 100이고, 공은 120까지 날아가면 발사함)
        2. 길이 비교를 할 때는 Length 말고 LengthSquare를 이용하면 더 빠르다. 
           수학 공식을 떠올리면서 Length 대신에 Square를 통해서 길이 비교를 해보자.
        3. 캐릭터의 이동도 방향벡터를 이용해서 구현해보자
        
        그 이외에 법선벡터, 내적, 외적 등을 통해 더 다양한 내용을 처리할 수 있다
    */

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
            foreach (var obj in GameObjects.ToList())
            {
                obj.EventHandle(e, frame_time);
            }
        }

        static void Render()
        {
            Context.ClearWindow();
            foreach (var obj in GameObjects.ToList())
            {
                obj.Render();
            }
            Context.UpdateWindow();
        }

        static void Update(double frame_time)
        {
            foreach (var obj in GameObjects.ToList())
            {
                obj.Update(frame_time);
            }
        }

        public static void AddObject(IGameObject obj)
        {
            GameObjects.Add(obj);
        }

        public static void RemoveObject(IGameObject obj)
        {
            GameObjects.Remove(obj);
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

            GameObjects.Add(new Text(100, 300, "Sample10")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(20, 80));

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
