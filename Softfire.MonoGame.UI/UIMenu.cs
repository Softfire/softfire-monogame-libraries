using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.UI
{
    public class UIMenu : UIBase
    {
        /// <summary>
        /// Logger.
        /// </summary>
        public Logger Logger { get; }

        /// <summary>
        /// Parent Group.
        /// </summary>
        internal UIGroup ParentGroup { private get; set; }

        /// <summary>
        /// Active Menu Index Number.
        /// Currently selected Menu Index Number.
        /// </summary>
        public int ActiveMenuIndexNumber { get; set; }

        /// <summary>
        /// Column Alignment.
        /// </summary>
        public ColumnAlignments ColumnAlignment { get; set; }

        /// <summary>
        /// Column Alignments.
        /// </summary>
        public enum ColumnAlignments
        {
            LeftToRight,
            RightToLeft
        }
        
        /// <summary>
        /// Columns.
        /// </summary>
        public Dictionary<int, UIMenuColumn> Columns { get; }

        /// <summary>
        /// UI Menu Constructor.
        /// </summary>
        /// <param name="position">The menu's position. Position is located the center of the menu. Intaken as a Vector2.</param>
        /// <param name="orderNumber">The menu's order number. Depending on orientation and direction the menu's will be drawn in order based on their order numbers. Intaken as an int.</param>
        /// <param name="orientation">The menu's orientation. Either Ascending or Descending. Rows will be ordered based on this option.</param>
        /// <param name="rowAlignment">The menu's direction. Either LTR (Left-to-right) or RTL (Right-to-Left). Columns' draw order will be based on this option.</param>
        /// <param name="columnAlignment">The menu's alignment. Rows will aligned to either side or center based on the option.</param>
        /// <param name="backgroundColor">The menu's background color. Intaken as a Color.</param>
        /// <param name="highlightColor">The menu's highlight color. If provided then highlighting will be available when in focus. Intaken as a Color.</param>
        /// <param name="outlineColor">The menu;s outline color. Intaken as a Color.</param>
        /// <param name="textureFilePath">The texture path for a texture that will be used as a background. Relative to the Content root path. Intaken as a string.</param>
        /// <param name="isVisible">The menu's visibility. Intaken as a bool.</param>
        public UIMenu(Vector2 position, int orderNumber,
                                        ColumnAlignments columnAlignment = ColumnAlignments.LeftToRight,
                                        Color? backgroundColor = null,
                                        Color? highlightColor = null,
                                        Color? outlineColor = null,
                                        string textureFilePath = null,
                                        bool isVisible = false) : base(position, 0, 0, orderNumber, backgroundColor, textureFilePath, isVisible)
        {
            ColumnAlignment = columnAlignment;

            HasBackground = backgroundColor != null;
            Highlight(highlightColor != null, highlightColor);
            OutlineColor = outlineColor ?? OutlineColor;
            ActivateOutlines(backgroundColor != null || outlineColor != null);

            Columns = new Dictionary<int, UIMenuColumn>();
            Logger = new Logger(@"Config\Logs\UI");
        }

        /// <summary>
        /// Hide.
        /// Hides Menu, Columns, Rows and Menu Items.
        /// </summary>
        /// <returns>Returns a bool indicating whether the Menu and it's contents were hidden.</returns>
        public bool Hide()
        {
            var result = false;

            if (IsVisible)
            {
                IsVisible = false;

                for (var colIndex = 1; colIndex <= Columns.Count; colIndex++)
                {
                    var column = GetColumn(colIndex);

                    for (var rowIndex = 1; rowIndex <= column.Rows.Count; rowIndex++)
                    {
                        var menuItem = column.Rows[rowIndex].MenuItem;

                        if (menuItem != null &&
                            menuItem.IsASpacer == false)
                        {
                            menuItem.IsVisible = false;
                        }
                    }
                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Reveal.
        /// Reveals Menu, Columns, Rows and Menu Items.
        /// </summary>
        /// <returns>Returns a bool indicating whether the Menu and it's contents were revealed.</returns>
        public bool Reveal()
        {
            var result = false;

            if (IsVisible == false)
            {
                IsVisible = true;

                for (var colIndex = 1; colIndex <= Columns.Count; colIndex++)
                {
                    var column = GetColumn(colIndex);

                    for (var rowIndex = 1; rowIndex <= column.Rows.Count; rowIndex++)
                    {
                        var menuItem = column.Rows[rowIndex].MenuItem;

                        if (menuItem != null &&
                            menuItem.IsASpacer == false)
                        {
                            menuItem.IsVisible = true;
                        }
                    }
                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add Column.
        /// Adds a new column.
        /// </summary>
        /// <param name="rows">The number of rows in the new column. Intaken as an int.</param>
        public void AddColumn(int rows)
        {
            var nextColumnNumber = Columns.Count + 1;
            var column = new UIMenuColumn(nextColumnNumber, rows, isVisible: true)
            {
                ParentGroup = ParentGroup,
                ParentMenu = this
            };
            column.LoadContent();

            for (var rowIndex = 1; rowIndex <= rows; rowIndex++)
            {
                column.AddRow();
            }

            Columns.Add(nextColumnNumber, column);
        }

        /// <summary>
        /// Get Column.
        /// </summary>
        /// <param name="column">The column to retrieve. Intaken as an int.</param>
        /// <returns>Returns a UIMenuGridColumn or null, if not found.</returns>
        public UIMenuColumn GetColumn(int column)
        {
            UIMenuColumn result = null;

            if (Columns.ContainsKey(column))
            {
                result = Columns[column];
            }

            return result;
        }

        /// <summary>
        /// Remove Column.
        /// Removes the requested column.
        /// </summary>
        /// <param name="column">The column to remove. Intaken as an int.</param>
        public void RemoveColumn(int column)
        {
            Columns.Remove(column);
        }

        /// <summary>
        /// Calculate Column Dimensions.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        private void CalculateColumnDimensions()
        {
            for (var colIndex = 1; colIndex <= Columns.Count; colIndex++)
            {
                var column = GetColumn(colIndex);

                if (column != null)
                {
                    // Reset Column Width.
                    column.Width = column.Defaults.Width;

                    for (var rowIndex = 1; rowIndex <= column.Rows.Count; rowIndex++)
                    {
                        var row = column.Rows[rowIndex];
                        var menuItem = row?.MenuItem;

                        if (menuItem != null)
                        {
                            row.Height = menuItem.MarginTop + menuItem.OutlineThickness + menuItem.PaddingTop + menuItem.Height + menuItem.PaddingBottom + menuItem.OutlineThickness + menuItem.MarginBottom;

                            // Sets Column Width to widest Row Width.
                            if (menuItem.MarginLeft + menuItem.OutlineThickness + menuItem.PaddingLeft + menuItem.Width + menuItem.PaddingRight + menuItem.OutlineThickness + menuItem.MarginRight > column.Width)
                            {
                                column.Width = menuItem.MarginLeft + menuItem.OutlineThickness + menuItem.PaddingLeft + menuItem.Width + menuItem.PaddingRight + menuItem.OutlineThickness + menuItem.MarginRight;
                            }
                        }
                    }

                    foreach (var row in column.Rows)
                    {
                        row.Value.Width = column.Rectangle.Width;
                    }

                    column.Height = column.Rows.Sum(row => row.Value.Rectangle.Height);
                }
            }
        }

        /// <summary>
        /// Calculate Column Positions.
        /// </summary>
        private async Task CalculateColumnPositions(GameTime gameTime)
        {
            for (var colIndex = 1; colIndex <= Columns.Count; colIndex++)
            {
                var column = GetColumn(colIndex);

                if (column != null)
                {
                    Vector2 startPosition;
                    Vector2 columnOffset;

                    switch (ColumnAlignment)
                    {
                        case ColumnAlignments.LeftToRight:
                            startPosition = new Vector2(-(Width / 2f) + column.Width / 2f, 0);
                            columnOffset = new Vector2(Columns.Where(c => c.Value.Number < colIndex).Sum(c => c.Value.Rectangle.Width), 0);

                            break;
                        case ColumnAlignments.RightToLeft:
                            startPosition = new Vector2((Width / 2f) - column.Width / 2f, 0);
                            columnOffset = new Vector2(-(Columns.Where(c => c.Value.Number < colIndex).Sum(c => c.Value.Rectangle.Width)), 0);
                            
                            break;
                        default:
                            startPosition = Vector2.Zero;
                            columnOffset = Vector2.Zero;

                            break;
                    }

                    column.Position = startPosition + columnOffset;

                    if (column.IsVisible)
                    {
                        // Check if any input devices are over top of any of the columns.
                        for (var index = 0; index < ParentGroup.ActiveInputDevices.Count; index++)
                        {
                            var inputDevice = ParentGroup.ActiveInputDevices[index];
                            column.CheckIsInFocus(new Rectangle((int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                                (int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                                inputDevice.Width,
                                                                inputDevice.Height));
                        }
                    }

                    await column.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// UIMenu Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes a GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            Width = Columns.Sum(column => column.Value.Rectangle.Width);
            Height = Columns.Max(row => row.Value.Rectangle.Height);

            CalculateColumnDimensions();
            await CalculateColumnPositions(gameTime);

            await base.Update(gameTime);
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
                foreach (var column in Columns.Values.OrderBy(column => column.Number))
                {
                    column.Draw(spriteBatch);
                }
            }
        }
    }
}