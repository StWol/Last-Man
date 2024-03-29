using System;
using LastMan.Location;

namespace LastMan.Objects.Items
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
        public int ShotCount { get; private set; }
        public bool BigWeapon {get; private set;}

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
            ShotCount = hi.shotCount;
            BigWeapon = hi.bigWeapon;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class HauptteilInner
        {
            public float rateOfFire;
            public ItemInner item;
            public int shotCount;
            public bool bigWeapon;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public HauptteilInner GetInner()
        {
            HauptteilInner hi = new HauptteilInner();

            hi.rateOfFire = RateOfFire;
            hi.shotCount = ShotCount;
            hi.bigWeapon = BigWeapon;

            hi.item = base.GetInner();
            return hi;
        }

        // ***************************************************************************
        // Clont Object
        public Hauptteil Clone()
        {
            Hauptteil h = new Hauptteil(TypeId, Group, Name,RateOfFire, Weight,Description, LocationBehavior.Clone());
            h.Renderer = Renderer;
            h.RequiredLiquid = RequiredLiquid;
            return h;
        }

        public override string ToString()
        {
            return "Hauptteil: " + base.ToString();
        }
    }
}
