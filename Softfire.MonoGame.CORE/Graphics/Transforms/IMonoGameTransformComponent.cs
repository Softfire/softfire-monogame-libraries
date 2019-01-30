using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Graphics.Transforms
{
    /// <summary>
    /// An interface for the positioning, rotating and scaling of 2D objects.
    /// </summary>
    public interface IMonoGameTransformComponent
    {
        /// <summary>
        /// The parent <see cref="Transform2D"/>.
        /// </summary>
        /// <remarks><see cref="Position"/>, <see cref="Scale"/> and <see cref="Rotation"/> are all inheriting the <see cref="Parent"/>'s attributes and are using them in the <see cref="Matrix"/> transformations.</remarks>
        Transform2D Parent { get; set; }

        /// <summary>
        /// The transform's position.
        /// </summary>
        /// <remarks>Used in creating the <see cref="WorldPosition"/> transformation.</remarks>
        Vector2 Position { get; set; }

        /// <summary>
        /// The transform's scale.
        /// </summary>
        /// <remarks>Used in creating the <see cref="WorldScale"/> transformation.</remarks>
        Vector2 Scale { get; set; }

        /// <summary>
        /// The transform's rotation angle.
        /// </summary>
        /// <remarks>Used in creating the <see cref="WorldRotation"/> transformation.</remarks>
        float Rotation { get; set; }
        
        /// <summary>
        /// Retrieves the world matrix's position.
        /// </summary>
        Vector2 WorldPosition();

        /// <summary>
        /// Retrieves the world matrix's scale.
        /// </summary>
        Vector2 WorldScale();

        /// <summary>
        /// Retrieves the world matrix's rotation.
        /// </summary>
        float WorldRotation();

        /// <summary>
        /// Retrieves the local matrix.
        /// </summary>
        Matrix GetLocalMatrix();

        /// <summary>
        /// Retrieves the world matrix.
        /// </summary>
        Matrix GetWorldMatrix();
    }
}