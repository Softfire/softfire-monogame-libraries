using System;
using System.Threading.Tasks;

namespace Softfire.MonoGame.UI
{
    public sealed class UIMenuItemFunction
    {
        /// <summary>
        /// Is Enabled?
        /// Used to check if the action should be actioned.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Function.
        /// </summary>
        public Func<Task<bool>> Function { get; }

        /// <summary>
        /// State.
        /// Current function state.
        /// </summary>
        public States State { get; private set; }

        /// <summary>
        /// States.
        /// </summary>
        public enum States
        {
            Stopped,
            Running
        }

        /// <summary>
        /// UI Menu Item Function Constructor.
        /// </summary>
        /// <param name="identifier">A unique identifier.</param>
        /// <param name="function">The delegate function to perform.</param>
        /// <param name="isEnabled">Is the function enabled?</param>
        public UIMenuItemFunction(string identifier, Func<Task<bool>> function, bool isEnabled = true)
        {
            Identifier = identifier;
            Function = function;
            IsEnabled = isEnabled;

            State = States.Stopped;
        }

        /// <summary>
        /// Run.
        /// Performs the assigned function on a background thread.
        /// </summary>
        /// <returns>Returns a bool indicating whether the function had run it's course.</returns>
        public async Task<bool> Run()
        {
            var result = false;

            if (IsEnabled &&
                Function != null &&
                State == States.Stopped)
            {
                State = States.Running;
                result = await Function.Invoke();
                State = States.Stopped;
            }

            return result;
        }
    }
}