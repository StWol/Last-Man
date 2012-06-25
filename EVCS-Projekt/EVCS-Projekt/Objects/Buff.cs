using System;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */

    public class Buff
    {

        //Attributes
        public float Duration { get; private set; }
        public float ValuePerSecond { get; private set; }
        public float Value { get; private set; }
        public EBuffType Type { get; private set; }

        // *********************************************************************
        //Constructor
        public Buff(float value, float modifier, float duration, EBuffType type)
        {
            Value = value;
            ValuePerSecond = modifier;
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
    }
}





