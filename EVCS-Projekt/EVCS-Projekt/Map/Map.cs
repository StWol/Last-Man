using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace EVCS_Projekt.Map
{
    class Map
    {

        public Texture2D[] sprites { get; set; }
        

        public static void LoadMap(string mapFile)
        {
            string fileContents = string.Empty;
		 TextReader reader = new StreamReader(mapFile);
		  
		  while (reader.Peek() != -1)
		  {
				fileContents += reader.ReadLine().ToString();
		  }
          reader.Close();
        }
    }
}
