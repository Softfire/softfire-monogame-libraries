using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Softfire.MonoGame.CORE;

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
        /// The animation manager for animations.
        /// </summary>
        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> to use for drawing content.</param>
        /// <param name="contentManager">The content manager used to create the manager's own content manager. Intaken as a <see cref="ContentManager"/>.</param>
        public AnimationManager(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            AnimationBatch = new SpriteBatch(graphicsDevice);
            Content = new ContentManager(contentManager.ServiceProvider, "Content");

            Animations = new List<Animation>();
        }

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