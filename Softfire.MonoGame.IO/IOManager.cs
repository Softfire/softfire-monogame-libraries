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
        private static Dictionary<InputType, Keys> ManagedKeys { get; } = new Dictionary<InputType, Keys>();

        /// <summary>
        /// Managed Buttons.
        /// Contains the mappings designated for each supported input type. 
        /// </summary>
        private static Dictionary<InputType, Buttons> ManagedButtons { get; } = new Dictionary<InputType, Buttons>();

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
            ManagedKeys[inputType] = key;

            return ManagedKeys[inputType] == key;
        }

        private static bool LoadKeyboardInput()
        {
            ManagedKeys.Add(InputType.Confirm, Keys.Enter);
            ManagedKeys.Add(InputType.Cancel, Keys.Escape);
            ManagedKeys.Add(InputType.MoveUp, Keys.W);
            ManagedKeys.Add(InputType.MoveLeft, Keys.A);
            ManagedKeys.Add(InputType.MoveDown, Keys.S);
            ManagedKeys.Add(InputType.MoveRight, Keys.D);
            ManagedKeys.Add(InputType.ScrollUp, Keys.Up);
            ManagedKeys.Add(InputType.ScrollLeft, Keys.Left);
            ManagedKeys.Add(InputType.ScrollDown, Keys.Down);
            ManagedKeys.Add(InputType.ScrollRight, Keys.Right);
            ManagedKeys.Add(InputType.ToggleCamera, Keys.T);
            ManagedKeys.Add(InputType.SnapCamera, Keys.R);
            ManagedKeys.Add(InputType.ZoomInCamera, Keys.PageUp);
            ManagedKeys.Add(InputType.ZoomOutCamera, Keys.PageDown);

            return true;
        }

        public static bool ResetKeyboardInput()
        {
            ManagedKeys.Clear();
            return LoadKeyboardInput();
        }

        public static bool SetGamepadInput(InputType inputType, Buttons button)
        {
            ManagedButtons[inputType] = button;

            return ManagedButtons[inputType] == button;
        }

        private static bool LoadGamepadInput()
        {
            ManagedButtons.Add(InputType.Confirm, Buttons.A);
            ManagedButtons.Add(InputType.Cancel, Buttons.B);
            ManagedButtons.Add(InputType.MoveUp, Buttons.LeftThumbstickUp);
            ManagedButtons.Add(InputType.MoveLeft, Buttons.LeftThumbstickLeft);
            ManagedButtons.Add(InputType.MoveDown, Buttons.LeftThumbstickDown);
            ManagedButtons.Add(InputType.MoveRight, Buttons.LeftThumbstickRight);
            ManagedButtons.Add(InputType.ScrollUp, Buttons.RightThumbstickUp);
            ManagedButtons.Add(InputType.ScrollLeft, Buttons.RightThumbstickLeft);
            ManagedButtons.Add(InputType.ScrollDown, Buttons.RightThumbstickDown);
            ManagedButtons.Add(InputType.ScrollRight, Buttons.RightThumbstickRight);
            ManagedButtons.Add(InputType.ToggleCamera, Buttons.RightStick);
            ManagedButtons.Add(InputType.SnapCamera, Buttons.LeftStick);
            ManagedButtons.Add(InputType.ZoomInCamera, Buttons.LeftShoulder);
            ManagedButtons.Add(InputType.ZoomOutCamera, Buttons.RightShoulder);

            return true;
        }

        public static bool ResetGamepadInput()
        {
            ManagedButtons.Clear();
            return LoadGamepadInput();
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
            return IOKeyboard.KeyIdle(ManagedKeys[inputType]) || Gamepads[playerIndex].ButtonIdle(ManagedButtons[inputType]);
        }

        /// <summary>
        /// Input Released.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input was released.</returns>
        public static bool InputReleased(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyRelease(ManagedKeys[inputType]) || Gamepads[playerIndex].ButtonRelease(ManagedButtons[inputType]);
        }

        /// <summary>
        /// Input Pressed.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input was pressed.</returns>
        public static bool InputPressed(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyPress(ManagedKeys[inputType]) || Gamepads[playerIndex].ButtonPress(ManagedButtons[inputType]);
        }

        /// <summary>
        /// Input Held.
        /// </summary>
        /// <param name="inputType">Type of input to check. Intaken as an InputType.</param>
        /// <param name="playerIndex">Player's device to check. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the input is being held.</returns>
        public static bool InputHeld(InputType inputType, int playerIndex)
        {
            return IOKeyboard.KeyHeld(ManagedKeys[inputType]) || Gamepads[playerIndex].ButtonHeld(ManagedButtons[inputType]);
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