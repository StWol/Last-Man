using System;
using System.Diagnostics;
using LastMan.UI;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.Managers
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
            
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch batch)
        {
            throw new NotImplementedException();
        }

        public void OnMouseDown(UIElement element)
        {
            throw new NotImplementedException();
        }


        public void OnMouseUp(UIElement element)
        {
            // leer
        }
    }

    
}
