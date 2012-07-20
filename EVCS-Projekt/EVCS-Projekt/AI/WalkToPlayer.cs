using Microsoft.Xna.Framework;
using LastMan.Map;
using LastMan.Managers;

namespace LastMan.AI
{
    public class WalkToPlayer : Activity
    {
        /*
        // Calculate
        private PathNode _path { get; set; }
        private WayPoint _target { get; set; }
        private WayPoint _currentWayPoint { get; set; }
        private Vector2 _lastPlayerPosition { get; set; }
        private bool _walkToLastPlayerPosition { get; set; }

        // Do
        private bool _lookAtPlayer { get; set; }
        private bool _attacking { get; set; }

        public WalkToPlayer()
        {
            _attacking = true;
        }

        public override void CalculateAction(Enemy e)
        {
            if (GameManager.PointSeePoint(e.LocationBehavior.Position, Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position, e.LocationBehavior.Size))
            {
                e.CanSeePlayer = true;

                _lookAtPlayer = true;
                _lastPlayerPosition = Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position;
                _walkToLastPlayerPosition = false;
                return;
            }
            else
            {
                e.CanSeePlayer = false;

                // Wenn er player aus den augen verloren hat, nearest waypoint suchen
                if (_lookAtPlayer == true)
                {
                    _path = null;
                    //_currentWayPoint = Karte.SearchNearest(_lastPlayerPosition);
                    _walkToLastPlayerPosition = true;
                }

                _lookAtPlayer = false;
            }

            if (_walkToLastPlayerPosition)
            {
                if (Vector2.Distance(_lastPlayerPosition, e.LocationBehavior.Position) < e.LocationBehavior.Size.X)
                {
                    _walkToLastPlayerPosition = false;
                    _currentWayPoint = null;
                } 
                else
                    return;
            }

            WayPoint target = Main.MainObject.GameManager.GameState.Player.NearestWayPoint;

            if (target == null || target == _target)
                return;

            if (_path == null || target != _target && !PathNode.IsWaypointInPath(target.ID, _path))
            {
                //Debug.WriteLine("target = " + target.ID);
                _target = target;

                if (_currentWayPoint == null)
                    _currentWayPoint = Karte.SearchNearest(e.LocationBehavior.Position);


                if (_currentWayPoint == null || target == null)
                {
                    e.Activity = new NoActivity();
                    return;
                }

                double sT = DateTime.Now.Ticks;
                _path = PathFinder.FindePath(_currentWayPoint, target);
                Debug.WriteLine(( DateTime.Now.Ticks-sT)/10);
            }

            WayPoint next = Main.MainObject.GameManager.GameState.Karte.WayPoints[_path.ID];

            if (Vector2.Distance(next.Location.Position, e.LocationBehavior.Position) < next.Location.Size.X)
            {
                _currentWayPoint = Main.MainObject.GameManager.GameState.Karte.WayPoints[next.ID];

                if (_path.NextNode != null)
                    _path = _path.NextNode;
            }



            //Debug.WriteLineIf(_path == null, "path is null");
        }

        private void Move(Enemy e)
        {
            // Direction muss umgekehrt werden ( sonst läuft er rückwärts ;) )
            Vector2 moveVector = -e.LocationBehavior.Direction;

            //
            moveVector.Normalize();
            moveVector = moveVector * (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * e.Speed;

            Vector2 moved = new Vector2();

            if ( e.MoveGameObject(moveVector, out moved, false, true, true))
            {
                e.HasMoved = true;

                // QT update
                Main.MainObject.GameManager.GameState.QuadTreeEnemies.Move(e);

                float dist = moved.Length() / (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

                if (dist < e.Speed * 0.75F)
                    e.HasMoved = false;
                //Debug.WriteLine(e.LocationBehavior.Position + " "+dist);
            }
            else
            {
                e.HasMoved = false;
            }
        }

        public override void DoActivity(Enemy e)
        {
            bool move = true;

            if (_walkToLastPlayerPosition)
            {
                e.LookAt(_lastPlayerPosition);
            }
            else if (_lookAtPlayer)
            {
                e.LookAt(Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position);
            }
            else if (_path != null)
            {
                WayPoint next = Main.MainObject.GameManager.GameState.Karte.WayPoints[_path.ID];

                e.LookAt(next.Location.Position);
            }

            // attack when distance < attackdistance
            if (_attacking && e.CanSeePlayer && e.DistanceLessThan(Main.MainObject.GameManager.GameState.Player, e.AttackDistance) )
            {
                if ( e.DistanceLessThan(Main.MainObject.GameManager.GameState.Player, e.AttackDistance/2) )
                    move = false;

                //e.LookAt(Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position);
                e.Attack();
            }

            if (move)
            {
                Move(e);

                if (!e.HasMoved && _walkToLastPlayerPosition)
                {
                    //_walkToLastPlayerPosition = false;
                    //_currentWayPoint = searchNextInPath(e);
                    //_path = null;
                }
            }
            else
                e.HasMoved = false;
        }

        private WayPoint searchNextInPath(Enemy e)
        {
            WayPoint w = null;
            PathNode p = _path;

            while (p != null)
            {
                if (GameManager.PointSeePoint(e.LocationBehavior.Position, Main.MainObject.GameManager.GameState.Karte.WayPoints[p.ID].Location.Position))
                {
                    w = Main.MainObject.GameManager.GameState.Karte.WayPoints[p.ID];
                    p = _path.NextNode;
                }
                else
                {
                    return w;
                }
            }

            return w;
        }
         * */

        ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////

        //
        private static Player _player = Main.MainObject.GameManager.GameState.Player;

        // wegfindung
        private WayPoint _nearest;
        private PathNode _currentPath;

        // todo
        private bool _lookAtNextNode = false;
        private bool _lookAtPlayer = false;
        private bool _attack = false;
        private bool _walk = false;

        private bool _sawPlayer = false;
        private Vector2 _sawPlayerAt = new Vector2();
        private WayPoint _lastPlayerWaypoint;

        private float _walkedDistance = 0;

        // Tue die Action
        public override void CalculateAction(Enemy e)
        {
            // ist player in sichweite
            if (e.DistanceLessThan(_player, e.SightiningDistance))
            {
                // Kann er den playe sehen oder ist dieser von einem objekt bedeckt / 20x20 ist ca. ein schuss
                if (GameManager.PointSeePoint(e.LocationBehavior.Position, _player.LocationBehavior.Position, new Vector2(20, 20)))
                {
                    // kann spieler sehen
                    _sawPlayer = true;
                    _sawPlayerAt = _player.LocationBehavior.Position;

                    // wenn er in angriffreichweite ist angreifen
                    if (e.DistanceLessThan(_player, e.SightiningDistance))
                    {
                        _lookAtPlayer = true;
                        _attack = true;
                        _lookAtNextNode = false;
                        _walk = false;

                        return;
                    }
                    else
                    {
                        // Kann er zum spieler laufen ohne an eine wand zu stoßen?
                        if (GameManager.PointSeePoint(e.LocationBehavior.Position, _player.LocationBehavior.Position, e.LocationBehavior.Size))
                        {
                            _lookAtPlayer = true;
                            _attack = false;
                            _lookAtNextNode = false;
                            _walk = true;

                            return;
                        }
                    }
                }
            }
            else
            {
                _lookAtPlayer = false;
                _attack = false;
            }

            // ## Sieht er den player und kann schießen wird abgebrochen

            // wenn er den spiele sehen konnte aber jetzt nicht mehr
            if (_sawPlayer)
            {
                // sieht player nicht mehr
                _sawPlayer = false;

                // Suche den nähsten WP und schaue ob er auf der route lag
                WayPoint _newNearest = Karte.SearchNearest(_sawPlayerAt);

                if (_newNearest != null)
                {

                    // nearest in path
                    PathNode nearestInPath = null;

                    // wenn der nähste wegpunkt im pfad ist, diesen als neues ziel setzten
                    if (_currentPath.IsWaypointInPath(_newNearest.ID, out nearestInPath))
                    {
                        _currentPath = nearestInPath;
                    }
                    else
                    {
                        // ansonsten den aktuellen pfad löschen und neuen berechnen
                        _nearest = _newNearest;
                        _currentPath = null;
                    }

                }
            }

            //

            // WP des Players
            WayPoint playerWayPoint = Main.MainObject.GameManager.GameState.Player.NearestWayPoint;

            // falls playerwegpunkt neu ist pfad neu berechnen und auch neuen nearest suchen
            if (playerWayPoint != null && _lastPlayerWaypoint != playerWayPoint)
            {
                _nearest = null;
                _currentPath = null;
            }

            // Falls es kein Wegpunkt gibt, der am nähesten bei mir ist
            if (_nearest == null)
                _nearest = Karte.SearchNearest(e.LocationBehavior.Position);

            // Sollte es kein Wegpunkt in meiner Nähe geben abbrechen
            if (_nearest == null)
                return;

            // wenn es kein pfad gigt, pfad zu player berechnen
            if (_currentPath == null && playerWayPoint != null)
            {
                _lastPlayerWaypoint = playerWayPoint;
                _currentPath = PathFinder.FindePath(_nearest, playerWayPoint);
            }

            // wenn _currentPath == null stehen bleiben )
            if (_currentPath != null)
            {
                // Steht enemy über wp => next wp
                if (Vector2.Distance(_currentPath.WayPoint.Location.Position, e.LocationBehavior.Position) < 10)
                {
                    // wenn es einen nächste wp gibt gehe zu dem ansonsten laufen=false /stehen bleiben
                    if (_currentPath.NextNode == null)
                    {
                        _lookAtPlayer = false;
                        _attack = false;
                        _lookAtNextNode = false;
                        _walk = false;
                    }
                    else
                    {
                        _currentPath = _currentPath.NextNode;

                        _lookAtPlayer = false;
                        _attack = false;
                        _lookAtNextNode = true;
                        _walk = true;
                    }
                }
                else
                {
                    // checken ob enemy festhängt - hängt fest wenn er halb so schnell wie normal läuft
                    if (_walkedDistance < e.Speed / 2)
                    {
                        searchNextVisibleNode(e);
                    }

                    _lookAtPlayer = false;
                    _attack = false;
                    _lookAtNextNode = true;
                    _walk = true;
                }
            }
            else
            {
                _lookAtPlayer = false;
                _attack = false;
                _lookAtNextNode = false;
                _walk = false;
            }
        }

        private void searchNextVisibleNode(Enemy e)
        {
            // wenn es kein pfad gibt abbrechen
            if (_currentPath == null)
                return;

            PathNode temp = _currentPath.NextNode;

            // nodes durchgenen
            while (temp != null)
            {
                if (GameManager.PointSeePoint(e.LocationBehavior.Position, temp.WayPoint.Location.Position, e.LocationBehavior.Size))
                {
                    _currentPath = temp;
                    temp = _currentPath.NextNode;
                }
                else
                {
                    // aufhören zu suchen
                    break;
                }
            }
        }

        // Tue die Action
        public override void DoActivity(Enemy e)
        {
            // laufe zu nächstem pfad
            if (_lookAtNextNode)
            {
                PathNode next = _currentPath;

                // schaue zu nächstem wegpunkt
                if (next != null)
                    e.LookAt(next.WayPoint.Location.Position);
            }

            // schaue auf player
            if (_lookAtPlayer)
            {
                e.LookAt(_player.LocationBehavior.Position);
            }

            // greife an
            if (_attack && Main.MainObject.GameManager.GameState.Player.IsAlive)
            {
                e.Attack();
            }

            // laufe gerade aus (richtung wp)
            if (_walk)
            {
                Vector2 moved = Move(e);

                _walkedDistance = (float)(moved.Length() / Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds);
            }
        }

        // Laufe gerade aus
        private Vector2 Move(Enemy e)
        {
            // Direction - muss umgekehrt werden sonst läuft er rückwärts
            Vector2 moveVector = -e.LocationBehavior.Direction;

            // Geschwindigkeit berechnen
            moveVector.Normalize();
            moveVector = moveVector * (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * e.Speed;

            // Wieviel gelaufen wurde
            Vector2 moved = new Vector2();

            // Laufe falls möglich
            if (e.MoveGameObject(moveVector, out moved, false, true, true))
            {
                // ist gelaufen
                e.HasMoved = true;

                // QT update
                Main.MainObject.GameManager.GameState.QuadTreeEnemies.Move(e);
            }
            else
            {
                // konnte nicht laufen
                e.HasMoved = false;
            }

            return moved;
        }
    }
}
