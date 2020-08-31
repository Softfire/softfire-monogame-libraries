using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.SM.V2
{
    /// <summary>
    /// A transition class for use between states.
    /// </summary>
    public abstract class Transition
    {
        /// <summary>
        /// DeltaTime.
        /// Time between update calls in seconds.
        /// </summary>
        public static double DeltaTime { protected get; set; }

        /// <summary>
        /// Elapsed Time.
        /// </summary>
        protected double ElapsedTime { get; private set; }

        /// <summary>
        /// Is Completed?
        /// </summary>
        public bool IsCompleted { get; protected set; }

        /// <summary>
        /// Parent State.
        /// </summary>
        protected State ParentState { get; }

        /// <summary>
        /// Duration In Seconds.
        /// </summary>
        protected float DurationInSeconds { get; }

        /// <summary>
        /// Start Delay In Seconds.
        /// </summary>
        protected float StartDelayInSeconds { get; }

        /// <summary>
        /// Rate of Change.
        /// </summary>
        protected double RateOfChange { get; set; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// Transition will be run in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// Transition Constructor.
        /// </summary>
        /// <param name="state">The Parent State. Intakes a State.</param>
        /// <param name="durationInSeconds">Transition duration in seconds. Intaken as a <see cref="float"/>. Default is 1.0f.</param>
        /// <param name="startDelayInSeconds">Transition start delay in seconds. Intaken as a <see cref="float"/>. Default is 0.0f.</param>
        /// <param name="orderNumber">Intakes the Transition's run Order Number as an int. Default is 0.</param>
        public Transition(State state, float durationInSeconds = 1.0f, float startDelayInSeconds = 0.0f, int orderNumber = 0)
        {
            ParentState = state;
            DurationInSeconds = durationInSeconds;
            StartDelayInSeconds = startDelayInSeconds;
            OrderNumber = orderNumber;

            ElapsedTime = 0;
            IsCompleted = false;
        }

        /// <summary>
        /// Size.
        /// </summary>
        /// <param name="state">A State.</param>
        /// <param name="startWidth"></param>
        /// <param name="startHeight"></param>
        /// <param name="endWidth"></param>
        /// <param name="endHeight"></param>
        /// <param name="duration"></param>
        /// <param name="deltaTime"></param>
        public async Task ReSize(State state, int startWidth, int startHeight, int endWidth, int endHeight, float duration, float deltaTime)
        {
            var rateOfWidthChange = (startWidth - endWidth) / duration;
            var rateOfHeightChange = (startHeight - endHeight) / duration;

            await Task.Run(() => state.Width = MathHelper.Clamp(state.Width + (int)rateOfWidthChange * (int)deltaTime, startWidth, endWidth));
            await Task.Run(() => state.Height = MathHelper.Clamp(state.Height + (int)rateOfHeightChange * (int)deltaTime, startHeight, endHeight));
        }

        /// <summary>
        /// Run.
        /// Runs the Transition's Action method.
        /// </summary>
        /// <returns>Returns a Task{bool} indicating the result of the Transition's Action.</returns>
        public async Task<bool> Run()
        {
            ElapsedTime += DeltaTime;

            return IsCompleted = await Task.Run(() => Action());
        }

        /// <summary>
        /// Reset.
        /// Resets the transition so it can be run again.
        /// </summary>
        public void Reset()
        {
            IsCompleted = false;
            ElapsedTime = 0.0;
        }

        /// <summary>
        /// Convert Degrees To Radians.
        /// </summary>
        /// <param name="angleInDegrees">The angle in Degrees. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the angle in Radians as a double.</returns>
        public double ConvertToRadians(double angleInDegrees)
        {
            return (MathHelper.Pi / 180) * angleInDegrees;
        }
        
        /// <summary>
        /// Convert Radians To Degrees.
        /// </summary>
        /// <param name="angleInRadians">The angle in Radians. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the angle in Degrees as a double.</returns>
        public double ConvertToDegrees(double angleInRadians)
        {
            return angleInRadians * (180.0 / MathHelper.Pi);
        }

        /// <summary>
        /// Action.
        /// Defines Transition.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected abstract bool Action();
    }
}