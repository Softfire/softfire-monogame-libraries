using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.V2
{
    /// <summary>
    /// A class containing useful mathematical methods.
    /// </summary>
    public static class MonoMath
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="angleInDegrees">The angle in degrees. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the angle in radians as a <see cref="double"/>.</returns>
        public static double ToRadians(double angleInDegrees) => (MathHelper.Pi / 180) * angleInDegrees;

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="angleInRadians">The angle in radians. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the angle in degrees as a <see cref="double"/>.</returns>
        public static double ToDegrees(double angleInRadians) => angleInRadians * (180.0 / MathHelper.Pi);

        /// <summary>
        /// Checks if the provided value is divisible by two, into a whole number, making it an even number.
        /// </summary>
        /// <param name="value">The value to inspect. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the value given was divisible by 2.</returns>
        public static bool IsEven(int value) => value % 2 == 0;

        /// <summary>
        /// Checks if the provided value is not divisible by two, into a whole number, making it an odd number.
        /// </summary>
        /// <param name="value">The value to inspect. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the value given was not divisible by 2.</returns>
        public static bool IsOdd(int value) => value % 2 != 0;
    }
}