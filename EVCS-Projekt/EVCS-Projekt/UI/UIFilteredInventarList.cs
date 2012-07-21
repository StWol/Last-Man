using System.Collections.Generic;
using LastMan.Objects;
using LastMan.Objects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.UI
{
    internal class UIFilteredInventarList : UIPanel, UIActionListener
    {
        private UIToggleButton toggleAlles;
        private UIToggleButton toggleWaffe;
        private UIToggleButton toggleHauptteil;
        private UIToggleButton toggleStabilisator;
        private UIToggleButton toggleMunition;
        private UIToggleButton toggleVisier;
        private UIToggleButton toggleAntrieb;
        private UIToggleButton togglePowerup;

        private Dictionary<int, int> listAlles;
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
        

        public UIFilteredInventarList(int width, int height, Vector2 position, UIActionListener listener)
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
            GenerateFilteredLists(player.Inventar);
        }

        private void GenerateFilteredLists(Dictionary<int, int> inventar)
        {
            listAlles = new Dictionary<int, int>();
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

                listAlles[typeId]= count;
                var type = item.GetType();
                if (type == typeof(Weapon))
                {
                    listWaffe[typeId] = count;
                }
                else if (type == typeof(Hauptteil))
                {
                    listHauptteil[typeId] = count;
                }
                else if (type == typeof(Stabilisator))
                {
                    listStabilisator[typeId] = count;
                }
                else if (type == typeof(Munition))
                {
                    listMunition[typeId] = count;
                }
                else if (type == typeof(Visier))
                {
                    listVisier[typeId] = count;
                }
                else if (type == typeof(Antrieb))
                {
                    listAntrieb[typeId] = count;
                }
                else if (type == typeof(Powerup))
                {
                    listPowerup[typeId] = count;
                }
            }
        }

        public void ResetToggleButtons()
        {
            toggleWaffe.IsActive = false;
            toggleHauptteil.IsActive = false;
            toggleStabilisator.IsActive = false;
            toggleMunition.IsActive = false;
            toggleVisier.IsActive = false;
            toggleAntrieb.IsActive = false;
            togglePowerup.IsActive = false;
        }

        private void CreateCheckBoxPanel()
        {
            ContentManager content = Main.ContentManager;

            var waffe = content.Load<Texture2D>("images/gui/inventar/filter_test");
            var waffeH = content.Load<Texture2D>("images/gui/inventar/filter_test_h");
            var waffeA = content.Load<Texture2D>("images/gui/inventar/filter_test_a");
            var waffeAH = content.Load<Texture2D>("images/gui/inventar/filter_test_a_h");

            //var hauptteil = content.Load<Texture2D>("images/gui/inventar/hauptteil");
            //var hauptteilH = content.Load<Texture2D>("images/gui/inventar/hauptteil_h");
            //var hauptteilA = content.Load<Texture2D>("images/gui/inventar/hauptteil_a");
            //var hauptteilAH = content.Load<Texture2D>("images/gui/inventar/hauptteil_a_h");

            //var stabilisator = content.Load<Texture2D>("images/gui/inventar/stabilisator");
            //var stabilisatorH = content.Load<Texture2D>("images/gui/inventar/stabilisator_h");
            //var stabilisatorA = content.Load<Texture2D>("images/gui/inventar/stabilisator_a");
            //var stabilisatorAH = content.Load<Texture2D>("images/gui/inventar/stabilisator_a_h");

            //var munition = content.Load<Texture2D>("images/gui/inventar/munition");
            //var munitionH = content.Load<Texture2D>("images/gui/inventar/munition_h");
            //var munitionA = content.Load<Texture2D>("images/gui/inventar/munition_a");
            //var munitionAH = content.Load<Texture2D>("images/gui/inventar/munition_a_h");

            //var visier = content.Load<Texture2D>("images/gui/inventar/visier");
            //var visierH = content.Load<Texture2D>("images/gui/inventar/visier_h");
            //var visierA = content.Load<Texture2D>("images/gui/inventar/visier_a");
            //var visierAH = content.Load<Texture2D>("images/gui/inventar/visier_a_h");

            //var antrieb = content.Load<Texture2D>("images/gui/inventar/antrieb");
            //var antriebH = content.Load<Texture2D>("images/gui/inventar/antrieb_h");
            //var antriebA = content.Load<Texture2D>("images/gui/inventar/antrieb_a");
            //var antriebAH = content.Load<Texture2D>("images/gui/inventar/antrieb_a_h");

            //var sonstiges = content.Load<Texture2D>("images/gui/inventar/sonstiges");
            //var sonstigesH = content.Load<Texture2D>("images/gui/inventar/sonstiges_h");
            //var sonstigesA = content.Load<Texture2D>("images/gui/inventar/sonstiges_a");
            //var sonstigesAH = content.Load<Texture2D>("images/gui/inventar/sonstiges_a_h");

            toggleAlles = new UIToggleButton(30, 30, new Vector2(280, 0), waffe, waffeH, waffeA, waffeAH, "");
            toggleWaffe = new UIToggleButton ( 30, 30, new Vector2 ( 280, 40 ), waffe, waffeH, waffeA, waffeAH, "" );
            toggleHauptteil = new UIToggleButton(30, 30, new Vector2(280, 80), waffe, waffeH, waffeA, waffeAH, "");
            toggleStabilisator = new UIToggleButton(30, 30, new Vector2(280, 120), waffe, waffeH, waffeA, waffeAH, "");
            toggleMunition = new UIToggleButton(30, 30, new Vector2(280, 160), waffe, waffeH, waffeA, waffeAH, "");
            toggleVisier = new UIToggleButton(30, 30, new Vector2(280, 200), waffe, waffeH, waffeA, waffeAH, "");
            toggleAntrieb = new UIToggleButton(30, 30, new Vector2(280, 240), waffe, waffeH, waffeA, waffeAH, "");
            togglePowerup = new UIToggleButton(30, 30, new Vector2(280, 280), waffe, waffeH, waffeA, waffeAH, "");


            //toggleHauptteil = new UIToggleButton(120, 30, new Vector2(280, 40), hauptteil, hauptteilH, hauptteilA, hauptteilAH, "");
            //toggleStabilisator = new UIToggleButton(120, 30, new Vector2(280, 80), stabilisator, stabilisatorH, stabilisatorA, stabilisatorAH, "");
            //toggleMunition = new UIToggleButton ( 120, 30, new Vector2 ( 280, 120 ), munition, munitionH, munitionA,munitionAH, "" );
            //toggleVisier = new UIToggleButton(120, 30, new Vector2(280, 160), visier, visierH, visierA, visierAH, "");
            //toggleAntrieb = new UIToggleButton(120, 30, new Vector2(280, 200), antrieb, antriebH, antriebA, antriebAH, "");
            //togglePowerup = new UIToggleButton(120, 30, new Vector2(280, 240), sonstiges, sonstigesH, sonstigesA, sonstigesAH, "");

            toggleAlles.AddActionListener(this);
            toggleWaffe.AddActionListener(this);
            toggleHauptteil.AddActionListener(this);
            toggleStabilisator.AddActionListener(this);
            toggleMunition.AddActionListener(this);
            toggleVisier.AddActionListener(this);
            toggleAntrieb.AddActionListener(this);
            togglePowerup.AddActionListener(this);

            Add(toggleAlles);
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
            if (element.GetType() == typeof(UIToggleButton))
            {
                inventarList.FirsVisibleButtonIndex = 0;
            }

            ResetToggleButtons();
            if (element == toggleAlles)
            {
                inventarList.SetItems(listAlles);
            }
            if (element == toggleWaffe)
            {
                inventarList.SetItems(listWaffe);
            }
            else if (element == toggleHauptteil)
            {
                inventarList.SetItems(listHauptteil);
            }
            else if (element == toggleMunition)
            {
                inventarList.SetItems(listMunition);
            }
            else if (element == toggleStabilisator)
            {
                inventarList.SetItems(listStabilisator);
            }
            else if (element == toggleVisier)
            {
                inventarList.SetItems(listVisier);
            }
            else if (element == toggleAntrieb)
            {
                inventarList.SetItems(listAntrieb);
            }
            else if (element == togglePowerup)
            {
                inventarList.SetItems(listPowerup);
            }

            
        }

        public void OnMouseUp(UIElement element)
        {
            //inventarList.FirsVisibleButtonIndex = 0;
            //if (element == toggleWaffe)
            //{
            //    inventarList.RemoveItems(listWaffe);
            //}
            //else if (element == toggleHauptteil)
            //{
            //    inventarList.RemoveItems(listHauptteil);
            //}
            //else if (element == toggleMunition)
            //{
            //    inventarList.RemoveItems(listMunition);
            //}
            //else if (element == toggleStabilisator)
            //{
            //    inventarList.RemoveItems(listStabilisator);
            //}
            //else if (element == toggleVisier)
            //{
            //    inventarList.RemoveItems(listVisier);
            //}
            //else if (element == toggleAntrieb)
            //{
            //    inventarList.RemoveItems(listAntrieb);
            //}
            //else if (element == togglePowerup)
            //{
            //    inventarList.RemoveItems(listPowerup);

            //}
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
