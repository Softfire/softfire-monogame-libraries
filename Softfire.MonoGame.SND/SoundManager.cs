using Microsoft.Xna.Framework.Content;

namespace Softfire.MonoGame.SND
{
    public class SoundManager
    {
        /// <summary>
        /// Primary Sound Content Manager.
        /// </summary>
        private ContentManager SoundContent { get; }

        /// <summary>
        /// Tracks Manager.
        /// </summary>
        public TrackManager Tracks { get; }

        /// <summary>
        /// Effects Manager.
        /// </summary>
        public EffectManager Effects { get; }

        /// <summary>
        /// Sound Manager Constructor.
        /// Provides access to the Track and Effects Managers.
        /// </summary>
        /// <param name="parentContentManager">Intakes the parent's Content Manager.</param>
        public SoundManager(ContentManager parentContentManager)
        {
            SoundContent = new ContentManager(parentContentManager.ServiceProvider, "Content");

            Tracks = new TrackManager();
            Effects = new EffectManager();
        }

        /// <summary>
        /// Add Track.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier that can be used to find the track as string.</param>
        /// <param name="filePath">Intakes the file path used to load the track as a string.</param>
        public void AddTrack(string identifier, string filePath)
        {
            var track = new Track(filePath);
            track.LoadContent(SoundContent);
            Tracks.Catalogue.Add(identifier, track);
        }

        /// <summary>
        /// Add Effect.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier that can be used to find the effect as string.</param>
        /// <param name="filePath">Intakes the file path used to load the effect as a string.</param>
        public void AddEffect(string identifier, string filePath)
        {
            var effect = new Effect(identifier, filePath);
            effect.LoadContent(SoundContent);
            Effects.Catalogue.Add(identifier, effect);
        }
    }
}