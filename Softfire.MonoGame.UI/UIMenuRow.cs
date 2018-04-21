using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UIMenuRow : UIBase
    {
        /// <summary>
        /// Parent Group.
        /// </summary>
        internal UIGroup ParentGroup { private get; set; }

        /// <summary>
        /// Parent Menu.
        /// </summary>
        internal UIMenu ParentMenu { private get; set; }

        /// <summary>
        /// Parent Column.
        /// </summary>
        internal UIMenuColumn ParentColumn { private get; set; }

        /// <summary>
        /// Row Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Menu Item.
        /// </summary>
        public UIMenuItem MenuItem { get; private set; }

        /// <summary>
        /// UI Menu Row Constructor.
        /// </summary>
        /// <param name="number">The row's number. Intaken as an int.</param>
        /// <param name="isVisible">Indicates whether the row is visible. Intaken as a bool.</param>
        public UIMenuRow(int number, bool isVisible = false) : base(new Vector2(), 0, 0, number, isVisible: isVisible)
        {
            Number = number;
        }

        /// <summary>
        /// Add Menu Item.
        /// </summary>
        /// <param name="menuItem">The menu item to add. Intaken as a UIMenuItem.</param>
        /// <returns>Returns a bool indicating whether the menu item was added.</returns>
        public bool AddMenuItem(UIMenuItem menuItem)
        {
            var result = false;

            if (menuItem != null)
            {
                menuItem.ParentGroup = ParentGroup;
                menuItem.ParentMenu = ParentMenu;
                menuItem.ParentColumn = ParentColumn;
                menuItem.ParentRow = this;
                menuItem.LoadContent();

                MenuItem = menuItem;

                result = true;
            }

            return result;
        }

        /// <summary>
        /// UI Menu Row Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            ParentPosition = ParentColumn.ParentPosition + ParentColumn.Position;
            Transparency = ParentColumn.Transparency;

            if (MenuItem != null)
            {
                await MenuItem.Update(gameTime);
            }

            await base.Update(gameTime);
        }

        /// <summary>
        /// UI Menu Row Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsVisible)
            {
                MenuItem?.Draw(spriteBatch);
            }
        }
    }
}