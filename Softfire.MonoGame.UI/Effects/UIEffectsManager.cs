using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Softfire.MonoGame.UI.Effects
{
    public class UIEffectsManager
    {
        /// <summary>
        /// Loaded Effects.
        /// </summary>
        private Dictionary<string, UIEffectBase> LoadedEffects { get; }

        /// <summary>
        /// Effects.
        /// </summary>
        private List<UIEffectBase> ActiveEffects { get; }

        /// <summary>
        /// Are Effects Running?
        /// </summary>
        public bool AreEffectsRunning { get; protected set; }

        /// <summary>
        /// Activate Effects.
        /// Call to perform any Effects that have been loaded.
        /// </summary>
        public bool ActivateEffects { get; set; }

        /// <summary>
        /// Are Effects Run In Sequencial Order?
        /// </summary>
        public bool AreEffectsRunInSequentialOrder { get; set; } = true;

        /// <summary>
        /// UI Effecs Manager.
        /// </summary>
        public UIEffectsManager()
        {
            LoadedEffects = new Dictionary<string, UIEffectBase>();
            ActiveEffects = new List<UIEffectBase>();
        }

        /// <summary>
        /// Load Effect.
        /// Adds the provided Effect to the UIBase's Loaded Effects Dictionary and modifying the Order Number to be equal to the number of current effects, if Order Number is found to be 0.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to run. Intaken as a string.</param>
        /// <param name="effect">The Effect to be loaded.</param>
        /// <returns>Returns a bool indicating whether the Effect was added.</returns>
        public bool LoadEffect(string identifier, UIEffectBase effect)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier) == false)
            {
                if (effect.OrderNumber == 0)
                {
                    effect.OrderNumber = ActiveEffects.Count + 1;
                }

                LoadedEffects.Add(identifier, effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to retrieve. Intaken as a string.</param>
        /// <returns>Returns a Effect, if found, otherwise null.</returns>
        public UIEffectBase GetLoadedEffect(string identifier)
        {
            UIEffectBase result = null;

            if (CheckForLoadedEffect(identifier))
            {
                result = LoadedEffects[identifier];
            }

            return result;
        }

        /// <summary>
        /// Check For Loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the Effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect has been loaded.</returns>
        public bool CheckForLoadedEffect(string identifier)
        {
            return LoadedEffects.ContainsKey(identifier);
        }

        /// <summary>
        /// Remove Effect.
        /// Removes the Effect from Loaded Effects using the provided identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to remove. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect was removed.</returns>
        public bool RemoveEffect(string identifier)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier))
            {
                result = LoadedEffects.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Check For Activated Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the Effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect is currently active.</returns>
        public bool CheckForActivatedEffect(string identifier)
        {
            return ActiveEffects.Contains(GetLoadedEffect(identifier));
        }

        /// <summary>
        /// Activate Loaded Effect.
        /// Called to activate a loaded Effect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Effect to activate. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the Effect was activated.</returns>
        public bool ActivateLoadedEffect(string identifier)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier))
            {
                ActiveEffects.Add(GetLoadedEffect(identifier));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Activate Immediate Effect.
        /// Called to activate the passed effect immediately without storing it for reuse.
        /// </summary>
        /// <param name="effect">The UIBaseEffect object to activate immediately.</param>
        /// <returns>Returns a boolean indicating whether the effect was activated.</returns>
        public bool ActivateImmediateEffect(UIEffectBase effect)
        {
            var result = false;

            if (effect != null)
            {
                ActiveEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Run Effects.
        /// Called to run all loaded Effects by ascending Order Number.
        /// </summary>
        /// <returns>Returns a bool indicating whether all of the loaded transitions completed.</returns>
        public async Task<bool> RunActiveEffects()
        {
            var result = false;

            if (AreEffectsRunInSequentialOrder)
            {
                if (ActiveEffects.Count > 0)
                {
                    var currentEffect = ActiveEffects[0];
                    if (await currentEffect.Run())
                    {
                        currentEffect.Reset();
                        ActiveEffects.Remove(currentEffect);
                        AreEffectsRunning = false;
                    }
                    else
                    {
                        AreEffectsRunning = true;
                    }
                }
            }
            else
            {
                for (var index = 0; index < ActiveEffects.Count; index++)
                {
                    var activeEffect = ActiveEffects[index];

                    if (await activeEffect.Run())
                    {
                        activeEffect.Reset();
                        ActiveEffects.Remove(activeEffect);
                        AreEffectsRunning = false;
                    }
                    else
                    {
                        AreEffectsRunning = true;
                    }
                }
            }

            if (ActiveEffects.Count == 0)
            {
                ActivateEffects = false;
                result = true;
            }

            return result;
        }
    }
}