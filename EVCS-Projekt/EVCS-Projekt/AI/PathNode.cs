using LastMan.Map;

namespace LastMan.AI
{
    public class PathNode
    {
        public int ID { get; set; }
        public float Distance { get; set; }
        public WayPoint WayPoint { get; set; }

        public PathNode PreviousNode { get; set; }
        public PathNode NextNode { get; set; }

        public PathNode(float distance, int waypointID)
        {
            Distance = distance;
            ID = waypointID;
            WayPoint = Main.MainObject.GameManager.GameState.Karte.WayPoints[waypointID];
        }

        public PathNode(float distance, WayPoint waypoint)
        {
            Distance = distance;
            WayPoint = waypoint;
            ID = waypoint.ID;
        }

        // PathNode kopieren
        public PathNode Clone()
        {
            PathNode n = new PathNode(Distance, ID);

            if (NextNode != null)
                n.NextNode = NextNode.Clone();

            if (PreviousNode != null)
                n.PreviousNode = PreviousNode.Clone();

            n.WayPoint = WayPoint;

            n.Distance = Distance;

            return n;
        }

        // **************************************************************************
        // Prüft ob sich in einem pfad ein bestimmter wegpunkt befindet
        public bool IsWaypointInPath(int waypointId)
        {
            PathNode current = this;
            while (current != null)
            {
                if (current.ID == waypointId)
                    return true;

                current = current.NextNode;
            }
            return false;
        }

        // **************************************************************************
        // Prüft ob sich in einem pfad ein bestimmter wegpunkt befindet
        public bool IsWaypointInPath(int waypointId, out PathNode outNode)
        {
            PathNode current = this;
            while (current != null)
            {
                if (current.ID == waypointId)
                {
                    outNode = current;
                    return true;
                }

                current = current.NextNode;
            }

            outNode = null;
            return false;
        }
    }
}
