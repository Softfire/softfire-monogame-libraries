using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Softfire.MonoGame.SND
{
    public class Effect
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Effect Data.
        /// </summary>
        public SoundEffect Data { get; set; }

        /// <summary>
        /// Effect Instances.
        /// </summary>
        public Dictionary<string, List<SoundEffectInstance>> EffectInstances { get; }

        /// <summary>
        /// File Path.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Effect Constructor.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="filePath">Intakes a file path for the Effect. Relative to the Content Manager.</param>
        public Effect(string identifier, string filePath)
        {
            Identifier = identifier;
            FilePath = filePath;

            EffectInstances = new Dictionary<string, List<SoundEffectInstance>>();
        }

        /// <summary>
        /// Load Content.
        /// Loads the Eeffect into the provided Content Manager.
        /// </summary>
        /// <param name="content">Intakes a ContentManager.</param>
        public void LoadContent(ContentManager content)
        {
            Data = content.Load<SoundEffect>(FilePath);
            Data.Name = Identifier;
        }

        /// <summary>
        /// Create Effect Instance.
        /// Sound Effect Instances can be paused, resumed, have their volume, pitch and pan adjusted during playback.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="numberOfInstances">Intakes the number of instances of this Effect to create as an int. Default is 16.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string CreateEffectInstance(string identifier, int numberOfInstances = 16)
        {
            string result;

            if (EffectInstances.ContainsKey(identifier) == false)
            {
                var sfxInstances = new List<SoundEffectInstance>(numberOfInstances);

                for (var i = 0; i < numberOfInstances; i++)
                {
                    sfxInstances.Add(Data.CreateInstance());
                }

                EffectInstances.Add(identifier, sfxInstances);

                result = $"Sound Effect Instance: '{identifier}' created!";
            }
            else
            {
                result = $"Sound Effect Instance: '{identifier}' already exists!";
            }

            return result;
        }

        /// <summary>
        /// Get Effect Instances.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns a List of SoundEffectInstance.</returns>
        public List<SoundEffectInstance> GetEffectInstances(string identifier)
        {
            List<SoundEffectInstance> sfxInstances = null;

            if (EffectInstances.ContainsKey(identifier))
            {
                sfxInstances = EffectInstances[identifier];
            }

            return sfxInstances;
        }

        /// <summary>
        /// Remove Effect Instance.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string RemoveEffectInstance(string identifier)
        {
            var result = $"Sound Effect Instance: '{identifier}' was not found!";

            if (EffectInstances.ContainsKey(identifier))
            {
                EffectInstances.Remove(identifier);
                result = $"Sound Effect Instance: '{identifier}' has been removed!";
            }

            return result;
        }

        /// <summary>
        /// Play Sound Effect Instances.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <param name="volume">Intakes a float between 0.0f and 1.0f to set the volume of the Effect. Default is 1.0f.</param>
        /// <param name="pitch">Intakes a float between -1.0f and 1.0f to set the pitch of the Effect. Default is 0.0f.</param>
        /// <param name="pan">Intakes a float between -1.0f and 1.0f to set the pan of the Effect. Pan is the audio balance between stereo speakers. -1.0 is left, 0 is both and 1 is right. Default is 0.0f.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string Play(string identifier, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            List<SoundEffectInstance> sfxInstanceList;
            var result = $"Sound Effect Instance: '{identifier}' was not found!";

            if ((sfxInstanceList = GetEffectInstances(identifier)) != null)
            {
                var sfxInstance = sfxInstanceList.First(sfx => sfx.State == SoundState.Stopped);

                ClampSoundValues(ref volume, ref pitch, ref pan);

                sfxInstance.Volume = volume;
                sfxInstance.Pitch = pitch;
                sfxInstance.Pan = pan;
                sfxInstance.Play();

                result = $"Sound Effect Instance: '{identifier}' is now playing!";
            }

            return result;
        }

        /// <summary>
        /// Pause Effect.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string Pause(string identifier)
        {
            List<SoundEffectInstance> sfxInstanceList;
            var result = $"Sound Effect Instance: '{identifier}' was not found!";

            if ((sfxInstanceList = GetEffectInstances(identifier)) != null)
            {
                foreach (var instance in sfxInstanceList.Where(sfx => sfx.State == SoundState.Playing))
                {
                    instance.Pause();
                }

                result = $"Sound Effect Instance: '{identifier}' is now paused!";
            }

            return result;
        }

        /// <summary>
        /// Resume Track.
        /// </summary>
        /// <param name="identifier">Intakes a unique identifier as a string.</param>
        /// <returns>Returns a message of the results of the action.</returns>
        public string Resume(string identifier)
        {
            List<SoundEffectInstance> sfxInstanceList;
            var result = $"Sound Effect Instance: '{identifier}' was not found!";

            if ((sfxInstanceList = GetEffectInstances(identifier)) != null)
            {
                foreach (var instance in sfxInstanceList.Where(sfx => sfx.State == SoundState.Paused))
                {
                    instance.Resume();
                }
                result = $"Sound Effect Instance: '{identifier}'  has resumed playing!";
            }

            return result;
        }

        /// <summary>
        /// Clamp Sound Values.
        /// </summary>
        /// <param name="volume">Intakes volume as a reference of a float.</param>
        /// <param name="pitch">Intakes pitch as a reference of a float.</param>
        /// <param name="pan">Intakes pan as a reference of a float.</param>
        public static void ClampSoundValues(ref float volume, ref float pitch, ref float pan)
        {
            volume = MathHelper.Clamp(volume, 0f, 1f);
            pitch = MathHelper.Clamp(pitch, -1f, 1f);
            pan = MathHelper.Clamp(pan, -1f, 1f);
        }
    }
}
