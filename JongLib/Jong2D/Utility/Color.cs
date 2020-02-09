using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Jong2D.Utility
{
    public struct Color : IEquatable<Color>
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte a { get; set; }

        public Color(byte r = 0, byte g = 0, byte b = 0, byte a = 0)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
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

        public bool Equals(Color other)
        {
            if (this.r != other.r) return false;
            if (this.g != other.g) return false;
            if (this.b != other.b) return false;
            if (this.a != other.a) return false;
            return true;
        }
    }
}
