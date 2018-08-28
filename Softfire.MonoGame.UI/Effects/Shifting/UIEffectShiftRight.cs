using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Effects.Shifting
{
    /// <summary>
    /// An effect to shift the UI to the right along the X axis from it's current position.
    /// </summary>
    public class UIEffectShiftRight : UIEffectBase
    {
        /// <summary>
        /// Distance to shift along the X axis.
        /// </summary>
        private Vector2 ShiftVector { get; }

        /// <summary>
        /// The initial position to shift from.
        /// </summary>
        private Vector2 InitialPosition { get; set; }

        /// <summary>
        /// An effect that shifts the UI to the right along the X axis.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="shiftVector">The shift vector (X) to offset the UI by over the duration of time provided. Intaken as a Vector2.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        /// <see cref="SetStartPosition()"/>
        public UIEffectShiftRight(UIBase uiBase, int id, string name, Vector2 shiftVector,
                                  float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0) : base(uiBase, id, name, durationInSeconds, startDelayInSeconds, orderNumber)
        {
            ShiftVector = shiftVector;
            RateOfChange = ShiftVector.X / DurationInSeconds;

            // Sets the initial start position.
            SetStartPosition();
        }

        /// <summary>
        /// Sets the starting position to the UIBase's Position.
        /// </summary>
        /// <remarks>Call this method when you want to begin the shift from the UI's current position.</remarks>
        public void SetStartPosition()
        {
            InitialPosition = ParentUIBase.Position;
        }

        /// <summary>
        /// Shifts the UI to the right.
        /// </summary>
        /// <returns>Returns a bool indicating whether the shift was completed.</returns>
        protected override bool Action()
        {
            var position = ParentUIBase.Position;

            if (ElapsedTime >= StartDelayInSeconds)
            {
                position.X += (float)RateOfChange * (float)DeltaTime;
            }

            // Correction for float calculations.
            if (position.X >= InitialPosition.X + ShiftVector.X)
            {
                position.X = InitialPosition.X + ShiftVector.X;
            }

            ParentUIBase.Position = position;

            return ParentUIBase.Position.X >= InitialPosition.X + ShiftVector.X;
        }
    }
}