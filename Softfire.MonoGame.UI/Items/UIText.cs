using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects.Coloring;

namespace Softfire.MonoGame.UI.Items
{
    public partial class UIText : UIBase
    {
        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Text Altered String.
        /// Any modifications performed to String, such as wrapping or scaling, should be passed to the AlteredString so as to keep the original String intact.
        /// </summary>
        public string AlteredString { get; set; }

        /// <summary>
        /// Selection Text Color.
        /// </summary>
        public Color SelectionColor { get; set; }

        /// <summary>
        /// Horizontal Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public HorizontalAlignments HorizontalAlignment { get; set; } = HorizontalAlignments.Center;

        /// <summary>
        /// Horizontal Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public enum HorizontalAlignments
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        /// Vertical Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public VerticalAlignments VerticalAlignment { get; set; } = VerticalAlignments.Center;

        /// <summary>
        /// Vertical Alignments.
        /// </summary>
        public enum VerticalAlignments
        {
            Upper,
            Center,
            Lower
        }

        /// <summary>
        /// Texture.
        /// Used for Highlighting.
        /// </summary>
        private Texture2D Texture { get; set; }

        /// <summary>
        /// UIText Constructor. 
        /// Can be customized and displayed.
        /// </summary>
        /// <param name="id">The text's id. Intaken as an int.</param>
        /// <param name="name">The text's name.</param>
        /// <param name="font">Intakes a SpriteFont.</param>
        /// <param name="text">Intakes the text to output as a string.</param>
        /// <param name="orderNumber">Intakes an int defining the update/draw order number.</param>
        /// <param name="position">Intakes the text's position as a Vector2.</param>
        public UIText(int id, string name, SpriteFont font, string text, int orderNumber, Vector2 position = new Vector2()) : base(id, name, position, (int)font.MeasureString(text).X,
                                                                                                                                                       (int)font.MeasureString(text).Y,
                                                                                                                                                       orderNumber)
        {
            Font = font;
            String = text ?? "Text";
            SelectionColor = Color.LightGray;

            AlteredString = null;
            ActivateOutlines(OutlineDepth);

            Defaults.Font = Font;
            Defaults.Texts.Add(1, String);
        }

        /// <summary>
        /// Action to be performed if Text is actioned.
        /// </summary>
        /// <param name="isActioned">Boolean expression determining whether the text has been actioned.</param>
        /// <param name="action">Action to perform.</param>
        public void Action(bool isActioned, Action action)
        {
            if (isActioned)
            {
                action();
            }
        }

        /// <summary>
        /// Set Alignments.
        /// </summary>
        private void SetAlignments()
        {
            switch (VerticalAlignment)
            {
                case VerticalAlignments.Upper:
                    Position = new Vector2(Position.X, Position.Y - Height / 2f);
                    break;
                case VerticalAlignments.Center:
                    Position = new Vector2(Position.X, Position.Y);
                    break;
                case VerticalAlignments.Lower:
                    Position = new Vector2(Position.X, Position.Y + Height / 2f);
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignments.Left:
                    Position = new Vector2(Position.X + Width / 2f, Position.Y);
                    break;
                case HorizontalAlignments.Center:
                    Position = new Vector2(Position.X, Position.Y);
                    break;
                case HorizontalAlignments.Right:
                    Position = new Vector2(Position.X - Width / 2f, Position.Y);
                    break;
            }
        }

        /// <summary>
        /// Get Length.
        /// Use to get the length and width of the String based on the current Font in use as a Vector2. Vector2(Width, Height).
        /// </summary>
        /// <returns>Returns a Vector2 defining the width and height of the String in the current Font.</returns>
        public Vector2 GetLength()
        {
            return Font.MeasureString(AlteredString ?? String);
        }

        /// <summary>
        /// Reset String.
        /// Uses Defaults.
        /// </summary>
        protected void ResetString()
        {
            String = Defaults.Texts[0];
        }

        /// <summary>
        /// Reset AlteredString.
        /// Uses Defaults.
        /// </summary>
        protected void ResetAlteredString()
        {
            AlteredString = null;
        }
        
        /// <summary>
        /// Clear.
        /// Clears the String and AlteredString.
        /// </summary>
        public void Clear()
        {
            ResetString();
            ResetAlteredString();
        }

        /// <summary>
        /// Load Content.
        /// </summary>
        public override void LoadContent()
        {
            Texture = CreateTexture2D();
            base.LoadContent();
        }

        /// <summary>
        /// Update Method.
        /// Updates UIText attributes.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                ElapsedTime += DeltaTime;

                Width = (int)GetLength().X;
                Height = (int)GetLength().Y;

                SetAlignments();

                await base.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw Method.
        /// Draws UIText.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);

                Write(spriteBatch, string.IsNullOrWhiteSpace(AlteredString) ? String : AlteredString);
            }
        }
    }
}