﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.ANIM.Demos.WinDX.Animations.Ships;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            AnimationManager.LoadAnimation(new Ship(null, 1, "Cutler", @"Sprites\Ships\SS_Cutler", new Vector2(100)));
            AnimationManager.GetAnimation<Ship>(1).AddAction("Idle", new Vector2(0, 0), 32, 32,8, 20);
            AnimationManager.GetAnimation<Ship>(1).AddAction("Up", new Vector2(0, 32), 32, 32, 8, 20);
            AnimationManager.GetAnimation<Ship>(1).StartAction("Idle");

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
