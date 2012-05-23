using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Tree;

namespace EVCS_Projekt.Objects
{

    public abstract class GameObject : IQuadStorable
    {
        // Baucht man die Vererbung der GameComponent von XNA und die initialize methode?

        public IRenderBehavior Renderer { get; set; }
        public ILocationBehavior LocationBehavior { get; private set; }


        public GameObject(ILocationBehavior locationBehavior, IRenderBehavior renderBehavior)
        {
            this.LocationBehavior = locationBehavior;
            this.Renderer = renderBehavior;
        }

        public GameObject(ILocationBehavior locationBehavior)
        {
            this.LocationBehavior = locationBehavior;
            this.Renderer = new NoRenderer();
        }

        // ***************************************************************************
        // Setzt die Gr��e des LocationBehavior auf die Gr��e der Textur bzw die Gr��e des Renderers
        public void LocationSizing()
        {
            // Nicht sch�n, aber NoRenderer und SimpleRenderer werden abgefangen, da diese keine Gr��e besitzten
            if ( Renderer.GetType() == typeof(NoRenderer) || Renderer.GetType() == typeof(SimpleRenderer) )
                return;

            LocationBehavior.Size = Renderer.Size;
        }

        // ***************************************************************************
        // F�r Quadtree ben�tigt - Gibt Position als Rectangle zur�ck / BoundingBox
        public Rectangle Rect
        {
            get
            {
                return LocationBehavior.BoundingBox;
            }
        }

        // ***************************************************************************
        // F�r Quadtree ben�tigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
