using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LastMan.Helper
{
    public struct FRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public FRectangle(Rectangle r)
        {
            X = r.X;
            Y = r.Y;
            Width = r.Width;
            Height = r.Height;
        }

        public FRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle Rect()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}
