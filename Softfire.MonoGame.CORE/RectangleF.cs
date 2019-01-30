using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE
{
    /// <summary>
    /// Describes a rectangle with floats.
    /// </summary>
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        /// Returns an empty <see cref="RectangleF"/>.
        /// </summary>
        /// <remarks><see cref="X"/>, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height"/> are zero.</remarks>
        public static RectangleF Empty => new RectangleF();

        /// <summary>
        /// The X coordinate of the rectangle.
        /// </summary>
        /// <remarks>
        /// Can be modified using <see cref="Offset(float,float)"/>,
        /// <see cref="Offset(Vector2)"/> or with <see cref="Position()"/></remarks>
        public float X { get; set; }

        /// <summary>
        /// The Y coordinate of the rectangle.
        /// </summary>
        /// <remarks>
        /// Can be modified using <see cref="Offset(float,float)"/>,
        /// <see cref="Offset(Vector2)"/> or with <see cref="Position()"/></remarks>
        public float Y { get; set; }

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// The top most <see cref="Y"/> coordinate of the rectangle.
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// The right most <see cref="X"/> coordinate of the rectangle.
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// The bottom most <see cref="Y"/> coordinate of the rectangle.
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// The left most <see cref="X"/> coordinate of the rectangle.
        /// </summary>
        public float Left => X;

        /// <summary>
        /// Checks to see if the rectangle's <see cref="Width"/> or <see cref="Height"/> are zero.
        /// </summary>
        public bool IsEmpty => (Width <= 0f) || (Height <= 0f);

        /// <summary>
        /// The rectangle's position. A Vector2 containing the rectangle's <see cref="X"/> and <see cref="X"/> coordinates.
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// The rectangle's size. A <see cref="SizeF"/> containing the rectangle's <see cref="Width"/> and <see cref="Height"/> respectively.
        /// </summary>
        /// <returns>Returns a new <see cref="SizeF"/> with the rectangle's current <see cref="Width"/> and <see cref="Height"/>.</returns>
        public SizeF Size
        {
            get => new SizeF(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }
        
        /// <summary>
        /// The rectangle's center most point.
        /// </summary>
        /// <returns>Returns the location of the center most point within this <see cref="RectangleF"/> as a <see cref="Vector2"/>.</returns>
        public Vector2 Origin => new Vector2(Width / 2f, Height / 2f);

        /// <summary>
        /// The rectangle's center most positional coordinates.
        /// </summary>
        /// <returns>Returns the location of the center of the rectangle as a <see cref="Vector2"/>.</returns>
        /// <remarks>The center includes it's current <see cref="Position"/> in the calculation.</remarks>
        public Vector2 Center => new Vector2(X + (Width / 2f), Y + (Height / 2f));

        /// <summary>
        /// The rectangle's top left coordinate.
        /// </summary>
        /// <returns>Returns the top-left coordinates of the rectangle as a <see cref="Vector2"/>.</returns>
        public Vector2 TopLeft => new Vector2(X, Y);

        /// <summary>
        /// The rectangle's top right coordinate.
        /// </summary>
        /// <returns>Returns the top-right coordinates of the rectangle as a <see cref="Vector2"/>.</returns>
        public Vector2 TopRight => new Vector2(X + Width, Y);

        /// <summary>
        /// The rectangle's top right coordinate.
        /// </summary>
        /// <returns>Returns the bottom-right coordinates of the rectangle as a <see cref="Vector2"/>.</returns>
        public Vector2 BottomRight => new Vector2(X + Width, Y + Height);

        /// <summary>
        /// The rectangle's bottom left coordinate.
        /// </summary>
        /// <returns>Returns the bottom-left coordinates of the rectangle as a <see cref="Vector2"/>.</returns>
        public Vector2 BottomLeft => new Vector2(X, Y + Height);

        /// <summary>
        /// A rectangle consisting of floats.
        /// </summary>
        /// <param name="x">The rectangle's <see cref="X"/> coordinate along the <see cref="X"/> axis. Intaken as a <see cref="float"/>.</param>
        /// <param name="y">The rectangle's <see cref="Y"/> coordinate along the <see cref="Y"/> axis. Intaken as a <see cref="float"/>.</param>
        /// <param name="width">The rectangle's <see cref="Width"/>. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The rectangle's <see cref="Height"/>. Intaken as a <see cref="float"/>.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// A rectangle consisting of floats.
        /// </summary>
        /// <param name="position">The rectangle's <see cref="Position"/> along the X and Y axis. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The rectangle's <see cref="Width"/>. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The rectangle's <see cref="Height"/>. Intaken as a <see cref="float"/>.</param>
        public RectangleF(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// A rectangle consisting of floats.
        /// </summary>
        /// <param name="position">The rectangle's position along the <see cref="X"/> and <see cref="Y"/> axis. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="size">The rectangle's size. A <see cref="SizeF"/> containing the rectangle's <see cref="Width"/> and <see cref="Height"/> respectively.</param>
        public RectangleF(Vector2 position, SizeF size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Generates a new rectangle between the provided vectors.
        /// </summary>
        /// <param name="vectorOne">The starting vector for the new rectangle. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="vectorTwo">The ending vector for the new rectangle. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a new rectangle created from the provided vectors.</returns>
        public static RectangleF Generate(Vector2 vectorOne, Vector2 vectorTwo)
        {
            return new RectangleF
            {
                X = vectorTwo.X,
                Y = vectorTwo.Y,
                Width = vectorOne.X - vectorTwo.X,
                Height = vectorOne.Y - vectorTwo.Y
            };
        }

        /// <summary>
        /// Generates a new rectangle between this rectangle and a vector.
        /// </summary>
        /// <param name="vector">The ending vector for the new rectangle. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a new rectangle created from the provided vector.</returns>
        public RectangleF GenerateTo(Vector2 vector) => Generate(Position, vector);

        /// <summary>
        /// Generates a new rectangle containing the area of overlap between two rectangles.
        /// </summary>
        /// <param name="left">A rectangle on the left. Intaken as a <see cref="RectangleF"/>.</param>
        /// <param name="right">The rectangle on the right. Intaken as a <see cref="RectangleF"/>.</param>
        /// <returns>Returns a new rectangle containing the area of overlap between two rectangles, if overlapping, other an empty rectangle is returned.</returns>
        public static RectangleF Overlap(RectangleF left, RectangleF right)
        {
            var max = new Vector2(Math.Max(left.X, right.X),
                                  Math.Max(left.Y, right.Y));

            var min = new Vector2(Math.Min(left.X + left.Width, right.X + right.Width),
                                  Math.Min(left.Y + left.Height, right.Y + right.Height));

            return min.X >= max.X &&
                   min.Y >= max.Y ? Generate(min, max) : Empty;
        }

        /// <summary>
        /// Generates a new rectangle containing the area of overlap between this rectangle and another rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to check against for an overlap.</param>
        /// <returns>Returns a new rectangle containing the area of overlap between two rectangles, if overlapping, other an empty rectangle is returned.</returns>
        public RectangleF OverlapWith(RectangleF rectangle) => Overlap(this, rectangle);

        /// <summary>
        /// Determines whether the rectangles intersect with each other.
        /// </summary>
        /// <param name="left">A <see cref="RectangleF"/> on the left.</param>
        /// <param name="right">The <see cref="RectangleF"/> on the right.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the rectangles intersect.</returns>
        public static bool Intersect(RectangleF left, RectangleF right)
        {
            return (right.X < left.X + left.Width) && (left.X < right.X + right.Width) &&
                   (right.Y < left.Y + left.Height) && (left.Y < right.Y + right.Height);
        }

        /// <summary>
        /// Determines whether the rectangles intersect with each other.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to check against for an intersection.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the rectangles intersect.</returns>
        public bool IntersectsWith(RectangleF rectangle) => Intersect(this, rectangle);

        /// <summary>
        /// Determines whether the rectangle is entirely contained within.
        /// </summary>
        /// <param name="rectangle">The see <cref="RectangleF"/> to check if contained within.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the rectangle is contained within.</returns>
        public bool Contains(RectangleF rectangle) => X <= rectangle.X &&
                                                      rectangle.X + rectangle.Width <= X + Width &&
                                                      Y <= rectangle.Y &&
                                                      rectangle.Y + rectangle.Height <= Y + Height;

        /// <summary>
        /// Determines whether the coordinate is entirely contained within.
        /// </summary>
        /// <param name="x">The <see cref="X"/> coordinate along the <see cref="X"/> axis to check if contained within. Intaken as a <see cref="float"/>.</param>
        /// <param name="y">The <see cref="Y"/> coordinate along the <see cref="Y"/> axis to check if contained within. Intaken as a <see cref="float"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the coordinate is contained within.</returns>
        public bool Contains(float x, float y) => X <= x &&
                                                  x < X + Width &&
                                                  Y <= y &&
                                                  y < Y + Height;

        /// <summary>
        /// Determines whether the vector is entirely contained within.
        /// </summary>
        /// <param name="vector">The vector coordinate to check if contained within. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the vector is contained within.</returns>
        public bool Contains(Vector2 vector) => Contains(vector.X, vector.Y);

        /// <summary>
        /// Joins the two rectangles.
        /// </summary>
        /// <param name="left">The left <see cref="RectangleF"/>.</param>
        /// <param name="right">The right <see cref="RectangleF"/>.</param>
        /// <returns>Returns a new rectangle comprised of both rectangles.</returns>
        public static RectangleF Join(RectangleF left, RectangleF right)
        {
            var minX = Math.Min(left.X, right.X);
            var minY = Math.Min(left.Y, right.Y);
            var maxX = Math.Max(left.X + left.Width, right.X + right.Width);
            var maxY = Math.Max(left.Y + left.Height, right.Y + right.Height);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Joins the rectangles.
        /// </summary>
        /// <param name="rectangle">The <see cref="RectangleF"/> to join.</param>
        public void Join(RectangleF rectangle)
        {
            var minX = Math.Min(X, rectangle.X);
            var minY = Math.Min(Y, rectangle.Y);
            var maxX = Math.Max(X + Width, rectangle.X + rectangle.Width);
            var maxY = Math.Max(Y + Height, rectangle.Y + rectangle.Height);

            X = minX;
            Y = minY;
            Width = maxX - minX;
            Height = maxY - minY;
        }

        /// <summary>
        /// Increases the rectangle's size.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to increase the rectangle's <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Inflate(SizeF size)
        {
            Inflate(size.Width, size.Height);
        }

        /// <summary>
        /// Increases the rectangle's size.
        /// </summary>
        /// <param name="incrementX">The amount to increment the rectangle's <see cref="Width"/>. Intaken as a <see cref="float"/>.</param>
        /// <param name="incrementY">The amount to increment the rectangle's <see cref="Height"/>. Intaken as a <see cref="float"/>.</param>
        public void Inflate(float incrementX, float incrementY)
        {
            X -= incrementX;
            Y -= incrementY;
            Width += incrementX * 2;
            Height += incrementY * 2;
        }

        /// <summary>
        /// Decreases the rectangle's size.
        /// </summary>
        /// <param name="size">The <see cref="SizeF"/> to decrease the rectangle's <see cref="Width"/> and <see cref="Height"/> by.</param>
        public void Deflate(SizeF size)
        {
            Deflate(size.Width, size.Height);
        }

        /// <summary>
        /// Decreases the rectangle's size.
        /// </summary>
        /// <param name="incrementX">The amount to decrement the rectangle's <see cref="Width"/>. Intaken as a <see cref="float"/>.</param>
        /// <param name="incrementY">The amount to decrement the rectangle's <see cref="Height"/>. Intaken as a <see cref="float"/>.</param>
        public void Deflate(float incrementX, float incrementY)
        {
            X += incrementX;
            Y += incrementY;
            Width -= incrementX * 2;
            Height -= incrementY * 2;
        }

        /// <summary>
        /// Changes the <see cref="Position"/> of the rectangle
        /// </summary>
        /// <param name="offsetX">The x coordinate to add to the <see cref="RectangleF"/>. Intaken as a <see cref="float"/>.</param>
        /// <param name="offsetY">The y coordinate to add to the <see cref="RectangleF"/>. Intaken as a <see cref="float"/>.</param>
        public void Offset(float offsetX, float offsetY)
        {
            X += (int)offsetX;
            Y += (int)offsetY;
        }

        /// <summary>
        /// Changes the <see cref="Position"/> of the rectangle.
        /// </summary>
        /// <param name="amount">The x and y components to add to the <see cref="RectangleF"/>. Intaken as a <see cref="Vector2"/>.</param>
        public void Offset(Vector2 amount)
        {
            X += (int)amount.X;
            Y += (int)amount.Y;
        }

        /// <summary>
        /// Determines whether the <see cref="RectangleF"/>'s values are equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="RectangleF"/>.</param>
        /// <param name="right">The right hand <see cref="RectangleF"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="RectangleF"/>'s values are equal.</returns>
        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the <see cref="RectangleF"/>'s values are not equal.
        /// </summary>
        /// <param name="left">The left hand <see cref="RectangleF"/>.</param>
        /// <param name="right">The right hand <see cref="RectangleF"/>.</param>
        /// <returns>Returns a boolean indicating whether the <see cref="RectangleF"/>'s values are not equal.</returns>
        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Converts a <see cref="Rectangle"/> to a <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert. Intaken as a <see cref="Rectangle"/>.</param>
        public static implicit operator RectangleF(Rectangle rectangle) => new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

        /// <summary>
        /// Converts a <see cref="RectangleF"/> to a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert. Intaken as a <see cref="RectangleF"/>.</param>
        public static explicit operator Rectangle(RectangleF rectangle) => new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);

        /// <summary>
        /// <see cref="RectangleF"/>'s <see cref="object.ToString()"/> override.
        /// </summary>
        /// <returns>Returns a <see cref="string"/> containing the characteristics of the <see cref="RectangleF"/>.</returns>
        public override string ToString()
        {
            return $@"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}";
        }

        /// <summary>
        /// Compares the equality of the <see cref="RectangleF"/>'s.
        /// </summary>
        /// <param name="other">The other <see cref="RectangleF"/> to compare against.</param>
        /// <returns>Returns a boolean indication whether the <see cref="RectangleF"/>'s are equal.</returns>
        public bool Equals(RectangleF other)
        {
            return X.Equals(other.X) &&
                   Y.Equals(other.Y) &&
                   Width.Equals(other.Width) &&
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

            return obj is RectangleF other && Equals(other);
        }

        /// <summary>
        /// Returns a <see cref="ValueType.GetHashCode"/> for comparing equality.
        /// </summary>
        /// <returns>Returns the <see cref="RectangleF"/>'s <see cref="ValueType.GetHashCode"/> as an <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }
    }
}