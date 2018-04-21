using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.SM
{
    public interface IState
    {
        /// <summary>
        /// DeltaTime.
        /// Time since last update.
        /// </summary>
        double DeltaTime { get; }

        /// <summary>
        /// Parent State Manager.
        /// Holds a reference to the state manager in which this State is being managed.
        /// </summary>
        StateManager ParentStateManager { get; set; }

        /// <summary>
        /// State Content Manager.
        /// </summary>
        ContentManager Content { get; set; }

        /// <summary>
        /// Camera.
        /// </summary>
        IOCamera2D Camera { get; }

        #region Properties

        /// <summary>
        /// Name.
        /// Must be unique.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Rectangle.
        /// Defines the drawable area.
        /// </summary>
        Rectangle Rectangle { get; set; }

        /// <summary>
        /// Origin.
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// Rotation Angle.
        /// </summary>
        double RotationAngle { get; set; }

        /// <summary>
        /// Background Color.
        /// Used for transitions.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Transparency.
        /// Used for transition effects.
        /// </summary>
        float Transparency { get; set; }

        /// <summary>
        /// Texture.
        /// Used for background color.
        /// </summary>
        Texture2D BackgroundTexture { set; }

        /// <summary>
        /// Order Number.
        /// State will be run in order from smallest to largest.
        /// </summary>
        int OrderNumber { get; set; }

        #endregion

        #region Booleans

        /// <summary>
        /// Has Focus?
        /// </summary>
        bool HasFocus { get; set; }

        /// <summary>
        /// Is Visible?
        /// Indicates whether the Draw method will be called.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Activate Transitions.
        /// Call to perform any transitions that have been loaded.
        /// </summary>
        bool ActivateTransitions { get; set; }

        /// <summary>
        /// Is Transitioning?
        /// </summary>
        bool IsTransitioning { get; }

        /// <summary>
        /// Activate Sleep.
        /// Call to activate Sleep.
        /// </summary>
        bool ActivateSleep { get; set; }

        /// <summary>
        /// Is Sleeping?
        /// </summary>
        bool IsSleeping { get; }

        /// <summary>
        /// Activate Wake.
        /// Call to activate Wake.
        /// </summary>
        bool ActivateWake { get; set; }

        /// <summary>
        /// Is Waking?
        /// </summary>
        bool IsWaking { get; }

        #endregion

        #region Sleep

        /// <summary>
        /// Lull.
        /// Lulls the State to Sleep.
        /// </summary>
        void Lull();

        /// <summary>
        /// Sleep.
        /// Called to sleep the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        Task<bool> Sleep();

        #endregion

        #region Wake

        /// <summary>
        /// Awaken.
        /// Wakes the State from Sleep.
        /// </summary>
        void Awaken();

        /// <summary>
        /// Wake.
        /// Called to waken the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        Task<bool> Wake();

        #endregion

        /// <summary>
        /// Load Content Method.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// State Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        Task Update(GameTime gameTime);

        /// <summary>
        /// State Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}