using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;

namespace Jong2DTest
{
    /* 
    과제 :
    1. 특정 키입력을 하면, 점프를 넣어보자
    2. 캐릭터 말고, 다른 Boy들은 랜덤으로 스스로 움직이게 해보자
    3. 다음과 같은 내용에 대해 생각해보자
    - 상속 구조를 사용하면 장/단점이 무엇일까?
    - GameObjects에 모든 객체에 담으면 장/단점이 무엇일까? 
    */

    class Program
    {
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;
        private static bool CloseGame { get; set; }
        static void HandleEvents()
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
                EventHandle(e);
            }
        }

        static void EventHandle(GameEvent e)
        {
            foreach (var obj in GameObjects)
            {
                if (obj is IControllable)
                {
                    var target = obj as IControllable;
                    target.EventHandle(e);
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

        static void Update()
        {
            foreach (var obj in GameObjects)
            {
                obj.Update();
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

            GameObjects.Add(new Text(100, 300, "Sample6")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Character(100, 80));

            var random = new Random();
            for (int i = 0; i < 10; ++i)
            {
                GameObjects.Add(new Boy(random.Next(50, Program.SCREEN_WIDTH - 100), random.Next(150, Program.SCREEN_HEIGHT - 100)));
            }

            // 게임 루프
            CloseGame = false;
            while (CloseGame == false)
            {
                HandleEvents();

                Update();

                Render();

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
