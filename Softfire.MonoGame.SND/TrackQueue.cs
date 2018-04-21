using System.Collections.Generic;

namespace Softfire.MonoGame.SND
{
    public class TrackQueue
    {
        /// <summary>
        /// Track Queue.
        /// </summary>
        public List<Track> Queue { get; }

        /// <summary>
        /// Tack Queue Constructor.
        /// </summary>
        public TrackQueue()
        {
            Queue = new List<Track>();
        }

        /// <summary>
        /// Add Track.
        /// </summary>
        /// <param name="track">Intakes a Track.</param>
        /// <returns>Returns the queue postion of the track.</returns>
        public int Add(Track track)
        {
            Queue.Add(track);

            return Queue.Count;
        }

        /// <summary>
        /// Add Track At.
        /// </summary>
        /// <param name="track">Intakes a Track.</param>
        /// <param name="queuePosition">Intakes the queue position in which the Track will be inserted as an int.</param>
        /// <returns>Returns the queue postion of the track.</returns>
        public int AddTrackAt(Track track, int queuePosition)
        {
            Queue.Insert(queuePosition, track);

            return queuePosition;
        }

        /// <summary>
        /// Remove At.
        /// </summary>
        /// <param name="queuePosition">Intakes the queue position in which the Track will be removed as an int.</param>
        public void RemoveAt(int queuePosition)
        {
            if (queuePosition <= Queue.Count)
            {
                Queue.RemoveAt(queuePosition);
            }
        }

        /// <summary>
        /// Clear Queue.
        /// </summary>
        public void ClearQueue()
        {
            Queue.Clear();
        }

        /// <summary>
        /// Get Track At.
        /// </summary>
        /// <param name="queuePosition">Intakes the queue position in which the Track will be retrieved as an int.</param>
        /// <returns>Returns a Track, if found, or null.</returns>
        public Track GetTrackAt(int queuePosition)
        {
            Track track = null;

            if (queuePosition <= Queue.Count)
            {
                track = Queue[queuePosition];
            }

            return track;
        }

        /// <summary>
        /// Get Next Track In Queue.
        /// </summary>
        /// <returns>Returns a Track from position 2 in the Queue, if found, or null.</returns>
        public Track GetNextTrackInQueue()
        {
            Track track = null;

            if (Queue.Count > 1)
            {
                track = GetTrackAt(1);
            }

            return track;
        }

        /// <summary>
        /// Get Next Track.
        /// </summary>
        /// <returns>Returns a Track from position 1 in the Queue, if found, or null.</returns>
        public Track GetNextTrack()
        {
            Track track = null;

            if (Queue.Count > 0)
            {
                track = GetTrackAt(0);
            }

            return track;
        }

        /// <summary>
        /// Move Up.
        /// </summary>
        /// <param name="queuePosition">Intakes the queue position in which the Track will be moved up from as an int.</param>
        public void MoveUp(int queuePosition)
        {
            if (queuePosition > 1)
            {
                var trackOne = GetTrackAt(queuePosition);
                var trackTwo = GetTrackAt(queuePosition - 1);

                RemoveAt(queuePosition);
                RemoveAt(queuePosition - 1);

                Queue.Insert(queuePosition - 1, trackOne);
                Queue.Insert(queuePosition, trackTwo);
            }
        }

        /// <summary>
        /// Move Down.
        /// </summary>
        /// <param name="queuePosition">Intakes the queue position in which the Track will be moved down from as an int.</param>
        public void MoveDown(int queuePosition)
        {
            if (queuePosition > 1)
            {
                var trackOne = GetTrackAt(queuePosition - 1);
                var trackTwo = GetTrackAt(queuePosition);

                RemoveAt(queuePosition - 1);
                RemoveAt(queuePosition);

                Queue.Insert(queuePosition, trackOne);
                Queue.Insert(queuePosition - 1, trackTwo);
            }
        }
    }
}