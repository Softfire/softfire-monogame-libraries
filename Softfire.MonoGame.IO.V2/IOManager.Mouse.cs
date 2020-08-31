using System;
using System.Collections.Generic;
using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    public partial class IOManager
    {
        #region Mouse Command Mappings

        /// <summary>
        /// Mouse action mappings to confirmation commands.
        /// </summary>
        private Dictionary<InputMappableConfirmationCommandFlags, InputMouseActionFlags> ConfirmationCommandsToMouseActionMappings { get; } = new Dictionary<InputMappableConfirmationCommandFlags, InputMouseActionFlags>();

        /// <summary>
        /// Mouse action mappings to movement commands.
        /// </summary>
        private Dictionary<InputMappableMovementCommandFlags, InputMouseActionFlags> MovementCommandsToMouseActionMappings { get; } = new Dictionary<InputMappableMovementCommandFlags, InputMouseActionFlags>();

        /// <summary>
        /// Mouse action mappings to camera commands.
        /// </summary>
        private Dictionary<InputMappableCameraCommandFlags, InputMouseActionFlags> CameraCommandsToMouseActionMappings { get; } = new Dictionary<InputMappableCameraCommandFlags, InputMouseActionFlags>();

        #endregion

        #region Mouse Input Mapping Controls

        /// <summary>
        /// Maps the mouse flag to the confirmation command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableConfirmationCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputMouseActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapMouseInput(InputMappableConfirmationCommandFlags command, InputMouseActionFlags flagToMap) => ConfirmationCommandsToMouseActionMappings[command] = flagToMap;


        /// <summary>
        /// Maps the mouse flag to the movement command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableMovementCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputMouseActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapMouseInput(InputMappableMovementCommandFlags command, InputMouseActionFlags flagToMap) => MovementCommandsToMouseActionMappings[command] = flagToMap;


        /// <summary>
        /// Maps the mouse flag to the camera command flag.
        /// </summary>
        /// <param name="command">The managed input command to map. Intaken as a <see cref="InputMappableCameraCommandFlags"/>.</param>
        /// <param name="flagToMap">The flag to map. Intaken as a <see cref="InputMouseActionFlags"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping was successful.</returns>
        public void MapMouseInput(InputMappableCameraCommandFlags command, InputMouseActionFlags flagToMap) => CameraCommandsToMouseActionMappings[command] = flagToMap;

        #endregion

        #region Mouse Button Action Handlers

        /// <summary>
        /// Processes the event handlers for the button action.
        /// </summary>
        /// <param name="buttonAction">The function to process. If true the appropriate event handler is raised. Intaken as a <see cref="Func{TResult}"/></param>
        /// <param name="flag">The <see cref="InputMouseActionFlags"/> to pass to the event handler, if the 'buttonAction' returns true.</param>
        /// <param name="state">The associated key state, such as <see cref="InputActionStateFlags.Press"/>, to pass to the event handler</param>
        private void MouseButtonActionHandler(Func<bool> buttonAction, InputMouseActionFlags flag, InputActionStateFlags state)
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
        /// Loads the default mouse input mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping load was successful.</returns>
        private bool LoadMouseInputMappings()
        {
            MapMouseInput(InputMappableConfirmationCommandFlags.Confirm, InputMouseActionFlags.LeftClick);
            MapMouseInput(InputMappableConfirmationCommandFlags.Cancel, InputMouseActionFlags.RightClick);

            MapMouseInput(InputMappableCameraCommandFlags.PanUp, InputMouseActionFlags.ScrollUp);
            MapMouseInput(InputMappableCameraCommandFlags.PanDown, InputMouseActionFlags.ScrollDown);
            MapMouseInput(InputMappableCameraCommandFlags.PanLeft, InputMouseActionFlags.ScrollLeft);
            MapMouseInput(InputMappableCameraCommandFlags.PanRight, InputMouseActionFlags.ScrollRight);

            return true;
        }

        /// <summary>
        /// Clears and reloads the default mouse feature mappings.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating the mapping reset was successful.</returns>
        public bool ResetMouseInputMappings()
        {
            ConfirmationCommandsToMouseActionMappings.Clear();
            MovementCommandsToMouseActionMappings.Clear();
            CameraCommandsToMouseActionMappings.Clear();
            return LoadMouseInputMappings();
        }
    }
}