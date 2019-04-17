using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// An interface for defining an easing.
    /// </summary>
    public interface IMonoGameEasingComponent
    {
        /// <summary>
        /// The easing's start position.
        /// </summary>
        Vector2 StartPosition { get; set; }

        /// <summary>
        /// The initial value of the easing.
        /// </summary>
        float InitialValue { get; set; }

        /// <summary>
        /// The change in value to occur over the easing's <see cref="Duration"/>.
        /// </summary>
        float ChangeInValue { get; set; }

        /// <summary>
        /// The duration, in seconds, for the easing.
        /// </summary>
        float Duration { get; set; }

        /// <summary>
        /// The amount to overshoot the easing.
        /// </summary>
        double Overshoot { get; set; }

        /// <summary>
        /// The amplitude changes the height of the curve for an elastic easing.
        /// </summary>
        double Amplitude { get; set; }

        /// <summary>
        /// The period slows the rate of elastic bounce for an elastic easing.
        /// </summary>
        double Period { get; set; }

        /// <summary>
        /// Determines whether the easing is looping.
        /// </summary>
        bool IsLooping { get; set; }

        /// <summary>
        /// Determines whether the easing returns to it's starting position following it's easing path in reverse.
        /// </summary>
        bool IsReturningInReverse { get; set; }

        /// <summary>
        /// Determines whether the easing is performing in reverse.
        /// </summary>
        bool IsInReverse { get; }
    }
}