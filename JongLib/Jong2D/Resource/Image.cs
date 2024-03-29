﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jong2D.Utility;
using SDL2;
using static SDL2.SDL;

namespace Jong2D
{
    public class Image : Resource
    {
        public Utility.Size2D size;
        public int height => size.height;
        public int width => size.width;
        private IntPtr texture { get; set; }

        internal Image(IntPtr texture)
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

        public void Render(double x, double y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x - w / 2, y - h / 2, w, h);
            SDL.SDL_RenderCopyF(Context.renderer, this.texture, IntPtr.Zero, ref rect);
        }

        public void RenderToOrigin(Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.RenderToOrigin(pos.x, pos.y, size);
        }

        public void RenderToOrigin(double x, double y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x, y, w, h);
            SDL.SDL_RenderCopyF(Context.renderer, this.texture, IntPtr.Zero, ref rect);
        }

        public void RotateRender(double rad, Utility.Vector2D pos, Utility.Size2D? size = null)
        {
            this.RotateRender(rad, pos.x, pos.y, size);
        }

        public void RotateRender(double rad, double x, double y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? this.width;
            int h = size?.height ?? this.height;

            var rect = Context.ToSDLRect(x - w / 2, y - h / 2, w, h);
            SDL.SDL_RenderCopyExF(Context.renderer, this.texture, IntPtr.Zero, ref rect, rad, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public void ClipRender(Utility.Rectangle rect, Utility.Vector2D pos)
        {
            ClipRender(rect, pos.x, pos.y);
        }

        public void ClipRender(Utility.Rectangle rect, double x, double y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? rect.width;
            int h = size?.height ?? rect.height;

            var srcRect = new SDL.SDL_Rect()
            {
                x = (int)rect.x,
                y = this.height - (int)rect.y - rect.height,
                w = rect.width,
                h = rect.height,
            };
            var destRect = Context.ToSDLRect(x - w/2, y - h/2, w, h);
            SDL.SDL_RenderCopyF(Context.renderer, this.texture, ref srcRect, ref destRect);
        }

        public void ClipRenderToOrigin(Utility.Rectangle rect, double x, double y, Utility.Size2D? size = null)
        {
            int w = size?.width ?? rect.width;
            int h = size?.height ?? rect.height;

            var srcRect = new SDL.SDL_Rect()
            {
                x = (int)rect.x,
                y = this.height - (int)rect.y - rect.height,
                w = rect.width,
                h = rect.height,
            };
            var destRect = Context.ToSDLRect((int)x, (int)y, w, h);
            SDL.SDL_RenderCopyF(Context.renderer, this.texture, ref srcRect, ref destRect);
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

        public override void Close()
        {
            if (this.texture != IntPtr.Zero)
            {
                SDL.SDL_DestroyTexture(this.texture);
                this.texture = IntPtr.Zero;
            }
        }
    }

    public static partial class Context
    {
        internal static SDL.SDL_FRect ToSDLRect(double x, double y, int width, int height)
        {
            var sdl_rect = new SDL.SDL_FRect();
            sdl_rect.x = (float)x;
            sdl_rect.y = (float)(Context.screen_height - y - height);
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

        public static void Draw(Utility.Rectangle rect, Utility.Color color, bool fill = false)
        {
            SDL.SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);

            double x, y;
            int w, h;

            x = rect.x;
            y = (Context.screen_height - rect.y - rect.height);
            h = rect.height;
            w = rect.width;

            if (fill)
            {
                var sdl_rect = new SDL_Rect()
                {
                    x = (int)x,
                    y = (int)y,
                    h = rect.height,
                    w = rect.width
                };
                SDL.SDL_RenderFillRect(renderer, ref sdl_rect);
            }
            else
            {
                var sdl_rect = new SDL.SDL_FRect()
                {
                    x = (float)x,
                    y = (float)y,
                    h = rect.height,
                    w = rect.width
                };
                SDL.SDL_RenderDrawRectF(renderer, ref sdl_rect);
            }
        }

        public static void DrawPoint(Utility.Vector2D vector, Utility.Color color)
        {
            SDL.SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);

            SDL.SDL_RenderDrawPoint(renderer, (int)vector.x, Context.screen_height - (int)(vector.y));
        }

        public static void DrawPoint(Utility.Vector2D vector, int size, Utility.Color color)
        {
            Draw(new Utility.Rectangle(vector - (size / 2), new Size2D(size, size)), color, true);
        }

        public static void DrawLine(Utility.Vector2D from, Utility.Vector2D to, Utility.Color color)
        {
            SDL.SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);

            SDL.SDL_RenderDrawLine(renderer, (int)from.x, Context.screen_height - (int)from.y, (int)to.x, Context.screen_height - (int)to.y);
        }
    }
}
