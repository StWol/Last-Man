using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Map;
using System.Diagnostics;

namespace EVCS_Projekt.AI
{
    public class PathFinder
    {
        private static Dictionary<int, Dictionary<int, PathNode>> savedPaths;

        // Zur berechnung nötig
        //private static List<PathNode> openList;
        //private static List<PathNode> closedList;

        private static bool callculated = false;

        public static void InitPaths()
        {
            savedPaths = new Dictionary<int, Dictionary<int, PathNode>>();

            allNodes = new Dictionary<int, PathNode>();

            foreach (WayPoint w in Main.MainObject.GameManager.GameState.Karte.WayPoints.Values) {
                PathNode n = new PathNode(float.MaxValue, w);
                allNodes.Add(w.ID, n);
            }

            //Debug.WriteLine("Pfade berechnen..");
            //int c = 0;
            /*foreach (WayPoint s in Main.MainObject.GameManager.GameState.Karte.WayPoints.Values)
            {
                //c++;
                foreach (WayPoint t in Main.MainObject.GameManager.GameState.Karte.WayPoints.Values)
                {
                    PathFinder.FindePath(s, t);
                }

                //Debug.WriteLineIf(c % 100 == 0, c + " pfade berechnet");
            }*/

            //callculated = true;
        }

        public static PathNode FindePath(WayPoint start, WayPoint target)
        {
            if (!callculated)
            {
                if ( savedPaths.ContainsKey(start.ID) && savedPaths[start.ID].ContainsKey(target.ID) )
                    return savedPaths[start.ID][target.ID].Clone();
                return _findePath(start, target);
            }
            else
                return savedPaths[start.ID][target.ID].Clone();
        }

        //
        private static List<PathNode> openList;
        private static List<PathNode> closedList;
        private static Dictionary<int, PathNode> allNodes;

        private static PathNode _findePath(WayPoint start, WayPoint target)
        {
            // Listen init
            openList = new List<PathNode>();
            closedList = new List<PathNode>();

            // Start einfügen
            PathNode startNode = new PathNode(0, start);
            openList.Add(startNode);

            // solange openlist nicht leer ist
            while (openList.Count > 0)
            {
                //next Node aus openlist holen
                PathNode next = openList[0];
                openList.RemoveAt(0);

                // Nachbarn einfügen
                expandNode(next);

                // Node in closeliste einfügn
                closedList.Add(next);

                // prüfen ob ich schon fertig bin
                if (next.ID == target.ID)
                    break;
            }

            // pfad bauen
            buildPath(startNode, target.ID);

            return startNode;
        }

        private static void buildPath (PathNode start, int targetId) {

            PathNode _build = allNodes[targetId];

            PathNode current = _build;

            while (current.ID != start.ID)
            {
                PathNode prev = allNodes[current.PreviousNode.ID];

                prev.NextNode = current;
                int c = current.PreviousNode.ID;
                current = prev;
            }

            start = allNodes[start.ID]; ;
        }

        private static void expandNode( PathNode n )
        {
            foreach (WayPoint w in n.WayPoint.connectedPoints)
            {
                // Pathnode mit distanz erzeugen
                PathNode node = allNodes[w.ID]; 

                // node adden falls er noch nicht in openlist und closelist ist
                if (closedList.Contains(node))
                    continue;

                // distanz zu nachbar
                float dist = n.Distance + n.WayPoint.distances[w.ID];

                // checken ob er schon in openlist ist
                if (openList.Contains(node))
                {
                    // Distanz und vorgänger udpaten falls distanz kleiner ist
                    if (dist < node.Distance)
                    {
                        node.Distance = dist;
                        node.PreviousNode = n;
                    }
                }
                else
                {
                    // vorgänger setzten
                    node.PreviousNode = n;
                    node.Distance = dist;

                    // pathnode in open list
                    openList.Add(node);
                }
            }

            // List sortieren
            openList.Sort(CompareNodes);
        }

        /*private static PathNode _findePath(WayPoint start, WayPoint target)
        {
            //Debug.WriteLine("pfad berechnen von " + start.ID + " zu " + target.ID);

            if (start == target)
            {
                if (!savedPaths.ContainsKey(start.ID))
                    savedPaths.Add(start.ID, new Dictionary<int, PathNode>());

                // Pfad clonen und speichern
                savedPaths[start.ID].Add(target.ID, new PathNode(0, start.ID));
                return new PathNode(0, start.ID);
            }

            // Listen initialisieren
            openList = new List<PathNode>();
            closedList = new List<PathNode>();

            // Startpunkt in die openList machen
            PathNode startNode = new PathNode(0, start.ID);
            openList.Add(startNode);

            // Gefundener Pfad
            PathNode found = null;

            // Solange openlist nicht leer ist
            while (openList.Count > 0)
            {
                // Erster Knoten aus der openlist holen und entfernen (kürzester weg zum ziel)
                PathNode current = openList[0];
                openList.Remove(current);

                // Falls current == ziel beenden
                if (current.ID == target.ID)
                {
                    found = current;
                    break;
                }

                // Kinder in open list einfügen
                expandNode(current);

                // aktuelle knoten in closed list einfügen
                closedList.Add(current);
            }

            // Pfad umdrehen
            if (found != null)
            {
                PathNode reverse = null;
                PathNode reverseCurrent = null;
                while (found.PreviousNode != null)
                {
                    // Letzte Knoten holen
                    PathNode last = popLastNode(found);

                    // Letzte knoten an neue liste hängen
                    if (reverse == null)
                    {
                        reverse = last;
                        reverseCurrent = reverse;
                    }
                    else
                    {
                        reverseCurrent.NextNode = last;
                        reverseCurrent = reverseCurrent.NextNode;
                    }
                }

                // Letztes Element noch anhängen
                reverseCurrent.NextNode = found;

                // Pfad speichern um ihn nicht erneut berechnen zu müssen
                if (!savedPaths.ContainsKey(start.ID))
                    savedPaths.Add(start.ID, new Dictionary<int, PathNode>());

                // Pfad clonen und speichern
                savedPaths[start.ID].Add(target.ID, reverse.Clone());

                // Pfad zurückgeben
                return reverse;
            }

            // Pfad zurückgeben
            return found;
        }

        // ***************************************************************
        // holt den letzten node aus dem pfad
        private static PathNode popLastNode(PathNode n)
        {
            while (n.PreviousNode.PreviousNode != null)
            {
                n = n.PreviousNode;
            }

            PathNode prev = n.PreviousNode;
            n.PreviousNode = null;
            return prev;
        }

        // ***************************************************************
        // openlist etc updaten
        private static void expandNode(PathNode current)
        {
            WayPoint currentWayPoint = Main.MainObject.GameManager.GameState.Karte.WayPoints[current.ID];

            // Alle verlinkten knoten durchgehen
            foreach (WayPoint w in currentWayPoint.connectedPoints)
            {
                // falls der knoten in der closed list ist nicht unternehmen
                if (isIdInClosedList(w.ID))
                    continue;

                // weg zwischen current und neuen knoten
                float dX = currentWayPoint.Location.Position.X - w.Location.Position.X;
                float dY = currentWayPoint.Location.Position.Y - w.Location.Position.Y;
                float dist = (float)Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

                // neuer knoten erstellen
                PathNode newNode = new PathNode(current.Distance + dist, w.ID);
                newNode.PreviousNode = current;

                // In openlist schauen ob es den punkt schon gibt und prüfen ob er schneller zu erreichen ist
                PathNode nexter = GetOfOpenList(w.ID);
                if (nexter != null)
                {
                    // wenn der alte weg kürzer ist weiter machen
                    if (nexter.Distance < newNode.Distance)
                        continue;
                    else
                    {
                        // wenn der neue weg kürzer ist, node aktuallisiern
                        nexter.Distance = newNode.Distance;
                        nexter.PreviousNode = current;
                    }
                }
                else
                {
                    // knoten in open list adden
                    openList.Add(newNode);
                }
            }

            // list sortieren, damit oben der mit der kürzesten heuristik steht
            openList.Sort(CompareNodes);
        }


        private static bool isIdInClosedList(int id)
        {
            foreach (PathNode n in closedList)
                if (n.ID == id)
                    return true;
            return false;
        }

        private static PathNode GetOfOpenList(int id)
        {
            foreach (PathNode n in closedList)
                if (n.ID == id)
                    return n;
            return null;
        }*/


        // **************************************************************************
        // Vergleicht zwei PathNodes
        private static int CompareNodes(PathNode x, PathNode y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // equal 
                    return 0;
                }
                else
                {
                    // y ist nicht null somit ist y "größer"
                    return -1;
                }
            }
            else
            {
                // x nicht null

                if (y == null)
                // und y ist null ist x "größer"
                {
                    return 1;
                }
                else
                {
                    // ansonsten vergleichen

                    if (x.Distance > y.Distance)
                    {
                        return 1;
                    }
                    else if (x.Distance < y.Distance)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
