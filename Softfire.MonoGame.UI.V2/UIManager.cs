using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.UI.Effects;
using Softfire.MonoGame.UI.Themes;
using System;
using System.Collections.Generic;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// A UI manager class. Provides UI groups, themes and fonts.
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// The UI manager's content manager.
        /// </summary>
        private ContentManager Content { get; }

        /// <summary>
        /// The UI manager's spritebatch.
        /// </summary>
        private SpriteBatch SpriteBatch { get; }
        
        /// <summary>
        /// The graphics device used by the UI manager.
        /// </summary>
        private GraphicsDevice GraphicsDevice { get; }
        
        /// <summary>
        /// Loaded fonts availble for use.
        /// </summary>
        public UIFontManager Fonts { get; }

        /// <summary>
        /// The UI groups in use.
        /// </summary>
        internal List<UIGroup> Groups { get; } = new List<UIGroup>();

        /// <summary>
        /// A Theme manager.
        /// Add, get, remove, reorder and apply themes.
        /// </summary>
        public UIThemeManager Themes { get; } = new UIThemeManager();

        /// <summary>
        /// The UI manager creates, maintains, updates and draws all UI and their contents.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use to display the UI. Intakes a GraphicsDevice.</param>
        /// <param name="parentContentManager">The parent content manager used to generate an independent content manager for UI elements. Intakes a ContentManager.</param>
        public UIManager(GraphicsDevice graphicsDevice, ContentManager parentContentManager)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            UIBase.GraphicsDevice = GraphicsDevice;
            Content = new ContentManager(parentContentManager.ServiceProvider, "Content");
            Fonts = new UIFontManager(Content);
        }

        #region Groups

        /// <summary>
        /// Adds a new UI group.
        /// </summary>
        /// <param name="name">The group's name. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the group id, if added, otherwise zero.</returns>
        /// <remarks>If a group already exists with the provided name then a -1 is returned indicating failure to add the group.</remarks>
        public int AddGroup(string name)
        {
            var nextGroupId = -1;

            if (!GroupExists(name))
            {
                nextGroupId = Identities.GetNextValidObjectId<UIGroup, UIGroup>(Groups);

                if (!GroupExists(nextGroupId))
                {
                    var newGroup = new UIGroup(this, nextGroupId, name);
                    Groups.Add(newGroup);
                }
            }

            return nextGroupId;
        }

        /// <summary>
        /// Determines whether a group exists, by id.
        /// </summary>
        /// <param name="id">The id of the group to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the group exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public bool GroupExists(int id) => Identities.ObjectExists<UIGroup, UIGroup>(Groups, id);

        /// <summary>
        /// Determines whether a group exists, by name.
        /// </summary>
        /// <param name="name">The name of the group to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the group exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public bool GroupExists(string name) => Identities.ObjectExists<UIGroup, UIGroup>(Groups, name);

        /// <summary>
        /// Retrieves a group by id.
        /// </summary>
        /// <param name="id">The id of the requested group. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="UIGroup"/>, if found, otherwise null.</returns>
        public UIGroup GetGroup(int id) => Identities.GetObject<UIGroup, UIGroup>(Groups, id);

        /// <summary>
        /// Retrieves a group by name.
        /// </summary>
        /// <param name="name">The name of the requested group. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="UIGroup"/>, if found, otherwise null.</returns>
        public UIGroup GetGroup(string name) => Identities.GetObject<UIGroup, UIGroup>(Groups, name);

        /// <summary>
        /// Removes a group by id.
        /// </summary>
        /// <param name="id">The id of the requested group. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the group was removed.</returns>
        public bool RemoveGroup(int id) => Identities.RemoveObject<UIGroup, UIGroup>(Groups, id);

        /// <summary>
        /// Removes a group by name.
        /// </summary>
        /// <param name="name">The name of the requested group. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the group was removed.</returns>
        public bool RemoveGroup(string name) => Identities.RemoveObject<UIGroup, UIGroup>(Groups, name);

        #endregion

        /// <summary>
        /// Gets the viewport's dimensions.
        /// </summary>
        /// <returns>Returns a Rectangle of the viewport's dimensions.</returns>
        internal RectangleF GetViewportDimensions()
        {
            var position = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            var width = GraphicsDevice.Viewport.Width;
            var height = GraphicsDevice.Viewport.Height;

            return new RectangleF(position.X, position.Y, width, height);
        }
        
        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public void Update(GameTime gameTime)
        {
            // UI Effects Delta Time.
            UIEffectBase.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var group in Groups)
            {
                group.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        public void Draw()
        {
            foreach (var group in Groups)
            {
                group.Draw(SpriteBatch, Matrix.Identity);
            }
        }
    }
}