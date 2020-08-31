using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.CORE.V2.Graphics.Transforms;
using Softfire.MonoGame.CORE.V2.Input;
using Softfire.MonoGame.CORE.V2.Physics;

namespace Softfire.MonoGame.CORE.V2.Common
{
    /// <summary>
    /// A MonoGame Object.
    /// </summary>
    public abstract class MonoGameObject : IMonoGame2DComponent
    {
        #region Fields
        
        /// <summary>
        /// The <see cref="MonoGameObject"/>'s internal tab order value.
        /// </summary>
        private int _tabOrder;

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s internal selection value.
        /// </summary>
        private bool _isSelected;

        #endregion

        #region Booleans

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/>'s updates can occur.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is selectable.
        /// </summary>
        public bool IsSelectable { get; set; } = true;

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = IsSelectable ? value : _isSelected;
        }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is hoverable.
        /// </summary>
        public bool IsHoverable { get; set; } = true;

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is focusable.
        /// </summary>
        public bool IsFocusable { get; set; } = true;

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is movable.
        /// </summary>
        public bool IsMovable { get; set; }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is highlightable.
        /// </summary>
        public bool IsHighlightable { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The time between updates.
        /// </summary>
        protected double DeltaTime { get; set; }

        /// <summary>
        /// An elapsed time counter.
        /// </summary>
        protected double ElapsedTime { get; set; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s unique name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Identifies the <see cref="MonoGameObject"/>'s tab order index.
        /// </summary>
        public int TabOrder
        {
            get => _tabOrder;
            set => _tabOrder = value >= 0 ? value : 0;
        }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s current layer.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// The parent <see cref="MonoGameObject"/>.
        /// </summary>
        public MonoGameObject Parent { get; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s child <see cref="MonoGameObject"/>'s.
        /// </summary>
        public IList<MonoGameObject> Children { get; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s position, scale and rotation.
        /// </summary>
        public Transform2D Transform { get; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s <see cref="Movement"/> class.
        /// </summary>
        public Movement Movement { get; }

        #region Dimensions

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s rectangle.
        /// </summary>
        public RectangleF Rectangle { get; protected set; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s extended rectangle.
        /// </summary>
        public RectangleF ExtendedRectangle { get; protected set; }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s origin.
        /// </summary>
        public Vector2 Origin => new Vector2((Size.Width * Transform.Scale.X) / 2f,
                                             (Size.Height * Transform.Scale.Y) / 2f);

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s height and width.
        /// </summary>
        public SizeF Size { get; }

        #endregion

        /// <summary>
        /// Input events used to interact with the <see cref="MonoGameObject"/>.
        /// </summary>
        public InputEventArgs Events { get; } = InputEventArgs.Empty;

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> has focus.
        /// </summary>
        public FocusStates FocusState { get; set; }

        /// <summary>
        /// Hovered <see cref="MonoGameObject"/> stack.
        /// <see cref="MonoGameObject"/>'s get pushed onto the stack when they are hovered and get cleared when focus is lost.
        /// </summary>
        public Stack<MonoGameObject> HoverStack { get; } = new Stack<MonoGameObject>();

        #endregion

        /// <summary>
        /// The base MonoGame object.
        /// </summary>
        /// <param name="parent">The parent <see cref="MonoGameObject"/>.</param>
        /// <param name="id">The <see cref="MonoGameObject"/>'s id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The <see cref="MonoGameObject"/>'s name. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The <see cref="MonoGameObject"/>'s position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The <see cref="MonoGameObject"/>'s width. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The <see cref="MonoGameObject"/>'s height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The <see cref="MonoGameObject"/>'s visibility. Intaken as a <see cref="bool"/>.</param>
        protected MonoGameObject(MonoGameObject parent, int id, string name, Vector2 position = default, int width = 10, int height = 10, bool isVisible = true)
        {
            // Core properties
            Id = id;
            Name = name;
            Parent = parent;
            Size = new SizeF(width, height);
            IsVisible = isVisible;
            
            Children = new List<MonoGameObject>();
            Movement = new Movement(this);

            Transform = new Transform2D(position, 0f, Vector2.One)
            {
                Parent = Parent?.Transform
            };
        }

        #region Events

        /// <summary>
        /// Passes this <see cref="MonoGameObject"/> up to it's parent.
        /// </summary>
        public void RiseUp()
        {
            RiseUp(this);
        }

        /// <summary>
        /// Passes the <see cref="MonoGameObject"/> up to it's parent.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="mgObject">The <see cref="MonoGameObject"/> to pass up to the parent.</param>
        public virtual void RiseUp<T>(T mgObject) where T : MonoGameObject
        {
            Parent?.RiseUp(mgObject);
        }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is being hovered. A recursive check is performed on all children.
        /// </summary>
        /// <param name="input">The input rectangle. Intaken as a <see cref="RectangleF"/>.</param>
        /// <returns>Returns the hovered <see cref="MonoGameObject"/>, if found, otherwise null.</returns>
        public MonoGameObject CheckForHover(RectangleF input) => CheckForHover(this, input);

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is being hovered. A recursive check is performed on all children.
        /// </summary>
        /// <param name="mgObject">The <see cref="MonoGameObject"/> to check for hover.</param>
        /// <param name="input">The input rectangle. Intaken as a <see cref="RectangleF"/>.</param>
        /// <returns>Returns the hovered <see cref="MonoGameObject"/>, if found, otherwise null.</returns>
        public static MonoGameObject CheckForHover(MonoGameObject mgObject, RectangleF input)
        {
            // Only check the children for hover if the MonoGameObject is active and visible.
            if (mgObject.IsActive && mgObject.IsVisible)
            {
                // Check all child MonoGameObjects for hover.
                foreach (var child in mgObject.Children)
                {
                    MonoGameObject hoveredMonoGameObject;

                    // Check current child and all it's children for hover.
                    // Performs a recursive loop for all child MonoGameObjects.
                    if ((hoveredMonoGameObject = CheckForHover(child, input)) != null)
                    {
                        // If the MonoGameObject intersects or contains the input rectangle.
                        if (hoveredMonoGameObject.Rectangle.IntersectsWith(input) || hoveredMonoGameObject.Rectangle.Contains(input))
                        {
                            // Rise up the last hovered child to the parent.
                            if (!hoveredMonoGameObject.IsStateSet(FocusStates.IsHovered) &&
                                hoveredMonoGameObject.IsHoverable)
                            {
                                hoveredMonoGameObject.RiseUp();
                            }

                            // Return the hovered child if it is hoverable, otherwise return it's parent.
                            return hoveredMonoGameObject.IsHoverable ? hoveredMonoGameObject : hoveredMonoGameObject.Parent;
                        }
                    }
                }

                // If the child intersects or contains the input rectangle.
                if (mgObject.Rectangle.IntersectsWith(input) || mgObject.Rectangle.Contains(input))
                {
                    // If no child is hovered then pass the current child.
                    return mgObject;
                }
            }

            // If the object is not intersecting or does not contain the input rectangle then return null.
            return null;
        }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is focused.
        /// </summary>
        /// <param name="children">The list of <see cref="MonoGameObject"/>s to check against for focus.</param>
        /// <param name="args">The event arguments. Intaken as a <see cref="InputEventArgs"/>.</param>
        /// <param name="condition">The condition that must be met to grant focus to the <see cref="MonoGameObject"/>.</param>
        /// <param name="multiFocus">Allows the focusing of multiple <see cref="MonoGameObject"/>s, if set to true.</param>
        public void CheckForFocus<T>(IList<T> children, InputEventArgs args, bool condition, bool multiFocus = false) where T : MonoGameObject => CheckForFocus(this, children, args, condition, multiFocus);

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is being focused. A recursive check is performed on all children.
        /// </summary>
        /// <param name="mgObject">The <see cref="MonoGameObject"/> to check for focus.</param>
        /// <param name="condition">The condition that must be met to grant focus to the <see cref="MonoGameObject"/>.</param>
        /// <returns>Returns the focused <see cref="MonoGameObject"/>, if found, otherwise null.</returns>
        public static MonoGameObject CheckForFocus(MonoGameObject mgObject, bool condition)
        {
            // Only check the children for focus if the MonoGameObject is active and visible.
            if (mgObject.IsActive && mgObject.IsVisible)
            {
                // Check all child MonoGameObjects for focus.
                foreach (var child in mgObject.Children)
                {
                    MonoGameObject focusedMonoGameObject;

                    // Check current child and all it's children for focus.
                    // Performs a recursive loop for all child MonoGameObjects.
                    if ((focusedMonoGameObject = CheckForFocus(child, condition)) != null)
                    {
                        // Rise up the last hovered child to the parent.
                        if (!focusedMonoGameObject.IsStateSet(FocusStates.IsFocused) &&
                            focusedMonoGameObject.IsFocusable &&
                            condition)
                        {
                            focusedMonoGameObject.RiseUp();
                        }

                        // Return the focused child if it is focusable, otherwise return it's parent.
                        return focusedMonoGameObject.IsFocusable ? focusedMonoGameObject : focusedMonoGameObject.Parent;
                    }

                    // If no child is focused then pass the current child.
                    return mgObject;
                }
            }

            // If the object is not intersecting or does not contain the input rectangle then return null.
            return null;
        }

        /// <summary>
        /// Determines whether the <see cref="MonoGameObject"/> is focused.
        /// </summary>
        /// <param name="mgObject">The <see cref="MonoGameObject"/> to check for focus.</param>
        /// <param name="children">The list of <see cref="MonoGameObject"/>s to check against for focus.</param>
        /// <param name="args">The event arguments. Intaken as a <see cref="InputEventArgs"/>.</param>
        /// <param name="condition">The condition that must be met to grant focus to the <see cref="MonoGameObject"/>.</param>
        /// <param name="multiFocus">Allows the focusing of multiple <see cref="MonoGameObject"/>s, if set to true.</param>
        public static void CheckForFocus<T>(MonoGameObject mgObject, IList<T> children, InputEventArgs args, bool condition, bool multiFocus = false) where T : MonoGameObject
        {
            // If the object is already focused then skip it.
            if (mgObject.IsStateSet(FocusStates.IsFocused))
            {
                return;
            }

            foreach (var child in children)
            {
                // Skip the current MonoGameObject if they're equal.
                if (!mgObject.Equals(child))
                {
                    // Calculate any overlap.
                    var overlap = mgObject.ExtendedRectangle.OverlapWith(child.ExtendedRectangle);

                    // If there is overlap and the input rectangle intersects or is contained within the overlap then skip to the next MonoGameObject.
                    if (overlap != RectangleF.Empty && (overlap.IntersectsWith(args.InputRectangle) || overlap.Contains(args.InputRectangle)))
                    {
                        continue;
                    }
                }

                // if the condition is met then apply focus to the MonoGameObject.
                if (condition)
                {
                    // Sets focus.
                    mgObject.AddState(FocusStates.IsFocused);

                    // If multi focus is false then remove focus from other MonoGameObjects.
                    if (!multiFocus)
                    {
                        // Remove focus from other MonoGameObjects, if focused.
                        foreach (var otherChild in children)
                        {
                            if (!mgObject.Equals(otherChild))
                            {
                                otherChild.RemoveState(FocusStates.IsFocused);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The subscription method to action when input changes.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnMove(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                Events.InputDeltas = args.InputDeltas;
                Events.InputRectangle = args.InputRectangle;
                Events.PlayerIndex = args.PlayerIndex;
            }
        }

        /// <summary>
        /// The subscription method to action when the <see cref="MonoGameObject"/> is scrolled, by an input device with a pointer such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnScrollHovered(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                MonoGameObject hoveredMonoGameObject;

                if ((hoveredMonoGameObject = CheckForHover(this, args.InputRectangle)) != null)
                {
                    hoveredMonoGameObject.Events.InputScrollVelocity = args.InputScrollVelocity;
                    hoveredMonoGameObject.Events.PlayerIndex = args.PlayerIndex;
                    hoveredMonoGameObject.Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollUp, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollUp));
                    hoveredMonoGameObject.Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollDown, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollDown));
                    hoveredMonoGameObject.Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollLeft, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollLeft));
                    hoveredMonoGameObject.Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollRight, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollRight));
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the <see cref="MonoGameObject"/> is scrolled.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnScroll(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                Events.InputScrollVelocity = args.InputScrollVelocity;
                Events.PlayerIndex = args.PlayerIndex;
                Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollUp, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollUp));
                Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollDown, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollDown));
                Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollLeft, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollLeft));
                Events.InputFlags.SetFlag(InputMouseActionFlags.ScrollRight, args.InputFlags.IsFlagSet(InputMouseActionFlags.ScrollRight));
                Events.InputFlags.SetFlag(InputKeyboardArrowFlags.UpKey, args.InputFlags.IsFlagSet(InputKeyboardArrowFlags.UpKey));
                Events.InputFlags.SetFlag(InputKeyboardArrowFlags.DownKey, args.InputFlags.IsFlagSet(InputKeyboardArrowFlags.DownKey));
                Events.InputFlags.SetFlag(InputKeyboardArrowFlags.LeftKey, args.InputFlags.IsFlagSet(InputKeyboardArrowFlags.LeftKey));
                Events.InputFlags.SetFlag(InputKeyboardArrowFlags.RightKey, args.InputFlags.IsFlagSet(InputKeyboardArrowFlags.RightKey));
                Events.InputFlags.SetFlag(InputKeyboardCommandFlags.PageUpKey, args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.PageUpKey));
                Events.InputFlags.SetFlag(InputKeyboardCommandFlags.PageDownKey, args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.PageDownKey));
                Events.InputFlags.SetFlag(InputKeyboardCommandFlags.HomeKey, args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.HomeKey));
                Events.InputFlags.SetFlag(InputKeyboardCommandFlags.EndKey, args.InputFlags.IsFlagSet(InputKeyboardCommandFlags.EndKey));
                Events.InputFlags.SetFlag(InputKeyboardNumPadFlags.NumPad8Key, args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad8Key));
                Events.InputFlags.SetFlag(InputKeyboardNumPadFlags.NumPad2Key, args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad2Key));
                Events.InputFlags.SetFlag(InputKeyboardNumPadFlags.NumPad4Key, args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad4Key));
                Events.InputFlags.SetFlag(InputKeyboardNumPadFlags.NumPad6Key, args.InputFlags.IsFlagSet(InputKeyboardNumPadFlags.NumPad6Key));
            }
        }

        /// <summary>
        /// The subscription method to action when the <see cref="MonoGameObject"/> gains focus, by an input device with a pointer such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnFocusHovered(object sender, InputEventArgs args)
        {
            MonoGameObject hoveredMonoGameObject;

            if ((hoveredMonoGameObject = CheckForHover(this, args.InputRectangle)) != null)
            {
                // Add the object to the hover stack for clearing of it's hover flag later.
                if (hoveredMonoGameObject.Equals(this) &&
                    !hoveredMonoGameObject.IsStateSet(FocusStates.IsHovered))
                {
                    hoveredMonoGameObject.RiseUp();
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the <see cref="MonoGameObject"/> gains focus, by the keyboard's tab function.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnFocusTabbed(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                TabOrder == args.InputTabOrderId)
            {
                RiseUp();
            }
        }

        /// <summary>
        /// The subscription method to action when the object loses focus or blurs.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnBlur(object sender, InputEventArgs args)
        {
            if (HoverStack.Count > 0)
            {
                var nextChild = HoverStack.Peek();

                if (!nextChild.Rectangle.IntersectsWith(args.InputRectangle) &&
                    !nextChild.Rectangle.Contains(args.InputRectangle))
                {
                    nextChild = HoverStack.Pop();
                    nextChild.RemoveState(FocusStates.IsHovered);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a press action, when an input device with a pointer such as a mouse or gamepad is hovering over the object.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnPressHovered(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsHovered) &&
                IsStateSet(FocusStates.IsFocused))
            {
                MonoGameObject hoveredMonoGameObject;

                if ((hoveredMonoGameObject = CheckForHover(this, args.InputRectangle)) != null)
                {
                    InputDeviceActionFlagCheck(hoveredMonoGameObject, args, InputActionStateFlags.Press);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a press action.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnPress(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                InputDeviceActionFlagCheck(this, args, InputActionStateFlags.Press);
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been released, when an input device with a pointer such as a mouse or gamepad is hovering over the object.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnReleaseHovered(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsHovered) &&
                IsStateSet(FocusStates.IsFocused))
            {
                MonoGameObject hoveredMonoGameObject;

                if ((hoveredMonoGameObject = CheckForHover(this, args.InputRectangle)) != null)
                {
                    InputDeviceActionFlagCheck(hoveredMonoGameObject, args, InputActionStateFlags.Release);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been released.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnRelease(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                InputDeviceActionFlagCheck(this, args, InputActionStateFlags.Release);
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been held, when an input device with a pointer such as a mouse or gamepad is hovering over the object.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnHeldHovered(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsHovered) &&
                IsStateSet(FocusStates.IsFocused))
            {
                MonoGameObject hoveredMonoGameObject;

                if ((hoveredMonoGameObject = CheckForHover(this, args.InputRectangle)) != null)
                {
                    InputDeviceActionFlagCheck(hoveredMonoGameObject, args, InputActionStateFlags.Held);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects a click has been held.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        public void OnHeld(object sender, InputEventArgs args)
        {
            if (IsActive && IsVisible &&
                IsStateSet(FocusStates.IsFocused))
            {
                InputDeviceActionFlagCheck(this, args, InputActionStateFlags.Held);
            }
        }

        /// <summary>
        /// Cycles through all input action flags to check if the flag is active.
        /// </summary>
        /// <param name="obj">The <see cref="MonoGameObject"/> to check against.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        /// <param name="inputActionStateFlagToCheck">The <see cref="InputActionStateFlags"/> to check to see if it is active on the provided <see cref="MonoGameObject"/>.</param>
        private void InputDeviceActionFlagCheck(MonoGameObject obj, InputEventArgs args, InputActionStateFlags inputActionStateFlagToCheck)
        {
            #region Mouse

            // Cycle through all flags.
            foreach (InputMouseActionFlags flag in Enum.GetValues(typeof(InputMouseActionFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            #endregion

            #region Keyboard

            // Cycle through all flags.
            foreach (InputKeyboardFunctionFlags flag in Enum.GetValues(typeof(InputKeyboardFunctionFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardNumPadFlags flag in Enum.GetValues(typeof(InputKeyboardNumPadFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardNumberFlags flag in Enum.GetValues(typeof(InputKeyboardNumberFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardCommandFlags flag in Enum.GetValues(typeof(InputKeyboardCommandFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardSpecialFlags flag in Enum.GetValues(typeof(InputKeyboardSpecialFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardSpecialFlags flag in Enum.GetValues(typeof(InputKeyboardSpecialFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardArrowFlags flag in Enum.GetValues(typeof(InputKeyboardArrowFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all flags.
            foreach (InputKeyboardLetterFlags flag in Enum.GetValues(typeof(InputKeyboardLetterFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            #endregion

            #region Gamepad

            // Cycle through all flags.
            foreach (InputGamepadActionFlags flag in Enum.GetValues(typeof(InputGamepadActionFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            #endregion

            #region Mappable

            // Cycle through all confirmation flags.
            foreach (InputMappableConfirmationCommandFlags flag in Enum.GetValues(typeof(InputMappableConfirmationCommandFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all movement flags.
            foreach (InputMappableMovementCommandFlags flag in Enum.GetValues(typeof(InputMappableMovementCommandFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            // Cycle through all movement flags.
            foreach (InputMappableCameraCommandFlags flag in Enum.GetValues(typeof(InputMappableCameraCommandFlags)))
            {
                // Is the flag in the provided state.
                if (args.InputStates.GetState(flag) == inputActionStateFlagToCheck)
                {
                    // Add the provided flag.
                    obj.Events.InputStates.SetState(flag, inputActionStateFlagToCheck);
                }
            }

            #endregion
        }

        #endregion

        #region States

        /// <summary>
        /// Sets the state flag.
        /// </summary>
        /// <param name="state">The state flag to set.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns>Returns the result of operation as a <see cref="bool"/>.</returns>
        public bool SetState(FocusStates state, bool result)
        {
            if (result)
            {
                AddState(state);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the state flag is set.
        /// </summary>
        /// <param name="state">The state flag to check.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the state flag is set.</returns>
        public bool IsStateSet(FocusStates state) => (FocusState & state) == state;

        /// <summary>
        /// Removes the state flag, if set.
        /// </summary>
        /// <param name="state">The state flag to remove, if set.</param>
        public void RemoveState(FocusStates state)
        {
            if (IsStateSet(state))
            {
                FocusState &= ~state;
            }
        }

        /// <summary>
        /// Adds the state flag.
        /// </summary>
        /// <param name="state">The state flag to add.</param>
        public void AddState(FocusStates state) => FocusState |= state;

        /// <summary>
        /// Clears the state flags.
        /// </summary>
        public void ClearStates() => FocusState = 0;

        #endregion

        #region Children

        /// <summary>
        /// Retrieves the next valid id for a child <see cref="MonoGameObject"/> on the provided layer.
        /// </summary>
        /// <typeparam name="T">An object of type <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to produce a valid id on. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a valid id for a child <see cref="MonoGameObject"/> as an <see cref="int"/>.</returns>
        protected int GetNextValidChildId<T>(int layer) where T : MonoGameObject => Identities.GetNextValidObjectId<MonoGameObject, T>(Children, layer);

        /// <summary>
        /// Determines whether a child <see cref="MonoGameObject"/> exists on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the child <see cref="MonoGameObject"/> to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the child <see cref="MonoGameObject"/> exists.</returns>
        protected bool ChildExists<T>(int layer, int id) where T : MonoGameObject => Identities.ObjectExists<MonoGameObject, T>(Children, layer, id);

        /// <summary>
        /// Determines whether a child <see cref="MonoGameObject"/> exists on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the child <see cref="MonoGameObject"/> to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the child <see cref="MonoGameObject"/> exists.</returns>
        protected bool ChildExists<T>(int layer, string name) where T : MonoGameObject => Identities.ObjectExists<MonoGameObject, T>(Children, layer, name);

        /// <summary>
        /// Gets the <see cref="MonoGameObject"/> on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the child <see cref="MonoGameObject"/> to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a child <see cref="MonoGameObject"/> with the provided id, if present, otherwise null.</returns>
        protected T GetChild<T>(int layer, int id) where T : MonoGameObject => Identities.GetObject<MonoGameObject, T>(Children, layer, id);

        /// <summary>
        /// Gets the <see cref="MonoGameObject"/> on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="elementName">The name of the child <see cref="MonoGameObject"/> to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a child <see cref="MonoGameObject"/> with the provided name, if present, otherwise null.</returns>
        protected T GetChild<T>(int layer, string elementName) where T : MonoGameObject => Identities.GetObject<MonoGameObject, T>(Children, layer, elementName);

        /// <summary>
        /// Removes a child <see cref="MonoGameObject"/> on the provided layer, by id.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the child <see cref="MonoGameObject"/> to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the child <see cref="MonoGameObject"/> was removed.</returns>
        protected bool RemoveChild<T>(int layer, int id) where T : MonoGameObject => Identities.RemoveObject<MonoGameObject, T>(Children, layer, id);

        /// <summary>
        /// Removes a child <see cref="MonoGameObject"/> on the provided layer, by name.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="elementName">The name of the child <see cref="MonoGameObject"/> to be removed. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the child <see cref="MonoGameObject"/> was removed.</returns>
        protected bool RemoveChild<T>(int layer, string elementName) where T : MonoGameObject => Identities.RemoveObject<MonoGameObject, T>(Children, layer, elementName);

        /// <summary>
        /// Changes the layer of the child <see cref="MonoGameObject"/> to the provided layer, by id.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="id">The id of the child <see cref="MonoGameObject"/> to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <param name="newLayer">The new layer the <see cref="MonoGameObject"/> will belong to. Intaken as a <see cref="int"/>.</param>
        protected void ChangeLayer<T>(int layer, int id, int newLayer) where T : MonoGameObject => Identities.ChangeObjectLayer<MonoGameObject, T>(Children, layer, id, newLayer);

        /// <summary>
        /// Changes the layer of the child <see cref="MonoGameObject"/> to the provided layer, by name.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="layer">The layer to search on. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The name of the child <see cref="MonoGameObject"/> to retrieve. Intaken as an <see cref="string"/>.</param>
        /// <param name="newLayer">The new layer the <see cref="MonoGameObject"/> will belong to. Intaken as a <see cref="int"/>.</param>
        protected void ChangeLayer<T>(int layer, string name, int newLayer) where T : MonoGameObject => Identities.ChangeObjectLayer<MonoGameObject, T>(Children, layer, name, newLayer);

        #endregion

        /// <summary>
        /// Sets <see cref="ElapsedTime"/> to zero.
        /// </summary>
        public void ResetElapsedTime()
        {
            ElapsedTime = 0D;
        }

        /// <summary>
        /// Calculates the <see cref="MonoGameObject"/>'s rectangle.
        /// </summary>
        /// <returns>Returns the calculated <see cref="Rectangle"/>, unless overridden.</returns>
        protected virtual RectangleF CalculateRectangle()
        {
            return new RectangleF(Transform.WorldPosition().X - Origin.X,
                                  Transform.WorldPosition().Y - Origin.Y,
                                  Size.Width * Transform.Scale.X,
                                  Size.Height * Transform.Scale.Y);
        }

        /// <summary>
        /// Calculates an extended <see cref="RectangleF"/>.
        /// </summary>
        /// <returns>Returns <see cref="Rectangle"/>, unless overridden.</returns>
        protected virtual RectangleF CalculateExtendedRectangle() => CalculateRectangle();

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s content loader.
        /// </summary>
        public virtual void LoadContent(ContentManager content = null)
        {
            // Initiates the MonoGameObject's rectangles.
            Rectangle = CalculateRectangle();
            ExtendedRectangle = CalculateExtendedRectangle();
        }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes <see cref="GameTime"/>.</param>
        public virtual void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive)
            {
                // Update the MonoGameObject's movement.
                Movement.Update(gameTime);

                // Calculate the bounds of the MonoGameObject.
                Rectangle = CalculateRectangle();

                // Calculate the MonoGameObject's extended rectangle.
                ExtendedRectangle = CalculateExtendedRectangle();
            }

            // Clears the events.
            Events.Clear();
        }

        /// <summary>
        /// The <see cref="MonoGameObject"/>'s draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            
        }
    }
}