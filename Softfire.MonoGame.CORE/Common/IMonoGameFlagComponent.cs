using System;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// A standard set of flagging methods for setting, adding, removing, checking and clearing flags.
    /// </summary>
    public interface IMonoGameFlagComponent<T> where T : Enum
    {
        /// <summary>
        /// The flags.
        /// </summary>
        /// <remarks>Annotate this property with <see cref="FlagsAttribute"/>.</remarks>
        T Flags { get; }

        /// <summary>
        /// Sets the flag.
        /// </summary>
        /// <param name="flag">The flag to set.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        bool SetFlag(T flag, bool result);

        /// <summary>
        /// Determines whether the flag is set.
        /// </summary>
        /// <param name="flag">The flag to check if it is set.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the flag is set.</returns>
        bool IsFlagSet(T flag);

        /// <summary>
        /// Removes the flag, if set.
        /// </summary>
        /// <param name="flag">The flag to remove if it is set.</param>
        void RemoveFlag(T flag);

        /// <summary>
        /// Adds the flag.
        /// </summary>
        /// <param name="flag">The flag to add.</param>
        void AddFlag(T flag);

        /// <summary>
        /// Clears the flags.
        /// </summary>
        void ClearFlags();
    }
}