using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.SM
{
    public class StateManager
    {
        /// <summary>
        /// Current Game.
        /// </summary>
        private Game Game { get; }

        /// <summary>
        /// State Manager Sprite Batch.
        /// </summary>
        private SpriteBatch StateBatch { get; }

        /// <summary>
        /// State Maanger Content Manager.
        /// </summary>
        private ContentManager ParentContentManager { get; }

        /// <summary>
        /// State Manager Active States.
        /// </summary>
        private Dictionary<string, State> ActiveStates { get; }
        
        /// <summary>
        /// Background Texture.
        /// Used to color State backgrounds.
        /// </summary>
        private Texture2D BackgroundTexture { get; }

        /// <summary>
        /// State Manager Constructor.
        /// </summary>
        /// <param name="game">The game instance the State Manager will manage. Intaken as Game.</param>
        /// <param name="graphicsDevice">Intakes a GraphicsDevice.</param>
        /// <param name="parentContentManager">Intakes the parent's ContentManager.</param>
        public StateManager(Game game, GraphicsDevice graphicsDevice, ContentManager parentContentManager)
        {
            Game = game;
            StateBatch = new SpriteBatch(graphicsDevice);
            ParentContentManager = parentContentManager;
            ActiveStates = new Dictionary<string, State>();

            BackgroundTexture = new Texture2D(StateBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            BackgroundTexture.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Add Available State.
        /// Add a new state that can be used by the StateManager.
        /// </summary>
        /// <param name="identifier">The state's unique identifier. Intakan as a string.</param>
        /// <param name="state">The state to be made available. Intakes a new State.</param>
        /// <returns>Returns a bool indicating whether the state was added.</returns>
        public bool AddState(string identifier, State state)
        {
            var result = false;

            if (ActiveStates.ContainsKey(identifier) == false)
            {
                state.ParentStateManager = this;
                state.Content = new ContentManager(ParentContentManager.ServiceProvider, "Content");
                state.BackgroundTexture = BackgroundTexture;

                ActiveStates.Add(identifier, state);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Avaiable State.
        /// </summary>
        /// <param name="identifier">The state's unique identifier. Intakan as a string.</param>
        /// <returns>Returns the requested State otherwise null.</returns>
        public T GetState<T>(string identifier) where T : State
        {
            T state = null;

            if (ActiveStates.ContainsKey(identifier))
            {
                state = (T)ActiveStates[identifier];
            }

            return state;
        }

        /// <summary>
        /// Remove Available State.
        /// </summary>
        /// <param name="identifier">The state's unique identifier. Intakan as a string.</param>
        public void RemoveState(string identifier)
        {
            if (ActiveStates.ContainsKey(identifier))
            {
                ActiveStates[identifier].Content.Unload();
                ActiveStates.Remove(identifier);
            }
        }

        /// <summary>
        /// Quit Game.
        /// </summary>
        public void Quit()
        {
            Game.Exit();
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public async Task Update(GameTime gameTime)
        {
            // Maintain DeltaTime for transitions.
            Transition.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update Active States.
            foreach (var state in ActiveStates.OrderBy(st => st.Value.OrderNumber))
            {
                await state.Value.Update(gameTime).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        public void Draw()
        {
            foreach (var state in ActiveStates.OrderBy(st => st.Value.OrderNumber))
            {
                // Draw the State.
                if (state.Value.IsVisible)
                {
                    state.Value.Draw(StateBatch);
                }
            }
        }
    }
}