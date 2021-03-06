﻿using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;
using System.Collections.Generic;

namespace Jong2DTest
{
     interface IGameObject
    {
        void Render();
        void Update(double frame_time);
        void EventHandle(GameEvent e, double frame_time);
    }

    public class Camera
    {
        Vector2D debug_pos = new Vector2D(0, 0);
        public Vector2D Pos;
        public void SetCamera(ref Vector2D pos)
        {
            // 그림을 그리기 위해서 좌표값을 계속 순환시켜야 합니다 (overflow, underflow에 대한 처리)
            Pos.x = (int)pos.x % BackGround.Width;
            if (Pos.x < 0)
            {
                int n = (int)(-Pos.x / BackGround.Width) + 1;
                Pos.x += n * BackGround.Width;

                // 결국 이 의미
                //while (Pos.x < 0)
                //{
                //    Pos.x += BackGround.WorldWidth;
                //}
            }

            Pos.y = (int)pos.y % BackGround.Height;
            if (Pos.y < 0)
            {
                int n = (int)(-Pos.y / BackGround.Height) + 1;
                Pos.y += n * BackGround.Height;
            }

            if (debug_pos.x != Pos.x || debug_pos.y != Pos.y)
            {
                Console.WriteLine(Pos + " / " + pos);
                debug_pos = Pos;
            }
        }

        // 스크린 좌표로 변환해줍니다.
        public Vector2D ToScreenPos(ref Vector2D pos)
        {
            return new Vector2D(Program.SCREEN_WIDTH / 2, pos.y);
        }
    }

    public class BackGround : IGameObject
    {
        public static int Width => BackGround.image.width;
        public static int Height => BackGround.image.height;

        static Image image;
        public Camera Camera = new Camera();

        static BackGround()
        {
            BackGround.image = Context.LoadImage(@"Resources\scroll_background.png");
            Program.Resources.Add(BackGround.image);
        }

        private static BackGround instance;
        public static BackGround Instance
        {
            get
            {
                if (BackGround.instance == null)
                {
                    BackGround.instance = new BackGround();
                }
                return BackGround.instance;
            }
        }

        private BackGround()
        {
            Camera.Pos = new Vector2D(Width / 2, Height / 2);
        }

        public void Render()
        {
            int x = (int)Camera.Pos.x;
            int w = (int)Math.Min(Width - x, Program.SCREEN_WIDTH);

            // 가장 좌측에 그릴 이미지 하나를 고릅니다.
            // 1. 화면 전체 크기보다 우측에 이미지가 더 길 경우 -> 화면 전체 크기의 이미지를 하나 그림
            // 2. 우측에 이미지가 얼마 안남았을 경우 -> 남은 이미지만 먼저 왼쪽에 그림
            var r1 = new Rectangle(x, 0, w, Program.SCREEN_HEIGHT);
            BackGround.image.ClipRenderToOrigin(r1, 0, 0);

            // r1을 그리고 남은 것을 실제 이미지의 왼쪽에서 잘라와서 우측에 이어서 붙여넣습니다.
            // 1. r1의 크기가 스크린 전체 크기라면 -> 결국 길이가 0이라서 안그려줍니다
            // 2. 1번이 아니라면 남은 크기만큼 왼쪽 0,0부터 가져와서 그립니다.
            var r2 = new Rectangle(0, 0, Program.SCREEN_WIDTH - w, Program.SCREEN_HEIGHT);
            BackGround.image.ClipRenderToOrigin(r2, w, 0);
            //Console.WriteLine($"r1 : {r1} /  r2 : {r2} / w : {w}");
        }

        public void Update(double frame_time) { }
        public void EventHandle(GameEvent e, double frame_time) { }
    }

    public class Grass : IGameObject
    {
        static Image image;
        Vector2D pos;
        static Grass()
        {
            Grass.image = Context.LoadImage(@"Resources\grass.png");
            Program.Resources.Add(Grass.image);
        }

        public Grass(int x, int y)
        {
            pos = new Vector2D(x, y);
        }

        public void Render()
        {
            Grass.image.Render(pos.x, pos.y);
        }

        public void Update(double frame_time) { }
        public void EventHandle(GameEvent e, double frame_time) { }
    }

    public class Text : IGameObject
    {
        private Font font;
        private Vector2D pos;

        public string Content { get; set; }
        public Color Color = new Color();

        public Text(int x, int y, string content, int size = 20)
        {
            pos = new Vector2D(x, y);
            font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", size);
            Program.Resources.Add(font);

            Content = content;
        }

        public void Render()
        {
            font.Render(pos.x, pos.y, Content, Color);
        }

        public void Update(double frame_time) { }
        public void EventHandle(GameEvent e, double frame_time) {}
    }

    public class Boy : IGameObject
    {
        static Image image;
        public Vector2D Pos;                    // 가상 세계의 위치

        private Rectangle imageFrame = new Rectangle(0, 0, 100, 100);
        int frame { get; set; }
        double total_frame { get; set; }
        int dir { get; set; }

        const double RUN_SPEED_PPS = 100; // 1초에 100을 옮긴다고 가정하자

        const double TIME_PER_ACTION = 2.0; // 액션을 하는데 총 소비할 시간 (초)
        const double ACTION_PER_TIME = 1.0 / TIME_PER_ACTION;   // 초당 액션 수
        const int FRAME_PER_ACTION = 8;     // 총 액션 수 (8개 프레임)

        enum STATE
        {
            LEFT_RUN,
            RIGHT_RUN,

            STATE_MAX,
        }

        STATE state { get; set; }
        Dictionary<STATE, Action> stateHandlers { get; set; }

        static Boy()
        {
            Boy.image = Context.LoadImage(@"Resources\animation_sheet.png");
            Program.Resources.Add(Boy.image);
        }

        public Boy(int x, int y)
        {
            Pos = new Vector2D(x, y);
            state = STATE.RIGHT_RUN;
            dir = 0;

            stateHandlers = new Dictionary<STATE, Action>();
            stateHandlers[STATE.LEFT_RUN] = LeftRun;
            stateHandlers[STATE.RIGHT_RUN] = RightRun;
        }

        public void Render()
        {
            imageFrame.x = frame * 100;
            imageFrame.y = ((int)state) * 100;
            Vector2D screenPos = BackGround.Instance.Camera.ToScreenPos(ref this.Pos);
            Boy.image.ClipRender(this.imageFrame, screenPos);
        }

        public virtual void Update(double frame_time)
        {
            updateFrame(frame_time);
            updatePos(frame_time);
            stateHandlers[state]();

            BackGround.Instance.Camera.SetCamera(ref Pos);
        }

        void updateFrame(double frame_time)
        {
            total_frame += FRAME_PER_ACTION * ACTION_PER_TIME * frame_time;
            frame = ((int)total_frame) % 8;
        }

        void updatePos(double frame_time)
        {
            double distance = RUN_SPEED_PPS * frame_time;
            double x = Pos.x + dir * distance;
            Pos.x = x;
        }

        void LeftRun()
        {
        }

        void RightRun()
        {
        }

        public void EventHandle(GameEvent e, double frame_time)
        {
            switch (e.Type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    {
                        if (e.Key == SDL.SDL_Keycode.SDLK_LEFT)
                        {
                            dir = -1;
                            state = STATE.LEFT_RUN;
                        }
                        if (e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                        {
                            dir = 1;
                            state = STATE.RIGHT_RUN;
                        }
                    }
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    {
                        if (e.Key == SDL.SDL_Keycode.SDLK_LEFT
                            || e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                        {
                            dir = 0;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
