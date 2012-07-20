using System;
using LastMan.Location;

namespace LastMan.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Stabilisator : Item
    {
        //Attributes
        public float Accuracy { get; set; }

        //Constructor
        public Stabilisator( int id, EGroup group, String name, float accuracy, float weight, string description, ILocationBehavior locationBehavior )
            : base( id, group, name, description, weight, locationBehavior )
        {
            this.Accuracy = accuracy;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Stabilisator( StabilisatorInner si )
            : base( si.item )
        {
            Accuracy = si.accuracy;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class StabilisatorInner
        {
            public float accuracy;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public StabilisatorInner GetInner()
        {
            StabilisatorInner si = new StabilisatorInner();
            si.accuracy = Accuracy;
            si.item = base.GetInner();
            return si;
        }

        // ***************************************************************************
        // Clont Object
        public Stabilisator Clone()
        {
            Stabilisator s = new Stabilisator( TypeId, Group, Name, Accuracy, Weight, Description, LocationBehavior.Clone() );
            s.Renderer = Renderer;
            s.RequiredLiquid = RequiredLiquid;
            return s;
        }

        public override string ToString()
        {
            return "Stabilisator: " + base.ToString();
        }
    }
}
