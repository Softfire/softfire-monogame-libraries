using Softfire.MonoGame.CORE.V2.Physics;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for defining an easing.
    /// </summary>
    public interface IMonoGameEasingComponent : IMonoGameIdentifierComponent, IMonoGameActiveComponent, IMonoGameUpdateComponent
    {
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
        bool WillReturnInReverse { get; set; }

        /// <summary>
        /// Determines whether the easing is performing in reverse.
        /// </summary>
        bool IsInReverse { get; }

        /// <summary> 
        /// The current easing in use on a <see cref="MonoGameObject"/>'s X axis.
        /// See <see cref="EasingEnums.Easings"/> for available easings.
        /// </summary>
        EasingEnums.Easings CurrentXAxisEasing { get; set; }

        /// <summary> 
        /// The current easing option in use on a <see cref="MonoGameObject"/>'s X axis.
        /// See <see cref="EasingEnums.EasingOptions"/> for available options.
        /// </summary>
        EasingEnums.EasingOptions CurrentXAxisEasingOption { get; set; }

        /// <summary>
        /// The direction in which a <see cref="MonoGameObject"/>'s X axis will be affected.
        /// See <see cref="EasingEnums.EasingXAxisDirections"/> for available directions.
        /// </summary>
        EasingEnums.EasingXAxisDirections CurrentXAxisDirection { get; set; }

        /// <summary> 
        /// The current easing in use on a <see cref="MonoGameObject"/>'s Y axis.
        /// See <see cref="EasingEnums.Easings"/> for available easings.
        /// </summary>
        EasingEnums.Easings CurrentYAxisEasing { get; set; }

        /// <summary> 
        /// The current easing option in use on a <see cref="MonoGameObject"/>'s Y axis.
        /// See <see cref="EasingEnums.EasingOptions"/> for available options.
        /// </summary>
        EasingEnums.EasingOptions CurrentYAxisEasingOption { get; set; }

        /// <summary>
        /// The direction in which a <see cref="MonoGameObject"/>'s Y axis will be affected.
        /// See <see cref="EasingEnums.EasingYAxisDirections"/> for available directions.
        /// </summary>
        EasingEnums.EasingYAxisDirections CurrentYAxisDirection { get; set; }
    }
}