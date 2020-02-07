using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
using Jong2D.Utility;

namespace Jong2D
{
    public class Font
    {
        private IntPtr font { get; set; }
        public Font(string name, int size = 20)
        {
            this.font = SDL_ttf.TTF_OpenFont(name, size);
            if (this.font == IntPtr.Zero)
            {
                throw new Exception($"Font Create Fail - name:{name}, size:{size}");
            }
        }

        public Image CreateImage(string str, Color color)
        {
            var surface = SDL_ttf.TTF_RenderUNICODE_Blended(this.font, str, color.ToSDLColor());
            var texture = SDL.SDL_CreateTextureFromSurface(Context.renderer, surface);
            SDL.SDL_FreeSurface(surface);

            return new Image(texture);
        }

        public void Render(int x, int y, string str, Color color)
        {
            using (var image = this.CreateImage(str, color))
            {
                image.Render(x, y);
            }
        }

        public void Render(Vector2D pos, string str, Color color)
        {
            using (var image = this.CreateImage(str, color))
            {
                image.Render(pos);
            }
        }
    }

    public static partial class Context
    {
        public static Font LoadFont(string name, int size = 20)
        {
            try
            {
                var font = new Font(name, size);
                return font;
            }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }
        }
    }
}
