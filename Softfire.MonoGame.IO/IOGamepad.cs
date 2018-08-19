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
        /// Analog Stick Sensitivity Level.
        /// </summary>
        public float AnalogStickSensitivityLevel { get; private set; } = 0.01f;

        /// <summary>
        /// Trigger Sensitivity Level.
        /// </summary>
        public float TriggerSensitivityLevel { get; private set; } = 0.01f;

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
        /// <param name="id">The gamepad id to set. Intaken as an int. Must be no greater than what is set in GamePad.MaximumGamePadCount.</param>
        public void SetId(int id)
        {
            Id = MathHelper.Clamp(id, 1, GamePad.MaximumGamePadCount);
        }

        /// <summary>
        /// Set Analog Stick Sensitivity Level.
        /// </summary>
        /// <param name="sensitivityLevel">The sensitivity level chosen, between 0 and 1.</param>
        public void SetAnalogStickSensitivityLevel(float sensitivityLevel)
        {
            AnalogStickSensitivityLevel = MathHelper.Clamp(sensitivityLevel, 0, 1);
        }

        /// <summary>
        /// Set Trigger Sensitivity Level.
        /// </summary>
        /// <param name="sensitivityLevel">The sensitivity level chosen, between 0 and 1.</param>
        public void SetTriggerSensitivityLevel(float sensitivityLevel)
        {
            TriggerSensitivityLevel = MathHelper.Clamp(sensitivityLevel, 0, 1);
        }

        #region Buttons

        /// <summary>
        /// Button Idle.
        /// </summary>
        /// <param name="button">The button to check if it is idle.</param>
        /// <returns>Returns a boolean indicating if the button is idle.</returns>
        public bool ButtonIdle(Buttons button)
        {
            return GamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Button Release.
        /// </summary>
        /// <param name="button">The button to check if it has been released.</param>
        /// <returns>Returns a boolean indicating if the button has been released.</returns>
        public bool ButtonRelease(Buttons button)
        {
            return GamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
        }

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
        /// Trigger Idle.
        /// </summary>
        /// <param name="trigger">The trigger to check if it is idle.</param>
        /// <returns>Returns a boolean indicating whether the trigger is idle.</returns>
        public bool TriggerIdle(Buttons trigger)
        {
            return ButtonIdle(trigger);
        }

        /// <summary>
        /// Trigger Release.
        /// </summary>
        /// <param name="trigger">The trigger to check if it has been released.</param>
        /// <returns>Returns a boolean indicating whether the trigger has been released.</returns>
        public bool TriggerRelease(Buttons trigger)
        {
            return ButtonRelease(trigger);
        }

        /// <summary>
        /// Trigger Press.
        /// </summary>
        /// <param name="trigger">The trigger to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating whether the trigger has been pressed.</returns>
        public bool TriggerPress(Buttons trigger)
        {
            return ButtonPress(trigger);
        }

        /// <summary>
        /// Trigger Held.
        /// </summary>
        /// <param name="trigger">The trigger to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating whether the trigger is being held down.</returns>
        public bool TriggerHeld(Buttons trigger)
        {
            return ButtonHeld(trigger);
        }

        /// <summary>
        /// Trigger Left Idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left trigger is idle.</returns>
        public bool TriggerLeftIdle()
        {
            return GamePadState.Triggers.Left < TriggerSensitivityLevel && Math.Abs(PreviousGamePadState.Triggers.Left) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Left Release.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left trigger was released.</returns>
        public bool TriggerLeftRelease()
        {
            return PreviousGamePadState.Triggers.Left > TriggerSensitivityLevel && Math.Abs(GamePadState.Triggers.Left) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Left Press.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left trigger was pressed.</returns>
        public bool TriggerLeftPress()
        {
            return GamePadState.Triggers.Left > TriggerSensitivityLevel && Math.Abs(PreviousGamePadState.Triggers.Left) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Left Held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left trigger is being held.</returns>
        public bool TriggerLeftHeld()
        {
            return PreviousGamePadState.Triggers.Left > TriggerSensitivityLevel && GamePadState.Triggers.Left > TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Right Idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right trigger is idle.</returns>
        public bool TriggerRightIdle()
        {
            return GamePadState.Triggers.Right < TriggerSensitivityLevel && Math.Abs(PreviousGamePadState.Triggers.Right) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Right Release.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right trigger was released.</returns>
        public bool TriggerRightRelease()
        {
            return PreviousGamePadState.Triggers.Right > TriggerSensitivityLevel && Math.Abs(GamePadState.Triggers.Right) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Right Press.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right trigger was pressed.</returns>
        public bool TriggerRightPress()
        {
            return GamePadState.Triggers.Right > TriggerSensitivityLevel && Math.Abs(PreviousGamePadState.Triggers.Right) < TriggerSensitivityLevel;
        }

        /// <summary>
        /// Trigger Right Held.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right trigger is being held.</returns>
        public bool TriggerRightHeld()
        {
            return PreviousGamePadState.Triggers.Right > TriggerSensitivityLevel && GamePadState.Triggers.Right > TriggerSensitivityLevel;
        }

        #endregion

        #region Shoulders

        /// <summary>
        /// Shoulder Idle.
        /// </summary>
        /// <param name="shoulder">The shoulder to check if it is idle.</param>
        /// <returns>Returns a boolean indicating whether the shoulder is idle.</returns>
        public bool ShoulderIdle(Buttons shoulder)
        {
            return ButtonIdle(shoulder);
        }

        /// <summary>
        /// Shoulder Release.
        /// </summary>
        /// <param name="shoulder">The shoulder to check if it has been released.</param>
        /// <returns>Returns a boolean indicating whether the shoulder has been released.</returns>
        public bool ShoulderRelease(Buttons shoulder)
        {
            return ButtonRelease(shoulder);
        }

        /// <summary>
        /// Shoulder Press.
        /// </summary>
        /// <param name="shoulder">The shoulder to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating whether the shoulder has been pressed.</returns>
        public bool ShoulderPress(Buttons shoulder)
        {
            return ButtonPress(shoulder);
        }

        /// <summary>
        /// Shoulder Held.
        /// </summary>
        /// <param name="shoulder">The shoulder to check if it is being held down.</param>
        /// <returns>Returns a boolean indicating whether the shoulder is being held down.</returns>
        public bool ShoulderHeld(Buttons shoulder)
        {
            return ButtonHeld(shoulder);
        }

        #endregion

        #region Analog Sticks

        /// <summary>
        /// Analog Stick Idle.
        /// </summary>
        /// <param name="stick">The stick to check if it is idle.</param>
        /// <returns>Returns a boolean indicating whether the stick is idle.</returns>
        public bool AnalogStickIdle(Buttons stick)
        {
            return ButtonIdle(stick);
        }

        /// <summary>
        /// Analog Stick Release.
        /// </summary>
        /// <param name="stick">The stick to check if it has been released.</param>
        /// <returns>Returns a boolean indicating whether the stick has been released.</returns>
        public bool AnalogStickRelease(Buttons stick)
        {
            return ButtonRelease(stick);
        }

        /// <summary>
        /// Analog Stick Press.
        /// </summary>
        /// <param name="stick">The stick to check if it has been pressed.</param>
        /// <returns>Returns a boolean indicating if the stick has been pressed.</returns>
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
        /// Analog Stick Left - Idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is idle.</returns>
        public bool AnalogStickLeftIdle()
        {
            return !AnalogStickLeftUp() &&
                   !AnalogStickLeftDown() &&
                   !AnalogStickLeftLeft() &&
                   !AnalogStickLeftRight();
        }

        /// <summary>
        /// Analog Stick Left - Up.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed up.</returns>
        public bool AnalogStickLeftUp()
        {
            return GamePadState.ThumbSticks.Left.Y > AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Left - Down.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed down.</returns>
        public bool AnalogStickLeftDown()
        {
            return GamePadState.ThumbSticks.Left.Y < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Left - Left.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed to the left.</returns>
        public bool AnalogStickLeftLeft()
        {
            return GamePadState.ThumbSticks.Left.X < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Left - Right.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the left analog stick is pushed to the right.</returns>
        public bool AnalogStickLeftRight()
        {
            return GamePadState.ThumbSticks.Left.X > AnalogStickSensitivityLevel;
        }

        #endregion

        #region Right Analog Stick

        /// <summary>
        /// Analog Stick Right - Idle.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is idle.</returns>
        public bool AnalogStickRightIdle()
        {
            return !AnalogStickRightUp() &&
                   !AnalogStickRightDown() &&
                   !AnalogStickRightLeft() &&
                   !AnalogStickRightRight();
        }

        /// <summary>
        /// Analog Stick Right - Up.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed up.</returns>
        public bool AnalogStickRightUp()
        {
            return GamePadState.ThumbSticks.Right.Y > AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Right - Down.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed down.</returns>
        public bool AnalogStickRightDown()
        {
            return GamePadState.ThumbSticks.Right.Y < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Right - Left.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed to the left.</returns>
        public bool AnalogStickRightLeft()
        {
            return GamePadState.ThumbSticks.Right.X < -AnalogStickSensitivityLevel;
        }

        /// <summary>
        /// Analog Stick Right - Right.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the right analog stick is pushed to the right.</returns>
        public bool AnalogStickRightRight()
        {
            return GamePadState.ThumbSticks.Right.X > AnalogStickSensitivityLevel;
        }

        #endregion

        #endregion

        #region DPad

        /// <summary>
        /// DPad Idle.
        /// </summary>
        /// <param name="dPadDirection">The DPad direction to check if it is idle.</param>
        /// <returns>Returns a boolean indicating if the DPad direction is idle.</returns>
        public bool DPadIdle(Buttons dPadDirection)
        {
            return ButtonIdle(dPadDirection);
        }

        /// <summary>
        /// DPad Release.
        /// </summary>
        /// <param name="dPadDirection">The DPad direction to check if it has been released.</param>
        /// <returns>Returns a boolean indicating if the DPad direction has been released.</returns>
        public bool DPadRelease(Buttons dPadDirection)
        {
            return ButtonRelease(dPadDirection);
        }

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
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsConnected)
            {
                PreviousGamePadState = GamePadState;
                GamePadState = GamePad.GetState(Id, GamePadDeadZone);
            }
        }
    }
}