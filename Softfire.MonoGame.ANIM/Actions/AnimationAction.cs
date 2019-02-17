using Microsoft.Xna.Framework;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.ANIM.Actions
{
    /// <summary>
    /// Defines an animation's actions.
    /// </summary>
    public class AnimationAction : IMonoGameIdentifierComponent, IMonoGameActiveComponent, IMonoGameUpdateComponent
    {
        #region Fields

        /// <summary>
        /// The action's internal frame width value.
        /// </summary>
        private int _frameWidth;

        /// <summary>
        /// The action's internal frame height value.
        /// </summary>
        private int _frameHeight;

        /// <summary>
        /// The action's internal loop counter value.
        /// </summary>
        private int _loopCounter;

        /// <summary>
        /// The action's internal loop length value.
        /// </summary>
        private int _loopLength = (int)LoopLengths.Infinite;

        #endregion

        #region Properties

        /// <summary>
        /// The time between updates.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// The action's elapsed time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// The action's unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The action's unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Determines whether the action is active.
        /// </summary>
        public bool IsActive { get; internal set; }

        /// <summary>
        /// The action's source rectangle in the <see cref="Animation"/>'s sprite sheet.
        /// </summary>
        public Rectangle SourceRectangle => new Rectangle((int)SourcePosition.X + (CurrentFrameIndex * FrameWidth), (int)SourcePosition.Y, FrameWidth, FrameHeight);

        /// <summary>
        /// The action's source position (top-left) on the <see cref="Animation"/>'s sprite sheet. 
        /// </summary>
        public Vector2 SourcePosition { get; }
        
        /// <summary>
        /// The action's frame width.
        /// </summary>
        public int FrameWidth
        {
            get => _frameWidth;
            private set => _frameWidth = value > 0 ? value : 1;
        }

        /// <summary>
        /// The action's frame height.
        /// </summary>
        public int FrameHeight
        {
            get => _frameHeight;
            private set => _frameHeight = value > 0 ? value : 1;
        }
        
        /// <summary>
        /// The number of frames for this action.
        /// </summary>
        private int FrameCount { get; }

        /// <summary>
        /// The action's current frame index.
        /// </summary>
        private int CurrentFrameIndex { get; set; }

        /// <summary>
        /// The action's frame speed in seconds.
        /// </summary>
        public float FrameSpeedInSeconds { get; set; }

        /// <summary>
        /// The action's loop style.
        /// </summary>
        public LoopStyles LoopStyle { get; set; }
        
        /// <summary>
        /// Defines the number of loops the animation will make.
        /// </summary>
        public int LoopLength
        {
            get => _loopLength;
            set => _loopLength = value > 0 || value == -1 ? value : (int)LoopLengths.Infinite;
        }

        /// <summary>
        /// Maintains a loop counter of the number of loops to draw.
        /// </summary>
        public int LoopCounter
        {
            get => _loopCounter;
            set => _loopCounter = value >= 0 ? value : 0;
        }

        #endregion

        #region Enums

        /// <summary>
        /// The action's available loop styles.
        /// </summary>
        /// <remarks>
        /// <see cref="Forward"/> - Draws the animation's frames from left to right along the X axis.
        /// <see cref="Reverse"/> - Draws the animation's frames from right to left along the X axis.
        /// <see cref="Alternating"/> - Alternates between <see cref="Forward"/> and <see cref="Reverse"/>.
        /// </remarks>
        public enum LoopStyles
        {
            /// <summary>
            /// Draws the animation's frames from left to right along the X axis.
            /// </summary>
            Forward,
            /// <summary>
            /// Draws the animation's frames from right to left along the X axis.
            /// </summary>
            Reverse,
            /// <summary>
            /// Alternates between <see cref="Forward"/> and <see cref="Reverse"/>.
            /// </summary>
            Alternating
        }

        /// <summary>
        /// The action's available loop lengths.
        /// </summary>
        public enum LoopLengths
        {
            /// <summary>
            /// An infinite number of action loops.
            /// </summary>
            Infinite = -1,
            /// <summary>
            /// A single loop of the action.
            /// </summary>
            None
        }

        #endregion

        #region Booleans

        /// <summary>
        /// Determines whether the action loop is complete.
        /// </summary>
        private bool IsLoopComplete { get; set; }

        #endregion

        /// <summary>
        /// An action for the animation to perform. Defined by sourcing from the animation's sprite sheet.
        /// </summary>
        /// <param name="id">The action's unique id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The action's unique name. Intaken as a <see cref="string"/>.</param>
        /// <param name="sourcePosition">The source position on sprite sheet (top-left) to read the action from. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="frameWidth">The frame width in pixels of the action. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameHeight">The frame height in pixels of the action. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameCount">Number of frames in the action. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameSpeedInSeconds">Time in seconds between frames. Intaken as a <see cref="float"/>.</param>
        /// <param name="loopStyle">The loop style to use. Intaken as a <see cref="LoopStyle"/>.</param>
        /// <param name="loopLength">The amount of loops the action will perform. Intaken as an <see cref="int"/>.</param>
        public AnimationAction(int id, string name, Vector2 sourcePosition, int frameWidth, int frameHeight, int frameCount,
                               float frameSpeedInSeconds, LoopStyles loopStyle = LoopStyles.Forward, int loopLength = (int)LoopLengths.Infinite)
        {
            Id = id;
            Name = name;

            SourcePosition = sourcePosition;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameCount = frameCount;
            FrameSpeedInSeconds = frameSpeedInSeconds;

            LoopStyle = loopStyle;
            LoopLength = loopLength;
        }

        /// <summary>
        /// The action frame selection pattern.
        /// </summary>
        private void AnimationPattern()
        {
            // If time elapsed is greater than the delay between frames.
            if (ElapsedTime >= FrameSpeedInSeconds * 1000)
            {
                switch (LoopStyle)
                {
                    // An infinite forward loop.
                    case LoopStyles.Forward when LoopLength == (int)LoopLengths.Infinite:
                    {
                        if (CurrentFrameIndex < FrameCount - 1)
                        {
                            CurrentFrameIndex++;
                        }
                        else
                        {
                            CurrentFrameIndex = 0;
                        }

                        // Reset Loop Counter in case of switched Loop Lengths.
                        ResetLoopCounter();
                        break;
                    }

                    // A limited forward loop.
                    case LoopStyles.Forward:
                    {
                        if (LoopLength > (int)LoopLengths.None &&
                            LoopLength > LoopCounter)
                        {
                            if (CurrentFrameIndex < FrameCount - 1)
                            {
                                CurrentFrameIndex++;
                            }
                            else
                            {
                                CurrentFrameIndex = 0;
                                LoopCounter++;
                            }
                        }

                        break;
                    }

                    // An infinite reverse loop.
                    case LoopStyles.Reverse when LoopLength == (int)LoopLengths.Infinite:
                    {
                        if (CurrentFrameIndex > 0)
                        {
                            CurrentFrameIndex--;
                        }
                        else
                        {
                            CurrentFrameIndex = FrameCount - 1;
                        }

                        // Reset Loop Counter in case of switched Loop Lengths.
                        ResetLoopCounter();
                        break;
                    }

                    // A limited reverse loop.
                    case LoopStyles.Reverse:
                    {
                        if (LoopLength > (int)LoopLengths.None &&
                            LoopLength > LoopCounter)
                        {
                            if (CurrentFrameIndex > 0)
                            {
                                CurrentFrameIndex--;
                            }
                            else
                            {
                                CurrentFrameIndex = FrameCount - 1;
                                LoopCounter++;
                            }
                        }

                        break;
                    }

                    // An alternating infinite loop.
                    case LoopStyles.Alternating when LoopLength == (int)LoopLengths.Infinite:
                    {
                        if (!IsLoopComplete)
                        {
                            // Forward
                            if (CurrentFrameIndex < FrameCount - 1)
                            {
                                CurrentFrameIndex++;
                            }
                            else
                            {
                                CurrentFrameIndex--;
                                IsLoopComplete = true;
                            }
                        }
                        else
                        {
                            // Reverse
                            if (CurrentFrameIndex > 0)
                            {
                                CurrentFrameIndex--;
                            }
                            else
                            {
                                CurrentFrameIndex++;
                                IsLoopComplete = false;
                            }
                        }

                        // Reset Loop Counter in case of switched Loop Lengths.
                        ResetLoopCounter();
                        break;
                    }

                    // A limited alternating loop.
                    case LoopStyles.Alternating:
                    {
                        if (LoopLength > (int)LoopLengths.None &&
                            LoopLength > LoopCounter)
                        {
                            if (!IsLoopComplete)
                            {
                                // Forward
                                if (CurrentFrameIndex < FrameCount - 1)
                                {
                                    CurrentFrameIndex++;
                                }
                                else
                                {
                                    CurrentFrameIndex--;
                                    IsLoopComplete = true;
                                }
                            }
                            else
                            {
                                // Reverse
                                if (CurrentFrameIndex > 0)
                                {
                                    CurrentFrameIndex--;
                                }
                                else
                                {
                                    CurrentFrameIndex++;
                                    IsLoopComplete = false;
                                    LoopCounter++;
                                }
                            }
                        }

                        break;
                    }
                }

                // Reset elapsed time.
                ElapsedTime = 0;
            }
        }

        /// <summary>
        /// Reset the loop counter and to begin the action loop over.
        /// </summary>
        public void ResetLoopCounter()
        {
            LoopCounter = 0;
        }

        /// <summary>
        /// The action's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalMilliseconds;
                AnimationPattern();
            }
        }
    }
}