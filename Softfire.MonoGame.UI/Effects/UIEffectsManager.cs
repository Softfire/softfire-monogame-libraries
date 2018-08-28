using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Softfire.MonoGame.UI.Effects
{
    public class UIEffectsManager
    {
        /// <summary>
        /// Currently loaded effects.
        /// </summary>
        /// <see cref="LoadEffect(string, UIEffectBase)"/>
        /// <see cref="ActivateLoadedEffect(string)"/>
        /// <see cref="GetLoadedEffect(string)"/>
        /// <seealso cref="RemoveLoadedEffect(string)"/>
        /// <remarks>A Dictionary containing all loaded effects. Effects can be loaded using {LoadEffect(string, UIEffectBase)} and can be activated with the {ActivateLoadedEffect(string)} method.</remarks>
        private Dictionary<string, UIEffectBase> LoadedEffects { get; }

        /// <summary>
        /// Currently active effects.
        /// </summary>
        /// <see cref="ActivateLoadedEffect(string)"/>
        /// <seealso cref="ActivateImmediateEffect(UIEffectBase)"/>
        private List<UIEffectBase> ActiveEffects { get; }

        /// <summary>
        /// Are effects running? Updated on each update.
        /// </summary>
        /// <remarks>It's good to check this to ensure actions such as adding effects do not get duplicated as this property is updated shortly after detecting if the ActiveEffectslist count is above zero.</remarks>
        public bool AreEffectsRunning { get; private set; }

        /// <summary>
        /// Are effects run in sequencial order?
        /// </summary>
        /// <remarks>Setting to false allows all active effects to run simultaneously as opposed to running one after the next.</remarks>
        public bool AreEffectsRunInSequentialOrder { get; set; } = true;

        /// <summary>
        /// Are effects sorted by order number?
        /// </summary>
        /// <remarks>By default the effects are run in the order they are loaded. Enabling this boolean will sort the effects by their order number in ascending order. This may have side effects. For instance if the same loaded effect is activated it will be sorted in ascending order thus making the second effect in line with the first effect as they have the same order number.</remarks>
        public bool AreEffectsSortedByOrderNumber { get; set; }

        /// <summary>
        /// The UI's effects manager.
        /// </summary>
        public UIEffectsManager()
        {
            LoadedEffects = new Dictionary<string, UIEffectBase>();
            ActiveEffects = new List<UIEffectBase>();
        }

        /// <summary>
        /// Adds the provided effect to the UIBase's loaded effects dictionary and modifying the order number to be equal to the number of current effects, if order number is found to be 0.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the effect to run. Intaken as a string.</param>
        /// <param name="effect">The effect to be loaded.</param>
        /// <returns>Returns a bool indicating whether the effect was added.</returns>
        public bool LoadEffect(string identifier, UIEffectBase effect)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier) == false)
            {
                if (effect.OrderNumber == 0)
                {
                    effect.OrderNumber = LoadedEffects.Count + 1;
                }

                LoadedEffects.Add(identifier, effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets the loaded effect by the identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the effect to retrieve. Intaken as a string.</param>
        /// <returns>Returns an effect, if found, otherwise null.</returns>
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
        /// Removes the effect from loaded effects by the identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the effect to remove. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveLoadedEffect(string identifier)
        {
            var result = false;

            if (CheckForLoadedEffect(identifier))
            {
                result = LoadedEffects.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Checks for a loaded effect by the identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect has been loaded.</returns>
        public bool CheckForLoadedEffect(string identifier)
        {
            return LoadedEffects.ContainsKey(identifier);
        }

        /// <summary>
        /// Call to activate a loaded effect by the identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the effect to activate. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
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
        /// Gets the specified effect by the identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier of the effect to find.</param>
        /// <returns>Returns the specified effect, if present, otherwise null.</returns>
        public UIEffectBase GetActivatedEffect(string identifier)
        {
            UIEffectBase result = null;

            if (CheckForActivatedEffect(identifier))
            {
                result = ActiveEffects.FirstOrDefault(effect => effect.Name == identifier);
            }

            return result;
        }

        /// <summary>
        /// Checks for an active Eeffect.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect is currently active.</returns>
        public bool CheckForActivatedEffect(string identifier)
        {
            return ActiveEffects.Contains(GetLoadedEffect(identifier));
        }

        /// <summary>
        /// Immediately activate an effect.
        /// </summary>
        /// <param name="effect">The UIBaseEffect object to activate immediately.</param>
        /// <returns>Returns a boolean indicating whether the effect was activated.</returns>
        /// <remarks>The passed in effect is not stored during this call and are treated as one time use effects.</remarks>
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
        /// Called to run all loaded effects.
        /// </summary>
        /// <returns>Returns a bool indicating whether all of the loaded transitions completed.</returns>
        public async Task RunActiveEffects()
        {
            if (ActiveEffects.Count > 0)
            {
                AreEffectsRunning = true;

                if (AreEffectsSortedByOrderNumber)
                {
                    if (AreEffectsRunInSequentialOrder)
                    {
                        var currentEffect = ActiveEffects.OrderBy(effect => effect.OrderNumber).ToList()[0];

                        if (await currentEffect.Run())
                        {
                            currentEffect.Reset();
                            ActiveEffects.RemoveAt(0);
                        }
                    }
                    else
                    {
                        foreach (var activeEffect in ActiveEffects.OrderBy(effect => effect.OrderNumber).ToList())
                        {
                            if (await activeEffect.Run())
                            {
                                activeEffect.Reset();
                                ActiveEffects.Remove(activeEffect);
                            }
                        }
                    }
                }
                else
                {
                    if (AreEffectsRunInSequentialOrder)
                    {
                        var currentEffect = ActiveEffects[0];

                        if (await currentEffect.Run())
                        {
                            currentEffect.Reset();
                            ActiveEffects.RemoveAt(0);
                        }
                    }
                    else
                    {
                        foreach (var activeEffect in ActiveEffects.ToList())
                        {
                            if (await activeEffect.Run())
                            {
                                activeEffect.Reset();
                                ActiveEffects.Remove(activeEffect);
                            }
                        }
                    }
                }
            }
            else
            {
                AreEffectsRunning = false;
            }
        }
    }
}