using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.Managers
{
    class GuiManager : Manager, UIActionListener
    {
        private UIPanel mainPanel;
        private UIPanel currentPanel;

        private Texture2D background;
        private UIButton btnKonstruktor ;
        private UIButton btnInventar;
        private UIButton btnMission;
        private UIButton btnMap;



        public GuiManager()
        {
            
        }

        private void LoadComponents()
        {
            Debug.WriteLine("Lade GUI Komponente...");
            ContentManager content = Main.ContentManager;

            background = content.Load<Texture2D>("images/menu/gui/background");

            Texture2D imgKonstruktor = content.Load<Texture2D>("images/gui/button_konstruktor");
            Texture2D imgKonstruktorHover = content.Load<Texture2D>("images/gui/button_konstruktor");

            Texture2D imgMap  = content.Load<Texture2D>("images/gui/button_konstruktor");
            Texture2D imgMapHover = content.Load<Texture2D>("images/gui/button_konstruktor");

            Texture2D imgMission = content.Load<Texture2D>("images/gui/button_konstruktor");
            Texture2D imgMissionHover = content.Load<Texture2D>("images/gui/button_konstruktor");

            Texture2D imgInventar = content.Load<Texture2D>("images/gui/button_konstruktor")
            Texture2D imgInventarHover = content.Load<Texture2D>("images/gui/button_konstruktor");


            btnKonstruktor = new UIButton(new Vector2(0, 0), imgKonstruktor, imgKonstruktorHover);
            btnMap = new UIButton(new Vector2(160, 0), imgMap, imgMapHover);
            btnMission = new UIButton(new Vector2(320, 0), imgMission, imgMissionHover);
            btnInventar = new UIButton(new Vector2(480, 0), imgInventar, imgInventarHover);

            btnKonstruktor.AddActionListener(this);
            btnMap.AddActionListener(this);
            btnMission.AddActionListener(this);
            btnInventar.AddActionListener(this);

            mainPanel.Add(btnKonstruktor);
            mainPanel.Add(btnMap);
            mainPanel.Add(btnMission);
            mainPanel.Add(btnInventar);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch batch)
        {
            throw new NotImplementedException();
        }

        public void ActionEvent(UIElement element)
        {
            throw new NotImplementedException();
        }
    }

    
}
