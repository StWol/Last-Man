using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVCS_Projekt
{

    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    class Buff
    {
        //Attributes
        private EBuffType type;
        private String name;
        private float value;
        private float modifier;
        private float duration;
        private bool expired;
        

        //Fuctions & Constructor

        //Begin of Cons
        public Buff(String name, float value, float modifier, float duration, EBuffType type, bool expired)
        {
            this.name = name;
            this.value = value;
            this.modifier = modifier;
            this.duration = duration;
            this.type = type;
            this.expired = false;
        }
        // End of Const



        // Getter 'n' Setter

        public String GetName()
        {
            return name;
        }

        public float GetValue()
        {
            return value;
        }

        public float GetModifikation()
        {
            return modifier;
        }

        public EBuffType GetBuffType()
        {
            return type;
        }

        //Functions

        public void UpdateDuration()
        {

        }

        public bool IsExpired()
        {
            return expired;
        }


    }
}
