using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// An interface for moving an object.
    /// </summary>
    public interface IMonoGameMovementComponent
    {
        /// <summary>
        /// The speed in a given direction.
        /// </summary>
        Vector2 Velocity { get; }

        /// <summary>
        /// The rate at which the object covers distance.
        /// </summary>
        double Acceleration { get; }

        /// <summary>
        /// The object's boundaries.
        /// </summary>
        RectangleF Bounds { get; }

        /// <summary>
        /// Sets the <see cref="Bounds"/> of the object to confine it's movements.
        /// </summary>
        /// <param name="bounds">The <see cref="RectangleF"/> to set as the <see cref="Bounds"/>.</param>
        void SetBounds(RectangleF bounds);

        /// <summary>
        /// Resets the <see cref="Bounds"/>.
        /// </summary>
        void ResetBounds();

        /// <summary>
        /// Increases the rate at which the object changes it's <see cref="Velocity"/>.
        /// </summary>
        /// <param name="increment">The amount to accelerate by. Intaken as a <see cref="double"/>.</param>
        void Accelerate(double increment);

        /// <summary>
        /// Increases the rate at which the object changes it's <see cref="Velocity"/>, up to the provided limit.
        /// </summary>
        /// <param name="increment">The amount to accelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="limit">The acceleration limit. Intaken as a <see cref="double"/>.</param>
        void Accelerate(double increment, double limit);

        /// <summary>
        /// Decreases the rate at which the object changes it's <see cref="Velocity"/>.
        /// </summary>
        /// <param name="decrement">The amount to decelerate by. Intaken as a <see cref="double"/>.</param>
        void Decelerate(double decrement);

        /// <summary>
        /// Decreases the rate at which the object changes its <see cref="Velocity"/>, up to it's limit.
        /// </summary>
        /// <param name="decrement">The amount to decelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="limit">The deceleration limit. Intaken as a <see cref="double"/>.</param>
        void Decelerate(double decrement, double limit);

        /// <summary>
        /// Calculates <see cref="Velocity"/> based on rotation angle of the object and <see cref="Acceleration"/>.
        /// </summary>
        void CalculateVelocity();

        /// <summary>
        /// Calculates <see cref="Velocity"/> based on provided rotation angle, in degrees, and <see cref="Acceleration"/>.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to use to calculate <see cref="Velocity"/>. Intaken as a <see cref="double"/>.</param>
        void CalculateVelocity(double rotationAngleInDegrees);

        /// <summary>
        /// Stabilizes <see cref="Acceleration"/> by returning <see cref="Acceleration"/> to a defined limit.
        /// </summary>
        /// <param name="increment">The amount to accelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="decrement">The amount to decelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="limit">The stabilization limit. Intaken as a <see cref="double"/>.</param>
        void Stabilize(double increment, double decrement, double limit);

        /// <summary>
        /// Applies <see cref="Velocity"/> to the <see cref="MonoGameObject"/>'s position.
        /// </summary>
        void ApplyVelocity();

        /// <summary>
        /// Moves the object by the provided deltas.
        /// </summary>
        /// <param name="deltas">Input deltas. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="useExtendedRectangle">Determines whether to use the object's extended or standard rectangle.</param>
        void Move(Vector2 deltas, bool useExtendedRectangle = true);

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the object towards the provided vector.
        /// </summary>
        /// <param name="targetVector">The target vector to rotate towards. Intaken as a <see cref="Vector2"/>.</param>
        void RotateTowards(Vector2 targetVector);

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the object away from the provided vector.
        /// </summary>
        /// <param name="targetVector">The target vector to rotate away from. Intaken as a <see cref="Vector2"/>.</param>
        void RotateAway(Vector2 targetVector);

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the object with the provided angle.
        /// </summary>
        /// <param name="rotationAngleInRadians">The rotation angle, in radians, to mirror. Intaken as a <see cref="float"/>.</param>
        void RotateWith(float rotationAngleInRadians);

        /// <summary>
        /// Calculates the vector to rotate around another object.
        /// </summary>
        /// <param name="pointOfRotation">The parent object's point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="rotationAngleInRadians">The parent object's rotation angle, in radians, to mirror. Intaken as a <see cref="double"/>.</param>
        /// <param name="offset">The object's positional offset from the point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        void RotateAround(Vector2 pointOfRotation, double rotationAngleInRadians, Vector2 offset = new Vector2());

        /// <summary>
        /// Rotates the object clockwise by the provided rotation angle, in degrees.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to rotate. Intaken as a <see cref="double"/>.</param>
        void RotateClockwise(double rotationAngleInDegrees);

        /// <summary>
        /// Rotates the object counter-clockwise by the provided rotation angle, in degrees.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to rotate. Intaken as a <see cref="double"/>.</param>
        void RotateCounterClockwise(double rotationAngleInDegrees);
    }
}