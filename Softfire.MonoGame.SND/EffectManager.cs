using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Softfire.MonoGame.SND
{
    public class EffectManager
    {
        /// <summary>
        /// Effect Catalogue.
        /// </summary>
        public Dictionary<string, Effect> Catalogue { get; }

        /// <summary>
        /// Current Volume Level.
        /// </summary>
        public float CurrentVolumeLevel { get; private set; }

        /// <summary>
        /// Effect Manager Constructor.
        /// </summary>
        public EffectManager()
        {
            Catalogue = new Dictionary<string, Effect>();

            // Set Volume.
            Volume(0.10f);
        }

        /// <summary>
        /// Get Effect From Catalogue.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns an Effect.</returns>
        public Effect GetEffectFromCatalogue(string identifier)
        {
            Effect result = null;

            if (Catalogue.ContainsKey(identifier))
            {
                result = Catalogue[identifier];
            }

            return result;
        }

        /// <summary>
        /// Play From Catalogue.
        /// Plays an Effect directly from the Catalogue.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="volume">Intakes a float between 0.0f and 1.0f to set the volume of the Effect. Default is 1.0f.</param>
        /// <param name="pitch">Intakes a float between -1.0f and 1.0f to set the pitch of the Effect. Default is 0.0f.</param>
        /// <param name="pan">Intakes a float between -1.0f and 1.0f to set the pan of the Effect. Pan is the audio balance between stereo speakers. -1.0 is left, 0 is both and 1 is right. Default is 0.0f.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string PlayDirectFromCatalogue(string identifier, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            Effect sfx;
            var result = $"Sound Effect: '{identifier}' was not found!";

            if ((sfx = GetEffectFromCatalogue(identifier)) != null)
            {
                Effect.ClampSoundValues(ref volume, ref pitch, ref pan);
                sfx.Data.Play(volume, pitch, pan);
                result = $"Sound Effect: '{identifier}' is now playing!";
            }

            return result;
        }

        /// <summary>
        /// Volume.
        /// Sets Master Volume. All SoundEffect and Instance volumes are relative to this volume level.
        /// Default is 0.10f.
        /// </summary>
        /// <param name="adjustment">Intakes a positive or negative float to modify the volume.</param>
        /// <returns>Returns the current volume.</returns>
        public float Volume(float adjustment)
        {
            SoundEffect.MasterVolume = CurrentVolumeLevel += MathHelper.Clamp(CurrentVolumeLevel += adjustment, 0f, 1.0f);

            return CurrentVolumeLevel;
        }
    }
}
