using System;

namespace Softfire.MonoGame.CORE
{
    /// <summary>
    /// A Size struct.
    /// </summary>
    public class Size : IEquatable<Size>
    {
        /// <summary>
        /// Returns an empty <see cref="Size"/>.
        /// </summary>
        public static Size Empty => new Size();

        #region Fields

        /// <summary>
        /// The <see cref="Size"/>'s internal width value.
        /// </summary>
        private int _width;

        /// <summary>
        /// The <see cref="Size"/>'s internal minimum width value.
        /// </summary>
        private int _widthMin;

        /// <summary>
        /// The <see cref="Size"/>'s internal maximum width value.
        /// </summary>
        private int _widthMax = int.MaxValue;

        /// <summary>
        /// The <see cref="Size"/>'s internal height value.
        /// </summary>
        private int _height;

        /// <summary>
        /// The <see cref="Size"/>'s internal minimum height value.
        /// </summary>
        private int _heightMin;

        /// <summary>
        /// The <see cref="Size"/>'s internal maximum height value.
        /// </summary>
        private int _heightMax = int.MaxValue;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Size"/>'s width.
        /// </summary>
        public int Width
        {
            get => _width;
            set => _width = value >= WidthMin &&
                            value <= WidthMax ? value :
                            value < WidthMin ? WidthMin :
                            value > WidthMax ? WidthMax : _width;
        }

        /// <summary>
        /// The <see cref="Size"/>'s minimum width.
        /// </summary>
        public int WidthMin
        {
            get => _widthMin;
            set => _widthMin = value > 0 && value <= WidthMax ? value : _widthMin;
        }

        /// <summary>
        /// The <see cref="Size"/>'s maximum width.
        /// </summary>
        public int WidthMax
        {
            get => _widthMax;
            set => _widthMax = value >= WidthMin && value <= int.MaxValue ? value : _widthMax;
        }

        /// <summary>
        /// The <see cref="Size"/>'s height.
        /// </summary>
        public int Height
        {
            get => _height;
            set => _height = value >= HeightMin &&
                             value <= HeightMax ? value :
                             value < HeightMin ? HeightMin :
                             value > HeightMax ? HeightMax : _height;
        }

        /// <summary>
        /// The <see cref="Size"/>'s minimum height.
        /// </summary>
        public int HeightMin
        {
            get => _heightMin;
            set => _heightMin = value > 0 && value <= HeightMax ? value : _heightMin;
        }

        /// <summary>
        /// The <see cref="Size"/>'s maximum height.
        /// </summary>
        public int HeightMax
        {
            get => _heightMax;
            set => _heightMax = value >= HeightMin && value <= int.MaxValue ? value : _heightMax;
        }
        
        #endregion

        /// <summary>
        /// Determines whether the <see cref="Size"/>'s <see cref="Width"/> or <see cref="Height"/> are zero.
        /// </summary>
        public bool IsEmpty => Width <= 0 && Height <= 0;

        /// <summary>
        /// Creates a new <see cref="Size"/> of the same size.
        /// </summary>
        /// <param name="size">The size to mirror on creation. Intaken as a <see cref="Size"/>.</param>
        public Size(Size size)
        {
            Width = size.Width > 0 ? size.Width : 0;
            Height = size.Height > 0 ? size.Height : 0;
        }

        /// <summary>
        /// Creates a new <see cref="Size"/> with the provided width and height.
        /// </summary>
        /// <param name="width">The width to set. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The height to set. Intaken as a <see cref="float"/>.</param>
        public Size(int width = 10, int height = 10)
        {
            Width = width > 0 ? width : 0;
            Height = height > 0 ? height : 0;
        }

        /// <summary>
        /// Increases size.
        /// </summary>
        /// <param name="size">The the amount to increase the <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Inflate(Size size)
        {
            Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// Increases size.
        /// </summary>
        /// <param name="width">The amount to increase the <see cref="Width"/> by. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">The amount to increment the <see cref="Height"/> by. Intaken as an <see cref="int"/>.</param>
        public void Inflate(int width, int height)
        {
            Width += width * 2;
            Height += height * 2;
        }

        /// <summary>
        /// Decreases size.
        /// </summary>
        /// <param name="size">The the amount to decrease the <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Deflate(Size size)
        {
            Deflate(size.Width, size.Height);
        }

        /// <summary>
        /// Decreases size.
        /// </summary>
        /// <param name="width">The amount to decrement the <see cref="Width"/> by. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">The amount to decrement the <see cref="Height"/> by. Intaken as an <see cref="int"/>.</param>
        public void Deflate(int width, int height)
        {
            Width -= width * 2;
            Height -= height * 2;
        }

        /// <summary>
        /// Adds two sizes together.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="Size"/> on the right. Intaken as a <see cref="Size"/>.</param>
        /// <returns>Returns a new <see cref="Size"/> of the combined sizes.</returns>
        public static Size Add(Size left, Size right) => new Size(left.Width + right.Width, left.Height + right.Height);

        /// <summary>
        /// Subtracts two sizes.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="Size"/> on the right. Intaken as a <see cref="Size"/>.</param>
        /// <returns>Returns a new <see cref="Size"/> of the reduced size.</returns>
        public static Size Subtract(Size left, Size right) => new Size(left.Width - right.Width, left.Height - right.Height);

        /// <summary>
        /// Multiplies a <see cref="Size"/> by a multiplier.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to multiply. Intaken as a <see cref="Size"/>.</param>
        /// <param name="multiplier">The multiplier. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a multiplied <see cref="Size"/>.</returns>
        private static Size Multiply(Size size, int multiplier) => new Size(size.Width * multiplier, size.Height * multiplier);

        /// <summary>
        /// Divides a <see cref="Size"/> by a divisor.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to divide. Intaken as a <see cref="Size"/>.</param>
        /// <param name="divisor">The divisor. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a divided <see cref="Size"/>.</returns>
        public static Size Divide(Size size, int divisor) => new Size(size.Width / divisor, size.Height / divisor);

        /// <summary>
        /// An addition operation between sizes.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="Size"/> on the right. Intaken as a <see cref="Size"/>.</param>
        /// <returns>Returns a <see cref="Size"/> of the combined sizes.</returns>
        public static Size operator +(Size left, Size right) => Add(left, right);

        /// <summary>
        /// A subtraction operation between sizes.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="Size"/> on the right. Intaken as a <see cref="Size"/>.</param>
        /// <returns></returns>
        public static Size operator -(Size left, Size right) => Subtract(left, right);

        /// <summary>
        /// A multiplication operation.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="int"/> on the right. Intaken as a <see cref="int"/>.</param>
        public static Size operator *(Size left, int right) => Multiply(left, right);

        /// <summary>
        /// A division operation.
        /// </summary>
        /// <param name="left">The <see cref="Size"/> on the left. Intaken as a <see cref="Size"/>.</param>
        /// <param name="right">The <see cref="int"/> on the right. Intaken as a <see cref="int"/>.</param>
        public static Size operator /(Size left, int right) => Divide(left, right);

        /// <summary>
        /// Converts a <see cref="Size"/> to a <see cref="SizeF"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to convert. Intaken as a <see cref="Size"/>.</param>
        public static implicit operator SizeF(Size size) => new SizeF(size.Width, size.Height);

        /// <summary>
        /// Converts a <see cref="SizeF"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to convert. Intaken as a <see cref="Size"/>.</param>
        public static explicit operator Size(SizeF size) => new Size((int)size.Width, (int)size.Height);

        /// <summary>
        /// Determines whether the <see cref="Size"/>'s values are equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="Size"/>.</param>
        /// <param name="right">The right hand <see cref="Size"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="Size"/>'s values are equal.</returns>
        public static bool operator ==(Size left, Size right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the <see cref="Size"/>'s values are not equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="Size"/>.</param>
        /// <param name="right">The right hand <see cref="Size"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="Size"/>'s values are not equal.</returns>
        public static bool operator !=(Size left, Size right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// <see cref="Size"/>'s <see cref="object.ToString()"/> override.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> containing the characteristics of the <see cref="Size"/>.</returns>
        public override string ToString() => $@"Width: {Width}, Height: {Height}";

        /// <summary>
        /// Compares the equality of the <see cref="Size"/>'s.
        /// </summary>
        /// <param name="other">The other <see cref="Size"/> to compare against.</param>
        /// <returns>Returns a boolean indication whether the <see cref="Size"/>'s are equal.</returns>
        public bool Equals(Size other)
        {
            return Width == other.Width &&
                   Height == other.Height;
        }

        /// <summary>
        /// Compares object equality between objects.
        /// </summary>
        /// <param name="obj">An object to compare for equality.</param>
        /// <returns>Returns a boolean indicating the equality of the objects.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Size other && Equals(other);
        }

        /// <summary>
        /// Returns a <see cref="ValueType.GetHashCode"/> for comparing equality.
        /// </summary>
        /// <returns>Returns the <see cref="Size"/>'s <see cref="ValueType.GetHashCode"/> as an <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }
    }
}