using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Softfire.MonoGame.IO
{
    public enum InputType : byte
    {
        Confirm,
        Cancel,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        ScrollUp,
        ScrollDown,
        ScrollLeft,
        ScrollRight,
        ToggleCamera,
        SnapCamera,
        ZoomInCamera,
        ZoomOutCamera
    }

    /// <summary>
        /// This is a game component that implements IUpdateable.
        /// </summary>
    public static class IOManager
    {
        /// <summary>
        /// Managed Keys.
        /// Contains the mappings designated for each supported input type.
        /// </summary>
        private static Dictionary<InputType, Keys> ManagedKeyboardKeys { get; } = new Dictionary<InputType, Keys>();

        /// <summary>
        /// Managed Buttons.
        /// Contains the mappings designated for each supported input type. 
        /// </summary>
        private static Dictionary<InputType, Buttons> ManagedGamepadButtons { get; } = new Dictionary<InputType, Buttons>();

        /// <summary>
        /// Managed Mouse Buttons.
        /// Contains the mappings designated for each supported input type. 
        /// </summary>
        private static Dictionary<InputType, IOMouse.Buttons> ManagedMouseButtons { get; } = new Dictionary<InputType, IOMouse.Buttons>();

        /// <summary>
        /// Gamepads.
        /// Contains the currently active gamepads.
        /// </summary>
        private static List<IOGamepad> Gamepads { get; } = new List<IOGamepad>(GamePad.MaximumGamePadCount);

        /// <summary>
        /// Active Player.
        /// </summary>
        public static int ActivePlayer { get; set; } = 1;

        /// <summary>
        /// Active Gamepads.
        /// </summary>
        public static int ActiveGamepads => Gamepads.Count;
        
        /// <summary>
        /// IO Manager.
        /// Manages input of all connected devices.
        /// Supporting Keyboard, Mouse and Gamepads.
        /// </summary>
        static IOManager()
        {
            LoadKeyboardInput();
            LoadMouseInput();
            LoadGamepadInput();
        }

        #region Gamepad Assignment

        /// <summary>
        /// Reassign Gamepad.
        /// </summary>
        /// <param name="gamepadToReassign"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        public static bool ReassignGamepad(IOGamepad gamepadToReassign, int newId)
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
        public static int AddGamepad()
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
        /// <param name="id">The id of the gamepad to retrieve. Intaken as an int.</param>
        /// <returns>Returns the requested IOGamepad, if present, otherwise null.</returns>
        public static IOGamepad GetGamepad(int id)
        {
            return Gamepads.FirstOrDefault(gamepad => gamepad.Id == id);
        }

        /// <summary>
        /// Remove Gamepad.
        /// </summary>
        /// <param name="id">The id of the gamepad to remove. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the gamepad was removed.</returns>
        public static bool RemoveGamepad(int id)
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

        #region Input Assignment Controls

        public static bool SetKeyboardInput(InputType inputType, Keys key)
        {
            ManagedKeyboardKeys[inputType] = key;

            return ManagedKeyboardKeys[inputType] == key;
        }

        private static bool LoadKeyboardInput()
        {
            ManagedKeyboardKeys.Add(InputType.Confirm, Keys.Enter);
            ManagedKeyboardKeys.Add(InputType.Cancel, Keys.Escape);
            ManagedKeyboardKeys.Add(InputType.MoveUp, Keys.W);
            ManagedKeyboardKeys.Add(InputType.MoveLeft, Keys.A);
            ManagedKeyboardKeys.Add(InputType.MoveDown, Keys.S);
            ManagedKeyboardKeys.Add(InputType.MoveRight, Keys.D);
            ManagedKeyboardKeys.Add(InputType.ScrollUp, Keys.Up);
            ManagedKeyboardKeys.Add(InputType.ScrollLeft, Keys.Left);
            ManagedKeyboardKeys.Add(InputType.ScrollDown, Keys.Down);
            ManagedKeyboardKeys.Add(InputType.ScrollRight, Keys.Right);
            ManagedKeyboardKeys.Add(InputType.ToggleCamera, Keys.T);
            ManagedKeyboardKeys.Add(InputType.SnapCamera, Keys.R);
            ManagedKeyboardKeys.Add(InputType.ZoomInCamera, Keys.PageUp);
            ManagedKeyboardKeys.Add(InputType.ZoomOutCamera, Keys.PageDown);

            return true;
        }

        public static bool ResetKeyboardInput()
        {
            ManagedKeyboardKeys.Clear();
            return LoadKeyboardInput();
        }

        public static bool SetGamepadInput(InputType inputType, Buttons button)
        {
            ManagedGamepadButtons[inputType] = button;

            return ManagedGamepadButtons[inputType] == button;
        }

        private static bool LoadGamepadInput()
        {
            ManagedGamepadButtons.Add(InputType.Confirm, Buttons.A);
            ManagedGamepadButtons.Add(InputType.Cancel, Buttons.B);
            ManagedGamepadButtons.Add(InputType.MoveUp, Buttons.LeftThumbstickUp);
            ManagedGamepadButtons.Add(InputType.MoveLeft, Buttons.LeftThumbstickLeft);
            ManagedGamepadButtons.Add(InputType.MoveDown, Buttons.LeftThumbstickDown);
            ManagedGamepadButtons.Add(InputType.MoveRight, Buttons.LeftThumbstickRight);
            ManagedGamepadButtons.Add(InputType.ScrollUp, Buttons.RightThumbstickUp);
            ManagedGamepadButtons.Add(InputType.ScrollLeft, Buttons.RightThumbstickLeft);
            ManagedGamepadButtons.Add(InputType.ScrollDown, Buttons.RightThumbstickDown);
            ManagedGamepadButtons.Add(InputType.ScrollRight, Buttons.RightThumbstickRight);
            ManagedGamepadButtons.Add(InputType.ToggleCamera, Buttons.RightStick);
            ManagedGamepadButtons.Add(InputType.SnapCamera, Buttons.LeftStick);
            ManagedGamepadButtons.Add(InputType.ZoomInCamera, Buttons.LeftShoulder);
            ManagedGamepadButtons.Add(InputType.ZoomOutCamera, Buttons.RightShoulder);

            return true;
        }

        public static bool ResetGamepadInput()
        {
            ManagedGamepadButtons.Clear();
            return LoadGamepadInput();
        }

        public static bool SetMouseInput(InputType inputType, IOMouse.Buttons button)
        {
            ManagedMouseButtons[inputType] = button;

            return ManagedMouseButtons[inputType] == button;
        }

        private static bool LoadMouseInput()
        {
            ManagedMouseButtons.Add(InputType.Confirm, IOMouse.Buttons.Left);
            ManagedMouseButtons.Add(InputType.Cancel, IOMouse.Buttons.Right);
            ManagedMouseButtons.Add(InputType.MoveUp, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.MoveLeft, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.MoveDown, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.MoveRight, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ScrollUp, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ScrollLeft, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ScrollDown, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ScrollRight, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ToggleCamera, IOMouse.Buttons.Middle);
            ManagedMouseButtons.Add(InputType.SnapCamera, IOMouse.Buttons.One);
            ManagedMouseButtons.Add(InputType.ZoomInCamera, IOMouse.Buttons.Undefined);
            ManagedMouseButtons.Add(InputType.ZoomOutCamera, IOMouse.Buttons.Undefined);

            return true;
        }

        public static bool ResetMouseInput()
        {
            ManagedMouseButtons.Clear();
            return LoadMouseInput();
        }

        #endregion

        #region Input Handling

        /// <summary>
        /// Input Idle.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input is idle.</returns>
        public static bool InputIdle(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyIdle(ManagedKeyboardKeys[inputType]) ||
                   IOMouse.ButtonIdle(ManagedMouseButtons[inputType]) ||
                   playerIndex <= Gamepads.Count && Gamepads[playerIndex].ButtonIdle(ManagedGamepadButtons[inputType]);
        }

        /// <summary>
        /// Input Released.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input was released.</returns>
        public static bool InputReleased(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyRelease(ManagedKeyboardKeys[inputType]) ||
                   IOMouse.ButtonRelease(ManagedMouseButtons[inputType]) ||
                   playerIndex <= Gamepads.Count && Gamepads[playerIndex].ButtonRelease(ManagedGamepadButtons[inputType]);
        }

        /// <summary>
        /// Input Pressed.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input was pressed.</returns>
        public static bool InputPressed(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyPress(ManagedKeyboardKeys[inputType]) ||
                   IOMouse.ButtonPress(ManagedMouseButtons[inputType]) ||
                   playerIndex <= Gamepads.Count && Gamepads[playerIndex].ButtonPress(ManagedGamepadButtons[inputType]);
        }

        /// <summary>
        /// Input Held.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input is being held.</returns>
        public static bool InputHeld(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyHeld(ManagedKeyboardKeys[inputType]) ||
                   IOMouse.ButtonHeld(ManagedMouseButtons[inputType]) ||
                   playerIndex <= Gamepads.Count && Gamepads[playerIndex].ButtonHeld(ManagedGamepadButtons[inputType]);
        }

        #endregion

        /// <summary>
        /// IO Manager Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes a MonoGame GameTime.</param>
        public static void Update(GameTime gameTime)
        {
            // Update Keyboard.
            IOKeyboard.Update(gameTime);

            // Update Mouse.
            IOMouse.Update(gameTime);

            // Update Gamepads.
            foreach (var gamepad in Gamepads)
            {
                gamepad.Update(gameTime);
            }
        }

        /// <summary>
        /// IO Manager Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame SpriteBatch.</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            IOMouse.Draw(spriteBatch);
        }
    }
}