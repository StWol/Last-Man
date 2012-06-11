using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIList : UIPanel, UIActionListener
    {
        private List<UIElement> buttonList;
        private List<Item> itemList;
        private Dictionary<int, int> countItemsDict;
        private int firsVisibleButtonIndex = 0;
        private const int MAX_VISIBLE_BUTTON_COUNT = 5;

        private UIButton btnPrevious;
        private UIButton btnNext;

        public UIList(int width, int height, Vector2 position, List<Item> itemList)
            : base(width, height, position)
        {
            this.itemList = itemList;
            buttonList = new List<UIElement>();

            CountItems();

            Texture2D imgPreviousButton = Main.ContentManager.Load<Texture2D>("images/gui/prev");
            Texture2D imgPreviousButtonHover = Main.ContentManager.Load<Texture2D>("images/gui/prevH");

            Texture2D imgNextButton = Main.ContentManager.Load<Texture2D>("images/gui/next");
            Texture2D imgNextButtonHover = Main.ContentManager.Load<Texture2D>("images/gui/nextH");

            btnPrevious = new UIButton(100, 10, new Vector2(0, 0), imgPreviousButton, imgPreviousButtonHover);
            btnNext = new UIButton(100, 10, new Vector2(0, 0), imgNextButton, imgNextButtonHover);

            btnPrevious.AddActionListener(this);
            btnNext.AddActionListener(this);

            Add(btnPrevious);
            GenerateButtons();
            Add(btnNext);
        }

        private void CountItems()
        {
            foreach (var item in itemList)
            {
                countItemsDict[item.Id] += 1;
            }
        }

        private void GenerateButtons()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Item item = itemList[i];
                UIListButton button = new UIListButton(100, 20, new Vector2(0, 200 * i), item.Icon, item.Name, countItemsDict[item.Id], item.Weight);
                buttonList.Add(button);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Clear();
            for (int i = firsVisibleButtonIndex; i < MAX_VISIBLE_BUTTON_COUNT + firsVisibleButtonIndex; i++)
            {
                Add(buttonList[i]);
            }
            base.Draw(sb);
        }


        /// <summary>
        /// Ein Button fuer die Liste
        /// </summary>
        private class UIListButton : UIElement
        {
            private readonly UIButton itemIcon;
            private readonly string name;
            private readonly int count;
            private float weigth;
            private SpriteFont fontDefault;
            private UIButton weightButton;

            public UIListButton(int width, int height, Vector2 position, Texture2D itemIcon, string name, int count, float weigth)
                : base(width, height, position)
            {
                this.itemIcon = new UIButton(new Vector2(0, 0), itemIcon, itemIcon);
                this.name = name;
                this.count = count;
                var weightIcon = Main.ContentManager.Load<Texture2D>("images/gui/button_konstruktor");
                weightButton = new UIButton(new Vector2(0, 0), weightIcon, weightIcon);
                this.weigth = weigth;

                fontDefault = Main.ContentManager.Load<SpriteFont>("fonts/defaultMedium");
            }

            public override void Draw(SpriteBatch sb)
            {
                itemIcon.Draw(sb);
                sb.DrawString(fontDefault, name, new Vector2(0,0), Color.Black);
                sb.DrawString(fontDefault, count.ToString(), new Vector2(100, 0), Color.Black);
                weightButton.Draw(sb);
                sb.DrawString(fontDefault, weigth.ToString(), new Vector2(200, 0), Color.Black);
            }
        }

        public void ActionEvent(UIElement element)
        {
            if (element == btnPrevious && firsVisibleButtonIndex > 0)
            {
                firsVisibleButtonIndex--;
            }
            else if (element == btnNext)
            {
                firsVisibleButtonIndex++;
            }
        }
    }
}
