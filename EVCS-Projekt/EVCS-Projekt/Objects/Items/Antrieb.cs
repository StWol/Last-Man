using System;
using LastMan.Location;

namespace LastMan.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Antrieb : Item
    {

        //Attributes
        public float RateOfFire { get; private set; }
        public float Damage { get; private set; }
        private Buff damageBuff;
        public string SoundId { get; private set; }

        //Constructor
        public Antrieb( int id, EGroup group, String name, float rateOfFire, float damage, string soundId, float weight, string description, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            SoundId = soundId;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Antrieb(AntriebInner ai)
            : base(ai.item)
        {
            RateOfFire = ai.rateOfFire;
            Damage = ai.damage;
            SoundId = ai.soundId;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class AntriebInner
        {

            public float rateOfFire, damage;
            public ItemInner item;
            public string soundId;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public AntriebInner GetInner()
        {
            AntriebInner ai = new AntriebInner();

            ai.rateOfFire = RateOfFire;
            ai.damage = Damage;
            ai.soundId = SoundId;

            ai.item = base.GetInner();
            return ai;
        }

        //Functions
        public float GetDamage()
        {
            return damageBuff.ValuePerSecond;
        }

        // ***************************************************************************
        // Clont Object
        public Antrieb Clone()
        {
            Antrieb a = new Antrieb(TypeId, Group, Name, RateOfFire, Damage, SoundId, Weight, Description, LocationBehavior.Clone());
            a.RequiredLiquid = RequiredLiquid;
            a.Renderer = Renderer;
            return a;
        }

        public override string ToString()
        {
            return "Antrieb: " + base.ToString();
        }
    }
}
