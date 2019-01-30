using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.CORE.Graphics.Views
{
    /// <summary>
    /// A set of static methods involving common graphic settings.
    /// </summary>
    public static class ViewSettings
    {
        /// <summary>
        /// Enables/disables vertical sync (VSync).
        /// </summary>
        /// <param name="graphics">The graphics device manager in use. Intaken as a <see cref="GraphicsDeviceManager"/>.</param>
        /// <param name="isEnabled">Determines whether to enable/disable VSync and FixedTimeStep. Intaken as a <see cref="bool"/>.</param>
        /// <remarks>Call <see cref="GraphicsDeviceManager.ApplyChanges()"/> to apply any changes made to the graphics device.</remarks>
        /// <remarks>Disabling <see cref="GraphicsDeviceManager.SynchronizeWithVerticalRetrace"/> allows the game to refresh at higher frequencies than your monitor can support.</remarks>
        public static void SetVSync(this GraphicsDeviceManager graphics, bool isEnabled)
        {
            graphics.SynchronizeWithVerticalRetrace = isEnabled;
        }

        /// <summary>
        /// Sets the graphics profile. Either HiDef or Reach.
        /// </summary>
        /// <param name="graphics">The graphics device manager in use. Intaken as a <see cref="GraphicsDeviceManager"/>.</param>
        /// <param name="profile">the profile to use. Intaken as a <see cref="GraphicsProfile"/>.</param>
        /// <remarks>Call <see cref="GraphicsDeviceManager.ApplyChanges()"/> to apply any changes made to the graphics device.</remarks>
        public static void SetGraphicsProfile(this GraphicsDeviceManager graphics, GraphicsProfile profile)
        {
            graphics.GraphicsProfile = profile;
        }

        /// <summary>
        /// Sets the client window size.
        /// </summary>
        /// <param name="graphics">The graphics device manager in use. Intaken as a <see cref="GraphicsDeviceManager"/>.</param>
        /// <param name="width">The width to set the window. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">The height to set the window. Intaken as an <see cref="int"/>.</param>
        /// <remarks>Call <see cref="GraphicsDeviceManager.ApplyChanges()"/> to apply any changes made to the graphics device.</remarks>
        public static void SetWindowSize(this GraphicsDeviceManager graphics, int width, int height)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
        }

        /// <summary>
        /// Set the window to full screen mode.
        /// </summary>
        /// <param name="graphics">The graphics device manager in use. Intaken as a <see cref="GraphicsDeviceManager"/>.</param>
        /// <param name="isFullScreen">Determines whether the client should be set to full scree mode.</param>
        /// <remarks><see cref="GraphicsDeviceManager.HardwareModeSwitch"/> is set to 'true' in this call.
        /// Call <see cref="GraphicsDeviceManager.ApplyChanges()"/> to apply any changes made to the graphics device.</remarks>
        public static void SetFullScreen(this GraphicsDeviceManager graphics, bool isFullScreen)
        {
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = isFullScreen;
        }

        /// <summary>
        /// Sets the target frame rate.
        /// </summary>
        /// <param name="game">The current game. Intaken as a <see cref="Game"/>.</param>
        /// <param name="frameRate">The target frame rate to run the game. Intaken as a <see cref="double"/>.</param>
        public static void SetFrameRate(this Game game, double frameRate = 60.0D)
        {
            game.TargetElapsedTime = TimeSpan.FromSeconds(1 / frameRate);
        }

        /// <summary>
        /// Enables/disables the Fixed Time Step within MonoGame.
        /// </summary>
        /// <param name="game">The current game. Intaken as a <see cref="Game"/>.</param>
        /// <param name="isEnabled">Determines whether to enable/disable VSync and FixedTimeStep. Intaken as a <see cref="bool"/>.</param>
        /// <remarks>Disabling <see cref="Game.IsFixedTimeStep"/> allows the game to run at speeds greater/lesser than 60 frames per second.</remarks>
        public static void SetFixedTimeStep(this Game game, bool isEnabled)
        {
            game.IsFixedTimeStep = isEnabled;
        }
    }
}