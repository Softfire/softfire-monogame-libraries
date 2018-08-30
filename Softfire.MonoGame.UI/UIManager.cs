using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects;
using Softfire.MonoGame.UI.Themes;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UIManager.
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// The UI manager's content manager.
        /// </summary>
        private ContentManager Content { get; }

        /// <summary>
        /// The graphics device used by the UI manager.
        /// </summary>
        private GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Loaded fonts availble for use.
        /// </summary>
        public UIFonts Fonts { get; }

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
        /// <param name="parentContentManager">The parent content manager used to generate an independant content manager for UI elements. Intakes a ContentManager.</param>
        public UIManager(GraphicsDevice graphicsDevice, ContentManager parentContentManager)
        {
            GraphicsDevice = graphicsDevice;
            UIBase.GraphicsDevice = GraphicsDevice;
            Content = new ContentManager(parentContentManager.ServiceProvider, "Content");
            Fonts = new UIFonts(Content);
        }

        #region Groups

        /// <summary>
        /// Adds a new UI group.
        /// </summary>
        /// <param name="groupName">The group's name. Intaken as a string.</param>
        /// <returns>Returns the group id, if added, otherwise zero.</returns>
        /// <remarks>If a group already exists with the provided name then a zero is returned indicating failure to add the group.</remarks>
        public int AddGroup(string groupName)
        {
            var nextGroupId = 0;

            if (CheckForGroup(groupName) == false)
            {
                nextGroupId = UIBase.GetNextValidItemId(Groups);

                if (CheckForGroup(nextGroupId) == false)
                {
                    Groups.Add(new UIGroup(this, nextGroupId, groupName, nextGroupId));
                }
            }

            return nextGroupId;
        }

        /// <summary>
        /// Checks for a group by id.
        /// </summary>
        /// <param name="groupId">The id of the group to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the group is present.</returns>
        public bool CheckForGroup(int groupId)
        {
            return UIBase.CheckItemById(Groups, groupId);
        }

        /// <summary>
        /// Checks for a group by name.
        /// </summary>
        /// <param name="groupName">The name of the group to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the group is present.</returns>
        public bool CheckForGroup(string groupName)
        {
            return UIBase.CheckItemByName(Groups, groupName);
        }

        /// <summary>
        /// Gets a group by id.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns the group with the specified id, if present, otherwise null.</returns>
        public UIGroup GetGroup(int groupId)
        {
            return CheckForGroup(groupId) ? UIBase.GetItemById(Groups, groupId) : default(UIGroup);
        }

        /// <summary>
        /// Gets a group by name.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns the group with the specified name, if present, otherwise null.</returns>
        public UIGroup GetGroup(string groupName)
        {
            return CheckForGroup(groupName) ? UIBase.GetItemByName(Groups, groupName) : default(UIGroup);
        }

        /// <summary>
        /// Removes a group by id.
        /// </summary>
        /// <param name="groupId">The id of the group to remove. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group was removed.</returns>
        public bool RemoveGroup(int groupId)
        {
            return UIBase.RemoveItemById(Groups, groupId);
        }

        /// <summary>
        /// Removes a group by name.
        /// </summary>
        /// <param name="groupName">The name of the group to remove. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group was removed.</returns>
        public bool RemoveGroup(string groupName)
        {
            return UIBase.RemoveItemByName(Groups, groupName);
        }

        /// <summary>
        /// Increases a groups order number by id.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was increased.</returns>
        public bool IncreaseGroupOrderNumber(int groupId)
        {
            return UIBase.IncreaseItemOrderNumber(Groups, groupId);
        }

        /// <summary>
        /// Increases a groups order number by name.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was increased.</returns>
        public bool IncreaseGroupOrderNumber(string groupName)
        {
            return UIBase.IncreaseItemOrderNumber(Groups, groupName);
        }

        /// <summary>
        /// Decreases a groups order number by id.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was decreased.</returns>
        public bool DecreaseGroupOrderNumber(int groupId)
        {
            return UIBase.DecreaseItemOrderNumber(Groups, groupId);
        }

        /// <summary>
        /// Decreases a groups order number by name.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was decreased.</returns>
        public bool DecreaseGroupOrderNumber(string groupName)
        {
            return UIBase.DecreaseItemOrderNumber(Groups, groupName);
        }

        #endregion

        /// <summary>
        /// Gets the viewport's dimensions.
        /// </summary>
        /// <returns>Returns a Rectangle of the viewport's dimensions.</returns>
        internal Rectangle GetViewportDimenions()
        {
            return GraphicsDevice.Viewport.Bounds;
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public async Task Update(GameTime gameTime)
        {
            // UI Effects Delta Time.
            UIEffectBase.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update order is ascending.
            foreach (var group in Groups.OrderBy(grp => grp.OrderNumber))
            {
                await group.Update(gameTime).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Order is Ascending.
            // Drawing from lowest to highest.
            foreach (var group in Groups.OrderBy(grp => grp.OrderNumber))
            {
                group.Draw(spriteBatch);
            }
        }
    }
}