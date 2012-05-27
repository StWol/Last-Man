using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Tree;
using System;
using System.Diagnostics;

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
        // Setzt die Größe des LocationBehavior auf die Größe der Textur bzw die Größe des Renderers
        public void LocationSizing()
        {
            // Nicht schön, aber NoRenderer und SimpleRenderer werden abgefangen, da diese keine Größe besitzten
            if (Renderer.GetType() == typeof(NoRenderer) )
                return;

            LocationBehavior.Size = new Vector2(Renderer.Size.X, Renderer.Size.Y);
        }

        // ***************************************************************************
        // Berechnet Direction und Rotation, dass Location in richtung des Vectors schaut
        public void LookAt(Vector2 target)
        {
            Vector2 norm = LocationBehavior.Position - target;
            norm.Normalize();
            LocationBehavior.Direction = norm;
            
            CalculateRotation();
        }

        // ***************************************************************************
        // Berechnet Direction und Rotation, dass Location in richtung des Vectors schaut abhänig des MapOffsets
        public void RelativeLookAt(Vector2 target)
        {
            Vector2 norm = LocationBehavior.RelativePosition - target;
            norm.Normalize();
            LocationBehavior.Direction = norm;

            CalculateRotation();
        }

        // ***************************************************************************
        // Set die Direction und berechnet die Rotation, dass Location in richtung des Vectors schaut
        public void SetDirection(Vector2 direction)
        {
            Vector2 norm = direction;
            norm.Normalize();
            LocationBehavior.Direction = norm;

            CalculateRotation();
        }

        // ***************************************************************************
        // berechnet die Rotation, dass Location in richtung des Vectors schaut
        private void CalculateRotation()
        {
            LocationBehavior.Rotation = -(float)(Math.Atan2(-LocationBehavior.Direction.Y, LocationBehavior.Direction.X));
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Gibt Position als Rectangle zurück / BoundingBox
        public Rectangle Rect
        {
            get
            {
                // Korrektur, da die gerendeten Bilder ihre position zentriert haben und nicht in der linken oberen ecke
                Rectangle r = LocationBehavior.BoundingBox;
                r.X -= r.Width / 2;
                r.Y -= r.Height / 2;
                return r;
            }
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
