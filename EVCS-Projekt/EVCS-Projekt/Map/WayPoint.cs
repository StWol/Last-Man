using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using EVCS_Projekt.Tree;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Map
{
    public class WayPoint : IQuadStorable
    {
        // WayPoint id
        public int ID { get; set; }

        // Renderer - static da für alle gleich
        public static IRenderBehavior Renderer
        {
            get
            {
                return LoadedRenderer.Get("S_WayPoint");
            }
        }

        // WegPunkte die mit diesem Verbunden sind
        public List<WayPoint> connectedPoints { get; private set; }

        // Position
        public MapLocation Location { get; set; }

        public WayPoint(int xPosition, int yPosition)
        {
            // Liste init
            connectedPoints = new List<WayPoint>();

            // Position bestimmen - jeder wegpunkt ist zur anzeige 32px groß
            Location = new MapLocation(new Vector2(xPosition, yPosition), new Vector2(32));
        }

        public Rectangle Rect
        {
            get { return Location.BoundingBox; }
        }

        public bool HasMoved { get; set; }
    }
}
