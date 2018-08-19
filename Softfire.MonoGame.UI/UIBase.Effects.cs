using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.UI.Effects;
using Softfire.MonoGame.UI.Effects.Highlighting;
using Softfire.MonoGame.UI.Effects.Transitions;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UIBase Class.
    /// </summary>
    public abstract partial class UIBase
    {
        #region Scaling Properties

        /// <summary>
        /// Is Currently Scaling Out?
        /// Indicates whether the object is scaling out.
        /// </summary>
        public bool IsCurrentlyScalingOut { get; private set; }

        /// <summary>
        /// Is Currently Scaling In?
        /// Indicates whether the object is scaling in.
        /// </summary>
        public bool IsCurrentlyScalingIn { get; private set; }

        /// <summary>
        /// Enable Scale Out On Selection?
        /// Set to true to enable scaling out on selection.
        /// </summary>
        private bool EnableScaleOutOnSelection { get; set; }

        /// <summary>
        /// Enable Scale In On Selection?
        /// Set to true to enable scaling in on selection.
        /// </summary>
        private bool EnableScaleInOnSelection { get; set; }

        /// <summary>
        /// On Selection Scale Out By.
        /// </summary>
        private Vector2 OnSelectionScaleOutBy { get; set; } = Vector2.One;

        /// <summary>
        /// On Selection Scale In By.
        /// </summary>
        private Vector2 OnSelectionScaleInBy { get; set; } = Vector2.One;

        /// <summary>
        /// Scaling Out Rate of Change.
        /// </summary>
        private Vector2 ScalingOutRateOfChange { get; set; }

        /// <summary>
        /// Scaling In Rate of Change.
        /// </summary>
        private Vector2 ScalingInRateOfChange { get; set; }

        /// <summary>
        /// Scaling Out Speed.
        /// </summary>
        private double ScalingOutSpeed { get; set; } = 15D;

        /// <summary>
        /// Scaling In Speed.
        /// </summary>
        private double ScalingInSpeed { get; set; } = 15D;

        #endregion

        #region Scaling Methods

        /// <summary>
        /// Set Scaling Out Properties.
        /// </summary>
        /// <param name="enableScaleOutOnSelection">A bool indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleOutBy">A Vector2 indicating to what scale the UI element will scale out to.</param>
        /// <param name="scalingOutSpeed">The value defining the rate of change the UI element will scale out at.</param>
        /// <param name="scalingInSpeed">The value defining the rate of change the UI element will scale in at.</param>
        public void SetScalingOutProperties(bool enableScaleOutOnSelection, Vector2 onSelectionScaleOutBy, double scalingOutSpeed = 15D, double scalingInSpeed = 15D)
        {
            EnableScaleOutOnSelection = enableScaleOutOnSelection;
            OnSelectionScaleOutBy = onSelectionScaleOutBy;
            ScalingOutSpeed = scalingOutSpeed;
            ScalingInSpeed = scalingInSpeed;
        }

        /// <summary>
        /// Set Scaling In Properties.
        /// </summary>
        /// <param name="enableScaleInOnSelection">A bool indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleInBy">A Vector2 indicating to what scale the UI element will scale in by.</param>
        /// <param name="scalingInSpeed">The value defining the rate of change the UI element will scale in at.</param>
        /// <param name="scalingOutSpeed">The value defining the rate of change the UI element will scale out at.</param>
        public void SetScalingInProperties(bool enableScaleInOnSelection, Vector2 onSelectionScaleInBy, double scalingInSpeed = 15D, double scalingOutSpeed = 15D)
        {
            EnableScaleInOnSelection = enableScaleInOnSelection;
            OnSelectionScaleInBy = onSelectionScaleInBy;
            ScalingInSpeed = scalingInSpeed;
            ScalingOutSpeed = scalingOutSpeed;
        }

        /// <summary>
        /// Scale Out.
        /// Scales out to the target Scale.
        /// </summary>
        /// <param name="initialScale">The initial scale. This is used to calculate the rate of change in which to transition the scaling effect from the initial scale to the target scale. Intaken as a Vector2.</param>
        /// <param name="targetScale">The target Scale. Intaken as a Vector2.</param>
        /// <param name="scalingSpeed">The scaling speed. Default is 15D.</param>
        private void ScaleOut()
        {
            var initialScale = Defaults.Scale;
            var targetScale = OnSelectionScaleOutBy;

            if (!EnableScaleOutOnSelection)
            {
                initialScale = OnSelectionScaleOutBy;
                targetScale = Defaults.Scale;
            }

            if (targetScale.X > 0 ||
                targetScale.Y > 0)
            {
                var scale = Scale;

                if (IsCurrentlyScalingOut)
                {
                    scale.X = MathHelper.Clamp(scale.X + ScalingOutRateOfChange.X * (float)DeltaTime, scale.X, targetScale.X);
                    scale.Y = MathHelper.Clamp(scale.Y + ScalingOutRateOfChange.Y * (float)DeltaTime, scale.Y, targetScale.Y);

                    if (scale == targetScale)
                    {
                        IsCurrentlyScalingOut = false;
                    }
                }
                else if (scale != targetScale)
                {
                    ScalingOutRateOfChange = (targetScale - initialScale) / ((int)ScalingOutSpeed / 100f);

                    IsCurrentlyScalingOut = true;
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
        /// <param name="scalingSpeed">The scaling speed. Default is 15D.</param>
        private void ScaleIn()
        {
            var initialScale = Defaults.Scale;
            var targetScale = OnSelectionScaleInBy;

            if (!EnableScaleInOnSelection)
            {
                initialScale = OnSelectionScaleInBy;
                targetScale = Defaults.Scale;
            }

            if (targetScale.X > 0 ||
                targetScale.Y > 0)
            {
                var scale = Scale;

                if (IsCurrentlyScalingIn)
                {
                    scale.X = MathHelper.Clamp(scale.X - ScalingInRateOfChange.X * (float)DeltaTime, targetScale.X, scale.X);
                    scale.Y = MathHelper.Clamp(scale.Y - ScalingInRateOfChange.Y * (float)DeltaTime, targetScale.Y, scale.Y);

                    if (scale == targetScale)
                    {
                        IsCurrentlyScalingIn = false;
                    }
                }
                else if (scale != targetScale)
                {
                    ScalingInRateOfChange = (initialScale - targetScale) / ((int)ScalingInSpeed / 100f);

                    IsCurrentlyScalingIn = true;
                }

                Scale = scale;
            }
        }

        #endregion

        #region Shifting Properties

        /// <summary>
        /// Is Currently Shifting Up?
        /// Indicates whether the object is shifting up.
        /// </summary>
        public bool IsCurrentlyShiftingUp { get; private set; }

        /// <summary>
        /// Is Currently Shifting Right?
        /// Indicates whether the object is shifting right.
        /// </summary>
        public bool IsCurrentlyShiftingRight { get; private set; }

        /// <summary>
        /// Is Currently Shifting Down?
        /// Indicates whether the object is shifting down.
        /// </summary>
        public bool IsCurrentlyShiftingDown { get; private set; }

        /// <summary>
        /// Is Currently Shifting Left?
        /// Indicates whether the object is shifting left.
        /// </summary>
        public bool IsCurrentlyShiftingLeft { get; private set; }

        /// <summary>
        /// Enable Shift Up On Selection?
        /// Set to true to enable shifting up on selection.
        /// </summary>
        private bool EnableShiftUpOnSelection { get; set; }

        /// <summary>
        /// Enable Shift Right On Selection?
        /// Set to true to enable shifting right on selection.
        /// </summary>
        private bool EnableShiftRightOnSelection { get; set; }

        /// <summary>
        /// Enable Shift Down On Selection?
        /// Set to true to enable shifting down on selection.
        /// </summary>
        private bool EnableShiftDownOnSelection { get; set; }

        /// <summary>
        /// Enable Shift Left On Selection?
        /// Set to true to enable shifting left on seelction.
        /// </summary>
        private bool EnableShiftLeftOnSelection { get; set; }

        /// <summary>
        /// On Selection Shift Up By.
        /// </summary>
        private Vector2 OnSelectionShiftUpBy { get; set; } = Vector2.Zero;

        /// <summary>
        /// On Selection Shift Right By.
        /// </summary>
        private Vector2 OnSelectionShiftRightBy { get; set; } = Vector2.Zero;

        /// <summary>
        /// On Selection Shift Down By.
        /// </summary>
        private Vector2 OnSelectionShiftDownBy { get; set; } = Vector2.Zero;

        /// <summary>
        /// On Selection Shift Left By.
        /// </summary>
        private Vector2 OnSelectionShiftLeftBy { get; set; } = Vector2.Zero;

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
        private double ShiftingUpSpeed { get; set; } = 15D;

        /// <summary>
        /// Shifting Right Speed.
        /// </summary>
        private double ShiftingRightSpeed { get; set; } = 15D;

        /// <summary>
        /// Shifting Down Speed.
        /// </summary>
        private double ShiftingDownSpeed { get; set; } = 15D;

        /// <summary>
        /// Shifting Left Speed.
        /// </summary>
        private double ShiftingLeftSpeed { get; set; } = 15D;

        #endregion

        #region Shifting Methods

        /// <summary>
        /// Set Shift Up Properties.
        /// </summary>
        /// <param name="enableShiftUpOnSelection">A bool indicating whether the UI element will shift up when in focus.</param>
        /// <param name="onSelectionShiftUpBy">A Vector2 indicating by what value the UI element will shift up by.</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        public void SetShiftUpProperties(bool enableShiftUpOnSelection, Vector2 onSelectionShiftUpBy, double shiftingUpSpeed = 15D, double shiftingDownSpeed = 15D)
        {
            EnableShiftUpOnSelection = enableShiftUpOnSelection;
            OnSelectionShiftUpBy = onSelectionShiftUpBy;
            ShiftingUpSpeed = shiftingUpSpeed;
            ShiftingDownSpeed = shiftingDownSpeed;
        }

        /// <summary>
        /// Set Shift Right Properties.
        /// </summary>
        /// <param name="enableShiftRightOnSelection">A bool indicating whether the UI element will shift right when in focus.</param>
        /// <param name="onSelectionShiftRightBy">A Vector2 indicating by what value the UI element will shift right by.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at</param>
        public void SetShiftRightProperties(bool enableShiftRightOnSelection, Vector2 onSelectionShiftRightBy, double shiftingRightSpeed = 15D, double shiftingLeftSpeed = 15D)
        {
            EnableShiftRightOnSelection = enableShiftRightOnSelection;
            OnSelectionShiftRightBy = onSelectionShiftRightBy;
            ShiftingRightSpeed = shiftingRightSpeed;
            ShiftingLeftSpeed = shiftingLeftSpeed;
        }

        /// <summary>
        /// Set Shift Down Properties.
        /// </summary>
        /// <param name="enableShiftDownOnSelection">A bool indicating whether the UI element will shift down when in focus.</param>
        /// <param name="onSelectionShiftDownBy">A Vector2 indicating by what value the UI element will shift down by.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        public void SetShiftDownProperties(bool enableShiftDownOnSelection, Vector2 onSelectionShiftDownBy, double shiftingDownSpeed = 15D, double shiftingUpSpeed = 15D)
        {
            EnableShiftDownOnSelection = enableShiftDownOnSelection;
            OnSelectionShiftDownBy = onSelectionShiftDownBy;
            ShiftingDownSpeed = shiftingDownSpeed;
            ShiftingUpSpeed = shiftingUpSpeed;
        }

        /// <summary>
        /// Set Shift Left Properties.
        /// </summary>
        /// <param name="enableShiftLeftOnSelection">A bool indicating whether the UI element will shift left when in focus.</param>
        /// <param name="onSelectionShiftLeftBy">A Vector2 indicating by what value the UI element will shift left by.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at</param>
        public void SetShiftLeftProperties(bool enableShiftLeftOnSelection, Vector2 onSelectionShiftLeftBy, double shiftingLeftSpeed = 15D, double shiftingRightSpeed = 15D)
        {
            EnableShiftLeftOnSelection = enableShiftLeftOnSelection;
            OnSelectionShiftLeftBy = onSelectionShiftLeftBy;
            ShiftingLeftSpeed = shiftingLeftSpeed;
            ShiftingRightSpeed = shiftingRightSpeed;
        }

        /// <summary>
        /// Shift Up.
        /// </summary>
        private void ShiftUp()
        {
            var position = Position;
            var targetPosition = Defaults.Position;

            if (!EnableShiftUpOnSelection)
            {
                targetPosition += OnSelectionShiftUpBy;
            }

            if (IsCurrentlyShiftingUp)
            {
                position.Y = MathHelper.Clamp(position.Y - ShiftingUpRateOfChange.Y * (float)DeltaTime, targetPosition.Y, position.Y);

                if (position.Y <= targetPosition.Y)
                {
                    IsCurrentlyShiftingUp = false;
                }
            }
            else if (position.Y > targetPosition.Y)
            {
                ShiftingUpRateOfChange = (Defaults.Position - targetPosition) / ((int)ShiftingUpSpeed / 100f);

                IsCurrentlyShiftingUp = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Right.
        /// </summary>
        private void ShiftRight()
        {
            var position = Position;
            var targetPosition = Defaults.Position;

            if (!EnableShiftRightOnSelection)
            {
                targetPosition += OnSelectionShiftRightBy;
            }

            if (IsCurrentlyShiftingRight)
            {
                position.X = MathHelper.Clamp(position.X + ShiftingRightRateOfChange.X * (float)DeltaTime, position.X, targetPosition.X);

                if (position.X >= targetPosition.X)
                {
                    IsCurrentlyShiftingRight = false;
                }
            }
            else if (position.X < targetPosition.X)
            {
                ShiftingRightRateOfChange = (targetPosition - Defaults.Position) / ((int)ShiftingRightSpeed / 100f);

                IsCurrentlyShiftingRight = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Down.
        /// </summary>
        private void ShiftDown()
        {
            var position = Position;
            var targetPosition = Defaults.Position;

            if (!EnableShiftDownOnSelection)
            {
                targetPosition += OnSelectionShiftDownBy;
            }

            if (IsCurrentlyShiftingDown)
            {
                position.Y = MathHelper.Clamp(position.Y + ShiftingDownRateOfChange.Y * (float)DeltaTime, position.Y, targetPosition.Y);

                if (position.Y >= targetPosition.Y)
                {
                    IsCurrentlyShiftingDown = false;
                }
            }
            else if (position.Y < targetPosition.Y)
            {
                ShiftingDownRateOfChange = (targetPosition - Defaults.Position) / ((int)ShiftingDownSpeed / 100f);

                IsCurrentlyShiftingDown = true;
            }

            Position = position;
        }

        /// <summary>
        /// Shift Left.
        /// </summary>
        private void ShiftLeft()
        {
            var position = Position;
            var targetPosition = Defaults.Position;

            if (!EnableShiftLeftOnSelection)
            {
                targetPosition += OnSelectionShiftLeftBy;
            }

            if (IsCurrentlyShiftingLeft)
            {
                position.X = MathHelper.Clamp(position.X - ShiftingLeftRateOfChange.X * (float)DeltaTime, targetPosition.X, position.X);

                if (position.X <= targetPosition.X)
                {
                    IsCurrentlyShiftingLeft = false;
                }
            }
            else if (position.X > targetPosition.X)
            {
                ShiftingLeftRateOfChange = (Defaults.Position - targetPosition) / ((int)ShiftingLeftSpeed / 100f);

                IsCurrentlyShiftingLeft = true;
            }

            Position = position;
        }

        #endregion

        #region Padding Properties

        /// <summary>
        /// Padding Top.
        /// </summary>
        public int PaddingTop { get; private set; }

        /// <summary>
        /// Padding Bottom.
        /// </summary>
        public int PaddingBottom { get; private set; }

        /// <summary>
        /// Padding Left.
        /// </summary>
        public int PaddingLeft { get; private set; }

        /// <summary>
        /// Padding Right.
        /// </summary>
        public int PaddingRight { get; private set; }

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
        public int MarginTop { get; private set; }

        /// <summary>
        /// Margin Bottom.
        /// </summary>
        public int MarginBottom { get; private set; }

        /// <summary>
        /// Margin Left.
        /// </summary>
        public int MarginLeft { get; private set; }

        /// <summary>
        /// Margin Right.
        /// </summary>
        public int MarginRight { get; private set; }

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
                    if (UIEffectsManager.CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.LoadEffect(triggerEffectIdentifier, new UIEffectBackgroundColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (UIEffectsManager.CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    UIEffectsManager.ActivateImmediateEffect(new UIEffectBackgroundColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
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
                    if (UIEffectsManager.CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.LoadEffect(triggerEffectIdentifier, new UIEffectOutlineColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (UIEffectsManager.CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    UIEffectsManager.ActivateImmediateEffect(new UIEffectOutlineColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
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
                    if (UIEffectsManager.CheckForLoadedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.LoadEffect(triggerEffectIdentifier, new UIEffectHighlightColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                    }

                    if (UIEffectsManager.CheckForActivatedEffect(triggerEffectIdentifier) == false)
                    {
                        UIEffectsManager.ActivateLoadedEffect(triggerEffectIdentifier);
                    }
                }
                else
                {
                    UIEffectsManager.ActivateImmediateEffect(new UIEffectHighlightColorGradiant(this, initialColor, targetColor, durationInSeconds, startDelayInSeconds));
                }
            }

            return condition;
        }
        
        #endregion
    }
}