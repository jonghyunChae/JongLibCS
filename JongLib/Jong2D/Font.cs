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

        public void Draw(int x, int y, string str, Color color)
        {
            var surface = SDL_ttf.TTF_RenderUNICODE_Blended(this.font, str, color.ToSDLColor());
            var texture = SDL.SDL_CreateTextureFromSurface(Context.renderer, surface);
            SDL.SDL_FreeSurface(surface);
            using (var image = new Image(texture))
            {
                image.Draw(x, y);
            }
        }
    }
}
