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
    /// Maintains UI Windows.
    /// Add, Remove and Update Windows.
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// UIManager Content Manager.
        /// </summary>
        public static ContentManager Content { private get; set; }

        /// <summary>
        /// UIManager Graphics Device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice { private get; set; }

        /// <summary>
        /// Fonts.
        /// </summary>
        public UIFonts Fonts { get; }

        /// <summary>
        /// UIManager InputDeviceRectangle.
        /// The Rectangle of an input device that will be used to confirm if a Window is in focus.
        /// </summary>
        private Dictionary<string, Rectangle> InputDevices { get; }

        /// <summary>
        /// UIManager Window Dictionary.
        /// </summary>
        private Dictionary<string, UIGroup> Groups { get; }

        /// <summary>
        /// UIManager Constructor.
        /// UIManager updates and draws all UI and their contents.
        /// </summary>
        /// <param name="graphicsDevice">Intakes a GraphicsDevice.</param>
        /// <param name="parentContentManager">Intakes a ContentManager.</param>
        public UIManager(GraphicsDevice graphicsDevice, ContentManager parentContentManager)
        {
            UIBase.GraphicsDevice = GraphicsDevice = graphicsDevice;
            UIBase.Content = Content = new ContentManager(parentContentManager.ServiceProvider, "Content");
            Fonts = new UIFonts(Content);
            
            Groups = new Dictionary<string, UIGroup>();
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

        /// <summary>
        /// Add Group.
        /// Adds a new Window group that can be accessed with CurrentWindowGroup.
        /// </summary>
        /// <param name="name">The unique Group Name. Intaken as a string.</param>
        /// <param name="orderNumber">Intakes the Group's Update/Draw Order Number as an int.</param>
        /// <returns>Returns a bool indicating whether the Group was added.</returns>
        public bool AddGroup(string name, int orderNumber)
        {
            var result = false;

            if (Groups.ContainsKey(name) == false)
            {
                Groups.Add(name, new UIGroup(name, orderNumber, GraphicsDevice));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Group.
        /// Get a specific UIGroup by it's identifier, if it exists.
        /// </summary>
        /// <param name="name">Intakes a unique group name as a string.</param>
        /// <returns>Returns the Group with the specified name, if present, otherwise null.</returns>
        public UIGroup GetGroup(string name)
        {
            UIGroup result = null;

            if (Groups.ContainsKey(name))
            {
                result = Groups[name];
            }

            return result;
        }

        /// <summary>
        /// Remove Window.
        /// Removes the named Group, if it is present.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Group was removed.</returns>
        public bool RemoveGroup(string identifier)
        {
            var result = false;

            if (Groups.ContainsKey(identifier))
            {
                result = Groups.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Reorder Group Up.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Group's Order Number was increased.</returns>
        public bool ReorderGroupUp(string identifier)
        {
            var result = false;
            UIGroup group;

            if ((group = GetGroup(identifier)) != null)
            {
                group.OrderNumber++;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Reorder Group Down.
        /// </summary>
        /// <param name="identifier">Intakes a unique Identifier as a string.</param>
        /// <returns>Returns a boolean indicating whether the Group's Order Number was decreased.</returns>
        public bool ReorderGroupDown(string identifier)
        {
            var result = false;
            UIGroup group;

            if ((group = GetGroup(identifier)) != null)
            {
                group.OrderNumber--;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public async Task Update(GameTime gameTime)
        {
            // UI Effects Delta Time.
            UIBaseEffect.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update Order is Ascending.
            // From lowest to highest.
            foreach (var group in Groups.OrderBy(grp => grp.Value.OrderNumber))
            {
                if (group.Value.IsActive)
                {
                    group.Value.ActiveInputDevices = InputDevices.Values.ToList();
                }

                await group.Value.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        public void Draw()
        {
            // Draw Order is Ascending.
            // Drawing from lowest to highest.
            foreach (var group in Groups.OrderBy(grp => grp.Value.OrderNumber))
            {
                group.Value.Draw();
            }
        }
    }
}