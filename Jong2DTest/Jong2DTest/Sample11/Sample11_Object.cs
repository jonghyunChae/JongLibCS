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
        public Vector2D Pos;
        public void SetCamera(ref Vector2D pos)
        {
            this.Pos.x = BackGround.Width - pos.x;
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
            BackGround.image.Render(Camera.Pos);
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
