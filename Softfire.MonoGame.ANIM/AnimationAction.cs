using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.ANIM
{
    public class AnimationAction
    {
        /// <summary>
        /// Delta Time.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Action's Elapsed Time.
        /// Elapsed game time since last frame.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// Parent Animation.
        /// </summary>
        private Animation ParentAnimation { get; }
        
        /// <summary>
        /// Number of Action frames.
        /// </summary>
        private int NumberOfFrames { get; }

        /// <summary>
        /// Current Frame Index.
        /// </summary>
        private int CurrentFrameIndex { get; set; }

        /// <summary>
        /// Action's Frame Width.
        /// Frame width is automatically determined by the Texture's width divided by FramesX.
        /// </summary>
        public int FrameWidth { get; }

        /// <summary>
        /// Action's Frame Height.
        /// </summary>
        public int FrameHeight { get; }

        /// <summary>
        /// Action's Frame Speed In Seconds.
        /// </summary>
        public float FrameSpeedInSeconds { get; set; }

        /// <summary>
        /// Meta Position.
        /// The stating coodinate for reading the Texture for this action. The top-left most pixel. Zero based. Intaken as a Vector2.
        /// </summary>
        private Vector2 MetaPosition { get; }

        /// <summary>
        /// Meta Width.
        /// Total width of animation.
        /// Equal to Frame Width multiplied by Number of Frames.
        /// </summary>
        private int MetaWidth { get; }

        /// <summary>
        /// Meta Height.
        /// Total Height of animation.
        /// Equal to Frame Width multiplied by Number of Frames.
        /// </summary>
        private int MetaHeight { get; }

        /// <summary>
        /// Meta Rectangle.
        /// </summary>
        private Rectangle MetaRectangle { get; set; }

        /// <summary>
        /// Action's Source Rectangle.
        /// The Source Rectangle is used to select certain areas in a texture based on the total width and height set for the action.
        /// </summary>
        public Rectangle SourceRectangle { get; private set; }

        /// <summary>
        /// Action's Loop Style.
        /// </summary>
        public LoopStyles LoopStyle { get; set; }

        /// <summary>
        /// Action's Loop Styles.
        /// Forward - Draws the animation's frames from left to right along the X axis.
        /// Reverse - Draws the animation's frames from right to left along the X axis.
        /// Alternating - Alternates between Forward and Reverse.
        /// </summary>
        public enum LoopStyles
        {
            Forward,
            Reverse,
            Alternating
        }

        /// <summary>
        /// Action's Internal Loop Length Value.
        /// </summary>
        private int _loopLength;

        /// <summary>
        /// Action's Loop Length.
        /// Defines the number of loops the animation will make. Default is -1. (Infinite)
        /// </summary>
        public int LoopLength
        {
            get => _loopLength;
            set
            {
                if (value >= -1)
                {
                    _loopLength = value;
                }
            }
        }

        /// <summary>
        /// Action Loop Lengths.
        /// </summary>
        public enum LoopLengths
        {
            Infinite = -1,
            None
        }

        /// <summary>
        /// Action's Internal Loop Counter.
        /// </summary>
        private int _loopCounter;

        /// <summary>
        /// Action's Loop Counter.
        /// Maintains a loop counter to control the number of loops to draw.
        /// </summary>
        private int LoopCounter
        {
            get => _loopCounter;
            set
            {
                if (value >= -1)
                {
                    _loopCounter = value;
                }
            }
        }

        /// <summary>
        /// Looped is used to trigger the Alternating LoopStyle.
        /// </summary>
        private bool IsLoopComplete { get; set; }

        /// <summary>
        /// Animation Action.
        /// </summary>
        /// <param name="parentAnimation">The parent animation.</param>
        /// <param name="metaPosition">Start position on spritesheet to read action from. Top-left most pixel coordinate. Intaken as a Vector2.</param>
        /// <param name="metaWidth">Total width in pixels of action. Intaken as an int.</param>
        /// <param name="metaHeight">Total height in pixels of action. Intaken as an int.</param>
        /// <param name="numberOfFrames">Number of frames in the action.</param>
        /// <param name="frameSpeedInSeconds">Time in seconds between frames. Intaken as a float.</param>
        /// <param name="loopStyle">The loop style to use.</param>
        /// <param name="loopLength">The amount of loops the action will perform. intaken as an int.</param>
        public AnimationAction(Animation parentAnimation, Vector2 metaPosition, int metaWidth, int metaHeight, int numberOfFrames, float frameSpeedInSeconds, LoopStyles loopStyle = LoopStyles.Forward, int loopLength = (int)LoopLengths.Infinite)
        {
            ParentAnimation = parentAnimation;
            MetaPosition = metaPosition;
            NumberOfFrames = numberOfFrames;
            FrameSpeedInSeconds = frameSpeedInSeconds;
            MetaWidth = metaWidth;
            MetaHeight = metaHeight;
            LoopStyle = loopStyle;
            LoopLength = loopLength;

            FrameWidth = MetaWidth / NumberOfFrames;
            FrameHeight = MetaHeight / NumberOfFrames;
        }

        /// <summary>
        /// Animation Pattern.
        /// </summary>
        private void AnimationPattern()
        {
            // If time elapsed is greater than the delay between frames.
            if (ElapsedTime >= FrameSpeedInSeconds)
            {
                if (LoopStyle == LoopStyles.Forward)
                {
                    // Infinite
                    if (LoopLength == (int)LoopLengths.Infinite)
                    {
                        if (CurrentFrameIndex < NumberOfFrames - 1)
                        {
                            CurrentFrameIndex++;
                        }
                        else
                        {
                            CurrentFrameIndex = 0;
                        }

                        // Reset Loop Counter in case of switched Loop Lengths.
                        ResetLoopCounter();
                    }

                    // Limited
                    else if (LoopLength > (int)LoopLengths.None &&
                             LoopLength > LoopCounter)
                    {
                        if (CurrentFrameIndex < NumberOfFrames - 1)
                        {
                            CurrentFrameIndex++;
                        }
                        else
                        {
                            CurrentFrameIndex = 0;
                            LoopCounter++;
                        }
                    }
                }
                else if (LoopStyle == LoopStyles.Reverse)
                {
                    // Infinite
                    if (LoopLength == (int)LoopLengths.Infinite)
                    {
                        if (CurrentFrameIndex > 0)
                        {
                            CurrentFrameIndex--;
                        }
                        else
                        {
                            CurrentFrameIndex = NumberOfFrames - 1;
                        }

                        // Reset Loop Counter in case of switched Loop Lengths.
                        ResetLoopCounter();
                    }

                    // Limited
                    else if (LoopLength > (int)LoopLengths.None &&
                             LoopLength > LoopCounter)
                    {
                        if (CurrentFrameIndex > 0)
                        {
                            CurrentFrameIndex--;
                        }
                        else
                        {
                            CurrentFrameIndex = NumberOfFrames - 1;
                            LoopCounter++;
                        }
                    }
                }
                else if (LoopStyle == LoopStyles.Alternating)
                {
                    if (LoopLength == (int)LoopLengths.Infinite)
                    {
                        if (IsLoopComplete == false)
                        {
                            // Forward
                            if (CurrentFrameIndex < NumberOfFrames - 1)
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
                    }
                    else if (LoopLength > (int)LoopLengths.None &&
                             LoopLength > LoopCounter)
                    {
                        if (IsLoopComplete == false)
                        {
                            // Forward
                            if (CurrentFrameIndex < NumberOfFrames - 1)
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
                }

                // Reset elapsed timer
                ElapsedTime = 0;
            }
        }

        /// <summary>
        /// Action Loop Counter Reset Method.
        /// Call to reset the loop counter and begin the action loop once more.
        /// </summary>
        public void ResetLoopCounter()
        {
            LoopCounter = 0;
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's GameTime.</param>
        public void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            MetaRectangle = new Rectangle((int)MetaPosition.X, (int)MetaPosition.Y, MetaWidth, MetaHeight);
            SourceRectangle = new Rectangle(MetaRectangle.X + (CurrentFrameIndex * FrameWidth), MetaRectangle.Y, FrameWidth, FrameHeight);

            AnimationPattern();
        }
    }
}