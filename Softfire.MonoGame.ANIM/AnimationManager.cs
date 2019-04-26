using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;
using Softfire.MonoGame.CORE.Input;
using Softfire.MonoGame.IO;

namespace Softfire.MonoGame.ANIM
{
    /// <summary>
    /// A manager for 2D animations.
    /// </summary>
    public class AnimationManager
    {
        /// <summary>
        /// The animation manager's content manager.
        /// </summary>
        private ContentManager Content { get; }
        
        /// <summary>
        /// The animation manager's sprite batch. 
        /// </summary>
        private SpriteBatch AnimationBatch { get; }

        /// <summary>
        /// The currently loaded animations.
        /// </summary>
        private IList<Animation> Animations { get; }
        
        /// <summary>
        /// The current tab order id.
        /// </summary>
        private int CurrentTabOrderId { get; set; } = 1;

        /// <summary>
        /// The animation manager for animations.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to use for drawing content.</param>
        /// <param name="contentManager">The content manager used to create the manager's own content manager. Intaken as a <see cref="ContentManager"/>.</param>
        public AnimationManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            AnimationBatch = new SpriteBatch(graphicsDevice);
            Content = new ContentManager(contentManager.ServiceProvider, "Content");

            Animations = new List<Animation>();

            // Register movement event.
            // Registration order matters for call order.
            IOManager.InputMovementHandler += OnMove;
            IOManager.InputMovementHandler += OnBlur;
            IOManager.InputMovementHandler += OnFocusHovered;

            // Register scrolling event.
            IOManager.InputScrollHandler += OnScrollHovered;

            // Register input events.
            IOManager.InputPressHandler += OnTab;
            IOManager.InputPressHandler += OnPress;
            IOManager.InputReleaseHandler += OnRelease;
            IOManager.InputHeldHandler += OnHeld;
        }

        #region Events

        /// <summary>
        /// The subscription method to action when input changes.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnMove(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                if (animation.IsStateSet(FocusStates.IsHovered))
                {
                    animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    animation.OnMove(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object is scrolled while being hovered by an input device with a pointer, such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnScrollHovered(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                if (animation.IsStateSet(FocusStates.IsHovered))
                {
                    animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.ScrollUp) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.ScrollDown) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.ScrollLeft) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.ScrollRight) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    animation.OnScrollHovered(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object is scrolled.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnScroll(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.ScrollUp) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMouseActionFlags.ScrollDown) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                animation.OnScroll(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object gains focus while being hovered by an input device with a pointer, such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnFocusHovered(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.OnFocusHovered(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object loses focus or blurs.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnBlur(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.OnBlur(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been pressed while being hovered by an input device with a pointer, such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnPressHovered(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                if (animation.IsStateSet(FocusStates.IsHovered))
                {
                    animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    animation.OnPressHovered(sender, args);
                }

                //if (animation.IsStateSet(FocusStates.IsFocused))
                //{
                //    // Set tab order to currently focused animation.
                //    CurrentTabOrderId = animation.TabOrder;
                //}
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been pressed.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnPress(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                animation.OnPress(sender, args);

                //if (animation.IsStateSet(FocusStates.IsFocused))
                //{
                //    // Set tab order to currently focused animation.
                //    CurrentTabOrderId = animation.TabOrder;
                //}
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been released while being hovered by an input device with a pointer, such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnReleaseHovered(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                if (animation.IsStateSet(FocusStates.IsHovered))
                {
                    animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                              args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                    animation.OnReleaseHovered(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been released.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnRelease(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.CheckForFocus(Animations, args, args.InputStates.GetState(InputMouseActionFlags.LeftClick) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMouseActionFlags.RightClick) == InputActionStateFlags.Press ||
                                                          args.InputStates.GetState(InputMappableConfirmationCommandFlags.Confirm) == InputActionStateFlags.Press);
                animation.OnRelease(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been held while being hovered by an input device with a pointer, such as a mouse or gamepad.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnHeldHovered(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                if (animation.IsStateSet(FocusStates.IsHovered))
                {
                    animation.OnHeldHovered(sender, args);
                }
            }
        }

        /// <summary>
        /// The subscription method to action when the object detects an input has been held.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnHeld(object sender, InputEventArgs args)
        {
            foreach (var animation in Animations)
            {
                animation.OnHeld(sender, args);
            }
        }

        /// <summary>
        /// The subscription method to action when the mapped tabbing button has been pressed.
        /// </summary>
        /// <param name="sender">The subscribed object.</param>
        /// <param name="args">The <see cref="InputEventArgs"/> sent with the call.</param>
        private void OnTab(object sender, InputEventArgs args)
        {
            // If Tab is pressed.
            //if (args.InputStates.GetState(InputMappableMovementCommandFlags.Tab) == InputActionStateFlags.Press ||
            //    args.InputStates.GetState(InputKeyboardCommandFlags.TabKey) == InputActionStateFlags.Press)
            //{
            //    if (CurrentTabOrderId + 1 > Children.Count)
            //    {
            //        CurrentTabOrderId = 1;
            //    }
            //    else
            //    {
            //        CurrentTabOrderId++;
            //    }

            //    foreach (var animation in Children)
            //    {
            //        if (animation.TabOrder == CurrentTabOrderId)
            //        {
            //            // Add focus.
            //            animation.AddState(FocusStates.IsFocused);

            //            // Remove focus from other animations, if focused.
            //            foreach (var child in Children)
            //            {
            //                if (!animation.Equals(child))
            //                {
            //                    child.RemoveState(FocusStates.IsFocused);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        #endregion

        /// <summary>
        /// Retrieves the next valid id from the list of animations.
        /// </summary>
        /// <returns>Returns the next valid id from the list of animations as an <see cref="int"/>.</returns>
        public int GetNextValidAnimationId() => Identities.GetNextValidObjectId<Animation, Animation>(Animations);

        /// <summary>
        /// Loads a new animation.
        /// </summary>
        /// <typeparam name="T">Type <see cref="Animation"/>.</typeparam>
        /// <returns>Returns a <see cref="bool"/> indicating whether the animation was loaded.</returns>
        /// <remarks>If an animation with the same id or name already exists the load will fail.</remarks>
        public bool LoadAnimation<T>(T animation) where T : Animation
        {
            var result = false;

            if (animation != null &&
                !AnimationExists(animation.Name) &&
                !AnimationExists(animation.Id))
            {
                animation.LoadContent(Content);
                Animations.Add(animation);
                result = true;
            }

            return result;
        }

        #region Animations

        /// <summary>
        /// Determines whether an animation exists, by id.
        /// </summary>
        /// <param name="id">The id of the animation to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the animation exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public bool AnimationExists(int id) => Identities.ObjectExists<Animation, Animation>(Animations, id);

        /// <summary>
        /// Determines whether an animation exists, by name.
        /// </summary>
        /// <param name="name">The name of the animation to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the animation exists.</returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> if the provided list is null.</exception>
        public bool AnimationExists(string name) => Identities.ObjectExists<Animation, Animation>(Animations, name);

        /// <summary>
        /// Retrieves an animation by id.
        /// </summary>
        /// <typeparam name="T">Type <see cref="Animation"/>.</typeparam>
        /// <param name="id">The id of the requested animation. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns an object of type <see cref="Animation"/>, if found, otherwise null.</returns>
        public T GetAnimation<T>(int id) where T : Animation => Identities.GetObject<Animation, T>(Animations, id);

        /// <summary>
        /// Retrieves an animation by name.
        /// </summary>
        /// <typeparam name="T">Type <see cref="Animation"/>.</typeparam>
        /// <param name="name">The name of the requested animation. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns an object of type <see cref="Animation"/>, if found, otherwise null.</returns>
        public T GetAnimation<T>(string name) where T : Animation => Identities.GetObject<Animation, T>(Animations, name);

        /// <summary>
        /// Removes an animation by id.
        /// </summary>
        /// <param name="id">The id of the requested animation. Intaken as an <see cref="int"/></param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the animation was removed.</returns>
        public bool RemoveAnimation(int id) => Identities.RemoveObject<Animation, Animation>(Animations, id);

        /// <summary>
        /// Removes an animation by name.
        /// </summary>
        /// <param name="name">The id of the requested animation. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the animation was removed.</returns>
        public bool RemoveAnimation(string name) => Identities.RemoveObject<Animation, Animation>(Animations, name);

        /// <summary> 
        /// Reorder an animation's draw order, by id.
        /// </summary>
        /// <param name="id">The id of the animation to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherId">The id of the animation to move the animation to. Intaken as an <see cref="int"/>.</param>
        public void ReorderAnimationDrawOrder(int id, int otherId) => Identities.ReorderObject<Animation, Animation>(Animations, id, otherId);
        
        /// <summary> 
        /// Reorder an animation's draw order, by name.
        /// </summary>
        /// <param name="name">The name of the animation to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherId">The id of the animation to move the animation to. Intaken as an <see cref="int"/>.</param>
        public void ReorderAnimationDrawOrder(string name, int otherId) => Identities.ReorderObject<Animation, Animation>(Animations, name, otherId);
        
        /// <summary> 
        /// Reorder an animation's draw order, by id.
        /// </summary>
        /// <param name="id">The id of the animation to move. Intaken as an <see cref="int"/>.</param>
        /// <param name="otherName">The name of the animation to move the animation to. Intaken as a <see cref="string"/>.</param>
        public void ReorderAnimationDrawOrder(int id, string otherName) => Identities.ReorderObject<Animation, Animation>(Animations, id, otherName);
        
        /// <summary> 
        /// Reorder an animation's draw order, by name.
        /// </summary>
        /// <param name="name">The name of the animation to move. Intaken as a <see cref="string"/>.</param>
        /// <param name="otherName">The name of the animation to move the animation to. Intaken as a <see cref="string"/>.</param>
        public void ReorderAnimationDrawOrder(string name, string otherName) => Identities.ReorderObject<Animation, Animation>(Animations, name, otherName);
        
        #endregion

        /// <summary>
        /// The animation manager's update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public void Update(GameTime gameTime)
        {
            foreach (var animation in Animations)
            {
                animation.Update(gameTime);
            }
        }

        /// <summary>
        /// The animation manager's draw method.
        /// </summary>
        public void Draw()
        {
            AnimationBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (var animation in Animations)
            {
                animation.Draw(AnimationBatch, Matrix.Identity);
            }

            AnimationBatch.End();
        }
    }
}