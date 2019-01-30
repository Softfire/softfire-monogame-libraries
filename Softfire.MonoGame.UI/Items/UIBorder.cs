using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Items
{
    /// <summary>
    /// A UI border class.
    /// </summary>
    public class UIBorder : UIBase
    {
        /// <summary>
        /// UI Border Constructor.
        /// </summary>
        /// <param name="parent">The parent object. Intaken as a <see cref="UIBase"/>.</param>
        /// <param name="id">The border's id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The border's name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The border's position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The border's width. Intaken as an <see cref="int"/>.</param>
        /// <param name="height">The border's height. Intaken as an <see cref="int"/>.</param>
        public UIBorder(UIBase parent, int id, string name, Vector2 position, int width, int height) : base(parent, id, name, position, width, height)
        {
        }
    }
}