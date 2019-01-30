using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Items
{
    /// <summary>
    /// A UI button.
    /// </summary>
    public class UIButton : UIBase
    {
        /// <summary>
        /// Button layers.
        /// </summary>
        private enum Layers
        {
            /// <summary>
            /// The base layer.
            /// </summary>
            Base,
            /// <summary>
            /// The text layer.
            /// </summary>
            Text,
            /// <summary>
            /// The overlay layer.
            /// </summary>
            Overlay
        }

        /// <summary>
        /// A UI button element.
        /// </summary>
        /// <param name="parent">The parent object of this button. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="id">The button's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The button's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The button's position. Intaken as a Vector2.</param>
        /// <param name="width">The button's width. Intaken as a float.</param>
        /// <param name="height">The button's height. Intaken as a float.</param>
        /// <param name="isVisible">The button's visibility. Intaken as a <see cref="bool"/>.</param>
        public UIButton(UIBase parent, int id, string name, Vector2 position, int width, int height, bool isVisible = true) : base(parent, id, name, position, width, height, isVisible)
        {
            Paddings.SetPadding(5);
        }

        #region Text

        /// <summary>
        /// Adds text.
        /// </summary>
        /// <param name="name">The text's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="font">The text's font. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="text">The text's text. Intaken as a <see cref="string"/>.</param>
        /// <param name="isVisible">The button's visibility. Intaken as a <see cref="bool"/>.</param>
        /// <returns>Returns the text id, if added, otherwise zero.</returns>
        /// <remarks>If text already exists with the provided name then a zero is returned indicating failure to add the text.</remarks>
        public int AddText(string name, SpriteFont font, string text, bool isVisible = true)
        {
            var nextTextId = 0;

            if (!TextExists(name))
            {
                nextTextId = GetNextValidChildId<UIText>((int)Layers.Text);

                if (!TextExists(nextTextId))
                {
                    var newText = new UIText(this, nextTextId, name, font, text, Vector2.Zero, isVisible)
                    {
                        Layer = (int)Layers.Text,
                        IsSelectable = false,
                        IsHoverable = false,
                        IsBackgroundVisible = false,
                        IsHighlightable = false
                    };

                    newText.SetOutlineVisibility(false);

                    newText.Transform.Parent = Transform;
                    newText.LoadContent();

                    Children.Add(newText);
                }
            }

            return nextTextId;
        }

        /// <summary>
        /// Determines whether a text exists, by id.
        /// </summary>
        /// <param name="id">The id of the text to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the text exists.</returns>
        public bool TextExists(int id) => ChildExists<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Determines whether a text exists, by name.
        /// </summary>
        /// <param name="name">The name of the text to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the text exists.</returns>
        public bool TextExists(string name) => ChildExists<UIText>((int)Layers.Text, name);

        /// <summary>
        /// Gets text, by id.
        /// </summary>
        /// <param name="id">The id of the text to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a text with the requested id, if present, otherwise null.</returns>
        public UIText GetText(int id) => GetChild<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Gets text, by name.
        /// </summary>
        /// <param name="name">The name of the text to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a text with the requested name, if present, otherwise null.</returns>
        public UIText GetText(string name) => GetChild<UIText>((int)Layers.Text, name);

        /// <summary>
        /// Removes text, by id.
        /// </summary>
        /// <param name="id">The id of the text to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text was removed.</returns>
        public bool RemoveText(int id) => RemoveChild<UIText>((int)Layers.Text, id);

        /// <summary>
        /// Removes text, by name.
        /// </summary>
        /// <param name="name">The name of the text to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the text was removed.</returns>
        public bool RemoveText(string name) => RemoveChild<UIText>((int)Layers.Text, name);

        #endregion

        /// <summary>
        /// The button's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var component in Children)
            {
                if (component.Layer == (int)Layers.Text)
                {
                    Size.Width = component.Size.Width;
                    Size.Height = component.Size.Height;
                }

                component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// The button's draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            base.Draw(spriteBatch, transform);

            if (IsVisible)
            {
                foreach (var component in Children)
                {
                    component.Draw(spriteBatch, transform);
                }
            }
        }
    }
}