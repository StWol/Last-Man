using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Renderer;
using System.Collections.Generic;

namespace EVCS_Projekt.Objects.Items
{

    public class Powerup : Item
    {

        // ValuePerSecond
        public IRenderBehavior Effect { get; private set; }
        public Vector2 EffectSize { get; private set; }

        private List<int> BuffRefList { get; set; }
        public float Duration { get; set; }

        //Constructor
        public Powerup(int id, EGroup group, String name, float regeneration, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Powerup(PowerupInner pi)
            : base(pi.item)
        {
            Effect = LoadedRenderer.Get(pi.effect);
            BuffRefList = pi.buffRefList;

            // Kleiner Workarround um einfach an die Größe des Effekts zu kommen :)
            IRenderBehavior temp = Renderer;
            Renderer = Effect;
            LocationSizing();
            EffectSize = LocationBehavior.Size;
            Renderer = temp;
            LocationSizing();

            // Duration setzten
            foreach (int i in BuffRefList)
            {
                if (Duration < Buff.Get(i).Duration)
                    Duration = Buff.Get(i).Duration;
            }
        }

        // Buffs des Powerups holen
        public List<Buff> Buffs
        {
            get
            {
                List<Buff> list = new List<Buff>();

                foreach (int i in BuffRefList)
                {
                    list.Add(Buff.Get(i));
                }

                return list;
            }
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class PowerupInner
        {
            public string effect;
            public List<int> buffRefList;

            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public PowerupInner GetInner()
        {
            PowerupInner pi = new PowerupInner();

            pi.effect = Effect.Name;
            pi.buffRefList = BuffRefList;

            pi.item = base.GetInner();
            return pi;
        }

        public Powerup Clone()
        {
            Powerup p = new Powerup(GetInner());
            p.Renderer = Renderer.Clone();
            p.LocationBehavior = LocationBehavior.Clone();
            return p;
        }

        public override string ToString()
        {
            return "Powerup: " + base.ToString();
        }
    }
}
