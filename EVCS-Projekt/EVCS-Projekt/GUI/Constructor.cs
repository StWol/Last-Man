using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.GUI
{
    class Constructor : UIPanel
    {
        private bool isVisible;
         private UIFilteredList filteredList;
        private Player player;
        private Item  activeItem;

        public bool Visible
        {
            get { return isVisible; }
            set
            {
                if ( value && !isVisible )
                {
                    filteredList.GenerateFilteredLists( player.Inventar );
                    //GenerateUiComponents();
                    filteredList.SetItems( player.Inventar );
                    filteredList.ResetToggleButtons();
                    activeItem = null;
                }
                isVisible = value;
            }
        }

        public Constructor( int width, int height, Vector2 position )
            : base( width, height, position )
        {

        }
    }
}
