using System;

namespace Softfire.MonoGame.CORE
{
    /// <summary>
    /// A Size struct with floats.
    /// </summary>
    public class SizeF : IEquatable<SizeF>
    {
        /// <summary>
        /// Returns an empty <see cref="SizeF"/>.
        /// </summary>
        public static SizeF Empty => new SizeF();

        #region Fields

        /// <summary>
        /// The <see cref="SizeF"/>'s internal width value.
        /// </summary>
        private float _width;

        /// <summary>
        /// The <see cref="SizeF"/>'s internal minimum width value.
        /// </summary>
        private float _widthMin;

        /// <summary>
        /// The <see cref="SizeF"/>'s internal maximum width value.
        /// </summary>
        private float _widthMax = float.MaxValue;

        /// <summary>
        /// The <see cref="SizeF"/>'s internal height value.
        /// </summary>
        private float _height;

        /// <summary>
        /// The <see cref="SizeF"/>'s internal minimum height value.
        /// </summary>
        private float _heightMin;

        /// <summary>
        /// The <see cref="SizeF"/>'s internal maximum height value.
        /// </summary>
        private float _heightMax = float.MaxValue;

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="SizeF"/>'s width.
        /// </summary>
        public float Width
        {
            get => _width;
            set => _width = value >= WidthMin &&
                            value <= WidthMax ? value :
                            value < WidthMin ? WidthMin :
                            value > WidthMax ? WidthMax : _width;
        }

        /// <summary>
        /// The <see cref="SizeF"/>'s minimum width.
        /// </summary>
        public float WidthMin
        {
            get => _widthMin;
            set => _widthMin = value > 0 && value <= WidthMax ? value : _widthMin;
        }

        /// <summary>
        /// The <see cref="SizeF"/>'s maximum width.
        /// </summary>
        public float WidthMax
        {
            get => _widthMax;
            set => _widthMax = value >= WidthMin && value <= float.MaxValue ? value : _widthMax;
        }

        /// <summary>
        /// The <see cref="SizeF"/>'s height.
        /// </summary>
        public float Height
        {
            get => _height;
            set => _height = value >= HeightMin &&
                             value <= HeightMax ? value :
                             value < HeightMin ? HeightMin :
                             value > HeightMax ? HeightMax : _height;
        }

        /// <summary>
        /// The <see cref="SizeF"/>'s minimum height.
        /// </summary>
        public float HeightMin
        {
            get => _heightMin;
            set => _heightMin = value > 0 && value <= HeightMax ? value : _heightMin;
        }

        /// <summary>
        /// The <see cref="SizeF"/>'s maximum height.
        /// </summary>
        public float HeightMax
        {
            get => _heightMax;
            set => _heightMax = value >= HeightMin && value <= float.MaxValue ? value : _heightMax;
        }

        #endregion

        /// <summary>
        /// Determines whether the <see cref="SizeF"/>'s <see cref="Width"/> or <see cref="Height"/> are zero.
        /// </summary>
        public bool IsEmpty => Width <= 0 && Height <= 0;

        /// <summary>
        /// Creates a new <see cref="SizeF"/> of the same size.
        /// </summary>
        /// <param name="size">The size to mirror on creation. Intaken as a <see cref="SizeF"/>.</param>
        public SizeF(SizeF size)
        {
            Width = size.Width > 0f ? size.Width : 0f;
            Height = size.Height > 0f ? size.Height : 0f;
        }

        /// <summary>
        /// Creates a new <see cref="SizeF"/> with the provided width and height.
        /// </summary>
        /// <param name="width">The width to set. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The height to set. Intaken as a <see cref="float"/>.</param>
        public SizeF(float width = 10f, float height = 10f)
        {
            Width = width > 0f ? width : 0f;
            Height = height > 0f ? height : 0f;
        }
        
        /// <summary>
        /// Increases size.
        /// </summary>
        /// <param name="size">The the amount to increase the <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Inflate(SizeF size)
        {
            Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// Increases size.
        /// </summary>
        /// <param name="width">The amount to increase the <see cref="Width"/> by. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The amount to increment the <see cref="Height"/> by. Intaken as a <see cref="float"/>.</param>
        public void Inflate(float width, float height)
        {
            Width += width * 2;
            Height += height * 2;
        }

        /// <summary>
        /// Decreases size.
        /// </summary>
        /// <param name="size">The the amount to decrease the <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Deflate(SizeF size)
        {
            Deflate(size.Width, size.Height);
        }

        /// <summary>
        /// Decreases size.
        /// </summary>
        /// <param name="width">The amount to decrement the <see cref="Width"/> by. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The amount to decrement the <see cref="Height"/> by. Intaken as a <see cref="float"/>.</param>
        public void Deflate(float width, float height)
        {
            Width -= width * 2;
            Height -= height * 2;
        }

        /// <summary>
        /// Adds two sizes together.
        /// </summary>
        /// <param name="left">The <see cref="SizeF"/> on the left. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The <see cref="SizeF"/> on the right. Intaken as a <see cref="SizeF"/>.</param>
        /// <returns>Returns a new <see cref="SizeF"/> of the combined sizes.</returns>
        public static SizeF Add(SizeF left, SizeF right) => new SizeF(left.Width + right.Width, left.Height + right.Height);

        /// <summary>
        /// Subtracts two sizes.
        /// </summary>
        /// <param name="left">The <see cref="SizeF"/> on the left. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The <see cref="SizeF"/> on the right. Intaken as a <see cref="SizeF"/>.</param>
        /// <returns>Returns a new <see cref="SizeF"/> of the reduced size.</returns>
        public static SizeF Subtract(SizeF left, SizeF right) => new SizeF(left.Width - right.Width, left.Height - right.Height);

        /// <summary>
        /// Multiplies a <see cref="SizeF"/> by a multiplier.
        /// </summary>
        /// <param name="size">The <see cref="SizeF"/> to multiply. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="multiplier">The multiplier. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns a multiplied <see cref="SizeF"/>.</returns>
        private static SizeF Multiply(SizeF size, float multiplier) => new SizeF(size.Width * multiplier, size.Height * multiplier);

        /// <summary>
        /// Divides a <see cref="SizeF"/> by a divisor.
        /// </summary>
        /// <param name="size">The <see cref="SizeF"/> to divide. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="divisor">The divisor. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns a divided <see cref="SizeF"/>.</returns>
        public static SizeF Divide(SizeF size, float divisor) => new SizeF(size.Width / divisor, size.Height / divisor);

        /// <summary>
        /// An addition operation between sizes.
        /// </summary>
        /// <param name="left">The <see cref="SizeF"/> on the left. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The <see cref="SizeF"/> on the right. Intaken as a <see cref="SizeF"/>.</param>
        /// <returns>Returns a <see cref="SizeF"/> of the combined sizes.</returns>
        public static SizeF operator +(SizeF left, SizeF right) => Add(left, right);

        /// <summary>
        /// A subtraction operation between sizes.
        /// </summary>
        /// <param name="left">The <see cref="SizeF"/> on the left. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The <see cref="SizeF"/> on the right. Intaken as a <see cref="SizeF"/>.</param>
        /// <returns></returns>
        public static SizeF operator -(SizeF left, SizeF right) => Subtract(left, right);
        
        /// <summary>
        /// A multiplication operation.
        /// </summary>
        /// <param name="left">The parameter on the left of the operator. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The parameter on the right of the operator. Intaken as a <see cref="float"/>.</param>
        public static SizeF operator *(SizeF left, float right) => Multiply(left, right);
        
        /// <summary>
        /// A division operation.
        /// </summary>
        /// <param name="left">The parameter on the left of the operator. Intaken as a <see cref="SizeF"/>.</param>
        /// <param name="right">The parameter on the right of the operator. Intaken as a <see cref="float"/>.</param>
        public static SizeF operator /(SizeF left, float right) => Divide(left, right);

        /// <summary>
        /// Converts a <see cref="Size"/> to a <see cref="SizeF"/>.
        /// </summary>
        /// <param name="size">The <see cref="SizeF"/> to convert. Intaken as a <see cref="SizeF"/>.</param>
        public static implicit operator SizeF(Size size) => new SizeF(size.Width, size.Height);

        /// <summary>
        /// Converts a <see cref="SizeF"/> to a <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The <see cref="SizeF"/> to convert. Intaken as a <see cref="SizeF"/>.</param>
        public static explicit operator Size(SizeF size) => new Size((int)size.Width, (int)size.Height);

        /// <summary>
        /// Determines whether the <see cref="SizeF"/>'s values are equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="SizeF"/>.</param>
        /// <param name="right">The right hand <see cref="SizeF"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="SizeF"/>'s values are equal.</returns>
        public static bool operator ==(SizeF left, SizeF right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the <see cref="SizeF"/>'s values are not equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="SizeF"/>.</param>
        /// <param name="right">The right hand <see cref="SizeF"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="SizeF"/>'s values are not equal.</returns>
        public static bool operator !=(SizeF left, SizeF right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// <see cref="SizeF"/>'s <see cref="object.ToString()"/> override.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> containing the characteristics of the <see cref="SizeF"/>.</returns>
        public override string ToString() => $@"Width: {Width}, Height: {Height}";

        /// <summary>
        /// Compares the equality of the <see cref="SizeF"/>'s.
        /// </summary>
        /// <param name="other">The other <see cref="SizeF"/> to compare against.</param>
        /// <returns>Returns a boolean indication whether the <see cref="SizeF"/>'s are equal.</returns>
        public bool Equals(SizeF other)
        {
            return Width.Equals(other.Width) &&
                   Height.Equals(other.Height);
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

            return obj is SizeF other && Equals(other);
        }

        /// <summary>
        /// Returns a <see cref="ValueType.GetHashCode"/> for comparing equality.
        /// </summary>
        /// <returns>Returns the <see cref="SizeF"/>'s <see cref="ValueType.GetHashCode"/> as an <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 397) ^ Height.GetHashCode();
            }
        }
    }
}