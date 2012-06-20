using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVCS_Projekt.UI
{
    interface UIMouseHoverListener: UIListener
    {
        void OnMouseIn(UIElement element);
        void OnMouseOut(UIElement element);
    }
}
