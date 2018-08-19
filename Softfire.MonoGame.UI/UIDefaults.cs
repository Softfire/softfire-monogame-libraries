using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    /// <summary>
    /// UI Defaults.
    /// Stored attributes.
    /// </summary>
    public class UIDefaults<T> where T : UIBase
    {
        /// <summary>
        /// Parent UIBase.
        /// </summary>
        private T ParentUIObject { get; }

        /// <summary>
        /// Texts.
        /// </summary>
        public Dictionary<int, string> Texts { get; } = new Dictionary<int, string>();

        /// <summary>
        /// Default Colors.
        /// </summary>
        public Dictionary<string, Color> Colors { get; } = new Dictionary<string, Color>();
        
        /// <summary>
        /// Default Transparencies.
        /// </summary>
        public Dictionary<string, float> Transparencies { get; } = new Dictionary<string, float>
        {
            { "Background", 1f },
            { "Highlight", 0.25f }
        };

        /// <summary>
        /// Default Outlines.
        /// </summary>
        public List<UIBaseOutline> Outlines { get; }

        /// <summary>
        /// Index.
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// Order Number.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Is Active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Is Visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Parent Position.
        /// </summary>
        public Vector2 ParentPosition { get; set; }

        /// <summary>
        /// Position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Rotation Angle.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// Draw Depth.
        /// </summary>
        public float DrawDepth { get; set; }

        /// <summary>
        /// Font.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Font Color.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Selection Color.
        /// </summary>
        public Color SelectionColor { get; set; }

        /// <summary>
        /// Font Transparency.
        /// </summary>
        public float FontTransparency { get; set; }

        /// <summary>
        /// Selection Transparency.
        /// </summary>
        public float SelectionTransparency { get; set; }

        /// <summary>
        /// UI Defaults Constructor.
        /// </summary>
        public UIDefaults(T parentUIObject)
        {
            ParentUIObject = parentUIObject;

            Index = ParentUIObject.IndexNumber;
            OrderNumber = ParentUIObject.OrderNumber;
            IsActive = ParentUIObject.IsActive;
            IsVisible = ParentUIObject.IsVisible;
            ParentPosition = ParentUIObject.ParentPosition;
            Position = ParentUIObject.Position;
            Width = ParentUIObject.Width;
            Height = ParentUIObject.Height;
            Scale = ParentUIObject.Scale;
            RotationAngle = ParentUIObject.RotationAngle;
            DrawDepth = ParentUIObject.DrawDepth;

            Outlines = new List<UIBaseOutline>(4)
            {
                new UIBaseOutline(ParentUIObject, 1, "Top", UIBase.GetItemById(ParentUIObject.Outlines, 1).Thickness, UIBase.GetItemById(ParentUIObject.Outlines, 1).Color),
                new UIBaseOutline(ParentUIObject, 2, "Right", UIBase.GetItemById(ParentUIObject.Outlines, 2).Thickness, UIBase.GetItemById(ParentUIObject.Outlines, 2).Color) ,
                new UIBaseOutline(ParentUIObject, 3, "Bottom", UIBase.GetItemById(ParentUIObject.Outlines, 3).Thickness, UIBase.GetItemById(ParentUIObject.Outlines, 3).Color) ,
                new UIBaseOutline(ParentUIObject, 4, "Left", UIBase.GetItemById(ParentUIObject.Outlines, 4).Thickness, UIBase.GetItemById(ParentUIObject.Outlines, 4).Color)
            };
        }

        /// <summary>
        /// Reset UI Visibility.
        /// </summary>
        public void ResetVisibility()
        {
            ParentUIObject.IsVisible = IsVisible;
        }

        /// <summary>
        /// Reset UI Position.
        /// </summary>
        public void ResetPosition()
        {
            ParentUIObject.Position = Position;
        }

        /// <summary>
        /// Reset UI Width.
        /// </summary>
        public void ResetWidth()
        {
            ParentUIObject.Width = Width;
        }

        /// <summary>
        /// Reset UI Height.
        /// </summary>
        public void ResetHeight()
        {
            ParentUIObject.Height = Height;
        }

        /// <summary>
        /// Reset UI Background Color.
        /// </summary>
        public void ResetColors()
        {
            ParentUIObject.Colors["Background"] = Colors["Background"];
            ParentUIObject.Colors["Highlight"] = Colors["Highlight"];
        }

        /// <summary>
        /// Reset UI Transparency.
        /// </summary>
        public void ResetTransparencies()
        {
            ParentUIObject.Transparencies["Background"] = Transparencies["Background"];
            ParentUIObject.Transparencies["Highlight"] = Transparencies["Highlight"];
        }

        /// <summary>
        /// Reset UI Draw Depth.
        /// </summary>
        public void ResetDrawDepth()
        {
            ParentUIObject.DrawDepth = DrawDepth;
        }

        /// <summary>
        /// Reset UI Order Number.
        /// </summary>
        public void ResetOrderNumber()
        {
            ParentUIObject.OrderNumber = OrderNumber;
        }

        /// <summary>
        /// Reset UI Outlines.
        /// </summary>
        public void ResetOutlines()
        {
            foreach (var parentOutline in ParentUIObject.Outlines)
            {
                foreach (var defaultOutline in Outlines)
                {
                    parentOutline.IsVisible = defaultOutline.IsVisible;
                    parentOutline.Color = defaultOutline.Color;
                    parentOutline.Thickness = defaultOutline.Thickness;
                    parentOutline.Transparency = defaultOutline.Transparency;
                }
            }
        }

        /// <summary>
        /// Reset UI Scale.
        /// </summary>
        public void ResetScale()
        {
            ParentUIObject.Scale = Scale;
        }

        /// <summary>
        /// Reset UI Rotation Angle.
        /// </summary>
        public void ResetRotationAngle()
        {
            ParentUIObject.RotationAngle = RotationAngle;
        }
    }
}