using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CD
{
    /// <summary>
    /// A handy class with methods for detecting collisions.
    /// </summary>
    public static class CollisionDetection
    {
        /// <summary>
        /// Compares pixels between color data and determines whether a collision has occured.
        /// </summary>
        /// <param name="rectangleA">The <see cref="Rectangle"/> to be used in comparison to determine if a collision occured.</param>
        /// <param name="dataA">The texture color data to use for comparison.</param>
        /// <param name="rectangleB">The <see cref="Rectangle"/> to be used in comparison to determine if a collision occured.</param>
        /// <param name="dataB">The texture color data to use for comparison.</param>
        /// <returns>Returns a boolean indicating whether a collision has occured.</returns>
        public static bool PerPixel(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
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

                    // If both pixels are not completely transparent.
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found.
                        return true;
                    }
                }
            }

            return false;
        }
    }
}