using Jong2D;
using SDL2;
using System;

namespace Jong2DTest.Sample12
{
    public class StartScene : IScene
    {
        public const int SCREEN_WIDTH = 1920;
        public const int SCREEN_HEIGHT = 1080;
        Image background;
        public void Enter()
        {
            Console.WriteLine("Start Scene Enter");
            Context.CreateWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Main");
            background = ResourceFactory.CreateImage("screen", @"Resources\main.jpg");
        }

        public void Exit()
        {
            ResourceFactory.Reset("screen");

            Context.CloseWindow();
            Console.WriteLine("Start Scene Close");
        }

        public void HandleEvents(GameEvent e, double frame_time)
        {
            switch (e.Type)
            {
                // 키보드 처리
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    {
                        if (e.Key == SDL.SDL_Keycode.SDLK_ESCAPE)
                            Framework.Instance.Quit();
                        if (e.Key == SDL.SDL_Keycode.SDLK_RETURN)
                            Framework.Instance.ChangeScene(new MainScene());
                    }
                    break;
                case SDL.SDL_EventType.SDL_QUIT:
                    Framework.Instance.Quit();
                    break;
                default:
                    break;
            }
        }

        public void Pause()
        {
        }

        public void Render()
        {
            background.Render(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
        }

        public void Resume()
        {
        }

        public void Update(double frame_time)
        {
        }
    }
}
