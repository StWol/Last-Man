using System;
using System.Collections.Generic;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace EVCS_Projekt.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Shot : Item
    {
        //Attributes
        private Vector2 direction;
        private float speed;
        public float Damage { get; private set; }
        private float distance;
        public List<Buff> BuffList { get; set; }
        public bool Delete { get; set; }

        private float lifetime = 2000; // TODO: Das irgendwie auslagern oder so

        //Constructor
        public Shot(int id, EGroup group, float speed, Vector2 direction, float damage, String name, float distance, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.speed = speed;
            this.Damage = damage;
            this.distance = distance;

            this.direction = direction;
            this.direction.Normalize();
        }

        // ********************************************************************************
        // Lässt den Schuss "weiter fliegen"
        public void UpdatePosition()
        {
            
            Vector2 moveVector = new Vector2((float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * speed * direction.X), (float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * speed * direction.Y));

            base.LocationBehavior.Position = base.LocationBehavior.Position + moveVector;

            lifetime -= (float)Math.Sqrt(Math.Pow(moveVector.X, 2) + Math.Pow(moveVector.Y, 2));
            if (lifetime <= 0)
                Main.MainObject.GameManager.GameState.ShotListVsEnemies.Remove(this);
        }

        // ********************************************************************************
        // Schuss ein Pixel zurüclfliegen lassen
        public void AdjustShot ()
        {

            Vector2 moveVector = new Vector2(-(float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * speed * direction.X), -(float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * speed * direction.Y));
            moveVector.Normalize();

            base.LocationBehavior.Position = base.LocationBehavior.Position + moveVector;
        }

    }
}
