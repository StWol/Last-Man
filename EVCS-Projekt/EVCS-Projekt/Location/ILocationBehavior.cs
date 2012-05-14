using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Location
{
    public interface ILocationBehavior
    {
         Vector2 GetPostition();
         Vector2 GetSize();
         Rectangle GetRectangle();

    }
}
