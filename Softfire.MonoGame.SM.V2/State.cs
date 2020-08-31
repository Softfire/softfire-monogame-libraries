using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.IO.V2;
using Softfire.MonoGame.SM.V2.Transitions;

namespace Softfire.MonoGame.SM.V2
{
    public abstract partial class State : IState
    {
        /// <summary>
        /// DeltaTime.
        /// Time since last update.
        /// </summary>
        public double DeltaTime { get; protected set; }

        /// <summary>
        /// Parent State Manager.
        /// Holds a reference to the state manager in which this State is being managed.
        /// </summary>
        public StateManager ParentStateManager { get; set; }

        /// <summary>
        /// State Content Manager.
        /// </summary>
        public ContentManager Content { get; set; }

        /// <summary>
        /// Camera.
        /// </summary>
        public IOCamera2D Camera { get; }

        /// <summary>
        /// Name.
        /// Must be unique.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Has Focus?
        /// </summary>
        public bool HasFocus { get; set; }

        /// <summary>
        /// Is Visible?
        /// Indicates whether the Draw method will be called.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Activate Transitions.
        /// Call to perform any transitions that have been loaded.
        /// </summary>
        public bool ActivateTransitions { get; set; }

        /// <summary>
        /// Is Transitioning?
        /// </summary>
        public bool IsTransitioning { get; protected set; }

        /// <summary>
        /// Activate Sleep.
        /// Call to activate Sleep.
        /// </summary>
        private bool ActivateSleep { get; set; }

        /// <summary>
        /// Is Sleeping?
        /// </summary>
        public bool IsSleeping { get; protected set; }

        /// <summary>
        /// Activate Wake.
        /// Call to activate Wake.
        /// </summary>
        private bool ActivateWake { get; set; }

        /// <summary>
        /// Is Waking?
        /// </summary>
        public bool IsWaking { get; protected set; }

        /// <summary>
        /// Position.
        /// Set to center of the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Rectangle.
        /// Defines the drawable area.
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Origin.
        /// Center of State.
        /// </summary>
        public Vector2 Origin { get; private set; }

        /// <summary>
        /// Rotation Angle.
        /// </summary>
        public double RotationAngle { get; set; }

        /// <summary>
        /// Background Color.
        /// Used for transitions.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Transparency.
        /// Used for transition effects.
        /// </summary>
        public float Transparency { get; set; }

        /// <summary>
        /// Texture.
        /// Used for background color.
        /// </summary>
        public Texture2D BackgroundTexture { private get; set; }

        /// <summary>
        /// Loaded Transitions.
        /// </summary>
        private Dictionary<string, Transition> LoadedTransitions { get; }

        /// <summary>
        /// Transitions.
        /// </summary>
        private List<Transition> ActiveTransitions { get; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// State will be run in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// State Constructor.
        /// </summary>
        /// <param name="name">State's unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">State's Position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">State's width. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">State's height. Intaken as an <see cref="int"/>.</param>
        /// <param name="orderNumber">State's Order number. Intaken as an <see cref="int"/>.</param>
        protected State(string name, Vector2 position, int width, int height, int orderNumber)
        {
            Name = name;
            Position = position;
            Width = width;
            Height = height;
            OrderNumber = orderNumber;

            Camera = new IOCamera2D(ParentStateManager.GraphicsDevice, Width, Height);
            LoadedTransitions = new Dictionary<string, Transition>();
            ActiveTransitions = new List<Transition>();
            
            BackgroundColor = Color.White;
            Transparency = 0.0f;
            HasFocus = false;
            IsVisible = false;
            ActivateTransitions = false;
            IsTransitioning = false;
            ActivateSleep = false;
            IsSleeping = false;
            ActivateWake = false;
            IsWaking = false;
        }

        /// <summary>
        /// Sync Actions.
        /// Can be performed while the State is active.
        /// </summary>
        protected abstract void SyncActions();

        /// <summary>
        /// Async Functions.
        /// Can be performed while the State is active.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        protected abstract Task AsyncFunctions();

        #region Sleep

        /// <summary>
        /// Lull.
        /// Lulls the State to Sleep.
        /// </summary>
        public void Lull()
        {
            ActivateSleep = true;
        }

        /// <summary>
        /// Post Sleep Sync Actions.
        /// Sync Actions taken after Sleeping the State.
        /// </summary>
        protected abstract void PostSleepSyncActions();

        /// <summary>
        /// Post Sleep Async Functions.
        /// Async Functoins taken after Sleeping the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        protected abstract Task PostSleepAsyncFunctions();

        /// <summary>
        /// Sleep.
        /// Called to sleep the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        public virtual async Task<bool> Sleep()
        {
            if (!IsSleeping)
            {
                LoadTransition("Sleep", new FadeOut(this, 1f, 0f, 1f, 0f, 1));
                ActivateLoadedTransition("Sleep");
                IsSleeping = true;
            }

            var result = await RunActiveTransitions();

            if (result)
            {
                ActivateSleep = IsSleeping = false;
            }

            return result;
        }

        #endregion

        #region Wake

        /// <summary>
        /// Awaken.
        /// Wakes the State from Sleep.
        /// </summary>
        public void Awaken()
        {
            IsVisible = true;
            ActivateWake = true;
        }

        /// <summary>
        /// Pre Wake Sync Actions.
        /// Sync Actions taken prior to Waking the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        protected abstract void PreWakeSyncActions();

        /// <summary>
        /// Pre Wake Async Functions.
        /// Async Functions taken prior to Waking the State.
        /// </summary>
        /// <returns></returns>
        protected abstract Task PreWakeAsyncFunctions();

        /// <summary>
        /// Wake.
        /// Called to waken the State.
        /// </summary>
        /// <returns>Returns a Threading Task.</returns>
        public virtual async Task<bool> Wake()
        {
            if (!IsWaking)
            {
                LoadTransition("Wake", new FadeIn(this, 0f, 1f, 1f, 0f, 1));
                ActivateLoadedTransition("Wake");
                IsWaking = true;
            }

            var result = await RunActiveTransitions();

            if (result)
            {
                ActivateWake = IsWaking = false;
            }

            return result;
        }

        #endregion
        
        /// <summary>
        /// Load Content Method.
        /// </summary>
        public virtual void LoadContent()
        {
            
        }

        /// <summary>
        /// State Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public virtual async Task Update(GameTime gameTime)
        {
            if (ActivateTransitions)
            {
                await RunActiveTransitions();
            }
            else if (ActivateWake)
            {
                await PreWakeAsyncFunctions();
                PreWakeSyncActions();

                await Wake();
            }
            else if (ActivateSleep)
            {
                if (await Sleep())
                {
                    await PostSleepAsyncFunctions();
                    PostSleepSyncActions();
                }
            }
            else
            {
                await AsyncFunctions();
                SyncActions();
            }

            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            Origin = new Vector2(BackgroundTexture.Width / 2f, BackgroundTexture.Height / 2f);
            Rectangle = new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height);

            Camera.Update(gameTime);
        }

        /// <summary>
        /// State Draw Method.
        /// Call SpriteBatch.Begin() and SpriteBatch.End().
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        /// <remarks>Using Vector2(Width, Height) as Scale for drawing Background Texture due to Background Texture being a 1x1 pixel. Origin is Center of Background Texture.</remarks>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTexture, Position, null, BackgroundColor * Transparency, (float)RotationAngle, Origin, new Vector2(Width, Height), SpriteEffects.None, 1f);
        }
    }
}