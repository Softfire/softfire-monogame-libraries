using Softfire.MonoGame.CORE.Graphics.Drawing;

namespace Softfire.MonoGame.UI.Items
{
    /// <summary>
    /// A class for defining paddings for a ui element.
    /// </summary>
    public class UIPadding : IMonoGameDrawingPaddingComponent
    {
        /// <summary>
        /// The top padding.
        /// </summary>
        public int Top { get; private set; }

        /// <summary>
        /// The bottom padding.
        /// </summary>
        public int Bottom { get; private set; }

        /// <summary>
        /// The left padding.
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// The right padding.
        /// </summary>
        public int Right { get; private set; }

        /// <summary>
        /// Controls padding offsets for UI elements.
        /// </summary>
        public UIPadding() : this(0)
        {
            
        }

        /// <summary>
        /// Controls padding offsets for UI elements.
        /// </summary>
        /// <param name="sides">The top, bottom, left and right padding offset. Intaken as an <see cref="int"/>.</param>
        public UIPadding(int sides) : this(sides, sides, sides, sides)
        {
            
        }

        /// <summary>
        /// Controls padding offsets for UI elements.
        /// </summary>
        /// <param name="topBottom">The top and bottom padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="leftRight">The left and right padding offset. Intaken as an <see cref="int"/>.</param>
        public UIPadding(int topBottom, int leftRight) : this(topBottom, leftRight, topBottom, leftRight)
        {
            
        }

        /// <summary>
        /// Controls padding offsets for UI elements.
        /// </summary>
        /// <param name="top">The top padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="leftRight">The left and right padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="bottom">The bottom padding offset. Intaken as an <see cref="int"/>.</param>
        public UIPadding(int top, int leftRight, int bottom) : this(top, leftRight, bottom, leftRight)
        {

        }

        /// <summary>
        /// Controls padding offsets for UI elements.
        /// </summary>
        /// <param name="top">The top padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="right">The right padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="bottom">The bottom padding offset. Intaken as an <see cref="int"/>.</param>
        /// <param name="left">The left padding offset. Intaken as an <see cref="int"/>.</param>
        public UIPadding(int top, int right, int bottom, int left)
        {
            Top = top >= 0 ? top : 0;
            Right = right >= 0 ? right : 0;
            Bottom = bottom >= 0 ? bottom : 0;
            Left = left >= 0 ? left : 0;
        }

        /// <summary>
        /// Sets the element's top, bottom, left and right paddings.
        /// </summary>
        /// <param name="sides">The padding, in pixels, to add to the top, right, bottom and left sides.</param>
        public void SetPadding(int sides)
        {
            SetPadding(sides, sides, sides, sides);
        }

        /// <summary>
        /// Sets the element's top, bottom, left and right paddings.
        /// </summary>
        /// <param name="topBottom">The padding, in pixels, to add to the top and bottom sides.</param>
        /// <param name="leftRight">The padding, in pixels, to add to the left and right sides.</param>
        public void SetPadding(int topBottom, int leftRight)
        {
            SetPadding(topBottom, leftRight, topBottom, leftRight);
        }

        /// <summary>
        /// Sets the element's top, bottom, left and right paddings.
        /// </summary>
        /// <param name="top">The padding, in pixels, to add to the top side.</param>
        /// <param name="bottom">The padding, in pixels, to add to the bottom side.</param>
        /// <param name="left">The padding, in pixels, to add to the left side.</param>
        /// <param name="right">The padding, in pixels, to add to the right side.</param>
        public void SetPadding(int top, int right, int bottom, int left)
        {
            Top = top >= 0 ? top : 0;
            Right = right >= 0 ? right : 0;
            Bottom = bottom >= 0 ? bottom : 0;
            Left = left >= 0 ? left : 0;
        }
    }
}