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
        protected int frame { get; set; }

        static Boy()
        {
            Boy.image = Context.LoadImage(@"Resources\run_animation.png");
            Program.Resources.Add(Boy.image);
        }

        public Boy(int x, int y)
        {
            Pos = new Vector2D(x, y);
        }

        public void Render()
        {
            imageFrame.x = frame * 100;
            Boy.image.ClipRender(imageFrame, Pos);
        }

        public virtual void Update()
        {
            frame = (this.frame + 1) % 8;
        }
    }


    public class Character : Boy, IControllable
    {
        private int dir { get; set; }

        public Character(int x, int y) : base(x, y)
        {
            Pos = new Vector2D(x, y);
        }

        public override void Update()
        {
            frame = (frame + 1) % 8;
            Pos.x = Pos.x + (dir * 10);
        }

        public void EventHandle(GameEvent e)
        {
            switch (e.Type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    {
                        if (e.Key == SDL.SDL_Keycode.SDLK_LEFT)
                            dir = -1;
                        if (e.Key == SDL.SDL_Keycode.SDLK_RIGHT)
                            dir = 1;
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
