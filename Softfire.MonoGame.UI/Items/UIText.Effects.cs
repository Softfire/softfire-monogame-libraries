using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Items
{
    partial class UIText
    {
        #region Outline Properties

        /// <summary>
        /// Outline Depth Internal Value.
        /// </summary>
        private int _outlineDepth;

        /// <summary>
        /// Outline Depth.
        /// </summary>
        public int OutlineDepth
        {
            get => _outlineDepth;
            set
            {
                if (value > 3)
                {
                    _outlineDepth = 3;
                }
                else if (value < 0)
                {
                    _outlineDepth = 0;
                }
                else
                {
                    _outlineDepth = value;
                }
            }
        }

        /// <summary>
        /// Draw Top Left Outline.
        /// </summary>
        private bool DrawTopLeftOutline { get; set; }

        /// <summary>
        /// Draw Top Outline.
        /// </summary>
        private bool DrawTopOutline { get; set; }

        /// <summary>
        /// Draw Top Right Outline.
        /// </summary>
        private bool DrawTopRightOutline { get; set; }

        /// <summary>
        /// Draw Right Outline.
        /// </summary>
        private bool DrawRightOutline { get; set; }

        /// <summary>
        /// Draw Bottom Right Outline.
        /// </summary>
        private bool DrawBottomRightOutline { get; set; }

        /// <summary>
        /// Draw Bottom Outline.
        /// </summary>
        private bool DrawBottomOutline { get; set; }

        /// <summary>
        /// Draw Bottom Left Outline.
        /// </summary>
        private bool DrawBottomLeftOutline { get; set; }

        /// <summary>
        /// Draw Left Outline.
        /// </summary>
        private bool DrawLeftOutline { get; set; }

        #endregion

        #region Outline Methods

        /// <summary>
        /// Activate Outlines.
        /// Call to enable/disable outlines and depth.
        /// </summary>
        /// <param name="outlineDepth">The outline draw depth. Intaken as an int between 0 and 3. Default is 1.</param>
        /// <param name="drawTopLeftOutline">Draws the top left outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawTopOutline">Draws the top outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawTopRightOutline">Draws the top right outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawRightOutline">Draws the right outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawBottomRightOutline">Draws the bottom right outline. Intaken as a bool. Default is true.</param>
        /// <param name="drawBottomOutline">Draws the bottom outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawBottomLeftOutline">Draws the bottom left outline. Intaken as a bool. Default is false.</param>
        /// <param name="drawLeftOutline">Draws the left outline. Intaken as a bool. Default is false.</param>
        public void ActivateOutlines(int outlineDepth = 1,
                                     bool drawTopLeftOutline = false,
                                     bool drawTopOutline = false,
                                     bool drawTopRightOutline = false,
                                     bool drawRightOutline = false,
                                     bool drawBottomRightOutline = true,
                                     bool drawBottomOutline = false,
                                     bool drawBottomLeftOutline = false,
                                     bool drawLeftOutline = false)
        {
            OutlineDepth = outlineDepth;
            DrawTopLeftOutline = drawTopLeftOutline;
            DrawTopOutline = drawTopOutline;
            DrawTopRightOutline = drawTopRightOutline;
            DrawRightOutline = drawRightOutline;
            DrawBottomRightOutline = drawBottomRightOutline;
            DrawBottomOutline = drawBottomOutline;
            DrawBottomLeftOutline = drawBottomLeftOutline;
            DrawLeftOutline = drawLeftOutline;
        }

        /// <summary>
        /// Apply Enhanced Text Outline.
        /// </summary>
        /// <param name="spriteBatch">Intakes the current SpriteBath.</param>
        /// <param name="text">Text to outline. Intaken as a string.</param>
        private void ApplyEnhancedTextOutline(SpriteBatch spriteBatch, string text)
        {
            if (IsVisible &&
                OutlineDepth > 0)
            {
                for (var i = OutlineDepth; i > 0; i--)
                {
                    if (DrawTopLeftOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(-i * Scale.X, -i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawTopOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(0, -i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawTopRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(i * Scale.X, -i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(i * Scale.X, 0),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(i * Scale.X, i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(0, i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomLeftOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(-i * Scale.X, i * Scale.Y),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawLeftOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position + new Vector2(-i * Scale.X, 0),
                            Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }
                }
            }
        }

        #endregion

        #region Shadow Depth Properties

        /// <summary>
        /// Shadow Depth Internal Value.
        /// </summary>
        private int _shadownDepth;

        /// <summary>
        /// Shadow Depth.
        /// </summary>
        public int ShadowDepth
        {
            get => _shadownDepth;
            set
            {
                if (value > 5)
                {
                    _shadownDepth = 5;
                }
                else if (value < 0)
                {
                    _shadownDepth = 0;
                }
                else
                {
                    _shadownDepth = value;
                }
            }
        }

        #endregion

        #region Shadow Depth Methods

        /// <summary>
        /// Apply Enhanced Text Shadow.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBath.</param>
        /// <param name="text">Text to add shadow to. Intaken as a string.</param>
        private void ApplyEnhancedTextShadow(SpriteBatch spriteBatch, string text)
        {
            if (IsVisible &&
                ShadowDepth > 0)
            {
                for (var i = ShadowDepth; i > 0; i--)
                {
                    spriteBatch.DrawString(Font, text, Position + new Vector2(i * 1.3f, -i * 0.7f),
                                           Color.Black * Transparencies["Background"], RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                }
            }
        }

        #endregion

        #region Writing & Replay Properties

        /// <summary>
        /// Write Speed.
        /// </summary>
        public WriteSpeeds WriteSpeed { get; set; } = WriteSpeeds.Immediate;

        /// <summary>
        /// Write Speeds.
        /// </summary>
        public enum WriteSpeeds
        {
            Immediate,
            Delayed
        }

        /// <summary>
        /// Write Speed Delay In Seconds.
        /// </summary>
        public float WriteSpeedDelayInSeconds { get; set; } = 0.10f;

        /// <summary>
        /// Write Index.
        /// </summary>
        private int WriteIndex { get; set; }

        /// <summary>
        /// Is Writing Complete?
        /// </summary>
        private bool IsWritingComplete { get; set; }

        /// <summary>
        /// Is Replaying?
        /// </summary>
        private bool IsReplaying { get; set; }

        /// <summary>
        /// Replay Start Internal Index.
        /// </summary>
        private int _replayStartIndex;

        /// <summary>
        /// Replay Start Index.
        /// </summary>
        private int ReplayStartIndex
        {
            get => _replayStartIndex;
            set
            {
                if ((value <= AlteredString?.Length ||
                     value <= String.Length) &&
                    value >= 0)
                {
                    _replayStartIndex = value;
                }
                else
                {
                    _replayStartIndex = String.Length;
                }
            }
        }

        /// <summary>
        /// Replay Count Index.
        /// </summary>
        private int ReplayCountIndex { get; set; }

        /// <summary>
        /// Replay Count Limit.
        /// </summary>
        private int ReplayCountLimit { get; set; }

        /// <summary>
        /// Replay Speed Delay In Seconds.
        /// </summary>
        public float ReplaySpeedDelayInSeconds { get; set; }

        #endregion

        #region Writing & Replay Methods

        /// <summary>
        /// Replay.
        /// </summary>
        /// <param name="isReplaying">A bool indicating whether the text will be repeating it's write cycle over continuously.</param>
        /// <param name="startIndex">The character position within the text to start replaying back from. Intaken as an int.</param>
        /// <param name="replayDelayInSeconds">The number of seconds after completing the write sequence to begin replaying. Intaken as a float.</param>
        /// <param name="replayLimit">The number of times to replay the text back. -1 for infinite. Intaken as an int.</param>
        public void Replay(bool isReplaying = true, int startIndex = 0, float replayDelayInSeconds = 0.5f, int replayLimit = -1)
        {
            IsReplaying = isReplaying;
            ReplayStartIndex = startIndex;
            ReplaySpeedDelayInSeconds = replayDelayInSeconds;
            ReplayCountIndex = 0;
            ReplayCountLimit = replayLimit;
        }

        /// <summary>
        /// Write.
        /// Writes out the AlteredString or String at the WriteDelayInSeconds interval.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        /// <param name="text">Intakes a string.</param>
        private void Write(SpriteBatch spriteBatch, string text)
        {
            switch (WriteSpeed)
            {
                case WriteSpeeds.Immediate:
                    ApplyEnhancedTextOutline(spriteBatch, text);
                    ApplyEnhancedTextShadow(spriteBatch, text);
                    spriteBatch.DrawString(Font, text, new Vector2(Rectangle.X, Rectangle.Y),
                        IsInFocus ? SelectionColor * Transparencies["Background"] : FontColor * Transparencies["Background"], RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);

                    break;
                case WriteSpeeds.Delayed:
                    if (string.IsNullOrWhiteSpace(text) == false)
                    {
                        if (WriteIndex == text.Length &&
                            IsWritingComplete == false)
                        {
                            StopWriting();
                        }

                        if (IsWritingComplete &&
                            IsReplaying &&
                            (ReplayCountLimit == -1 ||
                            ReplayCountIndex < ReplayCountLimit))
                        {
                            if (ElapsedTime >= ReplaySpeedDelayInSeconds)
                            {
                                IsWritingComplete = false;
                                WriteIndex = ReplayStartIndex;

                                ElapsedTime = 0;

                                if (ReplayCountIndex != -1)
                                {
                                    ReplayCountIndex++;
                                }
                            }
                        }

                        if (WriteIndex <= text.Length &&
                            IsWritingComplete)
                        {
                            ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex));
                            ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex));
                            spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), new Vector2(Rectangle.X, Rectangle.Y),
                                IsInFocus ? SelectionColor * Transparencies["Background"] : FontColor * Transparencies["Background"], RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);
                        }
                        else if (WriteIndex < text.Length &&
                                 IsWritingComplete == false)
                        {
                            if (ElapsedTime >= WriteSpeedDelayInSeconds)
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex) + text[WriteIndex], new Vector2(Rectangle.X, Rectangle.Y),
                                    IsInFocus ? SelectionColor * Transparencies["Background"] : FontColor * Transparencies["Background"], RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);

                                WriteIndex++;
                                ElapsedTime = 0;
                            }
                            else
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex));
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex));
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), new Vector2(Rectangle.X, Rectangle.Y),
                                    IsInFocus ? SelectionColor * Transparencies["Background"] : FontColor * Transparencies["Background"], RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);
                            }

                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// ReWrite.
        /// Resets writing to begin again.
        /// </summary>
        public void ReWrite()
        {
            WriteIndex = 0;
            IsWritingComplete = false;
        }

        /// <summary>
        /// Stop Writing.
        /// </summary>
        public void StopWriting()
        {
            IsWritingComplete = true;
        }

        /// <summary>
        /// Resume Writing.
        /// Resumes writing where it was paused.
        /// </summary>
        public bool ResumeWriting(string text)
        {
            var result = false;

            if (string.IsNullOrWhiteSpace(text) == false &&
                WriteIndex < text.Length)
            {
                IsWritingComplete = false;
                result = true;
            }

            return result;
        }

        #endregion
    }
}