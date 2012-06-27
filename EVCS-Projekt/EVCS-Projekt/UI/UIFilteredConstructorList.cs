using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.GUI;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    internal class UIFilteredConstructorList : UIPanel, UIActionListener
    {
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;


        private Dictionary<int, int> listHauptteil;
        private Dictionary<int, int> listStabilisator;
        private Dictionary<int, int> listVisier;
        private Dictionary<int, int> listAntrieb;

        private Player player;

        public Item activeItem;

        private readonly UIList inventarList;

        public UIFilteredConstructorList( int width, int height, Vector2 position, UIActionListener listener )
            : base(width, height, position)
        {
            player = Main.MainObject.GameManager.GameState.Player;
            inventarList = new UIList(260, height-40, new Vector2(0, 40), listener);

            Dictionary<int, int> tempDict = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> keyValuePair in player.Inventar)
            {
                if (Item.Get(keyValuePair.Key).GetType() != typeof(Munition) && Item.Get(keyValuePair.Key).GetType() != typeof(Powerup))
                {
                    tempDict[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            var inventarTitel = new UIButton(250, 40, new Vector2(0,0), "Inventar");
            var filterTitel = new UIButton(120, 40, new Vector2(280, 0), "Filter") { BackgroundColor = Color.LightGray };

            Add(inventarTitel);
            Add(inventarList);
            Add(filterTitel);

            inventarList.SetItems(tempDict);
            CreateCheckBoxPanel();
        }

        public void GenerateFilteredLists(Dictionary<int, int> inventar)
        {
            listHauptteil = new Dictionary<int, int>();
            listStabilisator = new Dictionary<int, int>();
            listVisier = new Dictionary<int, int>();
            listAntrieb = new Dictionary<int, int>();


            foreach (KeyValuePair<int, int> pair in inventar)
            {
                int typeId = pair.Key;
                int count = pair.Value;

                Item item = Item.Get(typeId);

                var type = item.GetType();
               
                if (type == typeof (Hauptteil))
                {
                    listHauptteil[typeId] = count;
                }
                else if (type == typeof (Stabilisator))
                {
                    listStabilisator[typeId] = count;
                }
                
                else if (type == typeof (Visier))
                {
                    listVisier[typeId] = count;
                }
                else if (type == typeof (Antrieb))
                {
                    listAntrieb[typeId] = count;
                }
                
            }
        }

        public void ResetToggleButtons()
        {
            toggleHauptteil.isActive = true;
            toggleStabilisator.isActive = true;
            toggleVisier.isActive = true;
            toggleAntrieb.isActive = true;
        }

        private void CreateCheckBoxPanel()
        {
            var content = Main.ContentManager;

            var hauptteil = content.Load<Texture2D>("images/gui/inventar/hauptteil");
            var hauptteilH = content.Load<Texture2D>("images/gui/inventar/hauptteil_h");
            var hauptteilA = content.Load<Texture2D>("images/gui/inventar/hauptteil_a");
            var hauptteilAH = content.Load<Texture2D>("images/gui/inventar/hauptteil_a_h");

            var stabilisator = content.Load<Texture2D>("images/gui/inventar/stabilisator");
            var stabilisatorH = content.Load<Texture2D>("images/gui/inventar/stabilisator_h");
            var stabilisatorA = content.Load<Texture2D>("images/gui/inventar/stabilisator_a");
            var stabilisatorAH = content.Load<Texture2D>("images/gui/inventar/stabilisator_a_h");

            var visier = content.Load<Texture2D>("images/gui/inventar/visier");
            var visierH = content.Load<Texture2D>("images/gui/inventar/visier_h");
            var visierA = content.Load<Texture2D>("images/gui/inventar/visier_a");
            var visierAH = content.Load<Texture2D>("images/gui/inventar/visier_a_h");

            var antrieb = content.Load<Texture2D>("images/gui/inventar/antrieb");
            var antriebH = content.Load<Texture2D>("images/gui/inventar/antrieb_h");
            var antriebA = content.Load<Texture2D>("images/gui/inventar/antrieb_a");
            var antriebAH = content.Load<Texture2D>("images/gui/inventar/antrieb_a_h");


            toggleHauptteil = new UIToggleButton(120, 30, new Vector2(280, 80), hauptteil, hauptteilH, hauptteilA, hauptteilAH, "") { isActive = true };
            toggleStabilisator = new UIToggleButton(120, 30, new Vector2(280, 120), stabilisator, stabilisatorH, stabilisatorA, stabilisatorAH, "") { isActive = true };
            toggleVisier = new UIToggleButton(120, 30, new Vector2(280, 160), visier, visierH, visierA, visierAH, "") { isActive = true };
            toggleAntrieb = new UIToggleButton(120, 30, new Vector2(280, 200), antrieb, antriebH, antriebA, antriebAH, "") { isActive = true };

            toggleHauptteil.AddActionListener(this);
            toggleStabilisator.AddActionListener(this);
            toggleVisier.AddActionListener(this);
            toggleAntrieb.AddActionListener(this);

            Add(toggleHauptteil);
            Add(toggleStabilisator);
            Add(toggleVisier);
            Add(toggleAntrieb);
        }

        

        public void OnMouseDown(UIElement element)
        {
            //////////////////////////////////////////////////////
            // ToggleButtons
            if (element.GetType() == typeof (UIToggleButton))
            {
                inventarList.FirsVisibleButtonIndex = 0;
            }

            if (element == toggleHauptteil)
            {
                inventarList.AddItemList(listHauptteil);
            }
            else if (element == toggleStabilisator)
            {
                inventarList.AddItemList(listStabilisator);
            }
            else if (element == toggleVisier)
            {
                inventarList.AddItemList(listVisier);
            }
            else if (element == toggleAntrieb)
            {
                inventarList.AddItemList(listAntrieb);
            }

            
        }

        public void OnMouseUp(UIElement element)
        {
            inventarList.FirsVisibleButtonIndex = 0;
            if (element == toggleHauptteil)
            {
                inventarList.RemoveItems(listHauptteil);
            }
            else if (element == toggleStabilisator)
            {
                inventarList.RemoveItems(listStabilisator);
            }
            else if (element == toggleVisier)
            {
                inventarList.RemoveItems(listVisier);
            }
            else if (element == toggleAntrieb)
            {
                inventarList.RemoveItems(listAntrieb);
            }
        }

        public void AddItem(Item item)
        {
            inventarList.AddItem(item);
        }

        public void RemoveActiveItem()
        {
            inventarList.RemoveActiveItem();
        }

        public void RefreshItemList()
        {
            inventarList.RefreshItemList();
        }

        public void SetItems(Dictionary<int, int> inventar)
        {
           inventarList.SetItems(inventar);
        }
    }
}
