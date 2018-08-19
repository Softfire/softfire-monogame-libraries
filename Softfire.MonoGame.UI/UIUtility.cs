using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public static class UIUtility
    {
        /// <summary>
        /// Frame Rate Counter.
        /// </summary>
        public sealed class FrameRateCounter
        {
            /// <summary>
            /// Text.
            /// </summary>
            private string Text { get; }

            /// <summary>
            /// Text Position.
            /// </summary>
            private Vector2 TextPosition { get; }

            /// <summary>
            /// Text Shade Position.
            /// </summary>
            private Vector2 TextShadePosition { get; }

            /// <summary>
            /// Text SpriteFont.
            /// </summary>
            private SpriteFont Font { get; }

            /// <summary>
            /// Text Color.
            /// </summary>
            private Color TextColor { get; }

            /// <summary>
            /// Text Shade Color.
            /// </summary>
            private Color ShadeColor { get; }

            /// <summary>
            /// Text Alert Color.
            /// </summary>
            private Color AlertColor { get; }

            /// <summary>
            /// Text Frame Rate.
            /// </summary>
            private int FrameRate { get; set; }

            /// <summary>
            /// Text Frame Counter.
            /// </summary>
            private int FrameCounter { get; set; }

            /// <summary>
            /// Elapsed Time.
            /// </summary>
            private TimeSpan ElapsedTime { get; set; }

            /// <summary>
            /// Frame Rate Counter.
            /// Use to keep track of your current FPS.
            /// </summary>
            /// <param name="textPosition">Intakes a Vector2 to define where to draw the Counter.</param>
            /// <param name="textFont">Intakes a SpriteFont defining the Font to use.</param>
            /// <param name="customText">Intakes a string to be used in place of the default text. Default is "FPS: ##".</param>
            /// <param name="textColor">Intakes a Color to use for the text.</param>
            public FrameRateCounter(Vector2 textPosition, SpriteFont textFont, string customText = null, Color? textColor = null)
            {
                FrameRate = 0;
                FrameCounter = 0;
                ElapsedTime = TimeSpan.Zero;

                TextPosition = textPosition;
                Font = textFont;
                Text = customText ?? "FPS: ";

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

                spriteBatch.Begin();
                spriteBatch.DrawString(Font, Text + FrameRate, TextShadePosition, ShadeColor);
                spriteBatch.DrawString(Font, Text + FrameRate, TextPosition, FrameRate > 15 ? TextColor : AlertColor);
                spriteBatch.End();
            }
        }
    }
}