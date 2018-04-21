using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CD
{
    public class RectangleCollisions
    {
        /// <summary>
        /// Per Pixel Collision Detection
        /// </summary>
        /// <param name="rectangleA">Intakes an Animation's Rectangle to be used in comparison to determine if a collision occured.</param>
        /// <param name="dataA">Intakes the Animation's Texture Color Data from Texture A.</param>
        /// <param name="rectangleB">Intakes an Animation's Rectangle to be used in comparison to determine if a collision occured.</param>
        /// <param name="dataB">Intakes the Animation's Texture Color Data from Texture B.</param>
        /// <returns></returns>
        public static bool PerPixel(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            var intersectFound = false;

            // Find the bounds of the rectangle intersection
            var top = Math.Max(rectangleA.Top, rectangleB.Top);
            var bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            var left = Math.Max(rectangleA.Left, rectangleB.Left);
            var right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (var y = top; y < bottom; y++)
            {
                for (var x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    var colorA = dataA[(x - rectangleA.Left) +
                                       (y - rectangleA.Top) * rectangleA.Width];
                    var colorB = dataB[(x - rectangleB.Left) +
                                       (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        intersectFound = true;
                    }
                }
            }

            return intersectFound;
        }

        /// <summary>
        /// Bounds Intersection Method
        /// Used to determine if a collision occured between two Animation's Rectangles.
        /// </summary>
        /// <param name="rectangleOne">Intakes an Animation's Rectangle.</param>
        /// <param name="rectangleTwo">Intakes an Animation's Rectangle.</param>
        /// <returns>Returns a bool on whether an Intersection has occured.</returns>
        public static bool BoundsIntersects(Rectangle rectangleOne, Rectangle rectangleTwo)
        {
            return rectangleOne.Intersects(rectangleTwo);
        }

        /// <summary>
        /// Bounds Contains Method
        /// Used to determine if an Animation's Rectangle is completely inside another Animation's Rectangle.
        /// </summary>
        /// <param name="rectangleOne">Intakes an Animation's Rectangle.</param>
        /// <param name="rectangleTwo">Intakes an Animation's Rectangle.</param>
        /// <returns>Returns a bool on whether RectangleTwo is inside RectangleOne.</returns>
        public static bool BoundsContains(Rectangle rectangleOne, Rectangle rectangleTwo)
        {
            return rectangleOne.Contains(rectangleTwo);
        }
    }
}
