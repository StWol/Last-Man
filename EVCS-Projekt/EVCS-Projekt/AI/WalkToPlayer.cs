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
        private PathNode _path { get; set; }
        private WayPoint _target { get; set; }

        public WalkToPlayer()
        {
        }

        public override void DoAction(Enemy e)
        {
            WayPoint target = Main.MainObject.GameManager.GameState.Player.NearestWayPoint;

            if (target == null)
                return;

            if (_path == null || target != _target)
            {
                Debug.WriteLine("target = "+target.ID);
                _target = target;

                WayPoint start = Karte.SearchNearest(e.LocationBehavior.Position);
                

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
