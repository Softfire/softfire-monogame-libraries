namespace Softfire.MonoGame.CORE.Graphics.Drawing
{
    /// <summary>
    /// An interface for controlling the positional paddings of 2D objects.
    /// </summary>
    public interface IMonoGameDrawingPaddingComponent
    {
        /// <summary>
        /// The area of space inside the top of object.
        /// </summary>
        int Top { get; }

        /// <summary>
        /// The area of space inside to the right of object.
        /// </summary>
        int Right { get; }

        /// <summary>
        /// The area of space inside the bottom of object.
        /// </summary>
        int Bottom { get; }

        /// <summary>
        /// The area of space inside to the left of object.
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Sets the padding, the area of space inside, on all sides.
        /// </summary>
        /// <param name="sides">The amount of space on the inside of each side of the object. Intaken as an <see cref="int"/>.</param>
        void SetPadding(int sides);

        /// <summary>
        /// Sets the paddings, the area of space inside, of the top and bottom to be equal and the left and right side to be equal.
        /// </summary>
        /// <param name="topBottom">The amount of space on the inside of the top and bottom sides of the object. Intaken as an <see cref="int"/>.</param>
        /// <param name="leftRight">The amount of space on the inside to the left and right sides of the object. Intaken as an <see cref="int"/>.</param>
        void SetPadding(int topBottom, int leftRight);

        /// <summary>
        /// Sets the padding, the area of space inside, on each side.
        /// </summary>
        /// <param name="top">The amount of space on the inside of the top of the object. Intaken as an <see cref="int"/>.</param>
        /// <param name="right">The amount of space to the right on inside of the object. Intaken as an <see cref="int"/>.</param>
        /// <param name="bottom">The amount of space on the inside of the bottom of the object. Intaken as an <see cref="int"/>.</param>
        /// <param name="left">The amount of space to the left on inside of the object. Intaken as an <see cref="int"/>.</param>
        void SetPadding(int top, int right, int bottom, int left);
    }
}