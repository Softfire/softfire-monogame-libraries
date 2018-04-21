using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UTIL
{
    public static class Calculations
    {
        /// <summary>
        /// Convert Degrees To Radians.
        /// </summary>
        /// <param name="angleInDegrees">The angle in Degrees. Intaken as a double.</param>
        /// <returns>Rturns the angle in Radians.</returns>
        public static double ConvertToRadians(double angleInDegrees)
        {
            return (MathHelper.Pi / 180) * angleInDegrees;
        }

        /// <summary>
        /// Convert Radians To Degrees.
        /// </summary>
        /// <param name="angleInRadians">The angle in Radians. Intaken as a double.</param>
        /// <returns>Returns the angle in Degrees.</returns>
        public static double ConvertToDegrees(double angleInRadians)
        {
            return angleInRadians * (180.0 / MathHelper.Pi);
        }

        /// <summary>
        /// Is Even.
        /// </summary>
        /// <param name="value">The value to inspect. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the value given was divisible by 2.</returns>
        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        /// Is Odd.
        /// </summary>
        /// <param name="value">The value to inspect. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the value given is not divisible by 2.</returns>
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}