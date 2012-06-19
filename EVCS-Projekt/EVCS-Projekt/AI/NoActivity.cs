using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVCS_Projekt.AI
{
    class NoActivity : Activity
    {
        // Macht nichts
        public override void CalculateAction(Enemy e)
        {
        }

        public override void DoActivity(Enemy e)
        {
        }
    }
}
