using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.ANIM
{
    /// <summary>
    /// The Animation Class.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Animation Content Manager.
        /// </summary>
        public static ContentManager Content { private get; set; }

        /// <summary>
        /// Delta Time.
        /// The time between updates in Seconds.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Animation's Color Data.
        /// Used for collision detection by comparing pixels in the CD DLL.
        /// </summary>
        public Color[] ColorData { get; private set; }

        /// <summary>
        /// Animation's Texture.
        /// Texture used for animation.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Animtaion's Texture Path.
        /// Path to the texture relative to folder strucure in Content Folder of Main project.
        /// Ex. "Assets/Sprites/Player"
        /// File extension is left out. Example file is Player.png.
        /// </summary>
        private string TexturePath { get; }

        /// <summary>
        /// Animation's Rectangle.
        /// Rectangle used for collision detection, draw area and boundaries.
        /// </summary>
        public Rectangle Rectangle { get; private set; }

        /// <summary>
        /// Animation's Origin.
        /// Center of Animation Texture.
        /// Used for positioning the animation relative to it's center.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Animation's Rotation Angle.
        /// Rotation Angle is used to rotate the Animation and is in Radians.
        /// Point of rotation is based on the Origin.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// Animation's Internal Transparnecy Value.
        /// Default is 1.0f.
        /// </summary>
        private float _transparency;

        /// <summary>
        /// Animation's Transparency.
        /// Transparency can be added to the Texture by passing a float between 0.0f and 1.0f.
        /// 1.0f is zero transparency and 0.0f is full transparency.
        /// Default is 1.0f.
        /// </summary>
        public float Transparency
        {
            get => _transparency;
            set => _transparency = MathHelper.Clamp(value, 0.0f, 1.0f);
        }

        /// <summary>
        /// Animation's Draw Depth.
        /// Used to draw the animation at certain depths to allow it to be drawn in front or behind other objects at varying depths.
        /// Float values will differ depending on the SpriteBatch settings in effect.
        /// Default is 1.0f.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Animation's Internal Scale Value as a Vector2.
        /// Default X and Y are 1.0f.
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// Animation's Texture Scale.
        /// Scale is used to enlarge or shrink the Texture size.
        /// Default is 1.0f.
        /// </summary>
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                if (value.X > 0 &&
                    value.Y > 0)
                {
                    _scale = value;
                }
            }
        }
        
        /// <summary>
        /// Animation's Position.
        /// Vector based positioning.
        /// Uses Origin in calculations.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Animation Velocity.
        /// The Animation's speed in a given direction.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Animation Movement Speed.
        /// The rate at which the Animation covers distance.
        /// </summary>
        public double Acceleration { get; set; }

        /// <summary>
        /// Animation Actions.
        /// </summary>
        private Dictionary<string, AnimationAction> Actions { get; }

        /// <summary>
        /// Current Action.
        /// </summary>
        private AnimationAction CurrentAction { get; set; }
        
        /// <summary>
        /// Animation's Constructor.
        /// </summary>
        /// <param name="texturePath">Intakes a string used to define where the texture should be loaded from. Relative path used in Content.</param>
        public Animation(string texturePath)
        {
            TexturePath = texturePath;
            Depth = 1.0f;
            Transparency = 1.0f;
            Scale = Vector2.One;
            
            Actions = new Dictionary<string, AnimationAction>();

            LoadContent();
        }

        /// <summary>
        /// Add Action.
        /// </summary>
        /// <param name="identifier">The action's unique identifier. Intaken as a string.</param>
        /// <param name="action">The action to add.</param>
        /// <returns></returns>
        public bool AddAction(string identifier, AnimationAction action)
        {
            var result = false;

            if (Actions.ContainsKey(identifier) == false)
            {
                Actions.Add(identifier, action);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Action.
        /// </summary>
        /// <param name="identifier">The action's unique identifier. Intaken as a string.</param>
        /// <returns>Returns the requested AnimationAction, if found, ortherwise null.</returns>
        public AnimationAction GetAction(string identifier)
        {
            AnimationAction result = null;

            if (Actions.ContainsKey(identifier))
            {
                result = Actions[identifier];
            }

            return result;
        }

        /// <summary>
        /// Set Action.
        /// </summary>
        /// <param name="identifier">The action's unique identifier. Intaken as a string.</param>
        public void SetAction(string identifier)
        {
            AnimationAction action;

            if ((action = GetAction(identifier)) != null)
            {
                CurrentAction = action;
            }
        }

        /// <summary>
        /// Remove Action.
        /// </summary>
        /// <param name="identifier">The action's unique identifier. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the AnimationAction was removed.</returns>
        public bool RemoveAction(string identifier)
        {
            var result = false;

            if (Actions.ContainsKey(identifier))
            {
                Actions.Remove(identifier);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Animation Acceleration.
        /// Acceleration is the increase in rate at which an object changes its velocity.
        /// </summary>
        /// <param name="increment">Intakes a positive double.</param>
        public void Accelerate(double increment)
        {
            Acceleration += increment;
        }

        /// <summary>
        /// Animation Acceleration with limit.
        /// Acceleration is the increase in rate at which an object changes its velocity.
        /// </summary>
        /// <param name="increment">Intakes a positive double.</param>
        /// <param name="limit">Intakes a positive double defining the speed limit.</param>
        public void Accelerate(double increment, double limit)
        {
            if (Acceleration + increment <= limit)
            {
                Acceleration += increment * DeltaTime;
            }
        }

        /// <summary>
        /// Animation Deceleration.
        /// Deceleration is the decrease in rate at which an object changes its velocity.
        /// </summary>
        /// <param name="decrement">Intakes a positive double.</param>
        public void Decelerate(double decrement)
        {
            Acceleration -= decrement;
        }

        /// <summary>
        /// Animation Deceleration with limit.
        /// Deceleration is the decrease in rate at which an object changes its velocity.
        /// </summary>
        /// <param name="decrement">Intakes a positive double.</param>
        /// <param name="limit">Intakes a positive double defining the speed limit.</param>
        public void Decelerate(double decrement, double limit)
        {
            if (Acceleration - decrement >= -limit)
            {
                Acceleration -= decrement * DeltaTime;
            }
        }

        /// <summary>
        /// Calculate Velocity.
        /// Calculates velocity based on RotationAngle and Acceleration.
        /// Use ApplyVelocity once calculated.
        /// </summary>
        public void CalculateVelocity()
        {
            Velocity = new Vector2((float)Math.Sin(RotationAngle) * (float)Acceleration, -(float)Math.Cos(RotationAngle) * (float)Acceleration);
        }

        /// <summary>
        /// Calculate Velocity.
        /// Calculates velocity based on rotation angle provided and Acceleration.
        /// Use ApplyVelocity once calculated.
        /// </summary>
        /// <param name="angle">Intakes a rotation angle, in Degrees, as a double.</param>
        public void CalculateVelocity(double angle)
        {
            angle = Math.PI / 180 * angle;

            Velocity = new Vector2((float)Math.Sin(angle) * (float)Acceleration, -(float)Math.Cos(angle) * (float)Acceleration);
        }

        /// <summary>
        /// Apply Velocity.
        /// Can be used with CalculateVelocity() to apply Velocity to Position.
        /// </summary>
        public void ApplyVelocity()
        {
            Position += Velocity;
        }

        /// <summary>
        /// Rotate Towards.
        /// Calculates and applies the rotation angle required, in Radians, to rotate the Animation towards the position given.
        /// </summary>
        /// <param name="targetVector">Intakes a target object's position.</param>
        public void RotateTowards(Vector2 targetVector)
        {
            RotationAngle = -(float)Math.Atan2(Position.X - targetVector.X, Position.Y - targetVector.Y);
        }

        /// <summary>
        /// Rotate Away.
        /// Calculates and applies the rotation angle required, in Radians, to rotate the Animation away from the position given.
        /// </summary>
        /// <param name="targetVector">Intakes a target object's position.</param>
        public void RotateAway(Vector2 targetVector)
        {
            RotationAngle = -(float)Math.Atan2(targetVector.X - Position.X, targetVector.Y - Position.Y);
        }

        /// <summary>
        /// Rotate With.
        /// Applies the rotation angle required, in Radians, to rotate the Animation at the angle given.
        /// </summary>
        /// <param name="parentRotationAngle">Intakes a rotation angle in Radians as a float.</param>
        public void RotateWith(float parentRotationAngle)
        {
            RotationAngle = parentRotationAngle;
        }

        /// <summary>
        /// Rotate Around.
        /// Calculates and rotates the Animation around another object by modifying the Animation's Position.
        /// </summary>
        /// <param name="rotationPoint">The parent position to rotate around.</param>
        /// <param name="parentRotationAngle">The parent's rotation angle in Radians.</param>
        /// <param name="positionalOffset">Relative positional offset from parent's center.</param>
        public void RotateAround(Vector2 rotationPoint, double parentRotationAngle, Vector2 positionalOffset = new Vector2())
        {
            var cosTheta = Math.Cos(parentRotationAngle);
            var sinTheta = Math.Sin(parentRotationAngle);

            Position = new Vector2
            {
                X =
                    (int)
                    (cosTheta * (Position.X + positionalOffset.X - rotationPoint.X) -
                     sinTheta * (Position.Y + positionalOffset.Y - rotationPoint.Y) + rotationPoint.X),
                Y =
                    (int)
                    (sinTheta * (Position.X + positionalOffset.X - rotationPoint.X) +
                     cosTheta * (Position.Y + positionalOffset.Y - rotationPoint.Y) + rotationPoint.Y)
            };
        }

        /// <summary>
        /// Rotates the Animation Clockwise by the angle given.
        /// </summary>
        /// <param name="angle">Intakes an angle, in Degrees, as a double.</param>
        public void RotateClockwise(double angle)
        {
            RotationAngle += (float)(Math.PI / 180 * angle);
        }

        /// <summary>
        /// Rotates the Animation Counter Clockwise by the angle given.
        /// </summary>
        /// <param name="angle">Intakes an angle, in Degrees, as a double.</param>
        public void RotateCounterClockwise(double angle)
        {
            RotationAngle -= (float)(Math.PI / 180 * angle);
        }

        /// <summary>
        /// Confine Within Boundaries.
        /// Confines the Animation within the Rectangle provided by modifying the Position.
        /// If Velocity is used it will set the correct Velocity axis to zero.
        /// </summary>
        /// <param name="worldRectangle"></param>
        public void ConfineWithinBoundaries(Rectangle worldRectangle)
        {
            // Top
            if (Position.Y < worldRectangle.Y + Origin.Y)
            {
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Vector2(Position.X, worldRectangle.Y + Origin.Y);
            }

            // Right
            if (Position.X > worldRectangle.Width - Origin.X)
            {
                Velocity = new Vector2(0, Velocity.Y);
                Position = new Vector2(worldRectangle.Width - Origin.X, Position.Y);
            }

            // Bottom
            if (Position.Y > worldRectangle.Height - Origin.Y)
            {
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Vector2(Position.X, worldRectangle.Height - Origin.Y);
            }

            // Left
            if (Position.X < worldRectangle.X + Origin.X)
            {
                Velocity = new Vector2(0, Velocity.Y);
                Position = new Vector2(worldRectangle.X + Origin.X, Position.Y);
            }
        }

        /// <summary>
        /// Load Content Method.
        /// Used to load the Animation's Texture and determine the FrameWidth, FrameHeight, ColorData and retrieve the ColorData from the Texture.
        /// </summary>
        public void LoadContent()
        {
            Texture = Content.Load<Texture2D>(TexturePath);
            
            ColorData = new Color[Texture.Width * Texture.Height];
            Texture.GetData(ColorData);
        }

        /// <summary>
        /// Update Method.
        /// Used to update the Animation's attributes.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame's GameTime.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (CurrentAction != null)
            {
                Origin = new Vector2((CurrentAction.FrameWidth * Scale.X) / 2f, (CurrentAction.FrameHeight * Scale.Y) / 2f);
                Rectangle = new Rectangle((int)(Position.X - Origin.X), (int)(Position.Y - Origin.Y), (int)(CurrentAction.FrameWidth * Scale.X), (int)(CurrentAction.FrameHeight * Scale.Y));

                CurrentAction.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw Method.
        /// Used to draw the Animation's Texture.
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentAction != null)
            {
                spriteBatch.Draw(Texture, Position, CurrentAction.SourceRectangle, Color.White, RotationAngle, Origin, Scale, SpriteEffects.None, Depth);
            }
        }
    }
}