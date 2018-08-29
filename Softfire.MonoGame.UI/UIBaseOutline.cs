using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UIBaseOutline : IUIIdentifier
    {
        /// <summary>
        /// Parent UI Base.
        /// </summary>
        private UIBase ParentUIBase { get; }

        /// <summary>
        /// Internal Order Number.
        /// </summary>
        private int _orderNumber;

        /// <summary>
        /// Internal Thickness.
        /// </summary>
        private int _thickness;

        /// <summary>
        /// UI Base Outline Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// UI Base Outline Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Order Number.
        /// </summary>
        public int OrderNumber
        {
            get => _orderNumber;
            set => _orderNumber = value > 0 ? value : 1;
        }

        /// <summary>
        /// Is Visible?
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Outline Thickness.
        /// </summary>
        public int Thickness
        {
            get => _thickness;
            set => _thickness = value > 0 ? value : 1;
        }

        /// <summary>
        /// Outline Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Outline Transparency.
        /// </summary>
        public float Transparency { get; set; }

        /// <summary>
        /// Outline Texture.
        /// </summary>
        internal Texture2D Texture { get; }

        /// <summary>
        /// UIBase Outline Defualt Constructor.
        /// </summary>
        /// <param name="parentUIBase">The parent UIBase.</param>
        /// <param name="id">The outline's id. Intaken as an int.</param>
        /// <param name="name">The outline's name.</param>
        /// <param name="thickness">The outline's thickness. Intaken as an int.</param>
        /// <param name="color">The outline's color.</param>
        /// <param name="transparency">The outline's transparency level. Intaken as a float.</param>
        public UIBaseOutline(UIBase parentUIBase, int id, string name, int thickness = 1, Color? color = null, float transparency = 1.0f)
        {
            ParentUIBase = parentUIBase;
            Id = id;
            Name = name;
            Thickness = thickness;
            Color = color ?? Color.Black;
            Transparency = transparency;

            Texture = ParentUIBase.CreateTexture2D();
        }
    }
}