using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public static class UIExtensions
    {
        private static Texture2D Texture { get; set; }

        /// <summary>
        /// Apply Basic Text Scaling.
        /// </summary>
        /// <param name="spriteBatch">Intakes the current SpriteBath.</param>
        /// <param name="text">Intakes Text.</param>
        /// <param name="font">Intakes a SpriteFont.</param>
        /// <param name="container">Intakes a Rectangle.</param>
        /// <param name="useContainerPosition">Intakes a boolean defining whether to use the container's position to offset the centering of the text. Default is false.</param>
        public static void ApplyBasicTextScaling(this SpriteBatch spriteBatch, string text, SpriteFont font, Rectangle container, Color? color = null, bool useContainerPosition = false)
        {
            // Get the size (length) of the string.
            var size = font.MeasureString(text);

            // Get the scale required to fill the rectangle.
            var scale = new Vector2(container.Width / size.X, container.Height / size.Y);

            // Calculate the text's width and height after scaling has been applied.
            var strWidth = (int)Math.Round(size.X * scale.X);
            var strHeight = (int)Math.Round(size.Y * scale.Y);

            Vector2 position;

            // Calculate the center most position with scaling applied.
            if (useContainerPosition)
            {
                position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f + container.X,
                    Y = (container.Height - strHeight) / 2f + container.Y
                };
            }
            else
            {
                position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f,
                    Y = (container.Height - strHeight) / 2f
                };
            }

            // Draw
            spriteBatch.DrawString(font, text, position, color ?? Color.Aqua, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Apply Enhanced Text Scaling.
        /// Use to scale the UIText to fit within the boundaries of the supplied container Rectangle.
        /// </summary>
        /// <param name="text">Intakes a UIText.</param>
        /// <param name="container">Intakes a Rectangle.</param>
        /// <param name="useContainerPosition">Intakes a boolean defining whether to use the container's position to offset the centering of the text. Default is false.</param>
        public static void ApplyEnhancedTextScaling(this UIText text, Rectangle container, bool useContainerPosition = false)
        {
            // Get the size (length) of the string.
            var size = text.GetLength();

            // Get the scale required to fill the rectangle.
            text.Scale = new Vector2(container.Width / size.X, container.Height / size.Y);

            // Calculate the text's width and height after scaling has been applied.
            var strWidth = (int)Math.Round(size.X * text.Scale.X);
            var strHeight = (int)Math.Round(size.Y * text.Scale.Y);

            // Calculate the center most position with scaling applied.
            if (useContainerPosition)
            {
                text.Position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f + container.X,
                    Y = (container.Height - strHeight) / 2f + container.Y
                };
            }
            else
            {
                text.Position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f,
                    Y = (container.Height - strHeight) / 2f
                };
            }
        }

        /// <summary>
        /// Appy Basic Text Wrap.
        /// Used to keep text within the bounds of the supplied Rectangle.
        /// </summary>
        /// <param name="text">Intakes Text.</param>
        /// <param name="font">Intakes a SpriteFont.</param>
        /// <param name="position">Intakes the text's current position as a Vector2.</param>
        /// <param name="container">Intakes the text's container's Rectangle.</param>
        /// <returns>Returns a modified string of the text that was passed in.</returns>
        public static string ApplyBasicTextWrap(this string text, SpriteFont font, Vector2 position, Rectangle container)
        {
            var line = string.Empty;
            var alteredString = string.Empty;
            var wordArray = text.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < wordArray.Length; ++i)
            {
                if (font.MeasureString(line + wordArray[i]).X + position.X > container.Width)
                {
                    if (font.MeasureString(wordArray[i]).X + position.X < container.Width)
                    {
                        alteredString += line + Environment.NewLine;
                        line = string.Empty;
                    }

                    foreach (var character in wordArray[i].ToCharArray())
                    {
                        if (font.MeasureString(line + character + "-").X + position.X > container.Width)
                        {
                            alteredString += line + "-" + Environment.NewLine;
                            line = string.Empty;
                        }

                        line += character;
                    }

                    line += " ";
                }
                else
                {
                    line += i == wordArray.Length - 1 ? wordArray[i] : wordArray[i] + " ";
                }
            }

            return alteredString + line;
        }

        /// <summary>
        /// Apply Enhanced Text Wrap.
        /// Used to keep text within the bounds of the supplied Rectangle.
        /// </summary>
        /// <param name="text">Intakes a UIText.</param>
        /// <param name="container">Intakes the text's container's Rectangle.</param>
        public static void ApplyEnhancedTextWrap(this UIText text, Rectangle container)
        {
            var line = string.Empty;
            var wordArray = text.String.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Reset AlteredString.
            text.AlteredString = null;

            for (var i = 0; i < wordArray.Length; ++i)
            {
                // Begin breaking down the line if the current line plus the next word goes beyond the container width.
                if (text.Font.MeasureString(line + wordArray[i]).X + text.Rectangle.X > container.Width)
                {
                    // Break the line if the current word goes is below the container width.
                    if (text.Font.MeasureString(wordArray[i]).X + text.Rectangle.X < container.Width)
                    {
                        text.AlteredString += line + Environment.NewLine;
                        line = string.Empty;
                    }

                    foreach (var character in wordArray[i].ToCharArray())
                    {
                        // Break the word that goes beyond the container width.
                        if (text.Font.MeasureString(line + character + "-").X + text.Rectangle.X > container.Width)
                        {
                            text.AlteredString += line + "-" + Environment.NewLine;
                            line = string.Empty;
                        }

                        line += character;
                    }

                    line += " ";
                }
                else
                {
                    line += i == wordArray.Length - 1 ? wordArray[i] : wordArray[i] + " ";
                }
            }

            text.AlteredString += line;
        }

        /// <summary>
        /// Apply Basic Text Shadow.
        /// </summary>
        /// <param name="spriteBatch">Intakes the current SpriteBath.</param>
        /// <param name="text">Intakes Text.</param>
        /// <param name="font">Intakes a SpriteFont.</param>
        /// <param name="position">Intakes a position as a Vector2.</param>
        /// <param name="color">Intakes a Color</param>
        /// <param name="shadowLength">Intakes the shadow length as an int. Minimum of 1 and maximum of 5.</param>
        public static void ApplyBasicTextShadow(this SpriteBatch spriteBatch, string text, SpriteFont font, Vector2 position = new Vector2(), Color? color = null, float shadowLength = 1)
        {
            // Restrict shadow length to no greater than 5 and a minimum of 1.
            if (shadowLength > 5)
            {
                shadowLength = 5;
            }
            else if (shadowLength <= 0)
            {
                shadowLength = 1;
            }

            for (var i = shadowLength; i > 0; i--)
            {
                spriteBatch.DrawString(font, text, position +
                                                   new Vector2(i * 1.3f, -i * 0.7f),
                                                   color ?? Color.Aqua);
            }

            spriteBatch.DrawString(font, text, position, color ?? Color.Aqua);
        }

        public static void ApplyBasicTextOutline(this SpriteBatch spriteBatch, string text, SpriteFont font, Vector2 position = new Vector2(), Color? color = null)
        {
            spriteBatch.DrawString(font, text, position +
                                               new Vector2(1, 1),
                                               Color.Black);

            spriteBatch.DrawString(font, text, position,
                                               color ?? Color.Aqua);
        }

        private static void CreateTexture(SpriteBatch spriteBatch)
        {
            Texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Draw Rectangle.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        /// <param name="position">Intakes a Vector2. Indicating where to draw the Rectangle.</param>
        /// <param name="dimensions">Intakes a Vector2 indicating the Rectangle's dimensions.</param>
        /// <param name="rotationAngle">intakes a rotation angle in radians. Rectangle will be rotated by this angle. Default is 0f.</param>
        /// <param name="useCenterOrigin">Intakes a bool. Indicates if the origin of the Rectangle is the center or the top left corner. Default is false.</param>
        /// <param name="origin">Intakes a point of origin to draw the rectangle as a Vector2. Default is Vector2(0, 0).</param>
        /// <param name="useBackground">Intakes a bool indicating whether to use a background or not.</param>
        /// <param name="backgroundColor">Intakes a Color. Rectangle's background will be drawn this Color.</param>
        /// <param name="useBorders">Intakes a bool indicating whether to use borders or not.</param>
        /// <param name="borderColor">Intakes a Color. Rectangle's borders will be drawn this Color.</param>
        /// <param name="borderthickness">Intakes a float defining the border thickness.</param>
        /// <param name="drawDepth">Intakes a float. Indicates draw depth. Default is 0f.</param>
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 dimensions,
                                         float rotationAngle = 0f,
                                         bool useCenterOrigin = false,
                                         Vector2 origin = new Vector2(),
                                         bool useBackground = true,
                                         Color? backgroundColor = null,
                                         bool useBorders = true,
                                         Color? borderColor = null,
                                         float borderthickness = 1f,
                                         float drawDepth = 0f)
        {
            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

            if (Texture == null)
            {
                CreateTexture(spriteBatch);
            }

            if (useCenterOrigin)
            {
                origin = new Vector2(dimensions.X / 2, dimensions.Y / 2);
            }

            if (useBackground)
            {
                var bgcolor = backgroundColor ?? Color.LightGray;

                // Frame
                spriteBatch.Draw(Texture, rectangle, null, bgcolor, rotationAngle, origin, SpriteEffects.None, drawDepth);
            }

            if (useBorders)
            {
                var bColor = borderColor ?? Color.Black;

                // Top
                DrawLine(spriteBatch, new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Right, rectangle.Y), bColor, borderthickness);

                // Left
                DrawLine(spriteBatch, new Vector2(rectangle.X + 1f, rectangle.Y), new Vector2(rectangle.X + 1f, rectangle.Bottom + borderthickness), bColor, borderthickness);

                // Bottom
                DrawLine(spriteBatch, new Vector2(rectangle.X, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), bColor, borderthickness);

                // Right
                DrawLine(spriteBatch, new Vector2(rectangle.Right + 1f, rectangle.Y), new Vector2(rectangle.Right + 1f, rectangle.Bottom + borderthickness), bColor, borderthickness);
            }
        }

        /// <summary>
        /// Draws Line Betwen Points.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        /// <param name="startPoint">Intakes the line's start point as a Vector2.</param>
        /// <param name="endPoint">Intakes the line's end point as a Vector2.</param>
        /// <param name="color">Intakes a Color. Line will be drawn this Color.</param>
        /// <param name="thickness">Intakes line thickness as a float. Default is 1f.</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(startPoint, endPoint);
            var angle = (float)Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X);

            spriteBatch.Draw(Texture, startPoint, null, color, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw Pixel.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        /// <param name="position">Intakes position as a Vector2.</param>
        /// <param name="color">Intakes a Color. Line will be drawn this Color.</param>
        public static void DrawPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            if (Texture == null)
            {
                CreateTexture(spriteBatch);
            }

            spriteBatch.Draw(Texture, position, color);
        }
    }
}