using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.UI.Effects;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// The base UI class.
    /// </summary>
    /// <remarks>Extended by UIBase.Effects and UIBase.Generics.</remarks>
    public abstract partial class UIBase : IUIIdentifier
    {
        /// <summary>
        /// The UI's graphics device.
        /// </summary>
        internal static GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// The UI's effects manager.
        /// </summary>
        public UIEffectsManager UIEffectsManager { get; } = new UIEffectsManager();

        /// <summary>
        /// The UI's textures.
        /// </summary>
        protected internal Dictionary<string, Texture2D> Textures { get; } = new Dictionary<string, Texture2D>();

        /// <summary>
        /// The UI's colors.
        /// </summary>
        public Dictionary<string, Color> Colors { get; } = new Dictionary<string, Color>(6)
        {
            { "Background", Color.White },
            { "Highlight", Color.AliceBlue },
            { "Outline", Color.Black },
            { "Font", Color.Black },
            { "FontHighlight", Color.LightGray },
            { "Selection", Color.Gray }
        };

        /// <summary>
        /// The UI's transparency levels.
        /// </summary>
        public Dictionary<string, float> Transparencies { get; } = new Dictionary<string, float>(5)
        {
            { "Background", 1f },
            { "Highlight", 0.25f },
            { "Outline", 1f },
            { "Font", 1f },
            { "Selection", 0.75f }
        };

        /// <summary>
        /// UIBase Outline.
        /// </summary>
        public List<UIBaseOutline> Outlines { get; }

        /// <summary>
        /// UI Defaults.
        /// </summary>
        public UIDefaults<UIBase> Defaults { get; }

        /// <summary>
        /// Delta Time.
        /// Time between updates.
        /// </summary>
        protected double DeltaTime { get; set; }

        #region Fields

        /// <summary>
        /// Internal Index Number.
        /// Identifies the UIBase and should be unique.
        /// </summary>
        private int _indexNumber;

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Internal Width.
        /// </summary>
        private int _width;

        /// <summary>
        /// Internal Width Minimum.
        /// </summary>
        private int _widthMin;

        /// <summary>
        /// Internal Width Maximum.
        /// </summary>
        private int _widthMax;

        /// <summary>
        /// Internal Height.
        /// </summary>
        private int _height;

        /// <summary>
        /// Internal Height Minimum.
        /// </summary>
        private int _heightMin;

        /// <summary>
        /// Internal Height Maximum.
        /// </summary>
        private int _heightMax;

        #endregion

        #region Properties

        /// <summary>
        /// UI Base Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// UI Base Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent Position.
        /// Set if this UIBase is a child of another class.
        /// Combined with Position in Update.
        /// Default is Vector.Zero.
        /// </summary>
        public Vector2 ParentPosition { get; set; } = Vector2.Zero;

        /// <summary>
        /// Index Number.
        /// Identifies the UIBase and should be unique.
        /// </summary>
        public int IndexNumber
        {
            get => _indexNumber;
            protected set => _indexNumber = value > 0 ? value : 1;
        }

        /// <summary>
        /// Order Number.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value > 0 ? value : 1;
        }

        /// <summary>
        /// UI Width.
        /// </summary>
        public int Width
        {
            get => _width;
            set
            {
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
        /// Width Minimum.
        /// </summary>
        public int WidthMin
        {
            get => _widthMin;
            set => _widthMin = value > 0 && value <= WidthMax ? value : _widthMin;
        }

        /// <summary>
        /// Width Maximum.
        /// </summary>
        public int WidthMax
        {
            get => _widthMax;
            set => _widthMax = value >= WidthMin ? value : _widthMax;
        }

        /// <summary>
        /// UI WidthF.
        /// Used in Drawing.
        /// </summary>
        protected internal float WidthF { get; private set; }

        /// <summary>
        /// UI Height.
        /// </summary>
        public int Height
        {
            get => _height;
            set
            {
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
        /// Height Minimum.
        /// </summary>
        public int HeightMin
        {
            get => _heightMin;
            set => _heightMin = value > 0 && value <= HeightMax ? value : _heightMin;
        }

        /// <summary>
        /// Height Maximum.
        /// </summary>
        public int HeightMax
        {
            get => _heightMax;
            set => _heightMax = value >= HeightMin ? value : _heightMax;
        }

        /// <summary>
        /// UI HeightF.
        /// Used in Drawing.
        /// </summary>
        protected internal float HeightF { get; private set; }
        
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
        /// UI Scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// UI Draw Depth.
        /// </summary>
        public float DrawDepth { get; set; }

        #region Booleans

        /// <summary>
        /// UIBase Is Visible?
        /// Defines whether the UIBase is drawn or not.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// UiBase Is Active?
        /// Determines whether the UIBase can be interacted with or not.
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Is In Focus?
        /// Determines whether the UIBase has focus or not.
        /// </summary>
        public bool IsInFocus { get; private set; }

        /// <summary>
        /// UIBase Is Movable?
        /// Determines whether the UIBase is movable or not.
        /// </summary>
        protected bool IsMovable { get; set; } = true;

        /// <summary>
        /// UIBase Is Moving?
        /// Determines whether the UIBase is moving or not.
        /// </summary>
        public bool IsMoving { get; set; }

        /// <summary>
        /// UIBase Is Highlighting?
        /// Determines if the UIBase is highlighting or not.
        /// </summary>
        public bool IsHighlighting { get; set; }

        /// <summary>
        /// UIBase Is Background Visible?
        /// Determines if the UIBase's background is visible or not.
        /// </summary>
        public bool IsBackgroundVisible { get; set; } = true;

        #endregion

        #endregion

        /// <summary>
        /// UIBase Default Constructor.
        /// </summary>
        /// <param name="id">The base's id. Intaken as an int.</param>
        /// <param name="name">The base's name. Intaken as a string.</param>
        /// <param name="position">The base's position. Intaken as a Vector2.</param>
        /// <param name="width">The base's width. Intaken as a float.</param>
        /// <param name="height">The base's height. Intaken as a float.</param>
        /// <param name="orderNumber">The base's order number. Intaken as an int.</param>
        protected UIBase(int id, string name, Vector2 position, int width, int height, int orderNumber)
        {
            Id = id;
            Name = name;

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
            Scale = Vector2.One;
            RotationAngle = 0f;
            DrawDepth = 1f;

            // Outlines
            Outlines = new List<UIBaseOutline>(4)
            {
                new UIBaseOutline(this, 1, "Top", 1, Colors["Outline"]),
                new UIBaseOutline(this, 2, "Right", 1, Colors["Outline"]),
                new UIBaseOutline(this, 3, "Bottom", 1, Colors["Outline"]),
                new UIBaseOutline(this, 4, "Left", 1, Colors["Outline"])
            };

            // Defaults
            Defaults = new UIDefaults<UIBase>(this);
        }

        /// <summary>
        /// Set Outline Visiblity.
        /// </summary>
        /// <param name="top">A boolean indicating whether the top outline is visible.</param>
        /// <param name="right">A boolean indicating whether the right outline is visible.</param>
        /// <param name="bottom">A boolean indicating whether the bottom outline is visible.</param>
        /// <param name="left">A boolean indicating whether the left outline is visible.</param>
        public void SetOutlineVisibility(bool top, bool right, bool bottom, bool left)
        {
            GetItemById(Outlines, 1).IsVisible = top;
            GetItemById(Outlines, 2).IsVisible = right;
            GetItemById(Outlines, 3).IsVisible = bottom;
            GetItemById(Outlines, 4).IsVisible = left;
        }

        /// <summary>
        /// Enables/Disables and sets the UI's highlight color and transparency level.
        /// </summary>
        /// <param name="enableHighlighting">The UI's ability to highlight. Intaken as a bool.</param>
        /// <param name="color">The UI's highlight color. Intaken as a Color. Default id Color.AliceBlue.</param>
        /// <param name="transparencyLevel">The UI's highlight transparency level. Intaken as a float. Default is 0.5f</param>
        public void SetHighlight(bool enableHighlighting, Color? color, float transparencyLevel = 0.5f)
        {
            IsHighlighting = enableHighlighting;
            Colors["Highlight"] = color ?? Color.AliceBlue;
            Transparencies["Highlight"] = MathHelper.Clamp(transparencyLevel, 0f, 1f);
        }

        /// <summary>
        /// Check Is In Focus.
        /// Pass in a rectangle to check if it intersects with or is inside this object's Rectangle.
        /// IsInFocus is updated in this call.
        /// </summary>
        /// <param name="rectangle">The rectangle to check against.</param>
        /// <returns>Returns a boolean indicating whether the UIBase is in focus.</returns>
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
        /// <returns>Returns a boolean indicating whether the UIBase is in focus.</returns>
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
        /// Check Is In Focus.
        /// Pass in a condition that will determine if the object is in focus.
        /// IsInFocus is updated in this call.
        /// </summary>
        /// <param name="condition">The condition result.</param>
        /// <returns>Returns a boolean indicating whether the UIBase is in focus.</returns>
        public bool CheckIsInFocus(bool condition)
        {
            return IsInFocus = condition;
        }

        /// <summary>
        /// Create Texture2D.
        /// </summary>
        protected internal Texture2D CreateTexture2D()
        {
            var texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            return texture;
        }

        /// <summary>
        /// UIBase Load Content Method.
        /// </summary>
        public virtual void LoadContent()
        {
            Textures.Add("Background", CreateTexture2D());
            Textures.Add("Highlight", CreateTexture2D());
        }

        /// <summary>
        /// UIBase Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public virtual async Task Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            Origin = new Vector2((Width * Scale.X) / 2f, (Height * Scale.Y) / 2f);

            WidthF = Width * Scale.X;
            HeightF = Height * Scale.Y;

            Rectangle = new Rectangle((int)(ParentPosition.X + Position.X - Origin.X),
                                      (int)(ParentPosition.Y + Position.Y - Origin.Y),
                                      (int)WidthF,
                                      (int)HeightF);

            await UIEffectsManager.RunActiveEffects();

            if (IsVisible)
            {
                if (IsInFocus)
                {
                    //If in focus, scale out else scale in.
                    if (EnableScaleOutOnSelection)
                    {
                        ScaleOut();
                    }
                    else if (EnableScaleInOnSelection)
                    {
                        ScaleIn();
                    }
                    
                    if (EnableShiftUpOnSelection)
                    {
                        ShiftUp();
                    }

                    if (EnableShiftRightOnSelection)
                    {
                        ShiftRight();
                    }

                    if (EnableShiftDownOnSelection)
                    {
                        ShiftDown();
                    }

                    if (EnableShiftLeftOnSelection)
                    {
                        ShiftLeft();
                    }
                }
                else
                {
                    //If not in focus, scale out else scale in.
                    if (EnableScaleOutOnSelection)
                    {
                        ScaleIn();
                    }
                    else if (EnableScaleInOnSelection)
                    {
                        ScaleOut();
                    }

                    if (EnableShiftUpOnSelection)
                    {
                        if (Position.Y < Defaults.Position.Y)
                        {
                            ShiftDown();
                        }
                    }

                    if (EnableShiftRightOnSelection)
                    {
                        if (Position.X > Defaults.Position.X)
                        {
                            ShiftLeft();
                        }
                    }

                    if (EnableShiftDownOnSelection)
                    {
                        if (Position.Y > Defaults.Position.Y)
                        {
                            ShiftUp();
                        }
                    }

                    if (EnableShiftLeftOnSelection)
                    {
                        if (Position.X < Defaults.Position.X)
                        {
                            ShiftRight();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// UIBase Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                if (IsBackgroundVisible)
                {
                    // Draw Base.
                    spriteBatch.Draw(Textures["Background"], new Vector2(Rectangle.X, Rectangle.Y), null, Colors["Background"] * Transparencies["Background"], RotationAngle, Vector2.Zero, new Vector2(WidthF, HeightF), SpriteEffects.None, DrawDepth);
                }

                // If Highlighting is enabled and base has focus.
                if (IsHighlighting &&
                    IsInFocus)
                {
                    // Highlight.
                    spriteBatch.Draw(Textures["Highlight"], new Vector2(Rectangle.X, Rectangle.Y), null, Colors["Highlight"] * Transparencies["Highlight"], RotationAngle, Vector2.Zero, new Vector2(WidthF, HeightF), SpriteEffects.None, DrawDepth);
                }

                #region Outlines

                var outline = GetItemById(Outlines, 1);

                if (outline.IsVisible)
                {
                    spriteBatch.Draw(outline.Texture, new Vector2(Rectangle.X - outline.Thickness * 2, Rectangle.Y - outline.Thickness * 2), null,
                                     outline.Color * outline.Transparency, RotationAngle, Vector2.Zero, new Vector2(WidthF + outline.Thickness * 4, outline.Thickness * 2), SpriteEffects.None, DrawDepth);
                }

                outline = GetItemById(Outlines, 2);

                if (outline.IsVisible)
                {
                    spriteBatch.Draw(outline.Texture, new Vector2(Rectangle.X - outline.Thickness * 2, Rectangle.Y - outline.Thickness * 2), null,
                                     outline.Color * outline.Transparency, RotationAngle, Vector2.Zero, new Vector2(outline.Thickness * 2, HeightF + outline.Thickness * 4), SpriteEffects.None, DrawDepth);
                }
                outline = GetItemById(Outlines, 3);

                if (outline.IsVisible)
                {
                    spriteBatch.Draw(outline.Texture, new Vector2(Rectangle.X - outline.Thickness * 2, Rectangle.Y + Rectangle.Height), null,
                                     outline.Color * outline.Transparency, RotationAngle, Vector2.Zero, new Vector2(WidthF + outline.Thickness * 4, outline.Thickness * 2), SpriteEffects.None, DrawDepth);
                }
                outline = GetItemById(Outlines, 4);

                if (outline.IsVisible)
                {
                    spriteBatch.Draw(outline.Texture, new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y - outline.Thickness * 2), null,
                                     outline.Color * outline.Transparency, RotationAngle, Vector2.Zero, new Vector2(outline.Thickness * 2, HeightF + outline.Thickness * 4), SpriteEffects.None, DrawDepth);
                }

                #endregion
            }
        }
    }
}