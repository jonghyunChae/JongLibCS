﻿using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;

namespace Jong2DTest
{
    /* 
        과제 : state를 멋지게 짜는 방법은 없을까?
        유한 상태 기계 : FSM(Finite State Machine)을 공부해보자
        state pattern은 결국 계속 복잡해지면 한계가 드러난다.
        위와 같은 한계를 해결하기 위해, 나온 다른 방법은 무엇인지 찾아보자
        그리고 state와 장/단점을 분석해보자
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
                            if (e.Key == SDL.SDL_Keycode.SDLK_RETURN)
                            {
                                // 앤터 입력. 
                                // 저장해보자
                            }

                            if (e.Key == SDL.SDL_Keycode.SDLK_SPACE)
                            {
                                // 스페이스바 입력
                                // 로드해보자
                            }
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

            GameObjects.Add(new Text(100, 300, "Sample7")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(Program.SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(150, 80));

            // 게임 루프
            CloseGame = false;
            while (CloseGame == false)
            {
                HandleEvents();

                Update();

                Render();

                Context.Delay(0.05);
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
