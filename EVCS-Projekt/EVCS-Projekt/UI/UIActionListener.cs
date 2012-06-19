using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVCS_Projekt.UI
{
    interface UIActionListener
    {
        void OnMouseDown(UIElement element);
        void OnMouseUp(UIElement element);
    }
}
