using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.PHYS.Easings
{
    /// <summary>
    /// A <see cref="MonoGameObject"/> for use with demonstrating easing functions.
    /// Easing functions specify the rate of change of a parameter over time.
    /// </summary>
    public class Easing : MonoGameObject
    {
        /// <summary>
        /// A delegate for usage with easing's that do not have an <see cref="Overshoot"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        private delegate double EasingDelegateWithoutOvershoot(double t, double b, double c, double d);

        /// <summary>
        /// A delegate for usage with easing's that have an <see cref="Overshoot"/>.
        /// </summary>
        /// <param name="t">The current time or position. Intaken as a <see cref="double"/>.</param>
        /// <param name="b">The initial starting value for the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="c">The change in value to occur over the duration of the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="d">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="double"/>.</param>
        /// <param name="s">The amount to overshoot (arc) the movement during the easing. The higher the overshoot, the greater the arc. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the eased value as a <see cref="double"/>.</returns>
        private delegate double EasingDelegateWithOvershoot(double t, double b, double c, double d, double s = 1.70158d);

        /// <summary>
        /// A delegate for usage with easing's that use <see cref="Amplitude"/> and <see cref="Period"/>.
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
        /// The easing's texture path. For use in demonstrations only.
        /// </summary>
        private string TexturePath { get; }

        /// <summary>
        /// The easing's texture to display. For use in demonstrations only.
        /// </summary>
        private Texture2D Texture { get; set; }

        /// <summary>
        /// The easing's start position.
        /// </summary>
        private Vector2 StartPosition { get; set; }

        /// <summary>
        /// The easing's reverse starting position.
        /// </summary>
        private Vector2 ReverseStartPosition { get; set; }

        /// <summary>
        /// The initial value of the easing.
        /// </summary>
        public float InitialValue { get; set; }

        /// <summary>
        /// The change in value of the easing.
        /// </summary>
        public float ChangeInValue { get; set; }

        /// <summary>
        /// The duration, in seconds, for the easing.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// The amount to overshoot the easing.
        /// </summary>
        public double Overshoot { get; set; }

        /// <summary>
        /// The amplitude changes the height of the curve for <see cref="Easings.Elastic"/>.
        /// </summary>
        public double Amplitude { get; set; }

        /// <summary>
        /// The period slows the rate of elastic bounce for <see cref="Easings.Elastic"/>.
        /// </summary>
        public double Period { get; set; }

        /// <summary>
        /// Determines whether the easing is looping.
        /// </summary>
        public bool IsLooping { get; set; }

        /// <summary>
        /// Determines whether the easing returns to it's starting position following it's easing path in reverse.
        /// </summary>
        public bool IsReturningInReverse { get; set; }

        /// <summary>
        /// Determines whether the easing is performing in reverse.
        /// </summary>
        public bool IsInReverse { get; private set; }

        /// <summary>
        /// Determines whether the easing is in reverse.
        /// </summary>
        private bool RecordReverseStartPosition { get; set; }

        /// <summary> 
        /// The current easing in use on the <see cref="MonoGameObject"/>'s X axis.
        /// </summary>
        public Easings CurrentXAxisEasing { get; set; }

        /// <summary> 
        /// The current easing option in use on the <see cref="MonoGameObject"/>'s X axis.
        /// </summary>
        public EasingOptions CurrentXAxisEasingOption { get; set; }

        /// <summary>
        /// The direction in which the <see cref="MonoGameObject"/>'s Y axis will be affected.
        /// </summary>
        public EasingXAxisDirections CurrentXAxisDirection { get; set; }
        
        /// <summary> 
        /// The current easing in use on the <see cref="MonoGameObject"/>'s Y axis.
        /// </summary>
        public Easings CurrentYAxisEasing { get; set; }

        /// <summary> 
        /// The current easing option in use on the <see cref="MonoGameObject"/>'s Y axis.
        /// </summary>
        public EasingOptions CurrentYAxisEasingOption { get; set; }

        /// <summary>
        /// The direction in which the <see cref="MonoGameObject"/>'s X axis will be affected.
        /// </summary>
        public EasingYAxisDirections CurrentYAxisDirection { get; set; }
        
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
            InOut
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
        private enum Axis
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

        /// <summary>
        /// An easing for moving a <see cref="MonoGameObject"/> over time.
        /// </summary>
        /// <param name="parent">The easing's parent object. Intaken as a <see cref="MonoGameObject"/>.</param>
        /// <param name="id">The easing's id. Intaken as a <see cref="int"/>.</param>
        /// <param name="name">The easing's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="initialValue">The initial starting value for the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="changeInValue">The change in value to occur over the duration of the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="easingXAxis">The <see cref="Easings"/> to use on the parent <see cref="MonoGameObject"/>'s X axis. Intaken as a <see cref="Easings"/>.</param>
        /// <param name="easingXAxisOption">The <see cref="EasingOptions"/> to use with the <see cref="CurrentXAxisEasing"/>. Intaken as a <see cref="EasingOptions"/>.</param>
        /// <param name="easingXAxisDirection">The <see cref="EasingXAxisDirections"/> to define the direction the easing will travel along the X axis. Intaken as a <see cref="EasingXAxisDirections"/>.</param>
        /// <param name="easingYAxis">The <see cref="Easings"/> to use on the parent <see cref="MonoGameObject"/>'s Y axis. Intaken as a <see cref="Easings"/>.</param>
        /// <param name="easingYAxisOption">The <see cref="EasingOptions"/> to use with the <see cref="CurrentYAxisEasing"/>. Intaken as a <see cref="EasingOptions"/>.</param>
        /// <param name="easingYAxisDirection">The <see cref="EasingYAxisDirections"/> to define the direction the easing will travel along the Y axis. Intaken as a <see cref="EasingYAxisDirections"/>.</param>
        /// <param name="overshoot">The amount to overshoot (arc) the movement during the <see cref="Easings.Back"/> easing. The higher the overshoot, the greater the arc. Intaken as a <see cref="double"/>.</param>
        /// <param name="amplitude">The amplitude changes the height of the curve for <see cref="Easings.Elastic"/>. Intaken as a <see cref="double"/>.</param>
        /// <param name="period">The period slows the rate of elastic bounce for <see cref="Easings.Elastic"/>. Intaken as a <see cref="double"/>.</param>
        /// <param name="duration">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="isLooping">Determines whether the easing will be performed in a loop. Intaken as a <see cref="bool"/>.</param>
        /// <param name="isReturningInReverse">Determines whether the easing will perform a reverse easing upon completing it's loop. This is dependent on <see cref="IsLooping"/> being true. Intaken as a <see cref="bool"/>.</param>
        /// <remarks>For use with <see cref="MonoGameObject"/>'s with their own textures and positions.</remarks>
        public Easing(MonoGameObject parent, int id, string name, float initialValue = 0f, float changeInValue = 100f,
                      Easings easingXAxis = Easings.Linear, EasingOptions easingXAxisOption = EasingOptions.In, EasingXAxisDirections easingXAxisDirection = EasingXAxisDirections.Right,
                      Easings easingYAxis = Easings.Sine, EasingOptions easingYAxisOption = EasingOptions.In, EasingYAxisDirections easingYAxisDirection = EasingYAxisDirections.Up,
                      double overshoot = 1.70158d, double amplitude = 0d, double period = 0d, float duration = 2.0f,
                      bool isLooping = true, bool isReturningInReverse = true) : this(parent, id, name, null, parent.Transform.WorldPosition(), initialValue, changeInValue,
                                                                                      easingXAxis, easingXAxisOption, easingXAxisDirection,
                                                                                      easingYAxis, easingYAxisOption, easingYAxisDirection,
                                                                                      overshoot, amplitude, period, duration,
                                                                                      isLooping, isReturningInReverse)
        {
        }

        /// <summary>
        /// An easing demonstrating movement over time.
        /// </summary>
        /// <param name="parent">The easing's parent object. Intaken as a <see cref="MonoGameObject"/>.</param>
        /// <param name="id">The easing's id. Intaken as a <see cref="int"/>.</param>
        /// <param name="name">The easing's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="texturePath">The easing's texture file path. Intaken as a <see cref="string"/>.</param>
        /// <param name="startPosition">The easing's starting position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="initialValue">The initial starting value for the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="changeInValue">The change in value to occur over the duration of the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="easingXAxis">The <see cref="Easings"/> to use on the parent <see cref="MonoGameObject"/>'s X axis. Intaken as a <see cref="Easings"/>.</param>
        /// <param name="easingXAxisOption">The <see cref="EasingOptions"/> to use with the <see cref="CurrentXAxisEasing"/>. Intaken as a <see cref="EasingOptions"/>.</param>
        /// <param name="easingXAxisDirection">The <see cref="EasingXAxisDirections"/> to define the direction the easing will travel along the X axis. Intaken as a <see cref="EasingXAxisDirections"/>.</param>
        /// <param name="easingYAxis">The <see cref="Easings"/> to use on the parent <see cref="MonoGameObject"/>'s Y axis. Intaken as a <see cref="Easings"/>.</param>
        /// <param name="easingYAxisOption">The <see cref="EasingOptions"/> to use with the <see cref="CurrentYAxisEasing"/>. Intaken as a <see cref="EasingOptions"/>.</param>
        /// <param name="easingYAxisDirection">The <see cref="EasingYAxisDirections"/> to define the direction the easing will travel along the Y axis. Intaken as a <see cref="EasingYAxisDirections"/>.</param>
        /// <param name="overshoot">The amount to overshoot (arc) the movement during the <see cref="Easings.Back"/> easing. The higher the overshoot, the greater the arc. Intaken as a <see cref="double"/>.</param>
        /// <param name="amplitude">The amplitude changes the height of the curve for <see cref="Easings.Elastic"/>. Intaken as a <see cref="double"/>.</param>
        /// <param name="period">The period slows the rate of elastic bounce for <see cref="Easings.Elastic"/>. Intaken as a <see cref="double"/>.</param>
        /// <param name="duration">The amount of time, in seconds, to perform the easing. Intaken as a <see cref="float"/>.</param>
        /// <param name="isLooping">Determines whether the easing will be performed in a loop. Intaken as a <see cref="bool"/>.</param>
        /// <param name="isReturningInReverse">Determines whether the easing will perform a reverse easing upon completing it's loop. This is dependent on <see cref="IsLooping"/> being true. Intaken as a <see cref="bool"/>.</param>
        /// <remarks>For demonstration purposes only. Call <see cref="LoadContent(ContentManager)"/> to load <see cref="Texture"/> and then <see cref="Draw(SpriteBatch, Matrix)"/> to draw a demonstration of the easing.</remarks>
        public Easing(MonoGameObject parent, int id, string name, string texturePath, Vector2 startPosition, float initialValue = 0f, float changeInValue = 100f,
                      Easings easingXAxis = Easings.Linear, EasingOptions easingXAxisOption = EasingOptions.In, EasingXAxisDirections easingXAxisDirection = EasingXAxisDirections.Right,
                      Easings easingYAxis = Easings.Sine, EasingOptions easingYAxisOption = EasingOptions.In, EasingYAxisDirections easingYAxisDirection = EasingYAxisDirections.Up,
                      double overshoot = 1.70158d, double amplitude = 0d, double period = 0d, float duration = 2.0f,
                      bool isLooping = true, bool isReturningInReverse = true) : base(parent, id, name, startPosition)
        {
            TexturePath = texturePath;
            StartPosition = startPosition;
            InitialValue = initialValue;
            ChangeInValue = changeInValue;
            CurrentXAxisEasing = easingXAxis;
            CurrentXAxisEasingOption = easingXAxisOption;
            CurrentXAxisDirection = easingXAxisDirection;
            CurrentYAxisEasing = easingYAxis;
            CurrentYAxisEasingOption = easingYAxisOption;
            CurrentYAxisDirection = easingYAxisDirection;
            Duration = duration;
            Overshoot = overshoot;
            Amplitude = amplitude;
            Period = period;
            IsLooping = isLooping;
            IsReturningInReverse = isReturningInReverse;
        }

        #region Easing Actions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithOvershoot easing, Axis axis)
        {
            if (RecordReverseStartPosition)
            {
                ReverseStartPosition = Transform.Position;
                RecordReverseStartPosition = !RecordReverseStartPosition;
            }

            var position = IsLooping && IsReturningInReverse && IsInReverse ? ReverseStartPosition : StartPosition;


            if (axis == Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingXAxisDirections.None:
                        return;
                    case EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot), 0);
                        break;
                    case EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot), 0);
                        break;
                }

                position.Y = Transform.Position.Y;
                Transform.Position = position;
            }

            if (axis == Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingYAxisDirections.None:
                        return;
                    case EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot));
                        break;
                    case EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Overshoot));
                        break;
                }

                position.X = Transform.Position.X;
                Transform.Position = position;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithoutOvershoot easing, Axis axis)
        {
            if (RecordReverseStartPosition)
            {
                ReverseStartPosition = Transform.Position;
                RecordReverseStartPosition = !RecordReverseStartPosition;
            }

            var position = IsLooping && IsReturningInReverse && IsInReverse ? ReverseStartPosition : StartPosition;

            if (axis == Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingXAxisDirections.None:
                        return;
                    case EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration), 0);
                        break;
                    case EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration), 0);
                        break;
                }

                position.Y = Transform.Position.Y;
                Transform.Position = position;
            }

            if (axis == Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingYAxisDirections.None:
                        return;
                    case EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration));
                        break;
                    case EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration));
                        break;
                }

                position.X = Transform.Position.X;
                Transform.Position = position;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="easing"></param>
        /// <param name="axis"></param>
        private void EasingActions(EasingDelegateWithAmplitudeAndPeriod easing, Axis axis)
        {
            if (RecordReverseStartPosition)
            {
                ReverseStartPosition = Transform.Position;
                RecordReverseStartPosition = !RecordReverseStartPosition;
            }

            var position = IsLooping && IsReturningInReverse && IsInReverse ? ReverseStartPosition : StartPosition;

            if (axis == Axis.X)
            {
                switch (CurrentXAxisDirection)
                {
                    case EasingXAxisDirections.None:
                        return;
                    case EasingXAxisDirections.Left:
                        position += new Vector2(-(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period), 0);
                        break;
                    case EasingXAxisDirections.Right:
                        position += new Vector2((float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period), 0);
                        break;
                }

                position.Y = Transform.Position.Y;
                Transform.Position = position;
            }

            if (axis == Axis.Y)
            {
                switch (CurrentYAxisDirection)
                {
                    case EasingYAxisDirections.None:
                        return;
                    case EasingYAxisDirections.Up:
                        position += new Vector2(0, -(float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period));
                        break;
                    case EasingYAxisDirections.Down:
                        position += new Vector2(0, (float)easing(ElapsedTime, InitialValue, ChangeInValue, Duration, Amplitude, Period));
                        break;
                }

                position.X = Transform.Position.X;
                Transform.Position = position;
            }
        }

        #endregion

        /// <summary>
        /// The <see cref="Easing"/>'s content loader.
        /// </summary>
        public override void LoadContent(ContentManager content = null)
        {
            if (content != null &&
                !string.IsNullOrWhiteSpace(TexturePath))
            {
                Texture = content.Load<Texture2D>(TexturePath);
                Size.Width = Texture.Width;
                Size.Height = Texture.Height;
            }

            base.LoadContent(content);
        }

        /// <summary>
        /// The easing's update method.
        /// </summary>
        /// <param name="gameTime">Intakes <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime;

            if (ElapsedTime <= Duration)
            {
                switch (CurrentXAxisEasing)
                {
                    case Easings.None:
                        break;
                    case Easings.Back:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Back.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Back.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Back.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Bounce:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Bounce.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Bounce.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Bounce.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Circular:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Circ.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Circ.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Circ.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Cubic:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Cubic.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Cubic.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Cubic.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Elastic:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Elastic.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Elastic.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Elastic.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Exponential:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Expo.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Expo.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Expo.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Linear:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Linear.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Linear.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Linear.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Quadratic:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quad.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quad.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quad.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Quartic:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quart.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quart.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quart.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Quintic:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quint.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quint.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quint.InOut, Axis.X);
                                break;
                        }
                        break;
                    case Easings.Sine:
                        switch (CurrentXAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Sine.In, Axis.X);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Sine.Out, Axis.X);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Sine.InOut, Axis.X);
                                break;
                        }
                        break;
                }

                switch (CurrentYAxisEasing)
                {
                    case Easings.None:
                        break;
                    case Easings.Back:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Back.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Back.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Back.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Bounce:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Bounce.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Bounce.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Bounce.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Circular:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Circ.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Circ.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Circ.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Cubic:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Cubic.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Cubic.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Cubic.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Elastic:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Elastic.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Elastic.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Elastic.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Exponential:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Expo.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Expo.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Expo.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Linear:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Linear.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Linear.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Linear.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Quadratic:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quad.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quad.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quad.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Quartic:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quart.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quart.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quart.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Quintic:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Quint.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Quint.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Quint.InOut, Axis.Y);
                                break;
                        }
                        break;
                    case Easings.Sine:
                        switch (CurrentYAxisEasingOption)
                        {
                            case EasingOptions.None:
                                break;
                            case EasingOptions.In:
                                EasingActions(Sine.In, Axis.Y);
                                break;
                            case EasingOptions.Out:
                                EasingActions(Sine.Out, Axis.Y);
                                break;
                            case EasingOptions.InOut:
                                EasingActions(Sine.InOut, Axis.Y);
                                break;
                        }
                        break;
                }
            }
            else if (IsLooping)
            {
                if (IsReturningInReverse)
                {
                    IsInReverse = !IsInReverse;
                    RecordReverseStartPosition = !RecordReverseStartPosition;
                    CurrentXAxisDirection = CurrentXAxisDirection == EasingXAxisDirections.Left ? EasingXAxisDirections.Right : EasingXAxisDirections.Left;
                    CurrentYAxisDirection = CurrentYAxisDirection == EasingYAxisDirections.Up ? EasingYAxisDirections.Down : EasingYAxisDirections.Up;
                }

                ResetElapsedTime();
            }
            else
            {
                IsActive = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// The easing's draw method for demonstrating the easing.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Transform.WorldPosition(), Color.White);
            }
        }
    }}