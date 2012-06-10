using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using EVCS_Projekt;

namespace EVCS_Projekt.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Hauptteil : Item
    {
        //Attributes
        public float RateOfFire { get; private set; }

        //Constructor
        public Hauptteil(int id, EGroup group, String name, float rateOfFire, float weight, string description, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            RateOfFire = rateOfFire;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Hauptteil(HauptteilInner hi)
            : base(hi.item)
        {
            RateOfFire = hi.rateOfFire;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class HauptteilInner
        {
            public float rateOfFire;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public HauptteilInner GetInner()
        {
            HauptteilInner hi = new HauptteilInner();

            hi.rateOfFire = RateOfFire;

            hi.item = base.GetInner();
            return hi;
        }

        // ***************************************************************************
        // Clont Object
        public Hauptteil Clone()
        {
            Hauptteil h = new Hauptteil(Id, Group, Name,RateOfFire, Weight,Description, LocationBehavior.Clone());
            return h;
        }

    }
}
