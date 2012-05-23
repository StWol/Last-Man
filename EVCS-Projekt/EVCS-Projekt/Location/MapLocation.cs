using System;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    class MapLocation : ILocationBehavior
    {

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)(Position.X-Size.X/2), (int)(Position.Y-Size.Y/2), (int)Size.X, (int)Size.Y);
            }
        }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Vector2 Direction { get; set; }

        public void Sizing()
        {

        }

        // ***************************************************************************
        // Konstruktor 1
        public MapLocation(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            Direction = new Vector2(0, 0);
        }

        // ***************************************************************************
        // Konstruktor 2
        public MapLocation(Rectangle rectangle)
        {
            Position = new Vector2(rectangle.X, rectangle.Y);
            Size = new Vector2(rectangle.Width, rectangle.Height);
            Direction = new Vector2(0, 0);
        }

    }
}
