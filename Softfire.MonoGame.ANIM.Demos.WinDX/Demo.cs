using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.ANIM.Demos.WinDX.Animations.Ships;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.ANIM.Demos.WinDX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Demo : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private AnimationManager AnimationManager { get; set; }

        private IOManager Input { get; set; }

        public Demo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            AnimationManager = new AnimationManager(graphics.GraphicsDevice, Content);
            Input = new IOManager();
            Input.SetInputInUse(InputsInUse.KeyboardAndMouse);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var ship = new Ship(null, 1, "Cutler", @"Sprites\Ships\SS_Cutler", new Vector2(128), 64, 64);
            AnimationManager.LoadAnimation(ship);
            ship.AddAction("Idle", new Vector2(0, 0), 64, 64,8, .06f);
            ship.AddAction("Up", new Vector2(0, 64), 64, 64, 8, .06f);
            ship.Movement.SetBounds(new RectangleF(0, 0, 640, 700));

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);

            var ship = AnimationManager.GetAnimation<Ship>(1);

            if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.WKey) == InputActionStateFlags.Idle &&
                ship.Events.InputStates.GetState(InputKeyboardLetterFlags.AKey) == InputActionStateFlags.Idle &&
                ship.Events.InputStates.GetState(InputKeyboardLetterFlags.SKey) == InputActionStateFlags.Idle &&
                ship.Events.InputStates.GetState(InputKeyboardLetterFlags.DKey) == InputActionStateFlags.Idle)
            {
                if (!ship.GetAction("Idle").IsActive)
                {
                    ship.StopAllActions();
                    ship.StartAction("Idle");
                }

                if (ship.Movement.MovementType == Movement.MovementTypes.Velocity)
                {
                    ship.Movement.Stabilize(1d, 1d, 0d);
                }
            }

            if (ship.Movement.MovementType == Movement.MovementTypes.Fixed)
            {
                ship.StopAllActions();
                ship.StartAction("Up");

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.WKey) == InputActionStateFlags.Press)
                {
                    ship.Movement.Move(new Vector2(0, -64));
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.AKey) == InputActionStateFlags.Press)
                {
                    ship.Movement.Move(new Vector2(-64, 0));
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.SKey) == InputActionStateFlags.Press)
                {
                    ship.Movement.Move(new Vector2(0, 64));
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.DKey) == InputActionStateFlags.Press)
                {
                    ship.Movement.Move(new Vector2(64, 0));
                }
            }

            if (ship.Movement.MovementType == Movement.MovementTypes.Velocity)
            {
                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.WKey) == InputActionStateFlags.Held)
                {
                    ship.StopAllActions();
                    ship.StartAction("Up");
                    ship.Movement.Accelerate(1d);
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.AKey) == InputActionStateFlags.Held)
                {
                    ship.Movement.RotateCounterClockwise(1d);
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.SKey) == InputActionStateFlags.Held)
                {
                    ship.Movement.Decelerate(2d);
                }

                if (ship.Events.InputStates.GetState(InputKeyboardLetterFlags.DKey) == InputActionStateFlags.Held)
                {
                    ship.Movement.RotateClockwise(1d);
                }

                ship.Movement.CalculateVelocity();
                ship.Movement.ApplyVelocity();
            }

            AnimationManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            AnimationManager.Draw();

            base.Draw(gameTime);
        }
    }
}
