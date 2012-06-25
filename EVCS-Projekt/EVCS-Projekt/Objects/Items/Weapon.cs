using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using EVCS_Projekt.Renderer;

namespace EVCS_Projekt.Objects.Items
{

    public class Weapon : Item
    {

        //Attributes
        public Visier Visier { get; private set; }
        public Munition Munition { get; set; }
        public Antrieb Antrieb { get; private set; }
        public Stabilisator Stabilisator { get; private set; }
        public Hauptteil Hauptteil { get; private set; }

        public float Cooldown { get; private set; }

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
            if (wi.munition != 0)
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

            wi.visier = Visier.TypeId;
            if (Munition != null)
                wi.munition = Munition.TypeId;
            else
                wi.munition = 0;
            wi.antrieb = Antrieb.TypeId;
            wi.stabilisator = Stabilisator.TypeId;
            wi.hauptteil = Hauptteil.TypeId;

            wi.item = base.GetInner();
            return wi;
        }

        // ***************************************************************************
        // Updaten
        public void Update(Dictionary<EBuffType, Buff> buffs)
        {
            if (Cooldown > 0)
            {
                // Cooldown verringern
                Cooldown -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
            }
        }

        // ***************************************************************************
        // schießen
        public Shot CreateShot()
        {
            //Debug.WriteLine("RateOfFire: " + RateOfFire);

            // Cooldown setzen
            ResetCooldown();

            // Schuss erzeugen
            //Shot s = Item.DefaultShots[Munition.ShotId].Clone();
            Shot s = new Shot(750F, Damage, 2000);
            s.Renderer = Munition.ShotRenderer;
            s.Damage = Damage;

            // Munition abziehen
            Munition.Count -= 1;

            //if (Munition.Count <= 0)
                //Main.MainObject.GameManager.GameState.Player.RemoveItemFromInventar(Item.Get(Munition.TypeId));
            return s;
        }


        public void Reload()
        {
            var player = Main.MainObject.GameManager.GameState.Player;

            int diff =0;
            if (player.Inventar.ContainsKey(Munition.TypeId))
            {
                if (player.Inventar[Munition.TypeId]+Munition.Count >= Munition.MagazineSize)
                {
                    diff = Munition.MagazineSize - Munition.Count;
                    Munition.Count = Munition.MagazineSize;
                }
                else
                {
                    diff = player.Inventar[Munition.TypeId];
                    Munition.Count += diff;
                }
            

            player.RemoveRangeItemFromInventar(Munition, diff);
        }
    }

        // ***************************************************************************
        // Schusscount
        public void ResetCooldown()
        {
            // Cooldown setzten
            Cooldown = 1F / RateOfFire;
        }

        // ***************************************************************************
        // Schusscount
        public int MunitionCount
        {
            get
            {
                if (Munition == null)
                    return -1;
                else
                    return Munition.Count;
            }
        }

        // ***************************************************************************
        // Feuerrate wird berechnet
        public float Damage
        {
            get
            {
                float d = Antrieb.Damage;

                if (Munition != null)
                    d += Munition.Damage;

                return d;
            }
        }

        // ***************************************************************************
        // Feuerrate wird berechnet
        public float RateOfFire
        {
            get
            {
                float r = Hauptteil.RateOfFire + Antrieb.RateOfFire;

                return r;
            }
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

        // ***************************************************************************
        // Gewicht wird berechnet
        public float Accuracy
        {
            get
            {
                float w = Math.Max((10 - (Visier.Accuracy + Stabilisator.Accuracy)) / 2 / 10, 0);

                return w;
            }
        }

        // ***************************************************************************
        // Wieviele schüsse werden geschossen
        public int ShotCount
        {
            get
            {
                return Hauptteil.ShotCount;
            }
        }

        // ***************************************************************************
        // handelt es sich um eine große waffe
        public bool BigWeapon
        {
            get
            {
                return Hauptteil.BigWeapon;
            }
        }

        // ***************************************************************************
        // Clone
        public Weapon Clone()
        {
            Weapon w = new Weapon(GetInner());
            w.Renderer = Renderer.Clone();
            w.LocationBehavior = LocationBehavior.Clone();
            return w;
        }

        
        
    }
}
