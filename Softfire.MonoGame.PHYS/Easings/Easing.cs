using Microsoft.Xna.Framework;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Physics;

namespace Softfire.MonoGame.PHYS.Easings
{
    /// <summary>
    /// For applying easing functions onto a <see cref="MonoGameObject"/>.
    /// Easing functions specify the rate of change of a parameter over time.
    /// </summary>
    public class Easing : IMonoGameEasingComponent
    {
        /// <summary>
        /// A delegate for usage with <see cref="Easing"/>'s that do not have an <see cref="Overshoot"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        private delegate double EasingDelegateWithoutOvershoot(double t, double b, double c, double d);

        /// <summary>
        /// A delegate for usage with <see cref="Easing"/>'s that have an <see cref="Overshoot"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="s">The amount to overshoot (arc) the movement during the easing. The higher the overshoot, the greater the arc. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        private delegate double EasingDelegateWithOvershoot(double t, double b, double c, double d, double s = 1.70158d);

        /// <summary>
        /// A delegate for usage with <see cref="Easing"/>'s that use <see cref="Amplitude"/> and <see cref="Period"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="a">The amplitude changes the height of the curve. Intaken as a <see cref="double"/>.</param>
        /// <param name="p">The period slows the rate of the elastic bounce. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        private delegate double EasingDelegateWithAmplitudeAndPeriod(double t, double b, double c, double d, double a = 0d, double p = 0d);

        /// <summary>
        /// Determines whether the <see cref="Easing"/>'s updates can occur.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// An elapsed time counter.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// The <see cref="Easing"/>'s unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The <see cref="Easing"/>'s unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The parent <see cref="MonoGameObject"/>.
        /// </summary>
        private MonoGameObject Parent { get; }
        
        /// <summary>
        /// The initial value of the easing.
        /// </summary>
        public float InitialValue { get; set; }

        /// <summary>
        /// The change in value of the easing.
        /// </summary>
        public float ChangeInValue { get; set; } = 100f;

        /// <summary>
        /// The duration, in seconds, for the easing.
        /// </summary>
        public float Duration { get; set; } = 2f;

        /// <summary>
        /// The amount to overshoot the easing.
        /// </summary>
        public double Overshoot { get; set; } = 1.70158d;

        /// <summary>
        /// The amplitude changes the height of the curve for <see cref="EasingEnums.Easings.Elastic"/>.
        /// </summary>
        public double Amplitude { get; set; }

        /// <summary>
        /// The period slows the rate of elastic bounce for <see cref="EasingEnums.Easings.Elastic"/>.
        /// </summary>
        public double Period { get; set; }

        /// <summary>
        /// Determines whether the easing will be performed in a loop.
        /// </summary>
        public bool IsLooping { get; set; }

        /// <summary>
        /// Determines whether the easing will perform a reverse easing upon completing it's loop. This is dependent on <see cref="IsLooping"/> being true.
        /// </summary>
        public bool WillReturnInReverse { get; set; }

        /// <summary>
        /// Determines whether the easing is performing in reverse.
        /// </summary>
        public bool IsInReverse { get; private set; }

        /// <summary>
        /// The forward position to start applying the <see cref="Easing"/>.
        /// </summary>
        private Vector2 StartingPosition { get; set; }

        /// <summary>
        /// The reverse position to start applying the <see cref="Easing"/>.
        /// </summary>
        private Vector2 ReverseStartingPosition { get; set; }

        /// <summary> 
        /// The current easing in use on the <see cref="MonoGameObject"/>'s X axis.
        /// </summary>
        public EasingEnums.Easings CurrentXAxisEasing { get; set; } = EasingEnums.Easings.Linear;

        /// <summary> 
        /// The current easing option in use on the <see cref="MonoGameObject"/>'s X axis.
        /// </summary>
        public EasingEnums.EasingOptions CurrentXAxisEasingOption { get; set; } = EasingEnums.EasingOptions.In;

        /// <summary>
        /// The direction in which the <see cref="MonoGameObject"/>'s Y axis will be affected.
        /// </summary>
        public EasingEnums.EasingXAxisDirections CurrentXAxisDirection { get; set; } = EasingEnums.EasingXAxisDirections.Right;

        /// <summary> 
        /// The current easing in use on the <see cref="MonoGameObject"/>'s Y axis.
        /// </summary>
        public EasingEnums.Easings CurrentYAxisEasing { get; set; } = EasingEnums.Easings.Sine;

        /// <summary> 
        /// The current easing option in use on the <see cref="MonoGameObject"/>'s Y axis.
        /// </summary>
        public EasingEnums.EasingOptions CurrentYAxisEasingOption { get; set; } = EasingEnums.EasingOptions.In;

        /// <summary>
        /// The direction in which the <see cref="MonoGameObject"/>'s X axis will be affected.
        /// </summary>
        public EasingEnums.EasingYAxisDirections CurrentYAxisDirection { get; set; } = EasingEnums.EasingYAxisDirections.Up;
        
        /// <summary>
        /// An easing for moving a <see cref="MonoGameObject"/> over time.
        /// </summary>
        /// <param name="parent">The <see cref="Easing"/>'s parent object. This object will be affected by the easing. Intaken as a <see cref="MonoGameObject"/>.</param>
        /// <param name="id">The <see cref="Easing"/>'s id. Intaken as a <see cref="int"/>.</param>
        /// <param name="name">The <see cref="Easing"/>'s name. Intaken as a <see cref="string"/>.</param>
        public Easing(MonoGameObject parent, int id, string name)
        {
            Parent = parent;
            Id = id;
            Name = name;

            StartingPosition = Parent.Transform.Position;
        }

        #region Easing Actions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithOvershoot easing, EasingEnums.Axis axis)
        {
            var position = IsLooping && WillReturnInReverse && IsInReverse ? ReverseStartingPosition : StartingPosition;

            if (axis == EasingEnums.Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingEnums.EasingXAxisDirections.None:
                        return;
                    case EasingEnums.EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot), 0);
                        break;
                    case EasingEnums.EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot), 0);
                        break;
                }
                
                position.Y = Parent.Transform.Position.Y;
                Parent.Transform.Position = position;
            }

            if (axis == EasingEnums.Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingEnums.EasingYAxisDirections.None:
                        return;
                    case EasingEnums.EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot));
                        break;
                    case EasingEnums.EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot));
                        break;
                }

                position.X = Parent.Transform.Position.X;
                Parent.Transform.Position = position;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithoutOvershoot easing, EasingEnums.Axis axis)
        {
            var position = IsLooping && WillReturnInReverse && IsInReverse ? ReverseStartingPosition : StartingPosition;

            if (axis == EasingEnums.Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingEnums.EasingXAxisDirections.None:
                        return;
                    case EasingEnums.EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration), 0);
                        break;
                    case EasingEnums.EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration), 0);
                        break;
                }

                position.Y = Parent.Transform.Position.Y;
                Parent.Transform.Position = position;
            }

            if (axis == EasingEnums.Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingEnums.EasingYAxisDirections.None:
                        return;
                    case EasingEnums.EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration));
                        break;
                    case EasingEnums.EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration));
                        break;
                }

                position.X = Parent.Transform.Position.X;
                Parent.Transform.Position = position;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithAmplitudeAndPeriod easing, EasingEnums.Axis axis)
        {
            var position = IsLooping && WillReturnInReverse && IsInReverse ? ReverseStartingPosition : StartingPosition;

            if (axis == EasingEnums.Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingEnums.EasingXAxisDirections.None:
                        return;
                    case EasingEnums.EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period), 0);
                        break;
                    case EasingEnums.EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period), 0);
                        break;
                }

                position.Y = Parent.Transform.Position.Y;
                Parent.Transform.Position = position;
            }

            if (axis == EasingEnums.Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingEnums.EasingYAxisDirections.None:
                        return;
                    case EasingEnums.EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period));
                        break;
                    case EasingEnums.EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period));
                        break;
                }

                position.X = Parent.Transform.Position.X;
                Parent.Transform.Position = position;
            }
        }

        #endregion

        /// <summary>
        /// The <see cref="Easing"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive)
            {
                ElapsedTime += DeltaTime;

                if (ElapsedTime < Duration)
                {
                    switch (CurrentXAxisEasing)
                    {
                        case EasingEnums.Easings.None:
                            break;
                        case EasingEnums.Easings.Back:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Back.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Back.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Back.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Back.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Bounce:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Bounce.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Bounce.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Bounce.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Bounce.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Circular:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Circ.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Circ.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Circ.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Circ.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Cubic:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Cubic.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Cubic.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Cubic.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Cubic.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Elastic:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Elastic.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Elastic.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Elastic.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Elastic.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Exponential:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Expo.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Expo.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Expo.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Expo.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Linear:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Linear.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Linear.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Linear.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Linear.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quadratic:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quad.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quad.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quad.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quad.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quartic:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quart.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quart.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quart.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quart.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quintic:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quint.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quint.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quint.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quint.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Sine:
                            switch (CurrentXAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Sine.In, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Sine.Out, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Sine.InOut, EasingEnums.Axis.X);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Sine.OutIn, EasingEnums.Axis.X);
                                    break;
                            }
                            break;
                    }

                    switch (CurrentYAxisEasing)
                    {
                        case EasingEnums.Easings.None:
                            break;
                        case EasingEnums.Easings.Back:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Back.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Back.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Back.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Back.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Bounce:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Bounce.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Bounce.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Bounce.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Bounce.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Circular:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Circ.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Circ.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Circ.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Circ.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Cubic:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Cubic.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Cubic.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Cubic.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Cubic.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Elastic:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Elastic.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Elastic.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Elastic.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Elastic.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Exponential:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Expo.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Expo.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Expo.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Expo.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Linear:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Linear.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Linear.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Linear.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Linear.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quadratic:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quad.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quad.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quad.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quad.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quartic:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quart.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quart.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quart.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quart.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Quintic:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Quint.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Quint.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Quint.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Quint.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                        case EasingEnums.Easings.Sine:
                            switch (CurrentYAxisEasingOption)
                            {
                                case EasingEnums.EasingOptions.None:
                                    break;
                                case EasingEnums.EasingOptions.In:
                                    EasingActions(Sine.In, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.Out:
                                    EasingActions(Sine.Out, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.InOut:
                                    EasingActions(Sine.InOut, EasingEnums.Axis.Y);
                                    break;
                                case EasingEnums.EasingOptions.OutIn:
                                    EasingActions(Sine.OutIn, EasingEnums.Axis.Y);
                                    break;
                            }
                            break;
                    }
                }
                else if (IsLooping)
                {
                    if (WillReturnInReverse)
                    {
                        IsInReverse = !IsInReverse;
                        CurrentXAxisDirection = CurrentXAxisDirection == EasingEnums.EasingXAxisDirections.Left ? EasingEnums.EasingXAxisDirections.Right : EasingEnums.EasingXAxisDirections.Left;
                        CurrentYAxisDirection = CurrentYAxisDirection == EasingEnums.EasingYAxisDirections.Up ? EasingEnums.EasingYAxisDirections.Down : EasingEnums.EasingYAxisDirections.Up;

                        if (IsInReverse)
                        {
                            ReverseStartingPosition = Parent.Transform.Position;
                        }
                        else
                        {
                            StartingPosition = Parent.Transform.Position;
                        }
                    }

                    ElapsedTime = 0;
                }
                else
                {
                    IsActive = false;
                    ElapsedTime = 0;
                }
            }
        }
    }
}