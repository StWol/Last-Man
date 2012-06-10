using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{
    public enum ELiquid
    {
        Green,
        Blue,
        Red
    }

    public class Liquid : Item
    {

        //Attributes
        public float Amount { get; private set; }

        public ELiquid TypeOfLiquid { get; private set; }


        public Liquid(int id, ELiquid typeOfLiquid, String name, float amount, EGroup group, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.Amount = amount;
            this.TypeOfLiquid = typeOfLiquid;
        }

        // ***************************************************************************
        // Clont Object
        public Liquid Clone()
        {
            Liquid l = new Liquid(Id, TypeOfLiquid, Name, Amount, Group, Description, Weight, LocationBehavior.Clone());
            return l;
        }

    }
}
