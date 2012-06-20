using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{
   
    public class Powerup : Item
    {
        //Attributes
        private float regeneration;

        //Constructor
        public Powerup( int id, EGroup group, String name, float regeneration, string description, float weight, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.regeneration = regeneration;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Powerup(PowerupInner pi)
            : base(pi.item)
        {
            regeneration = pi.regeneration;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class PowerupInner
        {
            public float regeneration;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public PowerupInner GetInner()
        {
            PowerupInner pi = new PowerupInner();

            pi.regeneration = regeneration;

            pi.item = base.GetInner();
            return pi;
        }

        //Functions
        public float GetRegeneration()
        {
            return this.regeneration;
        }

        public Powerup Clone () {
            return new Powerup(GetInner());
        }
    }
}
