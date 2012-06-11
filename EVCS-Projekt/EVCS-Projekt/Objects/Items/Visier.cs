using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Collections.Generic;

namespace EVCS_Projekt.Objects.Items
{
    public class Visier : Item
    {
        //Attributes
        private float accuracy;

        //Constructor
        public Visier(int id, EGroup group, String name, float accuracy, float weight, string description, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.accuracy = accuracy;
        }

        //Functions
        public float GetAccuracy()
        {
            return accuracy;
        }

        // ***************************************************************************
        // Konstruktor 3
        public Visier(VisierInner vi)
            : base(vi.item)
        {
            accuracy = vi.accuracy;
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
            vi.accuracy = accuracy;
            vi.item = base.GetInner();
            return vi;
        }

        // ***************************************************************************
        // Clont Object
        public Visier Clone()
        {
            Visier v = new Visier(Id, Group, Name, accuracy, Weight, Description, LocationBehavior.Clone());
            v.Renderer = Renderer;
            return v;
        }


    }
}
