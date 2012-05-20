using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects
{

    public abstract class GameObject
    {
        // Baucht man die Vererbung der GameComponent von XNA und die initialize methode?

        private IRenderBehavior renderBehavoir;
        public ILocationBehavior LocationBehavior { get; private set; }


        public GameObject( ILocationBehavior locationBehavior)

        {
            this.LocationBehavior = locationBehavior;
        }

        public Rectangle GetBoundingBox()
        {
            // TODO: BB zurückgeben
            return new Rectangle();
        }
 

    }
}
