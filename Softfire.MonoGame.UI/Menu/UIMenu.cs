using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Menu
{
    public class UIMenu : UIBase
    {
        /// <summary>
        /// Parent Group.
        /// </summary>
        internal UIGroup ParentGroup { get; set; }

        /// <summary>
        /// Content Sprite Batch.
        /// </summary>
        private SpriteBatch ContentSpriteBatch { get; set; }

        /// <summary>
        /// ViewPort.
        /// </summary>
        private Viewport ViewPort => new Viewport(Rectangle);

        /// <summary>
        /// Columns.
        /// </summary>
        internal List<UIMenuColumn> Columns { get; } = new List<UIMenuColumn>();

        /// <summary>
        /// Scrollable Area Rectangle.
        /// A Rectangle defining the boundaries of the entire menu that is scrollable.
        /// </summary>
        private Rectangle ScrollableAreaRectangle { get; set; }

        /// <summary>
        /// UI Menu.
        /// </summary>
        /// <param name="parentGroup">The parent group containing the menu. Intaken as a UIGroup.</param>
        /// <param name="id">The menu's id. Intaken as an int.</param>
        /// <param name="name">The menu's name. Intaken as a string.</param>
        /// <param name="position">The menu's position. Intaken as a Vector2.</param>
        /// <param name="width">The menu's width. Intaken as an int.</param>
        /// <param name="height">The menu's height. Intaken as an int.</param>
        /// <param name="orderNumber">The menu order number. Used in sorting. Intaken as an int.</param>
        public UIMenu(UIGroup parentGroup, int id, string name, Vector2 position, int width, int height, int orderNumber) : base(id, name, position, width, height, orderNumber)
        {
            ParentGroup = parentGroup;
        }

        #region Columns

        /// <summary>
        /// Adds a column to the menu.
        /// </summary>
        /// <param name="columnName">The column's name. Intaken as a string.</param>
        /// <returns>Returns the column id, if added, otherwise zero.</returns>
        /// <remarks>If a column already exists with the provided name then a zero is returned indicating failure to add the column.</remarks>
        public int AddColumn(string columnName)
        {
            var nextColumnId = 0;

            if (CheckForColumn(columnName) == false)
            {
                nextColumnId = GetNextValidItemId(Columns);

                if (CheckForColumn(nextColumnId) == false)
                {
                    var newColumn = new UIMenuColumn(this, nextColumnId, columnName, Columns.Count == 0 ? Width : Width / Columns.Count, Height, nextColumnId);
                    newColumn.LoadContent();

                    Columns.Add(newColumn);
                }
            }

            return nextColumnId;
        }

        /// <summary>
        /// Checks for a column by id.
        /// </summary>
        /// <param name="columnId">The id of the column to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the column is present.</returns>
        public bool CheckForColumn(int columnId)
        {
            return CheckItemById(Columns, columnId);
        }

        /// <summary>
        /// Checks for a column by name.
        /// </summary>
        /// <param name="columnName">The name of the column to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the column is present.</returns>
        public bool CheckForColumn(string columnName)
        {
            return CheckItemByName(Columns, columnName);
        }

        /// <summary>
        /// Gets a column by id.
        /// </summary>
        /// <param name="columnId">The id of the column to retrieve. Intaken as an int.</param>
        /// <returns>Returns the column with the specified id, if present, otherwise null.</returns>
        public UIMenuColumn GetColumn(int columnId)
        {
            return CheckForColumn(columnId) ? GetItemById(Columns, columnId) : default(UIMenuColumn);
        }

        /// <summary>
        /// Gets a column by name.
        /// </summary>
        /// <param name="columnName">The name of the column to retrieve. Intaken as a string.</param>
        /// <returns>Returns the column with the specified name, if present, otherwise null.</returns>
        public UIMenuColumn GetColumn(string columnName)
        {
            return CheckForColumn(columnName) ? GetItemByName(Columns, columnName) : default(UIMenuColumn);
        }

        /// <summary>
        /// Removes a column by id..
        /// </summary>
        /// <param name="columnId">The id of the column to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the column was removed.</returns>
        public bool RemoveColumn(int columnId)
        {
            return RemoveItemById(Columns, columnId);
        }
        
        /// <summary>
        /// Removes a column by name.
        /// </summary>
        /// <param name="columnName">The name of the column to be removed. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the column was removed.</returns>
        public bool RemoveColumn(string columnName)
        {
            return RemoveItemByName(Columns, columnName);
        }

        #endregion

        /// <summary>
        /// UIMenu Load Content Method.
        /// </summary>
        public override void LoadContent()
        {
            ContentSpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        /// <summary>
        /// UIMenu Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes a GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            await base.Update(gameTime);

            //ScrollableAreaRectangle = new Rectangle(ViewPort.X,
            //                                        ViewPort.Y,
            //                                        ScrollableAreaRectangle.Width < ViewPort.Width ? ViewPort.Width : Columns.Sum(column => column.Rectangle.Width),
            //                                        ScrollableAreaRectangle.Height < ViewPort.Height ? ViewPort.Height : Columns.Max(column => column.Rectangle.Height));

            foreach (var column in Columns.OrderBy(column => column.OrderNumber))
            {
                await column.Update(gameTime);
            }
        }

        /// <summary>
        /// UIMenu Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsVisible)
            {
                foreach (var column in Columns.OrderBy(column => column.OrderNumber))
                {
                    column.Draw(spriteBatch);
                }
            }
        }
    }
}