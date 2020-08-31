using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.V2.Items
{
    /// <summary>
    /// A dedicated class for displaying text.
    /// </summary>
    public partial class UIText : UIBase
    {
        /// <summary>
        /// The text's available vertical alignments.
        /// </summary>
        public enum VerticalAlignments
        {
            /// <summary>
            /// Aligns the text to the top.
            /// </summary>
            Top,
            /// <summary>
            /// Aligns the text to the center.
            /// </summary>
            Center,
            /// <summary>
            /// Aligns the text to the bottom.
            /// </summary>
            Bottom
        }

        /// <summary>
        /// The text's available horizontal alignments.
        /// </summary>
        public enum HorizontalAlignments
        {
            /// <summary>
            /// Aligns the text to the left.
            /// </summary>
            Left,
            /// <summary>
            /// Aligns the text in the center.
            /// </summary>
            Center,
            /// <summary>
            /// Aligns the text to the right.
            /// </summary>
            Right
        }

        #region Fields

        /// <summary>
        /// The text's internal string value.
        /// </summary>
        private string _string;

        /// <summary>
        /// The text's internal altered string value.
        /// </summary>
        private string _alteredString;

        /// <summary>
        /// The text's internal vertical alignment value.
        /// </summary>
        private VerticalAlignments _verticalAlignment = VerticalAlignments.Center;

        /// <summary>
        /// The text's internal horizontal alignment value.
        /// </summary>
        private HorizontalAlignments _horizontalAlignment = HorizontalAlignments.Center;

        #endregion

        #region Properties

        /// <summary>
        /// The text to write.
        /// </summary>
        public string String
        {
            get => _string;
            set
            {
                _string = value;
                // Trigger a resizing of the text's string.
                ResizeText?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Any modifications performed to String, such as wrapping or scaling, should be passed to the AlteredString so as to keep the original String intact.
        /// </summary>
        public string AlteredString
        {
            get => _alteredString;
            set
            {
                _alteredString = value;
                // Trigger a resizing of the text.
                ResizeText?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The text's horizontal alignment.
        /// </summary>
        public HorizontalAlignments HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                ApplyHorizontalAlignment();
            }
        }


        /// <summary>
        /// The text's vertical alignment.
        /// </summary>
        public VerticalAlignments VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                ApplyVerticalAlignment();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles text resizing.
        /// </summary>
        private event EventHandler ResizeText;

        #endregion

        /// <summary>
        /// A text class used to display text.
        /// </summary>
        /// <param name="parent">The parent object. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="id">The text's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The text's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="font">The text's font. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="text">The text's text string to draw. Intaken as a <see cref="string"/>..</param>
        /// <param name="position">The text's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="isVisible">The text's visibility. Intaken as a <see cref="bool"/>.</param>
        public UIText(UIBase parent, int id, string name, SpriteFont font, string text, Vector2 position, bool isVisible = true) : base(parent, id, name, position, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y, isVisible)
        {
            Font = font;
            ResizeText += OnResizeText;
            String = string.IsNullOrWhiteSpace(text) ? "Text" : text;

            AlteredString = null;

            TextsOutlineStatus = new Dictionary<string, bool>
            {
                { "Top", false },
                { "Right", false },
                { "Bottom", false },
                { "Left", false },
                { "TopLeft", false },
                { "TopRight", false },
                { "BottomRight", false },
                { "BottomLeft", false },
            };
        }

        /// <summary>
        /// Applies the text's vertical alignments within a container.
        /// </summary>
        public void ApplyVerticalAlignment()
        {
            switch (VerticalAlignment)
            {
                case VerticalAlignments.Top:
                    Transform.Position = new Vector2(Transform.Position.X, -(Parent.Origin.Y + ((Size.Height * Transform.Scale.Y) / 2f)));
                    break;
                case VerticalAlignments.Center:
                    Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y);
                    break;
                case VerticalAlignments.Bottom:
                    Transform.Position = new Vector2(Transform.Position.X, Parent.Origin.Y - ((Size.Height * Transform.Scale.Y) / 2f));
                    break;
            }
        }

        /// <summary>
        /// Applies the text's horizontal alignments within a container.
        /// </summary>
        public void ApplyHorizontalAlignment()
        {
            switch (HorizontalAlignment)
            {
                case HorizontalAlignments.Left:
                    Transform.Position = new Vector2(-(Parent.Origin.X - ((Size.Width * Transform.Scale.X) / 2f)), Transform.Position.Y);
                    break;
                case HorizontalAlignments.Center:
                    Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y);
                    break;
                case HorizontalAlignments.Right:
                    Transform.Position = new Vector2(Parent.Origin.X - ((Size.Width * Transform.Scale.X) / 2f), Transform.Position.Y);
                    break;
            }
        }

        /// <summary>
        /// Gets the width and height of the String based on the current Font in use.
        /// </summary>
        /// <returns>Returns a <see cref="Vector2"/> defining the width and height of the String in the current Font.</returns>
        public Vector2 GetLength()
        {
            return Font.MeasureString(AlteredString ?? String);
        }
        
        /// <summary>
        /// Clears the <see cref="AlteredString"/>.
        /// </summary>
        public void Clear()
        {
            AlteredString = null;
        }

        /// <summary>
        /// Event handling the resizing of text.
        /// </summary>
        private void OnResizeText(object sender, EventArgs e)
        {
            Size.Width = (int)GetLength().X;
            Size.Height = (int)GetLength().Y;
        }

        /// <summary>
        /// Updates the text's attributes.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            if (WriteSpeed == WriteSpeeds.Delayed)
            {
                ElapsedTime += DeltaTime;
            }
            else
            {
                ResetElapsedTime();
            }

            foreach (var component in Children)
            {
                component?.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// The text's draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform)
        {
            // Draw base.
            base.Draw(spriteBatch, transform);

            if (IsVisible)
            {
                Write(spriteBatch, transform, string.IsNullOrWhiteSpace(AlteredString) ? String : AlteredString);
            }
        }
    }
}