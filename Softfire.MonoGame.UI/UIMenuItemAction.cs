using System;

namespace Softfire.MonoGame.UI
{
    public sealed class UIMenuItemAction
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
        public Action Action { get; }

        /// <summary>
        /// UI Menu Item Action.
        /// </summary>
        /// <param name="identifier">A unique identifier.</param>
        /// <param name="action">The delegate action to perform synchronously.</param>
        /// <param name="isEnabled">Is the action enabled?</param>
        public UIMenuItemAction(string identifier, Action action, bool isEnabled = true)
        {
            Identifier = identifier;
            Action = action;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Run.
        /// Performs the assigned action synchronously.
        /// </summary>
        /// <returns>Returns a bool indicating whether the assigned ACtion was performed.</returns>
        public bool Run()
        {
            var result = false;

            if (IsEnabled &&
                Action != null)
            {
                Action.Invoke();
                result = true;
            }

            return result;
        }
    }
}