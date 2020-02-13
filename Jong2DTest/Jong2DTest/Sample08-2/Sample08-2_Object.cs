using System;
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
    }

    interface IControllable
    {
        void EventHandle(GameEvent e, double frame_time);
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
    }

    public class Boy : IGameObject
    {
        static Image image;
        public Vector2D Pos = new Vector2D();

        private Rectangle imageFrame = new Rectangle(0, 0, 100, 100);
        int frame { get; set; }
        double total_frame { get; set; }
        int dir { get; set; }

        /*
         * PIXEL_PER_METER = (10.0 / 0.3) # 10 pixel 30 cm 
         * RUN_SPEED_KMPH = 20.0 # Km / Hour 
         * RUN_SPEED_MPM = (RUN_SPEED_KMPH * 1000.0 / 60.0) 
         * RUN_SPEED_MPS = (RUN_SPEED_MPM / 60.0) 
         * RUN_SPEED_PPS = (RUN_SPEED_MPS * PIXEL_PER_METER)
         */
        const double RUN_SPEED_PPS = 100; // 1초에 100을 옮긴다고 가정하자

        const double TIME_PER_ACTION = 2.0;
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
            dir = 1;

            stateHandlers = new Dictionary<STATE, Action>();
            stateHandlers[STATE.LEFT_RUN] = LeftRun;
            stateHandlers[STATE.RIGHT_RUN] = RightRun;
        }

        public void Render()
        {
            imageFrame.x = frame * 100;
            imageFrame.y = ((int)state) * 100;
            Boy.image.ClipRender(this.imageFrame, this.Pos);
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
            if (this.Pos.x < 50)
            {
                state = STATE.RIGHT_RUN;
                dir = 1;
            }
        }

        void RightRun()
        {
            if (Pos.x > 750)
            {
                state = STATE.LEFT_RUN;
                dir = -1;
            }
        }
    }
}
