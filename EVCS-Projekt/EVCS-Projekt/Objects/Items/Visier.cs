using System;
using LastMan.Location;
using LastMan.Objects.Items;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Collections.Generic;

namespace LastMan.Objects.Items
{
    public class Visier : Item
    {
        //Attributes
        public float Accuracy { get; set; }

        //Constructor
        public Visier( int id, EGroup group, String name, float accuracy, float weight, string description, ILocationBehavior locationBehavior )
            : base( id, group, name, description, weight, locationBehavior )
        {
            this.Accuracy = accuracy;
        }

        // ***************************************************************************
        // Konstruktor 3
        public Visier( VisierInner vi )
            : base( vi.item )
        {
            Accuracy = vi.accuracy;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class VisierInner
        {
            public float accuracy;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public VisierInner GetInner()
        {
            VisierInner vi = new VisierInner();
            vi.accuracy = Accuracy;
            vi.item = base.GetInner();
            return vi;
        }

        // ***************************************************************************
        // Clont Object
        public Visier Clone()
        {
            Visier v = new Visier( TypeId, Group, Name, Accuracy, Weight, Description, LocationBehavior.Clone() );
            v.Renderer = Renderer;
            v.RequiredLiquid = RequiredLiquid;
            return v;
        }

        public override string ToString()
        {
            return "Visier: " + base.ToString();
        }
    }
}
