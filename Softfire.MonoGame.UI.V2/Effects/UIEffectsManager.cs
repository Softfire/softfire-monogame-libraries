using System.Collections.Generic;
using static Softfire.MonoGame.CORE.Identities;

namespace Softfire.MonoGame.UI.Effects
{
    /// <summary>
    /// A UI effects manager. Used to action effects on UI elements.
    /// </summary>
    public class UIEffectsManager
    {
        /// <summary>
        /// Currently loaded effects for the UI element to use by directly activating them or by programming the into a sequence.
        /// </summary>
        /// <remarks>A Dictionary containing all loaded effects. Effects can be loaded using <see cref="LoadEffect{T}"/> and can be activated individually with the <see cref="ActivateLoadedEffect(int)"/> and <see cref="ActivateLoadedEffect(string)"/> methods.
        /// The loaded effects can also by used to program a sequence of effects by using <see cref="ProgramEffect(int)"/> or <see cref="ProgramEffect(string)"/> methods</remarks>
        private List<UIEffectBase> LoadedEffects { get; }

        /// <summary>
        /// Currently active effects that the UI element is running.
        /// </summary>
        /// <remarks>Once an effect is activated it is run in the order it was activated.</remarks>
        private List<UIEffectBase> ActiveEffects { get; }

        /// <summary>
        /// A programmed list of effects that will activate in the order they are loaded when <see cref="ActivateProgrammedEffects()"/> is called.
        /// </summary>
        private List<UIEffectBase> ProgrammedEffects { get; }

        /// <summary>
        /// The currently active effect.
        /// </summary>
        /// <remarks>Can be used to check the currently running effects status.</remarks>
        public UIEffectBase CurrentEffect { get; private set; }

        /// <summary>
        /// Are effects currently running?
        /// </summary>
        public bool AreEffectsRunning { get; private set; }

        /// <summary>
        /// Are effects run in sequential order?
        /// </summary>
        /// <remarks>Setting to <value>false</value> allows all active effects to run simultaneously as opposed to running sequentially.</remarks>
        public bool AreEffectsRunInSequentialOrder { get; set; } = true;

        /// <summary>
        /// The UI elements effects manager.
        /// </summary>
        public UIEffectsManager()
        {
            LoadedEffects = new List<UIEffectBase>();
            ActiveEffects = new List<UIEffectBase>();
            ProgrammedEffects = new List<UIEffectBase>();
        }

        #region Loaded Effects

        /// <summary>
        /// Loads the effect into memory so it can be activated or programmed.
        /// </summary>
        /// <param name="effect">The new effect to be loaded.</param>
        /// <returns>Returns a bool indicating whether the effect was loaded.</returns>
        public bool LoadEffect<T>(T effect) where T : UIEffectBase
        {
            var result = false;

            if (!LoadedEffectExists(effect.Name) &&
                !LoadedEffectExists(effect.Id))
            {
                LoadedEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the loaded effect exists, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to search. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect exists.</returns>
        public bool LoadedEffectExists(int effectId) => ObjectExists<UIEffectBase, UIEffectBase>(LoadedEffects, effectId);

        /// <summary>
        /// Determines whether the loaded effect exists, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to search. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect exists.</returns>
        public bool LoadedEffectExists(string effectName) => ObjectExists<UIEffectBase, UIEffectBase>(LoadedEffects, effectName);
        
        /// <summary>
        /// Call to activate a loaded effect, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to activate. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ActivateLoadedEffect(int effectId)
        {
            var result = false;
            UIEffectBase effect;

            if ((effect = GetLoadedEffect(effectId)) != null)
            {
                ActiveEffects.Add(effect);
                AreEffectsRunning = true;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Call to activate a loaded effect, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to activate. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ActivateLoadedEffect(string effectName)
        {
            var result = false;
            UIEffectBase effect;

            if ((effect = GetLoadedEffect(effectName)) != null)
            {
                ActiveEffects.Add(effect);
                AreEffectsRunning = true;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets the loaded effect, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the requested effect, if present, otherwise null.</returns>
        public UIEffectBase GetLoadedEffect(int effectId) => GetObject<UIEffectBase, UIEffectBase>(LoadedEffects, effectId);

        /// <summary>
        /// Gets the loaded effect, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns the requested effect, if present, otherwise null.</returns>
        public UIEffectBase GetLoadedEffect(string effectName) => GetObject<UIEffectBase, UIEffectBase>(LoadedEffects, effectName);
        
        /// <summary>
        /// Removes the loaded effect, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveLoadedEffect(int effectId) => RemoveObject<UIEffectBase, UIEffectBase>(LoadedEffects, effectId);

        /// <summary>
        /// Removes the loaded effect, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveLoadedEffect(string effectName) => RemoveObject<UIEffectBase, UIEffectBase>(LoadedEffects, effectName);

        /// <summary>
        /// Clears any loaded effects.
        /// </summary>
        public void ClearLoadedEffects()
        {
            LoadedEffects.Clear();
        }

        #endregion

        #region Activated Effects

        /// <summary>
        /// Activates an anonymous effect immediately. The effect is run once and then discarded.
        /// </summary>
        /// <param name="effect">The anonymous effect to activate.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the effect was activated.</returns>
        /// <remarks>The effect is a single use effect as it is not stored in memory.</remarks>
        public bool ActivateEffect<T>(T effect) where T : UIEffectBase
        {
            var result = false;

            if (!ActivatedEffectExists(effect.Name) &&
                !ActivatedEffectExists(effect.Id))
            {
                ActiveEffects.Add(effect);
                AreEffectsRunning = true;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether an activated effect exists, by id.
        /// </summary>
        /// <param name="effectId">The id of the requested effect. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect is currently active.</returns>
        public bool ActivatedEffectExists(int effectId) => ObjectExists<UIEffectBase, UIEffectBase>(ActiveEffects, effectId);

        /// <summary>
        /// Determines whether an activated effect exists, by name.
        /// </summary>
        /// <param name="effectName">The name of the requested effect. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect is currently active.</returns>
        public bool ActivatedEffectExists(string effectName) => ObjectExists<UIEffectBase, UIEffectBase>(ActiveEffects, effectName);
        
        /// <summary>
        /// Gets the active effect, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to retrieve. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns the specified effect, if present, otherwise null.</returns>
        public UIEffectBase GetActivatedEffect(int effectId) => GetObject<UIEffectBase, UIEffectBase>(ActiveEffects, effectId);

        /// <summary>
        /// Gets the active effect, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to retrieve. Intaken as a <see cref="string"/></param>
        /// <returns>Returns the specified effect, if present, otherwise null.</returns>
        public UIEffectBase GetActivatedEffect(string effectName) => GetObject<UIEffectBase, UIEffectBase>(ActiveEffects, effectName);

        /// <summary>
        /// Removes an activated effect, by id.
        /// </summary>
        /// <param name="effectId">The id of the effect to remove. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveActivatedEffect(int effectId) => RemoveObject<UIEffectBase, UIEffectBase>(ActiveEffects, effectId);

        /// <summary>
        /// Removes an activated effect, by name.
        /// </summary>
        /// <param name="effectName">The name of the effect to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was removed.</returns>
        public bool RemoveActivatedEffect(string effectName) => RemoveObject<UIEffectBase, UIEffectBase>(ActiveEffects, effectName);

        /// <summary>
        /// Clears any active effects.
        /// </summary>
        public void ClearActiveEffects()
        {
            ActiveEffects.Clear();
        }

        #endregion

        #region Programmed Effects

        /// <summary>
        /// Adds an effect, by id, from the <see cref="LoadedEffects"/> list into a special list of effects that can be called with <see cref="ActivateProgrammedEffects()"/> in the order they were added.
        /// </summary>
        /// <param name="effectId">The id of the <see cref="LoadedEffects"/> effect to program. Intaken as an <see cref="int"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ProgramEffect(int effectId)
        {
            var result = false;
            UIEffectBase effect;
            
            if ((effect = GetLoadedEffect(effectId)) != null)
            {
                ProgrammedEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Adds an effect, by name, from the <see cref="LoadedEffects"/> list into a special list of effects that can be called with <see cref="ActivateProgrammedEffects()"/> in the order they were added.
        /// </summary>
        /// <param name="effectName">The name of the <see cref="LoadedEffects"/> effect to program. Intaken as an <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the effect was activated.</returns>
        public bool ProgramEffect(string effectName)
        {
            var result = false;
            UIEffectBase effect;
            
            if ((effect = GetLoadedEffect(effectName)) != null)
            {
                ProgrammedEffects.Add(effect);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Clears any programmed effects.
        /// </summary>
        public void ClearProgrammedEffects()
        {
            ProgrammedEffects.Clear();
        }

        /// <summary>
        /// Activates all programmed effects in the order they were added.
        /// </summary>
        public void ActivateProgrammedEffects()
        {
            foreach (var effect in ProgrammedEffects)
            {
                ActiveEffects.Add(effect);
            }

            AreEffectsRunning = true;
        }

        #endregion

        /// <summary>
        /// Runs all active effects that have been activated with <see cref="ActivateEffect{T}"/>, <see cref="ActivateLoadedEffect(int)"/> or <see cref="ActivateLoadedEffect(string)"/> and <see cref="ActivateProgrammedEffects()"/>.
        /// </summary>
        internal void RunActiveEffects()
        {
            if (ActiveEffects.Count > 0)
            {
                if (AreEffectsRunInSequentialOrder)
                {
                    CurrentEffect = ActiveEffects[0];

                    if (ActiveEffects[0].Run())
                    {
                        ActiveEffects[0].Reset();
                        ActiveEffects.RemoveAt(0);
                    }
                }
                else
                {
                    for (var i = 0; i < ActiveEffects.Count; i++)
                    {
                        CurrentEffect = ActiveEffects[i];

                        if (ActiveEffects[i].Run())
                        {
                            ActiveEffects[i].Reset();
                            ActiveEffects.RemoveAt(i);
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