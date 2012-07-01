using System;
using LastMan.Objects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using LastMan.Objects.Items;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace LastMan.Objects
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
        public static Dictionary<EBuffType, Texture2D> BuffIcons { get; private set; }

        //Attributes
        public float FullDuration { get; private set; }
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
            FullDuration = duration;
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
            b.FullDuration = FullDuration;
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
            BuffIcons = new Dictionary<EBuffType, Texture2D>();

            // open stream
            FileStream fs = new FileStream(Configuration.Get("buffs"), FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<BuffInner>));
            // Geladene liste
            List<BuffInner> loaded = (List<BuffInner>)serializer.Deserialize(fs);

            foreach (BuffInner bi in loaded)
            {
                // Buff erstellen und adden
                Buff b = new Buff(bi.Value, bi.ValuePerSecond, bi.Duration, bi.BuffType);

                LoadedBuffs.Add(bi.RefId, b);
            }

            // Icons der EBuffTypes laden
            foreach (EBuffType type in Enum.GetValues(typeof(EBuffType)))
            {
                BuffIcons.Add(type, Main.ContentManager.Load<Texture2D>(Configuration.Get("buffIcons") + type.ToString() ) );
            }
        }

        public static Buff Get(int id)
        {
            return LoadedBuffs[id].Clone();
        }
    }
}





