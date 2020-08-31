using Softfire.MonoGame.CORE.V2.Common;

namespace Softfire.MonoGame.UI.V2.Effects
{
    /// <summary>
    /// The base UI class for effects.
    /// </summary>
    public abstract class UIEffectBase : IMonoGameIdentifierComponent
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
        protected UIBase Parent { get; }
        
        /// <summary>
        /// The duration, in seconds, of the effect.
        /// </summary>
        protected float DurationInSeconds { get; }

        /// <summary>
        /// The delay, in seconds, before the start of the effect.
        /// </summary>
        protected float StartDelayInSeconds { get; }

        /// <summary>
        /// Is this the first run?
        /// </summary>
        protected bool IsFirstRun { get; set; } = true;

        /// <summary>
        /// The base class for UI Effects.
        /// </summary>
        /// <param name="parent">The UIBase that will be affected. Intaken as a UIBase.</param>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="durationInSeconds">The effect's duration in seconds. Intaken as a <see cref="float"/>. Default is 1f.</param>
        /// <param name="startDelayInSeconds">The effect's start delay in seconds. Intaken as a <see cref="float"/>. Default is 0f.</param>
        protected UIEffectBase(UIBase parent, int id, string name, float durationInSeconds = 1f, float startDelayInSeconds = 0f)
        {
            Parent = parent;
            Id = id;
            Name = name;
            DurationInSeconds = durationInSeconds;
            StartDelayInSeconds = startDelayInSeconds;

            ElapsedTime = 0;
        }

        /// <summary>
        /// Runs the effect's Action method.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the effect's Action.</returns>
        internal bool Run()
        {
            ElapsedTime += DeltaTime;

            return Action();
        }

        /// <summary>
        /// Resets the effect so it can be run again.
        /// </summary>
        protected internal virtual void Reset()
        {
            ElapsedTime = 0;
            IsFirstRun = true;
        }

        /// <summary>
        /// Used to define effects.
        /// </summary>
        /// <returns>Returns a bool indicating the result of the Action.</returns>
        protected abstract bool Action();
    }
}