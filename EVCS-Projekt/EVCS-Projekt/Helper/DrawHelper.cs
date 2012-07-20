using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LastMan.Helper
{
    class DrawHelper
    {
        // Die Referenzauflösung die zu berechnung verwendet wird
        private static Vector2 referenceSize = new Vector2(1024, 576);

        // Zwischenspeicher
        private static Dictionary<string, Vector2> dimensions = new Dictionary<string,Vector2>();
        private static Dictionary<string, Vector2> unmodifiedDimensions = new Dictionary<string,Vector2>();

        private static float scale = 1F;
        public static float Scale { get { return scale; } }

        public static void Callculate()
        {
            // Rechnet die Position in die Aktuelle Bildschirmgröße um
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            // Umrechnen und neu speichern
            foreach (var dim in unmodifiedDimensions )
            {
                Vector2 calc = new Vector2();

                calc.X = (int)(dim.Value.X / referenceSize.X * resWidht);
                calc.Y = (int)(dim.Value.Y / referenceSize.Y * resHeight);

                dimensions[dim.Key] = calc;
            }

            // Scale neu berechnen
            scale = 1F / referenceSize.Y * resHeight;
        }

        public static void AddDimension(string key, int x, int y)
        {
            // Speicher die werte zusätzlich unverändert
                
            

            // Rechnet die Position in die Aktuelle Bildschirmgröße um
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            Vector2 calc = new Vector2((int)(x / referenceSize.X * resWidht), (int)(y / referenceSize.Y * resHeight));
            if (!unmodifiedDimensions.ContainsKey(key))
            {
                unmodifiedDimensions.Add(key, new Vector2(x, y));
                dimensions.Add(key, calc);
            }
            else
            {
                unmodifiedDimensions[key] =  new Vector2(x, y);
                dimensions[key] =  calc;
            }

            
        }

        public static void Update(string key, Vector2 newVector)
        {
            dimensions[key] = newVector;
        }

        public static Vector2 Get(string key)
        {
            // Getter
            return dimensions[key];
        }
    }
}
