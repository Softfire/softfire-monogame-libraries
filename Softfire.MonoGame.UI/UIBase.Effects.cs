using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UI Base Class.
    /// </summary>
    public abstract partial class UIBase
    {
        #region Scaling Properties

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
        /// On Selection Scale Out To.
        /// </summary>
        private Vector2 OnSelectionScaleOutTo { get; set; }

        /// <summary>
        /// On Selection Scale In To.
        /// </summary>
        private Vector2 OnSelectionScaleInTo { get; set; }

        /// <summary>
        /// Scaling Out Rate of Change.
        /// </summary>
        private Vector2 ScalingOutRateOfChange { get; set; }

        /// <summary>
        /// Scaling In Rate of Change.
        /// </summary>
        private Vector2 ScalingInRateOfChange { get; set; }

        #endregion

        #region Scaling Methods

        /// <summary>
        /// Set Scaling Out Properties.
        /// </summary>
        /// <param name="enableScaleOutOnSelection">A boolean indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleOutTo">A Vector2 indicating to what scale the UI element will scale out to.</param>
        /// <param name="scalingOutSpeed">The value, in milliseconds, defining the rate of change the UI element will scale out at.</param>
        /// <param name="scalingInSpeed">The value, in milliseconds, defining the rate of change the UI element will scale in at.</param>
        public void SetScalingOutProperties(bool enableScaleOutOnSelection, Vector2 onSelectionScaleOutTo, float scalingOutSpeed = 1000f, float scalingInSpeed = 1000f)
        {
            EnableScaleOutOnSelection = enableScaleOutOnSelection;
            OnSelectionScaleOutTo = onSelectionScaleOutTo;
            ScalingOutRateOfChange = (OnSelectionScaleOutTo - Defaults.Scale) / (scalingOutSpeed / 1000f);

            OnSelectionScaleInTo = Defaults.Scale;
            ScalingInRateOfChange = (OnSelectionScaleOutTo - Defaults.Scale) / (scalingInSpeed / 1000f);
        }

        /// <summary>
        /// Set Scaling In Properties.
        /// </summary>
        /// <param name="enableScaleInOnSelection">A boolean indicating whether the UI element will scale out when in focus.</param>
        /// <param name="onSelectionScaleInTo">A Vector2 indicating to what scale the UI element will scale in to.</param>
        /// <param name="scalingInSpeed">The value, in milliseconds, defining the rate of change the UI element will scale in at.</param>
        /// <param name="scalingOutSpeed">The value, in milliseconds, defining the rate of change the UI element will scale out at.</param>
        public void SetScalingInProperties(bool enableScaleInOnSelection, Vector2 onSelectionScaleInTo, float scalingInSpeed = 1000f, float scalingOutSpeed = 1000f)
        {
            EnableScaleInOnSelection = enableScaleInOnSelection;
            OnSelectionScaleInTo = onSelectionScaleInTo;
            ScalingInRateOfChange = (Defaults.Scale - OnSelectionScaleInTo) / (scalingInSpeed / 1000f);

            OnSelectionScaleOutTo = Defaults.Scale;
            ScalingOutRateOfChange = (Defaults.Scale - OnSelectionScaleInTo) / (scalingOutSpeed / 1000f);
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
            if (OnSelectionScaleOutTo.X > 0 ||
                OnSelectionScaleOutTo.Y > 0)
            {
                var scale = Scale;
                scale.X = MathHelper.Clamp(scale.X + ScalingOutRateOfChange.X * (float)DeltaTime, scale.X, OnSelectionScaleOutTo.X);
                scale.Y = MathHelper.Clamp(scale.Y + ScalingOutRateOfChange.Y * (float)DeltaTime, scale.Y, OnSelectionScaleOutTo.Y);
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
            if (OnSelectionScaleInTo.X > 0 ||
                OnSelectionScaleInTo.Y > 0)
            {
                var scale = Scale;
                scale.X = MathHelper.Clamp(scale.X - ScalingInRateOfChange.X * (float)DeltaTime, OnSelectionScaleInTo.X, scale.X);
                scale.Y = MathHelper.Clamp(scale.Y - ScalingInRateOfChange.Y * (float)DeltaTime, OnSelectionScaleInTo.Y, scale.Y);
                Scale = scale;
            }
        }

        #endregion

        #region Shifting Properties

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

        #endregion

        #region Shifting Methods

        /// <summary>
        /// Set Shift Up Properties.
        /// </summary>
        /// <param name="enableShiftUpOnSelection">A bool indicating whether the UI element will shift up when in focus.</param>
        /// <param name="onSelectionShiftUpBy">A Vector2 indicating by what value the UI element will shift up by.</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        public void SetShiftUpProperties(bool enableShiftUpOnSelection, Vector2 onSelectionShiftUpBy, float shiftingUpSpeed = 1000f, float shiftingDownSpeed = 1000f)
        {
            EnableShiftUpOnSelection = enableShiftUpOnSelection;
            OnSelectionShiftDownBy = OnSelectionShiftUpBy = onSelectionShiftUpBy;
            ShiftingUpRateOfChange = OnSelectionShiftUpBy / (shiftingUpSpeed / 1000f);
            ShiftingDownRateOfChange = OnSelectionShiftUpBy / (shiftingDownSpeed / 1000f);
        }

        /// <summary>
        /// Set Shift Right Properties.
        /// </summary>
        /// <param name="enableShiftRightOnSelection">A bool indicating whether the UI element will shift right when in focus.</param>
        /// <param name="onSelectionShiftRightBy">A Vector2 indicating by what value the UI element will shift right by.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at</param>
        public void SetShiftRightProperties(bool enableShiftRightOnSelection, Vector2 onSelectionShiftRightBy, float shiftingRightSpeed = 1000f, float shiftingLeftSpeed = 1000f)
        {
            EnableShiftRightOnSelection = enableShiftRightOnSelection;
            OnSelectionShiftRightBy = onSelectionShiftRightBy;
            ShiftingRightRateOfChange = (Defaults.Position - OnSelectionShiftRightBy) / (shiftingRightSpeed / 1000f);

            OnSelectionShiftLeftBy = Defaults.Position;
            ShiftingLeftRateOfChange = (Defaults.Position - OnSelectionShiftLeftBy) / (shiftingLeftSpeed / 1000f);
        }

        /// <summary>
        /// Set Shift Down Properties.
        /// </summary>
        /// <param name="enableShiftDownOnSelection">A bool indicating whether the UI element will shift down when in focus.</param>
        /// <param name="onSelectionShiftDownBy">A Vector2 indicating by what value the UI element will shift down by.</param>
        /// <param name="shiftingDownSpeed">The value defining the rate of change the UI element will shift down at</param>
        /// <param name="shiftingUpSpeed">The value defining the rate of change the UI element will shift up at.</param>
        public void SetShiftDownProperties(bool enableShiftDownOnSelection, Vector2 onSelectionShiftDownBy, float shiftingDownSpeed = 1000f, float shiftingUpSpeed = 1000f)
        {
            EnableShiftDownOnSelection = enableShiftDownOnSelection;
            OnSelectionShiftUpBy = OnSelectionShiftDownBy = onSelectionShiftDownBy;
            ShiftingDownRateOfChange = (Defaults.Position - OnSelectionShiftDownBy) / (shiftingDownSpeed / 1000f);
            ShiftingUpRateOfChange = (Defaults.Position - OnSelectionShiftDownBy) / (shiftingUpSpeed / 1000f);
        }

        /// <summary>
        /// Set Shift Left Properties.
        /// </summary>
        /// <param name="enableShiftLeftOnSelection">A bool indicating whether the UI element will shift left when in focus.</param>
        /// <param name="onSelectionShiftLeftBy">A Vector2 indicating by what value the UI element will shift left by.</param>
        /// <param name="shiftingLeftSpeed">The value defining the rate of change the UI element will shift left at.</param>
        /// <param name="shiftingRightSpeed">The value defining the rate of change the UI element will shift right at</param>
        public void SetShiftLeftProperties(bool enableShiftLeftOnSelection, Vector2 onSelectionShiftLeftBy, float shiftingLeftSpeed = 1000f, float shiftingRightSpeed = 1000f)
        {
            EnableShiftLeftOnSelection = enableShiftLeftOnSelection;
            OnSelectionShiftLeftBy = onSelectionShiftLeftBy;
            ShiftingLeftRateOfChange = (Defaults.Position - OnSelectionShiftLeftBy) / (shiftingLeftSpeed / 1000f);

            OnSelectionShiftRightBy = Defaults.Position;
            ShiftingRightRateOfChange = (Defaults.Position - OnSelectionShiftRightBy) / (shiftingRightSpeed / 1000f);
        }

        /// <summary>
        /// Shift Up.
        /// </summary>
        private void ShiftUp()
        {
            var position = Position;
            position.Y = MathHelper.Clamp(position.Y - ShiftingUpRateOfChange.Y * (float)DeltaTime, -OnSelectionShiftUpBy.Y, OnSelectionShiftUpBy.Y);
            Position = position;
        }

        /// <summary>
        /// Shift Right.
        /// </summary>
        private void ShiftRight()
        {
            var position = Position;
            position.X = MathHelper.Clamp(position.X + ShiftingRightRateOfChange.X * (float)DeltaTime, -OnSelectionShiftRightBy.X, OnSelectionShiftRightBy.X);
            Position = position;
        }

        /// <summary>
        /// Shift Down.
        /// </summary>
        private void ShiftDown()
        {
            var position = Position;
            position.Y = MathHelper.Clamp(position.Y + ShiftingDownRateOfChange.Y * (float)DeltaTime, -OnSelectionShiftDownBy.Y, OnSelectionShiftDownBy.Y);
            Position = position;
        }

        /// <summary>
        /// Shift Left.
        /// </summary>
        private void ShiftLeft()
        {
            var position = Position;
            position.X = MathHelper.Clamp(position.X - ShiftingLeftRateOfChange.X * (float)DeltaTime, -OnSelectionShiftLeftBy.X, OnSelectionShiftLeftBy.X);
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
    }
}