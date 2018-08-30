using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.UI.Effects.Coloring;
using Softfire.MonoGame.UI.Effects.Shifting;

namespace Softfire.MonoGame.UI.Demos.WinDX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Demo : Game
    {
        #region Default Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #endregion

        /// <summary>
        /// Current object's type.
        /// </summary>
        private string ObjectType { get; set; }

        /// <summary>
        /// Current object's name.
        /// </summary>
        private string ObjectName { get; set; }

        /// <summary>
        /// Current object's position.
        /// </summary>
        private string ObjectPosition { get; set; }

        /// <summary>
        /// Current object's duration.
        /// </summary>
        private string ObjectDuration { get; set; }

        /// <summary>
        /// Current object's rate of change.
        /// </summary>
        private string ObjectRateOfChange { get; set; }

        // TODO: Step 1: Add a UIManager.
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
            // TODO: Step 2: Initialise a new UIManager. Pass in a GraphicsDevice and MonoGame ContentManager.
            UIManager = new UIManager(graphics.GraphicsDevice, Content);

            // TODO: Step 3: Enable the mouse cursor to be visible.
            IsMouseVisible = true;

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

            // TODO: Step 4: Load a Font.
            UIManager.Fonts.LoadFont("DemoFont12", @"Fonts\UncialAntiqua12");

            // TODO: Step 5: Add Content.

            // TODO: Step 5.1: Groups.
            // This is used to package any related UI into a single group.
            UIManager.AddGroup("Demo");

            // TODO: Step 5.2: Menus.
            // A menu contains columns.
            UIManager.GetGroup("Demo").AddMenu("Menu");

            // TODO: Step 5.3: Columns.
            // A column contains buttons.
            UIManager.GetGroup("Demo").GetMenu("Menu").AddColumn("Column");

            // TODO: Step 5.4: Buttons.
            // A button can perform action and contains text.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").AddButton("Button");

            // TODO: Step 5.4.1: Text.
            // A button can display text.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").AddText(UIManager.Fonts.GetFont("DemoFont12"), "Text");

            // TODO: Step 5.5: UI Effects.
            // The base UI class contains an UI Effects Manager for adding and using effects on the UI element.

            // TODO: Step 5.5.1: Are Effects Run In Sequential Order?
            // Effects are loaded into an ordered List and are run in sequence unless the "AreEffectsRunInSequentialOrder" property is set to false.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.AreEffectsRunInSequentialOrder = true;

            // TODO: Step 5.5.2: Load an Effect.
            // Numerous effects are available to customize the actions of a UI element.
            // Loading an effect stores it in the UI's internal Dictionary with the identifier as its key for reuse.

            #region Shifting

            // Shift Up - Shifts the UI element up over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectShiftUp(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 1, "Demo_Shift_Up",
                                                     new Vector2(50), .25f, .25f));

            // Shift Down - Shifts the UI element down over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectShiftDown(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 2, "Demo_Shift_Down",
                                                       new Vector2(50), .25f, .25f));

            // Shift Left - Shifts the UI element left over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectShiftLeft(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 3, "Demo_Shift_Left",
                                                       new Vector2(50), .25f, .25f));

            // Shift Right - Shifts the UI element right over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectShiftRight(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 4, "Demo_Shift_Right",
                                                        new Vector2(50), .25f, .25f));

            #endregion

            #region Background Color

            // Background Color Change - Changes the UI element's background color over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectBackgroundColorGradient(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 5, "Demo_Background_Color_Change_ForestGreen",
                                                                     Color.ForestGreen));

            // Background Color Change - Changes the UI element's background color over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager
                     .LoadEffect(new
                                     UIEffectBackgroundColorGradient(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button"), 6, "Demo_Background_Color_Change_White",
                                                                     Color.White));

            #endregion

            #region Font Color

            // Font Color Change - Changes the UI element's font color over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText().UIEffectsManager
                     .LoadEffect(new
                                     UIEffectFontColorGradient(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText(), 7, "Demo_Font_Color_Change_SlateBlue",
                                                               Color.SlateBlue, startDelayInSeconds: 2f));

            // Font Color Change - Changes the UI element's font color over time.
            UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText().UIEffectsManager
                     .LoadEffect(new
                                     UIEffectFontColorGradient(UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText(), 8, "Demo_Font_Color_Change_Black",
                                                               Color.Black, startDelayInSeconds: 2f));

            #endregion

            // TODO: Step 5.5.3: Activate an Effect.
            // Activating an effect by adding it to an internal List that is processed during the elements Update method.
            // See the Update method for this step.
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
        protected override async void Update(GameTime gameTime)
        {
            // TODO: Step 6: Update UIManager.
            // TODO: Step 6.1: UIManager runs Async. Use "async" in the method definition and "await" for UIManager.Update(gameTime).
            await UIManager.Update(gameTime);

            // TODO: Step 7: Configuration.
            var button = UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button");

            // TODO: Step 7.1: Mouse Handling - Button Focus.
            // CheckInFocus takes in the Mouse's Rectangle and determines if it intersects or is contained inside the Rectangle of the ui element.
            button.CheckIsInFocus(new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1));

            // TODO: Step 7.2: Button Focus Actions.
            // Is the button in focus?
            if (button.IsInFocus)
            {
                // TODO: Step 7.3: Check if effects are currently running otherwise effects will be repeatedly added.
                // Are effects currently running?
                if (button.UIEffectsManager.AreEffectsRunning == false)
                {
                    // Activate any loaded effects.
                    // TODO: Step 7.4: Activate Loaded Effects.
                    // Shifting
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Right");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Down");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Left");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Left");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Up");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Shift_Right");

                    // Colors
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Background_Color_Change_ForestGreen");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.ActivateLoadedEffect("Demo_Background_Color_Change_White");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText().UIEffectsManager.ActivateLoadedEffect("Demo_Font_Color_Change_SlateBlue");
                    UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").GetText().UIEffectsManager.ActivateLoadedEffect("Demo_Font_Color_Change_Black");
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            // TODO: Step 8: Draw.
            spriteBatch.Begin();

            // Draw all UI elements.
            UIManager.Draw(spriteBatch);

            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFont12"), "Type: " + ObjectType , new Vector2(), Color.Black);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFont12"), "Name: " + ObjectName, new Vector2(0, 20), Color.Black);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFont12"), "Duration: " + ObjectDuration + "ms" , new Vector2(0, 40), Color.Black);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFont12"), "Rate of Change: " + ObjectRateOfChange , new Vector2(0, 60), Color.Black);

            if (UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button").UIEffectsManager.CurrentEffect != null)
            {
                ObjectType = UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button")
                                      .UIEffectsManager.CurrentEffect.GetType().Name;

                ObjectName = UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button")
                                      .UIEffectsManager.CurrentEffect.Name;

                ObjectDuration = UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button")
                                          .UIEffectsManager.CurrentEffect.DurationInSeconds.ToString();

                ObjectRateOfChange = UIManager.GetGroup("Demo").GetMenu("Menu").GetColumn("Column").GetButton("Button")
                                              .UIEffectsManager.CurrentEffect.RateOfChange.ToString();
            }
            else
            {
                ObjectType = string.Empty;
                ObjectName = string.Empty;
                ObjectDuration = "0";
                ObjectRateOfChange = "0";
            }

            spriteBatch.End();
        }
    }
}