using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Common
{
    /// <summary>
    /// A movement class for <see cref="MonoGameObject"/>'s.
    /// </summary>
    public class Movement : IMonoGameMovementComponent, IMonoGameUpdateComponent
    {
        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The parent <see cref="MonoGameObject"/> to apply movement.
        /// </summary>
        private MonoGameObject Parent { get; }

        /// <summary>
        /// The speed in a given direction.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// The rate at which the <see cref="MonoGameObject"/> covers distance.
        /// </summary>
        public double Acceleration { get; private set; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s boundaries. Set to <see cref="RectangleF.Empty"/> to remove any boundaries.
        /// </summary>
        public RectangleF Bounds { get; private set; } = RectangleF.Empty;

        /// <summary>
        /// A class for applying movement to a <see cref="MonoGameObject"/>.
        /// </summary>
        /// <param name="parent">The <see cref="MonoGameObject"/> to apply movement.</param>
        public Movement(MonoGameObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Sets the <see cref="Bounds"/> of the <see cref="MonoGameObject"/> to confine it's movements.
        /// </summary>
        /// <param name="bounds">The <see cref="RectangleF"/> to set as the <see cref="Bounds"/>.</param>
        public void SetBounds(RectangleF bounds)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Resets the <see cref="Bounds"/> to <see cref="RectangleF.Empty"/>.
        /// </summary>
        public void ResetBounds()
        {
            Bounds = RectangleF.Empty;
        }

        #region Acceleration & Velocity

        /// <summary>
        /// Increases the rate at which the <see cref="MonoGameObject"/> changes it's <see cref="Velocity"/>.
        /// </summary>
        /// <param name="increment">The amount to accelerate by. Intaken as a <see cref="double"/>.</param>
        public void Accelerate(double increment) => Acceleration += increment > 0 ? increment : 0;

        /// <summary>
        /// Increases the rate at which the <see cref="MonoGameObject"/> changes it's <see cref="Velocity"/>, up to the provided limit.
        /// </summary>
        /// <param name="increment">The amount to accelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="limit">The acceleration limit. Intaken as a <see cref="double"/>.</param>
        public void Accelerate(double increment, double limit) => Acceleration += increment > 0 && limit >= 0 && Acceleration < limit ? increment * DeltaTime : 0;

        /// <summary>
        /// Decreases the rate at which the <see cref="MonoGameObject"/> changes it's <see cref="Velocity"/>.
        /// </summary>
        /// <param name="decrement">The amount to decelerate by. Intaken as a <see cref="double"/>.</param>
        public void Decelerate(double decrement) => Acceleration -= decrement > 0 ? decrement : 0;

        /// <summary>
        /// Decreases the rate at which the <see cref="MonoGameObject"/> changes its <see cref="Velocity"/>, up to it's limit.
        /// </summary>
        /// <param name="decrement">The amount to decelerate by. Intaken as a <see cref="double"/>.</param>
        /// <param name="limit">The deceleration limit. Intaken as a <see cref="double"/>.</param>
        public void Decelerate(double decrement, double limit) => Acceleration -= decrement > 0 && limit >= 0 && Acceleration > -limit ? decrement * DeltaTime : 0;

        /// <summary>
        /// Calculates <see cref="Velocity"/> based on rotation angle of the object and <see cref="Acceleration"/>.
        /// </summary>
        public void CalculateVelocity() => CalculateVelocity(Parent.Transform.Rotation, Acceleration);

        /// <summary>
        /// Calculates <see cref="Velocity"/> based on provided rotation angle, in degrees, and <see cref="Acceleration"/>.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to use to calculate <see cref="Velocity"/>. Intaken as a <see cref="double"/>.</param>
        public void CalculateVelocity(double rotationAngleInDegrees) => Velocity = CalculateVelocity(MonoMath.ToRadians(rotationAngleInDegrees), Acceleration);

        /// <summary>
        /// Calculates velocity based on provided rotation angle, in radians, and provided acceleration.
        /// </summary>
        /// <param name="rotationAngleInRadians">The rotation angle, in radians, used to calculate velocity. Intaken as a <see cref="double"/>.</param>
        /// <param name="rateOfAcceleration">The rate of acceleration to use in calculating the velocity. Intaken as a <see cref="double"/>.</param>
        /// <returns>Returns the calculated velocity from the provided rotation angle and rate of acceleration as a <see cref="Vector2"/>.</returns>
        public static Vector2 CalculateVelocity(double rotationAngleInRadians, double rateOfAcceleration) => new Vector2((float)(Math.Sin(rotationAngleInRadians) * rateOfAcceleration),
                                                                                                                        -(float)(Math.Cos(rotationAngleInRadians) * rateOfAcceleration));
        #endregion

        #region Movement

        /// <summary>
        /// Moves the <see cref="MonoGameObject"/>, by the provided deltas, within the set <see cref="Bounds"/>.
        /// </summary>
        /// <param name="deltas">Input deltas. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="useExtendedRectangle">Determines whether to use the <see cref="MonoGameObject"/>'s extended or standard rectangle.</param>
        public void Move(Vector2 deltas, bool useExtendedRectangle = true)
        {
            var position = Parent.Transform.Position;
            var rectangle = useExtendedRectangle ? Parent.ExtendedRectangle : Parent.Rectangle;

            if (!Bounds.Equals(RectangleF.Empty))
            {
                // Top & Bottom
                if (rectangle.Top + deltas.Y <= Bounds.Y)
                {
                    position.Y -= rectangle.Top;
                }
                else if (rectangle.Top + deltas.Y > Bounds.Y &&
                         rectangle.Bottom + deltas.Y < Bounds.Height)
                {
                    position.Y += deltas.Y;
                }
                else if (rectangle.Bottom + deltas.Y >= Bounds.Height)
                {
                    position.Y += Bounds.Height - rectangle.Bottom;
                }

                // Right & Left
                if (rectangle.Left + deltas.X <= Bounds.X)
                {
                    position.X -= rectangle.Left;
                }
                else if (rectangle.Left + deltas.X > Bounds.X &&
                         rectangle.Right + deltas.X < Bounds.Width)
                {
                    position.X += deltas.X;
                }
                else if (rectangle.Right + deltas.X >= Bounds.Width)
                {
                    position.X += Bounds.Width - rectangle.Right;
                }
            }
            else
            {
                position += deltas;
            }

            Parent.Transform.Position = position;
        }

        /// <summary>
        /// Moves the <see cref="IMonoGame2DComponent"/>, by the provided deltas, within the set <see cref="Bounds"/>.
        /// </summary>
        /// <param name="objectToMove">The object of type <see cref="IMonoGame2DComponent"/> to apply movement.</param>
        /// <param name="deltas">Movement deltas. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="useExtendedRectangle">Determines whether to use the <see cref="IMonoGame2DComponent"/>'s extended or standard rectangle.</param>
        public static void Move(IMonoGame2DComponent objectToMove, Vector2 deltas, bool useExtendedRectangle = true) => objectToMove?.Movement.Move(deltas, useExtendedRectangle);

        #endregion

        #region Rotation

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the <see cref="MonoGameObject"/> towards the provided vector.
        /// </summary>
        /// <param name="targetVector">The target vector to rotate towards. Intaken as a <see cref="Vector2"/>.</param>
        public void RotateTowards(Vector2 targetVector) => Parent.Transform.Rotation = (float)RotateTowards(Parent.Transform.Position, targetVector);

        /// <summary>
        /// Calculates the rotation angle required, in radians, to rotate away from the provided target vector.
        /// </summary>
        /// <param name="currentVector">The current vector relative to the target vector. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="targetVector">The target vector to rotate away from. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a rotation angle, in radians, towards the target vector as a <see cref="double"/>.</returns>
        public static double RotateTowards(Vector2 currentVector, Vector2 targetVector) => -Math.Atan2(currentVector.X - targetVector.X, currentVector.Y - targetVector.Y);

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the <see cref="MonoGameObject"/> away from the provided vector.
        /// </summary>
        /// <param name="targetVector">The target vector to rotate away from. Intaken as a <see cref="Vector2"/>.</param>
        public void RotateAway(Vector2 targetVector) => Parent.Transform.Rotation = (float)RotateAway(Parent.Transform.Position, targetVector);

        /// <summary>
        /// Calculates the rotation angle required, in radians, to rotate away from the provided target vector.
        /// </summary>
        /// <param name="currentVector">The current vector relative to the target vector. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="targetVector">The target vector to rotate away from. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a rotation angle, in radians, away from the target vector as a <see cref="double"/>.</returns>
        public static double RotateAway(Vector2 currentVector, Vector2 targetVector) => -Math.Atan2(targetVector.X - currentVector.X, targetVector.Y - currentVector.Y);

        /// <summary>
        /// Calculates and applies the rotation angle required, in radians, to rotate the <see cref="MonoGameObject"/> with the provided angle.
        /// </summary>
        /// <param name="rotationAngleInRadians">The rotation angle, in radians, to mirror. Intaken as a <see cref="float"/>.</param>
        public void RotateWith(float rotationAngleInRadians) => Parent.Transform.Rotation = rotationAngleInRadians;

        /// <summary>
        /// Calculates the vector to rotate around another <see cref="MonoGameObject"/>.
        /// </summary>
        /// <param name="pointOfRotation">The parent <see cref="MonoGameObject"/>'s point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="rotationAngleInRadians">The parent <see cref="MonoGameObject"/>'s rotation angle, in radians, to mirror. Intaken as a <see cref="double"/>.</param>
        /// <param name="offset">The <see cref="MonoGameObject"/>'s positional offset from the point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        public void RotateAround(Vector2 pointOfRotation, double rotationAngleInRadians, Vector2 offset = new Vector2()) =>
            Parent.Transform.Position = RotateAround(Parent.Transform.Position, pointOfRotation, rotationAngleInRadians, offset);

        /// <summary>
        /// Calculates the vector to rotate around another object.
        /// </summary>
        /// <param name="currentVector">The child object's current vector.</param>
        /// <param name="pointOfRotation">The parent object's point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="rotationAngleInRadians">The parent object's rotation angle, in radians, to mirror. Intaken as a <see cref="double"/>.</param>
        /// <param name="offset">The child object's positional offset from the point of rotation. Intaken as a <see cref="Vector2"/>.</param>
        public static Vector2 RotateAround(Vector2 currentVector, Vector2 pointOfRotation, double rotationAngleInRadians, Vector2 offset = new Vector2()) => new Vector2
        {
            X = (float)(Math.Cos(rotationAngleInRadians) * (currentVector.X + offset.X - pointOfRotation.X) -
                        Math.Sin(rotationAngleInRadians) * (currentVector.Y + offset.Y - pointOfRotation.Y) + pointOfRotation.X),
            Y = (float)(Math.Sin(rotationAngleInRadians) * (currentVector.X + offset.X - pointOfRotation.X) +
                        Math.Cos(rotationAngleInRadians) * (currentVector.Y + offset.Y - pointOfRotation.Y) + pointOfRotation.Y)
        };

        /// <summary>
        /// Rotates the <see cref="MonoGameObject"/> clockwise by the provided rotation angle, in degrees.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to rotate. Intaken as a <see cref="double"/>.</param>
        public void RotateClockwise(double rotationAngleInDegrees) => Parent.Transform.Rotation += (float)MonoMath.ToRadians(rotationAngleInDegrees);

        /// <summary>
        /// Rotates the <see cref="MonoGameObject"/> counter-clockwise by the provided rotation angle, in degrees.
        /// </summary>
        /// <param name="rotationAngleInDegrees">The rotation angle, in degrees, to rotate. Intaken as a <see cref="double"/>.</param>
        public void RotateCounterClockwise(double rotationAngleInDegrees) => Parent.Transform.Rotation -= (float)MonoMath.ToRadians(rotationAngleInDegrees);

        #endregion

        /// <summary>
        /// The <see cref="Movement"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}