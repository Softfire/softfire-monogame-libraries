using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public static class UIFeatures
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
            public int FrameRate { get; private set; }

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

        /// <summary>
        /// RectangleF.
        /// Custom Rectangle using floats.
        /// </summary>
        public struct RectangleF
        {
            /// <summary>
            /// Empty.
            /// </summary>
            public static RectangleF Empty => new RectangleF();

            /// <summary>
            /// Rectangle Tolerance.
            /// Precision Tolerance used in comparing floats.
            /// </summary>
            public static float Tolerance { get; set; } = 0f;

            /// <summary>
            /// Rectangle X Coordinate.
            /// </summary>
            public float X { get; set; }

            /// <summary>
            /// Rectangle Y Coordinate.
            /// </summary>
            public float Y { get; set; }

            /// <summary>
            /// Rectangle Width.
            /// </summary>
            public float Width { get; set; }

            /// <summary>
            /// Rectangle Height.
            /// </summary>
            public float Height { get; set; }

            /// <summary>
            /// Rectangle F.
            /// </summary>
            /// <param name="x">The X coordinate. Intaken as a float.</param>
            /// <param name="y">The Y coordinate. Intaken as a float.</param>
            /// <param name="width">The width. Intaken as a float.</param>
            /// <param name="height">The height. Intaken as a float.</param>
            public RectangleF(float x = 0, float y = 0, float width = 0, float height = 0)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            /// <summary>
            /// Top.
            /// Returns the Top edge coordinate.
            /// </summary>
            public float Top => Y;

            /// <summary>
            /// Bottom.
            /// Returns the Bottom edge coordinate.
            /// </summary>
            public float Bottom => Y + Height;

            /// <summary>
            /// Left.
            /// Returns the Left edge coordinate.
            /// </summary>
            public float Left => X;

            /// <summary>
            /// Right.
            /// Returns the Right edge coordinate.
            /// </summary>
            public float Right => X + Width;

            /// <summary>
            /// Center.
            /// Returns the Center coordinate as a Vector2.
            /// </summary>
            public Vector2 Center => new Vector2(X + Width / 2f, Y + Height / 2f);

            /// <summary>
            /// Position.
            /// Returns the X and Y coodinates as a Vector2.
            /// </summary>
            public Vector2 Position
            {
                get => new Vector2(X, Y);
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public static bool operator ==(RectangleF a, RectangleF b)
            {
                return Math.Abs(a.X - b.X) < Tolerance && Math.Abs(a.Y - b.Y) < Tolerance && Math.Abs(a.Width - b.Width) < Tolerance && Math.Abs(a.Height - b.Height) < Tolerance;
            }

            public bool Contains(float x, float y)
            {
                return X <= x && x < X + Width && Y <= y && y < Y + Height;
            }

            public bool Contains(Vector2 point)
            {
                return X <= point.X && point.X < X + Width && Y <= point.Y && point.Y < Y + Height;
            }

            public bool Contains(Point value)
            {
                return X <= value.X && value.X < X + Width && Y <= value.Y && value.Y < Y + Height;
            }

            public bool Contains(RectangleF value)
            {
                return X <= value.X && value.X + value.Width <= X + Width && Y <= value.Y && value.Y + value.Height <= Y + Height;
            }

            public static bool operator !=(RectangleF a, RectangleF b)
            {
                return !(a == b);
            }

            public void Offset(Vector2 offset)
            {
                X += offset.X;
                Y += offset.Y;
            }

            public void Offset(Point offset)
            {
                X += offset.X;
                Y += offset.Y;
            }

            public void Offset(float offsetX, float offsetY)
            {
                X += offsetX;
                Y += offsetY;
            }

            public void Inflate(int horizontalValue, int verticalValue)
            {
                X -= horizontalValue;
                Y -= verticalValue;
                Width += horizontalValue * 2;
                Height += verticalValue * 2;
            }

            public bool IsEmpty => Math.Abs(Width) < Tolerance && Math.Abs(Height) < Tolerance && Math.Abs(X) < Tolerance && Math.Abs(Y) < Tolerance;

            public bool Equals(RectangleF other)
            {
                return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is RectangleF f && Equals(f);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = X.GetHashCode();
                    hashCode = (hashCode * 397) ^ Y.GetHashCode();
                    hashCode = (hashCode * 397) ^ Width.GetHashCode();
                    hashCode = (hashCode * 397) ^ Height.GetHashCode();
                    return hashCode;
                }
            }

            public override string ToString()
            {
                return $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
            }
            
            public bool Intersects(Rectangle value)
            {
                return value.Left < Right &&
                       Left < value.Right &&
                       value.Top < Bottom &&
                       Top < value.Bottom;
            }


            public void Intersects(ref Rectangle value, out bool result)
            {
                result = value.Left < Right &&
                         Left < value.Right &&
                         value.Top < Bottom &&
                         Top < value.Bottom;
            }

            public static Rectangle Intersect(Rectangle value1, Rectangle value2)
            {
                Intersect(ref value1, ref value2, out var rectangle);
                return rectangle;
            }


            public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
            {
                if (value1.Intersects(value2))
                {
                    var right = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                    var left = Math.Max(value1.X, value2.X);
                    var top = Math.Max(value1.Y, value2.Y);
                    var bottom = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                    result = new Rectangle(left, top, right - left, bottom - top);
                }
                else
                {
                    result = new Rectangle(0, 0, 0, 0);
                }
            }

            public static Rectangle Union(Rectangle value1, Rectangle value2)
            {
                var x = Math.Min(value1.X, value2.X);
                var y = Math.Min(value1.Y, value2.Y);
                return new Rectangle(x, y,
                                     Math.Max(value1.Right, value2.Right) - x,
                                     Math.Max(value1.Bottom, value2.Bottom) - y);
            }

            public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
            {
                result.X = Math.Min(value1.X, value2.X);
                result.Y = Math.Min(value1.Y, value2.Y);
                result.Width = Math.Max(value1.Right, value2.Right) - result.X;
                result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
            }
        }
    }
}