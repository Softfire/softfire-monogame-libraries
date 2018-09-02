using Microsoft.Xna.Framework;

namespace Softfire.MonoGame.UI.Items
{
    public class UIBorder : UIBase
    {
        /// <summary>
        /// UI Border Constructor.
        /// </summary>
        /// <param name="id">The border's id. Intaken as an int.</param>
        /// <param name="name">The border's name. Intaken as a string.</param>
        /// <param name="position">The border's position. Intaken as a Vector2.</param>
        /// <param name="width">The border's width. Intaken as an int.</param>
        /// <param name="height">The border's height. Intaken as an int.</param>
        /// <param name="orderNumber">The border's order number. Intaken as an int.</param>
        public UIBorder(int id, string name, Vector2 position, int width, int height, int orderNumber) : base(id, name, position, width, height, orderNumber)
        {

        }
    }
}