using System;
using System.Collections.Generic;

namespace Softfire.MonoGame.CORE.Input
{
    /// <summary>
    /// The input states for keyboard, mice and gamepad actions.
    /// </summary>
    public class InputStates
    {
        #region States

        /// <summary>
        /// Holds the state of each keyboard function key.
        /// </summary>
        private Dictionary<InputKeyboardFunctionFlags, InputActionStateFlags> KeyboardFunctionStates { get; }

        /// <summary>
        /// Holds the state of each keyboard num pad key.
        /// </summary>
        private Dictionary<InputKeyboardNumPadFlags, InputActionStateFlags> KeyboardNumPadStates { get; }

        /// <summary>
        /// Holds the state of each keyboard number key.
        /// </summary>
        private Dictionary<InputKeyboardNumberFlags, InputActionStateFlags> KeyboardNumberStates { get; }

        /// <summary>
        /// Holds the state of each keyboard command key.
        /// </summary>
        private Dictionary<InputKeyboardCommandFlags, InputActionStateFlags> KeyboardCommandStates { get; }

        /// <summary>
        /// Holds the state of each keyboard special key.
        /// </summary>
        private Dictionary<InputKeyboardSpecialFlags, InputActionStateFlags> KeyboardSpecialStates { get; }

        /// <summary>
        /// Holds the state of each keyboard arrow key.
        /// </summary>
        private Dictionary<InputKeyboardArrowFlags, InputActionStateFlags> KeyboardArrowStates { get; }

        /// <summary>
        /// Holds the state of each keyboard letter key.
        /// </summary>
        private Dictionary<InputKeyboardLetterFlags, InputActionStateFlags> KeyboardLetterStates { get; }
        
        /// <summary>
        /// Holds the state of each gamepad input.
        /// </summary>
        private Dictionary<InputGamepadActionFlags, InputActionStateFlags> GamepadInputStates { get; }

        /// <summary>
        /// Holds the state of each mouse input.
        /// </summary>
        private Dictionary<InputMouseActionFlags, InputActionStateFlags> MouseInputStates { get; }

        /// <summary>
        /// Holds the state of each mappable confirmation command input.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputActionStateFlags> MappableConfirmationCommandInputStates { get; }

        /// <summary>
        /// Holds the state of each mappable movement command input.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputActionStateFlags> MappableMovementCommandInputStates { get; }

        /// <summary>
        /// Holds the state of each mappable camera command input.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputActionStateFlags> MappableCameraCommandInputStates { get; }

        #endregion

        /// <summary>
        /// Input states for keyboards, mice and gamepads.
        /// </summary>
        public InputStates()
        {
            KeyboardFunctionStates = new Dictionary<InputKeyboardFunctionFlags, InputActionStateFlags>();
            KeyboardNumPadStates = new Dictionary<InputKeyboardNumPadFlags, InputActionStateFlags>();
            KeyboardNumberStates = new Dictionary<InputKeyboardNumberFlags, InputActionStateFlags>();
            KeyboardCommandStates = new Dictionary<InputKeyboardCommandFlags, InputActionStateFlags>();
            KeyboardSpecialStates = new Dictionary<InputKeyboardSpecialFlags, InputActionStateFlags>();
            KeyboardArrowStates = new Dictionary<InputKeyboardArrowFlags, InputActionStateFlags>();
            KeyboardLetterStates = new Dictionary<InputKeyboardLetterFlags, InputActionStateFlags>();
            GamepadInputStates = new Dictionary<InputGamepadActionFlags, InputActionStateFlags>();
            MouseInputStates = new Dictionary<InputMouseActionFlags, InputActionStateFlags>();

            MappableConfirmationCommandInputStates = new Dictionary<InputMappableConfirmationCommandFlags, InputActionStateFlags>();
            MappableMovementCommandInputStates = new Dictionary<InputMappableMovementCommandFlags, InputActionStateFlags>();
            MappableCameraCommandInputStates = new Dictionary<InputMappableCameraCommandFlags, InputActionStateFlags>();

            Clear();
        }
        
        #region Input Keyboard Function State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardFunctionFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardFunctionFlags flag) => KeyboardFunctionStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardFunctionFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardFunctionFlags flag, InputActionStateFlags state) => KeyboardFunctionStates[flag] = state;
        
        #endregion

        #region Input Keyboard NumPad State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardNumPadFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardNumPadFlags flag) => KeyboardNumPadStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardNumPadFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardNumPadFlags flag, InputActionStateFlags state) => KeyboardNumPadStates[flag] = state;

        #endregion

        #region Input Keyboard Number State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardNumberFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardNumberFlags flag) => KeyboardNumberStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardNumberFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardNumberFlags flag, InputActionStateFlags state) => KeyboardNumberStates[flag] = state;

        #endregion

        #region Input Keyboard Command State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardCommandFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardCommandFlags flag) => KeyboardCommandStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardCommandFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardCommandFlags flag, InputActionStateFlags state) => KeyboardCommandStates[flag] = state;

        #endregion

        #region Input Keyboard Special State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardSpecialFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardSpecialFlags flag) => KeyboardSpecialStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardSpecialFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardSpecialFlags flag, InputActionStateFlags state) => KeyboardSpecialStates[flag] = state;

        #endregion

        #region Input Keyboard Arrow State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardArrowFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardArrowFlags flag) => KeyboardArrowStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardArrowFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardArrowFlags flag, InputActionStateFlags state) => KeyboardArrowStates[flag] = state;

        #endregion

        #region Input Keyboard Letter State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardLetterFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputKeyboardLetterFlags flag) => KeyboardLetterStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputKeyboardLetterFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputKeyboardLetterFlags flag, InputActionStateFlags state) => KeyboardLetterStates[flag] = state;

        #endregion

        #region Input Gamepad Action State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputGamepadActionFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputGamepadActionFlags flag) => GamepadInputStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputGamepadActionFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputGamepadActionFlags flag, InputActionStateFlags state) => GamepadInputStates[flag] = state;

        #endregion

        #region Input Mouse Action State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMouseActionFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputMouseActionFlags flag) => MouseInputStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMouseActionFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputMouseActionFlags flag, InputActionStateFlags state) => MouseInputStates[flag] = state;

        #endregion

        #region Input Mappable Confirmation Command State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputMappableConfirmationCommandFlags flag) => MappableConfirmationCommandInputStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputMappableConfirmationCommandFlags flag, InputActionStateFlags state) => MappableConfirmationCommandInputStates[flag] = state;

        #endregion

        #region Input Mappable Movement Command State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputMappableMovementCommandFlags flag) => MappableMovementCommandInputStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputMappableMovementCommandFlags flag, InputActionStateFlags state) => MappableMovementCommandInputStates[flag] = state;

        #endregion

        #region Input Mappable Camera Command State Methods

        /// <summary>
        /// Retrieves the current state.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <returns></returns>
        public InputActionStateFlags GetState(InputMappableCameraCommandFlags flag) => MappableCameraCommandInputStates[flag];

        /// <summary>
        /// Sets the state of the flag.
        /// </summary>
        /// <param name="flag">The flag to retrieve the status on. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="state">The flag's new state. Intaken as a <see cref="InputActionStateFlags"/>.</param>
        public void SetState(InputMappableCameraCommandFlags flag, InputActionStateFlags state) => MappableCameraCommandInputStates[flag] = state;

        #endregion

        /// <summary>
        /// Clears and reloads input states.
        /// </summary>
        public void Clear()
        {
            KeyboardFunctionStates.Clear();

            foreach (InputKeyboardFunctionFlags flag in Enum.GetValues(typeof(InputKeyboardFunctionFlags)))
            {
                KeyboardFunctionStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardNumPadStates.Clear();

            foreach (InputKeyboardNumPadFlags flag in Enum.GetValues(typeof(InputKeyboardNumPadFlags)))
            {
                KeyboardNumPadStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardNumberStates.Clear();

            foreach (InputKeyboardNumberFlags flag in Enum.GetValues(typeof(InputKeyboardNumberFlags)))
            {
                KeyboardNumberStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardCommandStates.Clear();

            foreach (InputKeyboardCommandFlags flag in Enum.GetValues(typeof(InputKeyboardCommandFlags)))
            {
                KeyboardCommandStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardSpecialStates.Clear();

            foreach (InputKeyboardSpecialFlags flag in Enum.GetValues(typeof(InputKeyboardSpecialFlags)))
            {
                KeyboardSpecialStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardArrowStates.Clear();

            foreach (InputKeyboardArrowFlags flag in Enum.GetValues(typeof(InputKeyboardArrowFlags)))
            {
                KeyboardArrowStates.Add(flag, InputActionStateFlags.Idle);
            }

            KeyboardLetterStates.Clear();

            foreach (InputKeyboardLetterFlags flag in Enum.GetValues(typeof(InputKeyboardLetterFlags)))
            {
                KeyboardLetterStates.Add(flag, InputActionStateFlags.Idle);
            }

            GamepadInputStates.Clear();

            foreach (InputGamepadActionFlags flag in Enum.GetValues(typeof(InputGamepadActionFlags)))
            {
                GamepadInputStates.Add(flag, InputActionStateFlags.Idle);
            }

            MouseInputStates.Clear();

            foreach (InputMouseActionFlags flag in Enum.GetValues(typeof(InputMouseActionFlags)))
            {
                MouseInputStates.Add(flag, InputActionStateFlags.Idle);
            }

            MappableConfirmationCommandInputStates.Clear();

            foreach (InputMappableConfirmationCommandFlags flag in Enum.GetValues(typeof(InputMappableConfirmationCommandFlags)))
            {
                MappableConfirmationCommandInputStates.Add(flag, InputActionStateFlags.Idle);
            }

            MappableMovementCommandInputStates.Clear();

            foreach (InputMappableMovementCommandFlags flag in Enum.GetValues(typeof(InputMappableMovementCommandFlags)))
            {
                MappableMovementCommandInputStates.Add(flag, InputActionStateFlags.Idle);
            }

            MappableCameraCommandInputStates.Clear();

            foreach (InputMappableCameraCommandFlags flag in Enum.GetValues(typeof(InputMappableCameraCommandFlags)))
            {
                MappableCameraCommandInputStates.Add(flag, InputActionStateFlags.Idle);
            }
        }

        /// <summary>
        /// Copies the input states of the passed in <see cref="InputStates"/>.
        /// </summary>
        /// <param name="input">The <see cref="InputStates"/> to copy.</param>
        public void Copy(InputStates input)
        {
            KeyboardFunctionStates.Clear();
            foreach (var state in input.KeyboardFunctionStates)
            {
                KeyboardFunctionStates.Add(state.Key, state.Value);
            }

            KeyboardNumPadStates.Clear();
            foreach (var state in input.KeyboardNumPadStates)
            {
                KeyboardNumPadStates.Add(state.Key, state.Value);
            }

            KeyboardNumberStates.Clear();
            foreach (var state in input.KeyboardNumberStates)
            {
                KeyboardNumberStates.Add(state.Key, state.Value);
            }

            KeyboardCommandStates.Clear();
            foreach (var state in input.KeyboardCommandStates)
            {
                KeyboardCommandStates.Add(state.Key, state.Value);
            }

            KeyboardSpecialStates.Clear();
            foreach (var state in input.KeyboardSpecialStates)
            {
                KeyboardSpecialStates.Add(state.Key, state.Value);
            }

            KeyboardArrowStates.Clear();
            foreach (var state in input.KeyboardArrowStates)
            {
                KeyboardArrowStates.Add(state.Key, state.Value);
            }

            KeyboardLetterStates.Clear();
            foreach (var state in input.KeyboardLetterStates)
            {
                KeyboardLetterStates.Add(state.Key, state.Value);
            }

            GamepadInputStates.Clear();
            foreach (var state in input.GamepadInputStates)
            {
                GamepadInputStates.Add(state.Key, state.Value);
            }

            MouseInputStates.Clear();
            foreach (var state in input.MouseInputStates)
            {
                MouseInputStates.Add(state.Key, state.Value);
            }

            MappableConfirmationCommandInputStates.Clear();
            foreach (var state in input.MappableConfirmationCommandInputStates)
            {
                MappableConfirmationCommandInputStates.Add(state.Key, state.Value);
            }

            MappableMovementCommandInputStates.Clear();
            foreach (var state in input.MappableMovementCommandInputStates)
            {
                MappableMovementCommandInputStates.Add(state.Key, state.Value);
            }

            MappableCameraCommandInputStates.Clear();
            foreach (var state in input.MappableCameraCommandInputStates)
            {
                MappableCameraCommandInputStates.Add(state.Key, state.Value);
            }
        }
    }
}