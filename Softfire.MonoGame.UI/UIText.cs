using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects.Transitions;

namespace Softfire.MonoGame.UI
{
    public class UIText : UIBase
    {
        /// <summary>
        /// Elapsed Time.
        /// </summary>
        private double ElapsedTime { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// Text Altered String.
        /// Any modifications performed to String, such as wrapping or scaling, should be passed to the AlteredString so as to keep the original String intact.
        /// </summary>
        public string AlteredString { get; set; }

        /// <summary>
        /// Text Font.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Font Color.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Selection Text Color.
        /// </summary>
        public Color SelectionColor { get; set; }

        /// <summary>
        /// Horizontal Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public HorizontalAlignments HorizontalAlignment { get; set; }

        /// <summary>
        /// Horizontal Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public enum HorizontalAlignments
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        /// Vertical Text Alignment.
        /// Alters Origin to align text.
        /// </summary>
        public VerticalAlignments VerticalAlignment { get; set; }

        /// <summary>
        /// Vertical Alignments.
        /// </summary>
        public enum VerticalAlignments
        {
            Upper,
            Center,
            Lower
        }

        /// <summary>
        /// Write Speed.
        /// </summary>
        public WriteSpeeds WriteSpeed { get; set; }

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
        public float WriteSpeedDelayInSeconds { get; set; }

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
            set {
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

        /// <summary>
        /// Texture.
        /// Used for Highlighting.
        /// </summary>
        private Texture2D Texture { get; set; }

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
            set {
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

        /// <summary>
        /// UIText Constructor. 
        /// </summary>
        /// <param name="font">Intakes a SpriteFont.</param>
        /// <param name="text">Intakes the text to output as a string.</param>
        public UIText(SpriteFont font, string text = null)
        {
            Font = font;
            String = text ?? string.Empty;
            FontColor = Color.Black;
            SelectionColor = Color.LightGray;
            OutlineColor = Color.Black;
            HorizontalAlignment = HorizontalAlignments.Center;
            VerticalAlignment = VerticalAlignments.Center;
            WriteSpeed = WriteSpeeds.Immediate;
            WriteSpeedDelayInSeconds = 0;
            OutlineDepth = 0;
            ShadowDepth = 0;

            AlteredString = null;
            ActivateOutlines(OutlineDepth);

            Defaults.SelectionColor = SelectionColor;
            Defaults.Font = Font;
            Defaults.Texts.Add(1, String);

            HasBackground = false;
            HasOutlines = false;
        }

        /// <summary>
        /// UIText Constructor. 
        /// Can be customized and displayed.
        /// </summary>
        /// <param name="position">Intakes the text's position as a Vector2.</param>
        /// <param name="fontColor">Intakes the Color to be used with the Font. Default is Color.White.</param>
        /// <param name="backgroundColor">Intakes the Color used for the Text's background. Default is Color.White.</param>
        /// <param name="selectionColor">Intakes the Color used when the Text is highlighted. Default is Color.LightGray.</param>
        /// <param name="outlineColor">Intakes the Color used to outline the text. Default is Color.Black.</param>
        /// <param name="horizontalAlignment">Intakes an Alignment type to align the Text. Default is HorizontalAlignments.Center.</param>
        /// <param name="verticalAlignment">Intakes an Alignment type to align the Text. Default is VerticalAlignments.Center.</param>
        /// <param name="writeSpeed">Intakes a enum value to define the writting speed of the test. Either Immediate or Delayed. Default is Immediate.</param>
        /// <param name="writeSpeedDelayInSeconds">Intakes a float to define the number of seconds to delay when writting each character of text. Default is 0.10f.</param>
        /// <param name="outlineDepth">The outline draw depth. Intaken as an int between 0 and 3. Default is 0.</param>
        /// <param name="shadowDepth">The shadow draw depth. Intaken as an int between 0 and 5. Default is 0.</param>
        /// <param name="orderNumber">Intakes an int defining the update/draw order number.</param>
        /// <param name="isVisible">Indicates whether the UIBase is visible. Intaken as a bool.</param>
        public UIText(SpriteFont font, string text,
                                       int orderNumber,
                                       Vector2 position = new Vector2(),
                                       Color? fontColor = null,
                                       Color? backgroundColor = null,
                                       Color? selectionColor = null,
                                       Color? outlineColor = null,
                                       HorizontalAlignments horizontalAlignment = HorizontalAlignments.Center,
                                       VerticalAlignments verticalAlignment = VerticalAlignments.Center,
                                       WriteSpeeds writeSpeed = WriteSpeeds.Immediate,
                                       float writeSpeedDelayInSeconds = 0.10f,
                                       int outlineDepth = 0,
                                       int shadowDepth = 0,
                                       bool isVisible = false) : base(position, (int)font.MeasureString(text).X,
                                                                                (int)font.MeasureString(text).Y,
                                                                                orderNumber,
                                                                                backgroundColor,
                                                                                null,
                                                                                isVisible)
        {
            Font = font;
            String = text ?? string.Empty;
            FontColor = fontColor ?? Color.White;
            SelectionColor = selectionColor ?? Color.LightGray;
            OutlineColor = outlineColor ?? Color.Black;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            WriteSpeed = writeSpeed;
            WriteSpeedDelayInSeconds = writeSpeedDelayInSeconds;
            OutlineDepth = outlineDepth;
            ShadowDepth = shadowDepth;

            AlteredString = null;
            ActivateOutlines(OutlineDepth);

            Defaults.SelectionColor = SelectionColor;
            Defaults.Font = Font;
            Defaults.Texts.Add(1, String);

            HasBackground = false;
            HasOutlines = false;
        }

        /// <summary>
        /// Action to be performed if Text is actioned.
        /// </summary>
        /// <param name="isActioned">Boolean expression determining whether the text has been actioned.</param>
        /// <param name="action">Action to perform.</param>
        public void Action(bool isActioned, Action action)
        {
            if (isActioned)
            {
                action();
            }
        }

        /// <summary>
        /// Set Alignments.
        /// </summary>
        private void SetAlignments()
        {
            switch (VerticalAlignment)
            {
                case VerticalAlignments.Upper:
                    Position = new Vector2(Position.X, Position.Y - Height / 2f);
                    break;
                case VerticalAlignments.Center:
                    Position = new Vector2(Position.X, Position.Y);
                    break;
                case VerticalAlignments.Lower:
                    Position = new Vector2(Position.X, Position.Y + Height / 2f);
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignments.Left:
                    Position = new Vector2(Position.X + Width / 2f, Position.Y);
                    break;
                case HorizontalAlignments.Center:
                    Position = new Vector2(Position.X, Position.Y);
                    break;
                case HorizontalAlignments.Right:
                    Position = new Vector2(Position.X - Width / 2f, Position.Y);
                    break;
            }
        }

        /// <summary>
        /// Get Length.
        /// Use to get the length and width of the String based on the current Font in use as a Vector2. Vector2(Width, Height).
        /// </summary>
        /// <returns>Returns a Vector2 defining the width and height of the String in the current Font.</returns>
        public Vector2 GetLength()
        {
            return Font.MeasureString(AlteredString ?? String);
        }

        /// <summary>
        /// Reset Font.
        /// Uses Defaults.
        /// </summary>
        public void ResetFont()
        {
            Font = Defaults.Font;
        }

        /// <summary>
        /// Reset Font Color.
        /// Uses Defaults.
        /// </summary>
        public void ResetFontColor()
        {
            FontColor = Defaults.FontColor;
        }

        /// <summary>
        /// Reset Selection Color.
        /// Uses Defaults.
        /// </summary>
        public void ResetSelectionColor()
        {
            SelectionColor = Defaults.SelectionColor;
        }

        /// <summary>
        /// Reset String.
        /// Uses Defaults.
        /// </summary>
        protected void ResetString()
        {
            String = Defaults.Texts[1];
        }

        /// <summary>
        /// Reset AlteredString.
        /// Uses Defaults.
        /// </summary>
        protected void ResetAlteredString()
        {
            AlteredString = null;
        }

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
                    spriteBatch.DrawString(Font, text, new Vector2(Rectangle.X, Rectangle.Y) +
                                                       new Vector2(PaddingLeft, PaddingTop) +
                                                       new Vector2(PaddingRight, PaddingBottom),
                        IsInFocus ? SelectionColor * Transparency : FontColor * Transparency, RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);

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
                            spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), new Vector2(Rectangle.X, Rectangle.Y) +
                                                                                        new Vector2(PaddingLeft, PaddingTop) +
                                                                                        new Vector2(PaddingRight, PaddingBottom),
                                IsInFocus ? SelectionColor * Transparency : FontColor * Transparency, RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);
                        }
                        else if (WriteIndex < text.Length &&
                                 IsWritingComplete == false)
                        {
                            if (ElapsedTime >= WriteSpeedDelayInSeconds)
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex) + text[WriteIndex]);
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex) + text[WriteIndex], new Vector2(Rectangle.X, Rectangle.Y) +
                                                                                                               new Vector2(PaddingLeft, PaddingTop) +
                                                                                                               new Vector2(PaddingRight, PaddingBottom),
                                    IsInFocus ? SelectionColor * Transparency : FontColor * Transparency, RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);

                                WriteIndex++;
                                ElapsedTime = 0;
                            }
                            else
                            {
                                ApplyEnhancedTextOutline(spriteBatch, text.Substring(0, WriteIndex));
                                ApplyEnhancedTextShadow(spriteBatch, text.Substring(0, WriteIndex));
                                spriteBatch.DrawString(Font, text.Substring(0, WriteIndex), new Vector2(Rectangle.X, Rectangle.Y) +
                                                                                            new Vector2(PaddingLeft, PaddingTop) +
                                                                                            new Vector2(PaddingRight, PaddingBottom),
                                    IsInFocus ? SelectionColor * Transparency : FontColor * Transparency, RotationAngle, Vector2.Zero, Scale, SpriteEffects.None, DrawDepth);
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
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(-i * Scale.X, -i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawTopOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(0, -i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawTopRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(i * Scale.X, -i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(i * Scale.X, 0),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomRightOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(i * Scale.X, i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(0, i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawBottomLeftOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(-i * Scale.X, i * Scale.Y),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }

                    if (DrawLeftOutline)
                    {
                        spriteBatch.DrawString(Font, text, Position +
                                                           new Vector2(PaddingLeft, PaddingTop) +
                                                           new Vector2(PaddingRight, PaddingBottom) +
                                                           new Vector2(-i * Scale.X, 0),
                            OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                    }
                }
            }
        }

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
                    spriteBatch.DrawString(Font, text, Position +
                                                       new Vector2(PaddingLeft, PaddingTop) +
                                                       new Vector2(PaddingRight, PaddingBottom) +
                                                       new Vector2(i * 1.3f, -i * 0.7f),
                        OutlineColor * Transparency, RotationAngle, Origin, Scale, SpriteEffects.None, DrawDepth);
                }
            }
        }

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
        /// Clear.
        /// Clears the String and AlteredString.
        /// </summary>
        public void Clear()
        {
            ResetString();
            ResetAlteredString();
        }

        /// <summary>
        /// Change Font Color Trigger.
        /// When the condition is met a UIEffectColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to </param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="targetColor">The target Color in the transition.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool ChangeFontColorTrigger(bool condition, string triggerEffectIdentifier, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0)
        {
            if (condition)
            {
                if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                {
                    LoadEffect(triggerEffectIdentifier, new UIEffectBackgroundColorGradiant(this, FontColor, targetColor, durationInSeconds, startDelayInSeconds));
                }

                if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                {
                    ActivateLoadedEffect(triggerEffectIdentifier);
                }
            }

            return condition;
        }

        /// <summary>
        /// Change Selection Color Trigger.
        /// When the condition is met a UIEffectColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to trigger the color change.</param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="targetColor">The target Color in the transition.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool ChangeSelectionColorTrigger(bool condition, string triggerEffectIdentifier, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0)
        {
            if (condition)
            {
                if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                {
                    LoadEffect(triggerEffectIdentifier, new UIEffectBackgroundColorGradiant(this, SelectionColor, targetColor, durationInSeconds, startDelayInSeconds));
                }

                if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                {
                    ActivateLoadedEffect(triggerEffectIdentifier);
                }
            }

            return condition;
        }

        /// <summary>
        /// Change Outline Color Trigger.
        /// When the condition is met a UIEffectColorGradiant object is loaded and activated.
        /// </summary>
        /// <param name="condition">Boolean statement used to trigger the color change.</param>
        /// <param name="triggerEffectIdentifier">The unique identifier used to retrieve the Effect. Intaken as a string.</param>
        /// <param name="targetColor">The target Color in the transition.</param>
        /// <param name="durationInSeconds">Effect duration in seconds. Intaken as a float.</param>
        /// <param name="startDelayInSeconds">Effect start delay in seconds. Intaken as a float.</param>
        /// <returns>Returns a bool indicating whether the condition was met.</returns>
        public bool ChangeOutlineColorTrigger(bool condition, string triggerEffectIdentifier, Color targetColor, float durationInSeconds = 1, float startDelayInSeconds = 0)
        {
            if (condition)
            {
                if (CheckForLoadedEffect(triggerEffectIdentifier) == false)
                {
                    LoadEffect(triggerEffectIdentifier, new UIEffectBackgroundColorGradiant(this, OutlineColor, targetColor, durationInSeconds, startDelayInSeconds));
                }

                if (CheckForActivatedEffect(triggerEffectIdentifier) == false)
                {
                    ActivateLoadedEffect(triggerEffectIdentifier);
                }
            }

            return condition;
        }

        /// <summary>
        /// Load Content.
        /// </summary>
        public override void LoadContent()
        {
            if (Texture == null)
            {
                CreateTexture2D();
            }

            base.LoadContent();
        }

        /// <summary>
        /// Update Method.
        /// Updates UIText attributes.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                ElapsedTime += DeltaTime;

                Width = (int)GetLength().X;
                Height = (int)GetLength().Y;

                SetAlignments();

                await base.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw Method.
        /// Draws UIText.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);

                Write(spriteBatch, string.IsNullOrWhiteSpace(AlteredString) ? String : AlteredString);
            }
        }
    }
}