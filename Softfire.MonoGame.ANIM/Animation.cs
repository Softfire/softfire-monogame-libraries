using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Softfire.MonoGame.ANIM.Actions;
using Softfire.MonoGame.CORE;
using Softfire.MonoGame.CORE.Common;

namespace Softfire.MonoGame.ANIM
{
    /// <summary>
    /// A 2D animation class.
    /// </summary>
    public abstract class Animation : MonoGameObject
    {
        #region Fields
        
        /// <summary>
        /// The animation's internal transparency value.
        /// </summary>
        private float _transparency = 1.0f;

        #endregion

        #region Properties

        /// <summary>
        /// The animation's texture path. Relative to the Content path.
        /// </summary>
        /// <example>"Assets/Sprites/Player"</example>
        /// <remarks>File extension is left out. Example file is Player.png.</remarks>
        private string TexturePath { get; }

        /// <summary>
        /// The animation's sprite sheet used for animation.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// The animation's color data can be used for collision detection.
        /// </summary>
        public Color[] ColorData { get; private set; }

        /// <summary>
        /// The animation's transparency level.
        /// </summary>
        public float Transparency
        {
            get => _transparency;
            set => _transparency = value >= 0f && value <= 1f ? value : _transparency;
        }

        /// <summary>
        /// The animation's stored actions.
        /// </summary>
        private List<AnimationAction> StoredActions { get; }

        #endregion

        /// <summary>
        /// A 2D animation.
        /// </summary>
        /// <param name="parent">The <see cref="Animation"/>'s parent. Intaken as a <see cref="Animation"/></param>
        /// <param name="id">The <see cref="Animation"/>'s id. Intaken as an <see cref="int"/>.</param>
        /// <param name="name">The <see cref="Animation"/>'s name. Intaken as a <see cref="string"/>.</param>
        /// <param name="texturePath">The <see cref="Animation"/>'s texture path. Relative to the Content folder location. Intaken as a <see cref="string"/>.</param>
        /// <param name="position">The <see cref="Animation"/>'s initial position. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="width">The <see cref="Animation"/>'s width. Intaken as a <see cref="float"/>.</param>
        /// <param name="height">The <see cref="Animation"/>'s height. Intaken as a <see cref="float"/>.</param>
        /// <param name="isVisible">The <see cref="Animation"/>'s visibility. Intaken as a <see cref="bool"/>.</param>
        protected Animation(Animation parent, int id, string name, string texturePath, Vector2 position, int width, int height, bool isVisible = true) : base(parent, id, name, position, width, height, isVisible)
        {
            TexturePath = texturePath;
            StoredActions = new List<AnimationAction>();
        }

        #region Stored Actions

        /// <summary>
        /// Defines a new action for the animation to perform and stores it in memory.
        /// </summary>
        /// <param name="name">The <see cref="AnimationAction"/>'s name. Intaken as a <see cref="string"/>.</param>
        /// <param name="sourcePosition">The <see cref="AnimationAction"/>'s source position within the sprite sheet. Intaken as a <see cref="Vector2"/>.</param>
        /// <param name="frameWidth">The <see cref="AnimationAction"/>'s frame width. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameHeight">The <see cref="AnimationAction"/>'s frame height. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameCount">The <see cref="AnimationAction"/>'s frame count. Intaken as an <see cref="int"/>.</param>
        /// <param name="frameSpeedInSeconds">The <see cref="AnimationAction"/>'s frame speed in seconds. Intaken as a <see cref="float"/>.</param>
        /// <param name="loopStyle">The <see cref="AnimationAction"/>'s loop style. Intaken as a <see cref="AnimationAction.LoopStyles"/>.</param>
        /// <param name="loopLength">The <see cref="AnimationAction"/>'s loop length. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the <see cref="AnimationAction"/>'s id as an <see cref="int"/>.</returns>
        public int AddAction(string name, Vector2 sourcePosition, int frameWidth, int frameHeight, int frameCount, float frameSpeedInSeconds,
                             AnimationAction.LoopStyles loopStyle = AnimationAction.LoopStyles.Forward, int loopLength = (int)AnimationAction.LoopLengths.Infinite)
        {
            var nextActionId = 0;

            if (!Identities.ObjectExists<AnimationAction, AnimationAction>(StoredActions, name))
            {
                nextActionId = Identities.GetNextValidObjectId<AnimationAction, AnimationAction>(StoredActions);

                if (!Identities.ObjectExists<AnimationAction, AnimationAction>(StoredActions, nextActionId))
                {
                    StoredActions.Add(new AnimationAction(nextActionId, name, sourcePosition, frameWidth, frameHeight, frameCount,
                                                          frameSpeedInSeconds, loopStyle, loopLength));
                }
            }

            return nextActionId;
        }

        /// <summary>
        /// Retrieves a <see cref="AnimationAction"/>, by id.
        /// </summary>
        /// <param name="id">The id of the action to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested <see cref="AnimationAction"/>, by id, if found, otherwise null.</returns>
        public AnimationAction GetAction(int id) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, id);

        /// <summary>
        /// Retrieves an <see cref="AnimationAction"/>, by name.
        /// </summary>
        /// <param name="name">The name of the <see cref="AnimationAction"/> to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested <see cref="AnimationAction"/>, by id, if found, otherwise null.</returns>
        public AnimationAction GetAction(string name) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, name);

        /// <summary>
        /// Starts an <see cref="AnimationAction"/>, by id.
        /// </summary>
        /// <param name="id">The id of the <see cref="AnimationAction"/> to retrieve. Intaken as an <see cref="int"/>.</param>
        public void StartAction(int id) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, id).IsActive = true;

        /// <summary>
        /// Starts an <see cref="AnimationAction"/>, by name.
        /// </summary>
        /// <param name="name">The name of the <see cref="AnimationAction"/> to retrieve. Intaken as a <see cref="string"/>.</param>
        public void StartAction(string name) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, name).IsActive = true;

        /// <summary>
        /// Starts all loaded <see cref="AnimationAction"/>'s in <see cref="StoredActions"/>.
        /// </summary>
        public void StartAllActions()
        {
            foreach (var action in StoredActions)
            {
                action.IsActive = false;
            }
        }

        /// <summary>
        /// Stops an <see cref="AnimationAction"/>, by id.
        /// </summary>
        /// <param name="id">The id of the <see cref="AnimationAction"/> to retrieve. Intaken as an <see cref="int"/>.</param>
        public void StopAction(int id) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, id).IsActive = false;

        /// <summary>
        /// Stops an <see cref="AnimationAction"/>, by name.
        /// </summary>
        /// <param name="name">The name of the <see cref="AnimationAction"/> to retrieve. Intaken as a <see cref="string"/>.</param>
        public void StopAction(string name) => Identities.GetObject<AnimationAction, AnimationAction>(StoredActions, name).IsActive = false;

        /// <summary>
        /// Stops all <see cref="AnimationAction"/>'s in <see cref="StoredActions"/> that are currently active.
        /// </summary>
        public void StopAllActions()
        {
            foreach (var action in StoredActions)
            {
                action.IsActive = false;
            }
        }

        /// <summary>
        /// Removes an <see cref="AnimationAction"/>, by id.
        /// </summary>
        /// <param name="id">The id of the <see cref="AnimationAction"/> to be removed. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the <see cref="AnimationAction"/> was removed.</returns>
        public bool RemoveAction(int id) => Identities.RemoveObject<AnimationAction, AnimationAction>(StoredActions, id);

        /// <summary>
        /// Removes an <see cref="AnimationAction"/>, by name.
        /// </summary>
        /// <param name="name">The name of the action to be removed. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the action was removed.</returns>
        public bool RemoveAction(string name) => Identities.RemoveObject<AnimationAction, AnimationAction>(StoredActions, name);

        #endregion

        /// <summary>
        /// Pushes the animation on to the <see cref="MonoGameObject.HoverStack"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="MonoGameObject"/>.</typeparam>
        /// <param name="animation">The animation to push on to the <see cref="MonoGameObject.HoverStack"/>.</param>
        public override void RiseUp<T>(T animation)
        {
            if (!animation.IsStateSet(FocusStates.IsHovered))
            {
                animation.AddState(FocusStates.IsHovered);
            }

            HoverStack.Push(animation);
        }

        /// <summary>
        /// Loads the <see cref="Animation"/>'s texture and color data.
        /// </summary>
        /// <param name="content">The <see cref="ContentManager"/> for the animation.</param>
        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(TexturePath);
            
            ColorData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(ColorData);
        }

        /// <summary>
        /// The <see cref="Animation"/>'s update method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's <see cref="GameTime"/>.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                foreach (var action in StoredActions.Where(action => action.IsActive))
                {
                    //Origin = new Vector2((action.FrameWidth * Transform.WorldScale().X) / 2f, (action.FrameHeight * Transform.WorldScale().Y) / 2f);
                    //Rectangle = new RectangleF(Transform.WorldPosition().X - Origin.X,
                    //                           Transform.WorldPosition().Y - Origin.Y,
                    //                           action.FrameWidth * Transform.WorldScale().X,
                    //                           action.FrameHeight * Transform.WorldScale().Y);

                    //ExtendedRectangle.Join(Rectangle);

                    action.Update(gameTime);
                }

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// The <see cref="Animation"/>'s draw method.
        /// </summary>
        /// <param name="spriteBatch">Intakes a MonoGame <see cref="SpriteBatch"/>.</param>
        /// <param name="transform">Intakes a <see cref="Matrix"/>.</param>
        public override void Draw(SpriteBatch spriteBatch, Matrix transform = default)
        {
            if (IsActive)
            {
                foreach (var action in StoredActions.Where(action => action.IsActive))
                {
                    spriteBatch.Draw(Texture, Vector2.Transform(Transform.WorldPosition(), transform), action.SourceRectangle, Color.White * Transparency, Transform.Rotation, Origin, Transform.Scale, SpriteEffects.None, 1);
                }
            }
        }
    }
}