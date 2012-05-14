using System;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    class MapLocation : ILocationBehavior
    {

        //Attributes

        private Rectangle rectangle;
        public Rectangle _Rectangle
        {
            get
            {
                return this.rectangle;
            }
            set
            {
                this.rectangle = value;
            }
        }
        private Vector2 direction;
        public Vector2 Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }
        private Vector2 size;
        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        //Constructor
        public MapLocation(Vector2 position, Vector2 size)
        {
            this.position.X = rectangle.X;
            this.position.Y = rectangle.Y;
            this.size.X = rectangle.Width;
            this.size.Y = rectangle.Height;
            
        }
        public MapLocation(Rectangle rectangle)
        {
           

        }


        public Vector2 GetPostition()
        {
            throw new NotImplementedException();
        }

        public Vector2 GetSize()
        {
            throw new NotImplementedException();
        }

        public Rectangle GetRectangle()
        {
            throw new NotImplementedException();
        }
    }
}
