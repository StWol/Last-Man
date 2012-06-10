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
        public Vector2 Direction { get; private set; }
        public float Speed { get; private set; }
        public float Damage { get; private set; }
        public float Distance { get; private set; }
        public List<int> BuffId { get; set; }
        public bool Delete { get; set; }

        public float Lifetime { get; set; } 

        //Constructor
        public Shot(int id, EGroup group, float speed, Vector2 direction, float damage, String name, float distance, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.Speed = speed;
            this.Damage = damage;
            this.Distance = distance;

            this.Direction = direction;
            this.Direction.Normalize();

            Lifetime = 2000;// TODO: Das irgendwie auslagern oder so
        }

        public List<Buff> BuffList
        {
            get
            {
                List<Buff> l = new List<Buff>();
                return l;
            }
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Shot(ShotInner si)
            : base(si.item)
        {
            Direction = si.direction;
            Speed = si.speed;
            Damage = si.damage;
            si.distance = Distance;
            Lifetime = si.lifetime;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class ShotInner
        {
            public Vector2 direction;
            public float speed, damage, distance, lifetime;
            public List<int> buffId;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public ShotInner GetInner()
        {
            ShotInner si = new ShotInner();

            si.direction = Direction;
            si.speed = Speed;
            si.damage = Damage;
            si.distance = Distance;
            si.lifetime = Lifetime;

            si.item = base.GetInner();
            return si;
        }

        // ********************************************************************************
        // Lässt den Schuss "weiter fliegen"
        public void UpdatePosition()
        {

            Vector2 moveVector = new Vector2((float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * Speed * Direction.X), (float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * Speed * Direction.Y));

            base.LocationBehavior.Position = base.LocationBehavior.Position + moveVector;

            Lifetime -= (float)Math.Sqrt(Math.Pow(moveVector.X, 2) + Math.Pow(moveVector.Y, 2));
            if (Lifetime <= 0)
                Main.MainObject.GameManager.GameState.ShotListVsEnemies.Remove(this);
        }

        // ********************************************************************************
        // Schuss ein Pixel zurüclfliegen lassen
        public void AdjustShot()
        {

            Vector2 moveVector = new Vector2(-(float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * Speed * Direction.X), -(float)(Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * Speed * Direction.Y));
            moveVector.Normalize();

            base.LocationBehavior.Position = base.LocationBehavior.Position + moveVector;
        }


        // ***************************************************************************
        // Clont Object
        public Shot Clone()
        {
            Shot s = new Shot(Id, Group, Speed, new Vector2(Direction.X, Direction.Y), Damage, Name, Distance, Description, Weight, LocationBehavior.Clone());
            return s;
        }
    }
}
