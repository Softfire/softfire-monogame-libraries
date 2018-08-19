using System.Threading.Tasks;

namespace Softfire.MonoGame.UI.Effects
{
    public abstract class UIEffectBase
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
        /// Parent UI Base.
        /// </summary>
        protected UIBase ParentUIBase { get; }

        /// <summary>
        /// Is Completed?
        /// </summary>
        public bool IsCompleted { get; protected set; }

        /// <summary>
        /// Rate of Change.
        /// </summary>
        protected double RateOfChange { get; set; }

        /// <summary>
        /// Duration In Seconds.
        /// </summary>
        protected float DurationInSeconds { get; }

        /// <summary>
        /// Start Delay In Seconds.
        /// </summary>
        protected float StartDelayInSeconds { get; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// Effect will be run in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 ? value : 0;
        }

        /// <summary>
        /// UI Base Effect.
        /// </summary>
        /// <param name="uiBase">The Parent UIBase.</param>
        /// <param name="effectType">The type of Effect.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float. Default is 1.0f.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float. Default is 0.0f.</param>
        /// <param name="orderNumber">Intakes the Effect's run Order Number as an int. Default is 0.</param>
        public UIEffectBase(UIBase uiBase, float durationInSeconds = 1.0f, float startDelayInSeconds = 0.0f, int orderNumber = 0)
        {
            ParentUIBase = uiBase;
            DurationInSeconds = durationInSeconds;
            StartDelayInSeconds = startDelayInSeconds;
            OrderNumber = orderNumber;

            ElapsedTime = 0;
            IsCompleted = false;
        }

        /// <summary>
        /// Run.
        /// Runs the Effect's Action method.
        /// </summary>
        /// <returns>Returns a Task{bool} indicating the result of the Effect's Action.</returns>
        public async Task<bool> Run()
        {
            ElapsedTime += DeltaTime;

            return IsCompleted = await Task.Run(() => Action());
        }

        /// <summary>
        /// Reset.
        /// Resets the effect so it can be run again.
        /// </summary>
        public void Reset()
        {
            ElapsedTime = 0;
            IsCompleted = false;
        }

        /// <summary>
        /// Action.
        /// Defines Non-Drawing Effect.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected abstract bool Action();
    }
}