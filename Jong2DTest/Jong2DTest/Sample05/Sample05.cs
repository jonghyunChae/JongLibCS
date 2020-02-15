using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;

namespace Jong2DTest
{
    /* 
      과제 : 각 클래스가 Image를 static으로 유지하는 것에 대해 이유를 생각해보자
      static으로 유지하면 얻는 장/단점을 정리해보자
      static으로 유지하면 안될 경우는 무엇일까?
    */

    public class Program
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
                            {
                                CloseGame = true;
                            }
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

        public static List<IResource> Resources = new List<IResource>();
        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Context.OnClosed += Close;      // 종료 이벤트 등록

            // 리소스 생성
            Music music = Context.LoadMusic(@"Resources\background.mp3");
            Resources.Add(music);

            music.PlayRepeat();

            Text text = new Text(100, 300, "Sample5")
            {
                Color = new Color(100, 25, 25),
            };

            Grass grass = new Grass(Program.SCREEN_WIDTH / 2, 30);
            Character character = new Character(100, 80);

            // 게임 루프
            CloseGame = false;
            while (CloseGame == false)
            {
                // 이벤트 처리
                HandleEvents();

                character.Update();

                Context.ClearWindow();

                grass.Render();
                character.Render();
                text.Render();

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
