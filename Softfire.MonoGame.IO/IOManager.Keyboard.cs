using System;
using System.Collections.Generic;
using Softfire.MonoGame.CORE.Input;

namespace Softfire.MonoGame.IO
{
    public partial class IOManager
    {
        #region Keyboard Confirmation Command Mappings

        /// <summary>
        /// Keyboard function mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardFunctionFlags> ConfirmationCommandsToKeyboardFunctionMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardFunctionFlags>();

        /// <summary>
        /// Keyboard num pad mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardNumPadFlags> ConfirmationCommandsToKeyboardNumPadMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardNumPadFlags>();

        /// <summary>
        /// Keyboard number mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardNumberFlags> ConfirmationCommandsToKeyboardNumberMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardNumberFlags>();

        /// <summary>
        /// Keyboard command mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardCommandFlags> ConfirmationCommandsToKeyboardCommandMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardCommandFlags>();

        /// <summary>
        /// Keyboard special mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardSpecialFlags> ConfirmationCommandsToKeyboardSpecialMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardSpecialFlags>();

        /// <summary>
        /// Keyboard arrow mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardArrowFlags> ConfirmationCommandsToKeyboardArrowMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardArrowFlags>();

        /// <summary>
        /// Keyboard letter mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardLetterFlags> ConfirmationCommandsToKeyboardLetterMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputKeyboardLetterFlags>();

        #endregion

        #region Keyboard Movement Command Mappings

        /// <summary>
        /// Keyboard function mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardFunctionFlags> MovementCommandsToKeyboardFunctionMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardFunctionFlags>();

        /// <summary>
        /// Keyboard num pad mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardNumPadFlags> MovementCommandsToKeyboardNumPadMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardNumPadFlags>();

        /// <summary>
        /// Keyboard number mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardNumberFlags> MovementCommandsToKeyboardNumberMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardNumberFlags>();

        /// <summary>
        /// Keyboard command mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardCommandFlags> MovementCommandsToKeyboardCommandMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardCommandFlags>();

        /// <summary>
        /// Keyboard special mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardSpecialFlags> MovementCommandsToKeyboardSpecialMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardSpecialFlags>();

        /// <summary>
        /// Keyboard arrow mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardArrowFlags> MovementCommandsToKeyboardArrowMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardArrowFlags>();

        /// <summary>
        /// Keyboard letter mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputKeyboardLetterFlags> MovementCommandsToKeyboardLetterMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputKeyboardLetterFlags>();

        #endregion

        #region Keyboard Camera Command Mappings

        /// <summary>
        /// Keyboard function mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardFunctionFlags> CameraCommandsToKeyboardFunctionMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardFunctionFlags>();

        /// <summary>
        /// Keyboard num pad mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardNumPadFlags> CameraCommandsToKeyboardNumPadMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardNumPadFlags>();

        /// <summary>
        /// Keyboard number mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardNumberFlags> CameraCommandsToKeyboardNumberMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardNumberFlags>();

        /// <summary>
        /// Keyboard command mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardCommandFlags> CameraCommandsToKeyboardCommandMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardCommandFlags>();

        /// <summary>
        /// Keyboard special mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardSpecialFlags> CameraCommandsToKeyboardSpecialMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardSpecialFlags>();

        /// <summary>
        /// Keyboard arrow mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardArrowFlags> CameraCommandsToKeyboardArrowMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardArrowFlags>();

        /// <summary>
        /// Keyboard letter mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputKeyboardLetterFlags> CameraCommandsToKeyboardLetterMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputKeyboardLetterFlags>();

        #endregion

        #region Keyboard Input Mapping To Confirmation Command Controls

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardFunctionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardFunctionFlags flagToMap) => ConfirmationCommandsToKeyboardFunctionMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumPadFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardNumPadFlags flagToMap) => ConfirmationCommandsToKeyboardNumPadMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumberFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardNumberFlags flagToMap) => ConfirmationCommandsToKeyboardNumberMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardCommandFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardCommandFlags flagToMap) => ConfirmationCommandsToKeyboardCommandMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardSpecialFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardSpecialFlags flagToMap) => ConfirmationCommandsToKeyboardSpecialMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardArrowFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardArrowFlags flagToMap) => ConfirmationCommandsToKeyboardArrowMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardLetterFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableConfirmationCommandFlags command, InputKeyboardLetterFlags flagToMap) => ConfirmationCommandsToKeyboardLetterMappings[command] = flagToMap;

        #endregion

        #region Keyboard Input Mapping To Movement Command Controls

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardFunctionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardFunctionFlags flagToMap) => MovementCommandsToKeyboardFunctionMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumPadFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardNumPadFlags flagToMap) => MovementCommandsToKeyboardNumPadMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumberFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardNumberFlags flagToMap) => MovementCommandsToKeyboardNumberMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardCommandFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardCommandFlags flagToMap) => MovementCommandsToKeyboardCommandMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardSpecialFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardSpecialFlags flagToMap) => MovementCommandsToKeyboardSpecialMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardArrowFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardArrowFlags flagToMap) => MovementCommandsToKeyboardArrowMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardLetterFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableMovementCommandFlags command, InputKeyboardLetterFlags flagToMap) => MovementCommandsToKeyboardLetterMappings[command] = flagToMap;

        #endregion

        #region Keyboard Input Mapping To Camera Command Controls

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardFunctionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardFunctionFlags flagToMap) => CameraCommandsToKeyboardFunctionMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumPadFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardNumPadFlags flagToMap) => CameraCommandsToKeyboardNumPadMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardNumberFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardNumberFlags flagToMap) => CameraCommandsToKeyboardNumberMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardCommandFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardCommandFlags flagToMap) => CameraCommandsToKeyboardCommandMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardSpecialFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardSpecialFlags flagToMap) => CameraCommandsToKeyboardSpecialMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardArrowFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardArrowFlags flagToMap) => CameraCommandsToKeyboardArrowMappings[command] = flagToMap;

        /// <summary>
        /// Maps the keyboard flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputKeyboardLetterFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapKeyboardInput(InputMappableCameraCommandFlags command, InputKeyboardLetterFlags flagToMap) => CameraCommandsToKeyboardLetterMappings[command] = flagToMap;

        #endregion

        #region Keyboard Key Action Handlers

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardFunctionFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardFunctionFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardNumPadFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardNumPadFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardCommandFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardCommandFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardSpecialFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardSpecialFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardArrowFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardArrowFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardLetterFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardLetterFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        /// <summary>
        /// Processes the event handlers for the key action.
        /// </summary>
        /// <param name="keyAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputKeyboardNumberFlags"/> to pass to the event handler, if the 'keyAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void KeyboardKeyActionHandler(Func<bool> keyAction, InputKeyboardNumberFlags flag, InputActionStateFlags state)
        {
            if (keyAction())
            {
                var args = new InputEventArgs
                {
                    InputDeltas = Mouse.GetMovementDeltas(),
                    InputRectangle = Mouse.Rectangle,
                    PlayerIndex = ActivePlayer
                };
                args.InputFlags.AddFlag(flag);
                args.InputStates.SetState(flag, state);

                switch (state)
                {
                    case InputActionStateFlags.Press:
                        InputPressHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Release:
                        InputReleaseHandler?.Invoke(this, args);
                        break;
                    case InputActionStateFlags.Held:
                        InputHeldHandler?.Invoke(this, args);
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Loads the default keyboard input mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping load was successful.</returns>
        private bool LoadKeyboardInputMappings()
        {
            // Confirmation
            MapKeyboardInput(InputMappableConfirmationCommandFlags.Confirm, InputKeyboardCommandFlags.EnterKey);
            MapKeyboardInput(InputMappableConfirmationCommandFlags.Cancel, InputKeyboardCommandFlags.EscapeKey);

            MapKeyboardInput(InputMappableMovementCommandFlags.MoveUp, InputKeyboardLetterFlags.WKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.MoveDown, InputKeyboardLetterFlags.SKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.MoveLeft, InputKeyboardLetterFlags.AKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.MoveRight, InputKeyboardLetterFlags.DKey);

            MapKeyboardInput(InputMappableMovementCommandFlags.StrafeUp, InputKeyboardArrowFlags.UpKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.StrafeDown, InputKeyboardArrowFlags.DownKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.StrafeLeft, InputKeyboardArrowFlags.LeftKey);
            MapKeyboardInput(InputMappableMovementCommandFlags.StrafeRight, InputKeyboardArrowFlags.RightKey);
            
            MapKeyboardInput(InputMappableCameraCommandFlags.ZoomIn, InputKeyboardCommandFlags.PageUpKey);
            MapKeyboardInput(InputMappableCameraCommandFlags.ZoomOut, InputKeyboardCommandFlags.PageDownKey);
            MapKeyboardInput(InputMappableCameraCommandFlags.PanUp, InputKeyboardNumPadFlags.NumPad8Key);
            MapKeyboardInput(InputMappableCameraCommandFlags.PanDown, InputKeyboardNumPadFlags.NumPad2Key);
            MapKeyboardInput(InputMappableCameraCommandFlags.Center, InputKeyboardNumPadFlags.NumPad5Key);
            MapKeyboardInput(InputMappableCameraCommandFlags.PanLeft, InputKeyboardNumPadFlags.NumPad4Key);
            MapKeyboardInput(InputMappableCameraCommandFlags.PanRight, InputKeyboardNumPadFlags.NumPad6Key);

            return true;
        }

        /// <summary>
        /// Clears and reloads the default gamepad input mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping reset was successful.</returns>
        public bool ResetKeyboardInputMappings()
        {
            ConfirmationCommandsToKeyboardFunctionMappings.Clear();
            ConfirmationCommandsToKeyboardNumPadMappings.Clear();
            ConfirmationCommandsToKeyboardNumberMappings.Clear();
            ConfirmationCommandsToKeyboardCommandMappings.Clear();
            ConfirmationCommandsToKeyboardSpecialMappings.Clear();
            ConfirmationCommandsToKeyboardArrowMappings.Clear();
            ConfirmationCommandsToKeyboardLetterMappings.Clear();
            return LoadKeyboardInputMappings();
        }
    }
}