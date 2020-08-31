using System;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// Flags for defining the current focus state.
    /// </summary>
    [Flags]
    public enum FocusStates
    {
        /// <summary>
        /// Declares that the object is neither focused or hovered.
        /// </summary>
        None = 0,
        /// <summary>
        /// Declares that the object has focus.
        /// </summary>
        IsFocused = 1 << 0,
        /// <summary>
        /// Declares that the object is being hovered over.
        /// </summary>
        IsHovered = 1 << 1
    }

    /// <summary>
    /// An interface for determining focus on 2D objects.
    /// </summary>
    public interface IMonoGameFocusComponent
    {
        /// <summary>
        /// The object's focus state flag.
        /// </summary>
        FocusStates FocusState { get; set; }

        /// <summary>
        /// Sets the state flag.
        /// </summary>
        /// <param name="state">The state flag to set.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        bool SetState(FocusStates state, bool result);

        /// <summary>
        /// Determines whether the state flag is set.
        /// </summary>
        /// <param name="state">The state flag to check.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the state flag is set.</returns>
        bool IsStateSet(FocusStates state);

        /// <summary>
        /// Removes the state flag, if set.
        /// </summary>
        /// <param name="state">The state flag to remove if it is set.</param>
        void RemoveState(FocusStates state);

        /// <summary>
        /// Adds the state flag.
        /// </summary>
        /// <param name="state">The state flag to add.</param>
        void AddState(FocusStates state);

        /// <summary>
        /// Clears the state flags.
        /// </summary>
        void ClearStates();
    }
}