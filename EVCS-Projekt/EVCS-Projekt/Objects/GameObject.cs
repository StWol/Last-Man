using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Tree;

namespace EVCS_Projekt.Objects
{

    public abstract class GameObject : IQuadStorable
    {
        // Baucht man die Vererbung der GameComponent von XNA und die initialize methode?

        private IRenderBehavior renderBehavoir;
        public ILocationBehavior LocationBehavior { get; private set; }


        public GameObject(ILocationBehavior locationBehavior)
        {
            this.LocationBehavior = locationBehavior;
        }

        public Rectangle GetBoundingBox()
        {
            // TODO: BB zur�ckgeben
            return new Rectangle();
        }

        // ***************************************************************************
        // F�r Quadtree ben�tigt - Gibt Position als Rectangle zur�ck
        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)LocationBehavior.Position.X, (int)LocationBehavior.Position.Y, (int)LocationBehavior.Size.X, (int)LocationBehavior.Size.Y);
            }
        }

        // ***************************************************************************
        // F�r Quadtree ben�tigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
