using System;
using System.Collections.Generic;
using Softfire.MonoGame.CORE.Input;

namespace Softfire.MonoGame.IO
{
    public partial class IOManager
    {
        #region Gamepad Command Mappings

        /// <summary>
        /// Gamepad action mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputGamepadActionFlags> ConfirmationCommandsToGamepadActionMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputGamepadActionFlags>();

        /// <summary>
        /// Gamepad action mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputGamepadActionFlags> MovementCommandsToGamepadActionMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputGamepadActionFlags>();

        /// <summary>
        /// Gamepad action mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputGamepadActionFlags> CameraCommandsToGamepadActionMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputGamepadActionFlags>();

        #endregion

        #region Gamepad Input Mapping Controls

        /// <summary>
        /// Maps the gamepad flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputGamepadActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapGamepadInput(InputMappableConfirmationCommandFlags command, InputGamepadActionFlags flagToMap) => ConfirmationCommandsToGamepadActionMappings[command] = flagToMap;
        
        /// <summary>
        /// Maps the gamepad flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputGamepadActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapGamepadInput(InputMappableMovementCommandFlags command, InputGamepadActionFlags flagToMap) => MovementCommandsToGamepadActionMappings[command] = flagToMap;
        
        /// <summary>
        /// Maps the gamepad flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputGamepadActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapGamepadInput(InputMappableCameraCommandFlags command, InputGamepadActionFlags flagToMap) => CameraCommandsToGamepadActionMappings[command] = flagToMap;

        #endregion
        
        #region Gamepad Button Action Handlers

        /// <summary>
        /// Processes the event handlers for the button action.
        /// </summary>
        /// <param name="buttonAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputGamepadActionFlags"/> to pass to the event handler, if the 'buttonAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void GamepadButtonActionHandler(Func<bool> buttonAction, InputGamepadActionFlags flag, InputActionStateFlags state)
        {
            if (buttonAction())
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
        /// Loads the default gamepad input mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping load was successful.</returns>
        private bool LoadGamepadInput()
        {
            MapGamepadInput(InputMappableConfirmationCommandFlags.Confirm, InputGamepadActionFlags.AButton);
            MapGamepadInput(InputMappableConfirmationCommandFlags.Cancel, InputGamepadActionFlags.BButton);

            MapGamepadInput(InputMappableMovementCommandFlags.MoveUp, InputGamepadActionFlags.DPadUpButton);
            MapGamepadInput(InputMappableMovementCommandFlags.MoveDown, InputGamepadActionFlags.DPadDownButton);
            MapGamepadInput(InputMappableMovementCommandFlags.MoveLeft, InputGamepadActionFlags.DPadLeftButton);
            MapGamepadInput(InputMappableMovementCommandFlags.MoveRight, InputGamepadActionFlags.DPadRightButton);

            MapGamepadInput(InputMappableCameraCommandFlags.PanUp, InputGamepadActionFlags.AnalogRightStickUp);
            MapGamepadInput(InputMappableCameraCommandFlags.PanDown, InputGamepadActionFlags.AnalogRightStickDown);
            MapGamepadInput(InputMappableCameraCommandFlags.PanLeft, InputGamepadActionFlags.AnalogRightStickLeft);
            MapGamepadInput(InputMappableCameraCommandFlags.PanRight, InputGamepadActionFlags.AnalogRightStickRight);
            MapGamepadInput(InputMappableCameraCommandFlags.Toggle, InputGamepadActionFlags.AnalogRightStickButton);
            MapGamepadInput(InputMappableCameraCommandFlags.Center, InputGamepadActionFlags.AnalogLeftStickButton);
            MapGamepadInput(InputMappableCameraCommandFlags.ZoomIn, InputGamepadActionFlags.ShoulderLeftButton);
            MapGamepadInput(InputMappableCameraCommandFlags.ZoomOut, InputGamepadActionFlags.ShoulderRightButton);

            return true;
        }

        /// <summary>
        /// Clears and reloads the default gamepad input mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping reset was successful.</returns>
        public bool ResetGamepadInputMappings()
        {
            ConfirmationCommandsToGamepadActionMappings.Clear();
            MovementCommandsToGamepadActionMappings.Clear();
            CameraCommandsToGamepadActionMappings.Clear();
            return LoadGamepadInput();
        }
    }
}