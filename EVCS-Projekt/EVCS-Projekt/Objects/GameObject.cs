using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Tree;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace EVCS_Projekt.Objects
{

    public abstract class GameObject : IQuadStorable
    {
        // Baucht man die Vererbung der GameComponent von XNA und die initialize methode?

        public IRenderBehavior Renderer { get; set; }
        public ILocationBehavior LocationBehavior { get; private set; }

        // ***************************************************************************
        // Konstruktor 1
        public GameObject(ILocationBehavior locationBehavior, IRenderBehavior renderBehavior)
        {
            this.LocationBehavior = locationBehavior;
            this.Renderer = renderBehavior;
        }

        // ***************************************************************************
        // Konstruktor 2
        public GameObject(ILocationBehavior locationBehavior)
        {
            this.LocationBehavior = locationBehavior;
            this.Renderer = new NoRenderer();
        }

        // ***************************************************************************
        // Konstruktor 3
        public GameObject(GameObjectInner goi)
        {
            if (goi.isMapLocation)
            {
                LocationBehavior = new MapLocation(new Vector2(goi.xPos, goi.yPos), new Vector2(goi.width, goi.height));
                LocationBehavior.Rotation = goi.rotation;
            }
            else
            {
                LocationBehavior = new UILocation(new Vector2(goi.xPos, goi.yPos), new Vector2(goi.width, goi.height));
                LocationBehavior.Rotation = goi.rotation;
            }

            Renderer = LoadedRenderer.Get(goi.renderer);
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class GameObjectInner
        {
            public float xPos, yPos, width, height, rotation;
            public bool isMapLocation;
            public string renderer;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public GameObjectInner GetInner()
        {
            GameObjectInner goi = new GameObjectInner();
            goi.xPos = LocationBehavior.Position.X;
            goi.yPos = LocationBehavior.Position.Y;
            goi.width = LocationBehavior.Size.X;
            goi.height = LocationBehavior.Size.Y;
            goi.rotation = LocationBehavior.Rotation;

            if (LocationBehavior.GetType() == typeof(MapLocation))
                goi.isMapLocation = true;
            else
                goi.isMapLocation = false;

            if (goi.renderer == null)
                goi.renderer = new NoRenderer().Name;
            else
                goi.renderer = Renderer.Name;

            return goi;
        }

        // ***************************************************************************
        // Setzt die Größe des LocationBehavior auf die Größe der Textur bzw die Größe des Renderers
        public void LocationSizing()
        {
            // Nicht schön, aber NoRenderer und SimpleRenderer werden abgefangen, da diese keine Größe besitzten
            if (Renderer.GetType() == typeof(NoRenderer))
                return;

            LocationBehavior.Size = new Vector2(Renderer.Size.X, Renderer.Size.Y);
        }

        // ***************************************************************************
        // Berechnet Direction und Rotation, dass Location in richtung des Vectors schaut
        public void LookAt(Vector2 target)
        {
            Vector2 norm = LocationBehavior.Position - target;
            norm.Normalize();
            LocationBehavior.Direction = norm;

            CalculateRotation();
        }

        // ***************************************************************************
        // Berechnet Direction und Rotation, dass Location in richtung des Vectors schaut abhänig des MapOffsets
        public void RelativeLookAt(Vector2 target)
        {
            Vector2 norm = LocationBehavior.RelativePosition - target;
            norm.Normalize();
            LocationBehavior.Direction = norm;

            CalculateRotation();
        }

        // ***************************************************************************
        // Set die Direction und berechnet die Rotation, dass Location in richtung des Vectors schaut
        public void SetDirection(Vector2 direction)
        {
            Vector2 norm = direction;
            norm.Normalize();
            LocationBehavior.Direction = norm;

            CalculateRotation();
        }

        // ***************************************************************************
        // berechnet die Rotation, dass Location in richtung des Vectors schaut
        private void CalculateRotation()
        {
            LocationBehavior.Rotation = -(float)(Math.Atan2(-LocationBehavior.Direction.Y, LocationBehavior.Direction.X));
        }

        // ***************************************************************************
        // Prüft kollision mit anderem GameObject
        public bool CollisionWith(GameObject g)
        {
            return g.Rect.Intersects(Rect);
        }

        // ***************************************************************************
        // Pixelgenaue Collisionsprüfung
        public bool PPCollisionWith(GameObject g)
        {
            bool rCollision = CollisionWith(g);

            // Bei false ist keine Kollision
            if (!rCollision)
                return false;

            // Falls einer der Renderer NoRender ist, keine PixelCollision machen
            if (Renderer.GetType() == typeof(NoRenderer) || g.Renderer.GetType() == typeof(NoRenderer)
                || Renderer.GetType() == typeof(SimpleRenderer) || g.Renderer.GetType() == typeof(SimpleRenderer))
                return true;

            // Rectangles
            Rectangle rectA = Rect;
            Rectangle rectB = g.Rect;

            // Texture Data
            Texture2D textA, textB;

            if (Renderer.GetType() == typeof(AnimationRenderer))
                textA = ((AnimationRenderer)Renderer).Textures[((AnimationRenderer)Renderer).Frame];
            else
                textA = ((StaticRenderer)Renderer).Texture;

            if (g.Renderer.GetType() == typeof(AnimationRenderer))
                textB = ((AnimationRenderer)g.Renderer).Textures[((AnimationRenderer)g.Renderer).Frame];
            else
                textB = ((StaticRenderer)g.Renderer).Texture;

            Color[] dataA = new Color[textA.Width * textA.Height];
            Color[] dataB = new Color[textB.Width * textB.Height];

            textA.GetData(dataA);
            textB.GetData(dataB);

            Vector2 originA = new Vector2(textA.Width / 2, textA.Height / 2);
            Vector2 originB = new Vector2(textB.Width / 2, textB.Height / 2);


            // Update each block
            bool personHit = false;

            // Build the block's transform
            Matrix transformA =
                Matrix.CreateTranslation(new Vector3(-originA, 0.0f)) *
                // Matrix.CreateScale(block.Scale) *  would go here
                Matrix.CreateRotationZ(LocationBehavior.Rotation) *
                Matrix.CreateTranslation(new Vector3(LocationBehavior.Position, 0.0f));

            // Build the block's transform
            Matrix transformB =
                Matrix.CreateTranslation(new Vector3(-originB, 0.0f)) *
                // Matrix.CreateScale(block.Scale) *  would go here
                Matrix.CreateRotationZ(g.LocationBehavior.Rotation) *
                Matrix.CreateTranslation(new Vector3(g.LocationBehavior.Position, 0.0f));

            // Calculate the bounding rectangle of this block in world space
            Rectangle blockRectangleA = CalculateBoundingRectangle(
                     new Rectangle(0, 0, textA.Width, textA.Height),
                     transformA);

            // Calculate the bounding rectangle of this block in world space
            Rectangle blockRectangleB = CalculateBoundingRectangle(
                     new Rectangle(0, 0, textB.Width, textB.Height),
                     transformB);

            // The per-pixel check is expensive, so check the bounding rectangles
            // first to prevent testing pixels when collisions are impossible.
            if (rectA.Intersects(blockRectangleB))
            {
                // Check collision with person
                if (IntersectPixels(transformA, textA.Width,
                                    textA.Height, dataA,
                                    transformB, textB.Width,
                                    textB.Height, dataB))
                {
                    personHit = true;
                    return true;
                }
            }




            return false;
        }




        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                           Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }



        // ***************************************************************************
        // Prüft ob ein objekt näher drann ist wie wert
        public bool DistanceLessThan(GameObject g, float distance)
        {
            if (Vector2.Distance(new Vector2(Rect.X, Rect.Y), new Vector2(g.Rect.X, g.Rect.Y)) < distance)
                return true;
            else
                return false;
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Gibt Position als Rectangle zurück / BoundingBox
        public Rectangle Rect
        {
            get
            {
                // Korrektur, da die gerendeten Bilder ihre position zentriert haben und nicht in der linken oberen ecke
                Rectangle r = LocationBehavior.BoundingBox;
                r.X -= r.Width / 2;
                r.Y -= r.Height / 2;
                return r;
            }
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
