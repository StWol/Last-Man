using LastMan.Renderer;
using LastMan.Location;

namespace LastMan.Objects
{
    public class StaticObject : GameObject
    {
        public StaticObject(ILocationBehavior location, IRenderBehavior renderer)
            : this(location, renderer, true)
        {
        }

        public StaticObject(ILocationBehavior location, IRenderBehavior renderer, bool locationSizing)
            : base(location, renderer)
        {
            if (locationSizing)
                LocationSizing();

            allowToRotate = true;
        }
    }
}
