using System.Collections.Generic;
using Microsoft.Xna.Framework;
using LastMan.Map;
using LastMan.Managers;

namespace LastMan.AI
{
    public class WalkTo : Activity
    {
        private Vector2 _walkTo { get; set; }
        private PathNode _path { get; set; }

        public WalkTo(Vector2 walkTo)
        {
            _walkTo = walkTo;
        }

        public override void DoActivity(Enemy e)
        {
        }

        public override void CalculateAction(Enemy e)
        {
            if (_path == null)
            {
                WayPoint start = SearchNearest(e.LocationBehavior.Position);
                WayPoint target = SearchNearest(_walkTo);

                if (start == null || target == null)
                {
                    e.Activity = new NoActivity();
                    return;
                }

                _path = PathFinder.FindePath(start, target);
            }

            WayPoint next = Main.MainObject.GameManager.GameState.Karte.WayPoints[_path.ID];

            if (Vector2.Distance(next.Location.Position, e.LocationBehavior.Position) < next.Location.Size.X)
            {
                if (_path.NextNode != null)
                    _path = _path.NextNode;
            }
            else
            {
                e.LookAt(next.Location.Position);

                Move(e);
            }
        }

        private WayPoint SearchNearest(Vector2 position)
        {
            int sizeX = 1000, sizeY = 1000;
            List<WayPoint> wpList = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(new Rectangle((int)(position.X - sizeX / 2), (int)(position.Y - sizeY / 2), sizeX, sizeY));

            // Nearest
            WayPoint nearest = null;
            float dist = 0;

            foreach (WayPoint w in wpList)
            {
                if (nearest == null)
                {
                    if (GameManager.PointSeePoint(position, w.Location.Position))
                    {
                        // Bei null setzten
                        nearest = w;
                        dist = Vector2.Distance(w.Location.Position, position);
                    }
                }
                else
                {
                    // prüfen ob wp näher an position liegt
                    float tDist = Vector2.Distance(w.Location.Position, position);
                    if (tDist < dist && GameManager.PointSeePoint(position, w.Location.Position))
                    {
                        dist = tDist;
                        nearest = w;
                    }
                }
            }

            return nearest;
        }

        private void Move(Enemy e)
        {
            // Direction muss umgekehrt werden ( sonst läuft er rückwärts ;) )
            Vector2 moveVector = -e.LocationBehavior.Direction;

            //
            moveVector.Normalize();
            moveVector = moveVector * (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * e.Speed;

            if (e.MoveGameObject(moveVector))
            {
                e.HasMoved = true;

                // QT update
                Main.MainObject.GameManager.GameState.QuadTreeEnemies.Move(e);
            }
            else
            {
                e.HasMoved = false;
            }
        }
    }
}
