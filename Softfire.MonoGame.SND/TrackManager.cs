using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Softfire.MonoGame.SND
{
    public class TrackManager
    {
        /// <summary>
        /// Track Catalogue.
        /// </summary>
        public Dictionary<string, Track> Catalogue { get; }

        /// <summary>
        /// Track Queue.
        /// </summary>
        public TrackQueue Queue { get; }

        /// <summary>
        /// Current Volume Level.
        /// </summary>
        public float CurrentVolumeLevel { get; private set; }

        /// <summary>
        /// Is Media Player Repeating?
        /// </summary>
        public bool IsRepeating { get; set; }

        /// <summary>
        /// Is Media Player Muted?
        /// </summary>
        public bool IsMuted { get; set; }

        /// <summary>
        /// Track Manager Constructor.
        /// </summary>
        public TrackManager()
        {
            Catalogue = new Dictionary<string, Track>();
            Queue = new TrackQueue();

            IsRepeating = false;
            IsMuted = false;
            ApplySettings();

            // Set Volume
            Volume(0.10f);
        }

        /// <summary>
        /// Get Track From Catalogue.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns the Track with the supplied identifier or null if not found.</returns>
        public Track GetTrackFromCatalogue(string identifier)
        {
            Track result = null;

            if (Catalogue.ContainsKey(identifier))
            {
                result = Catalogue[identifier];
            }

            return result;
        }

        /// <summary>
        /// Add Track to Queue.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns the Track's index in the Queue.</returns>
        public int AddToQueue(string identifier)
        {
            return Queue.Add(Catalogue[identifier]);
        }

        /// <summary>
        /// Remove From Queue.
        /// </summary>
        /// <param name="queuePosition"></param>
        /// <returns></returns>
        public string RemoveFromQueue(int queuePosition)
        {
            Track track;
            var result = $"A track was not found at position {queuePosition} in the queue!";

            if ((track = Queue.GetTrackAt(queuePosition)) != null)
            {
                Queue.RemoveAt(queuePosition);
                result = $"The track '{track.Data.Name}' at position {queuePosition} was removed from the queue!";
            }

            return result;
        }

        /// <summary>
        /// Play From Catalogue.
        /// Plays a Track directly from the Catalogue.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="startPosition">Intakes a TimeSpan indicating the start position for the song. If null the Track's StartPosition will be used. Default is null.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string PlayDirectFromCatalogue(string identifier, TimeSpan? startPosition = null)
        {
            Track song;
            var result = $"Track: '{identifier}' was not found!";

            if ((song = GetTrackFromCatalogue(identifier)) != null)
            {
                MediaPlayer.Play(song.Data, startPosition ?? song.StartPosition);
                result = $"Track: '{identifier}' is now playing!";
            }

            return result;
        }

        /// <summary>
        /// Play Queue.
        /// </summary>
        /// <returns>Returns a message of the results of the action.</returns>
        public string PlayQueue()
        {
            Track song;
            var result = "No Tracks found in Queue!";

            if ((song = Queue.GetNextTrack()) != null)
            {
                MediaPlayer.Play(song.Data, song.StartPosition);
                result = $"Track: '{song.Data.Name}' by '{song.Data.Artist}' is now playing!";
            }

            return result;
        }

        /// <summary>
        /// Pause Track.
        /// </summary>
        /// <returns>Returns a message of the results of the action.</returns>
        public string Pause()
        {
            MediaPlayer.Pause();

            return $"Track: '{MediaPlayer.Queue.ActiveSong}' by '{MediaPlayer.Queue.ActiveSong.Artist}' has been paused!";
        }

        /// <summary>
        /// Resume Track.
        /// </summary>
        /// <returns>Returns a message of the results of the action.</returns>
        public string Resume()
        {
            MediaPlayer.Resume();

            return $"Track: '{MediaPlayer.Queue.ActiveSong}' by '{MediaPlayer.Queue.ActiveSong.Artist}' has resumed playing!";
        }

        /// <summary>
        /// Track Volume.
        /// Sets Master Volume. All track volumes are relative to this volume level.
        /// Default is 0.10f.
        /// </summary>
        /// <param name="adjustment">Intakes a positive or negative float to modify the volume.</param>
        /// <returns>Returns the current volume.</returns>
        public float Volume(float adjustment)
        {
            MediaPlayer.Volume = CurrentVolumeLevel = MathHelper.Clamp(CurrentVolumeLevel += adjustment, 0f, 1.0f);

            return CurrentVolumeLevel;
        }

        /// <summary>
        /// Display Time.
        /// Formats the provided TimeSpan into the format of 0:00.
        /// </summary>
        /// <param name="timeSpan">Intakes a TimeSpan.</param>
        /// <returns>Returns a string in the format of 0:00.</returns>
        public string DisplayTime(TimeSpan timeSpan)
        {
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;
            string result;

            if (seconds < 10)
                result = minutes + ":0" + seconds;
            else
                result = minutes + ":" + seconds;

            return result;
        }

        /// <summary>
        /// Apply Settings.
        /// Applies IsRepeating and IsMuted to MediaPlayer.
        /// </summary>
        public void ApplySettings()
        {
            MediaPlayer.IsRepeating = IsRepeating;
            MediaPlayer.IsMuted = IsMuted;
        }
    }
}
