using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;
using Jong2D.Utility;

namespace Jong2D
{
    public class Font : Resource
    {
        private string cacheStr { get; set; }
        private Color cacheColor { get; set; }
        private Image image { get; set; }

        private IntPtr font { get; set; }

        internal Font(IntPtr font)
        {
            this.font = font;
        }

        public Image CreateImage(string str, Color color)
        {
            var surface = SDL_ttf.TTF_RenderUNICODE_Blended(this.font, str, color.ToSDLColor());
            var texture = SDL.SDL_CreateTextureFromSurface(Context.renderer, surface);
            SDL.SDL_FreeSurface(surface);

            return new Image(texture);
        }

        private void CreateImageProxy(string str, Color color)
        {
            if (this.image != null)
            {
                if (string.IsNullOrEmpty(str))
                    throw new Exception("font string is null or empty");

                if (this.cacheStr == str 
                    && this.cacheColor.Equals(color))
                {
                    return;
                }

                this.image.Dispose();
                this.image = null;
            }

            this.cacheStr = str;
            this.cacheColor = color;
            this.image = this.CreateImage(str, color);
        }

        public void Render(double x, double y, string str, Color color)
        {
            this.CreateImageProxy(str, color);

            this.image.Render(x, y);
        }

        public void Render(Vector2D pos, string str, Color color)
        {
            this.CreateImageProxy(str, color);

            this.image.Render(pos);
        }

        public override void Close()
        {
            this.image?.Dispose();
            this.image = null;
            if (this.font != IntPtr.Zero)
            {
                SDL_ttf.TTF_CloseFont(this.font);
                this.font = IntPtr.Zero;
            }
        }
    }

    public static partial class Context
    {
        public static Font LoadFont(string name, int size = 20)
        {
            IntPtr data = SDL_ttf.TTF_OpenFont(name, size);
            if (data == IntPtr.Zero)
            {
                string msg = $"Error Load Font!! name:{name} size:{size}";
                Log(msg);
                throw new Exception(msg);
            }

            return new Font(data);
        }
    }
}
