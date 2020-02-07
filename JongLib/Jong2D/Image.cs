﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDL2;

namespace Jong2D
{
    public class Image : IDisposable
    {
        public Utility.Size2D size;
        public int height => size.height;
        public int width => size.width;
        private IntPtr texture { get; set; }

        public Image(IntPtr texture)
        {
            this.texture = texture;
            uint format;
            int access;
            int w, h;
            SDL.SDL_QueryTexture(this.texture, out format, out access, out w, out h);
            this.size = new Utility.Size2D(w, h);
        }

        public void Render(Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.Render(pos.x, pos.y, size);
        }

        public void Render(int x, int y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x - w / 2, y - h / 2, w, h);
            SDL.SDL_RenderCopy(Context.renderer, this.texture, IntPtr.Zero, ref rect);
        }

        public void RenderToOrigin(Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.RenderToOrigin(pos.x, pos.y, size);
        }

        public void RenderToOrigin(int x, int y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x, y, w, h);
            SDL.SDL_RenderCopy(Context.renderer, this.texture, IntPtr.Zero, ref rect);
        }

        public void RotateRender(double rad, Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.RotateRender(rad, pos.x, pos.y, size);
        }

        public void RotateRender(double rad, int x, int y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x - w / 2, y - h / 2, w, h);
            SDL.SDL_RenderCopyEx(Context.renderer, this.texture, IntPtr.Zero, ref rect, rad, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public void ClipRender(Utility.Rectangle rect, Utility.Vector2D pos)
        {
            ClipRender(rect, pos.x, pos.y);
        }

        public void ClipRender(Utility.Rectangle rect, int x, int y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? rect.width;
            int h = size?.height ?? rect.height;

            var srcRect = new SDL.SDL_Rect()
            {
                x = rect.x,
                y = this.height - rect.y - rect.height,
                w = rect.width,
                h = rect.height,
            };
            var destRect = Context.ToSDLRect(x - w/2, y - h/2, w, h);
            SDL.SDL_RenderCopy(Context.renderer, this.texture, ref srcRect, ref destRect);
        }

        public void ClipRenderToOrigin(Utility.Rectangle rect, int x, int y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? rect.width;
            int h = size?.height ?? rect.height;

            var srcRect = new SDL.SDL_Rect()
            {
                x = rect.x,
                y = this.height - rect.y - rect.height,
                w = rect.width,
                h = rect.height,
            };
            var destRect = Context.ToSDLRect(x, y, w, h);
            SDL.SDL_RenderCopy(Context.renderer, this.texture, ref srcRect, ref destRect);
        }

        public void RenderNow(Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.Render(pos, size);
            Context.UpdateWindow();
            this.Render(pos, size);
            Context.UpdateWindow();
        }

        public void Opacity(float o)
        {
            byte alpha = 0;
            unchecked
            {
                alpha = (byte)(o * 255.0);
            }
            SDL.SDL_SetTextureAlphaMod(this.texture, alpha);
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

    public static partial class Context
    {
        //internal static SDL.SDL_Rect ToSDLRect(ref Jong2D.Utility.Rectangle rect)
        //{
        //    var sdl_rect = new SDL.SDL_Rect();
        //    sdl_rect.x = rect.x;
        //    sdl_rect.y = Context.screen_height - rect.y - rect.height;
        //    sdl_rect.w = width;
        //    sdl_rect.h = height;
        //    return sdl_rect;
        //}

        internal static SDL.SDL_Rect ToSDLRect(int x, int y, int width, int height)
        {
            var sdl_rect = new SDL.SDL_Rect();
            sdl_rect.x = x;
            sdl_rect.y = Context.screen_height - y - height;
            sdl_rect.w = width;
            sdl_rect.h = height;
            return sdl_rect;
        }

        public static Image LoadImage(string name)
        {
            var texture = SDL_image.IMG_LoadTexture(renderer, name);
            if (texture == IntPtr.Zero)
            {
                string msg = $"Cannot Load Image {name}";
                Log(msg);
                throw new Exception(msg);
            }

            return new Image(texture);
        }
    }
}
