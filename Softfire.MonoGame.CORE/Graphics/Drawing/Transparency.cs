using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.CORE.Graphics.Drawing
{
    /// <summary>
    /// A transparency class for adding transparency to objects.
    /// </summary>
    public class Transparency : IMonoGameIdentifierComponent, IMonoGameDrawingTransparencyComponent
    {
        /// <summary>
        /// The internal transparency level value.
        /// </summary>
        private float _level = 1f;

        /// <summary>
        /// The transparency's unique identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The transparency's unique identifying name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The level of transparency.
        /// </summary>
        /// <remarks>Generally from 0 (invisible) to 1 (visible).</remarks>
        public float Level
        {
            get => _level;
            set => _level = value >= 0f && value <= 1f ? value : _level;
        }

        /// <summary>
        /// Provides transparency level for objects.
        /// </summary>
        /// <param name="id">A unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">A unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="level">The transparency level. Intaken as a <see cref="float"/>.</param>
        public Transparency(int id, string name, float level)
        {
            Id = id;
            Name = name;
            Level = level;
        }
    }
}