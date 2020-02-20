using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;
using System.Collections.Generic;

namespace Jong2DTest
{
    public class BoundingBox
    {
        public Vector2D MinPoint { get; set; }
        public Vector2D MaxPoint { get; set; }

        public bool Collide(BoundingBox bb)
        {
            if (this.MinPoint.x > bb.MaxPoint.x) return false;
            if (this.MaxPoint.x < bb.MinPoint.x) return false;
            if (this.MinPoint.y > bb.MaxPoint.y) return false;
            if (this.MaxPoint.y < bb.MinPoint.y) return false;

            return true;
        }

        public static BoundingBox Create(Vector2D Pos, Size2D size)
        {
            int w = size.width / 2;
            int h = size.height / 2;
            return new BoundingBox()
            {
                MinPoint = new Vector2D(Pos.x - w, Pos.y - h),
                MaxPoint = new Vector2D(Pos.x + w, Pos.y + h),
            };
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(MinPoint, new Size2D((int)(MaxPoint.x - MinPoint.x), (int)(MaxPoint.y - MinPoint.y)));
        }
    }

    interface IGameObject
    {
        void Render();
        void Update(double frame_time);
        void EventHandle(GameEvent e, double frame_time);
    }

    public interface ICollidable
    {
        BoundingBox GetBB();
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

    public class Boy : IGameObject, ICollidable
    {
        static Image image;
        public Vector2D Pos = new Vector2D();

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
            Boy.image.ClipRender(this.imageFrame, this.Pos);

            Context.Draw(GetBB().ToRectangle(), new Color(255, 0, 0));
        }

        public virtual void Update(double frame_time)
        {
            total_frame += FRAME_PER_ACTION * ACTION_PER_TIME * frame_time;
            frame = ((int)total_frame) % 8;

            double distance = RUN_SPEED_PPS * frame_time;
            double x = Pos.x + dir * distance;
            Pos.x = x;

            stateHandlers[state]();
        }

        void LeftRun()
        {
        }

        void RightRun()
        {
        }

        public BoundingBox GetBB()
        {
            return BoundingBox.Create(this.Pos, new Size2D(60, 80));
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
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    {
                        // 좌표계를 뒤집어 줍니다.
                        Vector2D mousePos = new Vector2D(e.x, (Program.SCREEN_HEIGHT - e.y));
                        Console.WriteLine(mousePos);

                        // 목표 위치 - 내 위치를하여 벡터 기준을 원점으로 되돌립니다.
                        Vector2D dirVector = mousePos - Pos;
                        Ball ball = new Ball(Pos, dirVector); // 내부에서 노말라이즈를 합니다.

                        Program.AddObject(ball);
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public class Ball : IGameObject, ICollidable
    {
        static Image image;
        Vector2D pos = new Vector2D();
        Vector2D dir;
        const int move_speed = 200;      // 초당 속도
        double life_time = 1.5;     // 생명 시간
        static Size2D Size = new Size2D(22, 22);

        static Ball()
        {
            Ball.image = Context.LoadImage(@"Resources\ball21x21.png");
            Program.Resources.Add(Ball.image);
        }

        public Ball(Vector2D pos, Vector2D dir)
        {
            this.pos = pos;
            this.dir = dir.Normalize(); // 방향벡터는 정규화 상태를 유지 해야합니다.
        }

        public void EventHandle(GameEvent e, double frame_time)
        {
        }

        public BoundingBox GetBB()
        {
            return BoundingBox.Create(pos, Size);
        }

        public void Render()
        {
            Ball.image.Render(pos);
            Context.Draw(GetBB().ToRectangle(), new Color(255, 0, 0));
        }

        public void Update(double frame_time)
        {
            // 이동할 거리 = 방향 벡터 * 속도 * 시간
            Vector2D moveVector = dir * move_speed * frame_time;
            // 내 위치 = 내 위치 + 이동할 거리
            pos = pos + moveVector;

            life_time -= frame_time;
            if (life_time < 0)
            {
                Program.RemoveObject(this);
            }
        }
    }

}
