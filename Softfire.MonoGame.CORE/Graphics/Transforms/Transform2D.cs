using System;
using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.CORE.Graphics.Transforms
{
    /// <summary>
    /// Flags for defining the current state of the internal matrices.
    /// </summary>
    [Flags]
    internal enum TransformFlags : byte
    {
        /// <summary>
        /// The flag used to trigger a build of the world matrix.
        /// </summary>
        BuildWorldMatrix = 1 << 0,
        /// <summary>
        /// The flag used to trigger a build of the local matrix.
        /// </summary>
        BuildLocalMatrix = 1 << 1,
        /// <summary>
        /// The flag used to trigger a build of all matrices.
        /// </summary>
        BuildAll = BuildLocalMatrix | BuildWorldMatrix
    }

    /// <summary>
    /// A 2D transformation class for positioning, scaling and rotating objects.
    /// </summary>
    public class Transform2D : IMonoGameTransformComponent
    {
        #region Fields

        /// <summary>
        /// The transform's internal parent reference.
        /// </summary>
        private Transform2D _parent;

        /// <summary>
        /// The transform's internal local matrix reference.
        /// </summary>
        private Matrix _localMatrix;

        /// <summary>
        /// The transform's internal world matrix reference.
        /// </summary>
        private Matrix _worldMatrix;

        /// <summary>
        /// The transforms internal position value.
        /// </summary>
        private Vector2 _position = Vector2.Zero;

        /// <summary>
        /// The transform's internal rotation value, in radians.
        /// </summary>
        private float _rotation;

        /// <summary>
        /// The transform's internal scale value.
        /// </summary>
        private Vector2 _scale = Vector2.One;

        #endregion

        #region Properties

        /// <summary>
        /// The transform's flags determine whether to build a local or world matrix or both.
        /// </summary>
        private TransformFlags Flags { get; set; }

        /// <summary>
        /// The build event. Subscribers are notified when a build request is made.
        /// </summary>
        public event EventHandler Build;

        /// <summary>
        /// The parent transform. Retrieves the current parent. Sets the current parent and updates all associated parents.
        /// </summary>
        public Transform2D Parent
        {
            get => _parent;
            set
            {
                // If the parent has changed, perform an update to all related transforms.
                if (_parent != value)
                {
                    var parent = Parent;
                    _parent = value;
                    
                    // Unsubscribe from old parent's build event.
                    // Loop through the upwards chain of parent transforms until there are none to update.
                    while (parent != null)
                    {
                        parent.Build -= OnParentUpdate;
                        parent = parent.Parent;
                    }

                    // Subscribe to the new parent's build event.
                    // Loop through the upwards chain of parent transforms until there are none to update.
                    parent = _parent;
                    while (parent != null)
                    {
                        parent.Build += OnParentUpdate;
                        parent = parent.Parent;
                    }
                }
            }
        }

        /// <summary>
        /// The transform's position. Retrieves the transform's position. Sets the transform's position and triggers builds of the local and world matrices.
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;

                    // Adds a flag to build the matrices.
                    Flags |= TransformFlags.BuildAll;

                    // Notify other transforms that a build is required.
                    Build?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The transform's scale. Retrieves the transform's scale. Sets the transform's scale and triggers builds of the local and world matrices.
        /// </summary>
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;

                // Adds a flag to build the matrices.
                Flags |= TransformFlags.BuildAll;

                // Notify other transforms that a build is required.
                Build?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The transform's rotation angle. Retrieves the transform's rotation angle. Sets the transform's rotation angle and triggers builds of the local and world matrices.
        /// </summary>
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;

                // Adds a flag to build the matrices.
                Flags |= TransformFlags.BuildAll;

                // Notify other transforms that a build is required.
                Build?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        /// <summary>
        /// Retrieves the world matrix's position.
        /// </summary>
        public Vector2 WorldPosition() => new Vector2(GetWorldMatrix().Position().X, GetWorldMatrix().Position().Y);

        /// <summary>
        /// Retrieves the world matrix's scale.
        /// </summary>
        public Vector2 WorldScale() => new Vector2(GetWorldMatrix().Scale().X, GetWorldMatrix().Scale().Y);

        /// <summary>
        /// Retrieves the world matrix's rotation.
        /// </summary>
        public float WorldRotation() => GetWorldMatrix().Rotation();

        /// <summary>
        /// A 2D transformation class with local and world matrices.
        /// </summary>
        public Transform2D() : this(Vector2.Zero, 0f, Vector2.One)
        {
        }

        /// <summary>
        /// A 2D transformation class with local and world matrices.
        /// </summary>
        /// <param name="x">The x coordinate of the transform. Intaken as a <see cref="float"/>.</param>
        /// <param name="y">The x coordinate of the transform. Intaken as a <see cref="float"/>.</param>
        /// <param name="rotation">The rotation angle of the transform. Intaken as a <see cref="float"/>.</param>
        /// <param name="scaleX">The scale width of the transform. Intaken as a <see cref="float"/>.</param>
        /// <param name="scaleY">The scale height of the transform. Intaken as a <see cref="float"/>.</param>
        public Transform2D(float x, float y, float rotation = 0, float scaleX = 1, float scaleY = 1) : this(new Vector2(x, y), rotation, new Vector2(scaleX, scaleY))
        {
        }

        /// <summary>
        /// A 2D transformation class with local and world matrices.
        /// </summary>
        /// <param name="position">The position of the transform. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="rotation">The rotation angle of the transform. Intaken as a <see cref="float"/>.</param>
        /// <param name="scale">The scale of the transform. Intaken as a <see cref="Vector2"/>.</param>
        public Transform2D(Vector2 position, float rotation, Vector2 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            Flags = TransformFlags.BuildAll;
        }

        /// <summary>
        /// Retrieves the local matrix and builds one if needed.
        /// </summary>
        public Matrix GetLocalMatrix()
        {
            BuildLocalMatrix();
            return _localMatrix;
        }

        /// <summary>
        /// Builds the local matrix, if a <see cref="TransformFlags.BuildLocalMatrix"/> has been set.
        /// </summary>
        private void BuildLocalMatrix()
        {
            // Check if the flag TransformFlags.BuildLocalMatrix is set.
            // If true, then build a local matrix.
            if ((Flags & TransformFlags.BuildLocalMatrix) == TransformFlags.BuildLocalMatrix)
            {
                // Build the local matrix.
                _localMatrix = Matrix.CreateScale(_scale.X, _scale.Y, 0) *
                               Matrix.CreateRotationX(_rotation) *
                               Matrix.CreateTranslation(_position.X, _position.Y, 0);

                // Remove the flag to build a local matrix as it was just done.
                Flags &= ~TransformFlags.BuildLocalMatrix;
                
                // Adds a flag to build the world matrix.
                Flags |= TransformFlags.BuildWorldMatrix;

                // Notify other transforms that a build is required.
                Build?.Invoke(this, EventArgs.Empty);
            }
        }
        
        /// <summary>
        /// Retrieves the world matrix and builds one if needed.
        /// </summary>
        public Matrix GetWorldMatrix()
        {
            BuildWorldMatrix();
            return _worldMatrix;
        }

        /// <summary>
        /// Builds the local matrix, if a <see cref="TransformFlags.BuildWorldMatrix"/> has been set.
        /// </summary>
        private void BuildWorldMatrix()
        {
            // Check if the flag TransformFlags.BuildWorldMatrix is set.
            // If true, then build a world matrix from the local matrix.
            if ((Flags & TransformFlags.BuildWorldMatrix) == TransformFlags.BuildWorldMatrix)
            {
                // Build a local matrix if the TransformFlags.BuildLocalMatrix is set.
                BuildLocalMatrix();

                // Build a world matrix using the local matrix.
                _worldMatrix = Parent != null ? Matrix.Multiply(_localMatrix, Parent.GetWorldMatrix()) : _localMatrix;

                // Remove the flag to build a world matrix as it was just done.
                Flags &= ~TransformFlags.BuildWorldMatrix;
            }
        }

        /// <summary>
        /// Flags the transform to update the local and world matrix due to a parent transform update.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The events arguments.</param>
        private void OnParentUpdate(object sender, EventArgs args)
        {
            // Adds a flag to build all matrices.
            Flags |= TransformFlags.BuildAll;
        }
    }
}