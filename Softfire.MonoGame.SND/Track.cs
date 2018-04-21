using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Softfire.MonoGame.SND
{
    public class Track
    {
        public Song Data { get; set; }

        public TimeSpan StartPosition { get; set; }

        public string FilePath { get; }

        public Track(string filePath)
        {
            FilePath = filePath;
            StartPosition = TimeSpan.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            Data = content.Load<Song>(FilePath);
        }
    }
}
