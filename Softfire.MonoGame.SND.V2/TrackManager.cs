using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Softfire.MonoGame.SND
{
    /// <summary>
    /// A track manager for sound tracks.
    /// </summary>
    public class TrackManager
    {
        /// <summary>
        /// The manager's catalog of tracks.
        /// </summary>
        public Dictionary<string, Track> Catalog { get; }

        /// <summary>
        /// The track queue.
        /// </summary>
        private TrackQueue Queue { get; }

        /// <summary>
        /// The current volume level.
        /// </summary>
        public float CurrentVolumeLevel { get; private set; }

        /// <summary>
        /// The current track index.
        /// </summary>
        private int CurrentTrackIndex { get; set; }

        /// <summary>
        /// Is the track repeating?
        /// </summary>
        public bool IsRepeating
        {
            get => MediaPlayer.IsRepeating;
            set => MediaPlayer.IsRepeating = value;
        }

        /// <summary>
        /// Is the track muted?
        /// </summary>
        public bool IsMuted
        {
            get => MediaPlayer.IsMuted;
            set => MediaPlayer.IsMuted = value;
        }

        /// <summary>
        /// The track manager's constructor.
        /// </summary>
        public TrackManager()
        {
            Catalog = new Dictionary<string, Track>();
            Queue = new TrackQueue();

            IsRepeating = false;
            IsMuted = false;

            // Set Volume
            Volume(0.10f);
        }

        /// <summary>
        /// Gets a <see cref="Track"/> from the <see cref="Catalog"/>.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a <see cref="string"/>.</param>
        /// <returns>Returns the <see cref="Track"/> with the provided identifier, if found, otherwise null.</returns>
        public Track GetTrackFromCatalog(string identifier)
        {
            Track result = null;

            if (Catalog.ContainsKey(identifier))
            {
                result = Catalog[identifier];
            }

            return result;
        }

        /// <summary>
        /// Adds a <see cref="Track"/> to the <see cref="Queue"/>.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a <see cref="string"/>.</param>
        /// <returns>Returns the <see cref="Track"/>'s index in the <see cref="Queue"/>.</returns>
        public int AddToQueue(string identifier) => Queue.Add(Catalog[identifier]);

        /// <summary>
        /// Removes the <see cref="Track"/> from the <see cref="Queue"/> at the provided position.
        /// </summary>
        /// <param name="queuePosition"></param>
        public void RemoveFromQueue(int queuePosition) => Queue.Remove(queuePosition);

        /// <summary>
        /// Plays a <see cref="Track"/> directly from the <see cref="Catalog"/>.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a <see cref="string"/>.</param>
        /// <param name="startPosition">Intakes a <see cref="TimeSpan"/> indicating the start position for the song.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public void PlayDirectFromCatalog(string identifier, TimeSpan? startPosition = null)
        {
            Track song;

            if ((song = GetTrackFromCatalog(identifier)) != null)
            {
                MediaPlayer.Play(song.Data, startPosition ?? song.StartPosition);
            }
        }

        /// <summary>
        /// Plays the queue in order, by index.
        /// </summary>
        /// <returns>Returns a message of the results of the action.</returns>
        public void PlayQueue()
        {
            Track song;

            if ((song = Queue.GetTrack(CurrentTrackIndex)) != null)
            {
                // TODO: Setup playback of the entire queue.
                // MediaPlayer.Play(song.Data, song.StartPosition);
            }
        }

        /// <summary>
        /// Pauses the <see cref="Track"/>.
        /// </summary>
        public void Pause() => MediaPlayer.Pause();

        /// <summary>
        /// Resumes the <see cref="Track"/>.
        /// </summary>
        public void Resume() => MediaPlayer.Resume();

        /// <summary>
        /// Track Volume.
        /// Sets Master Volume. All track volumes are relative to this volume level.
        /// Default is 0.10f.
        /// </summary>
        /// <param name="adjustment">Intakes a positive or negative float to modify the volume.</param>
        /// <returns>Returns the current volume.</returns>
        public float Volume(float adjustment) => MediaPlayer.Volume = CurrentVolumeLevel = MathHelper.Clamp(CurrentVolumeLevel += adjustment, 0f, 1.0f);

        /// <summary>
        /// Formats the provided <see cref="TimeSpan"/> into the format of 0:00.
        /// </summary>
        /// <param name="timeSpan">Intakes a <see cref="TimeSpan"/>.</param>
        /// <returns>Returns a string in the format of 0:00.</returns>
        public string DisplayTime(TimeSpan timeSpan) => $"{timeSpan.Minutes}{(timeSpan.Seconds < 10 ? ":0" : ":")}{timeSpan.Seconds}";

        /// <summary>
        /// Retrieved the currently playing track info.
        /// </summary>
        /// <returns>The current track's data.</returns>
        public string GetCurrentTrackInfo()
        {
            var track = Queue.GetTrack(CurrentTrackIndex);

            return $"Track: '{track.Data.Name}' by '{track.Data.Artist}' is now playing!";
        }
    }
}
