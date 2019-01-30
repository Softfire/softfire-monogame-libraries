using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Graphics
{
    /// <summary>
    /// A class of static methods for <see cref="Matrix"/>s.
    /// </summary>
    public static class Matrices
    {
        /// <summary>
        /// Retrieves the position of the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix containing the position to retrieve.</param>
        /// <returns>Returns the position of the matrix as a <see cref="Vector2"/>.</returns>
        public static Vector2 Position(this Matrix matrix)
        {
            DecomposeMatrix(ref matrix, out var position, out _, out _);
            return new Vector2(position.X, position.Y);
        }

        /// <summary>
        /// Retrieves the rotation of the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix containing the rotation to retrieve.</param>
        /// <returns>Returns the rotation of the matrix as a <see cref="float"/>.</returns>
        public static float Rotation(this Matrix matrix)
        {
            DecomposeMatrix(ref matrix, out _, out var rotation, out _);
            return rotation;
        }

        /// <summary>
        /// Retrieves the scale of the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix containing the scale to retrieve.</param>
        /// <returns>Returns the scale of the matrix as a <see cref="Vector2"/>.</returns>
        public static Vector2 Scale(this Matrix matrix)
        {
            DecomposeMatrix(ref matrix, out _, out _, out var scale);
            return new Vector2(scale.X, scale.Y);
        }

        /// <summary>
        /// A method to decompose the referenced matrix.
        /// </summary>
        /// <param name="matrix">The matrix to decompose. Intaken as a <see cref="Matrix"/>.</param>
        /// <param name="position">The matrix's decomposed position. Output as a <see cref="Vector2"/>.</param>
        /// <param name="rotation">The matrix's decomposed rotation. Output as a <see cref="float"/>.</param>
        /// <param name="scale">The matrix's decomposed scale. Output as a <see cref="Vector2"/>.</param>
        public static void DecomposeMatrix(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            matrix.Decompose(out var scale3, out var rotationQ, out var position3);

            var direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

        /// <summary>
        /// A method to recompose a <see cref="Matrix"/>.
        /// Order is Scale * Rotation * Position.
        /// </summary>
        /// <param name="position">The position to be recomposed into a <see cref="Matrix"/>. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="rotation">The rotation to be recomposed into a <see cref="Matrix"/> Intaken as a <see cref="float"/>.</param>
        /// <param name="scale">The scale to be recomposed into a <see cref="Matrix"/>. Intaken as a <see cref="Vector2"/>.</param>
        /// <returns>Returns a new <see cref="Matrix"/> composed of the provided variables. Order is Scale * Rotation * Position.</returns>
        public static Matrix RecomposeMatrix(Vector2 position, float rotation, Vector2 scale) => Matrix.CreateScale(new Vector3(scale, 1.0f)) *
                                                                                                 Matrix.CreateRotationZ(rotation) *
                                                                                                 Matrix.CreateTranslation(new Vector3(position, 0));

        /// <summary>
        /// Translates the world position to the screen position.
        /// </summary>
        /// <param name="worldPosition">The world position to translate to screen position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="viewMatrix">The view matrix to get the screen position from. Intaken as a <see cref="Matrix"/>.</param>
        /// <returns>Returns the location in the world that corresponds with the screen position given as a <see cref="Vector2"/>.</returns>
        public static Vector2 GetScreenPosition(Vector2 worldPosition, Matrix viewMatrix) => Vector2.Transform(worldPosition, viewMatrix);

        /// <summary>
        /// Translates the screen position to the world position.
        /// </summary>
        /// <param name="screenPosition">The screen position to translate to world position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="viewMatrix">The view matrix to invert to get the world position from. Intaken as a <see cref="Matrix"/>.</param>
        /// <returns>Returns the location on the world that corresponds with the screen position given as a <see cref="Vector2"/>.</returns>
        public static Vector2 GetWorldPosition(Vector2 screenPosition, Matrix viewMatrix) => Vector2.Transform(screenPosition, Matrix.Invert(viewMatrix));
    }
}