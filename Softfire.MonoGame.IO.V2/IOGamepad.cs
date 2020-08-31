using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.CORE.V2.Input;

namespace Softfire.MonoGame.IO.V2
{
    /// <summary>
    /// A class for your gamepads.
    /// </summary>
    public partial class IOGamepad : IMonoGameInputComponent
    {
        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The elapsed time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// The gamepad's id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Determines whether the gamepad is connected.
        /// </summary>
        public bool IsConnected => GamePad.GetCapabilities(Id).IsConnected;

        /// <summary>
        /// The gamepad's current state.
        /// </summary>
        private GamePadState GamepadState { get; set; }

        /// <summary>
        /// The gamepad's previous state.
        /// </summary>
        private GamePadState PreviousGamepadState { get; set; }

        /// <summary>
        /// The gamepad's type.
        /// </summary>
        public GamePadType GamepadType { get; }

        /// <summary>
        /// The gamepad's dead zone. The amount of space offset from center that is will not trigger the analog sticks to engage.
        /// </summary>
        public GamePadDeadZone GamepadDeadZone { get; }

        /// <summary>
        /// The gamepad's internal analog sensitivity level value.
        /// </summary>
        private float _analogStickSensitivityLevel = 0.01f;

        /// <summary>
        /// the gamepad's analog stick sensitivity level.
        /// </summary>
        public float AnalogStickSensitivityLevel
        {
            get => _analogStickSensitivityLevel;
            set => _analogStickSensitivityLevel = MathHelper.Clamp(value, 0, 1);
        }

        /// <summary>
        /// The gamepad's internal trigger stick sensitivity level value.
        /// </summary>
        private float _triggerSensitivityLevel = 0.01f;

        /// <summary>
        /// The gamepad's trigger sensitivity level.
        /// </summary>
        public float TriggerSensitivityLevel
        {
            get => _triggerSensitivityLevel;
            set => _triggerSensitivityLevel = MathHelper.Clamp(value, 0, 1);
        }
        
        /// <summary>
        /// A gamepad. Used to play games.
        /// </summary>
        /// <param name="gamepadIndex">The index of the gamepad. Must be less than or equal to <see cref="GamePad.MaximumGamePadCount"/>.</param>
        /// <param name="gamepadType">The type of gamepad. Default type is GamePadType.GamePad.</param>
        /// <param name="gamepadDeadZone">The gamepad's dead zone to use for analog controls. Intaken as a <see cref="GamepadDeadZone"/>.</param>
        public IOGamepad(int gamepadIndex, GamePadType gamepadType = GamePadType.GamePad, GamePadDeadZone gamepadDeadZone = GamePadDeadZone.None)
        {
            SetId(gamepadIndex);
            GamepadType = gamepadType;
            GamepadDeadZone = gamepadDeadZone;
        }

        /// <summary>
        /// Sets the gamepad's id.
        /// </summary>
        /// <param name="id">The gamepad id to set. Must be less than or equal to <see cref="GamePad.MaximumGamePadCount"/>. Intaken as an <see cref="int"/>.</param>
        public void SetId(int id)
        {
            Id = MathHelper.Clamp(id, 1, GamePad.MaximumGamePadCount);
        }
        
        /// <summary>
        /// Determines whether the button is in a pressed state.
        /// </summary>
        /// <param name="button">The button to check if it is in a pressed state. Intaken as a <see cref="Buttons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool ButtonPress(Buttons button) => GamepadState.IsButtonDown(button) && PreviousGamepadState.IsButtonUp(button);

        /// <summary>
        /// Determines whether the button is in a released state.
        /// </summary>
        /// <param name="button">The button to check if it is in a released state. Intaken as a <see cref="Buttons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool ButtonRelease(Buttons button) => GamepadState.IsButtonUp(button) && PreviousGamepadState.IsButtonDown(button);

        /// <summary>
        /// Determines whether the button is in a held state.
        /// </summary>
        /// <param name="button">The button to check if it is in a held state. Intaken as a <see cref="Buttons"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool ButtonHeld(Buttons button) => GamepadState.IsButtonDown(button) && PreviousGamepadState.IsButtonDown(button);

        #region Buttons

        /// <summary>
        /// Determines whether the button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool AButtonPress()
        {
            return ButtonPress(Buttons.A);
        }

        /// <summary>
        /// Determines whether the button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool AButtonRelease()
        {
            return ButtonRelease(Buttons.A);
        }

        /// <summary>
        /// Determines whether the button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool AButtonHeld()
        {
            return ButtonHeld(Buttons.A);
        }

        /// <summary>
        /// Determines whether the button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool BButtonPress()
        {
            return ButtonPress(Buttons.B);
        }

        /// <summary>
        /// Determines whether the button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool BButtonRelease()
        {
            return ButtonRelease(Buttons.B);
        }

        /// <summary>
        /// Determines whether the button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool BButtonHeld()
        {
            return ButtonHeld(Buttons.B);
        }

        /// <summary>
        /// Determines whether the button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool YButtonPress()
        {
            return ButtonPress(Buttons.Y);
        }

        /// <summary>
        /// Determines whether the button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool YButtonRelease()
        {
            return ButtonRelease(Buttons.Y);
        }

        /// <summary>
        /// Determines whether the button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool YButtonHeld()
        {
            return ButtonHeld(Buttons.Y);
        }

        /// <summary>
        /// Determines whether the button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a pressed state.</returns>
        public bool XButtonPress()
        {
            return ButtonPress(Buttons.X);
        }

        /// <summary>
        /// Determines whether the button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a released state.</returns>
        public bool XButtonRelease()
        {
            return ButtonRelease(Buttons.X);
        }

        /// <summary>
        /// Determines whether the button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the button is in a held state.</returns>
        public bool XButtonHeld()
        {
            return ButtonHeld(Buttons.X);
        }

        #endregion

        #region Left Trigger

        /// <summary>
        /// Determines whether the trigger button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a pressed state.</returns>
        public bool TriggerLeftButtonPress()
        {
            return ButtonPress(Buttons.LeftTrigger);
        }

        /// <summary>
        /// Determines whether the trigger button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a released state.</returns>
        public bool TriggerLeftButtonRelease()
        {
            return ButtonRelease(Buttons.LeftTrigger);
        }

        /// <summary>
        /// Determines whether the trigger button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a held state.</returns>
        public bool TriggerLeftButtonHeld()
        {
            return ButtonHeld(Buttons.LeftTrigger);
        }

        /// <summary>
        /// Determines whether the trigger is in a pressed pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the left trigger is in a pressed pressure state.</returns>
        public bool TriggerLeftPressurePress()
        {
            return GamepadState.Triggers.Left > TriggerSensitivityLevel && Math.Abs(PreviousGamepadState.Triggers.Left) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the trigger is in a released pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger is in a released pressure state.</returns>
        public bool TriggerLeftPressureRelease()
        {
            return PreviousGamepadState.Triggers.Left > TriggerSensitivityLevel && Math.Abs(GamepadState.Triggers.Left) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the trigger is in a held pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger is in a held pressure state.</returns>
        public bool TriggerLeftPressureHeld()
        {
            return PreviousGamepadState.Triggers.Left > TriggerSensitivityLevel && GamepadState.Triggers.Left > TriggerSensitivityLevel;
        }

        #endregion

        #region Right Trigger

        /// <summary>
        /// Determines whether the trigger button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a pressed state.</returns>
        public bool TriggerRightButtonPress()
        {
            return ButtonPress(Buttons.RightTrigger);
        }

        /// <summary>
        /// Determines whether the trigger button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a released state.</returns>
        public bool TriggerRightButtonRelease()
        {
            return ButtonRelease(Buttons.RightTrigger);
        }

        /// <summary>
        /// Determines whether the trigger button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger button is in a held state.</returns>
        public bool TriggerRightButtonHeld()
        {
            return ButtonHeld(Buttons.RightTrigger);
        }

        /// <summary>
        /// Determines whether the trigger is in a pressed pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the left trigger is in a pressed pressure state.</returns>
        public bool TriggerRightPressurePress()
        {
            return GamepadState.Triggers.Right > TriggerSensitivityLevel && Math.Abs(PreviousGamepadState.Triggers.Right) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the trigger is in a released pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger is in a released pressure state.</returns>
        public bool TriggerRightPressureRelease()
        {
            return PreviousGamepadState.Triggers.Right > TriggerSensitivityLevel && Math.Abs(GamepadState.Triggers.Right) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the trigger is in a held pressure state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the trigger is in a held pressure state.</returns>
        public bool TriggerRightPressureHeld()
        {
            return PreviousGamepadState.Triggers.Right > TriggerSensitivityLevel && GamepadState.Triggers.Right > TriggerSensitivityLevel;
        }

        #endregion

        #region Left Shoulder

        /// <summary>
        /// Determines whether the shoulder button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a pressed state.</returns>
        public bool ShoulderLeftButtonPress()
        {
            return ButtonPress(Buttons.LeftShoulder);
        }

        /// <summary>
        /// Determines whether the shoulder button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a released state.</returns>
        public bool ShoulderLeftButtonRelease()
        {
            return ButtonRelease(Buttons.LeftShoulder);
        }

        /// <summary>
        /// Determines whether the shoulder button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a held state.</returns>
        public bool ShoulderLeftButtonHeld()
        {
            return ButtonHeld(Buttons.LeftShoulder);
        }

        #endregion

        #region Right Shoulder

        /// <summary>
        /// Determines whether the shoulder button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a pressed state.</returns>
        public bool ShoulderRightButtonPress()
        {
            return ButtonPress(Buttons.RightShoulder);
        }

        /// <summary>
        /// Determines whether the shoulder button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a released state.</returns>
        public bool ShoulderRightButtonRelease()
        {
            return ButtonRelease(Buttons.RightShoulder);
        }

        /// <summary>
        /// Determines whether the shoulder button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the shoulder button is in a held state.</returns>
        public bool ShoulderRightButtonHeld()
        {
            return ButtonHeld(Buttons.RightShoulder);
        }

        #endregion
        
        #region Left Analog Stick

        /// <summary>
        /// Determines whether the analog stick button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a pressed state.</returns>
        public bool AnalogStickLeftButtonPress()
        {
            return ButtonPress(Buttons.LeftStick);
        }

        /// <summary>
        /// Determines whether the analog stick button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a released state.</returns>
        public bool AnalogStickLeftButtonRelease()
        {
            return ButtonRelease(Buttons.LeftStick);
        }

        /// <summary>
        /// Determines whether the analog stick button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a held state.</returns>
        public bool AnalogStickLeftButtonHeld()
        {
            return ButtonHeld(Buttons.LeftStick);
        }
        
        /// <summary>
        /// Determines whether the analog stick is in an upward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in an upward state.</returns>
        public bool AnalogStickLeftUpward()
        {
            return GamepadState.ThumbSticks.Left.Y > AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a downward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a downward state.</returns>
        public bool AnalogStickLeftDownward()
        {
            return GamepadState.ThumbSticks.Left.Y < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a leftward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a leftward state.</returns>
        public bool AnalogStickLeftLeftward()
        {
            return GamepadState.ThumbSticks.Left.X < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a rightward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a rightward state.</returns>
        public bool AnalogStickLeftRightward()
        {
            return GamepadState.ThumbSticks.Left.X > AnalogStickSensitivityLevel;
        }

        #endregion

        #region Right Analog Stick

        /// <summary>
        /// Determines whether the analog stick button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a pressed state.</returns>
        public bool AnalogStickRightButtonPress()
        {
            return ButtonPress(Buttons.RightStick);
        }

        /// <summary>
        /// Determines whether the analog stick button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a released state.</returns>
        public bool AnalogStickRightButtonRelease()
        {
            return ButtonRelease(Buttons.RightStick);
        }

        /// <summary>
        /// Determines whether the analog stick button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick button is in a held state.</returns>
        public bool AnalogStickRightButtonHeld()
        {
            return ButtonHeld(Buttons.RightStick);
        }

        /// <summary>
        /// Determines whether the analog stick is in an upward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in an upward state.</returns>
        public bool AnalogStickRightUpward()
        {
            return GamepadState.ThumbSticks.Right.Y > AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a downward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a downward state.</returns>
        public bool AnalogStickRightDownward()
        {
            return GamepadState.ThumbSticks.Right.Y < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a leftward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a leftward state.</returns>
        public bool AnalogStickRightLeftward()
        {
            return GamepadState.ThumbSticks.Right.X < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Determines whether the analog stick is in a rightward state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the analog stick is in a rightward state.</returns>
        public bool AnalogStickRightRightward()
        {
            return GamepadState.ThumbSticks.Right.X > AnalogStickSensitivityLevel;
        }

        #endregion
        
        #region DPad Up

        /// <summary>
        /// Determines whether the d-pad's directional button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a pressed state.</returns>
        public bool DPadUpPress()
        {
            return ButtonPress(Buttons.DPadUp);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a released state.</returns>
        public bool DPadUpRelease()
        {
            return ButtonRelease(Buttons.DPadUp);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a held state.</returns>
        public bool DPadUpHeld()
        {
            return ButtonHeld(Buttons.DPadUp);
        }

        #endregion

        #region DPad Down

        /// <summary>
        /// Determines whether the d-pad's directional button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a pressed state.</returns>
        public bool DPadDownPress()
        {
            return ButtonPress(Buttons.DPadDown);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a released state.</returns>
        public bool DPadDownRelease()
        {
            return ButtonRelease(Buttons.DPadDown);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a held state.</returns>
        public bool DPadDownHeld()
        {
            return ButtonHeld(Buttons.DPadDown);
        }

        #endregion

        #region DPad Left

        /// <summary>
        /// Determines whether the d-pad's directional button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a pressed state.</returns>
        public bool DPadLeftPress()
        {
            return ButtonPress(Buttons.DPadLeft);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a released state.</returns>
        public bool DPadLeftRelease()
        {
            return ButtonRelease(Buttons.DPadLeft);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a held state.</returns>
        public bool DPadLeftHeld()
        {
            return ButtonHeld(Buttons.DPadLeft);
        }

        #endregion

        #region DPad Right
    
        /// <summary>
        /// Determines whether the d-pad's directional button is in a pressed state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a pressed state.</returns>
        public bool DPadRightPress()
        {
            return ButtonPress(Buttons.DPadRight);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a released state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a released state.</returns>
        public bool DPadRightRelease()
        {
            return ButtonRelease(Buttons.DPadRight);
        }

        /// <summary>
        /// Determines whether the d-pad's directional button is in a held state.
        /// </summary>
        /// <returns>Returns a <see cref="bool"/> indicating whether the d-pad's directional button is in a held state.</returns>
        public bool DPadRightHeld()
        {
            return ButtonHeld(Buttons.DPadRight);
        }

        #endregion
        
        /// <summary>
        /// The gamepad's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            // Updating elapsed time and delta time.
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            // Update gamepad if it is connected.
            if (IsConnected)
            {
                PreviousGamepadState = GamepadState;
                GamepadState = GamePad.GetState(Id, GamepadDeadZone);
            }
        }
    }
}