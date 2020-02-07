using System;
using System.Runtime.InteropServices;
using SDL2;
using Jong2D;
using Jong2D.Utility;
using System.Threading;

namespace Jong2DTest
{
    class Sample1
    {
        //Screen dimension constants
        private const int SCREEN_WIDTH = 640;
        private const int SCREEN_HEIGHT = 480;

        static void Main(string[] args)
        {
            Context.CreateWindow(Sample1.SCREEN_WIDTH, Sample1.SCREEN_HEIGHT);
            var font = Context.LoadFont(@"Resources\ConsolaMalgun.TTF", 16);
            var grass = Context.LoadImage(@"Resources\grass.png");
            var character = Context.LoadImage(@"Resources\character.png");

            var pos = new Vector2D(0, 80);
            while (pos.x < 800)
            {
                Context.ClearWindow();

                font.Render(100, 300, "Sample1", new Color(100, 25, 25));
                grass.Render(Sample1.SCREEN_WIDTH / 2, 30);
                character.Render(pos);

                Context.UpdateWindow();

                pos.x += 1;
                Thread.Sleep(1);
                Context.GetGameEvents();
            }

            Context.CloseWindow();
        }
    }
}
