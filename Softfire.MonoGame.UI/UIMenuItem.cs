using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Softfire.MonoGame.IO;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.UI
{
    public class UIMenuItem : UIBase
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
        /// Parent Row.
        /// </summary>
        internal UIMenuRow ParentRow { private get; set; }

        /// <summary>
        /// Parent Menu Item.
        /// </summary>
        private UIMenuItem ParentMenuItem { get; set; }

        /// <summary>
        /// Sub Menu.
        /// </summary>
        public UIMenu SubMenu { get; private set; }

        /// <summary>
        /// Text.
        /// </summary>
        public UIText Text { get; set; }

        /// <summary>
        /// Alignment.
        /// </summary>
        public Alignments Alignment { get; set; }

        /// <summary>
        /// Alignments.
        /// </summary>
        public enum Alignments
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        /// Is Matching Text Height?
        /// </summary>
        public bool IsMatchingTextHeight { get; set; }

        /// <summary>
        /// Is Matching Text Width?
        /// </summary>
        public bool IsMatchingTextWidth { get; set; }

        /// <summary>
        /// Is A Spacer?
        /// Menu items designated to be spacers have no sub-menu items and require their width and height to be manually set.
        /// </summary>
        public bool IsASpacer { get; set; }

        /// <summary>
        /// Is Auto Sensing?
        /// Set to true to enable CheckIsInFocus presets for Menu.ActiveMenuIndexNumber and IOMouse.Rectangle.
        /// Default is false.
        /// </summary>
        public bool IsAutoSensing { get; set; }

        /// <summary>
        /// Is Actionable.
        /// Enables/Disables this MenuItem's Sync/Async actions.
        /// </summary>
        public bool IsActionable { get; set; }

        /// <summary>
        /// Menu Item Functions.
        /// Set delegate methods to process asynchronously upon activating the menu item.
        /// </summary>
        private Dictionary<string, UIMenuItemFunction> Functions { get; }

        /// <summary>
        /// Menu Item Actions.
        /// Set delegate methods to process synchronously upon activating the menu item.
        /// </summary>
        private Dictionary<string, UIMenuItemAction> Actions { get; }

        /// <summary>
        /// UI Menu Item Constructor.
        /// </summary>
        /// <param name="indexNumber">The menu item's index number. Used in selection.</param>
        /// <param name="orderNumber">The menu item's order number. Used in sorting.</param>
        /// <param name="width">The menu item's width.</param>
        /// <param name="height">The menu itme's height.</param>
        /// <param name="isAutoSizingToTextDimensions">Indicates whether the width and height is to be manually set. Intaken as a bool.</param>
        /// <param name="backgroundColor">The menu's background color. Intaken as a Color.</param>
        /// <param name="highlightColor">The menu's highlight color. If provided then highlighting will be available when in focus. Intaken as a Color.</param>
        /// <param name="outlineColor">The menu;s outline color. Intaken as a Color.</param>
        /// <param name="alignment">The menu item's alignment. The menu item will aligned to either side or center based on the option.</param>
        /// <param name="textureFilePath">The texture path for a texture that will be used as a background. Relative to the Content root path. Intaken as a string.</param>
        /// <param name="isASpacer">Indicates whether the menu item will act a spacer. Menu items designated to be spacers have no sub-menu items and require their width and height to be manually set. Intaken as a bool.</param>
        /// <param name="isVisible">The menu's visibility. Intaken as a bool.</param>
        public UIMenuItem(int indexNumber,
                          int orderNumber,
                          int width = 100,
                          int height = 40,
                          Color? backgroundColor = null,
                          Color? highlightColor = null,
                          Color? outlineColor = null,
                          Alignments alignment = Alignments.Center,
                          string textureFilePath = null,
                          bool isASpacer = false,
                          bool isVisible = false) : base(new Vector2(), width, height, orderNumber,
                                                                                       backgroundColor,
                                                                                       textureFilePath,
                                                                                       isVisible)
        {
            IndexNumber = indexNumber;
            IsASpacer = isASpacer;
            HasBackground = backgroundColor != null;
            Highlight(highlightColor != null, highlightColor);
            OutlineColor = outlineColor ?? OutlineColor;
            Alignment = alignment;
            ActivateOutlines(backgroundColor != null || outlineColor != null);

            Functions = new Dictionary<string, UIMenuItemFunction>();
            Actions = new Dictionary<string, UIMenuItemAction>();

            IsAutoSensing = false;
            IsActionable = true;
            IsMatchingTextHeight = false;
            IsMatchingTextWidth = false;
        }

        /// <summary>
        /// Set Menu Item Alignment.
        /// </summary>
        private Vector2 SetMenuItemAlignment()
        {
            Vector2 position;

            switch (Alignment)
            {
                case Alignments.Left:
                    position = new Vector2(-(ParentRow.Width / 2f) + MarginLeft + OutlineThickness + PaddingLeft + (Width / 2f), 0);

                    break;
                case Alignments.Center:
                    position = Vector2.Zero;

                    break;
                case Alignments.Right:
                    position = new Vector2((ParentRow.Width / 2f) - MarginRight - OutlineThickness - PaddingRight - (Width / 2f), 0);

                    break;
                default:
                    position = Vector2.Zero;

                    break;
            }

            return position;
        }

        #region Sub Menu

        /// <summary>
        /// Add Sub Menu.
        /// Adds a new Sub Menu.
        /// </summary>
        /// <returns>Returns a boolean indicating if the Sub Menu was added.</returns>
        public bool AddSubMenu()
        {
            var result = false;

            if (SubMenu == null)
            {
                SubMenu = new UIMenu(Vector2.Zero, 1)
                {
                    ParentGroup = ParentGroup
                };

                SubMenu.LoadContent();

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Sub Menu.
        /// </summary>
        /// <returns>Returns the Sub Menu, if present, otherwise null.</returns>
        public UIMenu GetSubMenu()
        {
            UIMenu result = null;

            if (SubMenu != null)
            {
                result = SubMenu;
            }

            return result;
        }

        /// <summary>
        /// Remove Sub Menu.
        /// Removes the Sub Menu, if present.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the Sub Menu was removed.</returns>
        public bool RemoveSubMenu()
        {
            var result = false;

            if (SubMenu != null)
            {
                SubMenu = null;
                result = true;
            }

            return result;
        }

        #endregion

        #region Sync Actions

        /// <summary>
        /// Add Sync Action.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="action">The action to add. () => { Your method here. }</param>
        /// <param name="isEnabled">Is action enabled?</param>
        /// <returns>Returns a bool indicating whether the action was added.</returns>
        public bool AddSyncAction(string identifier, Action action, bool isEnabled = true)
        {
            var result = false;

            try
            {
                if (string.IsNullOrWhiteSpace(identifier) == false &&
                    Actions.ContainsKey(identifier) == false)
                {
                    Actions.Add(identifier, new UIMenuItemAction(identifier, action, isEnabled));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Get Sync Action.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <returns>Returns the stored UIMenuItemAction or null, if not found.</returns>
        public UIMenuItemAction GetSyncAction(string identifier)
        {
            UIMenuItemAction result = null;

            if (string.IsNullOrWhiteSpace(identifier) == false &&
                Actions.ContainsKey(identifier))
            {
                result = Actions[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Sync ACtion.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <returns>Returns a bool indicating whether the UIMenutItemAction was removed.</returns>
        public bool RemovesyncAction(string identifier)
        {
            var result = false;

            try
            {
                if (string.IsNullOrWhiteSpace(identifier) == false &&
                    Actions.ContainsKey(identifier))
                {
                    Actions.Remove(identifier);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// Calling this method runs the stored method that matches the identifier provided.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier)
        {
            var result = false;

            if (IsActionable)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(identifier) == false)
                    {
                        UIMenuItemAction action;

                        if ((action = GetSyncAction(identifier)) != null)
                        {
                            result = action.Run();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException)
                    {
                        ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// Calling this method runs the stored method that matches the identifier provided if the provided key is pressed.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="key">The keyboard key in which to check for a press.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier, Keys key)
        {
            var result = false;

            if (IsActionable)
            {
                if (IOKeyboard.KeyPress(key))
                {
                    result = ActivateSyncAction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// Calling this method runs the stored method that matches the identifier provided if the provided key is pressed and the menu item index numbers match.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="key">The keyboard key in which to check for a press.</param>
        /// <param name="currentMenuItemIndexNumber">The current menu item index number.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier, Keys key, int currentMenuItemIndexNumber)
        {
            var result = false;

            if (IsActionable)
            {
                if (IndexNumber == currentMenuItemIndexNumber &&
                    IOKeyboard.KeyPress(key))
                {
                    result = ActivateSyncAction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="rectangle">The rectangle to check if it intersects with or is inside the menu item's rectangle.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier, Rectangle rectangle)
        {
            var result = false;

            if (IsActionable)
            {
                if (CheckIsInFocus(rectangle))
                {
                    result = ActivateSyncAction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// Calling this method runs the stored method that matches the identifier provided if the provided mouse button is pressed.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="button">The mouse button in which to check against for a click.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier, IOMouse.Buttons button)
        {
            var result = false;

            if (IsActionable)
            {
                switch (button)
                {
                    case IOMouse.Buttons.Left:
                        if (IOMouse.LeftClickUpInside(Rectangle))
                        {
                            result = ActivateSyncAction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Middle:
                        if (IOMouse.MiddleClickUpInside(Rectangle))
                        {
                            result = ActivateSyncAction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Right:
                        if (IOMouse.RightClickUpInside(Rectangle))
                        {
                            result = ActivateSyncAction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.One:
                        if (IOMouse.ButtonOneClickUpInside(Rectangle))
                        {
                            result = ActivateSyncAction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Two:
                        if (IOMouse.ButtonTwoClickUpInside(Rectangle))
                        {
                            result = ActivateSyncAction(identifier);
                        }

                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Sync Action.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Action.</param>
        /// <param name="currentMenuItemIndexNumber">The current menu item index number.</param>
        /// <returns>Returns a bool indicating whether the Action was invoked.</returns>
        public bool ActivateSyncAction(string identifier, int currentMenuItemIndexNumber)
        {
            var result = false;

            if (IsActionable)
            {
                if (CheckIsInFocus(currentMenuItemIndexNumber))
                {
                    result = ActivateSyncAction(identifier);
                }
            }

            return result;
        }

        #endregion

        #region Async Functions

        /// <summary>
        /// Add Async Function.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="function">The asynchronous function to add. () => { Your method here. }</param>
        /// <param name="isEnabled">Is function enabled?</param>
        /// <returns>Returns a bool indicating whether the function was added.</returns>
        public bool AddAsyncFunction(string identifier, Func<Task<bool>> function, bool isEnabled = true)
        {
            var result = false;

            try
            {
                if (string.IsNullOrWhiteSpace(identifier) == false &&
                    Functions.ContainsKey(identifier) == false)
                {
                    Functions.Add(identifier, new UIMenuItemFunction(identifier, function, isEnabled));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is ArgumentException)
                {
                    ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Get Async Function.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <returns>Returns the stored UIMenuItemFunction or null, if not found.</returns>
        public UIMenuItemFunction GetAsyncFunction(string identifier)
        {
            UIMenuItemFunction result = null;

            if (string.IsNullOrWhiteSpace(identifier) == false &&
                Functions.ContainsKey(identifier))
            {
                result = Functions[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Async Function.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <returns>Returns a bool indicating whether the UIMenutItemFunction was removed.</returns>
        public bool RemoveAsyncFunction(string identifier)
        {
            var result = false;

            try
            {
                if (string.IsNullOrWhiteSpace(identifier) == false &&
                    Functions.ContainsKey(identifier))
                {
                    Functions.Remove(identifier);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// Calling this method runs the stored method that matches the identifier provided.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier)
        {
            var result = false;

            if (IsActionable)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(identifier) == false)
                    {
                        UIMenuItemFunction function;

                        if ((function = GetAsyncFunction(identifier)) != null &&
                            function.State == UIMenuItemFunction.States.Stopped)
                        {
                            result = await function.Run();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException)
                    {
                        ParentMenu.Logger.Write(LogTypes.Error, ex.ToString(), useInlineLayout: false);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// Calling this method runs the stored method that matches the identifier provided and the provided key is pressed.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="key">The keyboard key in which to check for a press.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier, Keys key)
        {
            var result = false;

            if (IsActionable)
            {
                if (IOKeyboard.KeyPress(key))
                {
                    result = await ActivateAsyncFunction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// Calling this method runs the stored method that matches the identifier provided if the provided key is pressed and the menu item index numbers match.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="key">The keyboard key in which to check for a press.</param>
        /// <param name="currentMenuItemIndexNumber">The current menu item index number.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier, Keys key, int currentMenuItemIndexNumber)
        {
            var result = false;

            if (IsActionable)
            {
                if (IndexNumber == currentMenuItemIndexNumber &&
                    IOKeyboard.KeyPress(key))
                {
                    result = await ActivateAsyncFunction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="rectangle">The rectangle to check if it intersects with or is inside the menu item's rectangle.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier, Rectangle rectangle)
        {
            var result = false;

            if (IsActionable)
            {
                if (CheckIsInFocus(rectangle))
                {
                    result = await ActivateAsyncFunction(identifier);
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// Calling this method runs the stored method that matches the identifier provided if the provided mouse button is pressed.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="button">The mouse button in which to check against for a click.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier, IOMouse.Buttons button)
        {
            var result = false;

            if (IsActionable)
            {
                switch (button)
                {
                    case IOMouse.Buttons.Left:
                        if (IOMouse.LeftClickUpInside(Rectangle))
                        {
                            result = await ActivateAsyncFunction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Middle:
                        if (IOMouse.MiddleClickUpInside(Rectangle))
                        {
                            result = await ActivateAsyncFunction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Right:
                        if (IOMouse.RightClickUpInside(Rectangle))
                        {
                            result = await ActivateAsyncFunction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.One:
                        if (IOMouse.ButtonOneClickUpInside(Rectangle))
                        {
                            result = await ActivateAsyncFunction(identifier);
                        }

                        break;
                    case IOMouse.Buttons.Two:
                        if (IOMouse.ButtonTwoClickUpInside(Rectangle))
                        {
                            result = await ActivateAsyncFunction(identifier);
                        }

                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Activate Async Function.
        /// </summary>
        /// <param name="identifier">A unique identifier used to access the stored Function.</param>
        /// <param name="currentMenuItemIndexNumber">The current menu item index number.</param>
        /// <returns>Returns a bool indicating whether the Function was invoked.</returns>
        public async Task<bool> ActivateAsyncFunction(string identifier, int currentMenuItemIndexNumber)
        {
            var result = false;

            if (IsActionable)
            {
                if (CheckIsInFocus(currentMenuItemIndexNumber))
                {
                    result = await ActivateAsyncFunction(identifier);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// UI Menu Item Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public override async Task Update(GameTime gameTime)
        {
            ParentPosition = ParentRow.ParentPosition + ParentRow.Position;
            Transparency = ParentRow.Transparency;

            if (Text != null)
            {
                Text.IsVisible = IsVisible;
                Text.IsInFocus = IsActive && IsInFocus;
                Text.Transparency = Transparency;

                if (IsASpacer == false)
                {
                    if (IsMatchingTextWidth)
                    {
                        Width = (int)Text.GetLength().X;
                    }

                    if (IsMatchingTextHeight)
                    {
                        Height = (int)Text.GetLength().Y;
                    }
                }
            }

            foreach (var function in Functions)
            {
                function.Value.IsEnabled = IsActive;
            }

            foreach (var action in Actions)
            {
                action.Value.IsEnabled = IsActive;
            }

            Defaults.Position = SetMenuItemAlignment();

            if (IsVisible)
            {
                if (SubMenu != null)
                {
                    SubMenu.ParentPosition = ParentPosition + Position;
                    SubMenu.Transparency = Transparency;

                    await SubMenu.Update(gameTime);
                }

                if (IsAutoSensing)
                {
                    // Check Focus against Menu Index and IOMouse Rectangle.
                    if (CheckIsInFocus(ParentMenu.ActiveMenuIndexNumber) == false)
                    {
                        // Check if any input devices are over top of any of the menu items.
                        for (var index = 0; index < ParentGroup.ActiveInputDevices.Count; index++)
                        {
                            var inputDevice = ParentGroup.ActiveInputDevices[index];

                            if (CheckIsInFocus(new Rectangle((int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).X,
                                                             (int)ParentGroup.UICamera.GetWorldPosition(new Vector2(inputDevice.X, inputDevice.Y)).Y,
                                                             inputDevice.Width,
                                                             inputDevice.Height)))
                            {
                                ParentMenu.ActiveMenuIndexNumber = IndexNumber;
                            }
                        }
                    }
                }

                if (IsInFocus)
                {
                    if (WillScaleOutOnSelection)
                    {
                        ScaleOut(Defaults.Scale, OnSelectionScaleOutTo, ScalingOutSpeed);
                        Text?.ScaleOut(Text.Defaults.Scale, OnSelectionScaleOutTo, ScalingOutSpeed);
                    }

                    if (WillScaleInOnSelection)
                    {
                        ScaleIn(Defaults.Scale, OnSelectionScaleInTo, ScalingInSpeed);
                        Text?.ScaleIn(Text.Defaults.Scale, OnSelectionScaleInTo, ScalingInSpeed);
                    }

                    if (WillShiftUpOnSelection)
                    {
                        ShiftUp(Defaults.Position, Defaults.Position + OnSelectionShiftUpBy, ShiftingUpSpeed);
                    }

                    if (WillShiftRightOnSelection)
                    {
                        ShiftRight(Defaults.Position, Defaults.Position + OnSelectionShiftRightBy, ShiftingRightSpeed);
                    }

                    if (WillShiftDownOnSelection)
                    {
                        ShiftDown(Defaults.Position, Defaults.Position + OnSelectionShiftDownBy, ShiftingDownSpeed);
                    }

                    if (WillShiftLeftOnSelection)
                    {
                        ShiftLeft(Defaults.Position, Defaults.Position + OnSelectionShiftLeftBy, ShiftingLeftSpeed);
                    }
                }
                else
                {
                    if (WillScaleOutOnSelection)
                    {
                        ScaleIn(OnSelectionScaleOutTo, Defaults.Scale, ScalingInSpeed);
                        Text?.ScaleIn(OnSelectionScaleOutTo, Text.Defaults.Scale, ScalingInSpeed);
                    }

                    if (WillScaleInOnSelection)
                    {
                        ScaleOut(OnSelectionScaleInTo, Defaults.Scale, ScalingOutSpeed);
                        Text?.ScaleOut(OnSelectionScaleInTo, Text.Defaults.Scale, ScalingOutSpeed);
                    }

                    if (WillShiftUpOnSelection)
                    {
                        ShiftDown(Defaults.Position + OnSelectionShiftUpBy, Defaults.Position, ShiftingDownSpeed);
                    }

                    if (WillShiftRightOnSelection)
                    {
                        ShiftLeft(Defaults.Position + OnSelectionShiftRightBy, Defaults.Position, ShiftingLeftSpeed);
                    }

                    if (WillShiftDownOnSelection)
                    {
                        ShiftUp(Defaults.Position + OnSelectionShiftDownBy, Defaults.Position, ShiftingUpSpeed);
                    }

                    if (WillShiftLeftOnSelection)
                    {
                        ShiftRight(Defaults.Position + OnSelectionShiftLeftBy, Defaults.Position, ShiftingRightSpeed);
                    }
                }
            }

            await base.Update(gameTime);

            if (Text != null)
            {
                Text.ParentPosition = ParentPosition + Position;

                switch (Text.VerticalAlignment)
                {
                    case UIText.VerticalAlignments.Upper:
                        Text.Position = new Vector2(0, -Height / 2f);
                        break;
                    case UIText.VerticalAlignments.Center:
                        Text.Position = Vector2.Zero;
                        break;
                    case UIText.VerticalAlignments.Lower:
                        Text.Position = new Vector2(0, Height / 2f);
                        break;
                }

                switch (Text.HorizontalAlignment)
                {
                    case UIText.HorizontalAlignments.Left:
                        Text.Position = new Vector2(-Width / 2f, 0);
                        break;
                    case UIText.HorizontalAlignments.Center:
                        Text.Position = Vector2.Zero;
                        break;
                    case UIText.HorizontalAlignments.Right:
                        Text.Position = new Vector2(Width / 2f, 0);
                        break;
                }

                await Text.Update(gameTime);
            }
        }

        /// <summary>
        /// UI Menu Item Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsVisible)
            {
                if (IsASpacer == false)
                {
                    Text?.Draw(spriteBatch);
                    SubMenu?.Draw(spriteBatch);
                }
            }
        }
    }
}