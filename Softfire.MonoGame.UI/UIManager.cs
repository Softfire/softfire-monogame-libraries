using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UIManager.
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// UIManager Content Manager.
        /// </summary>
        private ContentManager Content { get; }

        /// <summary>
        /// UIManager Graphics Device.
        /// </summary>
        private GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Fonts.
        /// </summary>
        public UIFonts Fonts { get; }

        /// <summary>
        /// Input Devices.
        /// </summary>
        internal Dictionary<string, Rectangle> InputDevices { get; }

        /// <summary>
        /// UIManager Window Dictionary.
        /// </summary>
        private List<UIGroup> Groups { get; } = new List<UIGroup>();

        /// <summary>
        /// UIManager Constructor.
        /// UIManager updates and draws all UI and their contents.
        /// </summary>
        /// <param name="graphicsDevice">Intakes a GraphicsDevice.</param>
        /// <param name="parentContentManager">Intakes a ContentManager.</param>
        public UIManager(GraphicsDevice graphicsDevice, ContentManager parentContentManager)
        {
            GraphicsDevice = graphicsDevice;
            UIBase.GraphicsDevice = GraphicsDevice;
            Content = new ContentManager(parentContentManager.ServiceProvider, "Content");
            Fonts = new UIFonts(Content);

            InputDevices = new Dictionary<string, Rectangle>();
        }

        /// <summary>
        /// Add Input Device.
        /// Pass in the Rectangle of an input device to have it checked for detection against all Windows.
        /// Features can be available once focus has been given. Such as scrolling and zoom.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="deviceRectangle">Intakes a device Rectangle.</param>
        public void AddInputDevice(string identifier, Rectangle deviceRectangle)
        {
            if (InputDevices.ContainsKey(identifier) == false)
            {
                InputDevices.Add(identifier, deviceRectangle);
            }
        }

        /// <summary>
        /// Update Input Device.
        /// Updates the corresponding input device's rectangle.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="deviceRectangle">Intakes a device Rectangle.</param>
        public void UpdateInputDevice(string identifier, Rectangle deviceRectangle)
        {
            if (InputDevices.ContainsKey(identifier))
            {
                InputDevices[identifier] = deviceRectangle;
            }
        }

        #region Groups

        /// <summary>
        /// Add Group.
        /// </summary>
        /// <param name="name">The group's name. Intaken as a string.</param>
        /// <returns>Returns the group id of the newly added group as an int.</returns>
        public int AddGroup(string name)
        {
            var nextGroupId = UIBase.GetNextValidItemId(Groups);

            var newGroup = new UIGroup(this, nextGroupId, name, nextGroupId);

            Groups.Add(newGroup);

            return nextGroupId;
        }

        /// <summary>
        /// Get Group.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns the group with the specified name, if present, otherwise null.</returns>
        public UIGroup GetGroup(int groupId)
        {
            return UIBase.GetItemById(Groups, groupId);
        }

        /// <summary>
        /// Get Group.
        /// </summary>
        /// <param name="name">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns the group with the specified name, if present, otherwise null.</returns>
        public UIGroup GetGroup(string name)
        {
            return UIBase.GetItemByName(Groups, name);
        }

        /// <summary>
        /// Remove Group.
        /// </summary>
        /// <param name="groupId">The id of the group to remove. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group was removed.</returns>
        public bool RemoveGroup(int groupId)
        {
            return UIBase.RemoveItemById(Groups, groupId);
        }

        /// <summary>
        /// Remove Group.
        /// </summary>
        /// <param name="groupName">The name of the group to remove. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group was removed.</returns>
        public bool RemoveGroup(string groupName)
        {
            return UIBase.RemoveItemByName(Groups, groupName);
        }

        /// <summary>
        /// Increase Group Order Number.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was increased.</returns>
        public bool IncreaseGroupOrderNumber(int groupId)
        {
            return UIBase.IncreaseItemOrderNumber(Groups, groupId);
        }

        /// <summary>
        /// Increase Group Order Number.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was increased.</returns>
        public bool IncreaseGroupOrderNumber(string groupName)
        {
            return UIBase.IncreaseItemOrderNumber(Groups, groupName);
        }

        /// <summary>
        /// Decrease Group Order Number.
        /// </summary>
        /// <param name="groupId">The id of the group to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was decreased.</returns>
        public bool DecreaseGroupOrderNumber(int groupId)
        {
            return UIBase.DecreaseItemOrderNumber(Groups, groupId);
        }

        /// <summary>
        /// Decrease Group Order Number.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group's order number was decreased.</returns>
        public bool DecreaseGroupOrderNumber(string groupName)
        {
            return UIBase.DecreaseItemOrderNumber(Groups, groupName);
        }

        #endregion

        /// <summary>
        /// Get Viewport Dimensions.
        /// </summary>
        /// <returns></returns>
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

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var group in Groups.OrderBy(grp => grp.OrderNumber))
            {
                await group.Update(gameTime);
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