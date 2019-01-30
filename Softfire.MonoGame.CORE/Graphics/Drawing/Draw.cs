using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Graphics.Drawing
{
    /// <summary>
    /// A static class for drawing common objects.
    /// </summary>
    public static class Draw
    {
        #region Objects

        /// <summary>
        /// Draws a Rectangle.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="texture">The texture to draw. Intaken as a <see cref="Texture2D"/>.</param>
        /// <param name="position">The position to start drawing the rectangle. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The rectangle's width. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">The rectangle's height. Intaken as an <see cref="int"/>.</param>
        /// <param name="rotationAngle">The rectangle's rotation angle, in radians. Intaken as a <see cref="float"/>.</param>
        /// <param name="drawBackground">Determines whether to draw a background. Intaken as a <see cref="bool"/>.</param>
        /// <param name="backgroundColor">The background color. Intaken as a <see cref="Color"/>.</param>
        /// <param name="drawBorders">Determines whether to draw borders. Intaken as a <see cref="bool"/>.</param>
        /// <param name="borderColor">The border's color. Intaken as a <see cref="Color"/>.</param>
        /// <param name="borderThickness">The border's thickness. Intaken as a <see cref="float"/>.</param>
        /// <param name="drawDepth">The rectangle's draw depth. intaken as a <see cref="float"/>.</param>
        public static void Rectangle(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, int width, int height,
                                     float rotationAngle = 0f, bool drawBackground = true, Color? backgroundColor = null, bool drawBorders = true,
                                     Color? borderColor = null, float borderThickness = 1f, float drawDepth = 0f)
        {
            var rectangle = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);

            if (drawBackground)
            {
                // Frame
                spriteBatch.Draw(texture, new Vector2(rectangle.X, rectangle.Y), null, backgroundColor ?? Color.LightGray, rotationAngle,
                                 Vector2.Zero, new Vector2(rectangle.Width, rectangle.Height), SpriteEffects.None, drawDepth);
            }

            if (drawBorders)
            {
                var bColor = borderColor ?? Color.Black;

                // Top
                Line(spriteBatch, texture, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Right, rectangle.Y), bColor, borderThickness);

                // Left
                Line(spriteBatch, texture, new Vector2(rectangle.X + 1f, rectangle.Y), new Vector2(rectangle.X + 1f, rectangle.Bottom + borderThickness), bColor, borderThickness);

                // Bottom
                Line(spriteBatch, texture, new Vector2(rectangle.X, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), bColor, borderThickness);

                // Right
                Line(spriteBatch, texture, new Vector2(rectangle.Right + 1f, rectangle.Y), new Vector2(rectangle.Right + 1f, rectangle.Bottom + borderThickness), bColor, borderThickness);
            }
        }

        /// <summary>
        /// Draws a line between vectors.
        /// </summary>
        /// <param name="spriteBatch">The current <see cref="SpriteBatch"/>.</param>
        /// <param name="texture">The texture to draw. Intaken as a <see cref="Texture2D"/>.</param>
        /// <param name="startVector">The line's starting vector. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="endVector">The line's ending vector. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="color">The line's color. Intaken as a <see cref="Color"/>.</param>
        /// <param name="thickness">The line's thickness. Intaken as a <see cref="float"/>.</param>
        public static void Line(this SpriteBatch spriteBatch, Texture2D texture, Vector2 startVector, Vector2 endVector, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(startVector, endVector);
            var angle = (float)Math.Atan2(endVector.Y - startVector.Y, endVector.X - startVector.X);

            spriteBatch.Draw(texture, startVector, null, color, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0);
        }

        #endregion
    }
}