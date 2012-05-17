using System;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    class UILocation : ILocationBehavior
    {

        private Rectangle rectangle;
        private Vector2 size;
        private Vector2 position;
        public Rectangle _Rechtangle
        {
            get
            {
                return this.rectangle;
            }
        }

        public UILocation(Vector2 position, Vector2 size)
        {
            this.position.X = rectangle.X;
            this.position.Y = rectangle.Y;
            this.size.X = rectangle.Width;
            this.size.Y = rectangle.Height;

        }
        public UILocation(Rectangle rectangle)
        {

        }

        public Vector2 GetPostition()
        {
            Vector2 tmp;
            tmp.X = rectangle.X;
            tmp.Y = rectangle.Y;
            return tmp;
        }

        public Vector2 GetSize()
        {
            Vector2 tmp;
            tmp.X = rectangle.Width;
            tmp.Y = rectangle.Height;
            return tmp;
        }
        
        public Rectangle GetRectangle()
        {
            throw new NotImplementedException();
        }
    }
}
