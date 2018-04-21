using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.UI.Effects;
using Softfire.MonoGame.UI.Effects.Transitions;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// Partial UIBase Class.
    /// Extends UIBase with Effects.
    /// </summary>
    public abstract partial class UIBase
    {
        /// <summary>
        /// Rates of Change.
        /// </summary>
        public enum RatesOfChange
        {
            X001 = 1,
            X002,
            X003,
            X004,
            X005,
            X006,
            X007,
            X008,
            X009,
            X010,
            X011,
            X012,
            X013,
            X014,
            X015,
            X016,
            X017,
            X018,
            X019,
            X020,
            X021,
            X022,
            X023,
            X024,
            X025,
            X026,
            X027,
            X028,
            X029,
            X030,
            X031,
            X032,
            X033,
            X034,
            X035,
            X036,
            X037,
            X038,
            X039,
            X040,
            X041,
            X042,
            X043,
            X044,
            X045,
            X046,
            X047,
            X048,
            X049,
            X050,
            X051,
            X052,
            X053,
            X054,
            X055,
            X056,
            X057,
            X058,
            X059,
            X060,
            X061,
            X062,
            X063,
            X064,
            X065,
            X066,
            X067,
            X068,
            X069,
            X070,
            X071,
            X072,
            X073,
            X074,
            X075,
            X076,
            X077,
            X078,
            X079,
            X080,
            X081,
            X082,
            X083,
            X084,
            X085,
            X086,
            X087,
            X088,
            X089,
            X090,
            X091,
            X092,
            X093,
            X094,
            X095,
            X096,
            X097,
            X098,
            X099,
            X100
        }

        #region Scaling Properties

        /// <summary>
        /// Will Scale Out On Selection?
        /// Set to true to enable scaling out on selection.
        /// </summary>
        public bool WillScaleOutOnSelection { get; set; }

        /// <summary>
        /// Will Scale In On Selection?
        /// Set to true to enable scaling in on selection.
        /// </summary>
        public bool WillScaleInOnSelection { get; set; }

        /// <summary>
        /// Is Scaling Out?
        /// Indicates whether the object is scaling out.
        /// </summary>
        public bool IsScalingOut { get; private set; }

        /// <summary>
        /// Is Scaling In?
        /// Indicates whether the object is scaling in.
        /// </summary>
        public bool IsScalingIn { get; private set; }

        /// <summary>
        /// On Selection Scale Out To.
        /// Set upper Scale target to reach on selection.
        /// </summary>
        public Vector2 OnSelectionScaleOutTo { get; set; }

        /// <summary>
        /// On Selection Scale In To.
        /// Set lower Scale target to reach on selection.
        /// </summary>
        public Vector2 OnSelectionScaleInTo { get; set; }

        /// <summary>
        /// Scaling Out Rate of Change.
        /// </summary>
        protected Vector2 ScalingOutRateOfChange { get; set; }

        /// <summary>
        /// Scaling In Rate of Change.
        /// </summary>
        protected Vector2 ScalingInRateOfChange { get; set; }

        /// <summary>
        /// Scaling Out Speed.
        /// </summary>
        public RatesOfChange ScalingOutSpeed { get; set; }

        /// <summary>
        /// Scaling In Speed.
        /// </summary>
        public RatesOfChange ScalingInSpeed { get; set; }

        #endregion

        #region Scaling Methods

        /// <summary>
        /// Set Scaling Out Properties.
        /// </summary>
        /// <param name="willScaleOutOnSelection">A bool indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleOutTo">A Vector2 indicating to what scale the UI element will scale out to.</param>
        /// <param name="scalingOutSpeed">The value defining the rate of change the UI element will scale out at.</param>
        /// <param name="scalingInSpeed">The value defining the rate of change the UI element will scale in at.</param>
        public void SetScalingOutProperties(bool willScaleOutOnSelection, Vector2 onSelectionScaleOutTo, RatesOfChange scalingOutSpeed = RatesOfChange.X015, RatesOfChange scalingInSpeed = RatesOfChange.X015)
        {
            WillScaleOutOnSelection = willScaleOutOnSelection;
            OnSelectionScaleOutTo = onSelectionScaleOutTo;
            ScalingOutSpeed = scalingOutSpeed;
            ScalingInSpeed = scalingInSpeed;
        }

        /// <summary>
        /// Set Scaling In Properties.
        /// </summary>
        /// <param name="willScaleInOnSelection">A bool indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleInTo">A Vector2 indicating to what scale the UI element will scale in to.</param>
        /// <param name="scalingInSpeed">The value defining the rate of change the UI element will scale in at.</param>
        /// <param name="scalingOutSpeed">The value defining the rate of change the UI element will scale out at.</param>
        public void SetScalingInProperties(bool willScaleInOnSelection, Vector2 onSelectionScaleInTo, RatesOfChange scalingInSpeed = RatesOfChange.X015, RatesOfChange scalingOutSpeed = RatesOfChange.X015)
        {
            WillScaleOutOnSelection = willScaleInOnSelection;
            OnSelectionScaleOutTo = onSelectionScaleInTo;
            ScalingInSpeed = scalingOutSpeed;
            ScalingOutSpeed = scalingInSpeed;
        }

        /// <summary>
        /// Scale Out.
        /// Scales out to the target Scale.
        /// </summary>
        /// <param name="initialScale">The initial scale. This is used to calculate the rate of change in which to transition the scaling effect from the initial scale to the target scale. Intaken as a Vector2.</param>
        /// <param name="targetScale">The target Scale. Intaken as a Vector2.</param>
        /// <param name="scalingSpeed">The scaling speed. Default is X015.</param>
        public void ScaleOut(Vector2 initialScale, Vector2 targetScale, RatesOfChange scalingSpeed = RatesOfChange.X015)
        {
            if (targetScale.X > 0 ||
                targetScale.Y > 0)
            {
                var scale = Scale;

                if (IsScalingOut)
                {
                    scale.X = MathHelper.Clamp(scale.X + ScalingOutRateOfChange.X * (float)DeltaTime, scale.X, targetScale.X);
                    scale.Y = MathHelper.Clamp(scale.Y + ScalingOutRateOfChange.Y * (float)DeltaTime, scale.Y, targetScale.Y);

                    if (scale == targetScale)
                    {
                        IsScalingOut = false;
                    }
                }
                else if (scale != targetScale)
                {
                    ScalingOutRateOfChange = (targetScale - initialScale) / ((int)scalingSpeed / 100f);

                    IsScalingOut = true;
                }

                Scale = scale;
            }
        }

        /// <summary>
        /// Scale In.
        /// Scales in to the target Scale.
        /// </summary>
        /// <param name="initialScale">The initial scale. This is used to calculate the rate of change in which to transition the scaling effect from the initial scale to the target scale. Intaken as a Vector2.</param>
        /// <param name="targetScale">The target Scale. Intaken as a Vector2.</param>
        /// <param name="scalingSpeed">The scaling speed. Default is X015.</param>
        public void ScaleIn(Vector2 initialScale, Vector2 targetScale, RatesOfChange scalingSpeed = RatesOfChange.X015)
        {
            if (targetScale.X > 0 ||
                targetScale.Y > 0)
            {
                var scale = Scale;

                if (IsScalingIn)
                {
                    scale.X = MathHelper.Clamp(scale.X - ScalingInRateOfChange.X * (float)DeltaTime, targetScale.X, scale.X);
                    scale.Y = MathHelper.Clamp(scale.Y - ScalingInRateOfChange.Y * (float)DeltaTime, targetScale.Y, scale.Y);

                    if (scale == targetScale)
                    {
                        IsScalingIn = false;
                    }
                }
                else if (scale != targetScale)
                {
                    ScalingInRateOfChange = (initialScale - targetScale) / ((int)scalingSpeed / 100f);

                    IsScalingIn = true;
                }

                Scale = scale;
            }
        }

        #endregion

        #region Shifting Properties

        /// <summary>
        /// Will Shift Up On Selection?
        /// Set to true to enable shifting up on seelction.
        /// </summary>
        public bool WillShiftUpOnSelection { get; set; }

        /// <summary>
        /// Will Shift Right On Selection?
        /// Set to true to enable shifting right on seelction.
        /// </summary>
        public bool WillShiftRightOnSelection { get; set; }

        /// <summary>
        /// Will Shift Down On Selection?
        /// Set to true to enable shifting down on seelction.
        /// </summary>
        public bool WillShiftDownOnSelection { get; set; }

        /// <summary>
        /// Will Shift Left On Selection?
        /// Set to true to enable shifting left on seelction.
        /// </summary>
        public bool WillShiftLeftOnSelection { get; set; }

        /// <summary>
        /// Is Shifting Up?
        /// Indicates whether the object is shifting up.
        /// </summary>
        public bool IsShiftingUp { get; private set; }

        /// <summary>
        /// Is Shifting Right?
        /// Indicates whether the object is shifting right.
        /// </summary>
        public bool IsShiftingRight { get; private set; }

        /// <summary>
        /// Is Shifting Down?
        /// Indicates whether the object is shifting down.
        /// </summary>
        public bool IsShiftingDown { get; private set; }

        /// <summary>
        /// Is Shifting Left?
        /// Indicates whether the object is shifting left.
        /// </summary>
        public bool IsShiftingLeft { get; private set; }

        /// <summary>
        /// On Selection Shift Up By.
        /// Shift object this Vector2.
        /// </summary>
        public Vector2 OnSelectionShiftUpBy { get; set; }

        /// <summary>
        /// On Selection Shift Right By.
        /// Shift object to target position.
        /// </summary>
        public Vector2 OnSelectionShiftRightBy { get; set; }

        /// <summary>
        /// On Selection Shift Down By.
        /// Shift object to target position.
        /// </summary>
        public Vector2 OnSelectionShiftDownBy { get; set; }

        /// <summary>
        /// On Selection Shift Left By.
        /// Shift object to target position.
        /// </summary>
        public Vector2 OnSelectionShiftLeftBy { get; set; }

        /// <summary>
        /// Shifting Up Rate of Change.
        /// </summary>
        private Vector2 ShiftingUpRateOfChange { get; set; }

        /// <summary>
        /// Shifting Right Rate of Change.
        /// </summary>
        private Vector2 ShiftingRightRateOfChange { get; set; }

        /// <summary>
        /// Shifting Down Rate of Change.
        /// </summary>
        private Vector2 ShiftingDownRateOfChange { get; set; }

        /// <summary>
        /// Shifting Left Rate of Change.
        /// </summary>
        private Vector2 ShiftingLeftRateOfChange { get; set; }

        /// <summary>
        /// Shifting Up Speed.
        /// </summary>
        public RatesOfChange ShiftingUpSpeed { get; set; }

        /// <summary>
        /// Shifting Right Speed.
        /// </summary>
        public RatesOfChange ShiftingRightSpeed { get; set; }

        /// <summary>
        /// Shifting Down Speed.
        /// </summary>
        public RatesOfChange ShiftingDownSpeed { get; set; }

        /// <summary>
        /// Shifting Left Speed.
        /// </summary>
        public RatesOfChange ShiftingLeftSpeed { get; set; }

        #endregion

        #region Shifting Methods

        /// <summary>
        /// Set Shift Up Properties.
        /// </summary>
        /// <param name="willShiftUpOnSelection">A bool indicating whether the UI element will shift up when in focus.</param>
        /// <param name="onSelectionShiftUpBy">A Vector2 indicating by what value the UI element will shift up by.</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        public void SetShiftUpProperties(bool willShiftUpOnSelection, Vector2 onSelectionShiftUpBy, RatesOfChange shiftingUpSpeed = RatesOfChange.X015, RatesOfChange shiftingDownSpeed = RatesOfChange.X015)
        {
            WillShiftUpOnSelection = willShiftUpOnSelection;
            OnSelectionShiftUpBy = onSelectionShiftUpBy;
            ShiftingUpSpeed = shiftingUpSpeed;
            ShiftingDownSpeed = shiftingDownSpeed;
        }

        /// <summary>
        /// Set Shift Right Properties.
        /// </summary>
        /// <param name="willShiftRightOnSelection">A bool indicating whether the UI element will shift right when in focus.</param>
        /// <param name="onSelectionShiftRightBy">A Vector2 indicating by what value the UI element will shift right by.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at</param>
        public void SetShiftRightProperties(bool willShiftRightOnSelection, Vector2 onSelectionShiftRightBy, RatesOfChange shiftingRightSpeed = RatesOfChange.X015, RatesOfChange shiftingLeftSpeed = RatesOfChange.X015)
        {
            WillShiftRightOnSelection = willShiftRightOnSelection;
            OnSelectionShiftRightBy = onSelectionShiftRightBy;
            ShiftingRightSpeed = shiftingRightSpeed;
            ShiftingLeftSpeed = shiftingLeftSpeed;
        }

        /// <summary>
        /// Set Shift Down Properties.
        /// </summary>
        /// <param name="willShiftDownOnSelection">A bool indicating whether the UI element will shift down when in focus.</param>
        /// <param name="onSelectionShiftDownBy">A Vector2 indicating by what value the UI element will shift down by.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        public void SetShiftDownProperties(bool willShiftDownOnSelection, Vector2 onSelectionShiftDownBy, RatesOfChange shiftingDownSpeed = RatesOfChange.X015, RatesOfChange shiftingUpSpeed = RatesOfChange.X015)
        {
            WillShiftDownOnSelection = willShiftDownOnSelection;
            OnSelectionShiftDownBy = onSelectionShiftDownBy;
            ShiftingDownSpeed = shiftingDownSpeed;
            ShiftingUpSpeed = shiftingUpSpeed;
        }

        /// <summary>
        /// Set Shift Left Properties.
        /// </summary>
        /// <param name="willShiftLeftOnSelection">A bool indicating whether the UI element will shift left when in focus.</param>
        /// <param name="onSelectionShiftLeftBy">A Vector2 indicating by what value the UI element will shift left by.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at</param>
        public void SetShiftLeftProperties(bool willShiftLeftOnSelection, Vector2 onSelectionShiftLeftBy, RatesOfChange shiftingLeftSpeed = RatesOfChange.X015, RatesOfChange shiftingRightSpeed = RatesOfChange.X015)
        {
            WillShiftLeftOnSelection = willShiftLeftOnSelection;
            OnSelectionShiftLeftBy = onSelectionShiftLeftBy;
            ShiftingLeftSpeed = shiftingLeftSpeed;
            ShiftingRightSpeed = shiftingRightSpeed;
        }

        /// <summary>
        /// Shift Up.
        /// Shifts up to the target Position.
        /// </summary>
        /// <param name="initialPosition">The initial position. This is used to calculate the rate of change in which to transition the shift effect from the initial position to the target position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">The target Position. Intaken as a Vector2.</param>
        /// <param name="shiftSpeed">The shift speed. Default is X015.</param>
        public void ShiftUp(Vector2 initialPosition, Vector2 targetPosition, RatesOfChange shiftSpeed = RatesOfChange.X015)
        {
            var position = Position;

            if (IsShiftingUp)
            {
                position.Y = MathHelper.Clamp(position.Y - ShiftingUpRateOfChange.Y * (float)DeltaTime, targetPosition.Y, position.Y);

                if (position.Y <= targetPosition.Y)
                {
                    IsShiftingUp = false;
                }
            }
            else if (position.Y > targetPosition.Y)
            {
                ShiftingUpRateOfChange = (initialPosition - targetPosition) / ((int)shiftSpeed / 100f);

                IsShiftingUp = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Right.
        /// Shifts right to the target Position.
        /// </summary>
        /// <param name="initialPosition">The initial position. This is used to calculate the rate of change in which to transition the shift effect from the initial position to the target position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">The target Position. Intaken as a Vector2.</param>
        /// <param name="shiftSpeed">The shift speed. Default is X015.</param>
        public void ShiftRight(Vector2 initialPosition, Vector2 targetPosition, RatesOfChange shiftSpeed = RatesOfChange.X015)
        {
            var position = Position;

            if (IsShiftingRight)
            {
                position.X = MathHelper.Clamp(position.X + ShiftingRightRateOfChange.X * (float)DeltaTime, position.X, targetPosition.X);

                if (position.X >= targetPosition.X)
                {
                    IsShiftingRight = false;
                }
            }
            else if (position.X < targetPosition.X)
            {
                ShiftingRightRateOfChange = (targetPosition - initialPosition) / ((int)shiftSpeed / 100f);

                IsShiftingRight = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Down.
        /// Shifts down to the target Position.
        /// </summary>
        /// <param name="initialPosition">The initial position. This is used to calculate the rate of change in which to transition the shift effect from the initial position to the target position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">The target Position. Intaken as a Vector2.</param>
        /// <param name="shiftSpeed">The shift speed. Default is X015.</param>
        public void ShiftDown(Vector2 initialPosition, Vector2 targetPosition, RatesOfChange shiftSpeed = RatesOfChange.X015)
        {
            var position = Position;

            if (IsShiftingDown)
            {
                position.Y = MathHelper.Clamp(position.Y + ShiftingDownRateOfChange.Y * (float)DeltaTime, position.Y, targetPosition.Y);

                if (position.Y >= targetPosition.Y)
                {
                    IsShiftingDown = false;
                }
            }
            else if (position.Y < targetPosition.Y)
            {
                ShiftingDownRateOfChange = (targetPosition - initialPosition) / ((int)shiftSpeed / 100f);

                IsShiftingDown = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Left.
        /// Shifts left to the target Position.
        /// </summary>
        /// <param name="initialPosition">The initial position. This is used to calculate the rate of change in which to transition the shift effect from the initial position to the target position. Intaken as a Vector2.</param>
        /// <param name="targetPosition">The target Position. Intaken as a Vector2.</param>
        /// <param name="shiftSpeed">The shift speed. Default is X015.</param>
        public void ShiftLeft(Vector2 initialPosition, Vector2 targetPosition, RatesOfChange shiftSpeed = RatesOfChange.X015)
        {
            var position = Position;

            if (IsShiftingLeft)
            {
                position.X = MathHelper.Clamp(position.X - ShiftingLeftRateOfChange.X * (float)DeltaTime, targetPosition.X, position.X);

                if (position.X <= targetPosition.X)
                {
                    IsShiftingLeft = false;
                }
            }
            else if (position.X > targetPosition.X)
            {
                ShiftingLeftRateOfChange = (initialPosition - targetPosition) / ((int)shiftSpeed / 100f);

                IsShiftingLeft = true;
            }

            Position = position;
        }

        #endregion

        #region Padding Properties

        /// <summary>
        /// Padding Top.
        /// </summary>
        public int PaddingTop { get; set; }

        /// <summary>
        /// Padding Bottom.
        /// </summary>
        public int PaddingBottom { get; set; }

        /// <summary>
        /// Padding Left.
        /// </summary>
        public int PaddingLeft { get; set; }

        /// <summary>
        /// Padding Right.
        /// </summary>
        public int PaddingRight { get; set; }

        #endregion

        #region Padding Methods

        /// <summary>
        /// Set Padding.
        /// Padding is added on the inside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="allSides">The padding, in pixels, to add to the top, right, bottom and left sides.</param>
        public void SetPadding(int allSides)
        {
            if (allSides >= 0)
            {
                PaddingLeft = PaddingBottom = PaddingRight = PaddingTop = allSides;
            }
        }

        /// <summary>
        /// Set Padding.
        /// Padding is added on the inside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="topBottom">The padding, in pixels, to add to the top and bottom.</param>
        /// <param name="leftRight">The padding, in pixels, to add to the left and right.</param>
        public void SetPadding(int topBottom, int leftRight)
        {
            if (topBottom >= 0)
            {
                PaddingBottom = PaddingTop = topBottom;
            }

            if (leftRight >= 0)
            {
                PaddingRight = PaddingLeft = leftRight;
            }
        }

        /// <summary>
        /// Set Padding.
        /// Padding is added on the inside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="top">The padding, in pixels, to add to the top.</param>
        /// <param name="right">The padding, in pixels, to add to the right.</param>
        /// <param name="bottom">The padding, in pixels, to add to the bottom.</param>
        /// <param name="left">The padding, in pixels, to add to the left.</param>
        public void SetPadding(int top, int right, int bottom, int left)
        {
            if (top >= 0)
            {
                PaddingTop = top;
            }

            if (right >= 0)
            {
                PaddingRight = right;
            }

            if (bottom >= 0)
            {
                PaddingBottom = bottom;
            }

            if (left >= 0)
            {
                PaddingLeft = left;
            }
        }

        #endregion

        #region Margin Properties

        /// <summary>
        /// Margin Top.
        /// </summary>
        public int MarginTop { get; set; }

        /// <summary>
        /// Margin Bottom.
        /// </summary>
        public int MarginBottom { get; set; }

        /// <summary>
        /// Margin Left.
        /// </summary>
        public int MarginLeft { get; set; }

        /// <summary>
        /// Margin Right.
        /// </summary>
        public int MarginRight { get; set; }

        #endregion

        #region Margin Methods

        /// <summary>
        /// Set Margin.
        /// Margin is added on the outside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="allSides">The margin, in pixels, to add to the top, right, bottom and left sides.</param>
        public void SetMargin(int allSides)
        {
            if (allSides >= 0)
            {
                MarginLeft = MarginBottom = MarginRight = MarginTop = allSides;
            }
        }

        /// <summary>
        /// Set Margin.
        /// Margin is added on the outside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="topBottom">The margin, in pixels, to add to the top and bottom.</param>
        /// <param name="leftRight">The margin, in pixels, to add to the left and right.</param>
        public void SetMargin(int topBottom, int leftRight)
        {
            if (topBottom >= 0)
            {
                MarginBottom = MarginTop = topBottom;
            }

            if (leftRight >= 0)
            {
                MarginRight = MarginLeft = leftRight;
            }
        }

        /// <summary>
        /// Set Margin.
        /// Margin is added on the outside of the base and affects objects related to it's position/rectangle.
        /// </summary>
        /// <param name="top">The margin, in pixels, to add to the top.</param>
        /// <param name="right">The margin, in pixels, to add to the right.</param>
        /// <param name="bottom">The margin, in pixels, to add to the bottom.</param>
        /// <param name="left">The margin, in pixels, to add to the left.</param>
        public void SetMargin(int top, int right, int bottom, int left)
        {
            if (top >= 0)
            {
                MarginTop = top;
            }

            if (right >= 0)
            {
                MarginRight = right;
            }

            if (bottom >= 0)
            {
                MarginBottom = bottom;
            }

            if (left >= 0)
            {
                MarginLeft = left;
            }
        }

        #endregion

        #region Effects Properties

        /// <summary>
        /// Loaded Effects.
        /// </summary>
        private Dictionary<string, UIBaseEffect> LoadedEffects { get; }

        /// <summary>
        /// Effects.
        /// </summary>
        private List<UIBaseEffect> ActiveEffects { get; }

        /// <summary>
        /// Are Effects Running?
        /// </summary>
        public bool AreEffectsRunning { get; protected set; }

        /// <summary>
        /// Activate Effects.
        /// Call to perform any Effects that have been loaded.
        /// </summary>
        public bool ActivateEffects { get; set; }

        /// <summary>
        /// Run Effects Sequencially.
        /// </summary>
        public bool RunEffectsSequencially { get; set; }

        #endregion

        #region Effects Methods

        /// <summary>
        /// Load Effect.
        /// Adds the provided Effect to the UIBase's Loaded Effects Dictionary and modifying the Order Number to be equal to the number of current effects, if Order Number is found to be 0.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to run. Intaken as a string.</param>
        /// <param name="effect">The Effect to be loaded.</param>
        /// <returns>Returns a bool indicating whether the Effect was added.</returns>
        public bool LoadEffect(string identifier, UIBaseEffect effect)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier) == false)
            {
                if (effect.OrderNumber == 0)
                {
                    effect.OrderNumber = ActiveEffects.Count + 1;
                }

                LoadedEffects.Add(identifier, effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to retrieve. Intaken as a string.</param>
        /// <returns>Returns a Effect, if found, otherwise null.</returns>
        public UIBaseEffect GetLoadedEffect(string identifier)
        {
            UIBaseEffect result = null;

            if (CheckForLoadedEffect(identifier))
            {
                result = LoadedEffects[identifier];
            }

            return result;
        }

        /// <summary>
        /// Check For Loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the Effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect has been loaded.</returns>
        public bool CheckForLoadedEffect(string identifier)
        {
            return LoadedEffects.ContainsKey(identifier);
        }

        /// <summary>
        /// Remove Effect.
        /// Removes the Effect from Loaded Effects using the provided identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to remove. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect was removed.</returns>
        public bool RemoveEffect(string identifier)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier))
            {
                result = LoadedEffects.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Check For Activated Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the Effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect is currently active.</returns>
        public bool CheckForActivatedEffect(string identifier)
        {
            return ActiveEffects.Contains(GetLoadedEffect(identifier));
        }

        /// <summary>
        /// Activate Loaded Effect.
        /// Called to activate a loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to activate. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect was activated.</returns>
        public bool ActivateLoadedEffect(string identifier)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier))
            {
                ActiveEffects.Add(GetLoadedEffect(identifier));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Activate Immediate Effect.
        /// Called to activate the passed effect immediately without storing it for reuse.
        /// </summary>
        /// <param name="effect">The UIBaseEffect object to activate immediately.</param>
        /// <returns>Returns a boolean indicating whether the effect was activated.</returns>
        public bool ActivateImmediateEffect(UIBaseEffect effect)
        {
            var result = false;

            if (effect != null)
            {
                ActiveEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Run Effects.
        /// Called to run all loaded Effects by ascending Order Number.
        /// </summary>
        /// <param name="inSequentialOrder">Indicates whether the Effects will run sequentially or all at once. Intaken as a bool. Default is true.</param>
        /// <returns>Returns a bool indicating whether all of the loaded transitions completed.</returns>
        public async Task<bool> RunActiveEffects(bool inSequentialOrder = true)
        {
            var result = false;

            if (inSequentialOrder)
            {
                if (ActiveEffects.Count > 0)
                {
                    var currentEffect = ActiveEffects[0];
                    if (await currentEffect.Run())
                    {
                        currentEffect.Reset();
                        ActiveEffects.Remove(currentEffect);
                        AreEffectsRunning = false;
                    }
                    else
                    {
                        AreEffectsRunning = true;
                    }
                }
            }
            else
            {
                for (var index = 0; index < ActiveEffects.Count; index++)
                {
                    var activeEffect = ActiveEffects[index];

                    if (await activeEffect.Run())
                    {
                        activeEffect.Reset();
                        ActiveEffects.Remove(activeEffect);
                        AreEffectsRunning = false;
                    }
                    else
                    {
                        AreEffectsRunning = true;
                    }
                }
            }

            if (ActiveEffects.Count == 0)
            {
                ActivateEffects = false;
                result = true;
            }

            return result;
        }

        #endregion

        #region Effect Triggers

        /// <summary>
        /// Trigger Background Color Gradiant.
        /// When the condition is met a UIEffectBackgroundColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to trigger the color change.</param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="initialColor">The initial Color.</param>
        /// <param name="targetColor">The target Color.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="storeEffect">A boolean indicating whether to store the effect for reuse. False will load then discard a new effect.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool TriggerBackgroundColorGradiant(bool condition, string triggerEffectIdentifier, Color initialColor, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0, bool storeEffect = true)
        {
            if (condition)
            {
                if (storeEffect)
                {
                    if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        LoadEffect(triggerEffectIdentifier, new UIEffectBackgroundColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    ActivateImmediateEffect(new UIEffectBackgroundColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                }
            }

            return condition;
        }

        /// <summary>
        /// Trigger Outline Color Gradiant.
        /// When the condition is met a UIEffectOutlineColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to trigger the color change.</param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="initialColor">The initial Color.</param>
        /// <param name="targetColor">The target Color.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="storeEffect">A boolean indicating whether to store the effect for reuse. False will load then discard a new effect.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool TriggerOutlineColorGradiant(bool condition, string triggerEffectIdentifier, Color initialColor, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0, bool storeEffect = true)
        {
            if (condition)
            {
                if (storeEffect)
                {
                    if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        LoadEffect(triggerEffectIdentifier, new UIEffectOutlineColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    ActivateImmediateEffect(new UIEffectOutlineColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                }
            }

            return condition;
        }

        /// <summary>
        /// Trigger Highlight Color Gradiant.
        /// When the condition is met a UIEffectHighlightColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to trigger the color change.</param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="initialColor">The initial Color.</param>
        /// <param name="targetColor">The target Color.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <param name="storeEffect">A boolean indicating whether to store the effect for reuse. False will load then discard a new effect.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool TriggerHighlightColorGradiant(bool condition, string triggerEffectIdentifier, Color initialColor, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0, bool storeEffect = true)
        {
            if (condition)
            {
                if (storeEffect)
                {
                    if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        LoadEffect(triggerEffectIdentifier, new UIEffectHighlightColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    ActivateImmediateEffect(new UIEffectHighlightColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                }
            }

            return condition;
        }
        
        #endregion
    }
}