using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace Jong2D
{
    public class Image : IDisposable
    {
        private int width { get; set; }
        private int height { get; set; }
        private IntPtr texture { get; set; }
        public Image(IntPtr texture)
        {
            this.texture = texture;
            uint format;
            int access;
            int w, h;
            SDL.SDL_QueryTexture(this.texture, out format, out access, out w, out h);
            this.width = w;
            this.height = h;
        }

        public void Draw(int x, int y)
        {
            this.Draw(x, y, this.width, this.height);
        }

        public void Draw(int x, int y, int w, int h)
        {
            var rect = Context.To_SDL_Rect(x - w / 2, y - h / 2, w, h);
            SDL.SDL_RenderCopy(Context.renderer, this.texture, IntPtr.Zero, ref rect);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                SDL.SDL_DestroyTexture(texture);
                texture = IntPtr.Zero;

                disposedValue = true;
            }
        }

        ~Image()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
