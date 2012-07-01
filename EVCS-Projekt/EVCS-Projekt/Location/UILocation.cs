using Microsoft.Xna.Framework;

namespace LastMan.Location
{
    class UILocation : ILocationBehavior
    {

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
        public Rectangle RelativeBoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 RelativePosition
        {
            get
            {
                return Position;
            }
        }

        public float Rotation { get; set; }
        public Vector2 Direction { get; set; }

        // ***************************************************************************
        // Konstruktor 1
        public UILocation(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        // ***************************************************************************
        // Konstruktor 2
        public UILocation(Rectangle rectangle)
        {
            Position = new Vector2(rectangle.X, rectangle.Y);
            Size = new Vector2(rectangle.Width, rectangle.Height);
        }

        // ***************************************************************************
        // Clone
        public ILocationBehavior Clone()
        {
            return new UILocation(BoundingBox);
        }
    }
}
