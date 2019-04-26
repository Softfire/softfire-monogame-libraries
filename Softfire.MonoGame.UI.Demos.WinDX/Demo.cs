using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.IO;
using Softfire.MonoGame.UI.Effects.Coloring;
using Softfire.MonoGame.UI.Effects.Scaling;
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

        #region UI Labels

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

        #endregion

        // TODO: Step 1.1: Add a UIManager.
        private UIManager UIManager { get; set; }

        // TODO: Step 1.2: Add the IOManager
        private IOManager InputManager { get; } = new IOManager();

        public Demo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
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

            // TODO: Step 3: Enable the mouse and keyboard to be usable and the cursor to be visible.
            InputManager.SetInputInUse(InputsInUse.KeyboardAndMouse);
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
            UIManager.Fonts.LoadFont("DemoFontSize16", @"Fonts\FrizQuadrata\Size16");

            // TODO: Step 5.1: Groups.
            // This is used to package any related UI into a single group.
            UIManager.AddGroup("Demo");

            // TODO: Step 5.2: Windows.
            // A window contains many types of items.
            UIManager.GetGroup("Demo").AddWindow("WindowOne", new Vector2(-180, 50), 300, 300);
            UIManager.GetGroup("Demo").AddWindow("WindowTwo", new Vector2(180, 50), 300, 300);

            // TODO: Step 5.2.1: Enabling Windows to move.
            UIManager.GetGroup("Demo").GetWindow("WindowOne").IsMovable = true;
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").IsMovable = true;

            // TODO: Step 5.3: Buttons.
            // A button can perform action and contains text.
            UIManager.GetGroup("Demo").GetWindow("WindowOne").AddButton("ButtonOne");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").Transform.Position = new Vector2(0, -30);
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Text One");

            UIManager.GetGroup("Demo").GetWindow("WindowOne").AddButton("ButtonTwo");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonTwo").Transform.Position = new Vector2(0, 30);
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonTwo").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Text Two");

            UIManager.GetGroup("Demo").GetWindow("WindowOne").AddButton("ButtonThree");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonThree").Transform.Position = new Vector2(100, -60);
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonThree").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Toggle");

            UIManager.GetGroup("Demo").GetWindow("WindowTwo").AddButton("Done");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").Transform.Position = new Vector2(-80, 110);
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Done");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").AddButton("Quit");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Quit").Transform.Position = new Vector2(80, 110);
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Quit").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Quit");

            // TODO: Step 5.4: Text.
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").AddText("Text", UIManager.Fonts.GetFont("DemoFontSize16"), "Text");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetText("Text").Transform.Position = new Vector2(0, -110);

            // TODO: Step 5.5: UI Effects.
            // The base UI class contains a UI Effects Manager for adding and using effects on the UI element.

            // TODO: Step 5.5.1: Are Effects Run In Sequential Order?
            // Effects are loaded into an ordered List and are run in sequence unless the "AreEffectsRunInSequentialOrder" property is set to false.
            // Set delays for each effect so they don't cancel each other out if they're opposing actions.
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.AreEffectsRunInSequentialOrder = true;
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").EffectsManager.AreEffectsRunInSequentialOrder = true;

            // TODO: Step 5.5.2: Load an Effect.
            // Numerous effects are available to customize the actions of a UI element.
            var buttonOne = UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne");
            var buttonDone = UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done");

            #region Shifting

            // Shift Up - Shifts the UI element up over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectShiftUp(buttonOne, 3, "Demo_Shift_Up", new Vector2(50), startDelayInSeconds: .5f));

            // Shift Down - Shifts the UI element down over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectShiftDown(buttonOne, 4, "Demo_Shift_Down", new Vector2(50), startDelayInSeconds: .75f));

            // Shift Left - Shifts the UI element left over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectShiftLeft(buttonOne, 5, "Demo_Shift_Left", new Vector2(50), startDelayInSeconds: 1f));

            // Shift Right - Shifts the UI element right over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectShiftRight(buttonOne, 6, "Demo_Shift_Right", new Vector2(50), startDelayInSeconds: 1.25f));

            #endregion

            #region Background Color

            // Background Color Change - Changes the UI element's background color over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectBackgroundColorGradient(buttonOne, 7, "Demo_Background_Color_Change_ForestGreen", Color.ForestGreen, startDelayInSeconds: .5f));

            // Background Color Change - Changes the UI element's background color over time.
            buttonOne.EffectsManager.LoadEffect(new UIEffectBackgroundColorGradient(buttonOne, 8, "Demo_Background_Color_Change_White", Color.White, startDelayInSeconds: 2f));

            #endregion

            #region Font Color

            // Font Color Change - Changes the UI element's font color over time.
            buttonOne.GetText("Text").EffectsManager.LoadEffect(new UIEffectFontColorGradient(buttonOne.GetText("Text"), 9, "Demo_Font_Color_Change_SlateBlue", Color.SlateBlue, startDelayInSeconds: 1f));

            // Font Color Change - Changes the UI element's font color over time.
            buttonOne.GetText("Text").EffectsManager.LoadEffect(new UIEffectFontColorGradient(buttonOne.GetText("Text"), 10, "Demo_Font_Color_Change_Black", Color.Black, startDelayInSeconds: 2f));

            #endregion

            #region Scaling

            // Scale Out - Scales the UI element out over time.
            buttonDone.EffectsManager.LoadEffect(new UIEffectScaleOut(buttonDone, 1, "Demo_Scale_Out", new Vector2(2), startDelayInSeconds: .5f));

            // Scale In - Scales the UI element in over time.
            buttonDone.EffectsManager.LoadEffect(new UIEffectScaleIn(buttonDone, 2, "Demo_Scale_In", new Vector2(1), startDelayInSeconds: 1.5f));

            #endregion

            // TODO: Step 5.5.3: Program Effects.
            // Programming an effect by adding it to an internal List that is processed during the elements Update method.
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Up");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Right");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Down");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Down");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Left");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Left");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Up");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Up");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Right");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Shift_Down");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Background_Color_Change_ForestGreen");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ProgramEffect("Demo_Background_Color_Change_White");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").GetText("Text").EffectsManager.ProgramEffect("Demo_Font_Color_Change_SlateBlue");
            UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").GetText("Text").EffectsManager.ProgramEffect("Demo_Font_Color_Change_Black");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").EffectsManager.ProgramEffect("Demo_Scale_Out");
            UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").EffectsManager.ProgramEffect("Demo_Scale_In");

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
            // Update the input / output devices.
            InputManager.Update(gameTime);
            
            // TODO: Step 6.1: Button Focus Actions.
            // Is the button is hovered?
            if (UIManager.GetGroup("Demo").GetWindow("WindowOne").IsStateSet(FocusStates.IsFocused) &&
                UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").IsStateSet(FocusStates.IsHovered))
            {
                // TODO: Step 6.1.1: Check if effects are currently running otherwise effects will be repeatedly added.
                // Are effects currently running?
                if (!UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.AreEffectsRunning)
                {
                    // Activate any loaded effects.
                    // TODO: Step 6.1.2: Activate Programmed Effects.
                    UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.ActivateProgrammedEffects();
                    UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").GetText("Text").EffectsManager.ActivateProgrammedEffects();
                }
            }

            if (UIManager.GetGroup("Demo").GetWindow("WindowTwo").IsStateSet(FocusStates.IsFocused) &&
                UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").IsStateSet(FocusStates.IsHovered))
            {
                if (!UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").EffectsManager.AreEffectsRunning)
                {
                    UIManager.GetGroup("Demo").GetWindow("WindowTwo").GetButton("Done").EffectsManager.ActivateProgrammedEffects();
                }
            }

            if (UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonThree").Events.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press)
            {
                UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.AreEffectsRunInSequentialOrder =
                    !UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.AreEffectsRunInSequentialOrder;
            }

            // TODO: Step 7: Update UIManager.
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
            base.Draw(gameTime);

            // TODO: Step 8: Draw.
            spriteBatch.Begin();

            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFontSize16"), "Type: " + ObjectType , new Vector2(), Color.Black);
            spriteBatch.DrawString(UIManager.Fonts.GetFont("DemoFontSize16"), "Name: " + ObjectName, new Vector2(0, 20), Color.Black);

            if (UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.CurrentEffect != null)
            {
                var currentEffect = UIManager.GetGroup("Demo").GetWindow("WindowOne").GetButton("ButtonOne").EffectsManager.CurrentEffect;

                ObjectType = currentEffect.GetType().Name;
                ObjectName = currentEffect.Name;
            }
            else
            {
                ObjectType = string.Empty;
                ObjectName = string.Empty;
            }

            spriteBatch.End();

            // Draw all UI elements.
            UIManager.Draw();
        }
    }
}