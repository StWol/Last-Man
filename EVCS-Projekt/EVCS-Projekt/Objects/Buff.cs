using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using EVCS_Projekt.Objects.Items;

namespace EVCS_Projekt.Objects
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */

    public class Buff
    {
        // Geladene Buffs
        private static Dictionary<int, Buff> LoadedBuffs { get; set; }

        //Attributes
        public float Duration { get; private set; }
        public float ValuePerSecond { get; private set; }
        public float Value { get; private set; }
        public EBuffType Type { get; private set; }

        // *********************************************************************
        //Constructor
        public Buff(float value, float valuePerSecond, float duration, EBuffType type)
        {
            Value = value;
            ValuePerSecond = valuePerSecond;
            Duration = duration;
            Type = type;
        }

        // *********************************************************************
        // Lebenszeit verringern
        public void Update()
        {
            Duration = Duration - (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
        }

        // *********************************************************************
        // Ist Buff abgelafuen?
        public bool IsExpired
        {
            get
            {
                if (Duration < 0)
                    return true;
                else
                    return false;
            }
        }

        // ***********************************************************************
        // Den berechneten Wert über die Zeit 
        public float ValueSinceLast
        {
            get
            {
                return (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * ValuePerSecond;
            }
        }

        // ***********************************************************************
        // Den berechneten Wert über die Zeit 
        public Buff Clone()
        {
            Buff b = new Buff(Value, ValuePerSecond, Duration, Type);
            return b;
        }

        // ***********************************************************************
        // Innere Klasse zum speichern
        public class BuffInner
        {
            public int RefId;
            public float Duration, ValuePerSecond, Value;
            public EBuffType BuffType;
        }

        public static void Load()
        {
            // Dic init
            LoadedBuffs = new Dictionary<int, Buff>();

            // open stream
            FileStream fs = new FileStream(Configuration.Get("buffs"), FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<BuffInner>));
            // Geladene liste
            List<BuffInner> loaded = (List<BuffInner>)serializer.Deserialize(fs);
            
            foreach (BuffInner bi in loaded)
            {
                // Buff erstellen und adden
                Buff b = new Buff( bi.Value, bi.ValuePerSecond, bi.Duration, bi.BuffType );

                LoadedBuffs.Add(bi.RefId, b);
            }
        }

        public static Buff Get(int id)
        {
            return LoadedBuffs[id].Clone();
        }
    }
}





