using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects;

namespace Softfire.MonoGame.UI
{
    public abstract partial class UIBase
    {

        /// <summary>
        /// Content Manager.
        /// </summary>
        public static ContentManager Content { protected get; set; }

        /// <summary>
        /// Graphics Device.
        /// </summary>
        public static GraphicsDevice GraphicsDevice { protected get; set; }

        /// <summary>
        /// Delta Time.
        /// </summary>
        protected double DeltaTime { get; set; }

        /// <summary>
        /// Internal Index Number.
        /// </summary>
        private int _index;

        /// <summary>
        /// Index Number.
        /// </summary>
        public int IndexNumber
        {
            get => _index;
            protected set => _index = value > 0 ? value : 1;
        }

        /// <summary>
        /// Internal Is Visible.
        /// </summary>
        private bool _isVisible;

        /// <summary>
        /// UIBase Is Visible?
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value)
                {
                    IsActive = true;
                    _isVisible = true;
                }
                else
                {
                    _isVisible = false;
                }
            }
        }

        /// <summary>
        /// Internal Is Active.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// UiBase Is Active?
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value)
                {
                    _isActive = true;
                }
                else
                {
                    IsVisible = false;
                    _isActive = false;
                }
            }
        }

        /// <summary>
        /// Is In Focus?
        /// </summary>
        public bool IsInFocus { get; set; }

        /// <summary>
        /// UIBase Is Movable?
        /// </summary>
        protected bool IsMovable { get; set; }

        /// <summary>
        /// UIBase Is Moving?
        /// </summary>
        public bool IsMoving { get; set; }
        
        /// <summary>
        /// Is Highlighting Enabled?
        /// Enable this to use highlighting!
        /// </summary>
        public bool IsHighlightingEnabled { get; set; }

        /// <summary>
        /// UIBase Has Outlines?
        /// Enables/Disables a colored border around the UIBase.
        /// </summary>
        public bool HasOutlines { get; set; }

        /// <summary>
        /// Has Background?
        /// </summary>
        public bool HasBackground { get; set; }

        /// <summary>
        /// UI Defaults.
        /// </summary>
        public UIDefaults Defaults { get; }

        /// <summary>
        /// Parent Position.
        /// Set if this UIBase is a child of another class.
        /// Combined with Position in Update.
        /// Default is Vector.Zero.
        /// </summary>
        public Vector2 ParentPosition { get; set; } = Vector2.Zero;

        /// <summary>
        /// UI Position.
        /// Position is relative to ParentPosition.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// UI Origin.
        /// </summary>
        public Vector2 Origin { get; protected set; }

        /// <summary>
        /// UI Rotation Angle.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// UI Rectangle.
        /// </summary>
        public Rectangle Rectangle { get; protected set; }

        /// <summary>
        /// Internal Width.
        /// </summary>
        private int _width;

        /// <summary>
        /// UI Width.
        /// </summary>
        public int Width
        {
            get => _width;
            set {
                if (value >= WidthMin &&
                    value <= WidthMax)
                {
                    _width = value;
                }
                else if (value < WidthMin)
                {
                    _width = WidthMin;
                }
                else if (value > WidthMax)
                {
                    _width = WidthMax;
                }
            }
        }

        /// <summary>
        /// UI WidthF.
        /// Used in Drawing.
        /// </summary>
        private float WidthF { get; set; }

        /// <summary>
        /// Internal Width Minimum.
        /// </summary>
        private int _widthMin;

        /// <summary>
        /// Width Minimum.
        /// </summary>
        public int WidthMin
        {
            get => _widthMin;
            set => _widthMin = value >= 0 ? value : 0;
        }

        /// <summary>
        /// Internal Width Maximum.
        /// </summary>
        private int _widthMax;

        /// <summary>
        /// Width Maximum.
        /// </summary>
        public int WidthMax
        {
            get => _widthMax;
            set => _widthMax = value >= 0 ? value : int.MaxValue;
        }

        /// <summary>
        /// Internal Height.
        /// </summary>
        private int _height;

        /// <summary>
        /// UI Height.
        /// </summary>
        public int Height
        {
            get => _height;
            set {
                if (value >= HeightMin &&
                    value <= HeightMax)
                {
                    _height = value;
                }
                else if (value < HeightMin)
                {
                    _height = HeightMin;
                }
                else if (value > HeightMax)
                {
                    _height = HeightMax;
                }
            }
        }

        /// <summary>
        /// UI HeightF.
        /// Used in Drawing.
        /// </summary>
        private float HeightF { get; set; }

        /// <summary>
        /// Internal Height Minimum.
        /// </summary>
        private int _heightMin;

        /// <summary>
        /// Height Minimum.
        /// </summary>
        public int HeightMin
        {
            get => _heightMin;
            set => _heightMin = value >= 0 ? value : 0;
        }
        
        /// <summary>
        /// Internal Height Maximum.
        /// </summary>
        private int _heightMax;

        /// <summary>
        /// Height Maximum.
        /// </summary>
        public int HeightMax
        {
            get => _heightMax;
            set => _heightMax = value >= 0 ? value : int.MaxValue;
        }

        /// <summary>
        /// UI Texture.
        /// </summary>
        private Texture2D BackgroundTexture { get; set; }

        /// <summary>
        /// Texture File Path.
        /// </summary>
        private string BackgroundTextureFilePath { get; }

        /// <summary>
        /// UI Color.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// UI Scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// UI Transparency.
        /// </summary>
        public float Transparency { get; set; }

        /// <summary>
        /// UI Draw Depth.
        /// </summary>
        public float DrawDepth { get; set; }

        /// <summary>
        /// UIBase Outline.
        /// </summary>
        public Dictionary<Outlines, bool> OutlinesDictionary { get; }

        /// <summary>
        /// UIBase Outline Edges.
        /// </summary>
        public enum Outlines
        {
            Top,
            Right,
            Bottom,
            Left
        }

        /// <summary>
        /// Outline Thickness.
        /// </summary>
        public int OutlineThickness { get; set; }

        /// <summary>
        /// Outline Color.
        /// </summary>
        public Color OutlineColor { get; set; }

        /// <summary>
        /// Highlight Texture.
        /// Used for Highlighting.
        /// </summary>
        private Texture2D HighlightTexture { get; set; }

        /// <summary>
        /// Highlight Color.
        /// </summary>
        public Color HighlightColor { get; set; }

        /// <summary>
        /// Highlight Transparency Level.
        /// </summary>
        public float HighlightTransparencyLevel { get; set; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Order Number.
        /// Window will be updated/drawn in order from smallest to largest.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value > 0 ? value : 1;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="position">Intakes the UI's position as a Vector2.</param>
        /// <param name="width">Intakes the UI's width as a float.</param>
        /// <param name="height">Intakes the UI's height as a float.</param>
        /// <param name="orderNumber">Intakes the UI's Update/Draw Order Number as an int.</param>
        protected UIBase(Vector2 position = new Vector2(), int width = 10, int height = 10, int orderNumber = 0)
        {
            // Prerequisites
            WidthMin = 0;
            WidthMax = int.MaxValue;
            HeightMin = 0;
            HeightMax = int.MaxValue;

            // Setup
            Position = position;
            Width = width;
            Height = height;
            OrderNumber = orderNumber;
            BackgroundColor = Color.White;

            // Effects
            LoadedEffects = new Dictionary<string, UIBaseEffect>();
            ActiveEffects = new List<UIBaseEffect>();
            AreEffectsRunning = false;
            ActivateEffects = false;
            RunEffectsSequencially = false;

            Scale = Vector2.One;
            Transparency = 0f;
            RotationAngle = 0f;
            DrawDepth = 1f;

            OutlineThickness = 1;
            OutlineColor = Color.Black;

            OutlinesDictionary = new Dictionary<Outlines, bool>(4)
            {
                {Outlines.Top, false},
                {Outlines.Right, false},
                {Outlines.Bottom, false},
                {Outlines.Left, false}
            };

            IsActive = true;
            IsVisible = false;
            IsMovable = false;
            IsMoving = false;
            HasBackground = false;
            HasOutlines = true;

            Defaults = new UIDefaults
            {
                Index = IndexNumber,
                IsActive = IsActive,
                IsVisible = IsVisible,
                Position = Position,
                Width = Width,
                Height = Height,
                BackgroundColor = BackgroundColor,
                BackgroundTransparency = Transparency,
                DrawDepth = DrawDepth,
                OrderNumber = OrderNumber,
                OutlineThickness = OutlineThickness,
                OutlineColor = OutlineColor,
                Scale = Scale,
                RotationAngle = RotationAngle
            };
        }

        /// <summary>
        /// UI Base Constructor.
        /// </summary>
        /// <param name="position">Intakes the UI's position as a Vector2.</param>
        /// <param name="width">Intakes the UI's width as a float.</param>
        /// <param name="height">Intakes the UI's height as a float.</param>
        /// <param name="orderNumber">Intakes the UI's Update/Draw Order Number as an int.</param>
        /// <param name="backgroundColor">Intakes the UI's background color as Color.</param>
        /// <param name="textureFilePath">Intakes the a texture's file path as a string.</param>
        /// <param name="isVisible">Indicates whether the UIBase is visible. Intaken as a bool.</param>
        protected UIBase(Vector2 position, int width, int height, int orderNumber, Color? backgroundColor = null, string textureFilePath = null, bool isVisible = false)
        {
            // Prerequisites
            WidthMin = 0;
            WidthMax = int.MaxValue;
            HeightMin = 0;
            HeightMax = int.MaxValue;

            // Setup
            Position = position;
            Width = width;
            Height = height;
            OrderNumber = orderNumber;
            BackgroundColor = backgroundColor ?? Color.White;

            if (textureFilePath != null)
            {
                BackgroundTextureFilePath = textureFilePath;
                
            }

            // Effects
            LoadedEffects = new Dictionary<string, UIBaseEffect>();
            ActiveEffects = new List<UIBaseEffect>();
            AreEffectsRunning = false;
            ActivateEffects = false;
            RunEffectsSequencially = false;

            Scale = Vector2.One;
            Transparency = 0f;
            RotationAngle = 0f;
            DrawDepth = 1f;

            OutlineThickness = 1;
            OutlineColor = Color.Black;

            OutlinesDictionary = new Dictionary<Outlines, bool>(4)
            {
                {Outlines.Top, false},
                {Outlines.Right, false},
                {Outlines.Bottom, false},
                {Outlines.Left, false}
            };

            IsActive = true;
            IsVisible = isVisible;
            IsMovable = false;
            IsMoving = false;
            HasBackground = backgroundColor != null;
            HasOutlines = string.IsNullOrWhiteSpace(textureFilePath);

            Defaults = new UIDefaults
            {
                Index = IndexNumber,
                IsVisible = IsVisible,
                Position = Position,
                Width = Width,
                Height = Height,
                BackgroundColor = BackgroundColor,
                BackgroundTransparency = Transparency,
                DrawDepth = DrawDepth,
                OrderNumber = OrderNumber,
                OutlineThickness = OutlineThickness,
                OutlineColor = OutlineColor,
                Scale = Scale,
                RotationAngle = RotationAngle
            };
        }

        /// <summary>
        /// Create Texture2D.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        protected Texture2D CreateTexture2D()
        {
            var texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            return texture;
        }

        /// <summary>
        /// Highlight.
        /// Use this method to control highlighting.
        /// </summary>
        /// <param name="isHighlightingEnabled">Intakes a boolean defining whether highlighting is to be used.</param>
        /// <param name="highlightColor">Intakes a Color to define the default highlight Color to use. Default is Color.AliceBlue.</param>
        /// <param name="highlightTransparencyLevel">Intakes a float between 0.0f and 1.0f to define the level of transparency to apply. The lower the number, the greater the level of transparency. Default is 0.25f.</param>
        public void Highlight(bool isHighlightingEnabled = true, Color? highlightColor = null, float highlightTransparencyLevel = 0.25f)
        {
            IsHighlightingEnabled = isHighlightingEnabled;
            HighlightColor = highlightColor ?? Color.AliceBlue;
            HighlightTransparencyLevel = highlightTransparencyLevel;
        }

        /// <summary>
        /// Activate Outlines.
        /// </summary>
        /// <param name="isActive">Intakes a bool indicating whether the Outlines are to be activated.</param>
        public void ActivateOutlines(bool isActive)
        {
            OutlinesDictionary[Outlines.Top] = isActive;
            OutlinesDictionary[Outlines.Right] = isActive;
            OutlinesDictionary[Outlines.Bottom] = isActive;
            OutlinesDictionary[Outlines.Left] = isActive;
        }

        /// <summary>
        /// Reset UI Visibility.
        /// Uses Defaults.
        /// </summary>
        public void ResetVisibility()
        {
            IsVisible = Defaults.IsVisible;
        }

        /// <summary>
        /// Reset UI Position.
        /// Uses Defaults.
        /// </summary>
        public void ResetPosition()
        {
            Position = Defaults.Position;
        }

        /// <summary>
        /// Reset UI Width.
        /// Uses Defaults.
        /// </summary>
        public void ResetWidth()
        {
            Width = Defaults.Width;
        }

        /// <summary>
        /// Reset UI Height.
        /// Uses Defaults.
        /// </summary>
        public void ResetHeight()
        {
            Height = Defaults.Height;
        }

        /// <summary>
        /// Reset UI Background Color.
        /// Uses Defaults.
        /// </summary>
        public void ResetBackgroundColor()
        {
            BackgroundColor = Defaults.BackgroundColor;
        }

        /// <summary>
        /// Reset UI Transparency.
        /// Uses Defaults.
        /// </summary>
        public void ResetTransparency()
        {
            Transparency = Defaults.BackgroundTransparency;
        }

        /// <summary>
        /// Reset UI Draw Depth.
        /// Uses Defaults.
        /// </summary>
        public void ResetDrawDepth()
        {
            DrawDepth = Defaults.DrawDepth;
        }

        /// <summary>
        /// Reset UI Order Number.
        /// Uses Defaults.
        /// </summary>
        public void ResetOrderNumber()
        {
            OrderNumber = Defaults.OrderNumber;
        }

        /// <summary>
        /// Reset UI Outline Thickness.
        /// Uses Defaults.
        /// </summary>
        public void ResetOutlineThickness()
        {
            OutlineThickness = Defaults.OutlineThickness;
        }

        /// <summary>
        /// Reset UI Outline Color.
        /// Uses Defaults.
        /// </summary>
        public void ResetOutlineColor()
        {
            OutlineColor = Defaults.OutlineColor;
        }

        /// <summary>
        /// Reset UI Scale.
        /// Uses Defaults.
        /// </summary>
        public void ResetScale()
        {
            Scale = Defaults.Scale;
        }

        /// <summary>
        /// Reset UI Rotation Angle.
        /// Uses Defaults.
        /// </summary>
        public void ResetRotationAngle()
        {
            RotationAngle = Defaults.RotationAngle;
        }

        /// <summary>
        /// Check Is In Focus.
        /// Pass in a rectangle to check if it intersects with or is inside this object's Rectangle.
        /// IsInFocus is updated in this call.
        /// </summary>
        /// <param name="rectangle">The rectangle to check against.</param>
        public bool CheckIsInFocus(Rectangle rectangle)
        {
            var result = false;

            if (IsActive)
            {
                result = Rectangle.Intersects(rectangle) ||
                         Rectangle.Contains(rectangle);
            }

            return IsInFocus = result;
        }

        /// <summary> 
        /// Check Is In Focus.
        /// Pass in an index number to be used to check if it matches the Index Number.
        /// IsInFocus is updated in this call.
        /// </summary>
        /// <param name="indexNumber">The index number to compare. Intaken as a int.</param>
        public bool CheckIsInFocus(int indexNumber)
        {
            var result = false;

            if (IsActive)
            {
                result = IndexNumber == indexNumber;
            }

            return IsInFocus = result;
        }

        /// <summary>
        /// UI Base Load Content Method.
        /// </summary>
        /// <param name="textureFilePath">Intakes the a texture's file path as a string.</param>
        public virtual void LoadContent()
        {
            BackgroundTexture = string.IsNullOrWhiteSpace(BackgroundTextureFilePath) == false ? Content.Load<Texture2D>(BackgroundTextureFilePath) : CreateTexture2D();

            if (HighlightTexture == null)
            {
                HighlightTexture = CreateTexture2D();
            }
        }

        /// <summary>
        /// UI Base Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public virtual async Task Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            Origin = string.IsNullOrWhiteSpace(BackgroundTextureFilePath) ?
                     new Vector2((Width * Scale.X) / 2f, (Height * Scale.Y) / 2f) :
                     new Vector2((BackgroundTexture.Width * Scale.X) / 2f, (BackgroundTexture.Height * Scale.Y) / 2f);

            WidthF = PaddingLeft + OutlineThickness + Width * Scale.X + OutlineThickness + PaddingRight;
            HeightF = PaddingTop + OutlineThickness + Height * Scale.Y + OutlineThickness + PaddingBottom;

            Rectangle = new Rectangle((int)(ParentPosition.X + Position.X - Origin.X - OutlineThickness - PaddingLeft),
                                      (int)(ParentPosition.Y + Position.Y - Origin.Y - OutlineThickness - PaddingTop),
                                      (int)WidthF,
                                      (int)HeightF);

            if (ActivateEffects)
            {
                await RunActiveEffects(RunEffectsSequencially);
            }
        }

        /// <summary>
        /// UI Base Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                if (HasOutlines)
                {
                    if (OutlinesDictionary[Outlines.Top])
                    {
                        spriteBatch.Draw(BackgroundTexture, new Vector2(Rectangle.X - OutlineThickness * 2, Rectangle.Y - OutlineThickness * 2), null, OutlineColor * Transparency, RotationAngle, Vector2.Zero, new Vector2(WidthF + OutlineThickness * 4, OutlineThickness * 2), SpriteEffects.None, DrawDepth);
                    }

                    if (OutlinesDictionary[Outlines.Right])
                    {
                        spriteBatch.Draw(BackgroundTexture, new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y - OutlineThickness * 2), null, OutlineColor * Transparency, RotationAngle, Vector2.Zero, new Vector2(OutlineThickness * 2, HeightF + OutlineThickness * 4), SpriteEffects.None, DrawDepth);
                    }

                    if (OutlinesDictionary[Outlines.Bottom])
                    {
                        spriteBatch.Draw(BackgroundTexture, new Vector2(Rectangle.X - OutlineThickness * 2, Rectangle.Y + Rectangle.Height), null, OutlineColor * Transparency, RotationAngle, Vector2.Zero, new Vector2(WidthF + OutlineThickness * 4, OutlineThickness * 2), SpriteEffects.None, DrawDepth);
                    }

                    if (OutlinesDictionary[Outlines.Left])
                    {
                        spriteBatch.Draw(BackgroundTexture, new Vector2(Rectangle.X - OutlineThickness * 2, Rectangle.Y - OutlineThickness * 2), null, OutlineColor * Transparency, RotationAngle, Vector2.Zero, new Vector2(OutlineThickness * 2, HeightF + OutlineThickness * 4), SpriteEffects.None, DrawDepth);
                    }
                }

                if (HasBackground)
                {
                    // Draw Base.
                    spriteBatch.Draw(BackgroundTexture, new Vector2(Rectangle.X, Rectangle.Y), null, BackgroundColor * Transparency, RotationAngle, Vector2.Zero, new Vector2(WidthF, HeightF), SpriteEffects.None, DrawDepth);
                }

                // If Highlighting is enabled and base has focus.
                if (IsHighlightingEnabled &&
                    IsInFocus)
                {
                    // Highlight.
                    spriteBatch.Draw(HighlightTexture, new Vector2(Rectangle.X, Rectangle.Y), null, HighlightColor * HighlightTransparencyLevel, RotationAngle, Vector2.Zero, new Vector2(WidthF, HeightF), SpriteEffects.None, DrawDepth);
                }
            }
        }
    }
}