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
        /// <see cref="LoadEffect{T}"/>
        /// <see cref="ActivateLoadedEffect(int)"/>
        /// <see cref="ActivateLoadedEffect(string)"/>
        /// <see cref="CheckForLoadedEffect(int)"/>
        /// <see cref="CheckForLoadedEffect(string)"/>
        /// <see cref="GetLoadedEffect(int)"/>
        /// <see cref="GetLoadedEffect(string)"/>
        /// <seealso cref="RemoveLoadedEffect(int)"/>
        /// <seealso cref="RemoveLoadedEffect(string)"/>
        /// <remarks>A Dictionary containing all loaded effects. Effects can be loaded using {LoadEffect(string, UIEffectBase)} and can be activated with the {ActivateLoadedEffect(string)} method.</remarks>
        private List<UIEffectBase> LoadedEffects { get; }

        /// <summary>
        /// Currently active effects.
        /// </summary>
        /// <see cref="ActivateEffect{T}"/>
        /// <see cref="CheckForActivatedEffect(int)"/>
        /// <see cref="CheckForActivatedEffect(string)"/>
        /// <see cref="GetActivatedEffect(int)"/>
        /// <see cref="GetActivatedEffect(string)"/>
        private List<UIEffectBase> ActiveEffects { get; }

        /// <summary>
        /// The currently active effect.
        /// </summary>
        public UIEffectBase CurrentEffect { get; private set; }

        /// <summary>
        /// Are effects running? Updated on each update.
        /// </summary>
        /// <remarks>It's good to check this to ensure actions such as adding effects do not get duplicated as this property is updated shortly after detecting if the ActiveEffectslist count is above zero.</remarks>
        public bool AreEffectsRunning { get; private set; }

        /// <summary>
        /// Are effects run in sequencial order?
        /// </summary>
        /// <remarks>Setting to false allows all active effects to run simultaneously as opposed to running one after the next.</remarks>
        /// <value>true</value>
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
            LoadedEffects = new List<UIEffectBase>();
            ActiveEffects = new List<UIEffectBase>();
        }

        #region Loaded Effects

        /// <summary>
        /// Loads the effect into memory.
        /// </summary>
        /// <param name="effect">The effect to be loaded.</param>
        /// <returns>Returns a bool indicating whether the effect was loaded.</returns>
        public bool LoadEffect<T>(T effect) where T : UIEffectBase
        {
            var result = false;

            if (CheckForLoadedEffect(effect.Name) == false &&
                CheckForLoadedEffect(effect.Id) == false)
            {
                LoadedEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks for a loaded effect by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to search. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the effect is present.</returns>
        public bool CheckForLoadedEffect(int effectId)
        {
            return UIBase.CheckItemById(LoadedEffects, effectId);
        }

        /// <summary>
        /// Checks for a loaded effect by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to search. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect is present.</returns>
        public bool CheckForLoadedEffect(string effectName)
        {
            return UIBase.CheckItemByName(LoadedEffects, effectName);
        }

        /// <summary>
        /// Call to activate a loaded effect by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to activate. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ActivateLoadedEffect(int effectId)
        {
            var result = false;

            if (CheckForLoadedEffect(effectId))
            {
                ActiveEffects.Add(GetLoadedEffect(effectId));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Call to activate a loaded effect by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to activate. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ActivateLoadedEffect(string effectName)
        {
            var result = false;

            if (CheckForLoadedEffect(effectName))
            {
                ActiveEffects.Add(GetLoadedEffect(effectName));
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets the loaded effect by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to retrieve. Intaken as an int.</param>
        /// <returns>Returns the requested effect, if present, otherwise null.</returns>
        public UIEffectBase GetLoadedEffect(int effectId)
        {
            return CheckForLoadedEffect(effectId) ? UIBase.GetItemById(LoadedEffects, effectId) : default(UIEffectBase);
        }

        /// <summary>
        /// Gets the loaded effect by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to retrieve. Intaken as a string.</param>
        /// <returns>Returns the requested effect, if present, otherwise null.</returns>
        public UIEffectBase GetLoadedEffect(string effectName)
        {
            return CheckForLoadedEffect(effectName) ? UIBase.GetItemByName(LoadedEffects, effectName) : default(UIEffectBase);
        }

        /// <summary>
        /// Removes the loaded effect by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to remove. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveLoadedEffect(int effectId)
        {
            return UIBase.RemoveItemById(LoadedEffects, effectId);
        }

        /// <summary>
        /// Removes the loaded effect by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to remove. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveLoadedEffect(string effectName)
        {
            return UIBase.RemoveItemByName(LoadedEffects, effectName);
        }

        #endregion

        #region Activated Effects

        /// <summary>
        /// Activate an effect.
        /// </summary>
        /// <param name="effect">The effect to activate.</param>
        /// <returns>Returns a boolean indicating whether the effect was activated.</returns>
        /// <remarks>The effect is a single use effect as it is not stored in memory.</remarks>
        public bool ActivateEffect<T>(T effect) where T : UIEffectBase
        {
            var result = false;

            if (CheckForActivatedEffect(effect.Name) == false &&
                CheckForActivatedEffect(effect.Id) == false)
            {
                ActiveEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks for an active effect by id.
        /// </summary>
        /// <param name="effectId">The id of the requested effect. Intaken as an int.</param>
        /// <returns>Returns a bool indicating whether the effect is currently active.</returns>
        public bool CheckForActivatedEffect(int effectId)
        {
            return UIBase.CheckItemById(ActiveEffects, effectId);
        }

        /// <summary>
        /// Checks for an active effect by name.
        /// </summary>
        /// <param name="effectName">The name of the requested effect. Intaken as a string.</param>
        /// <returns>Returns a bool indicating whether the effect is currently active.</returns>
        public bool CheckForActivatedEffect(string effectName)
        {
            return UIBase.CheckItemByName(ActiveEffects, effectName);
        }

        /// <summary>
        /// Gets the active effect by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to retrieve. Intaken as an int.</param>
        /// <returns>Returns the specified effect, if present, otherwise null.</returns>
        public UIEffectBase GetActivatedEffect(int effectId)
        {
            return CheckForActivatedEffect(effectId) ? UIBase.GetItemById(ActiveEffects, effectId) : default(UIEffectBase);
        }

        /// <summary>
        /// Gets the active effect by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to retrieve. Intaken as a string</param>
        /// <returns>Returns the specified effect, if present, otherwise null.</returns>
        public UIEffectBase GetActivatedEffect(string effectName)
        {
            return CheckForActivatedEffect(effectName) ? UIBase.GetItemByName(ActiveEffects, effectName) : default(UIEffectBase);
        }

        #endregion

        /// <summary>
        /// Runs all active effects.
        /// </summary>
        /// <returns>Returns a bool indicating whether all of the loaded transitions completed.</returns>
        /// <remarks>
        /// Updates "AreEffectRunning" boolean during cycle.
        /// <para>
        /// Flag "AreEffectsSortedByOrderNumber" as true to sort active effects by their assigned order number.
        /// Flag "AreEffectsRunInSequentialOrder" as true to run active effects in the order they were activated otherwise set to false to run all activated effects at once.
        /// </para>
        /// </remarks>
        /// <see cref="ActivateEffect{T}"/>
        /// <see cref="ActivateLoadedEffect(int)"/>
        /// <see cref="ActivateLoadedEffect(string)"/>
        /// <see cref="AreEffectsRunning"/>
        /// <see cref="AreEffectsSortedByOrderNumber"/>
        /// <see cref="AreEffectsRunInSequentialOrder"/>
        public async Task RunActiveEffects()
        {
            if (ActiveEffects.Count > 0)
            {
                AreEffectsRunning = true;

                if (AreEffectsRunInSequentialOrder)
                {
                    var nextEffect = CurrentEffect = AreEffectsSortedByOrderNumber
                        ? ActiveEffects.OrderBy(effect => effect.OrderNumber).ToList()[0]
                        : ActiveEffects[0];

                    if (await nextEffect.Run())
                    {
                        nextEffect.Reset();
                        ActiveEffects.RemoveAt(0);
                    }
                }
                else
                {
                    var effects = AreEffectsSortedByOrderNumber
                        ? ActiveEffects.OrderBy(effect => effect.OrderNumber).ToList()
                        : ActiveEffects.ToList();

                    foreach (var effect in effects)
                    {
                        if (await effect.Run())
                        {
                            effect.Reset();
                            ActiveEffects.Remove(effect);
                        }
                    }
                }
            }
            else
            {
                AreEffectsRunning = false;
                CurrentEffect = null;
            }
        }
    }
}