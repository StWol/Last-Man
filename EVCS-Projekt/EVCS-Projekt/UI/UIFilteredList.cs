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
    internal class UIFilteredList : UIPanel, UIActionListener
    {
        private UIToggleButton toggleWaffe;
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleMunition;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;
        private UIToggleButton togglePowerup;


        private Dictionary<int, int> listWaffe;
        private Dictionary<int, int> listHauptteil;
        private Dictionary<int, int> listStabilisator;
        private Dictionary<int, int> listMunition;
        private Dictionary<int, int> listVisier;
        private Dictionary<int, int> listAntrieb;
        private Dictionary<int, int> listPowerup;

        private Player player;

        public Item activeItem;

        private readonly UIList inventarList;

        public UIFilteredList(int width, int height, Vector2 position, UIActionListener listener)
            : base(width, height, position)
        {
            unscaledWidth = width;
            unscaledHeight = height;
            unscaledPos = position;

            player = Main.MainObject.GameManager.GameState.Player;
            inventarList = new UIList(260, 236, new Vector2(0, 0), listener);
            inventarList.AddItemList(player.Inventar);

            Add(inventarList);
            CreateCheckBoxPanel();
        }

        public void GenerateFilteredLists(Dictionary<int, int> inventar)
        {
            listWaffe = new Dictionary<int, int>();
            listHauptteil = new Dictionary<int, int>();
            listStabilisator = new Dictionary<int, int>();
            listMunition = new Dictionary<int, int>();
            listVisier = new Dictionary<int, int>();
            listAntrieb = new Dictionary<int, int>();
            listPowerup = new Dictionary<int, int>();


            foreach (KeyValuePair<int, int> pair in inventar)
            {
                int typeId = pair.Key;
                int count = pair.Value;

                Item item = Item.Get(typeId);

                var type = item.GetType();
                if (type == typeof (Weapon))
                {
                    listWaffe[typeId] = count;
                }
                else if (type == typeof (Hauptteil))
                {
                    listHauptteil[typeId] = count;
                }
                else if (type == typeof (Stabilisator))
                {
                    listStabilisator[typeId] = count;
                }
                else if (type == typeof (Munition))
                {
                    listMunition[typeId] = count;
                }
                else if (type == typeof (Visier))
                {
                    listVisier[typeId] = count;
                }
                else if (type == typeof (Antrieb))
                {
                    listAntrieb[typeId] = count;
                }
                else if (type == typeof (Powerup))
                {
                    listPowerup[typeId] = count;
                }
            }
        }

        public void ResetToggleButtons()
        {
            toggleWaffe.isActive = true;
            toggleHauptteil.isActive = true;
            toggleStabilisator.isActive = true;
            toggleMunition.isActive = true;
            toggleVisier.isActive = true;
            toggleAntrieb.isActive = true;
            togglePowerup.isActive = true;
        }

        private void CreateCheckBoxPanel()
        {
            ContentManager content = Main.ContentManager;

            var waffe = content.Load<Texture2D>("images/gui/inventar/waffe");
            var waffeH = content.Load<Texture2D>("images/gui/inventar/waffe_h");
            var waffeA = content.Load<Texture2D>("images/gui/inventar/waffe_a");
            var waffeAH = content.Load<Texture2D>("images/gui/inventar/waffe_a_h");

            var hauptteil = content.Load<Texture2D>("images/gui/inventar/hauptteil");
            var hauptteilH = content.Load<Texture2D>("images/gui/inventar/hauptteil_h");
            var hauptteilA = content.Load<Texture2D>("images/gui/inventar/hauptteil_a");
            var hauptteilAH = content.Load<Texture2D>("images/gui/inventar/hauptteil_a_h");

            var stabilisator = content.Load<Texture2D>("images/gui/inventar/stabilisator");
            var stabilisatorH = content.Load<Texture2D>("images/gui/inventar/stabilisator_h");
            var stabilisatorA = content.Load<Texture2D>("images/gui/inventar/stabilisator_a");
            var stabilisatorAH = content.Load<Texture2D>("images/gui/inventar/stabilisator_a_h");

            var munition = content.Load<Texture2D>("images/gui/inventar/munition");
            var munitionH = content.Load<Texture2D>("images/gui/inventar/munition_h");
            var munitionA = content.Load<Texture2D>("images/gui/inventar/munition_a");
            var munitionAH = content.Load<Texture2D>("images/gui/inventar/munition_a_h");

            var visier = content.Load<Texture2D>("images/gui/inventar/visier");
            var visierH = content.Load<Texture2D>("images/gui/inventar/visier_h");
            var visierA = content.Load<Texture2D>("images/gui/inventar/visier_a");
            var visierAH = content.Load<Texture2D>("images/gui/inventar/visier_a_h");

            var antrieb = content.Load<Texture2D>("images/gui/inventar/antrieb");
            var antriebH = content.Load<Texture2D>("images/gui/inventar/antrieb_h");
            var antriebA = content.Load<Texture2D>("images/gui/inventar/antrieb_a");
            var antriebAH = content.Load<Texture2D>("images/gui/inventar/antrieb_a_h");

            var sonstiges = content.Load<Texture2D>("images/gui/inventar/sonstiges");
            var sonstigesH = content.Load<Texture2D>("images/gui/inventar/sonstiges_h");
            var sonstigesA = content.Load<Texture2D>("images/gui/inventar/sonstiges_a");
            var sonstigesAH = content.Load<Texture2D>("images/gui/inventar/sonstiges_a_h");

            toggleWaffe = new UIToggleButton(120, 30, new Vector2(280, 0), waffe, waffeH, waffeA, waffeAH, "") {isActive = true};
            toggleHauptteil = new UIToggleButton(120, 30, new Vector2(280, 40), hauptteil, hauptteilH, hauptteilA, hauptteilAH, "") { isActive = true };
            toggleStabilisator = new UIToggleButton(120, 30, new Vector2(280, 80), stabilisator, stabilisatorH, stabilisatorA, stabilisatorAH, "") { isActive = true };
            toggleMunition = new UIToggleButton(120, 30, new Vector2(280, 120), munition, munitionH, munitionA, munitionAH, "") { isActive = true };
            toggleVisier = new UIToggleButton(120, 30, new Vector2(280, 160), visier, visierH, visierA, visierAH, "") { isActive = true };
            toggleAntrieb = new UIToggleButton(120, 30, new Vector2(280, 200), antrieb, antriebH, antriebA, antriebAH, "") { isActive = true };
            togglePowerup = new UIToggleButton(120, 30, new Vector2(280, 240), sonstiges, sonstigesH, sonstigesA, sonstigesAH, "") { isActive = true };

            toggleWaffe.AddActionListener(this);
            toggleHauptteil.AddActionListener(this);
            toggleStabilisator.AddActionListener(this);
            toggleMunition.AddActionListener(this);
            toggleVisier.AddActionListener(this);
            toggleAntrieb.AddActionListener(this);
            togglePowerup.AddActionListener(this);

            Add(toggleWaffe);
            Add(toggleHauptteil);
            Add(toggleStabilisator);
            Add(toggleMunition);
            Add(toggleVisier);
            Add(toggleAntrieb);
            Add(togglePowerup);

        }

        

        public void OnMouseDown(UIElement element)
        {
            //////////////////////////////////////////////////////
            // ToggleButtons
            if (element.GetType() == typeof (UIToggleButton))
            {
                inventarList.FirsVisibleButtonIndex = 0;
            }


            if (element == toggleWaffe)
            {
                inventarList.AddItemList(listWaffe);
            }
            else if (element == toggleHauptteil)
            {
                inventarList.AddItemList(listHauptteil);
            }
            else if (element == toggleMunition)
            {
                inventarList.AddItemList(listMunition);
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
            else if (element == togglePowerup)
            {
                inventarList.AddItemList(listPowerup);
            }

            
        }

        public void OnMouseUp(UIElement element)
        {
            inventarList.FirsVisibleButtonIndex = 0;
            if (element == toggleWaffe)
            {
                inventarList.RemoveItems(listWaffe);
            }
            else if (element == toggleHauptteil)
            {
                inventarList.RemoveItems(listHauptteil);
            }
            else if (element == toggleMunition)
            {
                inventarList.RemoveItems(listMunition);
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
            else if (element == togglePowerup)
            {
                inventarList.RemoveItems(listPowerup);

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
