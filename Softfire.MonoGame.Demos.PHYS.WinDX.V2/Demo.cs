using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.CORE.V2.Physics.Easings;
using Softfire.MonoGame.UI.V2;
using Softfire.MonoGame.UI.V2.Items;

namespace Softfire.MonoGame.Demos.PHYS.WinDX.V2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Demo : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private UIManager UIManager { get; set; }

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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 549;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UIManager = new UIManager(graphics.GraphicsDevice, Content);
            UIManager.Fonts.LoadFont("Demo14", @"Fonts\Size14");
            var fontDemo10 = UIManager.Fonts.LoadFont("Demo10", @"Fonts\Size10");

            UIManager.AddGroup("Easings");
            var easingsGroup = UIManager.GetGroup("Easings");

            CreateEasing(easingsGroup, "Back", fontDemo10, new Vector2(-400, -150),
                         EasingEnums.Easings.Back, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Bounce", fontDemo10, new Vector2(-400, 0),
                         EasingEnums.Easings.Bounce, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Circ", fontDemo10, new Vector2(-400, 150),
                         EasingEnums.Easings.Circular, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Cubic", fontDemo10, new Vector2(-120, -150),
                         EasingEnums.Easings.Cubic, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Elastic", fontDemo10, new Vector2(-120, 0),
                         EasingEnums.Easings.Elastic, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Expo", fontDemo10, new Vector2(-120, 150),
                         EasingEnums.Easings.Exponential, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Linear", fontDemo10, new Vector2(160, -150),
                         EasingEnums.Easings.Linear, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Quad", fontDemo10, new Vector2(160, 0),
                         EasingEnums.Easings.Quadratic, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Quart", fontDemo10, new Vector2(160, 150),
                         EasingEnums.Easings.Quartic, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Quint", fontDemo10, new Vector2(440, -150),
                         EasingEnums.Easings.Quintic, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
            CreateEasing(easingsGroup, "Sine", fontDemo10, new Vector2(440, 0),
                         EasingEnums.Easings.Sine, EasingEnums.EasingOptions.In, EasingEnums.EasingXAxisDirections.Right,
                         EasingEnums.Easings.None, EasingEnums.EasingOptions.None, EasingEnums.EasingYAxisDirections.None);
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
        /// Creates a new easing to demonstrate in the provided <see cref="UIGroup"/>.
        /// </summary>
        /// <param name="group">The <see cref="UIGroup"/> to add the new window that will display the easing. Intaken as a <see cref="UIGroup"/>.</param>
        /// <param name="windowName">The unique name for the <see cref="UIWindow"/> that will house the easing demonstrations. Intaken as a <see cref="UIWindow"/>.</param>
        /// <param name="font">The <see cref="SpriteFont"/> used to display the easing titles. Intaken as a <see cref="SpriteFont"/>.</param>
        /// <param name="position">The position for where the <see cref="UIWindow"/> will display the easings. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="easingXAxis">The easing to demonstrate on the object's X axis. Intaken as an <see cref="EasingEnums.Easings"/>.</param>
        /// <param name="easingXAxisOption">The easing option to demonstrate on teh object's X axis. Intaken as a <see cref="EasingEnums.EasingOptions"/>.</param>
        /// <param name="easingXAxisDirection">The <see cref="EasingEnums.EasingXAxisDirections"/> to define the direction the easing will travel along the X axis. Intaken as a <see cref="EasingEnums.EasingXAxisDirections"/>.</param>
        /// <param name="easingYAxis">The easing to demonstrate on the object's Y axis. Intaken as an <see cref="EasingEnums.Easings"/>.</param>
        /// <param name="easingYAxisOption">The easing option to demonstrate on teh object's Y axis. Intaken as a <see cref="EasingEnums.EasingOptions"/>.</param>
        /// <param name="easingYAxisDirection">The <see cref="EasingEnums.EasingXAxisDirections"/> to define the direction the easing will travel along the Y axis. Intaken as a <see cref="EasingEnums.EasingXAxisDirections"/>.</param>
        private void CreateEasing(UIGroup group, string windowName, SpriteFont font, Vector2 position,
                                  EasingEnums.Easings easingXAxis, EasingEnums.EasingOptions easingXAxisOption, EasingEnums.EasingXAxisDirections easingXAxisDirection,
                                  EasingEnums.Easings easingYAxis, EasingEnums.EasingOptions easingYAxisOption, EasingEnums.EasingYAxisDirections easingYAxisDirection)
        {
            group.AddWindow(windowName, position, 250, 120, 0, 0, 0);

            var window = group.GetWindow(windowName);
            window.AddText("InTitle", font, "In", new Vector2(-110, -40));
            window.AddText("OutTitle", font, "Out", new Vector2(-104, -12));
            window.AddText("InOutTitle", font, "In/Out", new Vector2(-95, 14));
            window.AddText("OutInTitle", font, "Out/In", new Vector2(-95, 40));

            var demoObject1 = new DemoObject(null, 1, "In", "Point", new Vector2(-60, -42));
            demoObject1.Movement.AddEasing(new Easing(demoObject1, 1, "In")
            {
                CurrentXAxisEasing = easingXAxis,
                CurrentXAxisEasingOption = EasingEnums.EasingOptions.In,
                CurrentXAxisDirection = easingXAxisDirection,
                CurrentYAxisEasing = easingYAxis,
                CurrentYAxisEasingOption = easingYAxisOption,
                CurrentYAxisDirection = easingYAxisDirection,
                IsLooping = true,
                WillReturnInReverse = true
            });
            demoObject1.LoadContent(Content);

            window.AddContent(demoObject1);

            var demoObject2 = new DemoObject(null, 2, "Out", "Point", new Vector2(-60, -14));
            demoObject2.Movement.AddEasing(new Easing(demoObject2, 1, "Out")
            {
                CurrentXAxisEasing = easingXAxis,
                CurrentXAxisEasingOption = EasingEnums.EasingOptions.Out,
                CurrentXAxisDirection = easingXAxisDirection,
                CurrentYAxisEasing = easingYAxis,
                CurrentYAxisEasingOption = easingYAxisOption,
                CurrentYAxisDirection = easingYAxisDirection,
                IsLooping = true,
                WillReturnInReverse = true
            });
            demoObject2.LoadContent(Content);

            window.AddContent(demoObject2);

            var demoObject3 = new DemoObject(null, 3, "InOut", "Point", new Vector2(-50, 12));
            demoObject3.Movement.AddEasing(new Easing(demoObject3, 1, "InOut")
            {
                CurrentXAxisEasing = easingXAxis,
                CurrentXAxisEasingOption = EasingEnums.EasingOptions.InOut,
                CurrentXAxisDirection = easingXAxisDirection,
                CurrentYAxisEasing = easingYAxis,
                CurrentYAxisEasingOption = easingYAxisOption,
                CurrentYAxisDirection = easingYAxisDirection,
                IsLooping = true,
                WillReturnInReverse = true
            });
            demoObject3.LoadContent(Content);

            window.AddContent(demoObject3);

            var demoObject4 = new DemoObject(null, 4, "OutIn", "Point", new Vector2(-50, 38));
            demoObject4.Movement.AddEasing(new Easing(demoObject4, 1, "OutIn")
            {
                CurrentXAxisEasing = easingXAxis,
                CurrentXAxisEasingOption = EasingEnums.EasingOptions.OutIn,
                CurrentXAxisDirection = easingXAxisDirection,
                CurrentYAxisEasing = easingYAxis,
                CurrentYAxisEasingOption = easingYAxisOption,
                CurrentYAxisDirection = easingYAxisDirection,
                IsLooping = true,
                WillReturnInReverse = true
            });
            demoObject4.LoadContent(Content);

            window.AddContent(demoObject4);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            UIManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Easing Demonstrations", new Vector2(550, 10), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Back", new Vector2(220, 40), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Bounce", new Vector2(220, 190), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Circ", new Vector2(220, 340), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Cubic", new Vector2(490, 40), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Elastic", new Vector2(490, 190), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Expo", new Vector2(490, 340), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Linear", new Vector2(770, 40), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Quad", new Vector2(770, 190), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Quart", new Vector2(770, 340), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Quint", new Vector2(1050, 40), Color.White);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("Demo14"), "Sine", new Vector2(1050, 190), Color.White);
            UIManager.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}