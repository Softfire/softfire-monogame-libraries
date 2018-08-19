using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Softfire.MonoGame.IO
{
    public class IOGamepad
    {
        /// <summary>
        /// Delta Time.
        /// Time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// GamePad Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Is Connected?
        /// </summary>
        public bool IsConnected => GamePad.GetCapabilities(Id).IsConnected;

        /// <summary>
        /// GamePad State.
        /// </summary>
        private GamePadState GamePadState { get; set; }

        /// <summary>
        /// Previous GamePad State.
        /// </summary>
        private GamePadState PreviousGamePadState { get; set; }

        /// <summary>
        /// GamePad Type.
        /// </summary>
        public GamePadType GamePadType { get; }

        /// <summary>
        /// GamePad Dead Zone.
        /// </summary>
        public GamePadDeadZone GamePadDeadZone { get; }

        /// <summary>
        /// Analog Stick Sensitivity.
        /// </summary>
        public float AnalogStickSensitivity { get; private set; } = 0.001f;

        /// <summary>
        /// Trigger Sensitivity.
        /// </summary>
        public float TriggerSensitivity { get; private set; } = 0.001f;

        /// <summary>
        /// IO GamePad Constructor.
        /// </summary>
        /// <param name="gamePadIndex">The index of the gamepad.</param>
        /// <param name="gamePadType">The type of gamepad. Default type is GamePadType.GamePad.</param>
        /// <param name="gamePadDeadZone">The gamepad's dead zone to use for analog controls. Default zone is GamePadDeadZone.None.</param>
        public IOGamepad(int gamePadIndex, GamePadType gamePadType = GamePadType.GamePad, GamePadDeadZone gamePadDeadZone = GamePadDeadZone.None)
        {
            Id = gamePadIndex;
            GamePadType = gamePadType;
            GamePadDeadZone = gamePadDeadZone;
        }

        /// <summary>
        /// Set Id.
        /// </summary>
        /// <param name="id">The gamepad id to set. intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the application of the id was successful.</returns>
        public bool SetId(int id)
        {
            var result = false;

            if (id <= GamePad.MaximumGamePadCount)
            {
                Id = id;
                result = true;
            }

            return result;
        }

        #region Buttons

        /// <summary>
        /// Button Press.
        /// </summary>
        /// <param name="button">The button to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the button has been pressed.</returns>
        public bool ButtonPress(Buttons button)
        {
            return GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Button Held.
        /// </summary>
        /// <param name="button">The button to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating if the button is being held down.</returns>
        public bool ButtonHeld(Buttons button)
        {
            return PreviousGamePadState.IsButtonDown(button) && GamePadState.IsButtonDown(button);
        }

        #endregion

        #region Triggers

        /// <summary>
        /// Trigger Press.
        /// </summary>
        /// <param name="trigger">The trigger to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the trigger has been pressed.</returns>
        public bool TriggerPress(Buttons trigger)
        {
            return ButtonPress(trigger);
        }

        /// <summary>
        /// Trigger Held.
        /// </summary>
        /// <param name="trigger">The trigger to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating if the trigger is being held down.</returns>
        public bool TriggerHeld(Buttons trigger)
        {
            return ButtonHeld(trigger);
        }

        /// <summary>
        /// Trigger Left Press.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the trigger was pressed.</returns>
        public bool TriggerLeftPress()
        {
            return GamePadState.Triggers.Left > 0 && Math.Abs(PreviousGamePadState.Triggers.Left) < TriggerSensitivity;
        }

        /// <summary>
        /// Trigger Left Held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the trigger is being held.</returns>
        public bool TriggerLeftHeld()
        {
            return PreviousGamePadState.Triggers.Left > 0 && GamePadState.Triggers.Left > 0;
        }

        /// <summary>
        /// Trigger Right Press.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the trigger was pressed.</returns>
        public bool TriggerRightPress()
        {
            return GamePadState.Triggers.Right > 0 && Math.Abs(PreviousGamePadState.Triggers.Right) < TriggerSensitivity;
        }

        /// <summary>
        /// Trigger Right Held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the trigger is being held.</returns>
        public bool TriggerRightHeld()
        {
            return PreviousGamePadState.Triggers.Right > 0 && GamePadState.Triggers.Right > 0;
        }

        #endregion

        #region Analog Sticks

        /// <summary>
        /// Analog Stick Press.
        /// </summary>
        /// <param name="stick">The stick to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the stick is being held down.</returns>
        public bool AnalogStickPress(Buttons stick)
        {
            return ButtonPress(stick);
        }

        /// <summary>
        /// Analog Stick Held.
        /// </summary>
        /// <param name="stick">The stick to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the stick is being held down.</returns>
        public bool AnalogStickHeld(Buttons stick)
        {
            return ButtonHeld(stick);
        }

        #region Left Analog Stick

        /// <summary>
        /// Analog Stick Left - Up.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed up.</returns>
        public bool AnalogStickLeftUp()
        {
            return GamePadState.ThumbSticks.Left.Y > AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Left - Down.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed down.</returns>
        public bool AnalogStickLeftDown()
        {
            return GamePadState.ThumbSticks.Left.Y < -AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Left - Left.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed to the left.</returns>
        public bool AnalogStickLeftLeft()
        {
            return GamePadState.ThumbSticks.Left.X < -AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Left - Right.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed to the right.</returns>
        public bool AnalogStickLeftRight()
        {
            return GamePadState.ThumbSticks.Left.X > AnalogStickSensitivity;
        }

        #endregion

        #region Right Analog Stick

        /// <summary>
        /// Analog Stick Right - Up.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed up.</returns>
        public bool AnalogStickRightUp()
        {
            return GamePadState.ThumbSticks.Right.Y > AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Right - Down.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed down.</returns>
        public bool AnalogStickRightDown()
        {
            return GamePadState.ThumbSticks.Right.Y < -AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Right - Left.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed to the left.</returns>
        public bool AnalogStickRightLeft()
        {
            return GamePadState.ThumbSticks.Right.X < -AnalogStickSensitivity;
        }

        /// <summary>
        /// Analog Stick Right - Right.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed to the right.</returns>
        public bool AnalogStickRightRight()
        {
            return GamePadState.ThumbSticks.Right.X > AnalogStickSensitivity;
        }

        #endregion

        #endregion

        #region DPad

        /// <summary>
        /// DPad Press.
        /// </summary>
        /// <param name="dPadDirection">The DPad direction to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the DPad direction has been pressed.</returns>
        public bool DPadPress(Buttons dPadDirection)
        {
            return ButtonPress(dPadDirection);
        }

        /// <summary>
        /// DPad Held.
        /// </summary>
        /// <param name="dPadDirection">The DPad direction to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating if the DPad direction is being held down.</returns>
        public bool DPadHeld(Buttons dPadDirection)
        {
            return ButtonHeld(dPadDirection);
        }

        #endregion

        /// <summary>
        /// GamePad Update Method.
        /// Update GamePad States.
        /// </summary>
        /// <param name="gameTime">Intakes a MonoGame GameTime instance.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsConnected)
            {
                PreviousGamePadState = GamePadState;
                GamePadState = GamePad.GetState(Id, GamePadDeadZone);
            }
        }
    }
}