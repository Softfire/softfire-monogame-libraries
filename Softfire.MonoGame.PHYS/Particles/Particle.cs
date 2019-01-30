using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.PHYS.Particles
{
    public class Particle
    {
        /// <summary>
        /// Delta Time.
        /// Time since last update.
        /// </summary>
        private double DeltaTime { get; set; }

        /// <summary>
        /// Particle Texture.
        /// </summary>
        private Texture2D Texture { get; set; }

        /// <summary>
        /// Particle Position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Particle Rectangle.
        /// </summary>
        private Rectangle Rectangle { get; set; }

        /// <summary>
        /// Particle Origin.
        /// Center of Texture.
        /// </summary>
        private Vector2 Origin { get; set; }
        
        /// <summary>
        /// Particle Acceleration.
        /// </summary>
        public double Acceleration { get; set; }

        /// <summary>
        /// Particle Velocity.
        /// Direction and Speed of Particle.
        /// </summary>
        private Vector2 Velocity { get; set; }

        /// <summary>
        /// Particle Angle.
        /// </summary>
        public float RotationAngle { get; set; }

        /// <summary>
        /// Particle Rotation Acceleration.
        /// </summary>
        public double RotationAcceleration { get; set; }

        /// <summary>
        /// Particle Rotation Velocity
        /// Direction and Speed of Rotation.
        /// </summary>
        private double RotationVelocity { get; set; }

        /// <summary>
        /// Particle Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Particle Scale.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Particle Life Span In Seconds.
        /// </summary>
        public double LifeSpanInSeconds { get; set; }

        public Particle(Texture2D texture, Vector2 position, double acceleration, Vector2 scale, float rotationAngle = 0f,
                        double rotationAcceleration = 0, Color? color = null, double lifeSpanInSeconds = 1.0)
        {
            Texture = texture;
            Position = position;
            Acceleration = acceleration;
            Scale = scale.X > 0 && scale.Y > 0 ? scale : new Vector2(1, 1);

            RotationAngle = rotationAngle;
            RotationAcceleration = rotationAcceleration;
            Color = color ?? Color.White;
            LifeSpanInSeconds = lifeSpanInSeconds;
        }

        /// <summary>
        /// Calculate Velocity.
        /// Calculates velocity based on RotationAngle and Acceleration.
        /// Use ApplyVelocity once calculated.
        /// </summary>
        private void CalculateVelocity()
        {
            Velocity = new Vector2((float)Math.Sin(RotationAngle) * (float)Acceleration, -(float)Math.Cos(RotationAngle) * (float)Acceleration);
            RotationVelocity = RotationAngle * RotationAcceleration / DeltaTime;
        }

        /// <summary>
        /// Particle Update Method.
        /// </summary>
        /// <param name="gameTime">Intakes MonoGame GameTime.</param>
        public void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity;
            RotationAngle += (float)RotationVelocity;

            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            CalculateVelocity();

            LifeSpanInSeconds -= DeltaTime;
        }

        /// <summary>
        /// Particle Draw Method.
        /// </summary>
        /// <param name="spriteBatch">Intakes MonoGame SpriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Rectangle, Color, RotationAngle, Origin, Scale, SpriteEffects.None, 0f);
        }
    }
}