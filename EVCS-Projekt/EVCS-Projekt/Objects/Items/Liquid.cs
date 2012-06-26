using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EVCS_Projekt.Renderer;

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
        public int Amount { get; private set; }
        public ELiquid TypeOfLiquid { get; private set; }

        public static Liquid Get(ELiquid type, int amount)
        {
            return new Liquid(type, amount);
        }

        public Liquid(int id, ELiquid typeOfLiquid, String name, int amount, EGroup group, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.Amount = amount;
            this.TypeOfLiquid = typeOfLiquid;
        }

        public Liquid(ELiquid typeOfLiquid, int amount )
            : base(0, 0, "", "", 0, null)
        {
            Amount = amount;
            TypeOfLiquid = typeOfLiquid;

            switch (typeOfLiquid)
            {
                case ELiquid.Green:
                    Renderer = LoadedRenderer.Get("S_Liquid_Green");
                    break;
                case ELiquid.Blue:
                    Renderer = LoadedRenderer.Get("S_Liquid_Blue");
                    break;
                case ELiquid.Red:
                    Renderer = LoadedRenderer.Get("S_Liquid_Red");
                    break;
            }
        }

        // ***************************************************************************
        // Clont Object
        public Liquid Clone()
        {
            Liquid l = new Liquid(TypeId, TypeOfLiquid, Name, Amount, Group, Description, Weight, LocationBehavior.Clone());
            return l;
        }

    }
}
