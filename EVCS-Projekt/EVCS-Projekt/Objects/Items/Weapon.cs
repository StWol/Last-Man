using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{

    public class Weapon : Item
    {

        //Attributes
        private Visier visier;
        public Munition Munition { get; set; }
        private Antrieb antrieb;
        private Stabilisator stabilisator;
        private Hauptteil hauptteil;

        // ***************************************************************************
        // Konstruktor
        public Weapon( Visier visier, Antrieb antrieb, Stabilisator stabilisator, Hauptteil hauptteil, int id, EGroup group, String name, float weight, string description, ILocationBehavior locationBehavior)
            : base(   id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.visier = visier;
            this.antrieb = antrieb;
            this.stabilisator = stabilisator;
            this.hauptteil = hauptteil;
        }

        // ***************************************************************************
        // schieﬂen
        public Item Shoot (float accuratcy) 
        {
            return null; //Schuss generiert neues Item
        }

    }
}
