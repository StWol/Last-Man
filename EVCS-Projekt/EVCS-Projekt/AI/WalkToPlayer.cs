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

        // Do
        private bool _lookAtPlayer { get; set; }

        public WalkToPlayer()
        {
        }

        public override void CalculateAction(Enemy e)
        {
            if (GameManager.PointSeePoint(e.LocationBehavior.Position, Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position, e.LocationBehavior.Size))
            {
                _lookAtPlayer = true;
                _lastPlayerPosition = Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position;

                return;
            }
            else
            {
                // Wenn er player aus den augen verloren hat, nearest waypoint suchen
                if (_lookAtPlayer == true)
                {
                    _path = null;
                    _currentWayPoint = Karte.SearchNearest(_lastPlayerPosition);
                }

                _lookAtPlayer = false;
            }

            WayPoint target = Main.MainObject.GameManager.GameState.Player.NearestWayPoint;

            if (target == null)
                return;

            if (_path == null || target != _target)
            {
                Debug.WriteLine("target = " + target.ID);
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

        public override void DoActivity(Enemy e)
        {
            if (_lookAtPlayer)
            {
                e.LookAt(Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position);
            }
            else if (_path != null)
            {
                WayPoint next = Main.MainObject.GameManager.GameState.Karte.WayPoints[_path.ID];

                e.LookAt(next.Location.Position);
            }

            Move(e);
        }
    }
}
