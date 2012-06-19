using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Map;

namespace EVCS_Projekt.AI
{
    public class PathNode
    {
        public int ID { get; set; }
        public float Distance { get; set; }

        public PathNode PreviousNode { get; set; }
        public PathNode NextNode { get; set; }

        public PathNode(float distance, int waypointID )
        {
            Distance = distance;
            ID = waypointID;
        }

        // PathNode kopieren
        public PathNode Clone()
        {
            PathNode n = new PathNode(Distance, ID);

            if (NextNode != null)
                n.NextNode = NextNode.Clone();

            if (PreviousNode != null)
                n.PreviousNode = PreviousNode.Clone();

            return n;
        }

        // **************************************************************************
        // Prüft ob sich in einem pfad ein bestimmter wegpunkt befindet
        public static bool IsWaypointInPath(int waypointId, PathNode path)
        {
            PathNode current = path;
            while (current != null)
            {
                if (current.ID == waypointId)
                    return true;

                current = current.NextNode;
            }
            return false;
        }
    }
}
