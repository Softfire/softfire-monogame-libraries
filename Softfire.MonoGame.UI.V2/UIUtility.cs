using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.V2
{
    /// <summary>
    /// A UI Utility class of tools.
    /// </summary>
    public static class UIUtility
    {
        /// <summary>
        /// A frame rate counter.
        /// </summary>
        public sealed class FrameRateCounter
        {
            /// <summary>
            /// The counter's display text..
            /// </summary>
            private string Text { get; }

            /// <summary>
            /// The counter's text position.
            /// </summary>
            private Vector2 TextPosition { get; }

            /// <summary>
            /// The counter's text shade position.
            /// </summary>
            private Vector2 TextShadePosition { get; }

            /// <summary>
            /// The counter's font.
            /// </summary>
            private SpriteFont Font { get; }

            /// <summary>
            /// The counter's text color.
            /// </summary>
            private Color TextColor { get; }

            /// <summary>
            /// The counter's shade color.
            /// </summary>
            private Color ShadeColor { get; }

            /// <summary>
            /// The counter's alert color.
            /// </summary>
            private Color AlertColor { get; }

            /// <summary>
            /// The counter's frame rate.
            /// </summary>
            private int FrameRate { get; set; }

            /// <summary>
            /// The counter's frame Counter.
            /// </summary>
            private int FrameCounter { get; set; }

            /// <summary>
            /// The counter's elapsed time.
            /// </summary>
            private TimeSpan ElapsedTime { get; set; }

            /// <summary>
            /// Keeps track of your current FPS.
            /// </summary>
            /// <param name="textPosition">Intakes a Vector2 to define where to draw the Counter.</param>
            /// <param name="textFont">Intakes a SpriteFont defining the Font to use.</param>
            /// <param name="customText">Intakes a string to be used in place of the default text.</param>
            /// <param name="textColor">Intakes a Color to use for the text.</param>
            public FrameRateCounter(Vector2 textPosition, SpriteFont textFont, string customText, Color? textColor = null)
            {
                FrameRate = 0;
                FrameCounter = 0;
                ElapsedTime = TimeSpan.Zero;

                TextPosition = textPosition;
                Font = textFont;
                Text = string.IsNullOrWhiteSpace(customText) ? "FPS: " : customText;

                TextPosition = textPosition;
                TextShadePosition = new Vector2(TextPosition.X + 1, TextPosition.Y + 1);

                TextColor = textColor ?? Color.Aqua;
                ShadeColor = TextColor == Color.Black ? Color.White : Color.Black;
                AlertColor = TextColor == Color.Red ? Color.Yellow : Color.Red;
            }

            /// <summary>
            /// Frame Rate Counter Update Method.
            /// </summary>
            /// <param name="gameTime">Intakes MonoGame GameTime.</param>
            public void Update(GameTime gameTime)
            {
                ElapsedTime += gameTime.ElapsedGameTime;

                if (ElapsedTime > TimeSpan.FromSeconds(1))
                {
                    ElapsedTime -= TimeSpan.FromSeconds(1);
                    FrameRate = FrameCounter;
                    FrameCounter = 0;
                }
            }

            /// <summary>
            /// Frame Rate Counter Draw Method.
            /// </summary>
            /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
            public void Draw(SpriteBatch spriteBatch)
            {
                FrameCounter++;

                spriteBatch.DrawString(Font, Text + FrameRate, TextShadePosition, ShadeColor);
                spriteBatch.DrawString(Font, Text + FrameRate, TextPosition, FrameRate > 15 ? TextColor : AlertColor);
            }
        }
    }
}