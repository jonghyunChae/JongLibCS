using Jong2D;
using Jong2D.Framework;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;

namespace Jong2DTest.Sample13
{
    public partial class MainScene : IScene
    {
        static List<IGameObject> GameObjects = new List<IGameObject>();

        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 480;

        public void Enter()
        {
            Console.WriteLine("Enter Main!");
            Context.CreateWindow(SCREEN_WIDTH, SCREEN_HEIGHT);

            // 리소스 생성
            Music music = ResourceFactory.CreateMusic("bgm", @"Resources\background.mp3");
            music.PlayRepeat();
            BackGround.Instance.Load();

            // 배경은 항상 먼저 그려야한다. 
            GameObjects.Add(BackGround.Instance);
            GameObjects.Add(new Text(100, 300, "Sample13")
            {
                Color = new Color(100, 25, 25),
            });
            GameObjects.Add(new Grass(SCREEN_WIDTH / 2, 30));
            GameObjects.Add(new Boy(BackGround.Instance.Width / 2, 80));
        }

        public void Exit()
        {
            GameObjects.Clear();
            ResourceFactory.ResetAll();
            Console.WriteLine("Close Main!");
        }

        public void HandleEvents(GameEvent e, double frame_time)
        {
            switch (e.Type)
            {
                // 키보드 처리
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    {
                        if (e.Key == SDL.SDL_Keycode.SDLK_ESCAPE)
                            Framework.Quit();
                        if (e.Key == SDL.SDL_Keycode.SDLK_RETURN)
                            Framework.ChangeScene(new StartScene());
                    }
                    break;
                case SDL.SDL_EventType.SDL_QUIT:
                    Framework.Quit();
                    break;
                default:
                    break;
            }

            foreach (var obj in GameObjects)
            {
                obj.EventHandle(e, frame_time);
            }
        }

        public void Pause()
        {
        }

        public void Render()
        {
            foreach (var obj in GameObjects)
            {
                obj.Render();
            }
        }

        public void Resume()
        {
        }

        public void Update(double frame_time)
        {
            foreach (var obj in GameObjects)
            {
                obj.Update(frame_time);
            }
        }
    }


}
