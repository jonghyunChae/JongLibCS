using Jong2D;
using Jong2D.Utility;
using SDL2;
using System;
using System.Collections.Generic;

namespace Jong2DTest.Sample12
{
    public partial class MainScene
    {
        public class Camera : ICamera
        {
            public Vector2D Pos;
            public void SetCamera(ref Vector2D pos)
            {
                Pos.x = JongMath.Clamp(0, pos.x - Context.screen_width / 2, BackGround.Instance.Width - Context.screen_width);
                Pos.y = JongMath.Clamp(0, pos.y - Context.screen_height / 2, BackGround.Instance.Height - Context.screen_height);
            }

            // 스크린 좌표로 변환해줍니다.
            public Vector2D ToScreenPos(ref Vector2D pos)
            {
                int width_half = Context.screen_width / 2;
                double x_left_offset = Math.Min(0, pos.x - width_half);
                double x_right_offset = Math.Max(0, pos.x - BackGround.Instance.Width + width_half);
                double x_offset = x_left_offset + x_right_offset;

                int height_half = Context.screen_height / 2;
                double y_bottom_offset = Math.Min(0, pos.y - height_half);
                double y_up_offset = Math.Max(0, pos.y - BackGround.Instance.Height + height_half);
                double y_offset = y_bottom_offset + y_up_offset;

                return new Vector2D(width_half + x_offset, height_half + y_offset);
            }
        }

        [Resource(@"Resources\scroll_background.png")]
        public class BackGround : IGameObject
        {
            public int Width => image.width;
            public int Height => image.height;

            Image image;
            public Camera Camera = new Camera();
            Rectangle rect = new Rectangle(0, 0, Context.screen_width, Context.screen_height);

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
            }

            public void Load()
            {
                image = ResourceFactory.CreateImage(this);
                Camera.Pos = new Vector2D(Width / 2, Height / 2);
            }

            public void Render()
            {
                rect.pos = Camera.Pos;
                image.ClipRenderToOrigin(rect, 0, 0);
            }

            public void Update(double frame_time) { }
            public void EventHandle(GameEvent e, double frame_time) { }
        }

        [Resource(@"Resources\grass.png")]
        public class Grass : IGameObject
        {
            Image image;
            Vector2D pos;

            public Grass(int x, int y)
            {
                image = ResourceFactory.CreateImage(this);
                pos = new Vector2D(x, y);
            }

            public void Render()
            {
                image.Render(pos.x, pos.y);
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
                font = ResourceFactory.CreateFont("Font", @"Resources\ConsolaMalgun.TTF", size);

                Content = content;
            }

            public void Render()
            {
                font.Render(pos.x, pos.y, Content, Color);
            }

            public void Update(double frame_time) { }
            public void EventHandle(GameEvent e, double frame_time) { }
        }

        [Resource(@"Resources\animation_sheet.png")]
        public class Boy : IGameObject
        {
            Image image;
            public Vector2D Pos;                    // 가상 세계의 위치

            private Rectangle imageFrame = new Rectangle(0, 0, 100, 100);
            int frame { get; set; }
            double total_frame { get; set; }
            Vector2D dir;

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

            public Boy(int x, int y)
            {
                image = ResourceFactory.CreateImage(this);
                Pos = new Vector2D(x, y);
                state = STATE.RIGHT_RUN;
                dir = new Vector2D(0, 0);

                stateHandlers = new Dictionary<STATE, Action>();
                stateHandlers[STATE.LEFT_RUN] = LeftRun;
                stateHandlers[STATE.RIGHT_RUN] = RightRun;
            }

            public void Render()
            {
                imageFrame.x = frame * 100;
                imageFrame.y = ((int)state) * 100;
                Vector2D screenPos = BackGround.Instance.Camera.ToScreenPos(ref this.Pos);
                image.ClipRender(this.imageFrame, screenPos);
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
                Vector2D afterPos = Pos + (dir * distance);

                // 마지막 움직임 제한을 강제로 둔다 (50~ max-50)
                Pos.x = JongMath.Clamp(50, afterPos.x, BackGround.Instance.Width - 50);
                Pos.y = JongMath.Clamp(50, afterPos.y, BackGround.Instance.Height - 50);
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
                                dir.x = -1;
                                state = STATE.LEFT_RUN;
                            }
                            else if (e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                            {
                                dir.x = 1;
                                state = STATE.RIGHT_RUN;
                            }
                            else if (e.Key == SDL.SDL_Keycode.SDLK_UP)
                            {
                                dir.y = 1;
                            }
                            else if (e.Key == SDL.SDL_Keycode.SDLK_DOWN)
                            {
                                dir.y = -1;
                            }
                        }
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        {
                            if (e.Key == SDL.SDL_Keycode.SDLK_LEFT
                                || e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                            {
                                dir.x = 0;
                            }
                            if (e.Key == SDL.SDL_Keycode.SDLK_UP
                                || e.Key == SDL.SDL_Keycode.SDLK_DOWN)
                            {
                                dir.y = 0;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
