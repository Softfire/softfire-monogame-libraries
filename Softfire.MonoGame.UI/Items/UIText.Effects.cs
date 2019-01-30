using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.UI.Items
{
    public partial class UIText
    {
        #region Outline Properties

        /// <summary>
        /// The text's internal outline depth value.
        /// </summary>
        private int _outlineDepth;

        /// <summary>
        /// The text's outline depth.
        /// </summary>
        /// <remarks>Minimum of 0 and a maximum of 3.</remarks>
        public int OutlineDepth
        {
            get => _outlineDepth;
            set => _outlineDepth = value < 0 ? 0 : value > 3 ? 3 : value;
        }

        /// <summary>
        /// The text's outline status.
        /// </summary>
        private Dictionary<string, bool> TextsOutlineStatus { get; }

        #endregion

        #region Outline Methods

        /// <summary>
        /// Activates outlines around the text.
        /// </summary>
        /// <param name="topLeft">Enable/disables the top left outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="top">Enable/disables the top outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="topRight">Enable/disables the top right outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="right">Enable/disables the right outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="bottomRight">Enable/disables the bottom right outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="bottom">Enable/disables the bottom outline. Intaken as a <see cref="bool"/>. </param>
        /// <param name="bottomLeft">Enable/disables the bottom left outline. Intaken as a <see cref="bool"/>.</param>
        /// <param name="left">Enable/disables the left outline. Intaken as a <see cref="bool"/>.</param>
        public void ActivateOutlines(bool top = false, bool right = false, bool bottom = false, bool left = false,
                                     bool topLeft = false, bool topRight = false, bool bottomRight = true, bool bottomLeft = false)
        {
            TextsOutlineStatus["Top"] = top;
            TextsOutlineStatus["Right"] = right;
            TextsOutlineStatus["Bottom"] = bottom;
            TextsOutlineStatus["Left"] = left;
            TextsOutlineStatus["TopLeft"] = topLeft;
            TextsOutlineStatus["TopRight"] = topRight;
            TextsOutlineStatus["BottomRight"] = bottomRight;
            TextsOutlineStatus["BottomLeft"] = bottomLeft;
        }

        /// <summary>
        /// Applies a basic outline around the text.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        public void ApplyBasicTextOutline(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, AlteredString ?? String, Transform.Position + new Vector2(1, 1), Color.Black);

            spriteBatch.DrawString(Font, AlteredString ?? String, Transform.Position, Color.Aqua);
        }

        /// <summary>
        /// Applies the enhanced text outlines around the text.
        /// </summary>
        /// <param name="spriteBatch">The current <see cref="SpriteBatch"/>.</param>
        /// <param name="text">The text to outline. Intaken as a <see cref="string"/>.</param>
        private void ApplyEnhancedTextOutline(SpriteBatch spriteBatch, string text)
        {
            for (var i = OutlineDepth; i > 0; i--)
            {
                if (TextsOutlineStatus["TopLeft"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(-i * Transform.Scale.X, -i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["Top"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(0, -i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["TopRight"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(i * Transform.Scale.X, -i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["Right"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(i * Transform.Scale.X, 0),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["BottomRight"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(i * Transform.Scale.X, i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["Bottom"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(0, i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["BottomLeft"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(-i * Transform.Scale.X, i * Transform.Scale.Y),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }

                if (TextsOutlineStatus["Left"])
                {
                    spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(-i * Transform.Scale.X, 0),
                        Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }
            }
        }

        #endregion

        #region Shadow Depth Properties

        /// <summary>
        /// The text's internal shadow depth value.
        /// </summary>
        private int _shadowDepth;

        /// <summary>
        /// The text's shadow depth.
        /// </summary>
        public int ShadowDepth
        {
            get => _shadowDepth;
            set => _shadowDepth = value < 0 ? 0 : value > 5 ? 5 : value;
        }

        #endregion

        #region Shadow Depth Methods

        /// <summary>
        /// Apply Basic Text Shadow.
        /// </summary>
        /// <param name="spriteBatch">Intakes the current SpriteBath.</param>
        public void ApplyBasicTextShadow(SpriteBatch spriteBatch)
        {
            for (var i = ShadowDepth; i > 0; i--)
            {
                spriteBatch.DrawString(Font, AlteredString ?? String, Transform.Position + new Vector2(i * 1.3f, -i * 0.7f), Color.Aqua,
                                       Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);
            }

            spriteBatch.DrawString(Font, AlteredString ?? String, Transform.Position, Color.Aqua,
                                   Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);
        }
        
        /// <summary>
        /// Applies the enhanced text shadows to the text.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="text">The text to have shadows applied. Intaken as a <see cref="string"/>.</param>
        private void ApplyEnhancedTextShadow(SpriteBatch spriteBatch, string text)
        {
            for (var i = ShadowDepth; i > 0; i--)
            {
                spriteBatch.DrawString(Font, text, Transform.Position + new Vector2(i * 1.3f, -i * 0.7f),
                                       Color.Black * GetTransparency("Background").Level, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
            }
        }

        #endregion

        #region Writing & Replay Properties

        /// <summary>
        /// The text write speed.
        /// </summary>
        public WriteSpeeds WriteSpeed { get; set; } = WriteSpeeds.Immediate;

        /// <summary>
        /// The text's available write speeds.
        /// </summary>
        public enum WriteSpeeds
        {
            /// <summary>
            /// Text is written immediately.
            /// </summary>
            Immediate,
            /// <summary>
            /// Text is written after a delay.
            /// </summary>
            Delayed
        }

        /// <summary>
        /// The text's write speed delay in seconds.
        /// </summary>
        public float WriteSpeedDelayInSeconds { get; set; } = 0.10f;

        /// <summary>
        /// The text's write index.
        /// </summary>
        private int WriteIndex { get; set; }

        /// <summary>
        /// Is the text writing complete?
        /// </summary>
        private bool IsWritingComplete { get; set; }

        /// <summary>
        /// Is the text replaying?
        /// </summary>
        private bool IsReplaying { get; set; }

        /// <summary>
        /// The text's internal replay start index value.
        /// </summary>
        private int _replayStartIndex;

        /// <summary>
        /// The text's replay start index.
        /// </summary>
        private int ReplayStartIndex
        {
            get => _replayStartIndex;
            set => _replayStartIndex = (value <= AlteredString?.Length || value <= String.Length) && value >= 0 ? value : String.Length;
        }

        /// <summary>
        /// The text's replay count index.
        /// </summary>
        private int ReplayCountIndex { get; set; }

        /// <summary>
        /// The text's replay count limit.
        /// </summary>
        private int ReplayCountLimit { get; set; }

        /// <summary>
        /// The text's replay speed delay in seconds.
        /// </summary>
        public float ReplaySpeedDelayInSeconds { get; set; }

        #endregion

        #region Writing & Replay Methods

        /// <summary>
        /// The text's replay function. The text is re-written from the provided start index and can have a start delay and repeat limit.
        /// </summary>
        /// <param name="isReplaying">Whether the text will be repeating it's write cycle. Intaken as a <see cref="bool"/>.</param>
        /// <param name="startIndex">The character position within the text to start replaying back from. Intaken as an <see cref="int"/>.</param>
        /// <param name="replayDelayInSeconds">The number of seconds after completing the write sequence to begin replaying. Intaken as a <see cref="float"/>.</param>
        /// <param name="replayLimit">The number of times to replay the text back. Use -1 for infinite. Intaken as an <see cref="int"/>.</param>
        public void Replay(bool isReplaying = true, int startIndex = 0, float replayDelayInSeconds = 0.5f, int replayLimit = -1)
        {
            IsReplaying = isReplaying;
            ReplayStartIndex = startIndex;
            ReplaySpeedDelayInSeconds = replayDelayInSeconds;
            ReplayCountIndex = 0;
            ReplayCountLimit = replayLimit;
        }

        /// <summary>
        /// Writes out the AlteredString or String at the WriteDelayInSeconds interval.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        /// <param name="text">The text to write. Intaken as a <see cref="string"/>.</param>
        private void Write(SpriteBatch spriteBatch, Matrix transform, string text)
        {
            // Apply any transformations.
            var position = Vector2.Transform(new Vector2(Rectangle.X, Rectangle.Y), transform);

            switch (WriteSpeed)
            {
                case WriteSpeeds.Immediate:
                    ApplyEnhancedTextOutline(spriteBatch, text);
                    ApplyEnhancedTextShadow(spriteBatch, text);
                    spriteBatch.DrawString(Font, text, position,
                                           IsStateSet(FocusStates.IsHovered) && IsSelectable ? Colors["Selection"] * GetTransparency("Selection").Level : Colors["Font"] * GetTransparency("Background").Level,
                                           Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);

                    break;
                case WriteSpeeds.Delayed:
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        if (WriteIndex == text.Length &&
                            !IsWritingComplete)
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

                                ResetElapsedTime();

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
                            spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), position,
                                                   IsStateSet(FocusStates.IsHovered) && IsSelectable ? Colors["Selection"] * GetTransparency("Selection").Level : Colors["Font"] * GetTransparency("Background").Level,
                                                   Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);
                        }
                        else if (WriteIndex < text.Length &&
                                 !IsWritingComplete)
                        {
                            if (ElapsedTime >= WriteSpeedDelayInSeconds)
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex) + text[WriteIndex], position,
                                                       IsStateSet(FocusStates.IsHovered) && IsSelectable ? Colors["Selection"] * GetTransparency("Selection").Level : Colors["Font"] * GetTransparency("Background").Level,
                                                       Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);

                                WriteIndex++;
                                ResetElapsedTime();
                            }
                            else
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex));
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex));
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), position,
                                                       IsStateSet(FocusStates.IsHovered) && IsSelectable ? Colors["Selection"] * GetTransparency("Selection").Level : Colors["Font"] * GetTransparency("Background").Level,
                                                       Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 1);
                            }

                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Resets writing to begin again.
        /// </summary>
        public void ReWrite()
        {
            WriteIndex = 0;
            IsWritingComplete = false;
        }

        /// <summary>
        /// Stops the current writing process.
        /// </summary>
        public void StopWriting()
        {
            IsWritingComplete = true;
        }

        /// <summary>
        /// Resumes writing from where it was stopped.
        /// </summary>
        public bool ResumeWriting(string text)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(text) &&
                WriteIndex < text.Length)
            {
                IsWritingComplete = false;
                result = true;
            }

            return result;
        }

        #endregion

        #region Scaling

        /// <summary>
        /// Apply Basic Text Scaling.
        /// </summary>
        public void ApplyBasicTextScaling(out Vector2 position, out Vector2 scale)
        {
            // Get the size (length) of the string.
            var size = Font.MeasureString(String);

            // Get the scale required to fill the rectangle.
            scale = new Vector2(Parent.Size.Width / size.X, Parent.Size.Height / size.Y);

            // Calculate the text's width and height after scaling has been applied.
            var strWidth = (int)Math.Round(size.X * scale.X);
            var strHeight = (int)Math.Round(size.Y * scale.Y);
            
            // Calculate the center most position with scaling applied.
            if (Parent != null)
            {
                position = new Vector2
                {
                    X = (Parent.Size.Width - strWidth) / 2f + Parent.Transform.Position.X,
                    Y = (Parent.Size.Height - strHeight) / 2f + Parent.Transform.Position.Y
                };
            }
            else
            {
                position = new Vector2
                {
                    X = (Parent.Size.Width - strWidth) / 2f,
                    Y = (Parent.Size.Height - strHeight) / 2f
                };
            }
        }
        
        /// <summary>
        /// Apply Enhanced Text Scaling.
        /// Use to scale the text to fit within the boundaries of the supplied container Rectangle.
        /// </summary>
        /// <param name="container">Intakes a Rectangle.</param>
        /// <param name="useContainerPosition">Intakes a boolean defining whether to use the container's position to offset the centering of the text. Default is false.</param>
        public void ApplyEnhancedTextScaling(RectangleF container, bool useContainerPosition = false)
        {
            // Get the size (length) of the string.
            var size = GetLength();

            // Get the scale required to fill the rectangle.
            Transform.Scale = new Vector2(container.Width / size.X, container.Height / size.Y);

            // Calculate the text's width and height after scaling has been applied.
            var strWidth = (int)Math.Round(size.X * Transform.Scale.X);
            var strHeight = (int)Math.Round(size.Y * Transform.Scale.Y);

            // Calculate the center most position with scaling applied.
            if (useContainerPosition)
            {
                Transform.Position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f + container.X,
                    Y = (container.Height - strHeight) / 2f + container.Y
                };
            }
            else
            {
                Transform.Position = new Vector2
                {
                    X = (container.Width - strWidth) / 2f,
                    Y = (container.Height - strHeight) / 2f
                };
            }
        }

        #endregion

        #region Wrapping

        /// <summary>
        /// Apply Basic Text Wrap.
        /// Used to keep text within the bounds of the supplied Rectangle.
        /// </summary>
        /// <param name="position">Intakes the text's current position as a Vector2.</param>
        /// <param name="container">Intakes the text's container's Rectangle.</param>
        /// <returns>Returns a modified <see cref="String"/>.</returns>
        public string ApplyBasicTextWrap(Vector2 position, RectangleF container)
        {
            var line = string.Empty;
            var alteredString = string.Empty;
            var wordArray = String.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < wordArray.Length; ++i)
            {
                if (Font.MeasureString(line + wordArray[i]).X + position.X > container.Width)
                {
                    if (Font.MeasureString(wordArray[i]).X + position.X < container.Width)
                    {
                        alteredString += line + Environment.NewLine;
                        line = string.Empty;
                    }

                    foreach (var character in wordArray[i].ToCharArray())
                    {
                        if (Font.MeasureString(line + character + "-").X + position.X > container.Width)
                        {
                            alteredString += line + "-" + Environment.NewLine;
                            line = string.Empty;
                        }

                        line += character;
                    }

                    line += " ";
                }
                else
                {
                    line += i == wordArray.Length - 1 ? wordArray[i] : wordArray[i] + " ";
                }
            }

            return alteredString + line;
        }

        /// <summary>
        /// Apply Enhanced Text Wrap.
        /// Used to keep text within the bounds of the supplied Rectangle.
        /// </summary>
        /// <param name="container">Intakes the text's container's Rectangle.</param>
        public void ApplyEnhancedTextWrap(RectangleF container)
        {
            var line = string.Empty;
            var wordArray = String.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            // Clear AlteredString.
            Clear();

            for (var i = 0; i < wordArray.Length; ++i)
            {
                // Begin breaking down the line if the current line plus the next word goes beyond the container width.
                if (Font.MeasureString(line + wordArray[i]).X + Rectangle.X > container.Width)
                {
                    // Break the line if the current word goes is below the container width.
                    if (Font.MeasureString(wordArray[i]).X + Rectangle.X < container.Width)
                    {
                        AlteredString += line + Environment.NewLine;
                        line = string.Empty;
                    }

                    foreach (var character in wordArray[i].ToCharArray())
                    {
                        // Break the word that goes beyond the container width.
                        if (Font.MeasureString(line + character + "-").X + Rectangle.X > container.Width)
                        {
                            AlteredString += line + "-" + Environment.NewLine;
                            line = string.Empty;
                        }

                        line += character;
                    }

                    line += " ";
                }
                else
                {
                    line += i == wordArray.Length - 1 ? wordArray[i] : wordArray[i] + " ";
                }
            }

            AlteredString += line;
        }

        #endregion
    }
}