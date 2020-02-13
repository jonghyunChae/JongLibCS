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
        void Update();
    }

    interface IControllable
    {
        void EventHandle(GameEvent e);
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

        public void Update() { }
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

        public void Update() { }
    }

    public class Boy : IGameObject
    {
        static Image image;
        public Vector2D Pos = new Vector2D();

        private Rectangle imageFrame = new Rectangle(0, 0, 100, 100);
        int frame { get; set; }
        int stand_frame { get; set; }
        int run_frame { get; set; }

        enum STATE
        {
            LEFT_RUN,
            RIGHT_RUN,
            LEFT_STAND,
            RIGHT_STAND,

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
            state = STATE.LEFT_STAND;

            stateHandlers = new Dictionary<STATE, Action>();
            stateHandlers[STATE.LEFT_RUN] = LeftRun;
            stateHandlers[STATE.RIGHT_RUN] = RightRun;
            stateHandlers[STATE.LEFT_STAND] = LeftStand;
            stateHandlers[STATE.RIGHT_STAND] = RightStand;
        }

        public void Render()
        {
            imageFrame.x = frame * 100;
            imageFrame.y = ((int)state) * 100;
            Boy.image.ClipRender(this.imageFrame, this.Pos);

        }

        public virtual void Update()
        {
            frame = (frame + 1) % 8;
            stateHandlers[state]();
        }

        void LeftRun()
        {
            Pos.x -= 5;
            run_frame++;
            if (this.Pos.x < 10)
            {
                state = STATE.RIGHT_RUN;
            }
            if (run_frame == 100)
            {
                state = STATE.LEFT_STAND;
                stand_frame = 0;
            }
        }

        void RightRun()
        {
            Pos.x += 5;
            run_frame++;
            if (Pos.x > 800)
            {
                state = STATE.LEFT_RUN;
            }
            if (run_frame == 100)
            {
                state = STATE.RIGHT_STAND;
                stand_frame = 0;
            }
        }

        void LeftStand()
        {
            stand_frame++;
            if (stand_frame == 50)
            {
                state = STATE.LEFT_RUN;
                run_frame = 0;
            }
        }

        void RightStand()
        {
            stand_frame++;
            if (stand_frame == 50)
            {
                state = STATE.RIGHT_RUN;
                run_frame = 0;
            }
        }
    }
}
