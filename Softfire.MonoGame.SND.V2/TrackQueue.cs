using System.Collections.Generic;

namespace Softfire.MonoGame.SND
{
    /// <summary>
    /// A track queue class for use with the <see cref="SoundManager"/>.
    /// </summary>
    public class TrackQueue
    {
        /// <summary>
        /// The queue containing sound tracks.
        /// </summary>
        private List<Track> Queue { get; }

        /// <summary>
        /// The <see cref="TrackQueue"/> constructor.
        /// </summary>
        public TrackQueue()
        {
            Queue = new List<Track>();
        }

        /// <summary>
        /// Adds a track to the end of the <see cref="Queue"/>.
        /// </summary>
        /// <param name="track">Intakes a Track.</param>
        /// <returns>Returns the queue position of the track.</returns>
        public int Add(Track track)
        {
            Queue.Add(track);

            return Queue.Count;
        }

        /// <summary>
        /// Adds a <see cref="Track"/> at the provided position in the queue.
        /// </summary>
        /// <param name="track">The track to add. Intaken a <see cref="Track"/>.</param>
        /// <param name="queuePosition">The position in the queue where the track will be inserted. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the queue position of the track as an <see cref="int"/>.</returns>
        public int Add(Track track, int queuePosition)
        {
            Queue.Insert(queuePosition, track);

            return queuePosition;
        }

        /// <summary>
        /// Gets the <see cref="Track"/> at the provided queue position.
        /// </summary>
        /// <param name="queuePosition">The position in the queue in which to retrieve the track. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="Track"/>, if found, or null.</returns>
        public Track GetTrack(int queuePosition)
        {
            Track track = null;

            if (queuePosition > 0 &&
                queuePosition < Queue.Count)
            {
                track = Queue[queuePosition - 1];
            }

            return track;
        }

        /// <summary>
        /// Removes a <see cref="Track"/> at the provided position in the queue.
        /// </summary>
        /// <param name="queuePosition">The position in the queue where the track will be removed. Intaken as an <see cref="int"/>.</param>
        public void Remove(int queuePosition)
        {
            if (queuePosition > 0 &&
                queuePosition <= Queue.Count)
            {
                Queue.RemoveAt(queuePosition - 1);
            }
        }

        /// <summary>
        /// Moves the track up one position in the queue.
        /// </summary>
        /// <param name="queuePosition">The position in the queue where the track that will be moved. Intaken as an <see cref="int"/>.</param>
        public void MoveUp(int queuePosition)
        {
            if (queuePosition > 0 &&
                queuePosition <= Queue.Count)
            {
                var track = GetTrack(queuePosition);
                Remove(queuePosition);
                Queue.Insert(queuePosition - 1, track);
            }
        }

        /// <summary>
        /// Moves the track down one position in the queue.
        /// </summary>
        /// <param name="queuePosition">The position in the queue where the track that will be moved. Intaken as an <see cref="int"/>.</param>
        public void MoveDown(int queuePosition)
        {
            if (queuePosition > 0 &&
                queuePosition <= Queue.Count)
            {
                var track = GetTrack(queuePosition);
                Remove(queuePosition);
                Queue.Insert(queuePosition + 1, track);
            }
        }

        /// <summary>
        /// Clears the <see cref="Track"/>s from the queue.
        /// </summary>
        public void ClearQueue()
        {
            Queue.Clear();
        }
    }
}