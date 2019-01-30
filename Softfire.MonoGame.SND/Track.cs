using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Softfire.MonoGame.SND
{
    /// <summary>
    /// A sound track.
    /// </summary>
    public class Track
    {
        /// <summary>
        /// The track's meta-data.
        /// </summary>
        public Song Data { get; set; }

        /// <summary>
        /// The starting time of the track.
        /// </summary>
        public TimeSpan StartPosition { get; set; }

        /// <summary>
        /// The track's local file path.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// The sound track's constructor.
        /// </summary>
        /// <param name="filePath">The local file path for the track. Intaken as a <see cref="string"/>.</param>
        public Track(string filePath)
        {
            FilePath = filePath;
            StartPosition = TimeSpan.Zero;
        }

        /// <summary>
        /// Loads the track into memory.
        /// </summary>
        /// <param name="content">The content manager used to store the sound track. Intaken as a <see cref="ContentManager"/></param>
        public void LoadContent(ContentManager content)
        {
            Data = content.Load<Song>(FilePath);
        }
    }
}