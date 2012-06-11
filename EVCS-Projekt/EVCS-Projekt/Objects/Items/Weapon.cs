using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{

    public class Weapon : Item
    {

        //Attributes
        private Visier Visier { get; set; }
        public Munition Munition { get; set; }
        private Antrieb Antrieb { get; set; }
        private Stabilisator Stabilisator { get; set; }
        private Hauptteil Hauptteil { get; set; }

        // ***************************************************************************
        // Konstruktor
        public Weapon(int visierId, int antriebId, int stabilisatorId, int hauptteilId, int id, EGroup group, String name, float weight, string description, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.Visier = Item.DefaultVisiere[visierId];
            this.Antrieb = Item.DefaultAntrieb[antriebId];
            this.Stabilisator = Item.DefaultStabilisatoren[stabilisatorId];
            this.Hauptteil = Item.DefaultHauptteil[hauptteilId];
        }

        // ***************************************************************************
        // Konstruktor
        public Weapon(Visier visier, Antrieb antrieb, Stabilisator stabilisator, Hauptteil hauptteil, int id, EGroup group, String name, float weight, string description, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.Visier = visier;
            this.Antrieb = antrieb;
            this.Stabilisator = stabilisator;
            this.Hauptteil = hauptteil;
        }

        // ***************************************************************************
        // Konstruktor 3
        public Weapon(WeaponInner wi)
            : base(wi.item)
        {
            Visier = Item.DefaultVisiere[wi.visier];
            if (wi.munition != -1)
                Munition = Item.DefaultMunition[wi.munition];
            Antrieb = Item.DefaultAntrieb[wi.antrieb];
            Stabilisator = Item.DefaultStabilisatoren[wi.stabilisator];
            Hauptteil = Item.DefaultHauptteil[wi.hauptteil];
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class WeaponInner
        {
            public int visier;
            public int munition;
            public int antrieb;
            public int stabilisator;
            public int hauptteil;

            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public WeaponInner GetInner()
        {
            WeaponInner wi = new WeaponInner();

            wi.visier = Visier.Id;
            if (Munition != null)
                wi.munition = Munition.Id;
            else
                wi.munition = -1;
            wi.antrieb = Antrieb.Id;
            wi.stabilisator = Stabilisator.Id;
            wi.hauptteil = Hauptteil.Id;

            wi.item = base.GetInner();
            return wi;
        }

        // ***************************************************************************
        // schieﬂen
        public Item Shoot(float accuratcy)
        {
            return null; //Schuss generiert neues Item
        }

        // ***************************************************************************
        // Gewicht wird berechnet
        public new float Weight
        {
            get
            {
                float w = Hauptteil.Weight + Visier.Weight + Stabilisator.Weight + Antrieb.Weight;

                if (Munition != null)
                    w += Munition.Weight * Munition.Count;

                return w;
            }
        }
    }
}
