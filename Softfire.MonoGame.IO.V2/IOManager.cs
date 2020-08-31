using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    /// <summary>
    /// The available inputs and their usage statuses.
    /// </summary>
    [Flags]
    public enum InputsInUse : byte
    {
        /// <summary>
        /// The mouse is in use.
        /// </summary>
        Mouse = 1 << 0,
        /// <summary>
        /// The keyboard is in use.
        /// </summary>
        Keyboard = 1 << 1,
        /// <summary>
        /// One or more gamepads are in use.
        /// </summary>
        Gamepad = 1 << 2,
        /// <summary>
        /// The keyboard and mouse are in use.
        /// </summary>
        KeyboardAndMouse = Mouse | Keyboard,
        /// <summary>
        /// The keyboard, mouse and one or more gamepads are in use.
        /// </summary>
        All = Mouse | Keyboard | Gamepad
    }
    
    /// <summary>
    /// An IO Manager for all your IO needs.
    /// </summary>
    public partial class IOManager
    {
        /// <summary>
        /// The total elapsed time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The mouse. Squeak, squeak!
        /// </summary>
        private IOMouse Mouse { get; }

        /// <summary>
        /// The keyboard.
        /// </summary>
        private IOKeyboard Keyboard { get; }
        
        /// <summary>
        /// Gamepads.
        /// Contains the currently active gamepads.
        /// </summary>
        private List<IOGamepad> Gamepads { get; } = new List<IOGamepad>(GamePad.MaximumGamePadCount);

        /// <summary>
        /// Internal active player value.
        /// </summary>
        private int _activePlayer = 1;

        /// <summary>
        /// Active Player.
        /// </summary>
        public int ActivePlayer
        {
            get => _activePlayer;
            set => _activePlayer = value > 0 && value <= ActiveGamepads ? value : _activePlayer;
        }

        /// <summary>
        /// Active Gamepads.
        /// </summary>
        public int ActiveGamepads => Gamepads.Count;

        /// <summary>
        /// The timeout period between event calls.
        /// </summary>
        public double PeriodicRefreshTimeoutInSeconds = .33d;

        /// <summary>
        /// The physical inputs currently available.
        /// </summary>
        public InputsInUse InputsInUse { get; private set; } = InputsInUse.KeyboardAndMouse;

        #region Event Handling

        /// <summary>
        /// The input movement event handler.
        /// </summary>
        public static event EventHandler<InputEventArgs> InputMovementHandler; 

        /// <summary>
        /// The input press event handler.
        /// </summary>
        public static event EventHandler<InputEventArgs> InputPressHandler;

        /// <summary>
        /// The input release event handler.
        /// </summary>
        public static event EventHandler<InputEventArgs> InputReleaseHandler;

        /// <summary>
        /// The input held event handler.
        /// </summary>
        public static event EventHandler<InputEventArgs> InputHeldHandler;

        /// <summary>
        /// The input scroll event handler.
        /// </summary>
        public static event EventHandler<InputEventArgs> InputScrollHandler;

        #endregion
        
        /// <summary>
        /// Manages input of all connected devices.
        /// Supporting Keyboard, Mouse and Gamepads.
        /// </summary>
        public IOManager()
        {
            Mouse = new IOMouse();
            Keyboard = new IOKeyboard();

            LoadKeyboardInputMappings();
            LoadMouseInputMappings();
            LoadGamepadInput();
        }

        #region Gamepad Assignment

        /// <summary>
        /// Reassign Gamepad.
        /// </summary>
        /// <param name="gamepadToReassign"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        public bool ReassignGamepad(IOGamepad gamepadToReassign, int newId)
        {
            var result = false;

            if (gamepadToReassign != null &&
                newId <= GamePad.MaximumGamePadCount)
            {
                gamepadToReassign.SetId(newId);
                Gamepads.Remove(gamepadToReassign);

                IOGamepad tempGamepad;
                if ((tempGamepad = Gamepads.FirstOrDefault(gamepad => gamepad.Id == newId)) != null)
                {
                    tempGamepad.SetId(gamepadToReassign.Id);
                    Gamepads.Remove(tempGamepad);

                    Gamepads.Add(tempGamepad);
                }

                Gamepads.Add(gamepadToReassign);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Gamepad.
        /// </summary>
        /// <returns>Returns the id of the newly added gamepad as an int.</returns>
        public int AddGamepad()
        {
            var gamepadId = Gamepads.Count == 0 ? 1 : Gamepads.Count;

            if (Gamepads.Count < GamePad.MaximumGamePadCount)
            {
                Gamepads.Add(new IOGamepad(gamepadId));
            }

            return gamepadId;
        }

        /// <summary>
        /// Get Gamepad.
        /// </summary>
        /// <param name="id">The id of the gamepad to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested IOGamepad, if present, otherwise null.</returns>
        public IOGamepad GetGamepad(int id)
        {
            return Gamepads.FirstOrDefault(gamepad => gamepad.Id == id);
        }

        /// <summary>
        /// Remove Gamepad.
        /// </summary>
        /// <param name="id">The id of the gamepad to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the gamepad was removed.</returns>
        public bool RemoveGamepad(int id)
        {
            var result = false;
            IOGamepad gpad;

            if ((gpad = Gamepads.FirstOrDefault(gamepad => gamepad.Id == id)) != null)
            {
                result = Gamepads.Remove(gpad);
            }

            return result;
        }

        #endregion
        
        /// <summary>
        /// Sets the inputs to use.
        /// </summary>
        /// <param name="inputs">The inputs to use. Intaken as a <see cref="InputsInUse"/>.</param>
        public void SetInputInUse(InputsInUse inputs)
        {
            InputsInUse = 0;
            InputsInUse |= inputs;
        }

        /// <summary>
        /// Determines whether there has been any mouse input.
        /// </summary>
        private void MouseInput()
        {
            #region Movement

            // If the mouse is moved then send an event.
            if (Mouse.GetMovementDeltas() != Vector2.Zero ||
                ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                InputMovementHandler?.Invoke(this, new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                });
            }

            #endregion

            #region Clicks

            MouseButtonActionHandler(Mouse.LeftClickPress, InputMouseActionFlags.LeftClick, InputActionStateFlags.Press);
            MouseButtonActionHandler(Mouse.LeftClickRelease, InputMouseActionFlags.LeftClick, InputActionStateFlags.Release);
            MouseButtonActionHandler(Mouse.LeftClickHeld, InputMouseActionFlags.LeftClick, InputActionStateFlags.Held);

            MouseButtonActionHandler(Mouse.MiddleClickPress, InputMouseActionFlags.MiddleClick, InputActionStateFlags.Press);
            MouseButtonActionHandler(Mouse.MiddleClickRelease, InputMouseActionFlags.MiddleClick, InputActionStateFlags.Release);
            MouseButtonActionHandler(Mouse.MiddleClickHeld, InputMouseActionFlags.MiddleClick, InputActionStateFlags.Held);

            MouseButtonActionHandler(Mouse.RightClickPress, InputMouseActionFlags.RightClick, InputActionStateFlags.Press);
            MouseButtonActionHandler(Mouse.RightClickRelease, InputMouseActionFlags.RightClick, InputActionStateFlags.Release);
            MouseButtonActionHandler(Mouse.RightClickHeld, InputMouseActionFlags.RightClick, InputActionStateFlags.Held);

            MouseButtonActionHandler(Mouse.ButtonOneClickPress, InputMouseActionFlags.ButtonOneClick, InputActionStateFlags.Press);
            MouseButtonActionHandler(Mouse.ButtonOneClickRelease, InputMouseActionFlags.ButtonOneClick, InputActionStateFlags.Release);
            MouseButtonActionHandler(Mouse.ButtonOneClickHeld, InputMouseActionFlags.ButtonOneClick, InputActionStateFlags.Held);

            MouseButtonActionHandler(Mouse.ButtonTwoClickPress, InputMouseActionFlags.ButtonTwoClick, InputActionStateFlags.Press);
            MouseButtonActionHandler(Mouse.ButtonTwoClickRelease, InputMouseActionFlags.ButtonTwoClick, InputActionStateFlags.Release);
            MouseButtonActionHandler(Mouse.ButtonTwoClickHeld, InputMouseActionFlags.ButtonTwoClick, InputActionStateFlags.Held);

            #endregion

            #region Scrolling

            if (Mouse.CheckForUpwardScroll())
            {
                var args = new InputEventArgs
                {
                    InputScrollVelocity = new Vector2(0, Mouse.ScrollSpeed),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(InputMouseActionFlags.ScrollUp);

                InputScrollHandler?.Invoke(this, args);
            }

            if (Mouse.CheckForDownwardScroll())
            {
                var args = new InputEventArgs
                {
                    InputScrollVelocity = new Vector2(0, Mouse.ScrollSpeed),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(InputMouseActionFlags.ScrollDown);

                InputScrollHandler?.Invoke(this, args);
            }

            if (Mouse.CheckForLeftwardScroll())
            {
                var args = new InputEventArgs
                {
                    InputScrollVelocity = new Vector2(Mouse.ScrollSpeed, 0),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(InputMouseActionFlags.ScrollLeft);

                InputScrollHandler?.Invoke(this, args);
            }

            if (Mouse.CheckForRightwardScroll())
            {
                var args = new InputEventArgs
                {
                    InputScrollVelocity = new Vector2(Mouse.ScrollSpeed, 0),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(InputMouseActionFlags.ScrollRight);

                InputScrollHandler?.Invoke(this, args);
            }

            #endregion
        }

        /// <summary>
        /// Determines whether there has been any keyboard input.
        /// </summary>
        private void KeyboardInput()
        {
            #region Function Keys

            KeyboardKeyActionHandler(Keyboard.F1KeyPress, InputKeyboardFunctionFlags.F1Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F1KeyRelease, InputKeyboardFunctionFlags.F1Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F1KeyPress, InputKeyboardFunctionFlags.F1Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F2KeyPress, InputKeyboardFunctionFlags.F2Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F2KeyRelease, InputKeyboardFunctionFlags.F2Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F2KeyHeld, InputKeyboardFunctionFlags.F2Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F3KeyPress, InputKeyboardFunctionFlags.F3Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F3KeyRelease, InputKeyboardFunctionFlags.F3Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F3KeyHeld, InputKeyboardFunctionFlags.F3Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F4KeyPress, InputKeyboardFunctionFlags.F4Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F4KeyRelease, InputKeyboardFunctionFlags.F4Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F4KeyHeld, InputKeyboardFunctionFlags.F4Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F5KeyPress, InputKeyboardFunctionFlags.F5Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F5KeyRelease, InputKeyboardFunctionFlags.F5Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F5KeyHeld, InputKeyboardFunctionFlags.F5Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F6KeyPress, InputKeyboardFunctionFlags.F6Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F6KeyRelease, InputKeyboardFunctionFlags.F6Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F6KeyHeld, InputKeyboardFunctionFlags.F6Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F7KeyPress, InputKeyboardFunctionFlags.F7Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F7KeyRelease, InputKeyboardFunctionFlags.F7Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F7KeyHeld, InputKeyboardFunctionFlags.F7Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F8KeyPress, InputKeyboardFunctionFlags.F8Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F8KeyRelease, InputKeyboardFunctionFlags.F8Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F8KeyHeld, InputKeyboardFunctionFlags.F8Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F9KeyPress, InputKeyboardFunctionFlags.F9Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F9KeyRelease, InputKeyboardFunctionFlags.F9Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F9KeyHeld, InputKeyboardFunctionFlags.F9Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F10KeyPress, InputKeyboardFunctionFlags.F10Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F10KeyRelease, InputKeyboardFunctionFlags.F10Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F10KeyHeld, InputKeyboardFunctionFlags.F10Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F11KeyPress, InputKeyboardFunctionFlags.F11Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F11KeyRelease, InputKeyboardFunctionFlags.F11Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F11KeyHeld, InputKeyboardFunctionFlags.F11Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.F12KeyPress, InputKeyboardFunctionFlags.F12Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.F12KeyRelease, InputKeyboardFunctionFlags.F12Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.F12KeyHeld, InputKeyboardFunctionFlags.F12Key, InputActionStateFlags.Held);

            #endregion

            #region NumPad Keys

            KeyboardKeyActionHandler(Keyboard.NumPad0KeyPress, InputKeyboardNumPadFlags.NumPad0Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad0KeyRelease, InputKeyboardNumPadFlags.NumPad0Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad0KeyHeld, InputKeyboardNumPadFlags.NumPad0Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad1KeyPress, InputKeyboardNumPadFlags.NumPad1Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad1KeyRelease, InputKeyboardNumPadFlags.NumPad1Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad1KeyHeld, InputKeyboardNumPadFlags.NumPad1Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad2KeyPress, InputKeyboardNumPadFlags.NumPad2Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad2KeyRelease, InputKeyboardNumPadFlags.NumPad2Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad2KeyHeld, InputKeyboardNumPadFlags.NumPad2Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad3KeyPress, InputKeyboardNumPadFlags.NumPad3Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad3KeyRelease, InputKeyboardNumPadFlags.NumPad3Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad3KeyHeld, InputKeyboardNumPadFlags.NumPad3Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad4KeyPress, InputKeyboardNumPadFlags.NumPad4Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad4KeyRelease, InputKeyboardNumPadFlags.NumPad4Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad4KeyHeld, InputKeyboardNumPadFlags.NumPad4Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad5KeyPress, InputKeyboardNumPadFlags.NumPad5Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad5KeyRelease, InputKeyboardNumPadFlags.NumPad5Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad5KeyHeld, InputKeyboardNumPadFlags.NumPad5Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad6KeyPress, InputKeyboardNumPadFlags.NumPad6Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad6KeyRelease, InputKeyboardNumPadFlags.NumPad6Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad6KeyHeld, InputKeyboardNumPadFlags.NumPad6Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad7KeyPress, InputKeyboardNumPadFlags.NumPad7Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad7KeyRelease, InputKeyboardNumPadFlags.NumPad7Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad7KeyHeld, InputKeyboardNumPadFlags.NumPad7Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad8KeyPress, InputKeyboardNumPadFlags.NumPad8Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad8KeyRelease, InputKeyboardNumPadFlags.NumPad8Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad8KeyHeld, InputKeyboardNumPadFlags.NumPad8Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumPad9KeyPress, InputKeyboardNumPadFlags.NumPad9Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumPad9KeyRelease, InputKeyboardNumPadFlags.NumPad9Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumPad9KeyHeld, InputKeyboardNumPadFlags.NumPad9Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NumLockKeyPress, InputKeyboardNumPadFlags.NumLockKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NumLockKeyRelease, InputKeyboardNumPadFlags.NumLockKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NumLockKeyHeld, InputKeyboardNumPadFlags.NumLockKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.DivideKeyPress, InputKeyboardNumPadFlags.DivideKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.DivideKeyRelease, InputKeyboardNumPadFlags.DivideKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.DivideKeyHeld, InputKeyboardNumPadFlags.DivideKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.MultiplyKeyPress, InputKeyboardNumPadFlags.MultiplyKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.MultiplyKeyRelease, InputKeyboardNumPadFlags.MultiplyKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.MultiplyKeyHeld, InputKeyboardNumPadFlags.MultiplyKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.SubtractKeyPress, InputKeyboardNumPadFlags.SubtractKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.SubtractKeyRelease, InputKeyboardNumPadFlags.SubtractKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.SubtractKeyHeld, InputKeyboardNumPadFlags.SubtractKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.AddKeyPress, InputKeyboardNumPadFlags.AddKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.AddKeyRelease, InputKeyboardNumPadFlags.AddKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.AddKeyHeld, InputKeyboardNumPadFlags.AddKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.DecimalKeyPress, InputKeyboardNumPadFlags.DecimalKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.DecimalKeyRelease, InputKeyboardNumPadFlags.DecimalKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.DecimalKeyHeld, InputKeyboardNumPadFlags.DecimalKey, InputActionStateFlags.Held);

            #endregion

            #region Command Keys

            KeyboardKeyActionHandler(Keyboard.InsertKeyPress, InputKeyboardCommandFlags.InsertKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.InsertKeyRelease, InputKeyboardCommandFlags.InsertKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.InsertKeyHeld, InputKeyboardCommandFlags.InsertKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.HomeKeyPress, InputKeyboardCommandFlags.HomeKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.HomeKeyRelease, InputKeyboardCommandFlags.HomeKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.HomeKeyHeld, InputKeyboardCommandFlags.HomeKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.PageUpKeyPress, InputKeyboardCommandFlags.PageUpKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.PageUpKeyRelease, InputKeyboardCommandFlags.PageUpKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.PageUpKeyHeld, InputKeyboardCommandFlags.PageUpKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.DeleteKeyPress, InputKeyboardCommandFlags.DeleteKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.DeleteKeyRelease, InputKeyboardCommandFlags.DeleteKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.DeleteKeyHeld, InputKeyboardCommandFlags.DeleteKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.EndKeyPress, InputKeyboardCommandFlags.EndKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.EndKeyRelease, InputKeyboardCommandFlags.EndKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.EndKeyHeld, InputKeyboardCommandFlags.EndKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.PageDownKeyPress, InputKeyboardCommandFlags.PageDownKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.PageDownKeyRelease, InputKeyboardCommandFlags.PageDownKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.PageDownKeyHeld, InputKeyboardCommandFlags.PageDownKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.LeftCtrlKeyPress, InputKeyboardCommandFlags.LeftCtrlKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LeftCtrlKeyRelease, InputKeyboardCommandFlags.LeftCtrlKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.LeftCtrlKeyHeld, InputKeyboardCommandFlags.LeftCtrlKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.LeftAltKeyPress, InputKeyboardCommandFlags.LeftAltKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LeftAltKeyRelease, InputKeyboardCommandFlags.LeftAltKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.LeftAltKeyRelease, InputKeyboardCommandFlags.LeftAltKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.LeftShiftKeyPress, InputKeyboardCommandFlags.LeftShiftKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LeftShiftKeyRelease, InputKeyboardCommandFlags.LeftShiftKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.LeftShiftKeyHeld, InputKeyboardCommandFlags.LeftShiftKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.RightCtrlKeyPress, InputKeyboardCommandFlags.RightCtrlKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RightCtrlKeyRelease, InputKeyboardCommandFlags.RightCtrlKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RightCtrlKeyHeld, InputKeyboardCommandFlags.RightCtrlKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.RightAltKeyPress, InputKeyboardCommandFlags.RightAltKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RightAltKeyRelease, InputKeyboardCommandFlags.RightAltKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RightAltKeyRelease, InputKeyboardCommandFlags.RightAltKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.RightShiftKeyPress, InputKeyboardCommandFlags.RightShiftKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RightShiftKeyRelease, InputKeyboardCommandFlags.RightShiftKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RightShiftKeyHeld, InputKeyboardCommandFlags.RightShiftKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.EnterKeyPress, InputKeyboardCommandFlags.EnterKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.EnterKeyRelease, InputKeyboardCommandFlags.EnterKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.EnterKeyHeld, InputKeyboardCommandFlags.EnterKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.SpaceKeyPress, InputKeyboardCommandFlags.SpaceKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.SpaceKeyRelease, InputKeyboardCommandFlags.SpaceKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.SpaceKeyHeld, InputKeyboardCommandFlags.SpaceKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.BackspaceKeyPress, InputKeyboardCommandFlags.BackspaceKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.BackspaceKeyRelease, InputKeyboardCommandFlags.BackspaceKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.BackspaceKeyHeld, InputKeyboardCommandFlags.BackspaceKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.TabKeyPress, InputKeyboardCommandFlags.TabKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.TabKeyRelease, InputKeyboardCommandFlags.TabKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.TabKeyHeld, InputKeyboardCommandFlags.TabKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemCloseBracketsKeyPress, InputKeyboardCommandFlags.OemCloseBracketsKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemCloseBracketsKeyRelease, InputKeyboardCommandFlags.OemCloseBracketsKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemCloseBracketsKeyHeld, InputKeyboardCommandFlags.OemCloseBracketsKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemOpenBracketsKeyPress, InputKeyboardCommandFlags.OemOpenBracketsKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemOpenBracketsKeyRelease, InputKeyboardCommandFlags.OemOpenBracketsKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemOpenBracketsKeyHeld, InputKeyboardCommandFlags.OemOpenBracketsKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemPipeKeyPress, InputKeyboardCommandFlags.OemPipeKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemPipeKeyRelease, InputKeyboardCommandFlags.OemPipeKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemPipeKeyHeld, InputKeyboardCommandFlags.OemPipeKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemSemicolonKeyPress, InputKeyboardCommandFlags.OemSemicolonKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemSemicolonKeyRelease, InputKeyboardCommandFlags.OemSemicolonKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemSemicolonKeyHeld, InputKeyboardCommandFlags.OemSemicolonKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemQuotesKeyPress, InputKeyboardCommandFlags.OemQuotesKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemQuotesKeyRelease, InputKeyboardCommandFlags.OemQuotesKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemQuotesKeyHeld, InputKeyboardCommandFlags.OemQuotesKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemCommaKeyPress, InputKeyboardCommandFlags.OemCommaKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemCommaKeyRelease, InputKeyboardCommandFlags.OemCommaKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemCommaKeyHeld, InputKeyboardCommandFlags.OemCommaKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemPeriodKeyPress, InputKeyboardCommandFlags.OemPeriodKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemPeriodKeyRelease, InputKeyboardCommandFlags.OemPeriodKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemPeriodKeyHeld, InputKeyboardCommandFlags.OemPeriodKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemQuestionKeyPress, InputKeyboardCommandFlags.OemQuestionKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemQuestionKeyRelease, InputKeyboardCommandFlags.OemQuestionKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemQuestionKeyHeld, InputKeyboardCommandFlags.OemQuestionKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemMinusKeyPress, InputKeyboardCommandFlags.OemMinusKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemMinusKeyRelease, InputKeyboardCommandFlags.OemMinusKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemMinusKeyHeld, InputKeyboardCommandFlags.OemMinusKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemPlusKeyPress, InputKeyboardCommandFlags.OemPlusKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemPlusKeyRelease, InputKeyboardCommandFlags.OemPlusKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemPlusKeyHeld, InputKeyboardCommandFlags.OemPlusKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OemTildeKeyPress, InputKeyboardCommandFlags.OemTildeKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OemTildeKeyRelease, InputKeyboardCommandFlags.OemTildeKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OemTildeKeyHeld, InputKeyboardCommandFlags.OemTildeKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.PrintScreenKeyPress, InputKeyboardCommandFlags.PrintScreenKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.PrintScreenKeyRelease, InputKeyboardCommandFlags.PrintScreenKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.PrintScreenKeyHeld, InputKeyboardCommandFlags.PrintScreenKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.PauseKeyPress, InputKeyboardCommandFlags.PauseKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.PauseKeyRelease, InputKeyboardCommandFlags.PauseKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.PauseKeyHeld, InputKeyboardCommandFlags.PauseKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.ScrollLockKeyPress, InputKeyboardCommandFlags.ScrollLockKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.ScrollLockKeyRelease, InputKeyboardCommandFlags.ScrollLockKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.ScrollLockKeyHeld, InputKeyboardCommandFlags.ScrollLockKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.CapsLockKeyPress, InputKeyboardCommandFlags.CapsLockKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.CapsLockKeyRelease, InputKeyboardCommandFlags.CapsLockKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.CapsLockKeyHeld, InputKeyboardCommandFlags.CapsLockKey, InputActionStateFlags.Held);

            #endregion

            #region Special Keys
            
            KeyboardKeyActionHandler(Keyboard.LeftWindowsKeyPress, InputKeyboardSpecialFlags.LeftWindowsKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LeftWindowsKeyRelease, InputKeyboardSpecialFlags.LeftWindowsKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.LeftWindowsKeyHeld, InputKeyboardSpecialFlags.LeftWindowsKey, InputActionStateFlags.Held);
            
            KeyboardKeyActionHandler(Keyboard.RightWindowsKeyPress, InputKeyboardSpecialFlags.RightWindowsKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RightWindowsKeyRelease, InputKeyboardSpecialFlags.RightWindowsKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RightWindowsKeyHeld, InputKeyboardSpecialFlags.RightWindowsKey, InputActionStateFlags.Held);

            #endregion

            #region Arrow Keys
            
            KeyboardKeyActionHandler(Keyboard.UpKeyPress, InputKeyboardArrowFlags.UpKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.UpKeyRelease, InputKeyboardArrowFlags.UpKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.UpKeyHeld, InputKeyboardArrowFlags.UpKey, InputActionStateFlags.Held);
            
            KeyboardKeyActionHandler(Keyboard.DownKeyPress, InputKeyboardArrowFlags.DownKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.DownKeyRelease, InputKeyboardArrowFlags.DownKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.DownKeyHeld, InputKeyboardArrowFlags.DownKey, InputActionStateFlags.Held);
            
            KeyboardKeyActionHandler(Keyboard.LeftKeyPress, InputKeyboardArrowFlags.LeftKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LeftKeyRelease, InputKeyboardArrowFlags.LeftKey, InputActionStateFlags.Release
);
            KeyboardKeyActionHandler(Keyboard.LeftKeyHeld, InputKeyboardArrowFlags.LeftKey, InputActionStateFlags.Held);
            
            KeyboardKeyActionHandler(Keyboard.RightKeyPress, InputKeyboardArrowFlags.RightKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RightKeyRelease, InputKeyboardArrowFlags.RightKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RightKeyHeld, InputKeyboardArrowFlags.RightKey, InputActionStateFlags.Held);

            #endregion

            #region Letter Keys

            KeyboardKeyActionHandler(Keyboard.QKeyPress, InputKeyboardLetterFlags.QKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.QKeyRelease, InputKeyboardLetterFlags.QKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.QKeyHeld, InputKeyboardLetterFlags.QKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.WKeyPress, InputKeyboardLetterFlags.WKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.WKeyRelease, InputKeyboardLetterFlags.WKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.WKeyHeld, InputKeyboardLetterFlags.WKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.EKeyPress, InputKeyboardLetterFlags.EKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.EKeyRelease, InputKeyboardLetterFlags.EKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.EKeyHeld, InputKeyboardLetterFlags.EKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.RKeyPress, InputKeyboardLetterFlags.RKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.RKeyRelease, InputKeyboardLetterFlags.RKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.RKeyHeld, InputKeyboardLetterFlags.RKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.TKeyPress, InputKeyboardLetterFlags.TKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.TKeyRelease, InputKeyboardLetterFlags.TKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.TKeyHeld, InputKeyboardLetterFlags.TKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.YKeyPress, InputKeyboardLetterFlags.YKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.YKeyRelease, InputKeyboardLetterFlags.YKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.YKeyHeld, InputKeyboardLetterFlags.YKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.UKeyPress, InputKeyboardLetterFlags.UKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.UKeyRelease, InputKeyboardLetterFlags.UKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.UKeyHeld, InputKeyboardLetterFlags.UKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.IKeyPress, InputKeyboardLetterFlags.IKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.IKeyRelease, InputKeyboardLetterFlags.IKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.IKeyHeld, InputKeyboardLetterFlags.IKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.OKeyPress, InputKeyboardLetterFlags.OKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.OKeyRelease, InputKeyboardLetterFlags.OKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.OKeyHeld, InputKeyboardLetterFlags.OKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.PKeyPress, InputKeyboardLetterFlags.PKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.PKeyRelease, InputKeyboardLetterFlags.PKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.PKeyHeld, InputKeyboardLetterFlags.PKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.AKeyPress, InputKeyboardLetterFlags.AKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.AKeyRelease, InputKeyboardLetterFlags.AKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.AKeyHeld, InputKeyboardLetterFlags.AKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.SKeyPress, InputKeyboardLetterFlags.SKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.SKeyRelease, InputKeyboardLetterFlags.SKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.SKeyHeld, InputKeyboardLetterFlags.SKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.DKeyPress, InputKeyboardLetterFlags.DKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.DKeyRelease, InputKeyboardLetterFlags.DKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.DKeyHeld, InputKeyboardLetterFlags.DKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.FKeyPress, InputKeyboardLetterFlags.FKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.FKeyRelease, InputKeyboardLetterFlags.FKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.FKeyHeld, InputKeyboardLetterFlags.FKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.GKeyPress, InputKeyboardLetterFlags.GKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.GKeyRelease, InputKeyboardLetterFlags.GKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.GKeyHeld, InputKeyboardLetterFlags.GKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.HKeyPress, InputKeyboardLetterFlags.HKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.HKeyRelease, InputKeyboardLetterFlags.HKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.HKeyHeld, InputKeyboardLetterFlags.HKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.JKeyPress, InputKeyboardLetterFlags.JKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.JKeyRelease, InputKeyboardLetterFlags.JKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.JKeyHeld, InputKeyboardLetterFlags.JKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.KKeyPress, InputKeyboardLetterFlags.KKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.KKeyRelease, InputKeyboardLetterFlags.KKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.KKeyHeld, InputKeyboardLetterFlags.KKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.LKeyPress, InputKeyboardLetterFlags.LKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.LKeyRelease, InputKeyboardLetterFlags.LKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.LKeyHeld, InputKeyboardLetterFlags.LKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.ZKeyPress, InputKeyboardLetterFlags.ZKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.ZKeyRelease, InputKeyboardLetterFlags.ZKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.ZKeyHeld, InputKeyboardLetterFlags.ZKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.XKeyPress, InputKeyboardLetterFlags.XKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.XKeyRelease, InputKeyboardLetterFlags.XKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.XKeyHeld, InputKeyboardLetterFlags.XKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.CKeyPress, InputKeyboardLetterFlags.CKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.CKeyRelease, InputKeyboardLetterFlags.CKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.CKeyHeld, InputKeyboardLetterFlags.CKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.VKeyPress, InputKeyboardLetterFlags.VKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.VKeyRelease, InputKeyboardLetterFlags.VKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.VKeyHeld, InputKeyboardLetterFlags.VKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.BKeyPress, InputKeyboardLetterFlags.BKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.BKeyRelease, InputKeyboardLetterFlags.BKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.BKeyHeld, InputKeyboardLetterFlags.BKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.NKeyPress, InputKeyboardLetterFlags.NKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.NKeyRelease, InputKeyboardLetterFlags.NKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.NKeyHeld, InputKeyboardLetterFlags.NKey, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.MKeyPress, InputKeyboardLetterFlags.MKey, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.MKeyRelease, InputKeyboardLetterFlags.MKey, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.MKeyHeld, InputKeyboardLetterFlags.MKey, InputActionStateFlags.Held);

            #endregion

            #region Number Keys
            
            KeyboardKeyActionHandler(Keyboard.D1KeyPress, InputKeyboardNumberFlags.D1Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D1KeyRelease, InputKeyboardNumberFlags.D1Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D1KeyHeld, InputKeyboardNumberFlags.D1Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D2KeyPress, InputKeyboardNumberFlags.D2Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D2KeyRelease, InputKeyboardNumberFlags.D2Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D2KeyHeld, InputKeyboardNumberFlags.D2Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D3KeyPress, InputKeyboardNumberFlags.D3Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D3KeyRelease, InputKeyboardNumberFlags.D3Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D3KeyHeld, InputKeyboardNumberFlags.D3Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D4KeyPress, InputKeyboardNumberFlags.D4Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D4KeyRelease, InputKeyboardNumberFlags.D4Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D4KeyHeld, InputKeyboardNumberFlags.D4Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D5KeyPress, InputKeyboardNumberFlags.D5Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D5KeyRelease, InputKeyboardNumberFlags.D5Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D5KeyHeld, InputKeyboardNumberFlags.D5Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D6KeyPress, InputKeyboardNumberFlags.D6Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D6KeyRelease, InputKeyboardNumberFlags.D6Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D6KeyHeld, InputKeyboardNumberFlags.D6Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D7KeyPress, InputKeyboardNumberFlags.D7Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D7KeyRelease, InputKeyboardNumberFlags.D7Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D7KeyHeld, InputKeyboardNumberFlags.D7Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D8KeyPress, InputKeyboardNumberFlags.D8Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D8KeyRelease, InputKeyboardNumberFlags.D8Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D8KeyHeld, InputKeyboardNumberFlags.D8Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D9KeyPress, InputKeyboardNumberFlags.D9Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D9KeyRelease, InputKeyboardNumberFlags.D9Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D9KeyHeld, InputKeyboardNumberFlags.D9Key, InputActionStateFlags.Held);

            KeyboardKeyActionHandler(Keyboard.D0KeyPress, InputKeyboardNumberFlags.D0Key, InputActionStateFlags.Press);
            KeyboardKeyActionHandler(Keyboard.D0KeyRelease, InputKeyboardNumberFlags.D0Key, InputActionStateFlags.Release);
            KeyboardKeyActionHandler(Keyboard.D0KeyHeld, InputKeyboardNumberFlags.D0Key, InputActionStateFlags.Held);

            #endregion
        }

        /// <summary>
        /// Determines whether there has been any gamepad input.
        /// </summary>
        private void GamepadInput(IOGamepad gamepad)
        {
            #region Movement

            // If the gamepad's thumbs tick is moved then send an event.
            if (gamepad.GetLeftThumbStickMovementDeltas() != Vector2.Zero ||
                ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                InputMovementHandler?.Invoke(this, new InputEventArgs
                {
                    InputDeltas = gamepad.GetLeftThumbStickMovementDeltas(),
                    InputRectangle = gamepad.GetLeftThumbStickRectangle(),
                    PlayerIndex = gamepad.Id
                });
            }

            // If the gamepad's thumbs tick is moved then send an event.
            if (gamepad.GetLeftThumbStickMovementDeltas() != Vector2.Zero ||
                ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                InputMovementHandler?.Invoke(this, new InputEventArgs
                {
                    InputDeltas = gamepad.GetLeftThumbStickMovementDeltas(),
                    InputRectangle = gamepad.GetLeftThumbStickRectangle(),
                    PlayerIndex = gamepad.Id
                });
            }

            // If the gamepad's thumbs tick is moved then send an event.
            if (gamepad.GetRightThumbStickMovementDeltas() != Vector2.Zero ||
                ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                InputMovementHandler?.Invoke(this, new InputEventArgs
                {
                    InputDeltas = gamepad.GetRightThumbStickMovementDeltas(),
                    InputRectangle = gamepad.GetRightThumbStickRectangle(),
                    PlayerIndex = gamepad.Id
                });
            }

            // If the gamepad's thumbs tick is moved then send an event.
            if (gamepad.GetRightThumbStickMovementDeltas() != Vector2.Zero ||
                ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                InputMovementHandler?.Invoke(this, new InputEventArgs
                {
                    InputDeltas = gamepad.GetRightThumbStickMovementDeltas(),
                    InputRectangle = gamepad.GetRightThumbStickRectangle(),
                    PlayerIndex = gamepad.Id
                });
            }

            #endregion

            #region Buttons
            
            GamepadButtonActionHandler(gamepad.AButtonPress, InputGamepadActionFlags.AButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.AButtonRelease, InputGamepadActionFlags.AButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.AButtonHeld, InputGamepadActionFlags.AButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.BButtonPress, InputGamepadActionFlags.BButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.BButtonRelease, InputGamepadActionFlags.BButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.BButtonHeld, InputGamepadActionFlags.BButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.XButtonPress, InputGamepadActionFlags.XButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.XButtonRelease, InputGamepadActionFlags.XButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.XButtonHeld, InputGamepadActionFlags.XButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.YButtonPress, InputGamepadActionFlags.YButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.YButtonRelease, InputGamepadActionFlags.YButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.YButtonHeld, InputGamepadActionFlags.YButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.ShoulderLeftButtonPress, InputGamepadActionFlags.ShoulderLeftButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.ShoulderLeftButtonRelease, InputGamepadActionFlags.ShoulderLeftButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.ShoulderLeftButtonHeld, InputGamepadActionFlags.ShoulderLeftButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.ShoulderRightButtonPress, InputGamepadActionFlags.ShoulderRightButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.ShoulderRightButtonRelease, InputGamepadActionFlags.ShoulderRightButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.ShoulderRightButtonHeld, InputGamepadActionFlags.ShoulderRightButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.TriggerLeftButtonPress, InputGamepadActionFlags.TriggerLeftButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.TriggerLeftButtonRelease, InputGamepadActionFlags.TriggerLeftButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.TriggerLeftButtonHeld, InputGamepadActionFlags.TriggerLeftButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.TriggerRightButtonPress, InputGamepadActionFlags.TriggerRightButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.TriggerRightButtonRelease, InputGamepadActionFlags.TriggerRightButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.TriggerRightButtonHeld, InputGamepadActionFlags.TriggerRightButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.DPadUpPress, InputGamepadActionFlags.DPadUpButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.DPadUpRelease, InputGamepadActionFlags.DPadUpButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.DPadUpHeld, InputGamepadActionFlags.DPadUpButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.DPadDownPress, InputGamepadActionFlags.DPadDownButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.DPadDownRelease, InputGamepadActionFlags.DPadDownButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.DPadDownHeld, InputGamepadActionFlags.DPadDownButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.DPadLeftPress, InputGamepadActionFlags.DPadLeftButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.DPadLeftRelease, InputGamepadActionFlags.DPadLeftButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.DPadLeftHeld, InputGamepadActionFlags.DPadLeftButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.DPadRightPress, InputGamepadActionFlags.DPadRightButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.DPadRightRelease, InputGamepadActionFlags.DPadRightButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.DPadRightHeld, InputGamepadActionFlags.DPadRightButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.AnalogStickLeftButtonPress, InputGamepadActionFlags.AnalogLeftStickButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.AnalogStickLeftButtonRelease, InputGamepadActionFlags.AnalogLeftStickButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.AnalogStickLeftButtonHeld, InputGamepadActionFlags.AnalogLeftStickButton, InputActionStateFlags.Held);

            GamepadButtonActionHandler(gamepad.AnalogStickRightButtonPress, InputGamepadActionFlags.AnalogRightStickButton, InputActionStateFlags.Press);
            GamepadButtonActionHandler(gamepad.AnalogStickRightButtonRelease, InputGamepadActionFlags.AnalogRightStickButton, InputActionStateFlags.Release);
            GamepadButtonActionHandler(gamepad.AnalogStickRightButtonHeld, InputGamepadActionFlags.AnalogRightStickButton, InputActionStateFlags.Held);

            #endregion
        }

        /// <summary>
        /// Fires events as they occur.
        /// </summary>
        private void OnInputDetection()
        {
            #region Mouse

            if ((InputsInUse & InputsInUse.Mouse) == InputsInUse.Mouse)
            {
                MouseInput();
            }

            #endregion

            #region Keyboard

            if ((InputsInUse & InputsInUse.Keyboard) == InputsInUse.Keyboard)
            {
                KeyboardInput();
            }

            #endregion

            #region Gamepads

            if ((InputsInUse & InputsInUse.Gamepad) == InputsInUse.Gamepad)
            {
                foreach (var gamepad in Gamepads)
                {
                    GamepadInput(gamepad);
                }
            }

            #endregion
        }
        
        /// <summary>
        /// IO Manager Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes a MonoGame GameTime.</param>
        public void Update(GameTime gameTime)
        {
            // Calculate elapsed time.
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // If the keyboard is in use, update it.
            if ((InputsInUse & InputsInUse.Keyboard) == InputsInUse.Keyboard)
            {
                Keyboard.Update(gameTime);
            }

            // If the mouse is in use, update it.
            if ((InputsInUse & InputsInUse.Mouse) == InputsInUse.Mouse)
            {
                Mouse.Update(gameTime);
            }

            // If a gamepad is in use, update it.
            if ((InputsInUse & InputsInUse.Gamepad) == InputsInUse.Gamepad)
            {
                foreach (var gamepad in Gamepads)
                {
                    gamepad.Update(gameTime);
                }
            }

            // Perform input detection.
            OnInputDetection();

            // Reset elapsed timer.
            if (ElapsedTime >= PeriodicRefreshTimeoutInSeconds)
            {
                ElapsedTime = 0;
            }
        }
    }
}