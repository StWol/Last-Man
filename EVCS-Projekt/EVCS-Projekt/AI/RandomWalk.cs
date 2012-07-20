namespace LastMan.AI
{
    public class RandomWalk : Activity
    {
        public override void DoActivity(Enemy e)
        {
        }

        /*private WayPoint SearchNearest(Vector2 position)
        {
            int sizeX = 1000, sizeY = 1000;
            List<WayPoint> wpList = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(new Rectangle( (int)(position.X-sizeX/2), (int)(position.Y-sizeY/2),sizeX,sizeY ) );
            
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

        private WayPoint previous = null;
        private WayPoint target = null;
        private bool noWayPointVisible = false;

        private float Stand = 0;

        private float currentRotation;
        private float targetRotation;
        private float rotationDuration = 0.5F;
        private int rotation;
        private float rotationTime;

        // Läuft zufällig in der Gegend rum*/
        public override void CalculateAction(Enemy e)
        {
           /* if ( noWayPointVisible)
                return;

            // Nähster WP
            if (target == null)
            {
                target = SearchNearest(e.LocationBehavior.Position);

                if (target == null)
                {
                    noWayPointVisible = true;
                }
                else
                {
                    e.LookAt(target.Location.Position);
                }
            }
            else
            {
                Random r = new Random();

                if (Vector2.Distance(target.Location.Position, e.LocationBehavior.Position) < target.Location.Size.X)
                {
                    // Nächster WP
                    WayPoint nextTarget = null;

                    currentRotation = e.LocationBehavior.Rotation;

                    // Anzahl der möglichkeiten
                    int anzWP = target.connectedPoints.Count;

                    if (anzWP == 1)
                    {
                        nextTarget = target.connectedPoints[0];
                    }
                    else if (anzWP == 2)
                    {
                        if (target.connectedPoints[0] == previous)
                            nextTarget = target.connectedPoints[1];
                        else
                            nextTarget = target.connectedPoints[0];
                    }
                    else
                    {
                        // Möglichkeiten
                        List<WayPoint> possibleWayPoints = new List<WayPoint>(target.connectedPoints);

                        // previous rausnehmen
                        possibleWayPoints.Remove(previous);

                        // nächster besimmen
                        nextTarget = possibleWayPoints[r.Next(possibleWayPoints.Count)];

                        //Chance stehn zu bleiben
                        if (r.NextDouble() > 0.7)
                            Stand = r.Next(1, 3);
                    }

                    previous = target;
                    target = nextTarget;


                    e.LookAt(target.Location.Position);
                    targetRotation = e.LocationBehavior.Rotation;

                    e.LocationBehavior.Rotation = currentRotation;

                    rotation = (int)((targetRotation - currentRotation) / rotationDuration);
                    rotationTime = rotationDuration;
                }

                if (Stand > 0)
                {
                    Stand -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    // Rotieren
                    if (rotationTime > 0)
                    {
                        rotationTime -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

                        float newR = e.LocationBehavior.Rotation + rotation * (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

                        e.SetRotation(newR, true);

                        if (rotationTime < 0)
                            e.LookAt(target.Location.Position);
                    }

                    // Laufen
                    Move(e);
                }
            }*/
        }

        private void Move(Enemy e)
        {/*
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
            }*/
        }
    }
}
