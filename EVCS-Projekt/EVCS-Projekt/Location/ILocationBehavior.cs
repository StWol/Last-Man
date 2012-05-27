using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    public interface ILocationBehavior
    {
        Rectangle BoundingBox
        {
            get;
        }
        Rectangle RelativeBoundingBox
        {
            get;
        }

        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
        Vector2 RelativePosition
        {
            get;
        }

        float Rotation { get; set; }
        Vector2 Direction { get; set; }
    }
}
