namespace Softfire.MonoGame.CORE.Physics
{
    /// <summary>
    /// The available easing enums.
    /// </summary>
    public static class EasingEnums
    {
        /// <summary>
        /// The available easings to perform.
        /// </summary>
        public enum Easings
        {
            /// <summary>
            /// No easing is performed.
            /// </summary>
            None,
            /// <summary>
            /// A back easing is performed.
            /// </summary>
            Back,
            /// <summary>
            /// A bounce easing is performed.
            /// </summary>
            Bounce,
            /// <summary>
            /// A circular easing is performed.
            /// </summary>
            Circular,
            /// <summary>
            /// A cubic easing is performed.
            /// </summary>
            Cubic,
            /// <summary>
            /// An elastic easing is performed.
            /// </summary>
            Elastic,
            /// <summary>
            /// An exponential easing is performed.
            /// </summary>
            Exponential,
            /// <summary>
            /// A linear easing is performed.
            /// </summary>
            Linear,
            /// <summary>
            /// A quadratic easing is performed.
            /// </summary>
            Quadratic,
            /// <summary>
            /// A quartic easing is performed.
            /// </summary>
            Quartic,
            /// <summary>
            /// A quintic easing is performed.
            /// </summary>
            Quintic,
            /// <summary>
            /// A sine easing is performed.
            /// </summary>
            Sine
        }

        /// <summary>
        /// The available options for each <see cref="Easings"/> to perform.
        /// </summary>
        public enum EasingOptions
        {
            /// <summary>
            /// No easing option is performed.
            /// </summary>
            None,
            /// <summary>
            /// An inward easing option is performed.
            /// </summary>
            In,
            /// <summary>
            /// An outward easing option is performed.
            /// </summary>
            Out,
            /// <summary>
            /// An inward then an outward easing option is performed.
            /// </summary>
            InOut,
            /// <summary>
            /// An outward then an inward easing option is performed.
            /// </summary>
            OutIn
        }

        /// <summary>
        /// The available directions on the X axis to perform the <see cref="Easings"/> and it's <see cref="EasingOptions"/>.
        /// </summary>
        public enum EasingXAxisDirections
        {
            /// <summary>
            /// No direction selected along the X axis.
            /// </summary>
            None,
            /// <summary>
            /// The easing will be performed to the left along the X axis.
            /// </summary>
            Left,
            /// <summary>
            /// The easing will be performed to the right along the X axis.
            /// </summary>
            Right
        }

        /// <summary>
        /// The available directions on the Y axis to perform the <see cref="Easings"/> and it's <see cref="EasingOptions"/>.
        /// </summary>
        public enum EasingYAxisDirections
        {
            /// <summary>
            /// No direction selected along the Y axis.
            /// </summary>
            None,
            /// <summary>
            /// The easing will be performed upward along the Y axis.
            /// </summary>
            Up,
            /// <summary>
            /// The easing will be performed downward along the Y axis.
            /// </summary>
            Down
        }

        /// <summary>
        /// The available axis' to perform the <see cref="Easings"/> and it's <see cref="EasingOptions"/>.
        /// </summary>
        public enum Axis
        {
            /// <summary>
            /// The X axis.
            /// </summary>
            X,
            /// <summary>
            /// The Y axis.
            /// </summary>
            Y
        }
    }
}