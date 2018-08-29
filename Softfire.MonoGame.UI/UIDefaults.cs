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
        public Dictionary<string, Color> Colors { get; }
        
        /// <summary>
        /// Default Transparencies.
        /// </summary>
        public Dictionary<string, float> Transparencies { get; }

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
        /// UI Defaults for restoration.
        /// </summary>
        /// <remarks>Adds the ability to restore a UI back to it's creation state.</remarks>
        /// <see cref="ResetColors()"/>
        /// <see cref="ResetDrawDepth()"/>
        /// <see cref="ResetFont()"/>
        /// <see cref="ResetHeight()"/>
        /// <see cref="ResetOrderNumber()"/>
        /// <see cref="ResetOutlines()"/>
        /// <see cref="ResetPosition()"/>
        /// <see cref="ResetRotationAngle()"/>
        /// <see cref="ResetScale()"/>
        /// <see cref="ResetTransparencies()"/>
        /// <see cref="ResetVisibility()"/>
        /// <see cref="ResetWidth()"/>
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

            Colors = new Dictionary<string, Color>(6)
            {
                { "Background", ParentUIObject.Colors["Background"] },
                { "Highlight", ParentUIObject.Colors["Highlight"] },
                { "Outline", ParentUIObject.Colors["Outline"] },
                { "Font", ParentUIObject.Colors["Font"] },
                { "FontHighlight", ParentUIObject.Colors["FontHighlight"] },
                { "Selection", ParentUIObject.Colors["Selection"] }
            };

            Transparencies = new Dictionary<string, float>(6)
            {
                { "Background", ParentUIObject.Transparencies["Background"] },
                { "Highlight", ParentUIObject.Transparencies["Highlight"] },
                { "Outline", ParentUIObject.Transparencies["Outline"] },
                { "Font", ParentUIObject.Transparencies["Font"] },
                { "FontHighlight", ParentUIObject.Transparencies["FontHighlight"] },
                { "Selection", ParentUIObject.Transparencies["Selection"] }
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
            ParentUIObject.Colors["Outline"] = Colors["Outline"];
            ParentUIObject.Colors["Font"] = Colors["Font"];
            ParentUIObject.Colors["FontHighlight"] = Colors["FontHighlight"];
            ParentUIObject.Colors["Selection"] = Colors["Selection"];
        }

        /// <summary>
        /// Reset UI Transparency.
        /// </summary>
        public void ResetTransparencies()
        {
            ParentUIObject.Transparencies["Background"] = Transparencies["Background"];
            ParentUIObject.Transparencies["Highlight"] = Transparencies["Highlight"];
            ParentUIObject.Transparencies["Outline"] = Transparencies["Outline"];
            ParentUIObject.Transparencies["Font"] = Transparencies["Font"];
            ParentUIObject.Transparencies["FontHighlight"] = Transparencies["FontHighlight"];
            ParentUIObject.Transparencies["Selection"] = Transparencies["Selection"];
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

        /// <summary>
        /// Reset UI Font.
        /// </summary>
        public void ResetFont()
        {
            ParentUIObject.Font = Font;
        }
    }
}