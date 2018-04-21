using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI
{
    public class UIMenuColumn : UIBase
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
        /// Column Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Rows.
        /// </summary>
        public Dictionary<int, UIMenuRow> Rows { get; }

        /// <summary>
        /// Row Sorting Method.
        /// </summary>
        public RowSortingMethods RowSortingMethod { get; set; }

        /// <summary>
        /// Row Sorting Methods.
        /// </summary>
        public enum RowSortingMethods
        {
            Ascending,
            Descending,
        }

        /// <summary>
        /// UI Menu Column Constructor.
        /// </summary>
        /// <param name="number">The column's number. Intaken as an int.</param>
        /// <param name="rows">The number of rows in the column. Intaken as an int.</param>
        /// <param name="rowAlignment">The row alignment.</param>
        /// <param name="rowSortingMethod">The row sorting method.</param>
        /// <param name="isVisible">Indicates whether the column and rows are visible. Intaken as a bool.</param>
        public UIMenuColumn(int number, int rows, RowSortingMethods rowSortingMethod = RowSortingMethods.Ascending, bool isVisible = false) : base(new Vector2(), 0, 0, number, isVisible: isVisible)
        {
            Number = number;
            RowSortingMethod = rowSortingMethod;
            
            Rows = new Dictionary<int, UIMenuRow>(rows);
        }

        /// <summary>
        /// Add Row.
        /// Adds a new row on to the end.
        /// </summary>
        public void AddRow()
        {
            var nextRowNumber = Rows.Count + 1;
            var newRow = new UIMenuRow(nextRowNumber, true)
            {
                ParentGroup = ParentGroup,
                ParentMenu = ParentMenu,
                ParentColumn = this
            };
            newRow.LoadContent();

            Rows.Add(nextRowNumber, newRow);
        }

        /// <summary>
        /// Calculate Row Positions.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        private async Task CalculateRowPositions(GameTime gameTime)
        {
            for (var rowIndex = 1; rowIndex <= Rows.Count; rowIndex++)
            {
                var row = Rows[rowIndex];

                if (row != null)
                {
                    Vector2 startPosition;
                    Vector2 rowOffset;

                    switch (RowSortingMethod)
                    {
                        case RowSortingMethods.Ascending:
                            startPosition = new Vector2(0, -(Height / 2f) + row.Height / 2f);
                            rowOffset = new Vector2(0, Rows.Where(r => r.Value.Number < rowIndex).Sum(r => r.Value.Rectangle.Height));

                            break;
                        case RowSortingMethods.Descending:
                            startPosition = new Vector2(0, (Height / 2f) - row.Height / 2f);
                            rowOffset = new Vector2(0, -(Rows.Where(r => r.Value.Number < rowIndex).Sum(r => r.Value.Rectangle.Height)));

                            break;
                        default:
                            startPosition = Vector2.Zero;
                            rowOffset = Vector2.Zero;

                            break;
                    }

                    row.Position = startPosition + rowOffset;

                    if (row.IsVisible)
                    {
                        // Check if any input devices are over top of any of the columns.
                        for (var index = 0; index < ParentGroup.ActiveInputDevices.Count; index++)
                        {
                            var inputDevice = ParentGroup.ActiveInputDevices[index];
                            row.CheckIsInFocus(new Rectangle((int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                             (int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                             inputDevice.Width,
                                                             inputDevice.Height));
                        }
                    }

                    await row.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// UI Menu Column Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            ParentPosition = ParentMenu.ParentPosition + ParentMenu.Position;
            Transparency = ParentMenu.Transparency;

            await CalculateRowPositions(gameTime);
            
            await base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsVisible)
            {
                switch (RowSortingMethod)
                {
                    case RowSortingMethods.Ascending:
                        foreach (var row in Rows.OrderBy(row => row.Value.Number))
                        {
                            row.Value.Draw(spriteBatch);
                        }

                        break;
                    case RowSortingMethods.Descending:
                        foreach (var row in Rows.OrderByDescending(row => row.Value.Number))
                        {
                            row.Value.Draw(spriteBatch);
                        }

                        break;
                }
            }
        }
    }
}