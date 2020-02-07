using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;

namespace Jong2DTest
{
    class Program
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 480;

        static void Main(string[] args)
        {
            Context.CreateWindow(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT);
            Font font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", 16);
            Image grass = Context.LoadImage(@"Resources\grass.png");
            Image character = Context.LoadImage(@"Resources\character.png");

            var pos = new Vector2D(0, 80);
            while (pos.x < 800)
            {
                Context.ClearWindow();

                font.Render(100, 300, "Sample1", new Color(100, 25, 25));
                grass.Render(Program.SCREEN_WIDTH / 2, 30);
                character.Render(pos);

                Context.UpdateWindow();

                pos.x += 5;
                Context.Delay(0.05);
                Context.GetGameEvents();
            }

            Context.CloseWindow();
        }
    }
}
