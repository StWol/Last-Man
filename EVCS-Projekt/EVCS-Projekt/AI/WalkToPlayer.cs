using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Map;
using EVCS_Projekt.Managers;
using System.Diagnostics;

namespace EVCS_Projekt.AI
{
    public class WalkToPlayer : Activity
    {
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
            if (GameManager.PointSeePoint(e.LocationBehavior.Position, Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position, e.LocationBehavior.Size/4))
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
                    _walkToLastPlayerPosition = false;
                else
                    return;
            }

            WayPoint target = Main.MainObject.GameManager.GameState.Player.NearestWayPoint;

            if (target == null)
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

                _path = PathFinder.FindePath(_currentWayPoint, target);
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
    }
}
