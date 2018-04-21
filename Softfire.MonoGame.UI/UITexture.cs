using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UITexture : UIBase
    {
        /// <summary>
        /// Catalogue.
        /// </summary>
        private Dictionary<string, Texture2D> Catalogue { get; }

        /// <summary>
        /// Selected Texture.
        /// </summary>
        private Texture2D SelectedTexture { get; set; }
        
        /// <summary>
        /// UITexture.
        /// </summary>
        public UITexture()
        {
            Catalogue = new Dictionary<string, Texture2D>
            {
                {"Default", CreateTexture2D()}
            };
        }

        /// <summary>
        /// Add.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <param name="filePath">Intakes the texture's file path as a string.</param>
        /// <returns>Returns a boolean indicating whether the texture was added.</returns>
        public bool Add(string identifier, string filePath)
        {
            var result = false;

            if (CheckForTexture(identifier) == false)
            {
                var texture = Content.Load<Texture2D>(filePath);
                if (texture != null)
                {
                    Catalogue.Add(identifier, texture);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Remove.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns a boolean indicating whether the texture was removed.</returns>
        public bool Remove(string identifier)
        {
            var result = false;

            if (CheckForTexture(identifier) &&
                Catalogue[identifier].Equals(SelectedTexture) == false)
            {
                result = Catalogue.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Check For Texture.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns a boolean indicating whether the named texture was found.</returns>
        public bool CheckForTexture(string identifier)
        {
            return Catalogue.ContainsKey(identifier);
        }

        /// <summary>
        /// Select Texture.
        /// </summary>
        /// <param name="identifier">Intakes an identifying name as a string.</param>
        /// <returns>Returns a boolean indicating whether the texture was selected.</returns>
        public bool SelectTexture(string identifier)
        {
            var result = false;

            if (CheckForTexture(identifier))
            {
                SelectedTexture = Catalogue[identifier];
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">MonoGame's GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            if (SelectedTexture != null)
            {
                Width = SelectedTexture.Width;
                Height = SelectedTexture.Height;
            }

            await base.Update(gameTime);
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        /// <param name="spriteBatch">MonoGame's SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(SelectedTexture ?? Catalogue["Default"], Position, null, Color.White * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
        }
    }
}