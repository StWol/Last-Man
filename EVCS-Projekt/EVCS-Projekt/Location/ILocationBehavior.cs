using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    public interface ILocationBehavior
    {
        Rectangle BoundingBox
        {
            get;
        }

        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
    }
}
