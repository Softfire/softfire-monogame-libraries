using System.Threading.Tasks;

namespace Softfire.MonoGame.UI.Effects
{
    /// <summary>
    /// The base UI class for effects.
    /// </summary>
    public abstract class UIEffectBase : IUIIdentifier
    {
        /// <summary>
        /// Time between update calls in seconds.
        /// </summary>
        internal static double DeltaTime { get; set; }

        /// <summary>
        /// The elapsed time since activation.
        /// </summary>
        protected double ElapsedTime { get; private set; }

        /// <summary>
        /// A unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// A unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The parent UIBase.
        /// </summary>
        protected UIBase ParentUIBase { get; }
        
        /// <summary>
        /// The rate of change between calls.
        /// </summary>
        public double RateOfChange { get; protected set; }

        /// <summary>
        /// The duration, in seconds, of the effect.
        /// </summary>
        public float DurationInSeconds { get; }

        /// <summary>
        /// The delay, in seconds, before the start of the effect.
        /// </summary>
        public float StartDelayInSeconds { get; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// The order number for the effect. To be run in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value >= 1 & value < int.MaxValue ? value : 0;
        }

        /// <summary>
        /// The base class for UI Effects.
        /// </summary>
        /// <param name="uiBase">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an int.</param>
        /// <param name="name">A unique name. Intaken as a string.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a float. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a float. Default is 0f.</param>
        /// <param name="orderNumber">The effect's run order number. Intaken as an int. Default is 0.</param>
        /// <see cref="Action()"/>
        /// <seealso cref="Run()"/>
        protected UIEffectBase(UIBase uiBase, int id, string name, float durationInSeconds = 1f, float startDelayInSeconds = 0f, int orderNumber = 0)
        {
            ParentUIBase = uiBase;
            Id = id;
            Name = name;
            DurationInSeconds = durationInSeconds;
            StartDelayInSeconds = startDelayInSeconds;
            OrderNumber = orderNumber;

            ElapsedTime = 0;
        }

        /// <summary>
        /// Runs the effect's Action method.
        /// </summary>
        /// <returns>Returns a Task{bool} indicating the result of the effect's Action.</returns>
        internal async Task<bool> Run()
        {
            ElapsedTime += DeltaTime;

            return await Task.Run(() => Action());
        }

        /// <summary>
        /// Reset.
        /// Resets the effect so it can be run again.
        /// </summary>
        internal void Reset()
        {
            ElapsedTime = 0;
            RateOfChange = 0;
        }

        /// <summary>
        /// Use to define non-drawing effects.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected abstract bool Action();
    }
}