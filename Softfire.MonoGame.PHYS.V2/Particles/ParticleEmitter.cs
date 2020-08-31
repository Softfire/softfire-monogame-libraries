using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.PHYS.Particles
{
    public class ParticleEmitter
    {
        /// <summary>
        /// Particle Emitter Quantity.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Random Number Generator.
        /// </summary>
        private Random Random { get; }

        /// <summary>
        /// Particle Emitter Location.
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Particles.
        /// </summary>
        private List<Particle> Particles { get; }

        /// <summary>
        /// Textures.
        /// </summary>
        private List<Texture2D> Textures { get; }

        public ParticleEmitter(List<Texture2D> textures, Vector2 location, int quantity)
        {
            Textures = textures;
            Location = location;
            Quantity = quantity;

            Particles = new List<Particle>();
            Random = new Random();
        }

        public Vector2 ParticleDispersalVelocity()
        {
            var velocity = new Vector2();

            return velocity;
        }

        private Particle GenerateNewParticle()
        {
            var texture = Textures[Random.Next(Textures.Count)];
            var velocity = 1f * (float)(Random.NextDouble() * 2 - 1);
            var angle = 0;
            var angularVelocity = 0.1f * (float)(Random.NextDouble() * 2 - 1);
            var color = new Color((float)Random.NextDouble(),
                                  (float)Random.NextDouble(),
                                  (float)Random.NextDouble());
            var size = new Vector2((float)Random.NextDouble());
            var lifeSpanInSeconds = 20 + Random.Next(40);

            return new Particle(texture, Location, velocity, size, angle, angularVelocity, color, lifeSpanInSeconds);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < Quantity; i++)
            {
                Particles.Add(GenerateNewParticle());
            }

            for (var particle = 0; particle < Particles.Count; particle++)
            {
                Particles[particle].Update(gameTime);

                if (Particles[particle].LifeSpanInSeconds <= 0)
                {
                    Particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (var index = 0; index < Particles.Count; index++)
            {
                Particles[index].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}