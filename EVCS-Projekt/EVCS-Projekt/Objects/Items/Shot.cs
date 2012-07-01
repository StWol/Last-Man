using System;
using System.Collections.Generic;
using LastMan.Location;
using LastMan.Objects;
using LastMan.Objects.Items;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LastMan.Objects.Items
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
        public float Damage { get; set; }
        public float Distance { get; private set; }
        public Dictionary<EBuffType, Buff> Buffs { get; set; }
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

            Buffs = new Dictionary<EBuffType, Buff>();
        }

        //Constructor
        public Shot(float speed, float damage, float distance)
            : base(0, 0, "", "", 0, new MapLocation(new Vector2()) )
        {
            this.Speed = speed;
            this.Damage = damage;
            this.Distance = distance;

            Lifetime = 2000;// TODO: Das irgendwie auslagern oder so
        }

        // Buff adden
        public void AddBuff(Buff buff)
        {
            if (Buffs.ContainsKey(buff.Type))
                Buffs[buff.Type] = buff;
            else
                Buffs.Add(buff.Type, buff);
        }

        // ***************************************************************************
        // Setztt die Direction hier und im location
        public new void SetDirection(Vector2 d)
        {
            Direction = d;

            base.SetDirection(d);
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
            Shot s = new Shot(TypeId, Group, Speed, new Vector2(Direction.X, Direction.Y), Damage, Name, Distance, Description, Weight, LocationBehavior.Clone());
            s.Renderer = Renderer;
            return s;
        }

        public override string ToString()
        {
            return "Shot: " + base.ToString();
        }
    }
}
