using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Jong2D.Utility
{
    public struct Color
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte a { get; set; }

        public Color(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color(byte r, byte g, byte b) : this(r, g, b, 0)
        {
        }

        public SDL.SDL_Color ToSDLColor()
        {
            return new SDL.SDL_Color()
            {
                r = this.r,
                g = this.g,
                b = this.b,
                a = this.a,
            };
        }

        public SDL.SDL_Color ToSDLColor_RGB()
        {
            return new SDL.SDL_Color()
            {
                r = this.r,
                g = this.g,
                b = this.b,
            };
        }
    }
}
